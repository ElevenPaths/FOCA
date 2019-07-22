namespace FOCA
{
    partial class PanelProject
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelProject));
            this.panelCenter = new System.Windows.Forms.Panel();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.cmbProject = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDate = new System.Windows.Forms.DateTimePicker();
            this.picFOCA = new System.Windows.Forms.PictureBox();
            this.txtAlternativeDomains = new System.Windows.Forms.TextBox();
            this.lblAlternativeDomains = new System.Windows.Forms.Label();
            this.lblNotes = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSelectDirectory = new System.Windows.Forms.Button();
            this.lblDocumentFolder = new System.Windows.Forms.Label();
            this.txtFolderDocuments = new System.Windows.Forms.TextBox();
            this.lblSiteDomain = new System.Windows.Forms.Label();
            this.txtDomainWebsite = new System.Windows.Forms.TextBox();
            this.lblProjectName = new System.Windows.Forms.Label();
            this.txtProjectName = new System.Windows.Forms.TextBox();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.fbdMain = new System.Windows.Forms.FolderBrowserDialog();
            this.errorPorject = new System.Windows.Forms.ErrorProvider(this.components);
            this.openFileDialogImport = new System.Windows.Forms.OpenFileDialog();
            this.panelCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFOCA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorPorject)).BeginInit();
            this.SuspendLayout();
            // 
            // panelCenter
            // 
            this.panelCenter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.panelCenter.Controls.Add(this.btnImport);
            this.panelCenter.Controls.Add(this.btnExport);
            this.panelCenter.Controls.Add(this.cmbProject);
            this.panelCenter.Controls.Add(this.label1);
            this.panelCenter.Controls.Add(this.txtDate);
            this.panelCenter.Controls.Add(this.picFOCA);
            this.panelCenter.Controls.Add(this.txtAlternativeDomains);
            this.panelCenter.Controls.Add(this.lblAlternativeDomains);
            this.panelCenter.Controls.Add(this.lblNotes);
            this.panelCenter.Controls.Add(this.lblDate);
            this.panelCenter.Controls.Add(this.btnCancel);
            this.panelCenter.Controls.Add(this.btnSelectDirectory);
            this.panelCenter.Controls.Add(this.lblDocumentFolder);
            this.panelCenter.Controls.Add(this.txtFolderDocuments);
            this.panelCenter.Controls.Add(this.lblSiteDomain);
            this.panelCenter.Controls.Add(this.txtDomainWebsite);
            this.panelCenter.Controls.Add(this.lblProjectName);
            this.panelCenter.Controls.Add(this.txtProjectName);
            this.panelCenter.Controls.Add(this.txtNotes);
            this.panelCenter.Controls.Add(this.btnCreate);
            this.panelCenter.Location = new System.Drawing.Point(4, 0);
            this.panelCenter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(890, 580);
            this.panelCenter.TabIndex = 55;
            // 
            // btnImport
            // 
            this.btnImport.Image = ((System.Drawing.Image)(resources.GetObject("btnImport.Image")));
            this.btnImport.Location = new System.Drawing.Point(162, 531);
            this.btnImport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(134, 40);
            this.btnImport.TabIndex = 9;
            this.btnImport.Text = "Import";
            this.btnImport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnExport
            // 
            this.btnExport.Image = ((System.Drawing.Image)(resources.GetObject("btnExport.Image")));
            this.btnExport.Location = new System.Drawing.Point(304, 531);
            this.btnExport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(134, 40);
            this.btnExport.TabIndex = 94;
            this.btnExport.Text = "Export";
            this.btnExport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Visible = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // cmbProject
            // 
            this.cmbProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbProject.FormattingEnabled = true;
            this.cmbProject.Location = new System.Drawing.Point(540, 173);
            this.cmbProject.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cmbProject.Name = "cmbProject";
            this.cmbProject.Size = new System.Drawing.Size(324, 28);
            this.cmbProject.TabIndex = 0;
            this.cmbProject.SelectionChangeCommitted += new System.EventHandler(this.cmbProject_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(14, 181);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 20);
            this.label1.TabIndex = 92;
            this.label1.Text = "Select project";
            // 
            // txtDate
            // 
            this.txtDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDate.Location = new System.Drawing.Point(540, 412);
            this.txtDate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(322, 26);
            this.txtDate.TabIndex = 6;
            // 
            // picFOCA
            // 
            this.picFOCA.BackColor = System.Drawing.SystemColors.Control;
            this.picFOCA.Image = ((System.Drawing.Image)(resources.GetObject("picFOCA.Image")));
            this.picFOCA.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.picFOCA.Location = new System.Drawing.Point(6, 4);
            this.picFOCA.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picFOCA.Name = "picFOCA";
            this.picFOCA.Size = new System.Drawing.Size(303, 163);
            this.picFOCA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picFOCA.TabIndex = 87;
            this.picFOCA.TabStop = false;
            // 
            // txtAlternativeDomains
            // 
            this.txtAlternativeDomains.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAlternativeDomains.Location = new System.Drawing.Point(540, 304);
            this.txtAlternativeDomains.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtAlternativeDomains.Multiline = true;
            this.txtAlternativeDomains.Name = "txtAlternativeDomains";
            this.txtAlternativeDomains.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAlternativeDomains.Size = new System.Drawing.Size(322, 52);
            this.txtAlternativeDomains.TabIndex = 3;
            this.txtAlternativeDomains.WordWrap = false;
            // 
            // lblAlternativeDomains
            // 
            this.lblAlternativeDomains.AutoSize = true;
            this.lblAlternativeDomains.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblAlternativeDomains.Location = new System.Drawing.Point(14, 308);
            this.lblAlternativeDomains.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAlternativeDomains.Name = "lblAlternativeDomains";
            this.lblAlternativeDomains.Size = new System.Drawing.Size(148, 20);
            this.lblAlternativeDomains.TabIndex = 70;
            this.lblAlternativeDomains.Text = "Alternative domains";
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblNotes.Location = new System.Drawing.Point(14, 462);
            this.lblNotes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(102, 20);
            this.lblNotes.TabIndex = 69;
            this.lblNotes.Text = "Project notes";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDate.Location = new System.Drawing.Point(14, 419);
            this.lblDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(94, 20);
            this.lblDate.TabIndex = 68;
            this.lblDate.Text = "Project date";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(741, 531);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(134, 40);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSelectDirectory
            // 
            this.btnSelectDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectDirectory.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectDirectory.Image")));
            this.btnSelectDirectory.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSelectDirectory.Location = new System.Drawing.Point(828, 368);
            this.btnSelectDirectory.Margin = new System.Windows.Forms.Padding(0);
            this.btnSelectDirectory.Name = "btnSelectDirectory";
            this.btnSelectDirectory.Size = new System.Drawing.Size(36, 35);
            this.btnSelectDirectory.TabIndex = 5;
            this.btnSelectDirectory.UseCompatibleTextRendering = true;
            this.btnSelectDirectory.UseVisualStyleBackColor = true;
            this.btnSelectDirectory.Click += new System.EventHandler(this.buttonSelectFolder_Click);
            // 
            // lblDocumentFolder
            // 
            this.lblDocumentFolder.AutoSize = true;
            this.lblDocumentFolder.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDocumentFolder.Location = new System.Drawing.Point(14, 376);
            this.lblDocumentFolder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDocumentFolder.Name = "lblDocumentFolder";
            this.lblDocumentFolder.Size = new System.Drawing.Size(221, 20);
            this.lblDocumentFolder.TabIndex = 67;
            this.lblDocumentFolder.Text = "Folder where to save documents";
            // 
            // txtFolderDocuments
            // 
            this.txtFolderDocuments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolderDocuments.Location = new System.Drawing.Point(540, 371);
            this.txtFolderDocuments.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFolderDocuments.Name = "txtFolderDocuments";
            this.txtFolderDocuments.Size = new System.Drawing.Size(278, 26);
            this.txtFolderDocuments.TabIndex = 4;
            // 
            // lblSiteDomain
            // 
            this.lblSiteDomain.AutoSize = true;
            this.lblSiteDomain.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSiteDomain.Location = new System.Drawing.Point(14, 265);
            this.lblSiteDomain.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSiteDomain.Name = "lblSiteDomain";
            this.lblSiteDomain.Size = new System.Drawing.Size(122, 20);
            this.lblSiteDomain.TabIndex = 66;
            this.lblSiteDomain.Text = "Domain website";
            // 
            // txtDomainWebsite
            // 
            this.txtDomainWebsite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDomainWebsite.Location = new System.Drawing.Point(540, 260);
            this.txtDomainWebsite.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDomainWebsite.Name = "txtDomainWebsite";
            this.txtDomainWebsite.Size = new System.Drawing.Size(322, 26);
            this.txtDomainWebsite.TabIndex = 2;
            this.txtDomainWebsite.TextChanged += new System.EventHandler(this.txtDomainWebsite_TextChanged);
            // 
            // lblProjectName
            // 
            this.lblProjectName.AutoSize = true;
            this.lblProjectName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblProjectName.Location = new System.Drawing.Point(14, 222);
            this.lblProjectName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblProjectName.Name = "lblProjectName";
            this.lblProjectName.Size = new System.Drawing.Size(102, 20);
            this.lblProjectName.TabIndex = 65;
            this.lblProjectName.Text = "Project name";
            // 
            // txtProjectName
            // 
            this.txtProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProjectName.Location = new System.Drawing.Point(540, 217);
            this.txtProjectName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtProjectName.Name = "txtProjectName";
            this.txtProjectName.Size = new System.Drawing.Size(322, 26);
            this.txtProjectName.TabIndex = 1;
            this.txtProjectName.TextChanged += new System.EventHandler(this.txtProjectName_TextChanged);
            // 
            // txtNotes
            // 
            this.txtNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNotes.Location = new System.Drawing.Point(540, 457);
            this.txtNotes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(322, 62);
            this.txtNotes.TabIndex = 7;
            // 
            // btnCreate
            // 
            this.btnCreate.Image = ((System.Drawing.Image)(resources.GetObject("btnCreate.Image")));
            this.btnCreate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCreate.Location = new System.Drawing.Point(18, 531);
            this.btnCreate.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(136, 40);
            this.btnCreate.TabIndex = 8;
            this.btnCreate.Text = "Create";
            this.btnCreate.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCreate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // fbdMain
            // 
            this.fbdMain.Description = "Select the folder that contains the OpenOffice files";
            // 
            // errorPorject
            // 
            this.errorPorject.BlinkRate = 0;
            this.errorPorject.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorPorject.ContainerControl = this;
            // 
            // openFileDialogImport
            // 
            this.openFileDialogImport.FileName = "openFileDialogImport";
            // 
            // PanelProject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelCenter);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(693, 585);
            this.Name = "PanelProject";
            this.Size = new System.Drawing.Size(898, 585);
            this.panelCenter.ResumeLayout(false);
            this.panelCenter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFOCA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorPorject)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelCenter;
        private System.Windows.Forms.TextBox txtAlternativeDomains;
        private System.Windows.Forms.Label lblAlternativeDomains;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnSelectDirectory;
        private System.Windows.Forms.Label lblDocumentFolder;
        private System.Windows.Forms.TextBox txtFolderDocuments;
        private System.Windows.Forms.Label lblSiteDomain;
        private System.Windows.Forms.TextBox txtDomainWebsite;
        private System.Windows.Forms.Label lblProjectName;
        private System.Windows.Forms.TextBox txtProjectName;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.FolderBrowserDialog fbdMain;
        private System.Windows.Forms.PictureBox picFOCA;
        private System.Windows.Forms.DateTimePicker txtDate;
        private System.Windows.Forms.ComboBox cmbProject;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ErrorProvider errorPorject;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.OpenFileDialog openFileDialogImport;
    }
}
