using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace FOCA
{
    public partial class FormLoadSave : Form
    {
        public enum Action
        {
            Loading,
            Saving
        }

        private Action _action;

        public FormLoadSave(Form frmParent)
        {
            InitializeComponent();
            Left = frmParent.Left + (frmParent.Width/2) - Width/2;
            Top = frmParent.Top + (frmParent.Height/2) - Height/2;
        }

        public Action action
        {
            set
            {
                lblTitle.Text = $"{(value == Action.Loading ? "Loading" : "Saving")} project, please wait!";
                Text = $"{Application.ProductName} - {(value == Action.Loading ? "Loading" : "Saving")}...";
                _action = value;
            }
            get { return _action; }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value == progressBar1.Maximum)
                progressBar1.Value = 0;
            else
                progressBar1.PerformStep();

            lblLinksValue.Text = Program.FormMainInstance.panelMetadataSearch.listViewDocuments.Items.Count.ToString();
            lblDocumentsValue.Text = Program.FormMainInstance.TreeViewMetadataReturnAllDocuments().Count().ToString();

            var lstDominios = Program.data.GetDomains();
            ulong numberMutexFiles = 0;
            ulong numberFolders = 0;

            foreach (var strDom in lstDominios)
            {
                var dom = Program.data.GetDomain(strDom);
                numberMutexFiles += (ulong) dom.map.BackupModifiedFilenames.Count;
                numberFolders += (ulong) dom.map.Folders.Count;
            }

            lblMutexValue.Text = numberMutexFiles.ToString();
            lblFoldersValue.Text = numberFolders.ToString();

            lblComputersValue.Text = Program.data.computers.Items.Count.ToString();
            lblDomainsValue.Text = Program.data.domains.Items.Count.ToString();
            lblIpsValue.Text = Program.data.Ips.Items.Count.ToString();
        }

        private void FormLoadSave_Load(object sender, EventArgs e)
        {
            var t = new Thread(LoadSaveProjectProgrressThread) {IsBackground = true};
            t.Start();
        }

        /// <summary>
        /// Show load or save progress to the user
        /// </summary>
        private void LoadSaveProjectProgrressThread()
        {
            switch (action)
            {
                case Action.Loading:
                    try
                    {
                        Program.FormMainInstance.ProjectManager.LoadProjectDelegate((string) Tag);
                        Invoke(new MethodInvoker(delegate
                        {
                            DialogResult = DialogResult.OK;
                        }));
                    }
                    catch (Exception ex)
                    {
                        timer1.Enabled = false;
                        MessageBox.Show(@"Project is corrupted or old: " + ex.Message, Text, MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        Invoke(new MethodInvoker(delegate { this.DialogResult = DialogResult.Abort; }));
                    }
                    break;
                case Action.Saving:
                    try
                    {
                        Program.FormMainInstance.ProjectManager.SaveProjectDelegate();
                        Invoke(new MethodInvoker(delegate { DialogResult = DialogResult.OK; }));
                    }
                    catch (Exception ex)
                    {
                        timer1.Enabled = false;
                        Invoke(new MethodInvoker(delegate
                        {
                            MessageBox.Show(@"Error saving project!, reason: " + ex.Message + @", " + ex.Source,
                                this.Text,
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            this.DialogResult = DialogResult.Abort;
                        }));
                    }
                    break;
            }
        }
    }
}