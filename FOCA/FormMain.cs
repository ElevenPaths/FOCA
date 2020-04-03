using FOCA.Analysis;
using FOCA.Analysis.FingerPrinting;
using FOCA.Analysis.Pinger;
using FOCA.Core;
using FOCA.Database.Entities;
using FOCA.GUI;
using FOCA.Plugins;
using FOCA.Search;
using FOCA.Searcher;
using FOCA.SubdomainSearcher;
using FOCA.TaskManager;
using FOCA.Threads;
using FOCA.Utilites;
using MetadataExtractCore.Diagrams;
using MetadataExtractCore.Extractors;
using Microsoft.WindowsAPICodePack.Taskbar;
using NLog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FOCA
{
    public partial class FormMain : Form
    {
        public static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private CancellationTokenSource updateUITokenSource;

        /// <summary>
        /// Thread with all actions.
        /// </summary>
        public Thread ScannThread;

        /// <summary>
        /// Use in shodan search
        /// </summary>
        public ShodanSearcher ShodanRecognitionObject;

        public enum ProgramState { Normal, ExtractingMetadata, Searching };

        /// <summary>
        /// State of program.
        /// </summary>
        public ProgramState programState;

        /// <summary>
        /// Manager of projects
        /// </summary>
        public ProjectManager ProjectManager;

        /// <summary>
        /// Integrate with taskbar of windows 7
        /// </summary>
        TaskbarManager _tm;

        /// <summary>
        /// Controla las llamadas de los plugins a la FOCA
        /// </summary>
#if PLUGINS

        [NonSerialized]
        public ManagePluginCalls ManagePluginsApi;

#endif

        #region Events

        public FormMain()
        {
            InitializeComponent();

            if (TaskbarManager.IsPlatformSupported)
                _tm = TaskbarManager.Instance;

            ProjectManager = new ProjectManager(this);
            this.updateUITokenSource = new CancellationTokenSource();
        }

        private void formMain_Load(object sender, EventArgs e)
        {

            try
            {
                if (Program.DesignMode())
                    return;

#if PLUGINS
                ManagePluginsApi = new ManagePluginCalls();
                ManagePluginsApi.EnablePluginCalls();
#endif

                Program.data.tasker = new Tasker();
                Program.data.tasker.AsociaEventosTareas();

                Program.data.Project.ProjectName = string.Empty;
                Program.data.Project.Domain = string.Empty;


                Thread.CurrentThread.Priority = ThreadPriority.Highest;

                pluginsToolStripMenuItem.Visible = true;

                MinimumSize = new Size(Width - ClientSize.Width + 900, MinimumSize.Height);

                Program.cfgCurrent.LoadConfiguration();

                var lvwColumnSorter = new ListViewColumnSorter();
                panelMetadataSearch.listViewDocuments.Tag = lvwColumnSorter;
                panelMetadataSearch.listViewDocuments.ListViewItemSorter = lvwColumnSorter;
                var lvwColumnSorterValues = new ListViewColumnSorterValues();
                panelInformation.lvwInformation.Tag = lvwColumnSorterValues;

                SetItemsMenu(null, null);

                Task.Factory.StartNew(UpdateBackgroundAsync, this.updateUITokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

                Program.data.Project.ProjectName = Project.DefaultProjectName;
                Text = Program.ProgramName;

                LoadPanelIntroduccion();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }


        public void tasker_OnTaskStarting(object sender, EventArgs e)
        {
            if (sender == null)
                return;


            panelTasks.Invoke(new MethodInvoker(delegate
            {
                try
                {
                    var taskfoca = (TaskFOCA)sender;
                    panelTasks.StartTask(taskfoca);
                }
                catch
                {
                }
            }));
        }


        public void tasker_OnTaskFinished(object sender, EventArgs e)
        {
            if (sender == null)
                return;

            var taskfoca = (TaskFOCA)sender;

            panelTasks.Invoke(new MethodInvoker(delegate
            {
                try
                {
                    panelTasks.EndTask(taskfoca);
                }
                catch
                {
                }
            }));
        }


        public void tasker_OnTaskAdded(object sender, EventArgs e)
        {
            if (sender == null)
                return;

            var taskfoca = (TaskFOCA)sender;

            this.Invoke(new MethodInvoker(delegate
            {
                try
                {
                    panelTasks.AddNewTask(taskfoca);
                }
                catch
                {

                }
            }));
        }

        private void formMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Do you really want to exit?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                e.Cancel = true;
                return;
            }
            this.updateUITokenSource.Cancel();
            Text = Program.ProgramName + " - Closing...";
        }

        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        /// <summary>
        /// Stop Threads and delete data.
        /// </summary>
        public void Reset()
        {
            AbortThreads();
            panelTasks.ResetGUI();
            Program.data.Clear();
            panelMetadataSearch.listViewDocuments.Items.Clear();

            toolStripStatusLabelLeft.Text = string.Empty;
            toolStripProgressBarDownload.Value = 0;
        }

        /// <summary>
        /// Drag and drop files.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            var ficheros = (string[])e.Data.GetData(DataFormats.FileDrop);

            var t = new Thread(DropAllFiles) { IsBackground = true };
            t.Start(ficheros);
        }

        /// <summary>
        /// Add all files to project
        /// </summary>
        /// <param name="objectValue"></param>
        private void DropAllFiles(object objectValue)
        {
            var ficheros = (string[])objectValue;
            foreach (var fichero in ficheros)
            {
                if (Directory.Exists(fichero))
                {
                    var files = Directory.GetFiles(fichero);
                    var lvcs = (ListViewColumnSorter)panelMetadataSearch.listViewDocuments.ListViewItemSorter;
                    panelMetadataSearch.listViewDocuments.ListViewItemSorter = null;
                    foreach (var file in files)
                    {
                        Invoke(new MethodInvoker(delegate
                        {
                            try
                            {
                                panelMetadataSearch.AddFile(file);
                            }
                            catch
                            {
                                Program.LogThis(new Log(Log.ModuleType.FOCA, "Couldn't add file, maybe your AV detected it as malicious", Log.LogType.error));
                            }
                        }));
                    }
                    Invoke(new MethodInvoker(delegate
                    {
                        panelMetadataSearch.listViewDocuments.ListViewItemSorter = lvcs;
                    }));
                }
                else
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        try
                        {
                            panelMetadataSearch.AddFile(fichero);
                        }
                        catch
                        {
                            Program.LogThis(new Log(Log.ModuleType.FOCA, "Couldn't add file, maybe the file is being used or you don't have enough permissions", Log.LogType.error));
                        }
                    }));
                }
            }
        }

        #endregion

        /// <summary>
        /// Set menu items.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetItemsMenu(object sender, EventArgs e)
        {
            toolStripMenuItemSaveProject.Enabled = Program.data.Project.ProjectState != Project.ProjectStates.Uninitialized;
        }

        #region Menu Projects Events

        /// <summary>
        /// Load project.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LoadProject(object sender, EventArgs e)
        {
            var toolStripMenuItem = sender as ToolStripMenuItem;

            if (toolStripMenuItem != null) ProjectManager.LoadProject(toolStripMenuItem.Name);

            LoadSearchGui();
            panelProject.CheckDownloadDirectory();
        }

        private void toolStripMenuItemNewProject_Click(object sender, EventArgs e)
        {
            LoadProjectGui(true);
        }

        private void toolStripMenuItemSaveProject_Click(object sender, EventArgs e)
        {
            ProjectManager.SaveProject(Program.data.Project.ProjectSaveFile);
        }

        private void toolStripMenuItemClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        #region DNS Enumeration events

        public void ShodanDataFound(object sender, EventsThreads.CollectionFound<ShodanIPInformation> e)
        {
            try
            {
                if (e?.Data == null || e.Data.Count <= 0) return;

                ShodanIPInformation si = e.Data.First();

                if (String.IsNullOrWhiteSpace(si.OS) && !String.IsNullOrWhiteSpace(si.ServerBanner))
                {
                    si.OS = HTTP.GetOsFromBanner(si.ServerBanner).ToString();
                }

                Program.LogThis(new Log(Log.ModuleType.ShodanSearch, string.Format("Found IP Information {0}", si.IPAddress), Log.LogType.medium));

                panelDNSSearch.SearchIpBingSingleIp(si.IPAddress);

                var ei = new ExtendedIPInformation
                {
                    Country = si.Country,
                    ServerBanner = si.ServerBanner
                };

                if (si.HostNames.Count > 0)
                {
                    foreach (var hostName in si.HostNames)
                        Program.data.AddResolution(hostName, si.IPAddress, $"Shodan Hostname [{hostName}]", 0, Program.cfgCurrent, true);
                }
                else
                {
                    var hayRelacciones = Program.data.relations.Items.Count(r => r.Ip != null && r.Ip.Ip == si.IPAddress) > 0;

                    if (hayRelacciones == false)
                    {
                        Program.data.AddIP(si.IPAddress, "Shodan", Program.cfgCurrent.MaxRecursion);
                        var ipItem = Program.data.GetIp(si.IPAddress);
                        var computerItem = new ComputersItem
                        {
                            type = ComputersItem.Tipo.Server,
                            name = si.IPAddress,
                            os = OperatingSystem.OS.Unknown
                        };

                        Program.data.computers.Items.Add(computerItem);
                        Program.data.computerIPs.Items.Add(new ComputerIPsItem(computerItem, ipItem, "Manually added IP"));
                    }
                }

                ei.OS = si.OS;

                Program.data.SetIPInformation(si.IPAddress, ei);
                Program.data.GetServersFromIPs();
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                Program.LogThis(new Log(Log.ModuleType.Shodan, string.Format("Error managing Shodan data returned {0}", ex.Message), Log.LogType.error));
            }
        }

        /// <summary>
        /// Log Shodan execution.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ShodanLog(object sender, EventsThreads.ThreadStringEventArgs e)
        {
            Program.LogThis(new Log(Log.ModuleType.Shodan, e.Message, Log.LogType.low));
        }

        #endregion

        #region Options menu events

        private void toolStripMenuItemOptions_Click(object sender, EventArgs e)
        {
            Program.FormOptionsInstance.ShowDialog();
        }

        #endregion

        #region About menu events
        private void toolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            LoadAboutGui();
        }
        #endregion

        #region Form Components events

        /// <summary>
        /// Abort button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripDropDownButtonAbort_Click(object sender, EventArgs e)
        {
            AbortThreads();
        }

        /// <summary>
        /// Stop all threads
        /// </summary>
        public void AbortThreads()
        {
            panelMetadataSearch.Abort();
            panelDNSSearch.Abort();
            ScannThread?.Abort();
        }

        /// <summary>
        /// Update DownloadStatus
        /// </summary>
        /// <param name="newStatus">New DownloadStatus</param>
        public void ChangeStatus(string newStatus)
        {
            statusStripMain.Invoke(new MethodInvoker(delegate
            {
                toolStripStatusLabelLeft.Text = newStatus;
            }));
        }

        #endregion

        #region Show diferent panels

        /// <summary>
        /// Load new project panel
        /// </summary>
        /// <param name="newProject">Is new or not</param>
        public void LoadProjectGui(bool newProject)
        {
            panelProject.Visible = true;
            panelProject.BringToFront();

            if (newProject)
            {
                Program.FormMainInstance.Reset();
                Program.data.Project = new Project(); //Initialize object
                panelProject.ClearFields();
            }
            else
                panelProject.Fill(Program.data.Project);

            panelProject.LoadProject();

        }

        /// <summary>
        /// Load interfaz project
        /// </summary>
        public void LoadInitialProjectGui()
        {
            SetItemsMenu(null, null);

            Text = Program.data.Project.ProjectName + " - " + Program.ProgramName;

            panelProject.Visible = false;

            var sb = new StringBuilder("site:" + Program.data.Project.Domain);
            sb.Append(" (");

            foreach (int i in panelMetadataSearch.checkedListBoxExtensions.CheckedIndices)
            {
                sb.Append("filetype:");
                sb.Append(((string)panelMetadataSearch.checkedListBoxExtensions.Items[i]).Replace("*", string.Empty));
                sb.Append(" OR ");
            }

            sb.Remove(sb.Length - 4, 4);
            sb.Append(")");
            LoadSearchGui(sb.ToString(), true);
        }

        /// <summary>
        /// Load information gui.
        /// </summary>
        public void LoadInformationGui()
        {
            panelInformation.Visible = true;
            panelInformation.BringToFront();
        }

        public void LoadPanelIntroduccion()
        {
            panelMetadataSearch.Visible =
            splitContainerMain.Visible = true;
            splitContainerMain.BringToFront();
            PanelIntroduccion.Visible = true;
            PanelIntroduccion.BringToFront();
        }

        #region Search GUI Panel

        public void LoadSearchGui()
        {
            LoadSearchGui(panelMetadataSearch.txtSearch.Text, false);
        }

        /// <summary>
        /// Load search Gui.
        /// </summary>
        /// <param name="searchString">searchstring</param>
        /// <param name="isInitialSearch">Initial search</param>
        public void LoadSearchGui(string searchString, bool isInitialSearch)
        {
            splitContainerMain.BringToFront();

            bool isProjectInitialized = Program.data.Project.ProjectState != Project.ProjectStates.Uninitialized;

            HideOrShowProjectInitializedComponents(isProjectInitialized);

            if (!isProjectInitialized)
            {
                panelMetadataSearch.panelCustomSearch.BorderStyle = BorderStyle.FixedSingle;
                panelMetadataSearch.linkLabelCustomSearch.Visible = false;
            }

            if (isInitialSearch)
            {
                panelMetadataSearch.linkLabelCustomSearch.Visible = !string.IsNullOrWhiteSpace(searchString);
                panelMetadataSearch.txtSearch.Visible = !panelMetadataSearch.linkLabelCustomSearch.Visible;
                panelMetadataSearch.btnSearch.Visible = !panelMetadataSearch.linkLabelCustomSearch.Visible;
                panelMetadataSearch.panelCustomSearch.BorderStyle = BorderStyle.None;
            }

            panelMetadataSearch.txtSearch.Text = searchString;
            panelMetadataSearch.BringToFront();
        }

        private void HideOrShowProjectInitializedComponents(bool isProjectInitialized)
        {
            panelMetadataSearch.btnSearchAll.Enabled = isProjectInitialized;
            panelMetadataSearch.checkedListBoxExtensions.Visible = isProjectInitialized;
            panelMetadataSearch.lblExtensions.Visible = isProjectInitialized;
            panelMetadataSearch.lblAll.Visible = isProjectInitialized;
            panelMetadataSearch.lblNone.Visible = isProjectInitialized;
        }

        #endregion

        /// <summary>
        /// Show DNS panel
        /// </summary>
        public void LoadDnsEnumerationGui()
        {
            splitContainerMain.Visible = true;
            splitContainerMain.BringToFront();

            panelInformation.Visible = false;
            panelDNSSearch.Visible = true;
            panelDNSSearch.BringToFront();

            bool isProjectInitialized = Program.data.Project.ProjectState != Project.ProjectStates.Uninitialized;

            panelDNSSearch.Enabled = isProjectInitialized;
        }

        /// <summary>
        /// Show panel tasks
        /// </summary>
        public void LoadTasksGui()
        {
            splitContainerMain.Visible = true;
            splitContainerMain.BringToFront();
            panelInformation.Visible = false;
            panelTasks.Visible = true;
            panelTasks.Dock = DockStyle.Fill;
            panelTasks.BringToFront();
        }

        /// <summary>
        /// Show panel for DNS Snooping Gui.
        /// </summary>
        public void LoadDnsSnoopingGui()
        {
            panelDnsSnooping.LoadProjectConfig();
            panelDnsSnooping.Show();
            panelDnsSnooping.BringToFront();
        }

        /// <summary>
        /// Show panel about
        /// </summary>
        public void LoadAboutGui()
        {
            var fa = new FormAbout();
            fa.ShowDialog();
        }

        #endregion

        #region  TreeViewMetadata Events

        /// <summary>
        /// Search document in all nodes from treeviewdata
        /// </summary>
        /// <param name="path"></param>
        /// <returns>TreeNode</returns>
        public TreeNode TreeViewMetadataSearchDocument(string path)
        {
            return TreeViewMetadataReturnAllDocuments().FirstOrDefault(tn => tn.Name == path);
        }

        /// <summary>
        /// Add doc to metadata treeview
        /// </summary>
        /// <param name="name">name</param>
        /// <returns>TreeNode</returns>
        public TreeNode TreeViewMetadataAddDocument(FilesItem file)
        {
            TreeNode tnSearched = TreeViewMetadataSearchDocument(file.Path);

            if (tnSearched != null) return tnSearched;

            string extension = file.Ext;

            if (extension.Length == 0 || !DocumentExtractor.IsSupportedExtension(extension))
                extension = "Unknown";

            TreeNode documentsNode = TreeView.GetNode(GUI.Navigation.Project.DocumentAnalysis.Files.ToNavigationPath());
            TreeNode extNode = documentsNode.Nodes[extension];
            if (extNode == null)
            {
                extNode = documentsNode.Nodes.Insert(SearchTextInNodes(documentsNode.Nodes, extension), extension, extension);
                extNode.ImageIndex = extNode.SelectedImageIndex = GetImageToExtension(extension);
                documentsNode.Expand();
            }

            TreeNode child = extNode.Nodes.Insert(SearchTextInNodes(extNode.Nodes, System.IO.Path.GetFileName(file.Path)), file.Path, System.IO.Path.GetFileName(file.Path));

            child.ContextMenuStrip = Program.FormMainInstance.contextMenuStripDocuments;

            treeViewMetadata_UpdateDocumentsNumber();
            return child;
        }

        /// <summary>
        /// Return image value by extension
        /// </summary>
        /// <param name="extension">extension</param>
        /// <returns>int</returns>
        public int GetImageToExtension(string extension)
        {
            switch (extension.ToLower())
            {
                case ".sxw":
                case ".odt":
                case ".ods":
                case ".odg":
                case ".odp":
                    return 0;
                case ".docx":
                case ".doc":
                    return 7;
                case ".ppt":
                case ".pps":
                case ".pptx":
                case ".ppsx":
                    return 9;
                case ".xls":
                case ".xlsx":
                    return 10;
                case ".pdf":
                    return 8;
                case ".wpd":
                    return 29;
                case ".raw":
                case ".cr2":
                case ".crw":
                case ".jpg":
                case ".jpeg":
                case ".png":
                    return 32;
                case ".svg":
                case ".svgz":
                    return 51;
                case ".indd":
                    return 61;
                default:
                    return 22;
            }
        }

        /// <summary>
        /// Return all metadata documents
        /// </summary>
        /// <returns>IEnumerable<TreeNode></returns>
        public IEnumerable<TreeNode> TreeViewMetadataReturnAllDocuments()
        {
            try
            {
                return TreeView.GetNode(GUI.Navigation.Project.DocumentAnalysis.Files.ToNavigationPath()).Nodes.Cast<TreeNode>().SelectMany(p => p.Nodes.Cast<TreeNode>());
            }
            catch
            {
                return new List<TreeNode>();
            }
        }

        /// <summary>
        /// Update tree Documents number.
        /// </summary>
        public void treeViewMetadata_UpdateDocumentsNumber()
        {
            Program.FormMainInstance.TreeView.BeginUpdate();
            TreeNode documentsNode = Program.FormMainInstance.TreeView.GetNode(GUI.Navigation.Project.DocumentAnalysis.Files.ToNavigationPath());
            documentsNode.Text = String.Format("Files ({0}/{1})", Program.data.files.Items.Count(F => F.Downloaded), Program.data.files.Items.Count);

            foreach (TreeNode tn in documentsNode.Nodes)
            {
                string ext;
                ext = tn.Text.IndexOf(" (") > 0 ? tn.Text.Substring(0, tn.Text.IndexOf(" (")) : tn.Text;

                tn.Text = string.Format("{0} ({1})", ext, tn.Nodes.Count);
            }

            Program.FormMainInstance.TreeView.EndUpdate();
        }

        /// <summary>
        /// Remove documents tools s.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tn = Program.FormMainInstance.TreeView.SelectedNode;
            var fi = (FilesItem)tn.Tag;
            fi.Metadata = null;
            fi.MetadataExtracted = false;
            panelMetadataSearch.listViewDocuments_Update(fi);
            tn.Remove();
            treeViewMetadata_UpdateDocumentsNumber();
        }

        /// <summary>
        /// Open document menu item.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var tn = Program.FormMainInstance.TreeView.SelectedNode;

            try
            {
                Process.Start(((FilesItem)tn.Tag).Path);
            }
            catch
            {
                MessageBox.Show("File not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// Función para determinar si un nodo representa a un documento en el arbol de nodos
        /// </summary>
        /// <param name="nodeItem">tree node</param>
        /// <returns>bool</returns>
        private bool IsDocumentNode(TreeNode nodeItem)
        {
            TreeNode documentsNode = TreeView.GetNode(GUI.Navigation.Project.DocumentAnalysis.Files.ToNavigationPath());
            return nodeItem?.Parent?.Parent == documentsNode;
        }

        /// <summary>
        /// Add new item to listView panel information
        /// </summary>
        /// <param name="itemValue"></param>
        /// <param name="subItemValue"></param>
        /// <param name="groupValue"></param>
        private ListViewItem NewItemListView(string itemValue, string subItemValue, string groupValue)
        {
            ListViewItem lvi = panelInformation.lvwInformation.Items.Add(itemValue);
            lvi.SubItems.Add(subItemValue);
            lvi.Group = panelInformation.lvwInformation.Groups[groupValue];
            return lvi;
        }

        /// <summary>
        /// Initialize Panel Information.
        /// </summary>
        private void InitializePanelInformation()
        {
            LoadInformationGui();
            panelInformation.lvwInformation.Items.Clear();
            panelInformation.lvwInformation.Groups.Clear();

            panelInformation.lvwInformation.ListViewItemSorter = null;
            panelInformation.lvwInformation.Sort();
        }

        /// <summary>
        /// Event called when click in one node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TreeViewProjectAfterSelect(object sender, TreeViewEventArgs e)
        {
            panelInformation.splitPanel.Panel2Collapsed = true;

            Color metadataColor = TreeView.GetNode(GUI.Navigation.Project.DocumentAnalysis.ToNavigationPath()).ForeColor;
            foreach (TreeNode tn in TreeViewMetadataReturnAllDocuments())
                tn.ForeColor = metadataColor;

            TreeNode metadataSummaryNode = TreeView.GetNode(GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.ToNavigationPath());
            if (e.Node == TreeView.GetNode(GUI.Navigation.Project.DocumentAnalysis.Files.ToNavigationPath()))
            {
                LoadSearchGui();
            }
            else if (IsDocumentNode(e.Node))
            {
                InitializePanelInformation();

                panelInformation.lvwInformation.Groups.Add("File Information", "File Information");

                FilesItem currentFile = e.Node.Tag as FilesItem;
                if (currentFile == null)
                    currentFile = e.Node.Parent?.Parent?.Tag as FilesItem;

                if (e.Node.Tag != null)
                {
                    var lvi = panelInformation.lvwInformation.Items.Add("URL");

                    lvi.SubItems.Add(currentFile.URL);
                    lvi.Group = panelInformation.lvwInformation.Groups["File Information"];
                    lvi = panelInformation.lvwInformation.Items.Add("Local path");
                    lvi.SubItems.Add(currentFile.Path);
                    lvi.Group = panelInformation.lvwInformation.Groups["File Information"];
                    lvi = panelInformation.lvwInformation.Items.Add("Download");
                    lvi.SubItems.Add(currentFile.Downloaded ? "Yes" : "No");
                    lvi.Group = panelInformation.lvwInformation.Groups["File Information"];
                    lvi = panelInformation.lvwInformation.Items.Add("Analyzed");
                    lvi.SubItems.Add(currentFile.MetadataExtracted ? "Yes" : "No");
                    lvi.Group = panelInformation.lvwInformation.Groups["File Information"];
                    lvi = panelInformation.lvwInformation.Items.Add("Download date");
                    lvi.SubItems.Add(currentFile.Date.ToString());
                    lvi.Group = panelInformation.lvwInformation.Groups["File Information"];
                    lvi = panelInformation.lvwInformation.Items.Add("Size");
                    lvi.SubItems.Add(Functions.GetFileSizeAsString(currentFile.Size));
                    lvi.Group = panelInformation.lvwInformation.Groups["File Information"];

                    panelInformation.lvwInformation.Groups.Add("MalwareAnalysis", "Malware Analysis (Powered by DIARIO)");
                    if (currentFile.DiarioAnalyzed)
                    {
                        NewItemListView("Prediction", currentFile.DiarioPrediction, "MalwareAnalysis");
                    }
                    else if (DiarioAnalyzer.IsSupportedExtension(currentFile.Ext))
                    {
                        ListViewItem n = NewItemListView("Malware analysis pending", String.Empty, "MalwareAnalysis");
                        n.BackColor = Color.Orange;
                    }
                }
                else
                {
                    var lvi = panelInformation.lvwInformation.Items.Add("Local path");
                    lvi.SubItems.Add(e.Node.Text);
                    lvi.Group = panelInformation.lvwInformation.Groups["File Information"];
                }

                if (currentFile.MetadataExtracted)
                {
                    if (e.Node.Nodes["Users"] != null)
                    {
                        panelInformation.lvwInformation.Groups.Add("Users", "Users");

                        var u = (Users)e.Node.Nodes["Users"].Tag;
                        foreach (var ui in u.Items.Where(ui => !string.IsNullOrEmpty(ui.Name)))
                            NewItemListView("UserName", ui.Name, "Users");
                    }
                    if (e.Node.Nodes["Passwords"] != null)
                    {
                        panelInformation.lvwInformation.Groups.Add("Passwords", "Passwords");

                        var p = (Passwords)e.Node.Nodes["Passwords"].Tag;
                        foreach (var pi in p.Items.Where(pi => !string.IsNullOrEmpty(pi.Password)))
                            NewItemListView("Passwords", pi.Password, "Passwords");
                    }
                    if (e.Node.Nodes["Servers"] != null)
                    {
                        panelInformation.lvwInformation.Groups.Add("Servers", "Servers");

                        var serverItem = (Servers)e.Node.Nodes["Servers"].Tag;
                        foreach (var si in serverItem.Items.Where(si => !string.IsNullOrEmpty(si.Name)))
                            NewItemListView("Servers", si.Name, "Servers");

                    }
                    if (e.Node.Nodes["Folders"] != null)
                    {
                        panelInformation.lvwInformation.Groups.Add("Folders", "Folders");

                        var rutaItem = (Paths)e.Node.Nodes["Folders"].Tag;
                        foreach (var ri in rutaItem.Items.Where(ri => !string.IsNullOrEmpty(ri.Path)))
                            NewItemListView("Folder", ri.Path, "Folders");
                    }
                    if (e.Node.Nodes["Printers"] != null)
                    {
                        panelInformation.lvwInformation.Groups.Add("Printers", "Printers");

                        var printerItem = (Printers)e.Node.Nodes["Printers"].Tag;
                        foreach (var ii in printerItem.Items.Where(ii => !string.IsNullOrEmpty(ii.Printer)))
                            NewItemListView("Printer", ii.Printer, "Printers");
                    }
                    if (e.Node.Nodes["Emails"] != null)
                    {
                        panelInformation.lvwInformation.Groups.Add("Emails", "Emails");

                        var emailItem = (Emails)e.Node.Nodes["Emails"].Tag;
                        foreach (var ei in emailItem.Items.Where(ei => !string.IsNullOrEmpty(ei.Mail)))
                            NewItemListView("Email", ei.Mail, "Emails");
                    }
                    if (e.Node.Nodes["Dates"] != null)
                    {
                        panelInformation.lvwInformation.Groups.Add("Dates", "Dates");

                        var f = (Database.Entities.Dates)e.Node.Nodes["Dates"].Tag;
                        if (f.CreationDateSpecified)
                            NewItemListView("Creation date", f.CreationDate.ToString(), "Dates");

                        if (f.DatePrintingSpecified)
                            NewItemListView("Printed date", f.DatePrinting.ToString(), "Dates");

                        if (f.ModificationDateSpecified)
                            NewItemListView("Modified date", f.ModificationDate.ToString(), "Dates");

                    }
                    if (e.Node.Nodes["GPS"] != null && e.Node.Nodes["GPS"].Tag is FileMetadata fmd && fmd != null)
                    {
                        panelInformation.lvwInformation.Groups.Add("GPS location", "GPS location");

                        NewItemListView("Location", fmd.GPS.Value, "GPS");
                    }

                    SetOtherMetaParentNode(e);

                    SetOldVersionNodes(e);

                    if (e.Node.Nodes["History"] != null)
                    {
                        panelInformation.lvwInformation.Groups.Add("History", "History");

                        var h = (Database.Entities.History)e.Node.Nodes["History"].Tag;
                        foreach (var hi in h.Items)
                        {
                            if (String.IsNullOrWhiteSpace(hi.Author))
                                NewItemListView("Author", hi.Author, "History");

                            if (String.IsNullOrWhiteSpace(hi.Comments))
                                NewItemListView("Comments", hi.Comments, "History");

                            if (String.IsNullOrWhiteSpace(hi.Path))
                                NewItemListView("Path", hi.Path, "History");
                        }
                    }

                    if (e.Node.Nodes["Software"] != null)
                    {
                        panelInformation.lvwInformation.Groups.Add("Software", "Software");

                        var aplicaciones = (Applications)e.Node.Nodes["Software"].Tag;
                        foreach (var lvi in from ai in aplicaciones.Items where ai.Name != string.Empty select panelInformation.lvwInformation.Items.Add(ai.Name))
                            lvi.Group = panelInformation.lvwInformation.Groups["Software"];

                    }
                    if (e.Node.Nodes["EXIF in pictures"] != null)
                    {
                        panelInformation.lvwInformation.Groups.Add("EXIF in pictures", "EXIF in pictures");
                        foreach (TreeNode tn in e.Node.Nodes["EXIF in pictures"].Nodes)
                            NewItemListView("File", tn.Text, "EXIF in pictures");

                    }
                }
                else
                {
                    panelInformation.lvwInformation.Groups.Add("MetadataExtraction", "Metadata Extraction");
                    ListViewItem n = NewItemListView("Metadata extraction pending", String.Empty, "MetadataExtraction");
                    n.BackColor = Color.Orange;
                }
            }
            else if (IsDocumentNode(e.Node.Parent))
            {
                FilesItem currentFile = e.Node?.Parent?.Tag as FilesItem;
                if (e.Node.Text == "Users")
                {
                    InitializePanelInformation();

                    panelInformation.lvwInformation.Groups.Add("Users", "Users");
                    var userItem = (Users)e.Node.Tag;
                    foreach (var ui in userItem.Items)
                        NewItemListView("Username", ui.Name, "Users");

                }
                else if (e.Node.Text == "Servers")
                {
                    InitializePanelInformation();

                    panelInformation.lvwInformation.Groups.Add("Servers", "Servers");
                    var serverItem = (Servers)e.Node.Tag;
                    foreach (var si in serverItem.Items)
                        NewItemListView("Server", si.Name, "Servers");

                }
                else if (e.Node.Text == "Folders")
                {
                    InitializePanelInformation();

                    panelInformation.lvwInformation.Groups.Add("Folders", "Folders");
                    var r = (Paths)e.Node.Tag;
                    foreach (var ri in r.Items)
                        NewItemListView("Folder", ri.Path, "Folders");

                }
                else if (e.Node.Text == "Passwords")
                {
                    InitializePanelInformation();

                    panelInformation.lvwInformation.Groups.Add("Passwords", "Passwords");

                    var p = (Passwords)e.Node.Tag;
                    foreach (var pi in p.Items)
                        NewItemListView("Passwords", pi.Password, "Passwords");

                }
                else if (e.Node.Text == "Printers")
                {
                    InitializePanelInformation();

                    panelInformation.lvwInformation.Groups.Add("Printers", "Printers");
                    var printerItem = (Printers)e.Node.Tag;
                    foreach (var ii in printerItem.Items)
                        NewItemListView("Printer", ii.Printer, "Printers");

                }
                else if (e.Node.Text == "Emails")
                {
                    InitializePanelInformation();

                    panelInformation.lvwInformation.Groups.Add("Emails", "Emails");
                    var emailItem = (Emails)e.Node.Tag;
                    foreach (var ei in emailItem.Items)
                        NewItemListView("Email", ei.Mail, "Emails");

                }
                else if (e.Node.Text == "Dates")
                {
                    InitializePanelInformation();

                    panelInformation.lvwInformation.Groups.Add("Dates", "Dates");
                    var f = (Database.Entities.Dates)e.Node.Tag;

                    if (f.CreationDateSpecified)
                    {
                        NewItemListView("Creation date", f.CreationDate.ToString(), "Dates");
                    }
                    if (f.DatePrintingSpecified)
                        NewItemListView("Printed date", f.DatePrinting.ToString(), "Dates");

                    if (f.ModificationDateSpecified)
                        NewItemListView("Modified date", f.ModificationDate.ToString(), "Dates");
                }
                else if (e.Node.Text == "Other Metadata")
                {
                    InitializePanelInformation();
                    SetOtherMetadataNode(e);
                }
                else if (e.Node.Text == "Old versions")
                {
                    InitializePanelInformation();
                    SetOldVersionNode(e);
                }
                else if (e.Node.Text == "History")
                {
                    InitializePanelInformation();
                    SetHistoryNode(e);
                }
                else if (e.Node.Text == "Software")
                {
                    InitializePanelInformation();

                    panelInformation.lvwInformation.Groups.Add("Software", "Software");
                    var aplicaciones = (Applications)e.Node.Tag;
                    foreach (var lvi in aplicaciones.Items.Select(ai => panelInformation.lvwInformation.Items.Add(ai.Name)))
                        lvi.Group = panelInformation.lvwInformation.Groups["Software"];

                }
                else if (e.Node.Text == "EXIF in pictures")
                {
                    InitializePanelInformation();

                    panelInformation.lvwInformation.Groups.Add("EXIF in pictures", "EXIF in pictures");
                    if (e.Node.Nodes == null) return;
                    foreach (TreeNode tn in e.Node.Nodes)
                        NewItemListView("File", tn.Text, "EXIF in pictures");
                }
                else if (e.Node.Text == "GPS")
                {
                    InitializePanelInformation();

                    FileMetadata ed = e.Node.Tag as FileMetadata;
                    if (ed != null)
                    {
                        panelInformation.lvwInformation.Groups.Add("GPS location", "GPS location");
                        NewItemListView("DMS", ed.GPS.Value, "GPS location");
                        string longitude = ed.GPS.Longitude.ToString("0.000000", CultureInfo.InvariantCulture);
                        string latitude = ed.GPS.Latitude.ToString("0.000000", CultureInfo.InvariantCulture);
                        NewItemListView("Longitude", longitude, "GPS location");
                        NewItemListView("Latitude", latitude, "GPS location");
                        NewItemListView("Altitude", ed.GPS.Altitude, "GPS location");
                        NewItemListView("Google maps url", $"https://www.google.com/maps/search/?api=1&query={latitude},{longitude}", "GPS location");
                    }
                }
                else if (e.Node.Text == "Malware Analysis")
                {
                    InitializePanelInformation();

                    if (currentFile != null)
                    {
                        panelInformation.lvwInformation.Groups.Add("MalwareAnalysis", "Malware Analysis (Powered by DIARIO)");
                        NewItemListView("Prediction", currentFile.DiarioPrediction, "MalwareAnalysis");
                    }
                }
                else if (e.Node.Text == "EXIF")
                {
                    ShowEXIFPanelInformation(e.Node.Tag as FileMetadata);
                }
            }
            else if (e.Node.Text == "EXIF" && e.Node.Parent?.Parent?.Text == "EXIF in pictures")
            {
                ShowEXIFPanelInformation(e.Node.Tag as FileMetadata);
            }
            else if (e.Node == metadataSummaryNode.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.Users.Key])
            {
                InitializeInformationPanel();
                panelInformation.lvwInformation.ListViewItemSorter = (ListViewColumnSorterValues)panelInformation.lvwInformation.Tag;

                var users = (ConcurrentBag<UserItem>)e.Node.Tag;

                panelInformation.lvwInformation.Groups.Add("UsersFound", string.Format("All users found ({0}) - Times found", users.Count));
                if (users.Count == 0)
                {
                    var lvi = panelInformation.lvwInformation.Items.Add("No users found");
                    lvi.Group = panelInformation.lvwInformation.Groups["UsersFound"];
                }
                else
                {
                    var lvcsv = (ListViewColumnSorterValues)panelInformation.lvwInformation.ListViewItemSorter;
                    panelInformation.lvwInformation.ListViewItemSorter = null;

                    foreach (var s in users)
                        NewItemListView("Name", s.Name, "UsersFound");

                    panelInformation.lvwInformation.ListViewItemSorter = lvcsv;
                }
            }
            else if (e.Node == metadataSummaryNode.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.Folders.Key])
            {
                InitializeInformationPanel();
                panelInformation.lvwInformation.ListViewItemSorter = (ListViewColumnSorterValues)panelInformation.lvwInformation.Tag;

                var folders = (ConcurrentBag<PathsItem>)e.Node.Tag;

                panelInformation.lvwInformation.Groups.Add("FoldersFound", string.Format("All folders found ({0}) - Times found", folders.Count));
                if (folders.Count == 0)
                {
                    var lvi = panelInformation.lvwInformation.Items.Add("No folders found");
                    lvi.Group = panelInformation.lvwInformation.Groups["FoldersFound"];
                }
                else
                {
                    var lvcsv = (ListViewColumnSorterValues)panelInformation.lvwInformation.ListViewItemSorter;
                    panelInformation.lvwInformation.ListViewItemSorter = null;
                    foreach (var s in folders)
                        NewItemListView("Path", s.Path, "FoldersFound");

                    panelInformation.lvwInformation.ListViewItemSorter = lvcsv;
                }
            }
            else if (e.Node == metadataSummaryNode.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.Printers.Key])
            {
                InitializeInformationPanel();
                panelInformation.lvwInformation.ListViewItemSorter = (ListViewColumnSorterValues)panelInformation.lvwInformation.Tag;

                var printers = (ConcurrentBag<PrintersItem>)e.Node.Tag;

                panelInformation.lvwInformation.Groups.Add("PrintersFound", string.Format("All printers found ({0}) - Times found", printers.Count));
                if (printers.Count == 0)
                {
                    var lvi = panelInformation.lvwInformation.Items.Add("No printers found");
                    lvi.Group = panelInformation.lvwInformation.Groups["PrintersFound"];
                }
                else
                {
                    var lvcsv = (ListViewColumnSorterValues)panelInformation.lvwInformation.ListViewItemSorter;
                    panelInformation.lvwInformation.ListViewItemSorter = null;
                    foreach (var s in printers)
                        NewItemListView("Printer Name", s.Printer, "PrintersFound");

                    panelInformation.lvwInformation.ListViewItemSorter = lvcsv;
                }
            }
            else if (e.Node == metadataSummaryNode.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.Software.Key])
            {
                InitializeInformationPanel();
                panelInformation.lvwInformation.ListViewItemSorter = (ListViewColumnSorterValues)panelInformation.lvwInformation.Tag;

                var software = (ConcurrentBag<ApplicationsItem>)e.Node.Tag;

                panelInformation.lvwInformation.Groups.Add("SoftwareFound", string.Format("All software found ({0}) - Times found", software.Count));
                if (software.Count == 0)
                {
                    var lvi = panelInformation.lvwInformation.Items.Add("No software found");
                    lvi.Group = panelInformation.lvwInformation.Groups["SoftwareFound"];
                }
                else
                {
                    var lvcsv = (ListViewColumnSorterValues)panelInformation.lvwInformation.ListViewItemSorter;
                    panelInformation.lvwInformation.ListViewItemSorter = null;
                    foreach (var s in software)
                        NewItemListView("Software", s.Name, "SoftwareFound");

                    panelInformation.lvwInformation.ListViewItemSorter = lvcsv;
                }
            }
            else if (e.Node == metadataSummaryNode.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.Emails.Key])
            {
                InitializeInformationPanel();
                panelInformation.lvwInformation.ListViewItemSorter = (ListViewColumnSorterValues)panelInformation.lvwInformation.Tag;

                var email = (ConcurrentBag<EmailsItem>)e.Node.Tag;

                panelInformation.lvwInformation.Groups.Add("EmailsFound", string.Format("All emails found ({0}) - Times found", email.Count));
                if (email.Count == 0)
                {
                    var lvi = panelInformation.lvwInformation.Items.Add("No emails found");
                    lvi.Group = panelInformation.lvwInformation.Groups["EmailsFound"];
                }
                else
                {
                    var lvcsv = (ListViewColumnSorterValues)panelInformation.lvwInformation.ListViewItemSorter;
                    panelInformation.lvwInformation.ListViewItemSorter = null;
                    foreach (var s in email)
                        NewItemListView("Email", s.Mail, "EmailsFound");

                    panelInformation.lvwInformation.ListViewItemSorter = lvcsv;
                }
            }
            else if (e.Node == metadataSummaryNode.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.OperatingSystems.Key])
            {
                InitializeInformationPanel();
                panelInformation.lvwInformation.ListViewItemSorter = (ListViewColumnSorterValues)panelInformation.lvwInformation.Tag;

                var operatingsystems = (ConcurrentBag<string>)e.Node.Tag;

                panelInformation.lvwInformation.Groups.Add("OperatingSystemsFound", string.Format("All operating systems found ({0}) - Times found", 0));

                if (!operatingsystems.Any())
                {
                    var lvi = panelInformation.lvwInformation.Items.Add("No operating systems found");
                    lvi.Group = panelInformation.lvwInformation.Groups["OperatingSystemsFound"];
                }
                else
                {
                    var lvcsv = (ListViewColumnSorterValues)panelInformation.lvwInformation.ListViewItemSorter;
                    panelInformation.lvwInformation.ListViewItemSorter = null;
                    foreach (var s in operatingsystems)
                        NewItemListView("OS", s, "OperatingSystemsFound");

                    panelInformation.lvwInformation.ListViewItemSorter = lvcsv;
                }
            }
            else if (e.Node == metadataSummaryNode.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.Passwords.Key])
            {
                InitializeInformationPanel();
                panelInformation.lvwInformation.ListViewItemSorter = (ListViewColumnSorterValues)panelInformation.lvwInformation.Tag;

                var passwordsFound = (ConcurrentBag<PasswordsItem>)e.Node.Tag;

                panelInformation.lvwInformation.Groups.Add("PasswordsFound", string.Format("Passwords found ({0}) - Times found", passwordsFound.Count));
                if (passwordsFound.Count == 0)
                {
                    var lvi = panelInformation.lvwInformation.Items.Add("No passwords found");
                    lvi.Group = panelInformation.lvwInformation.Groups["PasswordsFound"];
                }
                else
                {
                    var lvcsv = (ListViewColumnSorterValues)panelInformation.lvwInformation.ListViewItemSorter;
                    panelInformation.lvwInformation.ListViewItemSorter = null;
                    foreach (var s in passwordsFound)
                    {
                        NewItemListView(s.Password, s.Source, "PasswordsFound");
                    }
                    panelInformation.lvwInformation.ListViewItemSorter = lvcsv;
                }
            }
            else if (e.Node == metadataSummaryNode.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.Servers.Key])
            {
                InitializeInformationPanel();
                panelInformation.lvwInformation.ListViewItemSorter = (ListViewColumnSorterValues)panelInformation.lvwInformation.Tag;

                var serversFound = (ConcurrentBag<ServersItem>)e.Node.Tag;

                panelInformation.lvwInformation.Groups.Add("ServersFound", string.Format("Servers found ({0}) - Times found", serversFound.Count));
                if (serversFound.Count == 0)
                {
                    var lvi = panelInformation.lvwInformation.Items.Add("No servers found");
                    lvi.Group = panelInformation.lvwInformation.Groups["ServersFound"];
                }
                else
                {
                    var lvcsv = (ListViewColumnSorterValues)panelInformation.lvwInformation.ListViewItemSorter;
                    panelInformation.lvwInformation.ListViewItemSorter = null;
                    foreach (var s in serversFound)
                        NewItemListView("Server", s.Name, "ServersFound");

                    panelInformation.lvwInformation.ListViewItemSorter = lvcsv;
                }
            }
            else if (e.Node == TreeView.GetNode(GUI.Navigation.Project.DocumentAnalysis.MalwareSummary.ToNavigationPath()))
            {
                InitializeInformationPanel();

                panelInformation.lvwInformation.Groups.Add("MalwareDocuments", "Malware documents");
                panelInformation.lvwInformation.Groups.Add("GoodwareDocuments", "Documents with no malware detected");
                foreach (FilesItem item in Program.data.files.Items.Where(p => p.DiarioAnalyzed))
                {
                    if (item.DiarioPrediction == "Malware")
                    {
                        NewItemListView(item.DiarioPrediction, item.URL, "MalwareDocuments");
                    }
                    else if (item.DiarioPrediction == "Goodware" || item.DiarioPrediction == "NoMacros")
                    {
                        NewItemListView(item.DiarioPrediction, item.URL, "GoodwareDocuments");
                    }
                }
            }
        }

        private void ShowEXIFPanelInformation(FileMetadata exifMetadata)
        {
            InitializePanelInformation();

            if (exifMetadata != null)
            {
                var dicExif = exifMetadata.Makernotes;
                foreach (var dicExifSection in dicExif)
                {
                    panelInformation.lvwInformation.Groups.Add(dicExifSection.Key, dicExifSection.Key);
                    foreach (var dicExifValue in dicExifSection.Value)
                        NewItemListView(dicExifValue.Key, dicExifValue.Value, dicExifSection.Key);

                    var lvit = panelInformation.lvwInformation.Items.Add(string.Empty);
                    lvit.Group = panelInformation.lvwInformation.Groups[dicExifSection.Key];
                }
                if (exifMetadata.Thumbnail == null) return;

                try
                {
                    PictureBox pictureBox = new PictureBox();
                    using (MemoryStream ms = new MemoryStream(exifMetadata.Thumbnail))
                    {
                        pictureBox.Image = new Bitmap(ms);
                    }

                    pictureBox.Height = pictureBox.Image.Height;
                    panelInformation.lvwInformation.Groups.Add("Thumbnail", "Thumbnail");
                    ListViewItem lvi = panelInformation.lvwInformation.Items.Add("Picture");
                    lvi.SubItems.Add(string.Empty);
                    lvi.Group = panelInformation.lvwInformation.Groups["Thumbnail"];
                    panelInformation.lvwInformation.AddEmbeddedControl(pictureBox, 1, lvi.Index, DockStyle.None);

                    int itemHeight = lvi.GetBounds(ItemBoundsPortion.Entire).Height;
                    for (int i = itemHeight; i < pictureBox.Height; i += itemHeight)
                    {
                        lvi = panelInformation.lvwInformation.Items.Add("");
                        lvi.Group = panelInformation.lvwInformation.Groups["Thumbnail"];
                    }
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Set Other Meta paren node.
        /// </summary>
        /// <param name="e"></param>
        private void SetOtherMetaParentNode(TreeViewEventArgs e)
        {
            if (e.Node.Nodes["Other Metadata"] != null)
            {
                panelInformation.lvwInformation.Groups.Add("Other Metadata", "Other Metadata");

                var metaDatos = (MetaData)e.Node.Nodes["Other Metadata"].Tag;
                var valueGroupMetadata = "Other Metadata";

                if (metaDatos.Subject != null)
                    NewItemListView("Subject", metaDatos.Subject, valueGroupMetadata);

                if (metaDatos.DataBase != null)
                    NewItemListView("Database", metaDatos.DataBase, valueGroupMetadata);

                if (metaDatos.Category != null)
                    NewItemListView("Category", metaDatos.Category, valueGroupMetadata);

                if (metaDatos.Codification != null)
                    NewItemListView("Encoding", metaDatos.Codification, valueGroupMetadata);

                if (metaDatos.Comments != null)
                    NewItemListView("Comments", metaDatos.Comments, valueGroupMetadata);

                if (metaDatos.Company != null)
                    NewItemListView("Company", metaDatos.Company, valueGroupMetadata);

                if (metaDatos.Description != null)
                    NewItemListView("Description", metaDatos.Description, valueGroupMetadata);

                if (metaDatos.Statistic != null)
                    NewItemListView("Statistics", metaDatos.Statistic, valueGroupMetadata);

                if (metaDatos.Language != null)
                    NewItemListView("Language", metaDatos.Language, valueGroupMetadata);

                if (metaDatos.UserInfo != null)
                {
                    var lvi = panelInformation.lvwInformation.Items.Add("User defined information");
                    lvi.Group = panelInformation.lvwInformation.Groups["Other Metadata"];
                    var values = metaDatos.UserInfo.Split('\t');
                    foreach (var item in values)
                    {
                        var name = item.IndexOf(": ") > 0 ? item.Remove(item.IndexOf(": ")) : string.Empty;
                        var value = item.IndexOf(": ") > 0 ? item.Substring(item.IndexOf(": ") + 2) : string.Empty;
                        if (name == string.Empty) continue;

                        lvi = panelInformation.lvwInformation.Items.Add("  " + name);
                        lvi.SubItems.Add(value);
                        lvi.Group = panelInformation.lvwInformation.Groups["Other Metadata"];
                    }
                }
                if (metaDatos.VersionNumber != 0)
                    NewItemListView("Revisions", metaDatos.VersionNumber.ToString(), valueGroupMetadata);

                if (metaDatos.Keywords != null)
                    NewItemListView("Keywords", metaDatos.Keywords, valueGroupMetadata);

                if (metaDatos.Template != null)
                    NewItemListView("Template", metaDatos.Template, valueGroupMetadata);

                if (metaDatos.OperativeSystem != null)
                    NewItemListView("Operating system", metaDatos.OperativeSystem, valueGroupMetadata);

                if (metaDatos.EditTime != 0)
                    NewItemListView("Edition time", TimeSpan.FromTicks((long)metaDatos.EditTime).ToString(),
                        valueGroupMetadata);

                if (metaDatos.Title != null)
                    NewItemListView("Title", metaDatos.Title, valueGroupMetadata);

                if (metaDatos.Model != null && metaDatos.Model.Trim() != string.Empty)
                    NewItemListView("Camera Model", metaDatos.Model, valueGroupMetadata);
            }
        }

        /// <summary>
        /// Set old version nodes
        /// </summary>
        /// <param name="e"></param>
        private void SetOldVersionNodes(TreeViewEventArgs e)
        {
            if (e.Node.Nodes["Old versions"] == null) return;
            panelInformation.lvwInformation.Groups.Add("Old versions", "Old versions");

            var va = (OldVersions)e.Node.Nodes["Old versions"].Tag;
            var valueOldVersion = "Old versions";

            foreach (var vai in va.Items)
            {
                if (String.IsNullOrWhiteSpace(vai.Author))
                    NewItemListView("Author", vai.Author, valueOldVersion);

                if (String.IsNullOrWhiteSpace(vai.Comments))
                    NewItemListView("Comments", vai.Comments, valueOldVersion);

                if (vai.SpecificDate)
                    NewItemListView("Date", vai.Date.ToString(), valueOldVersion);

                if (String.IsNullOrWhiteSpace(vai.Path))
                    NewItemListView("Path", vai.Path, valueOldVersion);
            }
        }

        /// <summary>
        /// Set history node.
        /// </summary>
        /// <param name="e"></param>
        private void SetHistoryNode(TreeViewEventArgs e)
        {
            var historyValue = "History";

            panelInformation.lvwInformation.Groups.Add(historyValue, historyValue);
            var historyItems = (Database.Entities.History)e.Node.Tag;
            foreach (var hi in historyItems.Items)
            {
                if (!String.IsNullOrWhiteSpace(hi.Author))
                    NewItemListView("Author", hi.Author, historyValue);

                if (!String.IsNullOrWhiteSpace(hi.Comments))
                    NewItemListView("Comments", hi.Comments, historyValue);

                if (!String.IsNullOrWhiteSpace(hi.Path))
                    NewItemListView("Path", hi.Path, historyValue);
            }
        }

        /// <summary>
        /// Set older version node.
        /// </summary>
        /// <param name="e"></param>
        private void SetOldVersionNode(TreeViewEventArgs e)
        {
            var oldVersionValue = "Old versions";

            panelInformation.lvwInformation.Groups.Add(oldVersionValue, oldVersionValue);
            var va = (OldVersions)e.Node.Tag;
            foreach (var vai in va.Items)
            {
                if (!String.IsNullOrWhiteSpace(vai.Author))
                    NewItemListView("Author", vai.Author, oldVersionValue);

                if (!String.IsNullOrWhiteSpace(vai.Comments))
                    NewItemListView("Comments", vai.Comments, oldVersionValue);

                if (vai.SpecificDate)
                    NewItemListView("Date", vai.Date.ToString(), oldVersionValue);

                if (!String.IsNullOrWhiteSpace(vai.Path))
                    NewItemListView("Path", vai.Path, oldVersionValue);
            }
        }

        /// <summary>
        /// Set other metadata node
        /// </summary>
        /// <param name="e"></param>
        private void SetOtherMetadataNode(TreeViewEventArgs e)
        {
            string otherMetaValue = "Other Metadata";

            panelInformation.lvwInformation.Groups.Add("Other Metadata", "Other Metadata");
            MetaData metaDatadosValue = (MetaData)e.Node.Tag;

            if (metaDatadosValue.Subject != null && metaDatadosValue.Subject.Trim() != string.Empty)
                NewItemListView("Subject", metaDatadosValue.Subject, otherMetaValue);

            if (metaDatadosValue.DataBase != null && metaDatadosValue.DataBase.Trim() != string.Empty)
                NewItemListView("Database", metaDatadosValue.DataBase, otherMetaValue);

            if (metaDatadosValue.Category != null && metaDatadosValue.Category.Trim() != string.Empty)
                NewItemListView("Category", metaDatadosValue.Category, otherMetaValue);

            if (metaDatadosValue.Codification != null && metaDatadosValue.Codification.Trim() != string.Empty)
                NewItemListView("Encoding", metaDatadosValue.Codification, otherMetaValue);

            if (metaDatadosValue.Comments != null && metaDatadosValue.Comments.Trim() != string.Empty)
                NewItemListView("Comments", metaDatadosValue.Comments, otherMetaValue);

            if (metaDatadosValue.Company != null && metaDatadosValue.Company.Trim() != string.Empty)
                NewItemListView("Company", metaDatadosValue.Company, otherMetaValue);

            if (metaDatadosValue.Description != null && metaDatadosValue.Description.Trim() != string.Empty)
                NewItemListView("Description", metaDatadosValue.Description, otherMetaValue);

            if (metaDatadosValue.Statistic != null && metaDatadosValue.Statistic.Trim() != string.Empty)
                NewItemListView("Statistics", metaDatadosValue.Statistic, otherMetaValue);

            if (metaDatadosValue.Language != null && metaDatadosValue.Language.Trim() != string.Empty)
                NewItemListView("Language", metaDatadosValue.Language, otherMetaValue);

            if (metaDatadosValue.UserInfo != null &&
                metaDatadosValue.UserInfo.Trim() != string.Empty)
                NewItemListView("User defined information", metaDatadosValue.UserInfo, otherMetaValue);

            if (metaDatadosValue.VersionNumber != 0)
                NewItemListView("Revisions", metaDatadosValue.VersionNumber.ToString(), otherMetaValue);

            if (metaDatadosValue.Keywords != null && metaDatadosValue.Keywords.Trim() != string.Empty)
                NewItemListView("Keywords", metaDatadosValue.Keywords, otherMetaValue);

            if (metaDatadosValue.Template != null && metaDatadosValue.Template.Trim() != string.Empty)
                NewItemListView("Template", metaDatadosValue.Template, otherMetaValue);

            if (metaDatadosValue.OperativeSystem != null && metaDatadosValue.OperativeSystem.Trim() != string.Empty)
                NewItemListView("Operating system", metaDatadosValue.OperativeSystem, otherMetaValue);

            if (metaDatadosValue.EditTime != 0)
                NewItemListView("Edition time", TimeSpan.FromTicks((long)metaDatadosValue.EditTime).ToString(),
                    otherMetaValue);

            if (metaDatadosValue.Title != null && metaDatadosValue.Title.Trim() != string.Empty)
                NewItemListView("Title", metaDatadosValue.Title, otherMetaValue);

            if (metaDatadosValue.Model != null && metaDatadosValue.Model.Trim() != string.Empty)
                NewItemListView("Camera Model", metaDatadosValue.Model, otherMetaValue);
        }

        #endregion

        #region Events TreeViewNetwork

        /// <summary>
        /// Delete nodes
        /// </summary>
        /// <param name="nodes"></param>
        public void DeleteNodesServers(TreeNodeCollection nodes)
        {
            for (var i = nodes.Count; i > 0; i--)
            {
                var tn = nodes[i - 1];
                if (tn.Tag is ComputersItem ci)
                {
                    if (Program.data.computers.Items.All(c => c.name != ci.name) ||
                        tn.Text != ci.name)
                        Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(delegate
                        {
                            tn.Remove();
                        }));

                    else if (tn.Parent != null && tn.Parent.Text == "Unknown Servers")
                    {
                        if (Program.data.computerIPs.Items.Any(c => c.Computer == ci))
                            Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(delegate
                            {
                                tn.Remove();
                            }));
                    }
                }
                else
                {
                    DeleteNodesServers(tn.Nodes);
                    if (Functions.IsIP(tn.Text) && tn.Nodes.Count == 0)
                        Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(delegate
                        {
                            tn.Remove();
                        }));
                }
            }
        }

        /// <summary>
        /// Remember last computer.
        /// </summary>
        private ComputersItem _lastComputerShown = null;

        private void treeViewNetwork_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is ComputersItem ci)
            {
                LoadInformationGui();

                if (_lastComputerShown != ci)
                {
                    panelInformation.lvwInformation.Items.Clear();
                    panelInformation.lvwInformation.Groups.Clear();
                    panelInformation.lvwInformation.ListViewItemSorter = null;
                    _lastComputerShown = ci;
                }

                //Si tiene nombre o sistema operativo entonces mostrar el apartado Information
                if (!string.IsNullOrEmpty(ci.name) || ci.os != OperatingSystem.OS.Unknown)
                {
                    if (panelInformation.lvwInformation.Groups["Information"] == null)
                        panelInformation.lvwInformation.Groups.Add("Information", "Information");

                    if (!string.IsNullOrEmpty(ci.name))
                    {
                        if (panelInformation.lvwInformation.Items["Name"] == null)
                        {
                            var lvi = panelInformation.lvwInformation.Items.Add("Name", "Name", 0);
                            lvi.SubItems.Add(ci.name);
                            lvi.Group = panelInformation.lvwInformation.Groups["Information"];
                        }
                    }
                    if (!string.IsNullOrEmpty(ci.localName))
                    {
                        if (panelInformation.lvwInformation.Items["Local name"] == null)
                        {
                            var lvi = panelInformation.lvwInformation.Items.Add("Local name", "Local name", 0);
                            lvi.SubItems.Add(ci.localName);
                            lvi.Group = panelInformation.lvwInformation.Groups["Information"];
                        }
                    }
                    if (ci.os != OperatingSystem.OS.Unknown)
                    {
                        if (panelInformation.lvwInformation.Items["Operating System"] == null)
                        {
                            var lvi = panelInformation.lvwInformation.Items.Add("Operating System", "Operating System", 0);
                            lvi.SubItems.Add(OperatingSystemUtils.OSToString(ci.os));
                            lvi.Group = panelInformation.lvwInformation.Groups["Information"];
                        }
                    }
                }

                if (Program.data.computerDomains.Items.Any(c => c.Computer.name == ci.name))
                {
                    if (panelInformation.lvwInformation.Groups["domains"] == null)
                        panelInformation.lvwInformation.Groups.Add("domains", "domains - Source");

                    var lstDomains = new List<ComputerDomainsItem>(Program.data.computerDomains.Items.Where(c => c.Computer.name == ci.name));
                    foreach (var cdi in lstDomains)
                    {
                        if (panelInformation.lvwInformation.Items[cdi.Domain.Domain] != null) continue;

                        var lvi = panelInformation.lvwInformation.Items.Add(cdi.Domain.Domain, cdi.Domain.Domain, 0);
                        lvi.SubItems.Add(cdi.Source);
                        lvi.Group = panelInformation.lvwInformation.Groups["domains"];
                    }
                }
                if (Program.data.computerIPs.Items.Any(c => c.Computer.name == ci.name))
                {
                    if (panelInformation.lvwInformation.Groups["IP Addresses"] == null)
                        panelInformation.lvwInformation.Groups.Add("IP Addresses", "IP Addresses - Source");
                    var lstIPs = new List<ComputerIPsItem>(Program.data.computerIPs.Items.Where(c => c.Computer.name == ci.name));
                    foreach (var cii in lstIPs)
                    {
                        if (panelInformation.lvwInformation.Items[cii.Ip.Ip] != null) continue;

                        var lvi = panelInformation.lvwInformation.Items.Add(cii.Ip.Ip, cii.Ip.Ip, 0);
                        lvi.SubItems.Add(cii.Source);
                        lvi.Group = panelInformation.lvwInformation.Groups["IP Addresses"];

                        NetRange nRange = cii.Ip.GetNetrange();
                        if (nRange != null)
                        {
                            lvi = panelInformation.lvwInformation.Items.Add(nRange.ToString(), nRange.ToString(), 0);
                            lvi.SubItems.Add("Netrange");
                            lvi.Group = panelInformation.lvwInformation.Groups["IP Addresses"];
                        }
                    }
                }
                if (ci.type == ComputersItem.Tipo.Server)
                {
                    if (panelInformation.lvwInformation.Groups["FingerPrinting"] == null)
                        panelInformation.lvwInformation.Groups.Add("FingerPrinting", "FingerPrinting");

                    var lstIPs = new List<ComputerIPsItem>(Program.data.computerIPs.Items.Where(c => c.Computer.name == ci.name));
                    foreach (var cii in lstIPs)
                    {
                        if (cii.Ip.Information == null || string.IsNullOrEmpty(cii.Ip.Information.ServerBanner))
                            continue;
                        var s = "Shodan (" + cii.Ip.Ip + ")";
                        if (panelInformation.lvwInformation.Items[s] != null) continue;

                        var lvi = panelInformation.lvwInformation.Items.Add(s, s, 0);
                        lvi.SubItems.Add(cii.Ip.Information.ServerBanner);
                        lvi.Group = panelInformation.lvwInformation.Groups["FingerPrinting"];
                    }

                    panelInformation.lvwInformation.Groups.Add("FingerPrintingHTTP", "FingerPrinting - HTTP");
                    panelInformation.lvwInformation.Groups.Add("FingerPrintingSMTP", "FingerPrinting - SMTP");
                    panelInformation.lvwInformation.Groups.Add("FingerPrintingFTP", "FingerPrinting - FTP");
                    panelInformation.lvwInformation.Groups.Add("FingerPrintingDNS", "FingerPrinting - DNS");
                    var lstDomains = new List<ComputerDomainsItem>(Program.data.computerDomains.Items.Where(c => c.Computer.name == ci.name));
                    foreach (var cdi in lstDomains)
                    {
                        for (var fpI = 0; fpI < cdi.Domain.fingerPrinting.Count; fpI++)
                        {
                            var fp = cdi.Domain.fingerPrinting[fpI];
                            var lvi = new ListViewItem();

                            if (fp is HTTP http)
                            {
                                if (panelInformation.lvwInformation.Groups["FingerPrintingHTTP"].Items.Cast<ListViewItem>().Any(item => item.Text == fp.Host + ":" + fp.Port))
                                    continue;

                                panelInformation.lvwInformation.Groups.Add("FingerPrintingHTTP", "FingerPrinting - HTTP");
                                lvi = panelInformation.lvwInformation.Items.Add(http.Host + ":" + http.Port);
                                lvi.Group = panelInformation.lvwInformation.Groups["FingerPrintingHTTP"];

                                lvi.SubItems.Add(http.Version);
                                lvi.Tag = http.Version;

                                if (string.IsNullOrEmpty(((HTTP)fp).Title)) continue;

                                panelInformation.lvwInformation.Groups.Add("Http Title", "HTML Title");

                                lvi = panelInformation.lvwInformation.Items.Add(http.Host + ":" + http.Port);
                                lvi.Group = panelInformation.lvwInformation.Groups["Http Title"];

                                lvi.SubItems.Add(http.Title);
                                lvi.Tag = http.Title;
                            }
                            else
                            {
                                if (fp is SMTP smtp)
                                {
                                    if (panelInformation.lvwInformation.Groups["FingerPrintingSMTP"].Items.Cast<ListViewItem>().Any(item => item.Text == smtp.Host + ":" + smtp.Port))
                                        continue;

                                    panelInformation.lvwInformation.Groups.Add("FingerPrintingSMTP", "FingerPrinting - SMTP");
                                    lvi = panelInformation.lvwInformation.Items.Add(smtp.Host + ":" + smtp.Port);
                                    lvi.Group = panelInformation.lvwInformation.Groups["FingerPrintingSMTP"];

                                    lvi.SubItems.Add(smtp.Version);
                                    lvi.Tag = smtp.Version;
                                }
                                else if (fp is FTP ftp)
                                {
                                    if (panelInformation.lvwInformation.Groups["FingerPrintingFTP"].Items.Cast<ListViewItem>().Any(item => item.Text == ftp.Host + ":" + ftp.Port))
                                        continue;

                                    panelInformation.lvwInformation.Groups.Add("FingerPrintingFTP", "FingerPrinting - FTP");
                                    lvi = panelInformation.lvwInformation.Items.Add(ftp.Host + ":" + ftp.Port);
                                    lvi.Group = panelInformation.lvwInformation.Groups["FingerPrintingFTP"];

                                    lvi.SubItems.Add(ftp.Version);
                                    lvi.Tag = ftp.Version;
                                }
                                else if (fp is DNS dns)
                                {
                                    if (panelInformation.lvwInformation.Groups["FingerPrintingDNS"].Items.Cast<ListViewItem>().Any(item => item.Text == dns.Host + ":" + dns.Port))
                                        continue;

                                    panelInformation.lvwInformation.Groups.Add("FingerPrintingDNS", "FingerPrinting - DNS");
                                    lvi = panelInformation.lvwInformation.Items.Add(dns.Host + ":" + dns.Port);
                                    lvi.Group = panelInformation.lvwInformation.Groups["FingerPrintingDNS"];

                                    lvi.SubItems.Add(dns.Version);
                                    lvi.Tag = dns.Version;
                                }
                            }
                        }
                    }
                }
                if (e.Node.Nodes["Users"] != null)
                {
                    if (panelInformation.lvwInformation.Groups["Users"] == null)
                        panelInformation.lvwInformation.Groups.Add("Users", "Users");
                    var u = (Users)e.Node.Nodes["Users"].Tag;

                    foreach (var ui in u.Items)
                    {
                        if (panelInformation.lvwInformation.Items[ui.Name] != null) continue;
                        var lvi = panelInformation.lvwInformation.Items.Add(ui.Name, ui.Name, 0);

                        if (!string.IsNullOrEmpty(ui.Notes))
                            lvi.SubItems.Add(ui.Notes);

                        lvi.Group = panelInformation.lvwInformation.Groups["Users"];
                    }
                }
                if (e.Node.Nodes["Description"] != null)
                {
                    if (panelInformation.lvwInformation.Groups["Description"] == null)
                        panelInformation.lvwInformation.Groups.Add("Description", "Description");
                    var d = (Descriptions)e.Node.Nodes["Description"].Tag;
                    foreach (var di in d.Items)
                    {
                        if (panelInformation.lvwInformation.Items[di.Description] != null) continue;

                        var lvi = panelInformation.lvwInformation.Items.Add(di.Description, di.Description, 0);
                        if (!string.IsNullOrEmpty(di.Description))
                            lvi.SubItems.Add(di.Source);
                        lvi.Group = panelInformation.lvwInformation.Groups["Description"];
                    }
                }
                if (e.Node.Nodes["Passwords"] != null)
                {
                    if (panelInformation.lvwInformation.Groups["Passwords"] == null)
                        panelInformation.lvwInformation.Groups.Add("Passwords", "Passwords");
                    var p = (Passwords)e.Node.Nodes["Passwords"].Tag;
                    foreach (var pi in p.Items)
                    {
                        if (panelInformation.lvwInformation.Items[pi.Password] != null) continue;
                        var lvi = panelInformation.lvwInformation.Items.Add(pi.Password, pi.Password, 0);
                        if (!string.IsNullOrEmpty(pi.Type))
                            lvi.SubItems.Add(pi.Type);
                        lvi.Group = panelInformation.lvwInformation.Groups["Passwords"];
                    }
                }
                if (e.Node.Nodes["Servers"] != null)
                {
                    if (panelInformation.lvwInformation.Groups["Servers"] == null)
                        panelInformation.lvwInformation.Groups.Add("Servers", "Servers");
                    var s = (Servers)e.Node.Nodes["Servers"].Tag;

                    foreach (ListViewItem lvi in from si in s.Items where panelInformation.lvwInformation.Items[si.Name] == null select panelInformation.lvwInformation.Items.Add(si.Name, si.Name, 0))
                    {
                        lvi.Group = panelInformation.lvwInformation.Groups["Servers"];
                    }
                }
                if (e.Node.Nodes["Users with access"] != null)
                {
                    if (panelInformation.lvwInformation.Groups["Users with access"] == null)
                        panelInformation.lvwInformation.Groups.Add("Users with access", "Users with access");
                    var u = (Users)e.Node.Nodes["Users with access"].Tag;
                    foreach (var ui in u.Items)
                    {
                        if (panelInformation.lvwInformation.Items[ui.Name] != null) continue;

                        var lvi = panelInformation.lvwInformation.Items.Add(ui.Name, ui.Name, 0);
                        if (!string.IsNullOrEmpty(ui.Notes))
                            lvi.SubItems.Add(ui.Notes);
                        lvi.Group = panelInformation.lvwInformation.Groups["Users with access"];
                    }
                }
                if (e.Node.Nodes["Folders"] != null && ((Paths)e.Node.Nodes["Folders"].Tag).Items.Count > 0)
                {
                    if (panelInformation.lvwInformation.Groups["Folders"] == null)
                        panelInformation.lvwInformation.Groups.Add("Folders", "Folders");
                    var r = (Paths)e.Node.Nodes["Folders"].Tag;
                    foreach (var lvi in from ri in r.Items where panelInformation.lvwInformation.Items[ri.Path] == null select panelInformation.lvwInformation.Items.Add(ri.Path, ri.Path, 0))
                    {
                        lvi.Group = panelInformation.lvwInformation.Groups["Folders"];
                    }
                }
                if (e.Node.Nodes["Remote Folders"] != null && ((Paths)e.Node.Nodes["Remote Folders"].Tag).Items.Count > 0)
                {
                    if (panelInformation.lvwInformation.Groups["Remote Folders"] == null)
                        panelInformation.lvwInformation.Groups.Add("Remote Folders", "Remote Folders");
                    var r = (Paths)e.Node.Nodes["Remote Folders"].Tag;

                    foreach (var lvi in from ri in r.Items where panelInformation.lvwInformation.Items[ri.Path] == null select panelInformation.lvwInformation.Items.Add(ri.Path, ri.Path, 0))
                    {
                        lvi.Group = panelInformation.lvwInformation.Groups["Remote Folders"];
                    }
                }
                if (e.Node.Nodes["Printers"] != null && ((Printers)e.Node.Nodes["Printers"].Tag).Items.Count > 0)
                {
                    if (panelInformation.lvwInformation.Groups["Printers"] == null)
                        panelInformation.lvwInformation.Groups.Add("Printers", "Printers");
                    var i = (Printers)e.Node.Nodes["Printers"].Tag;
                    foreach (ListViewItem lvi in from ii in i.Items where panelInformation.lvwInformation.Items[ii.Printer] == null select panelInformation.lvwInformation.Items.Add(ii.Printer, ii.Printer, 0))
                    {
                        lvi.Group = panelInformation.lvwInformation.Groups["Printers"];
                    }
                }
                if (e.Node.Nodes["Remote Printers"] != null && ((Printers)e.Node.Nodes["Remote Printers"].Tag).Items.Count > 0)
                {
                    if (panelInformation.lvwInformation.Groups["Remote Printers"] == null)
                        panelInformation.lvwInformation.Groups.Add("Remote Printers", "Remote Printers");
                    var i = (Printers)e.Node.Nodes["Remote Printers"].Tag;
                    foreach (ListViewItem lvi in from ii in i.Items where panelInformation.lvwInformation.Items[ii.Printer] == null select panelInformation.lvwInformation.Items.Add(ii.Printer, ii.Printer, 0))
                    {
                        lvi.Group = panelInformation.lvwInformation.Groups["Remote Printers"];
                    }
                }
                if (ci.Software != null && ci.Software.Items.Count > 0)
                {
                    if (panelInformation.lvwInformation.Groups["Software"] == null)
                        panelInformation.lvwInformation.Groups.Add("Software", "Software");
                    foreach (var aplicacion in ci.Software.Items)
                    {
                        if (panelInformation.lvwInformation.Items[aplicacion.Name] == null)
                        {
                            var lvi = panelInformation.lvwInformation.Items.Add(aplicacion.Name, aplicacion.Name, 0);
                            lvi.SubItems.Add(aplicacion.Source);
                            lvi.Group = panelInformation.lvwInformation.Groups["Software"];
                        }
                    }
                }
                if (((ComputersItem)e.Node.Tag).SourceDocuments == null ||
                    ((ComputersItem)e.Node.Tag).SourceDocuments.Count == 0) return;

                if (panelInformation.lvwInformation.Groups["Documents used to infer this computer"] == null)
                    panelInformation.lvwInformation.Groups.Add("Documents used to infer this computer", "Documents used to infer this computer");
                var documents = ((ComputersItem)e.Node.Tag).SourceDocuments;

                foreach (var lvi in from document in documents where panelInformation.lvwInformation.Items[document] == null select panelInformation.lvwInformation.Items.Add(document, document, 0))
                {
                    lvi.Group = panelInformation.lvwInformation.Groups["Documents used to infer this computer"];
                }
            }
            else
            {
                _lastComputerShown = null;
                if (Functions.IsIP(e.Node.Text) && e.Node.Nodes.Count > 0)
                {
                    InitializeInformationPanel();

                    panelInformation.lvwInformation.Groups.Add("Servers in Range", "Servers in Range");
                    foreach (var lvi in from TreeNode tn in e.Node.Nodes select panelInformation.lvwInformation.Items.Add(tn.Text))
                    {
                        lvi.Group = panelInformation.lvwInformation.Groups["Servers in Range"];
                    }
                }
                else if (e.Node.Parent?.Tag is ComputersItem && e.Node.Text == "Users")
                {
                    InitializeInformationPanel();

                    panelInformation.lvwInformation.Groups.Add("Users", "Users");
                    var u = (Users)e.Node.Tag;
                    foreach (var ui in u.Items)
                    {
                        var lvi = panelInformation.lvwInformation.Items.Add(ui.Name);
                        if (!string.IsNullOrEmpty(ui.Notes))
                            lvi.SubItems.Add(ui.Notes);
                        lvi.Group = panelInformation.lvwInformation.Groups["Users"];
                    }
                }
                else if (e.Node.Parent?.Tag is ComputersItem && e.Node.Text == "Users with access")
                {
                    InitializeInformationPanel();

                    panelInformation.lvwInformation.Groups.Add("Users with access", "Users with access");
                    var u = (Users)e.Node.Tag;
                    foreach (var ui in u.Items)
                    {
                        var lvi = panelInformation.lvwInformation.Items.Add(ui.Name);
                        if (!string.IsNullOrEmpty(ui.Notes))
                            lvi.SubItems.Add(ui.Notes);

                        lvi.Group = panelInformation.lvwInformation.Groups["Users with access"];
                    }
                }
                else if (e.Node.Parent?.Tag is ComputersItem && e.Node.Text == "Passwords")
                {
                    InitializeInformationPanel();

                    panelInformation.lvwInformation.Groups.Add("Passwords", "Passwords");
                    var p = (Passwords)e.Node.Tag;
                    foreach (var pi in p.Items)
                    {
                        var lvi = panelInformation.lvwInformation.Items.Add(pi.Password);
                        if (!string.IsNullOrEmpty(pi.Source))
                            lvi.SubItems.Add(pi.Source);

                        lvi.Group = panelInformation.lvwInformation.Groups["Passwords"];
                    }
                }
                else if (e.Node.Parent != null && e.Node.Parent.Tag is ComputersItem && e.Node.Text == "Folders" && e.Node.Parent.Parent?.Name == "Clients")
                {
                    InitializeInformationPanel();

                    panelInformation.lvwInformation.Groups.Add("Folders", "Folders");
                    var r = (Paths)e.Node.Tag;
                    foreach (ListViewItem lvi in r.Items.Select(ri => panelInformation.lvwInformation.Items.Add(ri.Path)))
                        lvi.Group = panelInformation.lvwInformation.Groups["Folders"];
                }
                else if (e.Node.Parent?.Parent?.Tag is ComputersItem && e.Node.Text == "Folders")
                {
                    InitializeInformationPanel();

                    var r = (Paths)e.Node.Tag;
                    foreach (var ri in r.Items)
                    {
                        panelInformation.lvwInformation.Groups.Add(ri.Path, string.Format("Folder: {0}", ri.Path));
                        if (ri.RemoteUsers.Items.Count > 0)
                        {
                            var lvi = panelInformation.lvwInformation.Items.Add("Users with access");
                            lvi.SubItems.Add(ri.RemoteUsers.Items[0].Name);
                            lvi.Group = panelInformation.lvwInformation.Groups[ri.Path];
                            for (var i = 1; i < ri.RemoteUsers.Items.Count; i++)
                            {
                                lvi = panelInformation.lvwInformation.Items.Add(string.Empty);
                                lvi.SubItems.Add(ri.RemoteUsers.Items[i].Name);
                                lvi.Group = panelInformation.lvwInformation.Groups[ri.Path];
                            }
                        }
                        else
                        {
                            var lvi = panelInformation.lvwInformation.Items.Add(string.Empty);
                            lvi.Group = panelInformation.lvwInformation.Groups[ri.Path];
                        }
                    }
                }
                else if (e.Node.Parent?.Parent?.Tag is ComputersItem && e.Node.Text == "Remote Folders")
                {
                    InitializeInformationPanel();

                    panelInformation.lvwInformation.Groups.Add("Remote Folders", "Remote Folders");
                    var r = (Paths)e.Node.Tag;
                    foreach (var ri in r.Items)
                    {
                        var lvi = panelInformation.lvwInformation.Items.Add(ri.Path);
                        lvi.Group = panelInformation.lvwInformation.Groups["Remote Folders"];
                    }
                }
                else if (e.Node.Parent?.Parent?.Tag is ComputersItem && e.Node.Text == "Printers")
                {
                    InitializeInformationPanel();

                    var i = (Printers)e.Node.Tag;
                    foreach (var ii in i.Items)
                    {
                        panelInformation.lvwInformation.Groups.Add(ii.Printer, string.Format("Printer: {0}", ii.Printer));

                        if (ii.RemoteUsers.Items.Count > 0)
                        {
                            var lvi = panelInformation.lvwInformation.Items.Add("Users with access");
                            lvi.SubItems.Add(ii.RemoteUsers.Items[0].Name);
                            lvi.Group = panelInformation.lvwInformation.Groups[ii.Printer];

                            for (var j = 1; j < ii.RemoteUsers.Items.Count; j++)
                            {
                                lvi = panelInformation.lvwInformation.Items.Add(string.Empty);
                                lvi.SubItems.Add(ii.RemoteUsers.Items[j].Name);
                                lvi.Group = panelInformation.lvwInformation.Groups[ii.Printer];
                            }
                        }
                        else
                        {
                            var lvi = panelInformation.lvwInformation.Items.Add(string.Empty);
                            lvi.Group = panelInformation.lvwInformation.Groups[ii.Printer];
                        }
                    }
                }
                else if (e.Node.Parent?.Parent?.Tag is ComputersItem && e.Node.Text == "Remote Printers")
                {
                    InitializeInformationPanel();

                    panelInformation.lvwInformation.Groups.Add("Remote Printers", "Remote Printers");
                    var i = (Printers)e.Node.Tag;
                    foreach (var lvi in i.Items.Select(ii => panelInformation.lvwInformation.Items.Add(ii.Printer)))
                    {
                        lvi.Group = panelInformation.lvwInformation.Groups["Remote Printers"];
                    }
                }
            }
        }

        /// <summary>
        /// Initialize values in panel information.
        /// </summary>
        private void InitializeInformationPanel()
        {
            LoadInformationGui();
            panelInformation.lvwInformation.Items.Clear();
            panelInformation.lvwInformation.Groups.Clear();
            panelInformation.lvwInformation.ListViewItemSorter = null;
        }

        #region Events contextMenuStripNetwork

        public void ViewDocumentsUsedFor(ComputersItem ci)
        {
            var formSearchInstance = new FormDocumentsSearch(this);
            formSearchInstance.Text = "Documents used to infer computer " + ci.name;
            formSearchInstance.lstDocumentsFound.Items.Clear();
            foreach (string document in ci.SourceDocuments)
            {
                formSearchInstance.lstDocumentsFound.Items.Add(document);
            }
            formSearchInstance.Show();
        }

        /// <summary>
        /// Search shodan ip.
        /// </summary>
        public void ScanIpRangeShodan(object baseIp)
        {
            var strBaseIp = (string)baseIp;
            List<IPAddress> lst = new List<IPAddress>();

            for (var i = 0; i < 254; i++)
            {
                var targetIp = strBaseIp + i;
                if (Program.data.Project.IsIpInNetrange(targetIp) && IPAddress.TryParse(targetIp, out IPAddress currentIP))
                    lst.Add(currentIP);
            }

            SearchIpInShodanAsync(lst);
        }

        /// <summary>
        /// Search for Bing from 254.
        /// </summary>
        /// <param name="baseIp">Espera una IP sin el último byte</param>
        public void ScanIpRangeBing(object baseIp)
        {
            var strBaseIp = (string)baseIp;

            for (var i = 0; i < 254; i++)
            {
                var targetIp = strBaseIp + i;
                if (Program.data.Project.IsIpInNetrange(targetIp))
                    SearchDomainsInBing(targetIp);
            }
        }

        /// <summary>
        /// Scan from Ip
        /// </summary>
        /// <param name="parameter">Object</param>
        public void ScanIpRangeIcmp(object parameter)
        {
            object[] parameters = (object[])parameter;
            var lowerIp = (IPAddress)parameters[0];
            var upperIp = (IPAddress)parameters[1];
            var includeInNetworkMap = (bool)parameters[2];

            if (Functions.IPToInt(lowerIp.ToString()) > Functions.IPToInt(upperIp.ToString())) return;

            try
            {
                Invoke(new MethodInvoker(delegate
                {
                    toolStripDropDownButtonStop.Enabled = true;
                    var strMessage = $"Searching hosts in range {lowerIp}-{upperIp}";
                    toolStripStatusLabelLeft.Text = strMessage;
                    Program.LogThis(new Log(Log.ModuleType.ScanIPRangeICMP, strMessage, Log.LogType.debug));
                }));

                var ips = new List<IPAddress>();
                for (var ipCurrent = lowerIp; !ipCurrent.Equals(upperIp); ipCurrent = DNSUtil.IncrementIP(ipCurrent))
                    ips.Add(ipCurrent);

                var po = new ParallelOptions();
                if (Program.cfgCurrent.ParallelDnsQueries != 0)
                    po.MaxDegreeOfParallelism = Program.cfgCurrent.ParallelDnsQueries;

                Parallel.For(0, ips.Count, po, delegate (int i)
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        ReportProgress(i, ips.Count);
                        toolStripProgressBarDownload.Value = i * 100 / ips.Count;
                        toolStripStatusLabelLeft.Text = $"Searching host in ip {ips[i]}";
                    }));
                    if (!Pinger.IsIPAlive(ips[i])) return;

                    Program.LogThis(new Log(Log.ModuleType.ScanIPRangeICMP, $"Found host alive (ICMP) {ips[i]} ", Log.LogType.medium));

                    var strSource = $"Searching hosts in IP range (ICMP)[{ips[i]}]";

                    Program.data.AddIP(ips[i].ToString(), strSource, Program.data.Project.Domain, Program.cfgCurrent.MaxRecursion, true);
                    if (!includeInNetworkMap) return;

                    var ci = new ComputersItem();
                    Program.data.computerIPs.Items.Add(new ComputerIPsItem(ci, Program.data.GetIp(ips[i].ToString()), strSource));

                    ci.name = ips[i].ToString();
                    ci.type = ComputersItem.Tipo.Server;
                    ci.os = OperatingSystem.OS.Unknown;

                    Program.data.computers.Items.Add(ci);
                }
                );

                Invoke(new MethodInvoker(delegate
                {
                    var strMessage = $"Searching hosts in range {lowerIp}-{upperIp}";
                    toolStripStatusLabelLeft.Text = strMessage;
                    Program.LogThis(new Log(Log.ModuleType.ScanIPRangeICMP, strMessage, Log.LogType.debug));
                }));
            }
            catch (ThreadAbortException)
            {
                Invoke(new MethodInvoker(delegate
                {
                    var strMessage = "Searching hosts aborted!";
                    toolStripStatusLabelLeft.Text = strMessage;
                    Program.LogThis(new Log(Log.ModuleType.ScanIPRangeICMP, strMessage, Log.LogType.debug));
                }));
            }
            finally
            {
                Invoke(new MethodInvoker(delegate
                {
                    ReportProgress(0, 254);
                    toolStripDropDownButtonStop.Enabled = false;
                    toolStripProgressBarDownload.Value = 0;
                    ScannThread = null;
                }));
            }
        }

        /// <summary>
        /// Resolve domains
        /// </summary>
        /// <param name="domains">Lista de dominios a resolver</param>
        public void TryDomains(object domains)
        {
            var lstDomains = (List<string>)domains;
            try
            {
                if (lstDomains.Count == 0) return;
                if (DNSUtilities.isDNSAnyCast(lstDomains[0]))
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        var strMessage = $"DNS {lstDomains[0]} resolve all domains!";
                        toolStripStatusLabelLeft.Text = strMessage;
                        Program.LogThis(new Log(Log.ModuleType.DNSSearch, strMessage, Log.LogType.low));
                    }));
                    return;
                }

                Invoke(new MethodInvoker(delegate
                {
                    toolStripDropDownButtonStop.Enabled = true;
                    var strMessage = "Try solving domains";
                    toolStripStatusLabelLeft.Text = strMessage;
                    Program.LogThis(new Log(Log.ModuleType.DNSSearch, strMessage, Log.LogType.debug));
                }));
                var newDomains = 0;
                for (var i = 0; i < lstDomains.Count; i++)
                {
                    var strDomain = lstDomains[i];
                    Invoke(new MethodInvoker(delegate
                    {
                        toolStripProgressBarDownload.Value = i * 100 / lstDomains.Count;
                        toolStripStatusLabelLeft.Text = $"Try resolving domain {strDomain}";
                    }));
                    IPAddress[] ips;
                    if (!DNSUtilities.ExistsDomain(strDomain, out ips)) continue;

                    Program.LogThis(new Log(Log.ModuleType.DNSSearch, $"Domain found: {strDomain}", Log.LogType.medium));
                    foreach (var ip in ips)
                    {
                        var strIp = ip.ToString();
                        if (Program.data.GetDomain(strDomain) != null && Program.data.GetIp(strIp) != null &&
                            Program.data.GetResolutionDomains(strIp).All(c => c.Domain != strDomain)) continue;

                        var strSource = $"Resolving domain [{strDomain}]";
                        Program.data.AddResolution(strDomain, ip.ToString(), strSource, 0, Program.cfgCurrent, true);
                        newDomains++;
                    }
                }
                Invoke(new MethodInvoker(delegate
                {
                    string strMessage = String.Format("Resolving {0} domains finished, {1} new domains found!", lstDomains.Count, newDomains);
                    toolStripStatusLabelLeft.Text = strMessage;
                    Program.LogThis(new Log(Log.ModuleType.DNSSearch, strMessage, Log.LogType.low));
                }));
            }
            catch (ThreadAbortException)
            {
                Invoke(new MethodInvoker(delegate
                {
                    var strMessage = "Resolving domains aborted!";
                    toolStripStatusLabelLeft.Text = strMessage;
                    Program.LogThis(new Log(Log.ModuleType.DNSSearch, strMessage, Log.LogType.debug));
                }));
            }
            finally
            {
                Invoke(new MethodInvoker(delegate
                {
                    toolStripDropDownButtonStop.Enabled = false;
                    toolStripProgressBarDownload.Value = 0;
                    ScannThread = null;
                }));
            }
        }

        /// <summary>
        /// Search domain in Bing by Ip range.
        /// </summary>
        /// <param name="oIp"></param>
        private void SearchDomainsInBing(string oIp)
        {
            try
            {
                string message = string.Format("[BingWeb] Searching domains in IP {0}", oIp);

                Program.LogThis(new Log(Log.ModuleType.IPBingSearch, message, Log.LogType.debug));
                Program.ChangeStatus(message);

                var bingSearcher = new BingWebSearcher();

                bingSearcher.LocatedInRegion = BingWebSearcher.Region.AnyRegion;
                bingSearcher.WriteInLanguage = BingWebSearcher.Language.AnyLanguage;

                var currentResults = new List<string>();

                bingSearcher.ItemsFoundEvent += delegate (object sender, EventsThreads.CollectionFound<Uri> e)
                {
                    foreach (Uri url in e.Data)
                    {
                        try
                        {
                            if (currentResults.Any(d => d.ToLower() == url.Host.ToLower())) continue;

                            currentResults.Add(url.Host);
                            var source = $"{Program.data.GetIpSource(oIp)} > Bing IP Search [{url.Host}]";
                            Program.data.AddResolution(url.Host, oIp, source, Program.cfgCurrent.MaxRecursion, Program.cfgCurrent, true);
                            Program.LogThis(new Log(Log.ModuleType.IPBingSearch,
                                $"[BingWeb] Found domain {url.Host} in IP {oIp}", Log.LogType.medium));
                        }
                        catch { }
                    }
                };
                bingSearcher.SearcherLogEvent += delegate (object sender, EventsThreads.ThreadStringEventArgs e)
                {
                    Program.LogThis(new Log(Log.ModuleType.IPBingSearch, e.Message, Log.LogType.debug));
                };
                var strSearchString = $"ip:{oIp}";
                bingSearcher.CustomSearch(strSearchString).Wait();
                Invoke(new MethodInvoker(delegate
                {
                    var strMessage = $"{currentResults.Count} domains with IP {oIp} found in bing";
                    Program.LogThis(new Log(Log.ModuleType.IPBingSearch, strMessage, Log.LogType.low));
                    Program.ChangeStatus(message);
                }));
            }
            catch
            {
                Program.LogThis(new Log(Log.ModuleType.IPBingSearch, "Something went wrong while searching on Bing", Log.LogType.debug));
            }
            finally
            {
                ScannThread = null;
            }
        }

        /// <summary>
        /// Search Ip Shodan Async.
        /// </summary>
        /// <param name="oThreadSafeListIp"></param>
        public void SearchIpInShodanAsync(Object oThreadSafeListIp)
        {
            if (string.IsNullOrEmpty(Program.cfgCurrent.ShodanApiKey))
            {
                MessageBox.Show(@"Before searching with Shodan's API you must set up a Shodan API Key. You can do it in 'Options > General Config'");
                return;
            }
            if (ShodanRecognitionObject != null)
            {
                MessageBox.Show("Already searching in Shodan, please wait", Program.ProgramName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                List<IPAddress> lstIPs = (oThreadSafeListIp as List<IPAddress>);
                ShodanRecognitionObject = new ShodanSearcher(Program.cfgCurrent.ShodanApiKey);
                ShodanRecognitionObject.ItemsFoundEvent += ShodanDataFound;
                ShodanRecognitionObject.SearcherLogEvent += ShodanLog;
                ShodanRecognitionObject.CustomSearch(lstIPs).ContinueWith((a) =>
                {
                    Invoke(new MethodInvoker(delegate
                    {
                        string strMessage = String.Format("Finish Searching IPs in Shodan");
                        Program.LogThis(new Log(Log.ModuleType.ShodanSearch, strMessage, Log.LogType.debug));
                    }));

                    ShodanRecognitionObject = null;
                });

                Invoke(new MethodInvoker(delegate
                {
                    var strMessage = "Searching IPs in Shodan";
                    toolStripStatusLabelLeft.Text = strMessage;
                    Program.LogThis(new Log(Log.ModuleType.ShodanSearch, strMessage, Log.LogType.debug));
                }));
            }
        }

        #endregion

        #endregion

        #region Eventos del treeViewDomain, treeViewIP

        /// <summary>
        /// Search in node param by text param.
        /// </summary>
        /// <param name="tnc"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public int SearchTextInNodes(TreeNodeCollection tnc, string text)
        {
            for (var i = 0; i < tnc.Count; i++)
            {
                if (Functions.IsIP(text) && Functions.IsIP(tnc[i].Name))
                {
                    var ip1 = Functions.IPToInt(text);
                    var ip2 = Functions.IPToInt(tnc[i].Name);
                    if (ip1 < ip2)
                        return i;
                }
                else
                {
                    if (string.Compare(text, tnc[i].Text, StringComparison.OrdinalIgnoreCase) < 0)
                        return i;
                }
            }

            return tnc.Count;
        }

        private void treeViewDomain_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var strSource = string.Empty;


            int scrollPos = panelInformation.lvwInformation.TopItem != null ? panelInformation.lvwInformation.TopItem.Index : 0;
            LoadInformationGui();
            panelInformation.lvwInformation.BeginUpdate();
            panelInformation.lvwInformation.Items.Clear();
            panelInformation.lvwInformation.Groups.Clear();

            DomainsItem d = Program.data.GetDomain(e.Node.Text);
            if (d == null)
                panelInformation.splitPanel.Panel2Collapsed = true;

            if (d != null)
            {
                panelInformation.splitPanel.Panel2Collapsed = false;
                UpdateBottomPanel(d);

                panelInformation.lvwInformation.Groups.Add("Domain", "Domain - Source");
                var lvi = panelInformation.lvwInformation.Items.Add(e.Node.Text);
                lvi.Group = panelInformation.lvwInformation.Groups["Domain"];
                strSource = Program.data.GetDomainSource(e.Node.Text);
                lvi.SubItems.Add(strSource);
                lvi.Tag = strSource;

                foreach (var cdi in Program.data.computerDomains.Items.Where(cdi => cdi.Domain.Domain == d.Domain))
                {
                    if (!string.IsNullOrEmpty(cdi.Computer.localName))
                    {
                        panelInformation.lvwInformation.Groups.Add("localInfo", "Domain - Local info");
                        lvi = panelInformation.lvwInformation.Items.Add("Local name");
                        lvi.Group = panelInformation.lvwInformation.Groups["localInfo"];
                        lvi.SubItems.Add(cdi.Computer.localName);
                    }

                    if (cdi.Domain.os != OperatingSystem.OS.Unknown)
                    {
                        panelInformation.lvwInformation.Groups.Add("localInfo", "Domain - Local info");
                        lvi = panelInformation.lvwInformation.Items.Add("OS");
                        lvi.Group = panelInformation.lvwInformation.Groups["localInfo"];
                        lvi.SubItems.Add(cdi.Domain.os.ToString());
                    }

                    if (cdi.Computer.RemotePasswords.Items.Count > 0)
                    {
                        foreach (var t in cdi.Computer.RemotePasswords.Items)
                        {
                            panelInformation.lvwInformation.Groups.Add("Remote Control", "Remote Control");
                            lvi = panelInformation.lvwInformation.Items.Add("Remote Control");
                            lvi.Group = panelInformation.lvwInformation.Groups["Remote Control"];
                            lvi.SubItems.Add(t.Password);
                            lvi.Tag = t.Type;
                        }
                    }

                    if (cdi.Computer.RemoteFolders.Items.Count > 0)
                    {
                        foreach (var t in cdi.Computer.RemoteFolders.Items)
                        {
                            panelInformation.lvwInformation.Groups.Add("Remote Folders", "Remote Folders");
                            lvi = panelInformation.lvwInformation.Items.Add("Remote Folders");
                            lvi.Group = panelInformation.lvwInformation.Groups["Remote Folders"];
                            lvi.SubItems.Add(t.Path);
                            lvi.Tag = t.Path;
                        }
                    }

                    if (cdi.Computer.Software == null || cdi.Computer.Software.Items.Count <= 0) continue;

                    if (panelInformation.lvwInformation.Groups["Software"] == null)
                        panelInformation.lvwInformation.Groups.Add("Software", "Software");

                    foreach (var aplicacion in cdi.Computer.Software.Items.Where(aplicacion => panelInformation.lvwInformation.Items[aplicacion.Name] == null))
                    {
                        lvi = panelInformation.lvwInformation.Items.Add(aplicacion.Name, aplicacion.Name, 0);
                        lvi.SubItems.Add(aplicacion.Source);
                        lvi.Group = panelInformation.lvwInformation.Groups["Software"];
                    }
                }

                var listaIps = Program.data.GetResolutionIPs(e.Node.Text);

                panelInformation.lvwInformation.Groups.Add("IPs", "IP Addresses - Source");
                foreach (var ip in listaIps)
                {
                    lvi = panelInformation.lvwInformation.Items.Add(ip.Ip);
                    lvi.Group = panelInformation.lvwInformation.Groups["IPs"];
                    lvi.SubItems.Add(ip.Source);
                    lvi.Tag = ip.Source;
                }

                panelInformation.lvwInformation.Groups.Add("FingerPrintingHTTP", "FingerPrinting - HTTP");
                panelInformation.lvwInformation.Groups.Add("FingerPrintingSMTP", "FingerPrinting - SMTP");
                panelInformation.lvwInformation.Groups.Add("FingerPrintingFTP", "FingerPrinting - FTP");
                panelInformation.lvwInformation.Groups.Add("FingerPrintingDNS", "FingerPrinting - DNS");

                for (var fpI = 0; fpI < d.fingerPrinting.Count; fpI++)
                {
                    var fp = d.fingerPrinting[fpI];

                    if (fp is HTTP http)
                    {
                        if (panelInformation.lvwInformation.Groups["FingerPrintingHTTP"].Items.Cast<ListViewItem>().Any(item => item.Text == http.Host + ":" + http.Port))
                            continue;

                        lvi = panelInformation.lvwInformation.Items.Add(http.Host + ":" + http.Port);
                        lvi.Group = panelInformation.lvwInformation.Groups["FingerPrintingHTTP"];

                        lvi.SubItems.Add(http.Version);
                        lvi.Tag = http.Version;

                        if (string.IsNullOrEmpty(http.Title)) continue;

                        panelInformation.lvwInformation.Groups.Add("Http Title", "HTML Title");
                        lvi = panelInformation.lvwInformation.Items.Add(http.Host + ":" + http.Port);
                        lvi.Group = panelInformation.lvwInformation.Groups["Http Title"];

                        lvi.SubItems.Add(http.Title);
                        lvi.Tag = http.Title;
                    }
                    else if (fp is SMTP smtp)
                    {
                        lvi = panelInformation.lvwInformation.Items.Add(smtp.Host);
                        lvi.Group = panelInformation.lvwInformation.Groups["FingerPrintingSMTP"];

                        lvi.SubItems.Add(smtp.Version);
                        lvi.Tag = smtp.Version;
                    }
                    else if (fp is FTP ftp)
                    {
                        lvi = panelInformation.lvwInformation.Items.Add(ftp.Host);
                        lvi.Group = panelInformation.lvwInformation.Groups["FingerPrintingFTP"];

                        lvi.SubItems.Add(ftp.Version);
                        lvi.Tag = ftp.Version;
                    }
                    else if (fp is DNS dns)
                    {
                        lvi = panelInformation.lvwInformation.Items.Add(dns.Host);
                        lvi.Group = panelInformation.lvwInformation.Groups["FingerPrintingDNS"];

                        lvi.SubItems.Add(dns.Version);
                        lvi.Tag = dns.Version;
                    }
                }
            }
            panelInformation.lvwInformation.EndUpdate();

            if (panelInformation.lvwInformation.Items.Count > scrollPos)
                for (var i = 0; i < 5 && panelInformation.lvwInformation.TopItem.Index != scrollPos; i++)
                    panelInformation.lvwInformation.TopItem = panelInformation.lvwInformation.Items[scrollPos];

            if (d == null)
                return;
        }

        #endregion

        #region Eventos del panel inferior

        /// <summary>
        /// Return control with foco.
        /// </summary>
        /// <returns></returns>
        private Control GetFocusedControl()
        {
            var c = ActiveControl;

            while (!c.Focused)
            {
                if (!(c is IContainerControl))
                    return null;

                c = (c as IContainerControl).ActiveControl;
            }
            return c;
        }

        /// <summary>
        /// Update panel inferior.
        /// </summary>
        /// <param name="lstDomains"></param>
        public void UpdateBottomPanel(List<DomainsItem> lstDomains)
        {
            foreach (var t in lstDomains)
                UpdateBottomPanel(t);
        }

        /// <summary>
        /// Update borrom panel,
        /// </summary>
        /// <param name="domain"></param>
        public void UpdateBottomPanel(DomainsItem domain)
        {
            if (panelInformation.panelInformationOptions.domain != domain)
                panelInformation.panelInformationOptions.LoadDomain(domain);

            if ((from TabPage tab in panelInformation.tabMap.TabPages select (PanelUrlsList)tab.Controls[0]).Any(list => list.Domain != domain.Domain))
            {
                var controlActivo = GetFocusedControl();
                panelInformation.tabMap.TabPages.Clear();
                ActiveControl = controlActivo;
            }
            panelInformation.lbDomain.Text = "Domain: " + domain.Domain;
            CreateTabs(domain);
            UpdateUrLsTabs(domain);
        }

        private void UpdateUrLsTabs(DomainsItem domain)
        {
            if (ExistsTab("Files"))
            {
                var tab = panelInformation.tabMap.TabPages["Files"];
                tab.Text = "Files (" + domain.map.Files.Count + " found)";

                var list = (PanelUrlsList)tab.Controls[0];
                for (var i = list.lstView.Items.Count; i < domain.map.Files.Count; i++)
                {
                    list.lstView.Items.Add(domain.map.Files[i]).SubItems.Add(System.IO.Path.GetExtension(domain.map.Files[i]));
                }
            }

            if (ExistsTab("Folders"))
            {
                var tab = panelInformation.tabMap.TabPages["Folders"];
                tab.Text = "Folders (" + domain.map.Folders.Count + " found)";

                var list = (PanelUrlsList)tab.Controls[0];
                for (var i = list.lstView.Items.Count; i < domain.map.Folders.Count; i++)
                {
                    list.lstView.Items.Add(domain.map.Folders[i]);
                }
            }

            if (ExistsTab("Documents published"))
            {
                var tab = panelInformation.tabMap.TabPages["Documents published"];
                tab.Text = "Documents published (" + domain.map.Documents.Count + " found)";

                var list = (PanelUrlsList)tab.Controls[0];
                for (int i = list.lstView.Items.Count; i < domain.map.Documents.Count; i++)
                {
                    list.lstView.Items.Add(domain.map.Documents[i]);
                }
            }

            if (ExistsTab("Parameterized"))
            {
                var tab = panelInformation.tabMap.TabPages["Parameterized"];
                tab.Text = "Parameterized (" + domain.map.Parametrized.Count + " found)";

                var list = (PanelUrlsList)tab.Controls[0];
                for (var i = list.lstView.Items.Count; i < domain.map.Parametrized.Count; i++)
                {
                    list.lstView.Items.Add(domain.map.Parametrized[i]);
                }
            }

            for (var i = 0; i < domain.techAnalysis.SelectedTechnologies.Count; i++)
            {
                var tech = domain.techAnalysis.SelectedTechnologies[i];

                if (!ExistsTab("Tech. " + tech.extension)) continue;

                var tab = panelInformation.tabMap.TabPages["Tech. " + tech.extension];
                tab.Text = "Tech. " + tech.extension + " (" + tech.GetURLs().Count + " found)";
                var list = (PanelUrlsList)panelInformation.tabMap.TabPages["Tech. " + tech.extension].Controls[0];

                for (var iTech = list.lstView.Items.Count; iTech < tech.GetURLs().Count; iTech++)
                {
                    list.lstView.Items.Add(tech.GetURLs()[iTech]);
                }
            }
        }

        /// <summary>
        /// Create Tabs.
        /// </summary>
        /// <param name="domain"></param>
        private void CreateTabs(DomainsItem domain)
        {
            if (!ExistsTab("Files"))
            {
                var tab = CreateTab("Files");
                var list = new PanelUrlsList();
                list.Domain = domain.Domain;
                list.Dock = DockStyle.Fill;
                tab.Controls.Add(list);
                list.lstView.Columns.Add("File", "File").Width = 400;
                list.lstView.Columns.Add("Extension", "Extension").Width = 80;
            }

            if (!ExistsTab("Folders"))
            {
                var tab = CreateTab("Folders");
                var list = new PanelUrlsList();
                list.Domain = domain.Domain;
                list.Dock = DockStyle.Fill;
                tab.Controls.Add(list);
                list.lstView.Columns.Add("Folder", "Folder").Width = -2;
            }

            if (!ExistsTab("Documents published"))
            {
                var tab = CreateTab("Documents published");
                var list = new PanelUrlsList();
                list.Domain = domain.Domain;
                list.Dock = DockStyle.Fill;
                tab.Controls.Add(list);
                list.lstView.Columns.Add("Document", "Document").Width = -2;
            }

            if (!ExistsTab("Parameterized"))
            {
                var tab = CreateTab("Parameterized");
                var list = new PanelUrlsList();
                list.Domain = domain.Domain;
                list.Dock = DockStyle.Fill;
                tab.Controls.Add(list);
                list.lstView.Columns.Add("Document", "Document").Width = 480;
            }

            for (var i = 0; i < domain.techAnalysis.SelectedTechnologies.Count; i++)
            {
                var tech = domain.techAnalysis.SelectedTechnologies[i];

                if ((!ExistsTab("Tech. " + tech.extension) && tech.GetURLs().Count > 0))
                {
                    var tab = CreateTab("Tech. " + tech.extension);
                    var list = new PanelUrlsList();
                    list.Domain = domain.Domain;
                    list.Dock = DockStyle.Fill;
                    tab.Controls.Add(list);
                    list.lstView.Columns.Add("File", "File").Width = -2;
                }
            }
        }

        /// <summary>
        /// Validate if tabs exist.
        /// </summary>
        /// <param name="tabName"></param>
        /// <returns></returns>
        private bool ExistsTab(string tabName)
        {
            return panelInformation.tabMap.TabPages.Cast<TabPage>().Any(tab => tab.Name == tabName);
        }

        /// <summary>
        /// create tab.
        /// </summary>
        /// <param name="tabName"></param>
        /// <returns></returns>
        private TabPage CreateTab(string tabName)
        {
            panelInformation.tabMap.TabPages.Add(tabName, tabName);
            return panelInformation.tabMap.TabPages[tabName];
        }
        #endregion

        private void splitContainerMain_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, Color.DarkGray, ButtonBorderStyle.Dashed);
        }

        private void splitContainerMain_Resize(object sender, EventArgs e)
        {
            splitContainerMain.Refresh();
        }

        private async Task UpdateBackgroundAsync()
        {
            TimeSpan delay = TimeSpan.FromSeconds(2);
            UpdateGUI.Initialize();
            do
            {
                try
                {
                    UpdateGUI.UpdateTree(this.updateUITokenSource.Token);
                    await Task.Delay(delay, this.updateUITokenSource.Token);
                }
                catch
                {
                }
            } while (!this.updateUITokenSource.Token.IsCancellationRequested);
        }

        /// <summary>
        /// Informa del progreso para mostrarlo en la barra de tareas de windows 7
        /// </summary>
        /// <param name="current"></param>
        /// <param name="max"></param>
        public void ReportProgress(int current, int max)
        {
            if (TaskbarManager.IsPlatformSupported)
            {
                if (current == 0)
                {
                    _tm.SetProgressState(TaskbarProgressBarState.NoProgress);
                }
                else
                {
                    _tm.SetProgressState(TaskbarProgressBarState.Normal);
                    _tm.SetProgressValue(current, max);
                }
            }
        }

        private void taskListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadTasksGui();
        }

        private void TreeView_NodeMouseClick_1(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if ((sender as TreeView).SelectedNode == e.Node)
                    TreeView_AfterSelect(null, new TreeViewEventArgs(e.Node));
                else
                    (sender as TreeView).SelectedNode = e.Node;
            }
            else if (e.Button == MouseButtons.Left)
            {
                (sender as TreeView).SelectedNode = null;
                (sender as TreeView).SelectedNode = e.Node;
            }
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Name == GUI.Navigation.Project.Key)
            {
                if (Program.data.Project.Domain == null)
                    LoadProjectGui(true);
                else
                    LoadProjectGui(false);
            }
            else if (e.Node.Name == GUI.Navigation.Project.Network.Key)
            {
                LoadDnsEnumerationGui();
            }
            else if (e.Node.Name == GUI.Navigation.Project.DocumentAnalysis.Key)
            {
                LoadSearchGui();
            }

            TreeNode tn = null;
            TreeNode tnCat = null;
            tn = Program.FormMainInstance.TreeView.SelectedNode;
            tnCat = Program.FormMainInstance.TreeView.SelectedNode;

            while (tnCat.Parent != null)
            {
                if (tnCat.Parent.Name == GUI.Navigation.Project.Domains.Key)
                {
                    treeViewDomain_AfterSelect(null, new TreeViewEventArgs(tn));
                    return;
                }
                if (tnCat.Parent.Name == GUI.Navigation.Project.DocumentAnalysis.Key)
                {
                    TreeViewProjectAfterSelect(null, new TreeViewEventArgs(tn));
                    return;
                }
                if (tnCat.Parent.Name == GUI.Navigation.Project.Network.Key)
                {
                    if (tn.Tag is ComputerDomainsItem cdi)
                    {
                        var tnNew = new TreeNode();
                        tnNew.Tag = cdi.Domain;
                        tnNew.Text = tn.Text;
                        treeViewDomain_AfterSelect(null, new TreeViewEventArgs(tnNew));
                    }
                    else
                        treeViewNetwork_AfterSelect(null, new TreeViewEventArgs(tn));
                    return;
                }
                if (tnCat.Parent.Name == GUI.Navigation.Project.Key)
                {
                    TreeViewProjectAfterSelect(null, new TreeViewEventArgs(tn));
                    return;
                }

                tnCat = tnCat.Parent;
            }
        }

        private void contextMenu_Opening(object sender, CancelEventArgs e)
        {
            contextMenu.Items.Clear();

            TreeNode tn = null;
            TreeNode tnCat = null;
            var sourceControl = ((ContextMenuStrip)sender).SourceControl;
            if (sourceControl != TreeView)
                return;

            tn = Program.FormMainInstance.TreeView.SelectedNode;
            tnCat = Program.FormMainInstance.TreeView.SelectedNode;

            Contextual.Global(tn, sourceControl);

            if (tn != null)
            {
                if (tn.Name == GUI.Navigation.Project.Key)
                {
                    Contextual.ShowProjectMenu(tn, sourceControl);
                    return;
                }
                if (tn.Name == GUI.Navigation.Project.Domains.Key)
                {
                    Contextual.ShowDomainsMenu(tn, sourceControl);
                    return;
                }
                if (tn.Name == GUI.Navigation.Project.DocumentAnalysis.Key)
                {
                    Contextual.ShowMetadataMenu(tn, sourceControl);
                    return;
                }
                if (tn.Name == GUI.Navigation.Project.Network.Key)
                {
                    Contextual.ShowNetworkMenu(tn, sourceControl);
                    return;
                }
            }

            if (tnCat == null)
                return;

            while (tnCat.Parent != null)
            {
                if (tnCat.Parent.Name == GUI.Navigation.Project.Domains.Key)
                {
                    // General domains options
                    if (tn?.Tag is DomainsItem || (tn?.Text.Equals(Program.data.Project.Domain) == true))
                    {
                        Contextual.ShowDomainsDomainMenu(tn, sourceControl);
                    }

                    return;
                }
                else if (tnCat.Parent.Name == GUI.Navigation.Project.DocumentAnalysis.Key)
                {
                    // Metadata general options
                    e.Cancel = true;
                    return;
                }
                else if (tnCat.Parent.Name == GUI.Navigation.Project.Network.Key)
                {
                    // Pcservers general options
                    if (tn?.Name == GUI.Navigation.Project.Network.Clients.Key)
                    {
                        Contextual.ShowNetworkClientsMenu(tn, sourceControl);
                    }
                    else if (tn?.Name == GUI.Navigation.Project.Network.Servers.Key)
                    {
                        Contextual.ShowNetworkServersMenu(tn, sourceControl);
                    }
                    else if (tn?.Name == GUI.Navigation.Project.Network.Servers.Unknown.Key)
                    {
                        Contextual.ShowNetworkUnlocatedMenu(tn, sourceControl);
                    }
                    else
                    {
                        if (tn.Tag.ToString() == "iprange")
                            Contextual.ShowNetworkIpRangeMenu(tn, sourceControl);

                        else if ((tn.Tag is ComputersItem) && (tn.Parent.Name == "Clients"))
                            Contextual.ShowNetworkClientsItemMenu(tn, sourceControl);

                        else if ((tn.Tag is ComputersItem) && (tn.Parent.Name != "Unknown Servers") && ((tn.Parent.Parent.Parent.Name == "Servers") || (tn.Parent.Parent.Name == "Servers") || (tn.Parent.Parent.Parent.Parent.Name == "Servers")))
                            Contextual.ShowNetworkServersItemMenu(tn, sourceControl);

                        else if ((tn.Tag is ComputersItem) && (tn.Parent.Name == "Unknown Servers"))
                            Contextual.ShowNetworkUnlocatedItemMenu(tn, sourceControl);

                        else if ((tn.Tag is ComputerDomainsItem) && (tn.Parent.Tag is ComputersItem))
                        {
                            var newTn = new TreeNode
                            {
                                Tag = ((ComputerDomainsItem)tn.Tag).Domain,
                                Text = tn.Text
                            };
                            Contextual.ShowDomainsDomainItemMenu(newTn, sourceControl);
                        }
                    }

                    return;
                }
                else if (tnCat.Parent.Name == GUI.Navigation.Project.Key)
                {
                    e.Cancel = true;
                    return;
                }

                tnCat = tnCat.Parent;
            }
        }

        private void FormMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.P)
            {
                TreeView.SelectedNode = TreeView.GetNode(GUI.Navigation.Project.ToNavigationPath());
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.N)
            {
                TreeView.SelectedNode = TreeView.GetNode(GUI.Navigation.Project.Network.ToNavigationPath());
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.D)
            {
                TreeView.SelectedNode = TreeView.GetNode(GUI.Navigation.Project.Domains.ToNavigationPath()); ;
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.M)
            {
                TreeView.SelectedNode = TreeView.GetNode(GUI.Navigation.Project.DocumentAnalysis.ToNavigationPath()); ;
                e.Handled = true;
            }
        }

        private void loadUnloadPluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
#if PLUGINS
            FormPlugins pluginsForm = new FormPlugins();
            pluginsForm.ShowDialog();
#endif
        }

        private void dNSSnoopingToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            LoadDnsSnoopingGui();
        }

        private void toolStripMenuItemMarket_Click(object sender, EventArgs e)
        {
            var url = "https://focamarket.elevenpaths.com/";
            try
            {
                Process.Start(url);
            }
            catch (Exception)
            {
                MessageBox.Show($"Couldn't open URL. Please, go to {url}", "Couldn't launch browser",
                    MessageBoxButtons.OK);
            }
        }
    }
}
