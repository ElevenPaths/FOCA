namespace FOCA
{
    partial class FormLoadSave
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLoadSave));
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblTitle = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblComputersValue = new System.Windows.Forms.Label();
            this.lblIpsValue = new System.Windows.Forms.Label();
            this.lblDomainsValue = new System.Windows.Forms.Label();
            this.lblComputers = new System.Windows.Forms.Label();
            this.lblIps = new System.Windows.Forms.Label();
            this.lblDomains = new System.Windows.Forms.Label();
            this.lblFoldersValue = new System.Windows.Forms.Label();
            this.lblFolders = new System.Windows.Forms.Label();
            this.lblMutexValue = new System.Windows.Forms.Label();
            this.lblMutex = new System.Windows.Forms.Label();
            this.lblDocumentsValue = new System.Windows.Forms.Label();
            this.lblLinksValue = new System.Windows.Forms.Label();
            this.lblDocuments = new System.Windows.Forms.Label();
            this.lblLinks = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 29);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(155, 23);
            this.progressBar1.Step = 2;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(16, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(142, 13);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Loading project, please wait!";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lblComputersValue);
            this.panel1.Controls.Add(this.lblIpsValue);
            this.panel1.Controls.Add(this.lblDomainsValue);
            this.panel1.Controls.Add(this.lblComputers);
            this.panel1.Controls.Add(this.lblIps);
            this.panel1.Controls.Add(this.lblDomains);
            this.panel1.Controls.Add(this.lblFoldersValue);
            this.panel1.Controls.Add(this.lblFolders);
            this.panel1.Controls.Add(this.lblMutexValue);
            this.panel1.Controls.Add(this.lblMutex);
            this.panel1.Controls.Add(this.lblDocumentsValue);
            this.panel1.Controls.Add(this.lblLinksValue);
            this.panel1.Controls.Add(this.lblDocuments);
            this.panel1.Controls.Add(this.lblLinks);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(179, 209);
            this.panel1.TabIndex = 2;
            // 
            // lblComputersValue
            // 
            this.lblComputersValue.AutoSize = true;
            this.lblComputersValue.Location = new System.Drawing.Point(116, 184);
            this.lblComputersValue.Name = "lblComputersValue";
            this.lblComputersValue.Size = new System.Drawing.Size(13, 13);
            this.lblComputersValue.TabIndex = 17;
            this.lblComputersValue.Text = "0";
            // 
            // lblIpsValue
            // 
            this.lblIpsValue.AutoSize = true;
            this.lblIpsValue.Location = new System.Drawing.Point(116, 164);
            this.lblIpsValue.Name = "lblIpsValue";
            this.lblIpsValue.Size = new System.Drawing.Size(13, 13);
            this.lblIpsValue.TabIndex = 16;
            this.lblIpsValue.Text = "0";
            // 
            // lblDomainsValue
            // 
            this.lblDomainsValue.AutoSize = true;
            this.lblDomainsValue.Location = new System.Drawing.Point(116, 144);
            this.lblDomainsValue.Name = "lblDomainsValue";
            this.lblDomainsValue.Size = new System.Drawing.Size(13, 13);
            this.lblDomainsValue.TabIndex = 15;
            this.lblDomainsValue.Text = "0";
            // 
            // lblComputers
            // 
            this.lblComputers.AutoSize = true;
            this.lblComputers.Location = new System.Drawing.Point(8, 182);
            this.lblComputers.Name = "lblComputers";
            this.lblComputers.Size = new System.Drawing.Size(57, 13);
            this.lblComputers.TabIndex = 14;
            this.lblComputers.Text = "Computers";
            // 
            // lblIps
            // 
            this.lblIps.AutoSize = true;
            this.lblIps.Location = new System.Drawing.Point(8, 162);
            this.lblIps.Name = "lblIps";
            this.lblIps.Size = new System.Drawing.Size(22, 13);
            this.lblIps.TabIndex = 13;
            this.lblIps.Text = "IPs";
            // 
            // lblDomains
            // 
            this.lblDomains.AutoSize = true;
            this.lblDomains.Location = new System.Drawing.Point(8, 142);
            this.lblDomains.Name = "lblDomains";
            this.lblDomains.Size = new System.Drawing.Size(48, 13);
            this.lblDomains.TabIndex = 12;
            this.lblDomains.Text = "Domains";
            // 
            // lblFoldersValue
            // 
            this.lblFoldersValue.AutoSize = true;
            this.lblFoldersValue.Location = new System.Drawing.Point(115, 122);
            this.lblFoldersValue.Name = "lblFoldersValue";
            this.lblFoldersValue.Size = new System.Drawing.Size(13, 13);
            this.lblFoldersValue.TabIndex = 10;
            this.lblFoldersValue.Text = "0";
            // 
            // lblFolders
            // 
            this.lblFolders.AutoSize = true;
            this.lblFolders.Location = new System.Drawing.Point(8, 122);
            this.lblFolders.Name = "lblFolders";
            this.lblFolders.Size = new System.Drawing.Size(68, 13);
            this.lblFolders.TabIndex = 8;
            this.lblFolders.Text = "Folders (http)";
            // 
            // lblMutexValue
            // 
            this.lblMutexValue.AutoSize = true;
            this.lblMutexValue.Location = new System.Drawing.Point(115, 102);
            this.lblMutexValue.Name = "lblMutexValue";
            this.lblMutexValue.Size = new System.Drawing.Size(13, 13);
            this.lblMutexValue.TabIndex = 7;
            this.lblMutexValue.Text = "0";
            // 
            // lblMutex
            // 
            this.lblMutex.AutoSize = true;
            this.lblMutex.Location = new System.Drawing.Point(8, 102);
            this.lblMutex.Name = "lblMutex";
            this.lblMutex.Size = new System.Drawing.Size(57, 13);
            this.lblMutex.TabIndex = 6;
            this.lblMutex.Text = "Mutex files";
            // 
            // lblDocumentsValue
            // 
            this.lblDocumentsValue.AutoSize = true;
            this.lblDocumentsValue.Location = new System.Drawing.Point(115, 82);
            this.lblDocumentsValue.Name = "lblDocumentsValue";
            this.lblDocumentsValue.Size = new System.Drawing.Size(13, 13);
            this.lblDocumentsValue.TabIndex = 5;
            this.lblDocumentsValue.Text = "0";
            // 
            // lblLinksValue
            // 
            this.lblLinksValue.AutoSize = true;
            this.lblLinksValue.Location = new System.Drawing.Point(115, 62);
            this.lblLinksValue.Name = "lblLinksValue";
            this.lblLinksValue.Size = new System.Drawing.Size(13, 13);
            this.lblLinksValue.TabIndex = 4;
            this.lblLinksValue.Text = "0";
            // 
            // lblDocuments
            // 
            this.lblDocuments.AutoSize = true;
            this.lblDocuments.Location = new System.Drawing.Point(8, 82);
            this.lblDocuments.Name = "lblDocuments";
            this.lblDocuments.Size = new System.Drawing.Size(64, 13);
            this.lblDocuments.TabIndex = 1;
            this.lblDocuments.Text = "Documents:";
            // 
            // lblLinks
            // 
            this.lblLinks.AutoSize = true;
            this.lblLinks.Location = new System.Drawing.Point(8, 62);
            this.lblLinks.Name = "lblLinks";
            this.lblLinks.Size = new System.Drawing.Size(35, 13);
            this.lblLinks.TabIndex = 0;
            this.lblLinks.Text = "Links:";
            // 
            // FormLoadSave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(179, 209);
            this.ControlBox = false;
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormLoadSave";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "FOCA - Loading...";
            this.Load += new System.EventHandler(this.FormLoadSave_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblDocuments;
        private System.Windows.Forms.Label lblLinks;
        private System.Windows.Forms.Label lblDocumentsValue;
        private System.Windows.Forms.Label lblLinksValue;
        private System.Windows.Forms.Label lblMutexValue;
        private System.Windows.Forms.Label lblMutex;
        private System.Windows.Forms.Label lblFolders;
        private System.Windows.Forms.Label lblFoldersValue;
        private System.Windows.Forms.Label lblComputers;
        private System.Windows.Forms.Label lblIps;
        private System.Windows.Forms.Label lblDomains;
        private System.Windows.Forms.Label lblComputersValue;
        private System.Windows.Forms.Label lblIpsValue;
        private System.Windows.Forms.Label lblDomainsValue;
    }
}