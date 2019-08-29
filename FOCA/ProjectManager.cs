using FOCA.Database.Controllers;
using FOCA.Database.Entities;
using FOCA.Threads;
using MetadataExtractCore.Diagrams;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace FOCA.Core
{
    /// <summary>
    /// Class aimed to contain all storing projects and loading projects logic
    /// </summary>
    public class ProjectManager
    {
        public FOCA.FormMain mainForm;
        public bool Saving = false;

        public ProjectManager(FormMain mainForm)
        {
            this.mainForm = mainForm;
        }

        #region Loading data methods

        public void LoadProject(string strPathSavedFile)
        {
            Thread t = new Thread(new ParameterizedThreadStart(LoadProjectThread));
            t.IsBackground = true;
            t.Start(strPathSavedFile);
        }

        public void SaveProjectDelegate()
        {
            SaveProjectData();
        }

        public void LoadProjectThread(object o)
        {
            var strPathSavedFile = (string)o;
            using (var fLoading = new FormLoadSave(mainForm))
            {
                try
                {
                    fLoading.action = FormLoadSave.Action.Loading;
                    fLoading.Tag = strPathSavedFile;

                    if (fLoading.ShowDialog() == DialogResult.OK)
                    {
                        mainForm.Invoke(new MethodInvoker(delegate
                        {
                            mainForm.Select();
                            MessageBox.Show("Project loaded successfully!", mainForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Program.data.tasker.AsociaEventosTareas();
                        }));
                    }
                }
                catch (System.IO.FileNotFoundException)
                {
                    mainForm.Invoke(new MethodInvoker(delegate
                    {
                        MessageBox.Show(String.Format("The file {0} don't exist", strPathSavedFile), mainForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }));
                }
                catch (Exception)
                {
                    mainForm.Invoke(new MethodInvoker(delegate
                    {
                        MessageBox.Show("Project file is corrupted or old", mainForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }));
                }
            }


            mainForm.Invoke(new MethodInvoker(delegate
            {
                if (Program.data.Project == null)
                    GUI.UpdateGUI.Reset();
            }));
        }

        public void LoadProjectDelegate(string strPathSavedFile)
        {
            IAsyncResult result = mainForm.BeginInvoke(new MethodInvoker(delegate
            {
                mainForm.Reset();
            }));

            mainForm.EndInvoke(result);

            Program.data.Project.ProjectSaveFile = strPathSavedFile;

            mainForm.Invoke(new MethodInvoker(delegate
            {
                mainForm.LoadInitialProjectGui();
                GUI.UpdateGUI.Reset();
            }));

            LoadData();
            LoadMetadataSumaryData();
        }

        /// <summary>
        /// Load Data.
        /// </summary>
        private void LoadData()
        {
            var idProject = Program.data.Project.Id;

            LoadProjectDataController(idProject);

            Program.data.OnLog +=
                new EventHandler<EventsThreads.ThreadStringEventArgs>(
                    delegate (object s, EventsThreads.ThreadStringEventArgs ev)
                    {
                        Program.LogThis(new Log(Log.ModuleType.DNS, ev.Message, Log.LogType.debug));
                    });

            mainForm.Invoke(new MethodInvoker(delegate
            {
                mainForm.panelMetadataSearch.listViewDocuments.BeginUpdate();

                foreach (FilesItem fi in Program.data.files.Items)
                {
                    Program.FormMainInstance.panelMetadataSearch.listViewDocuments_Update(fi);
                    if (fi.Processed)
                    {
                        TreeNode tn_file = Program.FormMainInstance.TreeViewMetadataAddDocument(fi);
                        tn_file.Tag = fi;
                        string extension = System.IO.Path.GetExtension(fi.Path).ToLower();
                        tn_file.ImageIndex =
                            tn_file.SelectedImageIndex = Program.FormMainInstance.GetImageToExtension(extension);
                        //Carga los metadatos si los tiene
                        if (fi.Metadata != null)
                        {
                            Program.FormMainInstance.panelMetadataSearch.AddDocumentNodes(fi.Metadata, tn_file);
                        }
                    }
                }

                mainForm.treeViewMetadata_UpdateDocumentsNumber();
                mainForm.panelMetadataSearch.listViewDocuments.EndUpdate();
            }));
        }

        public static void LoadProjectDataController(int idProject)
        {
            Program.data.Project = new ProjectController().GetProjectById(idProject);
            Program.data.domains.Items = new DomainsController().GetDomainsById(idProject);
            Program.data.computers.Items = new ComputersController().GetComputersByIdProject(idProject);
            Program.data.computerIPs.Items = new ComputerIpsController().GetComputerIpsByIdProject(idProject);
            Program.data.computerDomains.Items = new ComputerDomainController().GetComputerDomainByIdProject(idProject);
            Program.data.lstLimits = new LimitsController().GetLimitsByIdProject(idProject);
            Program.data.relations.Items = new RelationsController().GetRelationsByIdProject(idProject);
            Program.data.files.Items = new FilesController().GetFilesByIdProject(idProject);
            Program.data.Ips.Items = new IpsController().GetIpsByIdProject(idProject);
            Program.data.plugins.lstPlugins = new PluginsController().GetAllPlugins();
        }

        /// <summary>
        /// Load metadata resume.
        /// </summary>
        private void LoadMetadataSumaryData()
        {
            var filesItems = Program.data.files.Items;

            var listMetadata = (from item in filesItems where item.Metadata != null select item.Metadata).ToList();

            var usersValue = (from item in listMetadata where item.FoundUsers.Items.Count != 0 select item.FoundUsers.Items);
            ConcurrentBag<UserItem> listParamUser = new ConcurrentBag<UserItem>(usersValue.SelectMany(item => item));
            mainForm.TreeView.Nodes[GUI.UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[
            GUI.UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Users"].Tag = listParamUser;

            var printerValue = (from item in listMetadata where item.FoundPrinters.Items.Count != 0 select item.FoundPrinters.Items);
            ConcurrentBag<PrintersItem> listParamPrinter = new ConcurrentBag<PrintersItem>(printerValue.SelectMany(item => item));
            mainForm.TreeView.Nodes[GUI.UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[GUI.UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Printers"].Tag = listParamPrinter;

            var pathsValue = (from item in listMetadata where item.FoundPaths.Items.Count != 0 select item.FoundPaths.Items);
            ConcurrentBag<PathsItem> listParamPaths = new ConcurrentBag<PathsItem>(pathsValue.SelectMany(item => item));
            mainForm.TreeView.Nodes[GUI.UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[GUI.UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Folders"].Tag = listParamPaths;

            var emailsValue = (from item in listMetadata where item.FoundEmails.Items.Count != 0 select item.FoundEmails.Items);
            ConcurrentBag<EmailsItem> listParamEmails = new ConcurrentBag<EmailsItem>(emailsValue.SelectMany(item => item));
            mainForm.TreeView.Nodes[GUI.UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[GUI.UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Emails"].Tag = listParamEmails;

            var serversValue = (from item in listMetadata where item.FoundServers.Items.Count != 0 select item.FoundServers.Items);
            ConcurrentBag<ServersItem> listParamServers = new ConcurrentBag<ServersItem>(serversValue.SelectMany(item => item));
            mainForm.TreeView.Nodes[GUI.UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[GUI.UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Servers"].Tag = listParamServers;

            var softValue = (from item in listMetadata where item.FoundMetaData.Applications.Items.Count != 0 select item.FoundMetaData.Applications.Items);
            ConcurrentBag<ApplicationsItem> listParamsoft = new ConcurrentBag<ApplicationsItem>(softValue.SelectMany(item => item));
            mainForm.TreeView.Nodes[GUI.UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[GUI.UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Software"].Tag = listParamsoft;


            var passValue = (from item in listMetadata where item.FoundPasswords.Items.Count != 0 select item.FoundPasswords.Items);
            ConcurrentBag<PasswordsItem> listParamPass = new ConcurrentBag<PasswordsItem>(passValue.SelectMany(item => item));
            mainForm.TreeView.Nodes[GUI.UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[GUI.UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Passwords"].Tag = listParamPass;

            var listOs = new ConcurrentBag<string>(from item in listMetadata where item.FoundMetaData.OperativeSystem != null select item.FoundMetaData.OperativeSystem);
            mainForm.TreeView.Nodes[GUI.UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[GUI.UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Operating Systems"].Tag = listOs;

            mainForm.TreeView.Invoke(new MethodInvoker(delegate
            {
                mainForm.TreeView.Nodes[GUI.UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[GUI.UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Users"].Text = string.Format("Users ({0})", listParamUser.Count);

                mainForm.TreeView.Nodes[GUI.UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[GUI.UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Printers"].Text = string.Format("Printers ({0})", listParamPrinter.Count);

                mainForm.TreeView.Nodes[GUI.UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[GUI.UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Folders"].Text = string.Format("Folders ({0})", listParamPaths.Count);

                mainForm.TreeView.Nodes[GUI.UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[GUI.UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Software"].Text = string.Format("Software ({0})", listParamsoft.Count);

                mainForm.TreeView.Nodes[GUI.UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[GUI.UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Emails"].Text = string.Format("Emails ({0})", listParamEmails.Count);

                mainForm.TreeView.Nodes[GUI.UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[GUI.UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Operating Systems"].Text = string.Format("Operating Systems ({0})", listOs.Count);

                mainForm.TreeView.Nodes[GUI.UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[GUI.UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Passwords"].Text = string.Format("Passwords ({0})", listParamPass.Count);

                mainForm.TreeView.Nodes[GUI.UpdateGUI.TreeViewKeys.KProject.ToString()].Nodes[GUI.UpdateGUI.TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Servers"].Text = string.Format("Servers ({0})", listParamServers.Count);
            }));
        }

        #endregion

        #region Saving data methods

        public void SaveProject(string strPathSavedFile)
        {
            if (Saving == true)
            {
                MessageBox.Show("Already saving, please wait.");
                return;
            }

            Thread t = new Thread(new ParameterizedThreadStart(SaveProjectThread));
            t.IsBackground = true;
            t.Start(strPathSavedFile);
        }

        /// <summary>
        /// Save data in all controllers.
        /// </summary>
        public static void SaveProjectData()
        {
            new ProjectController().Save(Program.data.Project);
            new DomainsController().Save(Program.data.domains.Items);
            new ComputersController().Save(Program.data.computers.Items);
            new IpsController().Save(Program.data.Ips.Items);
            new ComputerIpsController().Save(Program.data.computerIPs.Items);
            new ComputerDomainController().Save(Program.data.computerDomains.Items);
            new LimitsController().Save(Program.data.lstLimits);
            new RelationsController().Save(Program.data.relations.Items);
            new FilesController().Save(Program.data.files.Items);
            new PluginsController().Save(Program.data.plugins.lstPlugins);
        }

        private void SaveProjectThread(object o)
        {
            Saving = true;

            var strPathSavedFile = (string)o;

            using (FormLoadSave fLoading = new FormLoadSave(mainForm))
            {
                try
                {
                    fLoading.action = FormLoadSave.Action.Saving;
                    fLoading.Tag = strPathSavedFile;

                    if (fLoading.ShowDialog() == DialogResult.OK)
                    {
                        mainForm.Invoke(new MethodInvoker(delegate
                        {
                            MessageBox.Show(mainForm, "Project saved successfully!", mainForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }));
                    }
                }
                catch
                {
                    mainForm.Invoke(new MethodInvoker(delegate
                    {
                        MessageBox.Show(mainForm, "Error saving project", mainForm.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }));
                }
                finally
                {
                    Saving = false;
                }
            }
        }

        #endregion
    }
}