using FOCA;
using FOCA.GUI;
using FOCA.ModifiedComponents;
using FOCA.Properties;
using FOCA.Search;
using FOCA.Searcher;
using FOCA.SubdomainSearcher;
using FOCA.Threads;
using MetadataExtractCore.Analysis;
using MetadataExtractCore.Diagrams;
using MetadataExtractCore.Metadata;
using MetadataExtractCore.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FOCA.Functions;

namespace FOCA
{
    public partial class PanelMetadataSearch : UserControl
    {
        private int activeDownloads;

        /// <summary>
        /// Threads used for download, extract and analyze the metadata
        /// </summary>
        public Thread CurrentDownloads, Metadata, Analysis, CurrentSearch;

        public int DownloadedFiles;
        public ThreadSafeList<Download> Downloads;
        private int enqueuedFiles;
        // Thread that runs in background and obtains size of files from given URLs
        public HTTPSizeDaemon HttpSizeDaemonInst;

        public PanelMetadataSearch()
        {
            InitializeComponent();
            // Accept all certificates so that there're no problems
            // with invalid certificates
            CookieAwareWebClient.AcceptAllCertificates();
            // Create pending downloads list
            Downloads = new ThreadSafeList<Download>();
            HttpSizeDaemonInst = new HTTPSizeDaemon();
        }

        /// <summary>
        ///     Handle event for found URLs
        /// </summary>
        /// <param name="o"></param>
        private void ProcessUrls(object o)
        {
            var lstUrLs = (List<object>)o;
            if (lstUrLs != null)
                HandleLinkFoundEvent(null, new EventsThreads.ThreadListDataFoundEventArgs(lstUrLs));
        }

        /// <summary>
        ///     Handle event for adding URLs from a file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addURLsFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofdURLList.ShowDialog() != DialogResult.OK) return;
            var lstUrLs = new List<object>(File.ReadAllLines(ofdURLList.FileName));

            var t = new Thread(ProcessUrls) { IsBackground = true };
            t.Start(lstUrLs);
        }

