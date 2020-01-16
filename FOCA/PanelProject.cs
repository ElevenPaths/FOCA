using FOCA.Core;
using FOCA.Database.Controllers;
using FOCA.Database.Entities;
using FOCA.GUI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace FOCA
{
    public partial class PanelProject : UserControl
    {
        private readonly Color oldColorBackground;
        private readonly Color oldColorForeground;
        private bool definedProject;

        public PanelProject()
        {
            InitializeComponent();

            oldColorBackground = txtDomainWebsite.BackColor;
            oldColorForeground = txtDomainWebsite.ForeColor;
        }

        /// <summary>
        ///     ProjectName property
        /// </summary>
        public string ProjectName
        {
            set { txtProjectName.Text = value; }
            get { return txtProjectName.Text; }
        }

        /// <summary>
        ///     DomainWebsite property
        /// </summary>
        public string DomainWebsite
        {
            set { txtDomainWebsite.Text = value; }
            get { return txtDomainWebsite.Text; }
        }

        /// <summary>
        ///     AlternativeDomains property
        /// </summary>
        public string AlternativeDomains
        {
            set { txtAlternativeDomains.Text = value; }
            get { return txtAlternativeDomains.Text; }
        }

        /// <summary>
        ///     FolderDocuments property
        /// </summary>
        public string FolderDocuments
        {
            set { txtFolderDocuments.Text = value; }
            get { return txtFolderDocuments.Text; }
        }

        /// <summary>
        ///     Notes property
        /// </summary>
        public string Notes
        {
            set { txtNotes.Text = value; }
            get { return txtNotes.Text; }
        }

        /// <summary>
        ///     Clear form fields
        /// </summary>
        public void ClearFields()
        {
            ProjectName = Project.DefaultProjectName;
            DomainWebsite = string.Empty;
            AlternativeDomains = string.Empty;
            FolderDocuments = System.IO.Path.GetTempPath();
            Notes = string.Empty;
            definedProject = false;
            Program.data = new Data();
            btnExport.Visible = false;
        }

        /// <summary>
        ///     Fill project fields with the received project's data
        /// </summary>
        /// <param name="project"></param>
        public void Fill(Project project)
        {
            cmbProject.SelectedItem = project;
            ProjectName = project.ProjectName;
            DomainWebsite = project.Domain;
            AlternativeDomains = string.Empty;
            foreach (var str in project.AlternativeDomains)
                AlternativeDomains += str + Environment.NewLine;
            FolderDocuments = project.FolderToDownload;
            Notes = project.ProjectNotes;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Visible = false;
            if (Program.FormMainInstance.panelMetadataSearch.Visible)
                Program.FormMainInstance.LoadSearchGui(Program.FormMainInstance.panelMetadataSearch.txtSearch.Text,
                    false);
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (ValidateControls())
                return;

            var failure = !CheckDownloadDirectory();
            if (failure) return;

            Program.data.Project.ProjectName = ProjectName;
            Program.data.Project.ProjectState = Project.ProjectStates.InitializedUnsave;
            Program.data.Project.Domain = DomainWebsite.ToLower();
            Program.data.Project.AlternativeDomains.Clear();
            Program.data.Project.AlternativeDomains.AddRange(AlternativeDomains.Split(new[] { Environment.NewLine },
                StringSplitOptions.RemoveEmptyEntries));
            Program.data.Project.ProjectDate = txtDate.Value;
            Program.data.Project.ProjectNotes = Notes;
            Program.data.Project.ProjectSaveFile = string.Empty;
            Program.FormMainInstance.toolStripStatusLabelLeft.Text = string.Empty;
            Program.FormMainInstance.toolStripProgressBarDownload.Value = 0;
            Program.FormMainInstance.LoadInitialProjectGui();

            // OnNewProject
#if PLUGINS
            var tPluginOnNewProject = new Thread(Program.data.plugins.OnNewProject) { IsBackground = true };

            object[] oProject = { new object[] { DomainWebsite } };
            tPluginOnNewProject.Start(oProject);
#endif
            Program.FormMainInstance.ProjectManager.SaveProject(string.Empty);

            if (Program.data.Project.Id == 0)
            {
                Program.FormMainInstance.Reset();
                UpdateGUI.Reset();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            var fallo = !CheckDownloadDirectory();
            if (fallo) return;
            Program.data.Project.ProjectName = ProjectName;
            Program.data.Project.Domain = DomainWebsite;
            var aDomains = AlternativeDomains.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            // check if there're new alternative domains
            var bNewAlternativeDomains = Program.data.Project.AlternativeDomains.Intersect(aDomains).Count() != aDomains.Length;

            bool bAnyServer =
                Program.FormMainInstance.TreeView.GetNode(GUI.Navigation.Project.Network.ToNavigationPath()) != null &&
                Program.FormMainInstance.TreeView.GetNode(GUI.Navigation.Project.Network.Servers.ToNavigationPath()) != null &&
                Program.FormMainInstance.TreeView.GetNode(GUI.Navigation.Project.Network.Servers.ToNavigationPath()).Nodes.Count != 0;

            Program.data.Project.AlternativeDomains.Clear();
            Program.data.Project.AlternativeDomains.AddRange(aDomains);
            Program.data.Project.FolderToDownload = FolderDocuments;
            Program.data.Project.ProjectDate = DateTime.Now;
            Program.data.Project.ProjectNotes = Notes;

            Program.FormMainInstance.Text = Program.data.Project.ProjectName + " - " + Program.ProgramName;

            Visible = false;
            Program.FormMainInstance.LoadSearchGui(Program.FormMainInstance.panelMetadataSearch.txtSearch.Text, false);
            if (!bNewAlternativeDomains || !bAnyServer) return;
            if (
                MessageBox.Show(@"New alternative domains found, do you want reanalyze network servers?",
                    System.Windows.Forms.Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                var tReAnalysis = new Thread(Program.FormMainInstance.panelMetadataSearch.ReanalyzeServers)
                {
                    IsBackground = true
                };
                tReAnalysis.Start();
            }
        }

        /// <summary>
        ///  Check if the download directory exists. If not, create it.
        /// </summary>
        /// <returns>False if it doesn't exist and it wasn't able to create it</returns>
        public bool CheckDownloadDirectory()
        {
            if (FolderDocuments != string.Empty)
            {
                Program.data.Project.FolderToDownload = FolderDocuments;
                if (Directory.Exists(FolderDocuments)) return true;
                try
                {
                    Directory.CreateDirectory(FolderDocuments);
                }
                catch
                {
                    Program.FormMainInstance.LoadProjectGui(false);
                    MessageBox.Show(
                        $"The folder {FolderDocuments} doesn't exist and can't be created. Please, select another folder.");
                    Program.data.Project.FolderToDownload = System.IO.Path.GetTempPath();
                    return false;
                }
            }
            else
                Program.data.Project.FolderToDownload = System.IO.Path.GetTempPath();
            return true;
        }

        private void buttonSelectFolder_Click(object sender, EventArgs e)
        {
            if (fbdMain.ShowDialog() == DialogResult.OK)
                FolderDocuments = fbdMain.SelectedPath;
        }

        private void txtDomainWebsite_TextChanged(object sender, EventArgs e)
        {
            if (definedProject) return;
            txtProjectName.Text = @"Project of " + txtDomainWebsite.Text;
            definedProject = false;
        }

        private void txtProjectName_TextChanged(object sender, EventArgs e)
        {
            definedProject = true;
        }

        /// <summary>
        /// Load all projects.
        /// </summary>
        public void LoadProject()
        {
            var projects = new ProjectController().GetAllProjects();

            cmbProject.ValueMember = "Id";
            cmbProject.DisplayMember = "ProjectName";
            cmbProject.DataSource = projects;

            cmbProject.SelectedIndex = -1;

            btnCreate.Text = Program.data.Project.Id == 0 ? "Create" : "Update";

        }

        private void DataBindProject(Project project)
        {
            txtProjectName.Text = project.ProjectName;
            txtFolderDocuments.Text = project.FolderToDownload;
            txtDate.Value = project.ProjectDate;
            txtNotes.Text = project.ProjectNotes;
            txtDomainWebsite.Text = project.Domain;
            SetAlternativeDomains(project.AlternativeDomains);

            Program.data.Clear();
            Program.data.Project = project;

            Program.FormMainInstance.ProjectManager.LoadProject("");
        }


        private void SetAlternativeDomains(List<string> alternativeDomains)
        {
            foreach (var item in alternativeDomains)
            {
                txtAlternativeDomains.Text = item;
                txtAlternativeDomains.AppendText(Environment.NewLine);
            }
        }

        private bool ValidateControls()
        {
            var result = false;
            errorPorject.Clear();
            if (string.IsNullOrEmpty(txtProjectName.Text))
            {
                errorPorject.SetError(txtProjectName, "Value is required."); result = true;
            }

            if (string.IsNullOrEmpty(txtDomainWebsite.Text))
            {
                errorPorject.SetError(txtDomainWebsite, "Value is required."); result = true;
            }

            if (string.IsNullOrEmpty(txtFolderDocuments.Text))
            {
                errorPorject.SetError(txtFolderDocuments, "Value is required."); result = true;
            }

            if (string.IsNullOrEmpty(txtDate.Value.ToString()))
            {
                errorPorject.SetError(txtDate, "Value is required."); result = true;
            }

            return result;
        }

        private void cmbProject_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (cmbProject.SelectedIndex >= 0)
            {
                var projectItem = (Project)cmbProject.SelectedItem;
                DataBindProject(projectItem);

                btnCreate.Text = "Update";
                btnExport.Visible = true;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            Stream fileStream = null;

            Program.data.Clear();

            string strContent = string.Empty;
            openFileDialogImport.Filter = "Json Files|*.json";
            openFileDialogImport.FileName = "";
            if (openFileDialogImport.ShowDialog() == DialogResult.OK && (fileStream = openFileDialogImport.OpenFile()) != null)
            {
                var fileName = openFileDialogImport.FileName;
                using (fileStream)
                {
                    var reader = new StreamReader(fileStream);
                    while (!reader.EndOfStream)
                    {
                        strContent += reader.ReadLine();
                    }
                }
            }

            var result = JsonConvert.DeserializeObject(strContent, typeof(Data));

            if (result != null)
            {
                Program.data = (Data)result;
                new ProjectController().Save(Program.data.Project);
                ProjectManager.SaveProjectData();
                LoadProject();
                MessageBox.Show("Successful importation", "Foca", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                cmbProject.SelectedIndex = cmbProject.Items.Count - 1;
                cmbProject_SelectionChangeCommitted(null, null);
            }
            else
                MessageBox.Show("Importation Cancel", "Foca", MessageBoxButtons.OK, MessageBoxIcon.Stop);

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                var dataExport = new Data();
                UpdateItemsToExport();
                Program.data.Project.Id = 0;
                dataExport.Project = Program.data.Project;

                if (Program.data.files.Items.Any()) dataExport.files = Program.data.files;
                if (Program.data.Ips.Items.Any()) dataExport.Ips = Program.data.Ips;
                if (Program.data.domains.Items.Any()) dataExport.domains = Program.data.domains;
                if (Program.data.relations.Items.Any()) dataExport.relations = Program.data.relations;
                if (Program.data.computers.Items.Any()) dataExport.computers = Program.data.computers;
                if (Program.data.computerIPs.Items.Any()) dataExport.computerIPs = Program.data.computerIPs;
                if (Program.data.computerDomains.Items.Any()) dataExport.computerDomains = Program.data.computerDomains;

                var result = JsonConvert.SerializeObject(dataExport);

                var sfd = new SaveFileDialog { Filter = "JSON files (*.json)|*.json" };

                if (sfd.ShowDialog() != DialogResult.OK) return;

                using (var sw = File.CreateText(sfd.FileName))
                {
                    sw.WriteLine(result);
                }

                MessageBox.Show("Successful Exportation", "Foca");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static void UpdateItemsToExport()
        {
            ProjectManager.LoadProjectDataController(Program.data.Project.Id);
            Program.data.files.Items.ToList().ForEach(x => { x.Id = 0; x.IdProject = 0; });
            Program.data.Ips.Items.ToList().ForEach(x => { x.Id = 0; x.IdProject = 0; });
            Program.data.domains.Items.ToList().ForEach(x => { x.Id = 0; x.IdProject = 0; });
            Program.data.relations.Items.ToList().ForEach(x => { x.Id = 0; x.IdProject = 0; });
            Program.data.computers.Items.ToList().ForEach(x => { x.Id = 0; x.IdProject = 0; });
            Program.data.computerIPs.Items.ToList().ForEach(x => { x.Id = 0; x.IdProject = 0; });
            Program.data.computerDomains.Items.ToList().ForEach(x => { x.Id = 0; x.IdProject = 0; });
        }
    }
}