        /// <summary>
        ///     Search for all filetypes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbAll_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < checkedListBoxExtensions.Items.Count; i++)
                checkedListBoxExtensions.SetItemChecked(i, true);
        }

        /// <summary>
        ///     Do not select any filetype
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbNone_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < checkedListBoxExtensions.Items.Count; i++)
                checkedListBoxExtensions.SetItemChecked(i, false);
        }

        /// <summary>
        ///     Analyze metadata of documents
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void analyzeMetadataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var t = new Thread(AnalyzeMetadata)
            {
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal
            };
            t.Start();
        }

        public class Download
        {
            public enum Status
            {
                Enqueued,
                Downloading,
                Inprogress
            };

            public CookieAwareWebClient CaClient;
            public Status DownloadStatus;
            public string DownloadUrl;
            public ListViewItem Lvi;
            public ProgressBar Pbar;
            public string PhysicalPath;
            public int Retries;
        }

        #region contextMenuStrip events

        /// <summary>
        ///     Event used for enabling/disabling context menu items when they're displayed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStripLinks_Opening(object sender, CancelEventArgs e)
        {
            //Validate if the thread for Analysis is run.
            if (Program.FormMainInstance.panelMetadataSearch.Analysis != null &&
                Program.FormMainInstance.panelMetadataSearch.Analysis.IsAlive)
                contextMenuStripLinks.Items["analyzeMetadataToolStripMenuItem"].Enabled = false;
            else
                contextMenuStripLinks.Items["analyzeMetadataToolStripMenuItem"].Enabled = true;

            if (Program.FormMainInstance.programState == FormMain.ProgramState.ExtractingMetadata ||
                Program.FormMainInstance.programState == FormMain.ProgramState.Searching)
            {
                contextMenuStripLinks.Items["toolStripMenuItemDownload"].Enabled =
                    contextMenuStripLinks.Items["toolStripMenuItemDelete"].Enabled =
                        contextMenuStripLinks.Items["toolStripMenuItemDownloadAll"].Enabled =
                            contextMenuStripLinks.Items["toolStripMenuItemDeleteAll"].Enabled =
                                    contextMenuStripLinks.Items["toolStripMenuItemExtractAllMetadata"].Enabled =
                                        contextMenuStripLinks.Items["toolStripMenuItemAddFile"].Enabled =
                                            contextMenuStripLinks.Items["toolStripMenuItemAddFolder"].Enabled = false;
                return;
            }
            contextMenuStripLinks.Items["toolStripMenuItemDownload"].Enabled =
                contextMenuStripLinks.Items["toolStripMenuItemDelete"].Enabled =
                    contextMenuStripLinks.Items["toolStripMenuItemDownloadAll"].Enabled =
                        contextMenuStripLinks.Items["toolStripMenuItemDeleteAll"].Enabled =
                                contextMenuStripLinks.Items["toolStripMenuItemExtractAllMetadata"].Enabled =
                                    contextMenuStripLinks.Items["toolStripMenuItemAddFile"].Enabled =
                                        contextMenuStripLinks.Items["toolStripMenuItemAddFolder"].Enabled = true;
            var lv = (ListView)((ContextMenuStrip)sender).SourceControl;
            var itemsDownloading =
                (from ListViewItem lvi in lv.SelectedItems from d in Downloads where d.Lvi.Index == lvi.Index select lvi)
                    .Count();
            switch (itemsDownloading)
            {
                case 0:
                    contextMenuStripLinks.Items["toolStripMenuItemDownload"].Text = "&Download";
                    contextMenuStripLinks.Items["toolStripMenuItemDownload"].Image = Resources.page_white_go;
                    break;
                case 1:
                    contextMenuStripLinks.Items["toolStripMenuItemDownload"].Text = "&Stop Download";
                    contextMenuStripLinks.Items["toolStripMenuItemDownload"].Image = Resources.page_white_go_stop;
                    break;
                default: // > 1
                    contextMenuStripLinks.Items["toolStripMenuItemDownload"].Text = "&Stop Downloads";
                    contextMenuStripLinks.Items["toolStripMenuItemDownload"].Image = Resources.page_white_go_stop;
                    break;
            }

            //If there're downloads and the menu "Stop All Downloads" hasn't been added yet, add menu
            if (Downloads.Count > 0 && contextMenuStripLinks.Items[2] is ToolStripSeparator)
            {
                ToolStripItem tsiStopDownloads = new ToolStripMenuItem("&Stop All Downloads");
                tsiStopDownloads.Image = Resources.page_white_stack_go_stop;
                tsiStopDownloads.Click +=
                    delegate
                    {
                        for (var i = Downloads.Count - 1; i >= 0; i--)
                        {
                            var d = Downloads[i];
                            if (d.CaClient != null)
                                d.CaClient.CancelAsync();
                            else
                            {
                                DownloadedFiles++;
                                Downloads.RemoveAt(i);
                            }
                        }
                    };
                contextMenuStripLinks.Items.Insert(2, tsiStopDownloads);
            }
            //If there aren't donwloads but the menu is alredy added, delete menu
            else if (Downloads.Count == 0 && !(contextMenuStripLinks.Items[2] is ToolStripSeparator))
            {
                contextMenuStripLinks.Items.RemoveAt(2);
            }
            contextMenuStripLinks.Items["toolStripMenuItemDownload"].Enabled =
                contextMenuStripLinks.Items["toolStripMenuItemDelete"].Enabled =
                    lv.SelectedItems != null && lv.SelectedItems.Count != 0;
            contextMenuStripLinks.Items["toolStripMenuItemDownloadAll"].Enabled =
                contextMenuStripLinks.Items["toolStripMenuItemDeleteAll"].Enabled = lv.Items.Count > 0;
            var someFileDownloaded = false;
            if (lv.SelectedItems != null)
            {
                if (
                    (from ListViewItem lvi in lv.SelectedItems select (FilesITem)lvi.Tag).Any(
                        fi => fi != null && fi.Downloaded))
                {
                    someFileDownloaded = true;
                }
            }
            // Look for an item already downloaded but unselected
            if (!someFileDownloaded)
            {
                if (
                    (from ListViewItem lvi in lv.Items select (FilesITem)lvi.Tag).Any(
                        fi => fi != null && fi.Downloaded))
                {
                    someFileDownloaded = true;
                }
            }
            contextMenuStripLinks.Items["toolStripMenuItemExtractAllMetadata"].Enabled = someFileDownloaded;
            contextMenuStripLinks.Items["linkToolStripMenuItem"].Enabled = lv.SelectedItems != null &&
                                                                           lv.SelectedItems.Count != 0;
            contextMenuStripLinks.Items["linkToolStripMenuItem"].Text = lv.SelectedItems != null &&
                                                                        lv.SelectedItems.Count > 1
                ? "&Links"
                : "&Link";
        }

        /// <summary>
        ///     Handle file download button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemDownload_Click(object sender, EventArgs e)
        {
            var toolStripDropDownItem = sender as ToolStripDropDownItem;
            if (toolStripDropDownItem != null && toolStripDropDownItem.Text == "&Download")
            {
                var directory = Program.data.Project.FolderToDownload;
                if (!directory.EndsWith("\\"))
                    directory += "\\";
                var urls =
                    Program.FormMainInstance.panelMetadataSearch.listViewDocuments.SelectedItems.Cast<ListViewItem>()
                        .ToList();
                DownloadFiles(urls, directory);
            }
            else
            {
                var stripDropDownItem = sender as ToolStripDropDownItem;
                if (stripDropDownItem == null || !stripDropDownItem.Text.StartsWith("&Stop Download")) return;
                // Stop all selected items
                foreach (
                    ListViewItem lvi in Program.FormMainInstance.panelMetadataSearch.listViewDocuments.SelectedItems)
                {
                    for (var i = 0; i < Downloads.Count; i++)
                    {
                        var d = Downloads[i];
                        if (d.Lvi.Index != lvi.Index) continue;
                        if (d.CaClient != null)
                            d.CaClient.CancelAsync();
                        else
                        {
                            DownloadedFiles++;
                            Downloads.Remove(d);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Handle file download all files button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void toolStripMenuItemDownloadAll_Click(object sender, EventArgs e)
        {
            var directory = Program.data.Project.FolderToDownload;
            if (!directory.EndsWith("\\"))
                directory += "\\";
            var urls =
                Program.FormMainInstance.panelMetadataSearch.listViewDocuments.Items.Cast<ListViewItem>().ToList();
            DownloadFiles(urls, directory);
        }

        /// <summary>
        ///     Handle file deletion from documents list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in Program.FormMainInstance.panelMetadataSearch.listViewDocuments.SelectedItems)
            {
                var fi = (FilesITem)lvi.Tag;
                if (fi != null)
                {
                    Program.data.files.Items.Remove(fi);
                    RemoveFileFromTreeNode(fi);
                    Program.LogThis(new Log(Log.ModuleType.MetadataSearch,
                        $"Removed document: {fi.URL}", Log.LogType.debug));
                }
                lvi.Remove();
            }
            Program.FormMainInstance.treeViewMetadata_UpdateDocumentsNumber();
        }

        /// <summary>
        ///     Handle all files deletion from documents list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemDeleteAll_Click(object sender, EventArgs e)
        {
            foreach (
                var fi in from ListViewItem lvi in Program.FormMainInstance.panelMetadataSearch.listViewDocuments.Items
                          select (FilesITem)lvi.Tag
                    into fi
                          where fi != null
                          select fi)
            {
                RemoveFileFromTreeNode(fi);
                Program.LogThis(new Log(Log.ModuleType.MetadataSearch,
                    $"Removed document: {fi.URL}", Log.LogType.debug));
            }
            Program.data.files.Items.Clear();
            Program.FormMainInstance.panelMetadataSearch.listViewDocuments.Items.Clear();
            Program.FormMainInstance.treeViewMetadata_UpdateDocumentsNumber();
        }

        /// <summary>
        ///     Remove file from files tree
        /// </summary>
        /// <param name="fi"></param>
        private void RemoveFileFromTreeNode(FilesITem fi)
        {
            if (fi != null && fi.Processed && Program.FormMainInstance.TreeViewMetadataSearchDocument(fi.Path) != null)
                Program.FormMainInstance.TreeViewMetadataSearchDocument(fi.Path).Remove();
        }


        /// <summary>
        ///     Handle extrect metadata from all files button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void toolStripMenuItemExtractAllMetadata_Click(object sender, EventArgs e)
        {
            if (Metadata != null && Metadata.IsAlive)
            {
                MessageBox.Show(@"Already extracting metadata", @"Please wait", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                var listlvi =
                    Program.FormMainInstance.panelMetadataSearch.listViewDocuments.SelectedItems.Cast<ListViewItem>()
                        .ToList();
                Metadata = new Thread(ExtractMetadata)
                {
                    // avoid CPU overload
                    Priority = ThreadPriority.Lowest,
                    IsBackground = true
                };
                Metadata.Start(listlvi);
            }
        }

        /// <summary>
        ///     Handle add local file to documents tree action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemAddFile_Click(object sender, EventArgs e)
        {
            if (ofdAddFile.ShowDialog() == DialogResult.OK)
                AddFile(ofdAddFile.FileName);
        }

        /// <summary>
        ///     Handle add all files from a local folder to documents tree action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemAddFolder_Click(object sender, EventArgs e)
        {
            if (fbdMain.ShowDialog() != DialogResult.OK) return;
            var files = Directory.GetFiles(fbdMain.SelectedPath);
            var lvcs = (ListViewColumnSorter)listViewDocuments.ListViewItemSorter;
            listViewDocuments.ListViewItemSorter = null;
            foreach (var file in files)
                AddFile(file);
            listViewDocuments.ListViewItemSorter = lvcs;
        }

        /// <summary>
        ///     Event called to show selected URLs in browser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemOpenInBrowser_Click(object sender, EventArgs e)
        {
            foreach (
                var lvi in
                    listViewDocuments.SelectedItems.Cast<ListViewItem>()
                        .Where(lvi => Uri.IsWellFormedUriString(lvi.SubItems[2].Text, UriKind.Absolute)))
            {
                Process.Start(lvi.SubItems[2].Text);
                Program.LogThis(new Log(Log.ModuleType.MetadataSearch,
                    $"Opening document {lvi.SubItems[2].Text}", Log.LogType.debug));
            }
        }

        /// <summary>
        ///     Event called to copy selected items into clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemCopyToClipboard_Click(object sender, EventArgs e)
        {
            var links = string.Empty;
            for (var i = 0; i < listViewDocuments.SelectedItems.Count; i++)
            {
                //Add newlines
                links += i != 0 ? Environment.NewLine : string.Empty;
                links += listViewDocuments.SelectedItems[i].SubItems[2].Text;
            }
            Clipboard.SetText(links);
            Program.LogThis(new Log(Log.ModuleType.MetadataSearch,
                $"Copied to clipboard {listViewDocuments.SelectedItems.Count} URL documents",
                Log.LogType.debug));
        }

        /// <summary>
        ///     Add file from path to the project
        ///     A file locally added is tagged with the "Downloaded" attribute as true
        /// </summary>
        /// <param name="path"></param>
        public void AddFile(string path)
        {
            var s = Path.GetExtension(path);
            if (s == null) return;
            var extension = s.ToLower();
            // verify that it's a well-known extension, if not, guess filetype
            var knownExtension = Program.FormMainInstance.AstrSuportedExtensions.Any(ext => "." + ext == s.ToLower());
            if (!knownExtension)
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    extension = MetadataExtractCore.Utilities.Functions.GetFormatFile(fs);
                }
                if (!string.IsNullOrEmpty(extension))
                {
                    Program.LogThis(new Log(Log.ModuleType.MetadataSearch,
                        $"Unknow extension {path}, found filetype {extension}", Log.LogType.medium));
                    var newFile = GetNotExistsPath(path + "." + extension);
                    File.Move(path, newFile);
                    Program.LogThis(new Log(Log.ModuleType.MetadataSearch,
                        $"File moved {path} to {newFile}", Log.LogType.medium));
                    path = newFile;
                }
            }
            var fi = new FilesITem
            {
                Ext = extension,
                URL = path,
                Date = DateTime.Now,
                Size = (int)new FileInfo(path).Length,
                Downloaded = true,
                Processed = false,
                Path = path
            };
            Program.data.files.Items.Add(fi);
            listViewDocuments_Update(fi);
            Program.FormMainInstance.treeViewMetadata_UpdateDocumentsNumber();

            Program.LogThis(new Log(Log.ModuleType.MetadataSearch, $"Added document {fi.Path}",
                Log.LogType.debug));
        }

        #endregion

        #region listViewDocuments events

        /// <summary>
        ///     Represents a document or link into the ListViewDocuments and creates a new item for it
        /// </summary>
        /// <param name="fi"></param>
        public ListViewItem listViewDocuments_Update(FilesITem fi)
        {
            // search for the corresponding listViewDocument
            ListViewItem lviCurrent = null;
            try
            {
                listViewDocuments.Invoke(new MethodInvoker(() =>
                {
                    ListViewItem.ListViewSubItem lviExtension;
                    ListViewItem.ListViewSubItem lviUrl;
                    ListViewItem.ListViewSubItem lviDownloaded;
                    ListViewItem.ListViewSubItem lviDownloadedDate;
                    ListViewItem.ListViewSubItem lviSize;
                    ListViewItem.ListViewSubItem lviAnalyzed;
                    ListViewItem.ListViewSubItem lviModifedDate;

                    foreach (
                        var lvi in
                            listViewDocuments.Items.Cast<ListViewItem>().Where(lvi => (FilesITem)lvi.Tag == fi))
                    {
                        lviCurrent = lvi;
                        break;
                    }

                    // new item
                    if (lviCurrent == null)
                    {
                        lviCurrent = listViewDocuments.Items.Add(listViewDocuments.Items.Count.ToString());
                        lviCurrent.Tag = fi;
                        lviCurrent.UseItemStyleForSubItems = false;
                        lviCurrent.ImageIndex = Program.FormMainInstance.GetImageToExtension(fi.Ext);
                        lviExtension = lviCurrent.SubItems.Add(string.Empty);
                        lviUrl = lviCurrent.SubItems.Add(string.Empty);
                        lviDownloaded = lviCurrent.SubItems.Add(string.Empty);
                        lviDownloadedDate = lviCurrent.SubItems.Add(string.Empty);
                        lviSize = lviCurrent.SubItems.Add(string.Empty);
                        lviAnalyzed = lviCurrent.SubItems.Add(string.Empty);
                        lviModifedDate = lviCurrent.SubItems.Add(string.Empty);
                    }
                    // already existing item
                    else
                    {
                        lviExtension = lviCurrent.SubItems[1];
                        lviUrl = lviCurrent.SubItems[2];
                        lviDownloaded = lviCurrent.SubItems[3];
                        lviDownloadedDate = lviCurrent.SubItems[4];
                        lviSize = lviCurrent.SubItems[5];
                        lviAnalyzed = lviCurrent.SubItems[6];
                        lviModifedDate = lviCurrent.SubItems[7];
                    }
                    // represent the item into the ListView
                    var extension = fi.Ext;
                    lviExtension.Text = extension.Length > 0 ? extension.Substring(1, extension.Length - 1) : extension;
                    lviUrl.Text = fi.URL;

                    lviDownloaded.Font = new Font(Font.FontFamily, 14);
                    lviDownloaded.Text = fi.Downloaded ? "•" : "×";
                    lviDownloaded.ForeColor = fi.Downloaded ? Color.Green : Color.Red;
                    lviDownloadedDate.Text = fi.Date == DateTime.MinValue
                        ? "-"
                        : fi.Date.ToString(CultureInfo.InvariantCulture);
                    lviSize.Text = fi.Downloaded ? GetFileSizeAsString(fi.Size) : "-";

                    lviAnalyzed.Font = new Font(Font.FontFamily, 14);
                    lviAnalyzed.Text = fi.Processed ? "•" : "×";
                    lviAnalyzed.ForeColor = fi.Processed ? Color.Green : Color.Red;
                    lviModifedDate.Text = fi.ModifiedDate == DateTime.MinValue
                        ? "-"
                        : fi.ModifiedDate.ToString(CultureInfo.InvariantCulture);
                }));
            }
            catch (Exception ex)
            {
                Program.LogThis(new Log(Log.ModuleType.MetadataSearch,
                    $"Error filling ListViewItemDocuments: {ex.Message}", Log.LogType.error));
            }
            return lviCurrent;
        }

        /// <summary>
        ///     Handle documents list view order change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewDocuments_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var lvwColumnSorter = (ListViewColumnSorter)listViewDocuments.Tag;
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                lvwColumnSorter.Order = lvwColumnSorter.Order == SortOrder.Ascending
                    ? SortOrder.Descending
                    : SortOrder.Ascending;
            }
            else
            {
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }
            listViewDocuments.Sort();
        }

        /// <summary>
        ///     Hadle key down action when placed in the documents list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewDocuments_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 46)
                toolStripMenuItemDelete_Click(null, null);
        }

        #endregion

        #region Form components events

        /// <summary>
        ///     Handle custom search click. Make text visible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelCustomSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panelCustomSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Visible = true;
            linkLabelCustomSearch.Visible = false;
            btnSearch.Visible = true;
        }

        /// <summary>
        ///     Handle any key press action when placed in the documents list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
                btnSearch_Click(btnSearch, null);
        }

        /// <summary>
        ///     Handle search button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Text == "&Stop")
            {
                CurrentSearch?.Abort();
            }
            else if (!chkGoogle.Checked && !chkBing.Checked)
            {
                MessageBox.Show(@"Select a search engine, please.", @"Select a search engine", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else if (CurrentSearch != null)
            {
                MessageBox.Show(@"Already searching, please wait", @"Please, wait", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                CurrentSearch = new Thread(CustomSearch) { IsBackground = true };
                CurrentSearch.Start(txtSearch.Text);
            }
        }

        /// <summary>
        ///     Handle search using all available engines button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                if (button != null && button.Text == "&Stop")
                {
                    if (CurrentSearch == null) return;
                    CurrentSearch.Abort();
                    Program.LogThis(new Log(Log.ModuleType.MetadataSearch, "Stopping documents search",
                        Log.LogType.debug));
                }
                else if (!chkGoogle.Checked && !chkBing.Checked && !chkDuck.Checked)
                {
                    MessageBox.Show(@"Select a search engine please.", @"Select a search engine", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else if (CurrentSearch != null)
                {
                    MessageBox.Show(@"Already searching, please wait", @"Please, wait", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    if (checkedListBoxExtensions.CheckedIndices.Count == 0)
                        MessageBox.Show(@"Select at least one extension please.", @"Select an extension",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    else
                    {
                        CurrentSearch = new Thread(SearchAll) { IsBackground = true };
                        CurrentSearch.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Metadata functions

        /// <summary>
        ///     Extract metadasta from file
        /// </summary>
        /// <param name="o">List which contains the documents from which the tool is going to extract metadata information</param>
        public void ExtractMetadata(object o)
        {
            var itemsTree =
                Program.FormMainInstance.TreeView.Nodes[GUI.UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[
                    GUI.UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"];

            if (Program.data.Project.Id == 0)
            {
                itemsTree.Nodes["Users"].Tag = new List<UserItem>();
                itemsTree.Nodes["Printers"].Tag = new List<PrintersItem>();
                itemsTree.Nodes["Folders"].Tag = new List<PathsItem>();
                itemsTree.Nodes["Software"].Tag = new List<ApplicationsItem>();
                itemsTree.Nodes["Emails"].Tag = new List<EmailsItem>();
                itemsTree.Nodes["Operating Systems"].Tag = new List<string>();
                itemsTree.Nodes["Passwords"].Tag = new List<PasswordsItem>();
                itemsTree.Nodes["Servers"].Tag = new List<ServersItem>();
            }

            var users = (List<UserItem>)itemsTree.Nodes["Users"].Tag;
            var printers = (List<PrintersItem>)itemsTree.Nodes["Printers"].Tag;
            var folders = (List<PathsItem>)itemsTree.Nodes["Folders"].Tag;
            var software = (List<ApplicationsItem>)itemsTree.Nodes["Software"].Tag;
            var emails = (List<EmailsItem>)itemsTree.Nodes["Emails"].Tag;
            var operatingsystems = (List<string>)itemsTree.Nodes["Operating Systems"].Tag;
            var passwords = (List<PasswordsItem>)itemsTree.Nodes["Passwords"].Tag;
            var servers = (List<ServersItem>)itemsTree.Nodes["Servers"].Tag;

            try
            {
                var listlvi = o as List<ListViewItem>;
                if (listlvi == null)
                {
                    return;
                }
                else
                {
                    Program.LogThis(new Log(Log.ModuleType.MetadataSearch,
                        $"Starting metadata extraction of {listlvi.Count} documents", Log.LogType.debug));
                    Invoke(new MethodInvoker(() =>
                    {
                        // Disable interface elements while metadata search is running
                        btnSearch.Enabled = false;
                        btnSearchAll.Enabled = false;
                        Program.FormMainInstance.programState = FormMain.ProgramState.ExtractingMetadata;
                        Program.FormMainInstance.SetItemsMenu(null, null);
                        Program.FormMainInstance.toolStripDropDownButtonStop.Enabled = true;
                    }));
                    var extractedFiles = 0; // counter
                    var po = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
                    Parallel.For(0, listlvi.Count, po, delegate (int i)
                    {
                        // Refresh DownloadStatus with the number of documents being analyzed and the current progress
                        var files = extractedFiles;
                        Invoke(new MethodInvoker(delegate
                        {
                            Program.FormMainInstance.toolStripProgressBarDownload.Value = files * 100 /
                                                                                          listlvi.Count;
                            Program.FormMainInstance.toolStripStatusLabelLeft.Text = @"Extracting metadata from " +
                                                                                     files + @"/" +
                                                                                     listlvi.Count +
                                                                                     @" documents";
                            Program.FormMainInstance.ReportProgress(files, listlvi.Count);
                        }));
                        var lvi = listlvi[i];
                        var fi = (FilesITem)lvi.Tag;
                        // Extract metadata only if the file has been downloaded and it is not empty
                        if (fi != null && fi.Downloaded && fi.Size != 0)
                        {
                            TreeNode tnFile = null;
                            Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(delegate
                            {
                                tnFile = Program.FormMainInstance.TreeViewMetadataAddDocument(fi.Path);
                                // delete all it's subnodes in case that the document already existed
                                tnFile.Nodes.Clear();
                            }));
                            // Processed == true --> document's metadata have been already extracted
                            fi.Processed = true;
                            tnFile.Tag = fi;
                            MetaExtractor doc = null;
                            var s = Path.GetExtension(fi.Path);
                            if (s != null)
                            {
                                var extension = s.ToLower();
                                try
                                {
                                    switch (extension)
                                    {
                                        case ".sxw":
                                        case ".odt":
                                        case ".ods":
                                        case ".odg":
                                        case ".odp":
                                            // fi.Path fi.Path contains the file's full path
                                            using (var fs = new FileStream(fi.Path, FileMode.Open, FileAccess.Read))
                                            {
                                                doc = new OpenOfficeDocument(fs, extension);
                                            }
                                            break;
                                        case ".docx":
                                        case ".xlsx":
                                        case ".pptx":
                                        case ".ppsx":
                                            using (
                                                var fs = new FileStream(fi.Path, FileMode.Open, FileAccess.Read,
                                                    FileShare.ReadWrite))
                                            {
                                                doc = new OfficeOpenXMLDocument(fs, extension);
                                            }
                                            break;
                                        case ".doc":
                                        case ".xls":
                                        case ".ppt":
                                        case ".pps":
                                            using (
                                                var fs = new FileStream(fi.Path, FileMode.Open, FileAccess.Read,
                                                    FileShare.ReadWrite))
                                            {
                                                doc = new Office972003(fs);
                                            }
                                            break;
                                        case ".pdf":
                                            using (
                                                var fs = new FileStream(fi.Path, FileMode.Open, FileAccess.Read,
                                                    FileShare.ReadWrite))
                                            {
                                                doc = new PDFDocument(fs);
                                            }
                                            break;
                                        case ".wpd":
                                            using (
                                                var fs = new FileStream(fi.Path, FileMode.Open, FileAccess.Read,
                                                    FileShare.ReadWrite))
                                            {
                                                doc = new WPDDocument(fs);
                                            }
                                            break;
                                        case ".raw":
                                        case ".cr2":
                                        case ".crw":
                                        case ".jpg":
                                        case ".jpeg":
                                            using (
                                                var fs = new FileStream(fi.Path, FileMode.Open, FileAccess.Read,
                                                    FileShare.ReadWrite))
                                            {
                                                doc = new EXIFDocument(fs, extension);
                                            }
                                            break;
                                        case ".svg":
                                        case ".svgz":
                                            using (
                                                var fs = new FileStream(fi.Path, FileMode.Open, FileAccess.Read,
                                                    FileShare.ReadWrite))
                                            {
                                                doc = new SVGDocument(fs);
                                            }
                                            break;
                                        case ".indd":
                                            using (
                                                var fs = new FileStream(fi.Path, FileMode.Open, FileAccess.Read,
                                                    FileShare.ReadWrite))
                                            {
                                                doc = new InDDDocument(fs);
                                            }
                                            break;
                                        case ".rdp":
                                            using (
                                                var fs = new FileStream(fi.Path, FileMode.Open, FileAccess.Read,
                                                    FileShare.ReadWrite))
                                            {
                                                doc = new RDPDocument(fs);
                                            }
                                            break;
                                        case ".ica":
                                            using (
                                                var fs = new FileStream(fi.Path, FileMode.Open, FileAccess.Read,
                                                    FileShare.ReadWrite))
                                            {
                                                doc = new ICADocument(fs);
                                            }
                                            break;
                                    }
                                    Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(delegate
                                    {
                                        tnFile.ImageIndex =
                                            tnFile.SelectedImageIndex =
                                                Program.FormMainInstance.GetImageToExtension(extension);
                                    }));
                                }
                                catch
                                {
                                    doc = null;
                                }
                            }
                            // if any filetype has been assigned to doc, just continue
                            if (doc != null)
                            {
                                // Analyze file and extract metadata
                                doc.analyzeFile();
                                // close the document
                                doc.Close();
                                // fi.Metadata contains the file's metadata so that other parts can also use them
                                fi.Metadata = doc;

                                Program.LogThis(new Log(Log.ModuleType.MetadataSearch,
                                    $"Document metadata extracted: {fi.Path}", Log.LogType.low));

                                // Extract primary metadata from the document
                                Program.FormMainInstance.TreeView.Invoke(
                                    new MethodInvoker(
                                        delegate
                                        {
                                            ExtractGenericMetadata(doc, ref users, ref passwords, ref servers,
                                                ref folders, ref printers, ref emails, ref software, ref operatingsystems);
                                        }));
                                // Add needed notes to show document's metadata
                                AddDocumentNodes(doc, tnFile);
                                // if any date has been found, use it for the 'Last modified' field into the ListView
                                if (doc.FoundDates.ModificationDateSpecified)
                                    fi.ModifiedDate = doc.FoundDates.ModificationDate;
                                else if (doc.FoundDates.CreationDateSpecified)
                                    fi.ModifiedDate = doc.FoundDates.CreationDate;
                                // if there're no old versions, just existing ones in OpenOffice, extract the metadata summary from them
                                var document = doc as OpenOfficeDocument;
                                if (document != null)
                                {
                                    var dicOldVersions = document.dicOldVersions;
                                    foreach (var oldVersion in dicOldVersions)
                                    {
                                        // Add every version's information to the summary
                                        ExtractGenericMetadata(oldVersion.Value, ref users, ref passwords,
                                            ref servers,
                                            ref folders, ref printers, ref emails, ref software,
                                            ref operatingsystems);
                                    }
                                }
                            }
                            Invoke(new MethodInvoker(delegate
                            {
                                listViewDocuments_Update(fi);
                                Program.FormMainInstance.treeViewMetadata_UpdateDocumentsNumber();
                            }));
                        }
                        Interlocked.Increment(ref extractedFiles);
                    }
                        );
                }
                Invoke(new MethodInvoker(delegate
                {
                    const string strMessage = "All documents were analyzed";
                    Program.LogThis(new Log(Log.ModuleType.MetadataSearch, strMessage, Log.LogType.debug));
                    Program.FormMainInstance.ChangeStatus(strMessage);

                    Program.FormMainInstance.TreeView.Nodes[UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[
                        UpdateGUI.TreeViewKeys.KMetadata.ToString()].Expand();
                    Program.FormMainInstance.TreeView.Nodes[UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[
                        UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Expand();
                }));
            }
            catch (ThreadAbortException)
            {
                Invoke(new MethodInvoker(delegate
                {
                    const string strMessage = "Metadata extraction was aborted!";
                    Program.LogThis(new Log(Log.ModuleType.MetadataSearch, strMessage, Log.LogType.debug));
                    Program.FormMainInstance.ChangeStatus(strMessage);
                }));
            }
            finally
            {
                Invoke(new MethodInvoker(delegate
                {
                    Program.FormMainInstance.ReportProgress(0, 100);
                    Program.FormMainInstance.toolStripProgressBarDownload.Value = 0;
                    Program.FormMainInstance.toolStripDropDownButtonStop.Enabled = false;

                    // update text from nodes which show metadata summary with the total values found number
                    Program.FormMainInstance.TreeView.Nodes[UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[
                        UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Users"].Text =
                        $"Users ({users.Count})";
                    Program.FormMainInstance.TreeView.Nodes[UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[
                        UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Printers"].Text =
                        $"Printers ({printers.Count})";
                    Program.FormMainInstance.TreeView.Nodes[UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[
                        UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Folders"].Text =
                        $"Folders ({folders.Count})";
                    Program.FormMainInstance.TreeView.Nodes[UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[
                        UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Software"].Text =
                        $"Software ({software.Count})";
                    Program.FormMainInstance.TreeView.Nodes[UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[
                        UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Emails"].Text =
                        $"Emails ({emails.Count})";

                    if (emails.Count != 0)
                    {
                        List<string> emailsValue = emails.Select(x => x.Mail).ToList();
                        PluginsAPI.SharedValues.FocaEmails = emailsValue;
                    }
                    else
                        PluginsAPI.SharedValues.FocaEmails = new List<string>();


                    Program.FormMainInstance.TreeView.Nodes[UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[
                        UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Operating Systems"
                        ].Text = $"Operating Systems ({operatingsystems.Count})";
                    Program.FormMainInstance.TreeView.Nodes[UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[
                        UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Passwords"].Text =
                        $"Passwords ({passwords.Count})";
                    Program.FormMainInstance.TreeView.Nodes[UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[
                        UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Servers"].Text =
                        $"Servers ({servers.Count})";
                    // enable interface elements which were disabled previously
                    btnSearchAll.Enabled = Program.data.Project.ProjectState != Project.ProjectStates.Uninitialized;
                    Program.FormMainInstance.programState = FormMain.ProgramState.Normal;
                    Metadata = null;
                }));
            }
        }

        /// <summary>
        ///     Obtain primary metadata from a document to elaborate a summary
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="users"></param>
        /// <param name="servers"></param>
        /// <param name="folders"></param>
        /// <param name="printers"></param>
        /// <param name="emails"></param>
        /// <param name="passwords"></param>
        /// <param name="software"></param>
        /// <param name="operatingsystems"></param>
        internal void ExtractGenericMetadata(MetaExtractor doc, ref List<UserItem> users,
            ref List<PasswordsItem> passwords, ref List<ServersItem> servers,
            ref List<PathsItem> folders, ref List<PrintersItem> printers,
            ref List<EmailsItem> emails, ref List<ApplicationsItem> software,
            ref List<string> operatingsystems)
        {
            var s = doc.FoundServers;
            foreach (var si in s.Items.Where(si => si.Name.Trim().Length > 1))
                if (servers.Count(x => x.Name == si.Name.Trim()) == 0)
                    servers.Add(si);
            //else
            //    servers[si.Name.Trim()]++;

            var u = doc.FoundUsers;
            foreach (var ui in u.Items.Where(ui => ui.Name.Trim().Length > 1))
                if (users.Count(x => x.Name == ui.Name.Trim()) == 0)
                    users.Add(ui);

            var p = doc.FoundPasswords;
            foreach (var pi in p.Items.Where(pi => pi.Password.Trim().Length > 1))
                if (passwords.Count(x => x.Password == pi.Password.Trim()) == 0)
                    passwords.Add(pi);
            //else
            //    passwords[pi.Password.Trim()]++;

            var r = doc.FoundPaths;
            foreach (var ri in r.Items.Where(ri => ri.Path.Trim().Length > 1))
                if (folders.Count(x => x.Path == ri.Path.Trim()) == 0)
                    folders.Add(ri);
            //else
            //    folders[ri.Path.Trim()]++;

            var im = doc.FoundPrinters;
            foreach (var ii in im.Items.Where(ii => ii.Printer.Trim().Length > 1))
                if (printers.Count(x => x.Printer == ii.Printer.Trim()) == 0)
                    printers.Add(ii);
            //else
            //    printers[ii.Printer.Trim()]++;

            var e = doc.FoundEmails;
            foreach (var ei in e.Items.Where(ei => ei.Mail.Trim().Length > 1))
                if (emails.Count(x => x.Mail == ei.Mail.Trim()) == 0)
                    emails.Add(ei);
            //else
            //    emails[ei.Mail.Trim()]++;

            foreach (
                var strSoftware in
                    doc.FoundMetaData.Applications.Items.Select(aplicacion => aplicacion.Name)
                        .Where(strSoftware => strSoftware.Trim().Length > 1))
            {
                if (software.Count(x => x.Name == strSoftware.Trim()) == 0)
                    software.Add(new ApplicationsItem(strSoftware));
            }

            var strOs = doc.FoundMetaData.OperativeSystem;
            if (string.IsNullOrEmpty(strOs)) return;
            strOs = strOs.Trim();
            if (operatingsystems.Count(i => i == strOs) == 0)
                operatingsystems.Add(strOs);
        }

        /// <summary>
        ///     Add needed nodes to treeViewMetadata to show document's metadata
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="trnParentNode">Parent node to which information will be added</param>
        internal void AddDocumentNodes(MetaExtractor doc, TreeNode trnParentNode)
        {
            if (doc.FoundUsers.Items.Count != 0)
            {
                Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(() =>
                {
                    var tnUsers = trnParentNode.Nodes.Add("Users", "Users");
                    tnUsers.ImageIndex =
                        tnUsers.SelectedImageIndex = 14;
                    tnUsers.Tag = doc.FoundUsers;
                }));
            }
            if (doc.FoundPasswords.Items.Count != 0)
            {
                Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(() =>
                {
                    var tnPasswords = trnParentNode.Nodes.Add("Passwords", "Passwords");
                    tnPasswords.ImageIndex =
                        tnPasswords.SelectedImageIndex = 75;
                    tnPasswords.Tag = doc.FoundPasswords;
                }));
            }
            if (doc.FoundServers.Items.Count != 0)
            {
                Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(() =>
                {
                    var tnServers = trnParentNode.Nodes.Add("Servers", "Servers");
                    tnServers.ImageIndex =
                        tnServers.SelectedImageIndex = 45;
                    tnServers.Tag = doc.FoundServers;
                }));
            }
            if (doc.FoundPaths.Items.Count != 0)
            {
                Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(() =>
                {
                    var tnFolders = trnParentNode.Nodes.Add("Folders", "Folders");
                    tnFolders.ImageIndex =
                        tnFolders.SelectedImageIndex = 13;
                    tnFolders.Tag = doc.FoundPaths;
                }));
            }
            if (doc.FoundPrinters.Items.Count != 0)
            {
                Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(() =>
                {
                    var tnPrinters = trnParentNode.Nodes.Add("Printers", "Printers");
                    tnPrinters.ImageIndex =
                        tnPrinters.SelectedImageIndex = 15;
                    tnPrinters.Tag = doc.FoundPrinters;
                }));
            }
            if (doc.FoundEmails.Items.Count != 0)
            {
                Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(() =>
                {
                    var tnEmails = trnParentNode.Nodes.Add("Emails", "Emails");
                    tnEmails.ImageIndex =
                        tnEmails.SelectedImageIndex = 23;
                    tnEmails.Tag = doc.FoundEmails;
                }));
            }
            if (doc.FoundDates.CreationDateSpecified || doc.FoundDates.DatePrintingSpecified ||
                doc.FoundDates.ModificationDateSpecified)
            {
                Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(() =>
                {
                    var tnDates = trnParentNode.Nodes.Add("Dates", "Dates");
                    tnDates.ImageIndex =
                        tnDates.SelectedImageIndex = 24;
                    tnDates.Tag = doc.FoundDates;
                }));
            }
            var m = doc.FoundMetaData;
            // if any metadata as found
            if ((m.Applications != null && m.Applications.Items.Count > 0) ||
                !string.IsNullOrEmpty(m.Subject) ||
                !string.IsNullOrEmpty(m.DataBase) ||
                !string.IsNullOrEmpty(m.Category) ||
                !string.IsNullOrEmpty(m.Codification) ||
                !string.IsNullOrEmpty(m.Comments) ||
                !string.IsNullOrEmpty(m.Company) ||
                !string.IsNullOrEmpty(m.Description) ||
                !string.IsNullOrEmpty(m.Statistic) ||
                !string.IsNullOrEmpty(m.Language) ||
                !string.IsNullOrEmpty(m.UserInfo) ||
                m.VersionNumber != 0 ||
                !string.IsNullOrEmpty(m.Keywords) ||
                !string.IsNullOrEmpty(m.Template) ||
                !string.IsNullOrEmpty(m.OperativeSystem) ||
                m.EditTime != 0 ||
                !string.IsNullOrEmpty(m.Title) ||
                !string.IsNullOrEmpty(m.Model))
            {
                Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(() =>
                {
                    var tnMetadata = trnParentNode.Nodes.Add("Other Metadata", "Other Metadata");
                    tnMetadata.ImageIndex =
                        tnMetadata.SelectedImageIndex = 25;
                    tnMetadata.Tag = doc.FoundMetaData;
                }));
            }
            if (doc.FoundOldVersions.Items.Count != 0)
            {
                Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(() =>
                {
                    var tnOld = trnParentNode.Nodes.Add("Old versions", "Old versions");
                    tnOld.ImageIndex =
                        tnOld.SelectedImageIndex = 26;
                    tnOld.Tag = doc.FoundOldVersions;
                }));
            }
            if (doc.FoundHistory.Items.Count != 0)
            {
                Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(() =>
                {
                    var tnOld = trnParentNode.Nodes.Add("History", "History");
                    tnOld.ImageIndex =
                        tnOld.SelectedImageIndex = 31;
                    tnOld.Tag = doc.FoundHistory;
                }));
            }
            if (doc.FoundMetaData.Applications.Items.Count > 0)
            {
                Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(() =>
                {
                    var tnSoftware = trnParentNode.Nodes.Add("Software", "Software");
                    tnSoftware.ImageIndex =
                        tnSoftware.SelectedImageIndex = 30;
                    tnSoftware.Tag = doc.FoundMetaData.Applications;
                }));
            }
            var document = doc as EXIFDocument;
            if (document != null)
            {
                if (document.dicAnotherMetadata.Count != 0 || document.Thumbnail != null)
                {
                    Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(() =>
                    {
                        var tnExif = trnParentNode.Nodes.Add("EXIF", "EXIF");
                        tnExif.ImageIndex = tnExif.SelectedImageIndex = 33;
                        tnExif.Tag = doc;
                    }));
                }
            }
            // extract EXIF information from embeded images
            var office972004 = doc as Office972003;
            var openOfficeDocument = doc as OpenOfficeDocument;
            var xmlDocument = doc as OfficeOpenXMLDocument;
            if (office972004 != null ||
                openOfficeDocument != null ||
                xmlDocument != null)
            {
                SerializableDictionary<string, EXIFDocument> dicPictureExif = null;
                var office972003 = doc as Office972003;
                if (office972003 != null)
                    dicPictureExif = office972003.dicPictureEXIF;
                var officeDocument = doc as OpenOfficeDocument;
                if (officeDocument != null)
                    dicPictureExif = officeDocument.dicPictureEXIF;
                var openXmlDocument = doc as OfficeOpenXMLDocument;
                if (openXmlDocument != null)
                    dicPictureExif = openXmlDocument.dicPictureEXIF;
                if (dicPictureExif.Count != 0)
                {
                    Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(delegate
                    {
                        // there's at least one image with EXIF information
                        var bPictureExif =
                            dicPictureExif.Any(
                                dicPicture =>
                                    dicPicture.Value.dicAnotherMetadata.Count != 0 || dicPicture.Value.Thumbnail != null);
                        if (!bPictureExif) return;
                        var tnExifRoot = trnParentNode.Nodes.Add("EXIF in pictures", "EXIF in pictures");
                        tnExifRoot.ImageIndex =
                            tnExifRoot.SelectedImageIndex = 32;
                        foreach (var dicPicture in dicPictureExif)
                        {
                            if (dicPicture.Value.dicAnotherMetadata.Count == 0 && dicPicture.Value.Thumbnail == null)
                                continue;
                            var tnExifPic = tnExifRoot.Nodes.Add(Path.GetFileName(dicPicture.Key),
                                Path.GetFileName(dicPicture.Key));
                            tnExifPic.ImageIndex =
                                tnExifPic.SelectedImageIndex = 34;
                            var tnExif = tnExifPic.Nodes.Add("EXIF", "EXIF");
                            tnExif.ImageIndex =
                                tnExif.SelectedImageIndex = 33;
                            tnExif.Tag = dicPicture.Value;
                        }
                    }));
                }
            }
            // extract old versions from OpenOffice documents
            if (!(doc is OpenOfficeDocument)) return;
            var dicOldVersions = ((OpenOfficeDocument)doc).dicOldVersions;
            if (dicOldVersions.Count != 0)
            {
                Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(() =>
                {
                    var tnOldVersionsRoot = trnParentNode.Nodes["Old versions"];
                    foreach (var oldVersion in dicOldVersions)
                    {
                        var tnOldVersion = tnOldVersionsRoot.Nodes.Add(oldVersion.Key, oldVersion.Key);
                        tnOldVersion.ImageIndex =
                            tnOldVersion.SelectedImageIndex = tnOldVersion.Parent.Parent.SelectedImageIndex;
                        AddDocumentNodes(oldVersion.Value, tnOldVersion);
                    }
                }));
            }
        }

        #endregion

        #region Analyze Metadata Functions

        /// <summary>
        ///     Analyze metadata of the current project files
        /// </summary>
        public void AnalyzeMetadata()
        {
            if (
                Program.FormMainInstance.TreeView.Nodes[UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[
                    UpdateGUI.TreeViewKeys.KPCServers.ToString()] == null)
            {
                MessageBox.Show(@"You need a project before analyzing metadata", "", MessageBoxButtons.OK);
                return;
            }

            try
            {
                Invoke(new MethodInvoker(() =>
                {
                    Program.FormMainInstance.toolStripStatusLabelLeft.Text = @"Identifying computers...";
                    Program.FormMainInstance.toolStripDropDownButtonStop.Enabled = true;
                }));

                GetPCsAndServers();
                if (Program.data.computers.Items.Any(c => c.type == ComputersItem.Tipo.ClientPC))
                {
                    // group by username match
                    GroupClientNodes();
                }
                IdentifyOsSoftware();
                Invoke(
                    new MethodInvoker(
                        () => Program.FormMainInstance.toolStripStatusLabelLeft.Text = @"Metadata analyzed"));
            }
            catch (ThreadAbortException)
            {
                Invoke(
                    new MethodInvoker(
                        () => { Program.FormMainInstance.toolStripStatusLabelLeft.Text = @"Metadata analysis aborted"; }));
            }
            finally
            {
                Invoke(new MethodInvoker(() =>
                {
                    Program.FormMainInstance.toolStripDropDownButtonStop.Enabled = false;
                    Program.FormMainInstance.TreeView.Nodes[UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[
                        UpdateGUI.TreeViewKeys.KPCServers.ToString()].Expand();
                    Program.FormMainInstance.TreeView.Nodes[UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[
                        UpdateGUI.TreeViewKeys.KPCServers.ToString()].Nodes["Clients"].Expand();
                    Program.FormMainInstance.TreeView.Nodes[UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[
                        UpdateGUI.TreeViewKeys.KPCServers.ToString()].Nodes["Servers"].Expand();
                    Program.FormMainInstance.TreeView.Nodes[UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[
                        UpdateGUI.TreeViewKeys.KPCServers.ToString()].Nodes["Servers"].Nodes["Unlocated Servers"].Expand
                        ();
                    Analysis = null;
                }));
            }
        }

        /// <summary>
        ///     Identify operating system and software installed in each machine
        /// </summary>
        private void IdentifyOsSoftware()
        {
            foreach (var ci in Program.data.computers.Items)
            {
                var so = ci.os;
                if (so == OperatingSystem.OS.Unknown && !ci.NotOS)
                {
                    if (ci.type == ComputersItem.Tipo.ClientPC)
                    {
                        var strSourceDocument = ci.SourceDocuments[0];
                        var fi = Program.data.files.Items.First(f => f.URL == strSourceDocument);
                        if (fi?.Metadata?.FoundMetaData != null)
                        {
                            var m = fi.Metadata.FoundMetaData;
                            if (m.OperativeSystem != null)
                            {
                                so = OperatingSystemUtils.StringToOS(m.OperativeSystem);
                            }
                            if ((so == OperatingSystem.OS.Unknown) && m.Applications.Items.Count > 0)
                            {
                                var os = OperatingSystemUtils.SoftwareToOS(m.Applications);
                                if (os != OperatingSystem.OS.Unknown)
                                    so = os;
                            }
                        }
                    }
                }
                if (so == OperatingSystem.OS.Windows || so == OperatingSystem.OS.Unknown)
                {
                    // if X:\users\ is found --> it's vista or higher
                    // if X:\Documents and Settings\ --> XP or lower
                    // if X:\ --> Windows
                    if (ci.Folders.Items.Count > 0)
                    {
                        int hitsWindows = 0, hitsWindowsXp = 0, hitsWindowsVista = 0, hitsWindowsNt4 = 0;
                        foreach (
                            var ri in
                                ci.Folders.Items.Where(ri => ri.IsComputerFolder)
                                    .Where(ri => char.IsLetter(ri.Path[0]) && ri.Path[1] == ':' && ri.Path[2] == '\\'))
                        {
                            hitsWindows++;
                            if (ri.Path.ToLower().IndexOf("documents and settings", StringComparison.Ordinal) == 3 ||
                                ri.Path.ToLower().IndexOf("dokumente und einstellungen", StringComparison.Ordinal) == 3)
                                hitsWindowsXp++;
                            else if (ri.Path.ToLower().IndexOf("users", StringComparison.Ordinal) == 3 &&
                                     ri.Path.ToLower().IndexOf("usuarios", StringComparison.Ordinal) == 3)
                                hitsWindowsVista++;
                            else if (ri.Path.ToLower().IndexOf("winnt", StringComparison.Ordinal) == 3)
                                hitsWindowsNt4++;
                        }
                        if (hitsWindowsNt4 > 0 && hitsWindowsNt4 > hitsWindowsXp && hitsWindowsNt4 > hitsWindowsVista)
                            so = OperatingSystem.OS.WindowsNT40;
                        else if (hitsWindowsXp > 0 && hitsWindowsXp > hitsWindowsVista && hitsWindowsXp >= hitsWindowsNt4)
                            so = OperatingSystem.OS.WindowsXP;
                        else if (hitsWindowsVista > 0 && hitsWindowsVista >= hitsWindowsXp &&
                                 hitsWindowsVista >= hitsWindowsNt4)
                            so = OperatingSystem.OS.WindowsVista;
                        else if (hitsWindows > 0)
                            so = OperatingSystem.OS.Windows;
                    }
                }
                ci.os = so;

                #region identify installed software

                if (ci.NotOS || ci.type != ComputersItem.Tipo.ClientPC) continue;
                foreach (var document in ci.SourceDocuments)
                {
                    var strSourceDocument = document;
                    FilesITem fi = Program.data.files.Items.FirstOrDefault(f => f.URL == strSourceDocument);
                    if (fi?.Metadata?.FoundMetaData == null || fi.Metadata.FoundMetaData.Applications.Items.Count <= 0)
                        continue;
                    foreach (var aplicacion in from aplicacion in fi.Metadata.FoundMetaData.Applications.Items
                                               let strSoftware = aplicacion.Name
                                               where !ci.Software.Items.Any(a => a.Name == strSoftware)
                                               select aplicacion)
                    {
                        ci.Software.Items.Add(aplicacion);
                    }
                }

                #endregion
            }
        }

        /// <summary>
        ///     Analyze all documents and create machines with users and paths,
        ///     also servers each time a shared path is found
        /// </summary>
        private void GetPCsAndServers()
        {
            var unIdentifiedComputer = 0;
            // for each document, a new machine is generated
            foreach (var fi in Program.data.files.Items)
            {
                // only use those files where metadata was found
                if (fi.Metadata == null) continue;
                var machineUsers = new List<UserItem>(fi.Metadata.FoundUsers.Items.Where(u => u.IsComputerUser));
                var machinePaths = new List<PathsItem>(fi.Metadata.FoundPaths.Items.Where(r => r.IsComputerFolder));
                var servers = new List<ServersItem>(fi.Metadata.FoundServers.Items);
                var passwords = new List<PasswordsItem>(fi.Metadata.FoundPasswords.Items);

                if (servers.Count > 0 || machineUsers.Count > 0 || machinePaths.Count > 0 ||
                    fi.Metadata.FoundPrinters.Items.Count > 0)
                {
                    var ciClient = new ComputersItem();
                    // add the machine to current data
                    Program.data.computers.Items.Add(ciClient);
                    ciClient.type = ComputersItem.Tipo.ClientPC;
                    ciClient.SourceDocuments.Add(fi.URL);
                    ciClient.SourceDocuments.Add(fi.Path);
                    ciClient.os = OperatingSystem.OS.Unknown;
                    if (machineUsers.Count > 0)
                    {
                        foreach (var t in machineUsers)
                        {
                            ciClient.Users.Items.Add(t);
                        }
                        var strNodeName = $"PC_{ciClient.Users.Items[0].Name}";
                        try
                        {
                            foreach (var c in Program.data.computers.Items)
                            {
                                if (c.name != null)
                                {
                                    if (string.Equals(c.name, strNodeName, StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        strNodeName = $"PC_Unknown{++unIdentifiedComputer}";
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        ciClient.name = strNodeName;
                    }
                    else // no username known, use a generic one
                    {
                        var strNodeName = "PC_Unknown0";
                        if (
                            Program.data.computers.Items.Any(
                                c => string.Equals(c.name, strNodeName, StringComparison.CurrentCultureIgnoreCase)))
                            strNodeName = $"PC_Unknown{++unIdentifiedComputer}";
                        ciClient.name = strNodeName;
                    }

                    #region identify servers in the 'servers' module of metadata (example: rdp metadata)

                    if (servers.Count > 0)
                    {
                        foreach (var si in servers)
                        {
                            ComputersItem ciServer;
                            // if an existing server was found, add the documents from where it was extracted
                            if (
                                Program.data.computers.Items.Any(
                                    c => string.Equals(c.name, si.Name, StringComparison.CurrentCultureIgnoreCase)))
                            {
                                var si1 = si;
                                ciServer =
                                    Program.data.computers.Items.First(
                                        c => string.Equals(c.name, si1.Name, StringComparison.CurrentCultureIgnoreCase));
                                if (!ciServer.SourceDocuments.Contains(fi.URL))
                                    ciServer.SourceDocuments.Add(fi.URL);
                            }
                            else // if it doesn't exist, create it and add the information
                            {
                                ciServer = new ComputersItem();
                                Program.data.computers.Items.Add(ciServer);
                                ciServer.name = si.Name;
                                ciServer.type = ComputersItem.Tipo.Server;
                                ciServer.NameChangedEvent += NameChangedFunction;
                                ciServer.SourceDocuments.Add(fi.URL);
                                ciServer.os = OperatingSystem.OS.Unknown;
                                ResolveHost(si.Name, ref ciServer);
                                try
                                {
                                    var ip = Dns.GetHostEntry(si.Name).AddressList[0].ToString();
                                    Program.data.AddIP(ip, "Metadata extraction", Program.cfgCurrent.MaxRecursion);
                                    Program.data.computerIPs.Items.Add(new ComputerIPsItem(ciServer,
                                        Program.data.GetIp(ip), "Metadata extraction"));

                                    Program.data.AddResolution(si.Name, ip, @"Metadata extraction",
                                        Program.cfgCurrent.MaxRecursion, Program.cfgCurrent, true);
                                }
                                catch
                                {
                                }
                                if (machineUsers.Count > 0)
                                {
                                    foreach (var ui in machineUsers)
                                        ciServer.RemoteUsers.AddUniqueItem(ui.Name, false);
                                }
                                if (passwords.Count <= 0) continue;
                                foreach (var pi in passwords)
                                    ciServer.RemotePasswords.AddUniqueItem(pi);
                            }
                        }
                    }

                    #endregion

                    #region Identify servers with shared routes

                    // client folders, example: C:\Documents and settings\
                    foreach (var ri in machinePaths)
                    {
                        string server;
                        // it isn't a local path, it's a shared folder route, so, let's add a server
                        if (IsServerPath(ri.Path, out server))
                        {
                            // add it only if the 'server' variable matches the current domain or it's alternative domains
                            var found = false;
                            if (Program.data.Project.Domain != null)
                            {
                                if (Program.data.Project.AlternativeDomains.Any(y => y.Contains(server)) ||
                                    server.Contains(Program.data.Project.Domain))
                                    found = true;
                            }

                            IPAddress ipAddress;
                            // if it's an IP address, it must be added to the list
                            if (IPAddress.TryParse(server, out ipAddress))
                                found = true;
                            else
                            {
                                // if it's a hostname which doesn't resolve to an IP, add it to the list
                                try
                                {
                                    // here an internet connection is needed because if there's no path to the internet,
                                    // when the DNS request for a host is done, it will return that it doesn't have IP address
                                    if (Dns.GetHostAddresses(server).Length == 0)
                                        found = true;
                                }
                                catch
                                {
                                    found = true;
                                }
                            }

                            if (!found)
                                continue;

                            ComputersItem ciServer;
                            // if an already existing server is found, add the documents
                            if (
                                Program.data.computers.Items.Any(
                                    c => string.Equals(c.name, server, StringComparison.CurrentCultureIgnoreCase)))
                            {
                                ciServer =
                                    Program.data.computers.Items.First(
                                        c => string.Equals(c.name, server, StringComparison.CurrentCultureIgnoreCase));
                                if (!ciServer.SourceDocuments.Contains(fi.URL))
                                    ciServer.SourceDocuments.Add(fi.URL);
                            }
                            else
                            {
                                ciServer = new ComputersItem();
                                Program.data.computers.Items.Add(ciServer);
                                ciServer.type = ComputersItem.Tipo.Server;
                                ciServer.NameChangedEvent += NameChangedFunction;
                                ciServer.SourceDocuments.Add(fi.URL);
                                ciServer.os = OperatingSystem.OS.Unknown;
                                if (ri.Path.StartsWith("\\\\"))
                                    ciServer.os = OperatingSystem.OS.Windows;
                                ResolveHost(server, ref ciServer);

                                try
                                {
                                    var ip = Dns.GetHostEntry(server).AddressList[0].ToString();
                                    Program.data.AddIP(ip, "Metadata extraction", Program.cfgCurrent.MaxRecursion);
                                    Program.data.computerIPs.Items.Add(new ComputerIPsItem(ciServer,
                                        Program.data.GetIp(ip), "Metadata extraction"));
                                    Program.data.AddResolution(server, ip, "Metadata extraction",
                                        Program.cfgCurrent.MaxRecursion, Program.cfgCurrent, true);
                                }
                                catch
                                {
                                }
                            }
                            // if the server's name is shorter than the original route, it means that it was a shared folder, example: \\Server\C\Windows\
                            if (ri.Path.Length > server.Length)
                            {
                                if (ri.Path.IndexOf(server, StringComparison.Ordinal) > 0 &&
                                    ri.Path.IndexOf(server, StringComparison.Ordinal) + server.Length + 1 <
                                    ri.Path.Length)
                                {
                                    var folder =
                                        ri.Path.Substring(ri.Path.IndexOf(server, StringComparison.Ordinal) +
                                                          server.Length);
                                    var rii = ciServer.Folders.AddUniqueItem(folder, true);
                                    // add remote users for this folder those who are users from the machine which had access to it
                                    if (rii != null)
                                    {
                                        foreach (var ui in machineUsers)
                                        {
                                            rii.RemoteUsers.AddUniqueItem(ui.Name, false);
                                        }
                                    }
                                }
                            }
                            // add to the server user's who have access to it
                            if (machineUsers.Count > 0)
                            {
                                foreach (var ui in machineUsers)
                                {
                                    ciServer.RemoteUsers.AddUniqueItem(ui.Name, false);
                                }
                            }

                            // add passwords to the server
                            if (passwords.Count > 0)
                            {
                                foreach (var pi in passwords)
                                {
                                    ciServer.RemotePasswords.AddUniqueItem(pi);
                                }
                            }

                            // Add to the user the remote folder to which he has access
                            ciClient.RemoteFolders.AddUniqueItem(ri.Path, false);
                        }
                        else
                        {
                            ciClient.Folders.AddUniqueItem(ri.Path, ri.IsComputerFolder);
                        }
                    }

                    #endregion

                    #region Identify severs with shared printers

                    // iterate over printers looking for servers
                    if (fi.Metadata.FoundPrinters.Items.Count > 0)
                    {
                        foreach (
                            var p in fi.Metadata.FoundPrinters.Items.Where(ii => !string.IsNullOrEmpty(ii.Printer)))
                        {
                            string server;
                            // it isn't a local printer, it's a printer in a shared server, so let's add a new server
                            if (IsServerPath(p.Printer, out server))
                            {
                                ComputersItem ciServer;
                                if (
                                    Program.data.computers.Items.Any(
                                        c => string.Equals(c.name, server, StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    // if an already existing server is found, add different printers
                                    ciServer =
                                        Program.data.computers.Items.First(
                                            c =>
                                                string.Equals(c.name, server, StringComparison.CurrentCultureIgnoreCase));
                                    if (!ciServer.SourceDocuments.Contains(fi.URL))
                                        ciServer.SourceDocuments.Add(fi.URL);
                                }
                                else // new server node
                                {
                                    ciServer = new ComputersItem();
                                    Program.data.computers.Items.Add(ciServer);
                                    ciServer.type = ComputersItem.Tipo.Server;
                                    ciServer.os = OperatingSystem.OS.Unknown;
                                    ciServer.SourceDocuments.Add(fi.URL);
                                    ResolveHost(server, ref ciServer);
                                }
                                if (p.Printer.Length > server.Length)
                                {
                                    var printerName =
                                        p.Printer.Substring(p.Printer.IndexOf(server, StringComparison.Ordinal) +
                                                            server.Length);
                                    var iitem = ciServer.Printers.AddUniqueItem(printerName.Trim());
                                    // add printer's remote users
                                    if (iitem != null)
                                    {
                                        foreach (var ui in machineUsers)
                                        {
                                            iitem.RemoteUsers.AddUniqueItem(ui.Name, false);
                                        }
                                    }
                                }
                                // add to the servers the users who have access to it
                                if (machineUsers.Count > 0)
                                {
                                    foreach (var ui in machineUsers)
                                    {
                                        ciServer.RemoteUsers.AddUniqueItem(ui.Name, false);
                                    }
                                }
                                // add the remote printer to the user
                                ciClient.RemotePrinters.AddUniqueItem(p.Printer);
                            }
                            else
                            {
                                ciClient.Printers.AddUniqueItem(p.Printer);
                            }
                        }
                    }

                    #endregion
                }

                // iterate over the history to add new machines and folders to the existing user's machines
                if (fi.Metadata.FoundHistory.Items.Count <= 0) continue;
                foreach (var hi in fi.Metadata.FoundHistory.Items)
                {
                    hi.Author = hi.Author?.Trim() ?? string.Empty;
                    hi.Path = hi.Path?.Trim() ?? string.Empty;
                    if (string.IsNullOrEmpty(hi.Path)) continue;
                    string server;
                    // it's a folder from a shared path so we add a new server
                    if (IsServerPath(hi.Path, out server))
                    {
                        ComputersItem ciServer;
                        if (
                            Program.data.computers.Items.Any(
                                c => string.Equals(c.name, server, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            ciServer =
                                Program.data.computers.Items.First(
                                    c => string.Equals(c.name, server, StringComparison.CurrentCultureIgnoreCase));
                            if (!ciServer.SourceDocuments.Contains(fi.URL))
                                ciServer.SourceDocuments.Add(fi.URL);
                        }
                        else
                        {
                            ciServer = new ComputersItem();
                            Program.data.computers.Items.Add(ciServer);
                            ciServer.type = ComputersItem.Tipo.Server;
                            ciServer.os = OperatingSystem.OS.Windows;
                            ciServer.SourceDocuments.Add(fi.URL);
                            ResolveHost(server, ref ciServer);
                        }
                        // if the found server's length is greater than the original route, that means that there's an external route like \\Server\\route
                        if (hi.Path.Length > server.Length)
                        {
                            // if the server isn't at the beggining or at the end, it means that it's a valid route
                            if (hi.Path.IndexOf(server, StringComparison.Ordinal) > 0 &&
                                hi.Path.IndexOf(server, StringComparison.Ordinal) + server.Length + 1 < hi.Path.Length)
                            {
                                var folder =
                                    hi.Path.Substring(hi.Path.IndexOf(server, StringComparison.Ordinal) + server.Length);
                                // The router is from the server itself
                                var ri = ciServer.Folders.AddUniqueItem(folder, true);
                                if (ri != null && Users.IsValidUser(hi.Author))
                                {
                                    ri.RemoteUsers.AddUniqueItem(hi.Author, false);
                                }
                            }
                        }
                        if (!Users.IsValidUser(hi.Author)) continue;
                        // it isn't an user from the remote host
                        ciServer.RemoteUsers.AddUniqueItem(hi.Author, false);

                        var ciClient = new ComputersItem();
                        Program.data.computers.Items.Add(ciClient);
                        ciClient.type = ComputersItem.Tipo.ClientPC;
                        ciClient.SourceDocuments.Add(fi.URL);
                        ciClient.os = OperatingSystem.OS.Unknown;
                        ciClient.NotOS = true;
                        ciClient.Users.AddUniqueItem(hi.Author, true);
                        var strNodeName = $"PC_{hi.Author}";
                        if (Program.data.computers.Items.Any(c => c.name.ToLower() == strNodeName.ToLower()))
                            strNodeName = $"PC_Unknown{++unIdentifiedComputer}";
                        ciClient.name = strNodeName;
                        ciClient.RemoteFolders.AddUniqueItem(hi.Path, false);
                    }
                    else
                    {
                        if (!Users.IsValidUser(hi.Author) || string.IsNullOrEmpty(hi.Path) ||
                            !PathAnalysis.IsValidPath(hi.Path.Trim())) continue;
                        // if it isn't a server (\\Server\) but it has an author, add it as personal computer
                        var ciClient = new ComputersItem();
                        Program.data.computers.Items.Add(ciClient);
                        ciClient.type = ComputersItem.Tipo.ClientPC;
                        ciClient.SourceDocuments.Add(fi.URL);
                        ciClient.os = OperatingSystem.OS.Unknown;
                        ciClient.NotOS = true;

                        var strNodeName = $"PC_{hi.Author}";
                        if (
                            Program.data.computers.Items.Any(
                                c => string.Equals(c.name, strNodeName, StringComparison.CurrentCultureIgnoreCase)))
                            strNodeName = $"PC_Unknown{++unIdentifiedComputer}";
                        ciClient.name = strNodeName;
                        ciClient.Users.AddUniqueItem(hi.Author, true);
                        // Search for users into the path
                        var strUser = PathAnalysis.ExtractUserFromPath(hi.Path);
                        if (!string.IsNullOrEmpty(strUser))
                            ciClient.Users.AddUniqueItem(strUser, true);
                        ciClient.Folders.AddUniqueItem(hi.Path, true);
                    }
                }
            }
        }

        /// <summary>
        ///     Group client nodes matching by username
        /// </summary>
        private void GroupClientNodes()
        {
            var clientComputers =
                new List<ComputersItem>(Program.data.computers.Items.Where(c => c.type == ComputersItem.Tipo.ClientPC));
            var index = 0;
            var ciCurrent = clientComputers[index];
            while (true)
            {
                var match = false;
                var index1 = index;
                Invoke(new MethodInvoker(() =>
                {
                    if (
                        Program.FormMainInstance.toolStripStatusLabelLeft.Text.StartsWith(
                            "Grouping computers from node " + index1))
                    {
                        if (Program.FormMainInstance.toolStripStatusLabelLeft.Text.Length <
                            ("Grouping computers from node " + index1).Length + 3)
                            Program.FormMainInstance.toolStripStatusLabelLeft.Text += @".";
                        else
                            Program.FormMainInstance.toolStripStatusLabelLeft.Text = @"Grouping computers from node " +
                                                                                     index1;
                    }
                    else
                        Program.FormMainInstance.toolStripStatusLabelLeft.Text = @"Grouping computers from node " +
                                                                                 index1;
                }));
                if (ciCurrent.Users.Items.Count > 0)
                {
                    // For each user found in the current node
                    foreach (var ui in ciCurrent.Users.Items.Where(ui => ui.IsComputerUser))
                    {
                        // search in the next nodes for a match
                        for (var j = index + 1; j < clientComputers.Count; j++)
                        {
                            if (clientComputers[j].Users.Items.Count <= 0) continue;
                            if (
                                clientComputers[j].Users.Items.Where(ui2 => ui2.IsComputerUser)
                                    .Any(
                                        ui2 =>
                                            string.Equals(ui2.Name.Trim(), ui.Name.Trim(),
                                                StringComparison.CurrentCultureIgnoreCase)))
                            {
                                JoinComputers(ciCurrent, clientComputers[j]);
                                clientComputers.Remove(clientComputers[j]);
                                match = true;
                            }
                            if (match) break;
                        }
                        if (match) break;
                    }
                }
                // if the machine has no users but it has routes, search for machines in those routes
                else if (ciCurrent.Folders.Items.Count > 0)
                {
                    // foreach folder found in those routes
                    foreach (var ri in ciCurrent.Folders.Items.Where(ri => ri.IsComputerFolder))
                    {
                        for (var j = index + 1; j < clientComputers.Count; j++)
                        {
                            if (clientComputers[j].Folders.Items.Count <= 0) continue;
                            if (
                                clientComputers[j].Folders.Items.Where(ri2 => ri2.IsComputerFolder)
                                    .Any(
                                        ri2 =>
                                            ri2.Path.Trim().ToLower().Contains(ri.Path.Trim().ToLower()) ||
                                            ri.Path.Trim().ToLower().Contains(ri2.Path.Trim().ToLower())))
                            {
                                JoinComputers(ciCurrent, clientComputers[j]);
                                clientComputers.Remove(clientComputers[j]);
                                match = true;
                            }
                            if (match) break;
                        }
                        if (match) break;
                    }
                }
                // if the machine has nor users nor routes but it has printers,
                // look for machines which also have those printers and join them
                else if (ciCurrent.Printers.Items.Count > 0)
                {
                    foreach (var ii in ciCurrent.Printers.Items)
                    {
                        for (var j = index + 1; j < clientComputers.Count; j++)
                        {
                            if (clientComputers[j].Printers.Items.Count <= 0) continue;
                            if (
                                clientComputers[j].Printers.Items.Any(
                                    ii2 =>
                                        string.Equals(ii2.Printer.Trim(), ii.Printer.Trim(),
                                            StringComparison.CurrentCultureIgnoreCase)))
                            {
                                JoinComputers(ciCurrent, clientComputers[j]);
                                clientComputers.Remove(clientComputers[j]);
                                match = true;
                            }
                            if (match) break;
                        }
                        if (match) break;
                    }
                }
                // if the machine has nor users nor routers nor printers, but it has remote routes,
                // search for machines which share those routes and join them
                else if (ciCurrent.RemoteFolders.Items.Count > 0)
                {
                    foreach (var ri in ciCurrent.RemoteFolders.Items)
                    {
                        for (var j = index + 1; j < clientComputers.Count; j++)
                        {
                            if (clientComputers[j].RemoteFolders.Items.Count <= 0) continue;
                            if (
                                clientComputers[j].RemoteFolders.Items.Any(
                                    ri2 =>
                                        string.Equals(ri2.Path.Trim(), ri.Path.Trim(),
                                            StringComparison.CurrentCultureIgnoreCase)))
                            {
                                JoinComputers(ciCurrent, clientComputers[j]);
                                clientComputers.Remove(clientComputers[j]);
                                match = true;
                            }
                            if (match) break;
                        }
                        if (match) break;
                    }
                }
                // if the machine has nor users, nor routes, nor printers nor remote routes,
                // but it has remote printers, search for machines which share those remote
                // printers and join them
                else if (ciCurrent.RemotePrinters.Items.Count > 0)
                {
                    foreach (var ii in ciCurrent.RemotePrinters.Items)
                    {
                        for (var j = index + 1; j < clientComputers.Count; j++)
                        {
                            if (clientComputers[j].RemotePrinters.Items.Count <= 0) continue;
                            if (
                                clientComputers[j].RemotePrinters.Items.Any(
                                    ii2 =>
                                        string.Equals(ii2.Printer.Trim(), ii.Printer.Trim(),
                                            StringComparison.CurrentCultureIgnoreCase)))
                            {
                                JoinComputers(ciCurrent, clientComputers[j]);
                                clientComputers.Remove(clientComputers[j]);
                                match = true;
                            }
                            if (match) break;
                        }
                        if (match) break;
                    }
                }

                // if there isn't a node where a match happens, go to the next one
                if (match) continue;
                if (index + 1 >= clientComputers.Count)
                    break;
                index++;
                ciCurrent = clientComputers[index];
            }
        }

        /// <summary>
        /// Joins the information of 2 computers into the first one and removes the last one
        /// </summary>
        /// <param name="ci1"></param>
        /// <param name="ci2"></param>
        private static void JoinComputers(ComputersItem ci1, ComputersItem ci2)
        {
            if (ci2.SourceDocuments.Count > 0 && !ci1.SourceDocuments.Contains(ci2.SourceDocuments[0]))
            {
                ci1.SourceDocuments.Add(ci2.SourceDocuments[0]);
            }
            // join operating systems
            if (ci2.os != OperatingSystem.OS.Unknown && ci2.os == OperatingSystem.OS.Unknown)
            {
                ci1.os = ci2.os;
            }
            // join users, folders and printers
            if (ci1.Users.Items.Count > 0 && ci2.Users.Items.Count > 0)
            {
                foreach (
                    var ui3 in
                        ci2.Users.Items.Where(
                            ui3 => !ci1.Users.Items.Contains(ui3, new CaseInsensitiveUserItemComparer())))
                {
                    ci1.Users.Items.Add(ui3);
                }
            }
            if (ci2.Folders.Items.Count > 0)
            {
                foreach (
                    var ri3 in
                        ci2.Folders.Items.Where(
                            ri3 => !ci1.Folders.Items.Contains(ri3, new CaseInsensitiveFolderItemComparer<PathsItem>()))
                    )
                {
                    ci1.Folders.Items.Add(ri3);
                }
            }
            if (ci2.RemoteFolders.Items.Count > 0)
            {
                foreach (
                    var ri3 in
                        ci2.RemoteFolders.Items.Where(
                            ri3 =>
                                !ci1.RemoteFolders.Items.Contains(ri3,
                                    new CaseInsensitiveFolderItemComparer<PathsItem>())))
                {
                    ci1.RemoteFolders.Items.Add(ri3);
                }
            }
            if (ci2.Printers.Items.Count > 0)
            {
                foreach (
                    var ii3 in
                        ci2.Printers.Items.Where(
                            ii3 =>
                                !ci1.Printers.Items.Contains(ii3, new CaseInsensitivePrinterItemComparer<PrintersItem>()))
                    )
                {
                    ci1.Printers.Items.Add(ii3);
                }
            }
            if (ci2.RemotePrinters.Items.Count > 0)
            {
                foreach (
                    var ii3 in
                        ci2.RemotePrinters.Items.Where(
                            ii3 =>
                                !ci1.RemotePrinters.Items.Contains(ii3,
                                    new CaseInsensitivePrinterItemComparer<PrintersItem>())))
                {
                    ci1.RemotePrinters.Items.Add(ii3);
                }
            }
            // remove the second computer, it's information is now stored into the first one
            Program.data.computers.Items.Remove(ci2);
        }

        /// <summary>
        /// For each named server, search for possible new resolutions
        /// </summary>
        public void ReanalyzeServers()
        {
            Invoke(new MethodInvoker(() =>
            {
                const string strMessage = "Resolving unknown servers...";
                Program.FormMainInstance.toolStripStatusLabelLeft.Text = strMessage;
                Program.LogThis(new Log(Log.ModuleType.FOCA, strMessage, Log.LogType.debug));
            }));

            var lstCi =
                new List<ComputersItem>(
                    Program.data.computers.Items.Where(
                        c => c.type == ComputersItem.Tipo.Server && c.name.IndexOf('.') < 0));
            foreach (var t in lstCi)
            {
                var ci = t;
                ResolveHost(ci.name, ref ci);
            }
            Invoke(new MethodInvoker(() =>
            {
                const string strMessage = "Reanalysis finished!";
                Program.FormMainInstance.toolStripStatusLabelLeft.Text = strMessage;
                Program.LogThis(new Log(Log.ModuleType.FOCA, strMessage, Log.LogType.debug));
            }));
            // obtain machines for each IP
            Program.data.GetServersFromIPs();
        }

        /// <summary>
        ///     Resolve directly and reversely a server's address
        /// </summary>
        /// <param name="server"></param>
        /// <param name="ci"></param>
        public void ResolveHost(string server, ref ComputersItem ci)
        {
            var strSource = ci.SourceDocuments.Count > 0
                ? $"Metadata extraction [{ci.SourceDocuments[0]}]"
                : "Metadata extraction";
            if (IsIP(server))
            {
                var strIp = server;
                Program.data.AddIP(strIp, strSource, Program.data.Project.Domain, Program.cfgCurrent.MaxRecursion, true);
                Program.data.computerIPs.Items.Add(new ComputerIPsItem(ci, Program.data.GetIp(strIp), strSource));
                // check if it was possible to reverse resolve the server's IP
                if (Program.data.GetResolutionDomains(server).Count > 0)
                {
                    var domains = Program.data.GetResolutionDomains(strIp);
                    ci.name = domains[0].Domain;
                    foreach (var domain in domains)
                    {
                        Program.data.computerDomains.Items.Add(new ComputerDomainsItem(ci, domain, strSource));
                    }
                }
                else
                {
                    ci.name = server;
                }
            }
            else
            {
                if (
                    Program.data.computerDomains.Items.Any(
                        c =>
                            c.Domain != null &&
                            string.Equals(c.Domain.Domain, server, StringComparison.OrdinalIgnoreCase)))
                {
                    // a machine assigned to that domain already exists, remove the new one
                    Program.data.computers.Items.Remove(ci);
                    // use the one that already exists
                    ci =
                        Program.data.computerDomains.Items.First(
                            c => string.Equals(c.Domain.Domain, server, StringComparison.OrdinalIgnoreCase)).Computer;
                }
                else
                {
                    ci.name = server;
                    var lstDomains = new List<string>();
                    if (!string.IsNullOrEmpty(Program.data.Project.Domain))
                        lstDomains.Add(Program.data.Project.Domain);
                    lstDomains.AddRange(Program.data.Project.AlternativeDomains);
                    // add all subdomains
                    lstDomains.AddRange(
                        Program.data.domains.Items.Where(
                            d =>
                                lstDomains.Any(
                                    d2 => d.Domain != d2 && d.Domain.EndsWith(d2, StringComparison.OrdinalIgnoreCase)))
                            .Select(d => d.Domain));
                    // if it isn't a domain, add the configuration's domain and alternative domains
                    if (server.IndexOf('.') < 0 && lstDomains.Count > 0)
                    {
                        if (!Program.cfgCurrent.ResolveHost) return;
                        foreach (var strDomain in lstDomains)
                        {
                            IPAddress[] ips;
                            Program.LogThis(new Log(Log.ModuleType.FOCA,
                                $"Trying resolve {server + "." + strDomain}", Log.LogType.debug));
                            if (DNSUtilities.isDNSAnyCast(strDomain) ||
                                !DNSUtilities.ExistsDomain(server + "." + strDomain, out ips)) continue;
                            ci.name = $"{server}.{strDomain}";

                            // add the domain to the DNS enumeration data
                            Program.data.AddDomain(ci.name, strSource, Program.cfgCurrent.MaxRecursion,
                                Program.cfgCurrent);
                            Program.data.computerDomains.Items.Add(new ComputerDomainsItem(ci,
                                Program.data.GetDomain(ci.name), strSource));
                            foreach (var ip in ips)
                            {
                                Program.data.AddIP(ip.ToString(), strSource,
                                    Program.cfgCurrent.ResolveHost ? 2 : 0);
                                Program.data.computerIPs.Items.Add(new ComputerIPsItem(ci,
                                    Program.data.GetIp(ip.ToString()), strSource));
                            }
                            break;
                        }
                    }
                    else
                    {
                        // add the domain to the DNS enumeration data
                        Program.data.AddDomain(server, strSource,
                            Program.cfgCurrent.ResolveHost ? Program.cfgCurrent.MaxRecursion : 0, Program.cfgCurrent);

                        IPAddress[] ips;
                        if (!Program.cfgCurrent.ResolveHost || DNSUtilities.isDNSAnyCast(server) ||
                            !DNSUtilities.ExistsDomain(server, out ips)) return;
                        foreach (var ip in ips)
                        {
                            Program.data.AddIP(ip.ToString(), strSource, Program.cfgCurrent.MaxRecursion);
                            Program.data.computerIPs.Items.Add(new ComputerIPsItem(ci,
                                Program.data.GetIp(ip.ToString()), strSource));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// If a machine is renamed, we must rename the node that represents it
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public void NameChangedFunction(object o, DoubleStringEventArgs e)
        {
            if (e.oldString != e.newString)
                RenameNode(e.oldString, e.newString);
        }

        /// <summary>
        /// Rename a machine, if a machine with the new name already exists, join their information
        /// </summary>
        /// <param name="strOldName"></param>
        /// <param name="strNewName"></param>
        private static void RenameNode(string strOldName, string strNewName)
        {
            if (Program.data.computers.Items.All(c => c.name != strNewName)) return;
            var ciOld = Program.data.computers.Items.First(c => c.name == strOldName);
            var ciNew = Program.data.computers.Items.First(c => c.name == strNewName);
            if (ciOld != null && ciNew != null)
            {
                // join users
                foreach (var ui in ciOld.Users.Items)
                {
                    ciNew.Users.AddUniqueItem(ui.Name, ui.IsComputerUser);
                }
                // join remote users
                foreach (var ui in ciOld.RemoteUsers.Items)
                {
                    ciNew.RemoteUsers.AddUniqueItem(ui.Name, ui.IsComputerUser);
                }
                // join routes
                foreach (var ri in ciOld.Folders.Items)
                {
                    ciNew.Folders.AddUniqueItem(ri.Path, ri.IsComputerFolder);
                }
                // join remote routes
                foreach (var ri in ciOld.RemoteFolders.Items)
                {
                    ciNew.RemoteFolders.AddUniqueItem(ri.Path, ri.IsComputerFolder);
                }
                // join printers
                foreach (var ii in ciOld.Printers.Items)
                {
                    ciNew.Printers.AddUniqueItem(ii.Printer);
                }
                // join remote printers
                foreach (var ii in ciOld.RemotePrinters.Items)
                {
                    ciNew.RemotePrinters.AddUniqueItem(ii.Printer);
                }
                // join identified software
                foreach (var aplicacion in from aplicacion in ciOld.Software.Items
                                           let any = ciNew.Software.Items.Any(a => a.Name.ToLower().Equals(aplicacion.Name.ToLower()))
                                           where !any
                                           select aplicacion)
                {
                    ciNew.Software.Items.Add(aplicacion);
                }
                // join source documents
                foreach (
                    var strDocument in
                        ciOld.SourceDocuments.Where(
                            strDocument =>
                                !ciNew.SourceDocuments.Contains(strDocument.ToLower(), StringComparer.OrdinalIgnoreCase))
                    )
                {
                    ciNew.SourceDocuments.Add(strDocument);
                }
                // join domains
                foreach (var cdi in Program.data.computerDomains.Items.Where(c => c.Computer.name == ciOld.name))
                {
                    if (Program.data.computerDomains.Items.All(c => c.Domain.Domain != cdi.Domain.Domain))
                    {
                        Program.data.computerDomains.Items.Add(new ComputerDomainsItem(ciNew, cdi.Domain, cdi.Source));
                    }
                    Program.data.computerDomains.Items.Remove(cdi);
                }
                // join IP addresses
                foreach (var cii in Program.data.computerIPs.Items.Where(c => c.Computer.name == ciOld.name))
                {
                    if (Program.data.computerIPs.Items.All(c => c.Ip.Ip != cii.Ip.Ip))
                    {
                        Program.data.computerIPs.Items.Add(new ComputerIPsItem(ciNew, cii.Ip, cii.Source));
                    }
                    Program.data.computerIPs.Items.Remove(cii);
                }
            }
            // remove the machine with the old name
            Program.data.computers.Items.Remove(ciOld);
        }

        #endregion

        #region WebSearch functions

        /// <summary>
        /// Custom search using all checked engines
        /// </summary>
        /// <param name="parameter"></param>
        private void CustomSearch(object parameter)
        {
            var searchString = parameter as string;
            try
            {
                if (chkGoogle.Checked)
                    CustomSearchEventsGeneric(new GoogleWebSearcher(), searchString);
                if (chkBing.Checked)
                    CustomSearchEventsGeneric(new BingWebSearcher(), searchString);
            }
            catch (ThreadAbortException)
            {
                Invoke(new MethodInvoker(delegate
                {
                    const string strMessage = "Aborted document search!";
                    Program.LogThis(new Log(Log.ModuleType.MetadataSearch, strMessage, Log.LogType.debug));
                    Program.FormMainInstance.ChangeStatus(strMessage);
                }));
            }
            finally
            {
                CurrentSearch = null;
            }
        }

        /// <summary>
        ///     Generic event for custom search action
        /// </summary>
        /// <param name="wsSearch"></param>
        /// <param name="searchString"></param>
        private void CustomSearchEventsGeneric(WebSearcher wsSearch, string searchString)
        {
            try
            {
                wsSearch.SearchAll = true;
                wsSearch.SearcherChangeStateEvent += HandleChangeStateEvent;
                wsSearch.SearcherStartEvent += HandleCustomSearchStartEvent;
                wsSearch.SearcherLinkFoundEvent += HandleLinkFoundEvent;
                wsSearch.SearcherLogEvent += WebSearcherLogEvent;
                wsSearch.SearcherEndEvent += HandleCustomSearchEndEvent;
                wsSearch.GetCustomLinks(searchString);
                wsSearch.Join();
            }
            catch (ThreadAbortException)
            {
                wsSearch.Abort();
            }
        }

        /// <summary>
        /// Search using all checked engines
        /// </summary>
        private void SearchAll()
        {
            try
            {
                if (chkGoogle.Checked)
                    SearchEventsGeneric(new GoogleWebSearcher());
                if (chkBing.Checked)
                    SearchEventsGeneric(new BingWebSearcher());
                if (chkDuck.Checked)
                    SearchEventsGeneric(new DuckduckgoWebSearcher());
            }
            catch (ThreadAbortException)
            {
                Invoke(new MethodInvoker(delegate
                {
                    const string strMessage = "Aborted document search!";
                    Program.LogThis(new Log(Log.ModuleType.MetadataSearch, strMessage, Log.LogType.debug));
                    Program.FormMainInstance.ChangeStatus(strMessage);
                }));
            }
            finally
            {
                CurrentSearch = null;
            }
        }

        /// <summary>
        /// Generic event for search action
        /// </summary>
        /// <param name="wsSearch"></param>
        private void SearchEventsGeneric(WebSearcher wsSearch)
        {
            try
            {
                foreach (int i in checkedListBoxExtensions.CheckedIndices)
                {
                    var strExt = (string)checkedListBoxExtensions.Items[i];
                    // some extensions are marked as '*', delete them
                    strExt = strExt.Replace("*", string.Empty);
                    wsSearch.AddExtension(strExt);
                }
                wsSearch.Site = Program.data.Project.Domain;
                wsSearch.SearcherChangeStateEvent += HandleChangeStateEvent;
                wsSearch.SearcherStartEvent += HandleSearchStartEvent;
                wsSearch.SearcherLinkFoundEvent += HandleLinkFoundEvent;
                wsSearch.SearcherLogEvent += WebSearcherLogEvent;
                wsSearch.SearcherEndEvent += HandleSearchEndEvent;

                wsSearch.GetLinks();
                wsSearch.Join();
            }
            catch (ThreadAbortException)
            {
                wsSearch.Abort();
            }
        }

        /// <summary>
        /// Handle searcher logging events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void WebSearcherLogEvent(object sender, EventsThreads.ThreadStringEventArgs e)
        {
            Program.LogThis(new Log(Log.ModuleType.MetadataSearch, $"Web Search, {e.Message}",
                Log.LogType.debug));
        }

        /// <summary>
        /// Handle searcher state changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleChangeStateEvent(object sender, EventsThreads.ThreadStringEventArgs e)
        {
            Invoke(new MethodInvoker(() =>
            {
                if (Program.FormMainInstance != null)
                    Program.FormMainInstance.toolStripStatusLabelLeft.Text = e.Message;
                Program.LogThis(new Log(Log.ModuleType.MetadataSearch, e.Message, Log.LogType.debug));
            }));
        }

        /// <summary>
        ///     Handle custom search start event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HandleCustomSearchStartEvent(object sender, EventArgs e)
        {
            Invoke(new MethodInvoker(() =>
            {
                Program.FormMainInstance.programState = FormMain.ProgramState.Searching;
                btnSearch.Text = "&Stop";
                btnSearch.Image = Resources.magnifier_stop;
            }));
        }

        /// <summary>
        ///     Event used to handle found links events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleLinkFoundEvent(object sender, EventsThreads.ThreadListDataFoundEventArgs e)
        {
            Program.LogThis(new Log(Log.ModuleType.MetadataSearch, $"Found {e.Data.Count} links",
                Log.LogType.debug));

            foreach (string link in e.Data)
            {
                try
                {
                    if (link == string.Empty)
                        continue;

                    if (Program.data.files.Items.Count(li => string.Equals(li.URL, link, StringComparison.CurrentCultureIgnoreCase)) > 0)
                        continue;

                    var u = new Uri(link);

                    if (Program.data.GetDomain(u.Host) == null)
                        Program.data.AddDomain(u.Host, "WebSearch", Program.cfgCurrent.MaxRecursion, Program.cfgCurrent);

                    var dominio = Program.data.GetDomain(u.Host);
                    dominio.map.AddUrl(u.ToString());

                    if (dominio.techAnalysis.domain == null)
                        dominio.techAnalysis.domain = dominio.Domain;

                    var listaUrl = new List<object> { u };
                    dominio.techAnalysis.eventLinkFoundDetailed(null,
                        new EventsThreads.ThreadListDataFoundEventArgs(listaUrl));
                }
                catch
                {
                }

                Invoke(new MethodInvoker(() =>
                {
                    var fi = new FilesITem
                    {
                        Ext = Path.GetExtension(new Uri(link).AbsolutePath).ToLower(),
                        URL = link,
                        Downloaded = false,
                        Processed = false,
                        Date = DateTime.MinValue,
                        ModifiedDate = DateTime.MinValue,
                        Path = string.Empty,
                        Size = -1
                    };
                    Program.data.files.Items.Add(fi);
                    Program.FormMainInstance.treeViewMetadata_UpdateDocumentsNumber();
                    var lvi = listViewDocuments_Update(fi);
                    HttpSizeDaemonInst.AddURL(link, lvi);
                }));
                // add the domain from the found link to the project's domains
                var uri = new Uri(link);
                if (Program.data.Project.ProjectState == Project.ProjectStates.Uninitialized)
                    continue;
                Program.data.AddDomain(uri.Host, "Documents search", 0,
                    Program.cfgCurrent);
                // add the URL to the domain's map
                Program.data.GetDomain(uri.Host).map.AddDocument(link);
            }
        }

        /// <summary>
        /// Event when a custom search is attempted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleCustomSearchEndEvent(object sender, EventsThreads.ThreadEndEventArgs e)
        {
            Invoke(new MethodInvoker(() =>
            {
                Program.FormMainInstance.programState = FormMain.ProgramState.Normal;
                btnSearch.Text = "&Search";
                btnSearch.Image = Resources.magnifier;
            }));
        }

        /// <summary>
        /// Event launched when a search is started
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleSearchStartEvent(object sender, EventArgs e)
        {
            Invoke(new MethodInvoker(() =>
            {
                btnSearchAll.Text = "&Stop";
                btnSearchAll.Image = Resources.world_search_stop;
                checkedListBoxExtensions.Enabled = panelSearchConfiguration.Enabled = false;
            }));
        }

        /// <summary>
        /// Event launched when a search finishes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleSearchEndEvent(object sender, EventsThreads.ThreadEndEventArgs e)
        {
            Invoke(new MethodInvoker(() =>
            {
                btnSearchAll.Text = "&Search All";
                btnSearchAll.Image = Resources.world_search;
                checkedListBoxExtensions.Enabled = panelSearchConfiguration.Enabled = true;
            }));
        }

        #endregion

        #region Functions to download files

        /// <summary>
        ///     Event launched each time a download's progress changes
        /// </summary>
        /// <param name="o"></param>
        /// <param name="dpce"></param>
        private void DownloadProgressChanged(object o, DownloadProgressChangedEventArgs dpce)
        {
            Invoke(new MethodInvoker(() =>
            {
                ((Download)dpce.UserState).Pbar.Value = dpce.ProgressPercentage;
                ((Download)dpce.UserState).DownloadStatus = Download.Status.Inprogress;
            }));
        }

        /// <summary>
        ///     Event launched each time a download is completed
        /// </summary>
        /// <param name="o"></param>
        /// <param name="acea"></param>
        private void DownloadProgressCompleted(object o, AsyncCompletedEventArgs acea)
        {
            // no data was downloaded, retry 5 times
            if (((Download)acea.UserState).DownloadStatus == Download.Status.Downloading &&
                ((Download)acea.UserState).Retries < 3)
            {
                var d = ((Download)acea.UserState);
                d.CaClient = new CookieAwareWebClient();
                d.CaClient.DownloadFileCompleted += DownloadProgressCompleted;
                d.CaClient.DownloadProgressChanged += DownloadProgressChanged;
                var killer = new Destructor(d, 10000);
                new Thread(killer.Kill).Start();

                d.CaClient.DownloadFileAsync(new Uri(d.DownloadUrl), d.PhysicalPath, d);
                d.Retries++;
                Program.LogThis(new Log(Log.ModuleType.MetadataSearch, "Retrying download " + d.DownloadUrl,
                    Log.LogType.debug));
            }
            else // successful download
            {
                Invoke(new MethodInvoker(() =>
                {
                    ((WebClient)o).Dispose();
                    activeDownloads--;
                    if (acea.Cancelled)
                        File.Delete(((Download)acea.UserState).PhysicalPath);
                    var lvi = ((Download)acea.UserState).Lvi;
                    listViewDocuments.RemoveEmbeddedControl(((Download)acea.UserState).Pbar);
                    var fi = (FilesITem)lvi.Tag;
                    if (fi == null) return;
                    fi.Downloaded = !acea.Cancelled;
                    if (!acea.Cancelled)
                    {
                        fi.Date = DateTime.Now;
                        fi.Size = (int)new FileInfo(((Download)acea.UserState).PhysicalPath).Length;
                        fi.Ext = Path.GetExtension(new Uri(fi.URL).AbsolutePath).ToLower();
                        var knownExtension =
                            Program.FormMainInstance.AstrSuportedExtensions.Any(ext => "." + ext == fi.Ext);
                        if (!knownExtension)
                        {
                            using (var fs = new FileStream(fi.Path, FileMode.Open, FileAccess.Read))
                            {
                                fi.Ext = MetadataExtractCore.Utilities.Functions.GetFormatFile(fs);
                            }
                            if (!string.IsNullOrEmpty(lvi.SubItems[1].Text)) // rename the file
                            {
                                var newFile = GetNotExistsPath(fi.Path + "." + lvi.SubItems[1].Text);
                                File.Move(fi.Path, newFile);
                                fi.Path = newFile;
                                fi.Ext = Path.GetExtension(fi.Path);
                            }
                        }
                        listViewDocuments_Update(fi);
                    }
                    DownloadedFiles++;
                    Program.LogThis(new Log(Log.ModuleType.MetadataSearch,
                        $"Downloaded document: {fi.URL}", Log.LogType.low));
                    if (enqueuedFiles - DownloadedFiles == 0)
                    {
                        Program.FormMainInstance.ReportProgress(0, 100);
                        Program.FormMainInstance.toolStripProgressBarDownload.Value = 0;
                        Program.FormMainInstance.toolStripStatusLabelLeft.Text = @"All documents are downloaded";
                        Program.LogThis(new Log(Log.ModuleType.MetadataSearch, "All documents are downloaded",
                            Log.LogType.debug));
                    }
                    else
                    {
                        if (enqueuedFiles > 0 && DownloadedFiles <= enqueuedFiles)
                        {
                            Program.FormMainInstance.ReportProgress(DownloadedFiles, enqueuedFiles);
                            Program.FormMainInstance.toolStripProgressBarDownload.Value = DownloadedFiles == 0
                                ? 0
                                : DownloadedFiles * 100 / enqueuedFiles;
                            Program.FormMainInstance.toolStripStatusLabelLeft.Text =
                                $"Downloading {DownloadedFiles}/{enqueuedFiles}";
                        }
                    }
                    Downloads.Remove((Download)acea.UserState);
                }));
            }
        }

        /// <summary>
        ///     Download list of files to a given path
        /// </summary>
        /// <param name="urls"></param>
        /// <param name="directory"></param>
        public void DownloadFiles(List<ListViewItem> urls, string directory)
        {
            //Enqueued all files with their url and bind it a physical path in hard disk
            foreach (var url in urls)
            {
                var d = new Download();
                var fileName = Path.GetFileName(new Uri(url.SubItems[2].Text).AbsolutePath);
                //Delete incorrect filename characters
                for (var i = 0; fileName.IndexOfAny(MyInvalidPathChars) != -1; i++)
                    fileName = fileName.Replace(MyInvalidPathChars[i], ' ');
                d.DownloadStatus = Download.Status.Enqueued;
                d.PhysicalPath = directory;
                d.DownloadUrl = url.SubItems[2].Text;
                d.Lvi = url;
                Downloads.Add(d);
            }
            if (CurrentDownloads != null && CurrentDownloads.IsAlive)
            {
                enqueuedFiles += urls.Count;
            }
            else
            {
                enqueuedFiles = urls.Count;
                DownloadedFiles = 0;
                CurrentDownloads = new Thread(DownloadQueque) { IsBackground = true };
                CurrentDownloads.Start();
            }
            Program.LogThis(new Log(Log.ModuleType.MetadataSearch,
                $"Enquequed {urls.Count} documents to download, total enquequed: {enqueuedFiles}",
                Log.LogType.debug));
            Program.FormMainInstance.ReportProgress(DownloadedFiles, enqueuedFiles);
            Program.FormMainInstance.toolStripProgressBarDownload.Value = DownloadedFiles == 0
                ? 0
                : DownloadedFiles * 100 / enqueuedFiles;
            Program.ChangeStatus($"Downloading {DownloadedFiles}/{enqueuedFiles}");
        }

        /// <summary>
        /// Download files from the queue
        /// </summary>
        private void DownloadQueque()
        {
            Invoke(new MethodInvoker(() => { Program.FormMainInstance.toolStripDropDownButtonStop.Enabled = true; }));
            try
            {
                while (Downloads.Count != 0)
                {
                    while (activeDownloads >= Program.cfgCurrent.SimultaneousDownloads)
                        Thread.Sleep(1000);

                    Download d;
                    // get an inactive download from the queue
                    for (var i = 0; i < Downloads.Count; i++)
                    {
                        var exists = true;
                        var i1 = i;
                        Invoke(new MethodInvoker(() =>
                        {
                            // the enqueued file was deleted
                            try
                            {
                                if (Downloads[i1].Lvi.Index != -1) return;
                                DownloadedFiles++;
                                try
                                {
                                    File.Delete(Downloads[i1].PhysicalPath);
                                }
                                catch
                                {
                                }
                                Downloads.RemoveAt(i1);
                                exists = false;
                            }
                            catch
                            {
                            }
                        }));

                        if (i >= Downloads.Count)
                            break;
                        if (!exists || Downloads[i].DownloadStatus != Download.Status.Enqueued) continue;
                        Downloads[i].DownloadStatus = Download.Status.Downloading;
                        d = Downloads[i];
                        var fileName = Path.GetFileName(new Uri(d.Lvi.SubItems[2].Text).AbsolutePath);
                        //Delete incorrect filename characters
                        for (var j = 0; fileName.IndexOfAny(MyInvalidPathChars) != -1; j++)
                            fileName = fileName.Replace(MyInvalidPathChars[j], ' ');
                        var downloadPath = GetNotExistsPath(d.PhysicalPath + fileName);
                        // create an empty file to take over the filename and avoid other downloads overwriting it
                        if (!ValidatePathExist(d.PhysicalPath))
                        {
                            MessageBox.Show("The selected folder does not exist, please check in the project configuration - " + d.PhysicalPath, "Message for User", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }

                        File.Create(downloadPath).Close();
                        var fi = (FilesITem)d.Lvi.Tag;
                        if (fi != null)
                            fi.Path = downloadPath;
                        d.PhysicalPath = downloadPath;


                        Invoke(new MethodInvoker(() =>
                        {
                            var pb = new ProgressBar
                            {
                                Top = -1000,
                                Left = -1000,
                                ForeColor = Color.Green
                            };
                            d.Pbar = pb;
                            listViewDocuments.AddEmbeddedControl(pb, 3, d.Lvi.Index);
                            pb.Value = 0;
                            listViewDocuments.Refresh();
                        }));
                        d.CaClient = new CookieAwareWebClient();
                        d.CaClient.Headers.Add(HttpRequestHeader.Accept,
                            "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");

                        #region moodle requires cookies to download files from it

                        if (d.DownloadUrl.Contains("/moodle/"))
                        {
                            Program.LogThis(new Log(Log.ModuleType.MetadataSearch,
                                "Found moodle application, adding cookies to download document", Log.LogType.debug));
                            var cc = new CookieContainer();
                            HttpWebRequest req;
                            HttpWebResponse resp;
                            var location = d.DownloadUrl;
                            do
                            {
                                req = (HttpWebRequest)WebRequest.Create(location);
                                req.AllowAutoRedirect = false;
                                req.CookieContainer = cc;
                                try
                                {
                                    resp = (HttpWebResponse)req.GetResponse();
                                }
                                catch
                                {
                                    break;
                                }
                                resp.Close();
                                cc.Add(resp.Cookies);
                                location = resp.Headers["Location"];
                            } while (resp.StatusCode == HttpStatusCode.SeeOther &&
                                     location != req.RequestUri.ToString());
                            d.CaClient.m_container = cc;
                        }

                        #endregion

                        d.CaClient.DownloadFileCompleted += DownloadProgressCompleted;
                        d.CaClient.DownloadProgressChanged += DownloadProgressChanged;
                        Invoke(new MethodInvoker(() => { activeDownloads++; }));

                        // 30 delay seconds before retrying the download
                        var killer = new Destructor(d, 30000);
                        var t = new Thread(killer.Kill) { IsBackground = true };
                        t.Start();

                        d.CaClient.DownloadFileAsync(new Uri(d.DownloadUrl), d.PhysicalPath, d);
                        break;
                    }
                }
            }
            catch (ThreadAbortException)
            {
                Invoke(new MethodInvoker(() =>
                {
                    for (var i = Downloads.Count - 1; i >= 0; i--)
                    {
                        var d = Downloads[i];
                        if (d.CaClient != null)
                            d.CaClient.CancelAsync();
                        else
                        {
                            DownloadedFiles++;
                            Downloads.RemoveAt(i);
                        }
                    }
                }));
                while (Downloads.Count != 0)
                    Thread.Sleep(100);
            }
            finally
            {
                Invoke(
                    new MethodInvoker(() => { Program.FormMainInstance.toolStripDropDownButtonStop.Enabled = false; }));
                CurrentDownloads = null;
            }
        }

        /// <summary>
        /// Validate if path exist
        /// </summary>
        /// <param name="downloadPath"></param>
        /// <returns></returns>
        private bool ValidatePathExist(string downloadPath)
        {
            return Directory.Exists(downloadPath);
        }

        #endregion
    }
}

public class Destructor
{
    private readonly PanelMetadataSearch.Download d;
    private readonly int time;

    public Destructor(PanelMetadataSearch.Download d, int time)
    {
        this.d = d;
        this.time = time;
    }

    /// <summary>
    ///     Kill a download
    /// </summary>
    public void Kill()
    {
        Thread.Sleep(time);
        if (d != null && d.DownloadStatus != PanelMetadataSearch.Download.Status.Inprogress)
        {
            d.CaClient.CancelAsync();
        }
    }
}