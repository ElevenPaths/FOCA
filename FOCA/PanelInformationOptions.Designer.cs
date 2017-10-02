namespace FOCA
{
    partial class PanelInformationOptions
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelInformationOptions));
            this.TabOptions = new System.Windows.Forms.TabControl();
            this.tabPageRecon = new System.Windows.Forms.TabPage();
            this.btnTechnologyRecon = new System.Windows.Forms.Button();
            this.tabPageCrawling = new System.Windows.Forms.TabPage();
            this.btnAllLinksDuckduckgo = new System.Windows.Forms.Button();
            this.btnAllLinksGoogle = new System.Windows.Forms.Button();
            this.btnAllLinksBing = new System.Windows.Forms.Button();
            this.tabPageFiles = new System.Windows.Forms.TabPage();
            this.panelSearchConfiguration = new System.Windows.Forms.Panel();
            this.lblNone = new System.Windows.Forms.Label();
            this.lblFilesSearchStatus = new System.Windows.Forms.Label();
            this.lblAll = new System.Windows.Forms.Label();
            this.tbnSearchFiles = new System.Windows.Forms.Button();
            this.checkedListBoxExtensions = new System.Windows.Forms.CheckedListBox();
            this.chkBing = new System.Windows.Forms.CheckBox();
            this.chkGoogle = new System.Windows.Forms.CheckBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.lboLog = new System.Windows.Forms.ListBox();
            this.TabOptions.SuspendLayout();
            this.tabPageRecon.SuspendLayout();
            this.tabPageCrawling.SuspendLayout();
            this.tabPageFiles.SuspendLayout();
            this.panelSearchConfiguration.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.SuspendLayout();
            // 
            // TabOptions
            // 
            this.TabOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TabOptions.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.TabOptions.Controls.Add(this.tabPageRecon);
            this.TabOptions.Controls.Add(this.tabPageCrawling);
            this.TabOptions.Controls.Add(this.tabPageFiles);
            this.TabOptions.Controls.Add(this.tabPage6);
            this.TabOptions.Location = new System.Drawing.Point(8, 5);
            this.TabOptions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TabOptions.Name = "TabOptions";
            this.TabOptions.SelectedIndex = 0;
            this.TabOptions.Size = new System.Drawing.Size(885, 171);
            this.TabOptions.TabIndex = 93;
            // 
            // tabPageRecon
            // 
            this.tabPageRecon.Controls.Add(this.btnTechnologyRecon);
            this.tabPageRecon.Location = new System.Drawing.Point(4, 32);
            this.tabPageRecon.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageRecon.Name = "tabPageRecon";
            this.tabPageRecon.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageRecon.Size = new System.Drawing.Size(877, 135);
            this.tabPageRecon.TabIndex = 0;
            this.tabPageRecon.Text = "Technology recognition";
            this.tabPageRecon.UseVisualStyleBackColor = true;
            // 
            // btnTechnologyRecon
            // 
            this.btnTechnologyRecon.Enabled = false;
            this.btnTechnologyRecon.Image = ((System.Drawing.Image)(resources.GetObject("btnTechnologyRecon.Image")));
            this.btnTechnologyRecon.Location = new System.Drawing.Point(9, 9);
            this.btnTechnologyRecon.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnTechnologyRecon.Name = "btnTechnologyRecon";
            this.btnTechnologyRecon.Size = new System.Drawing.Size(159, 63);
            this.btnTechnologyRecon.TabIndex = 86;
            this.btnTechnologyRecon.Text = "Technology Recognition";
            this.btnTechnologyRecon.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTechnologyRecon.UseVisualStyleBackColor = true;
            this.btnTechnologyRecon.Click += new System.EventHandler(this.BtTechRecog_Click);
            // 
            // tabPageCrawling
            // 
            this.tabPageCrawling.Controls.Add(this.btnAllLinksDuckduckgo);
            this.tabPageCrawling.Controls.Add(this.btnAllLinksGoogle);
            this.tabPageCrawling.Controls.Add(this.btnAllLinksBing);
            this.tabPageCrawling.Location = new System.Drawing.Point(4, 32);
            this.tabPageCrawling.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageCrawling.Name = "tabPageCrawling";
            this.tabPageCrawling.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageCrawling.Size = new System.Drawing.Size(877, 135);
            this.tabPageCrawling.TabIndex = 1;
            this.tabPageCrawling.Text = "Crawling";
            this.tabPageCrawling.UseVisualStyleBackColor = true;
            // 
            // btnAllLinksDuckduckgo
            // 
            this.btnAllLinksDuckduckgo.Image = ((System.Drawing.Image)(resources.GetObject("btnAllLinksDuckduckgo.Image")));
            this.btnAllLinksDuckduckgo.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAllLinksDuckduckgo.Location = new System.Drawing.Point(346, 11);
            this.btnAllLinksDuckduckgo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAllLinksDuckduckgo.Name = "btnAllLinksDuckduckgo";
            this.btnAllLinksDuckduckgo.Size = new System.Drawing.Size(159, 63);
            this.btnAllLinksDuckduckgo.TabIndex = 91;
            this.btnAllLinksDuckduckgo.Text = "Duckduckgo Crawling";
            this.btnAllLinksDuckduckgo.UseVisualStyleBackColor = true;
            this.btnAllLinksDuckduckgo.Click += new System.EventHandler(this.btAllLinksDuckduckGo_Click);
            // 
            // btnAllLinksGoogle
            // 
            this.btnAllLinksGoogle.Enabled = false;
            this.btnAllLinksGoogle.Image = ((System.Drawing.Image)(resources.GetObject("btnAllLinksGoogle.Image")));
            this.btnAllLinksGoogle.Location = new System.Drawing.Point(9, 9);
            this.btnAllLinksGoogle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAllLinksGoogle.Name = "btnAllLinksGoogle";
            this.btnAllLinksGoogle.Size = new System.Drawing.Size(159, 63);
            this.btnAllLinksGoogle.TabIndex = 89;
            this.btnAllLinksGoogle.Text = "Google crawling";
            this.btnAllLinksGoogle.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAllLinksGoogle.UseVisualStyleBackColor = true;
            this.btnAllLinksGoogle.Click += new System.EventHandler(this.BtAllLinksGoogle_Click);
            // 
            // btnAllLinksBing
            // 
            this.btnAllLinksBing.Enabled = false;
            this.btnAllLinksBing.Image = ((System.Drawing.Image)(resources.GetObject("btnAllLinksBing.Image")));
            this.btnAllLinksBing.Location = new System.Drawing.Point(177, 9);
            this.btnAllLinksBing.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAllLinksBing.Name = "btnAllLinksBing";
            this.btnAllLinksBing.Size = new System.Drawing.Size(159, 63);
            this.btnAllLinksBing.TabIndex = 90;
            this.btnAllLinksBing.Text = "Bing crawling";
            this.btnAllLinksBing.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAllLinksBing.UseVisualStyleBackColor = true;
            this.btnAllLinksBing.Click += new System.EventHandler(this.btAllLinksBing_Click);
            // 
            // tabPageFiles
            // 
            this.tabPageFiles.Controls.Add(this.panelSearchConfiguration);
            this.tabPageFiles.Location = new System.Drawing.Point(4, 32);
            this.tabPageFiles.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPageFiles.Name = "tabPageFiles";
            this.tabPageFiles.Size = new System.Drawing.Size(877, 135);
            this.tabPageFiles.TabIndex = 3;
            this.tabPageFiles.Text = "Files";
            this.tabPageFiles.UseVisualStyleBackColor = true;
            // 
            // panelSearchConfiguration
            // 
            this.panelSearchConfiguration.Controls.Add(this.lblNone);
            this.panelSearchConfiguration.Controls.Add(this.lblFilesSearchStatus);
            this.panelSearchConfiguration.Controls.Add(this.lblAll);
            this.panelSearchConfiguration.Controls.Add(this.tbnSearchFiles);
            this.panelSearchConfiguration.Controls.Add(this.checkedListBoxExtensions);
            this.panelSearchConfiguration.Controls.Add(this.chkBing);
            this.panelSearchConfiguration.Controls.Add(this.chkGoogle);
            this.panelSearchConfiguration.Location = new System.Drawing.Point(4, 5);
            this.panelSearchConfiguration.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelSearchConfiguration.Name = "panelSearchConfiguration";
            this.panelSearchConfiguration.Size = new System.Drawing.Size(861, 98);
            this.panelSearchConfiguration.TabIndex = 91;
            // 
            // lblNone
            // 
            this.lblNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNone.AutoSize = true;
            this.lblNone.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblNone.Location = new System.Drawing.Point(654, 69);
            this.lblNone.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNone.Name = "lblNone";
            this.lblNone.Size = new System.Drawing.Size(47, 20);
            this.lblNone.TabIndex = 93;
            this.lblNone.Text = "None";
            this.lblNone.Click += new System.EventHandler(this.lbNone_Click);
            // 
            // lblFilesSearchStatus
            // 
            this.lblFilesSearchStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFilesSearchStatus.AutoSize = true;
            this.lblFilesSearchStatus.Location = new System.Drawing.Point(734, 6);
            this.lblFilesSearchStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFilesSearchStatus.Name = "lblFilesSearchStatus";
            this.lblFilesSearchStatus.Size = new System.Drawing.Size(102, 20);
            this.lblFilesSearchStatus.TabIndex = 4;
            this.lblFilesSearchStatus.Text = "Status: None";
            // 
            // lblAll
            // 
            this.lblAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAll.AutoSize = true;
            this.lblAll.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblAll.Location = new System.Drawing.Point(654, 37);
            this.lblAll.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAll.Name = "lblAll";
            this.lblAll.Size = new System.Drawing.Size(26, 20);
            this.lblAll.TabIndex = 92;
            this.lblAll.Text = "All";
            this.lblAll.Click += new System.EventHandler(this.lbAll_Click);
            // 
            // tbnSearchFiles
            // 
            this.tbnSearchFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbnSearchFiles.Image = ((System.Drawing.Image)(resources.GetObject("tbnSearchFiles.Image")));
            this.tbnSearchFiles.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tbnSearchFiles.Location = new System.Drawing.Point(738, 51);
            this.tbnSearchFiles.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbnSearchFiles.Name = "tbnSearchFiles";
            this.tbnSearchFiles.Size = new System.Drawing.Size(118, 38);
            this.tbnSearchFiles.TabIndex = 3;
            this.tbnSearchFiles.Text = "Search";
            this.tbnSearchFiles.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.tbnSearchFiles.UseVisualStyleBackColor = true;
            this.tbnSearchFiles.Click += new System.EventHandler(this.btnSearchAll_Click);
            // 
            // checkedListBoxExtensions
            // 
            this.checkedListBoxExtensions.CheckOnClick = true;
            this.checkedListBoxExtensions.ColumnWidth = 50;
            this.checkedListBoxExtensions.FormattingEnabled = true;
            this.checkedListBoxExtensions.IntegralHeight = false;
            this.checkedListBoxExtensions.Items.AddRange(new object[] {
            "doc",
            "ppt",
            "pps",
            "xls",
            "docx",
            "pptx",
            "ppsx",
            "xlsx",
            "sxw",
            "sxc",
            "sxi",
            "odt",
            "ods",
            "odg",
            "odp",
            "pdf",
            "wpd",
            "svg",
            "svgz",
            "indd",
            "rdp",
            "ica"});
            this.checkedListBoxExtensions.Location = new System.Drawing.Point(104, 0);
            this.checkedListBoxExtensions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkedListBoxExtensions.MultiColumn = true;
            this.checkedListBoxExtensions.Name = "checkedListBoxExtensions";
            this.checkedListBoxExtensions.Size = new System.Drawing.Size(540, 90);
            this.checkedListBoxExtensions.TabIndex = 0;
            this.checkedListBoxExtensions.SelectedValueChanged += new System.EventHandler(this.SaveValuesFromDomain);
            // 
            // chkBing
            // 
            this.chkBing.AutoSize = true;
            this.chkBing.Checked = true;
            this.chkBing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBing.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkBing.Location = new System.Drawing.Point(4, 31);
            this.chkBing.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkBing.Name = "chkBing";
            this.chkBing.Size = new System.Drawing.Size(67, 24);
            this.chkBing.TabIndex = 1;
            this.chkBing.Text = "Bing";
            this.chkBing.UseVisualStyleBackColor = true;
            this.chkBing.Click += new System.EventHandler(this.SaveValuesFromDomain);
            // 
            // chkGoogle
            // 
            this.chkGoogle.AutoSize = true;
            this.chkGoogle.Checked = true;
            this.chkGoogle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGoogle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkGoogle.ImageKey = "(none)";
            this.chkGoogle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkGoogle.Location = new System.Drawing.Point(4, 5);
            this.chkGoogle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkGoogle.Name = "chkGoogle";
            this.chkGoogle.Size = new System.Drawing.Size(87, 24);
            this.chkGoogle.TabIndex = 0;
            this.chkGoogle.Text = "Google";
            this.chkGoogle.UseVisualStyleBackColor = true;
            this.chkGoogle.Click += new System.EventHandler(this.SaveValuesFromDomain);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.lboLog);
            this.tabPage6.Location = new System.Drawing.Point(4, 32);
            this.tabPage6.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(877, 135);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Log";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // lboLog
            // 
            this.lboLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lboLog.FormattingEnabled = true;
            this.lboLog.ItemHeight = 20;
            this.lboLog.Location = new System.Drawing.Point(0, 0);
            this.lboLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lboLog.Name = "lboLog";
            this.lboLog.ScrollAlwaysVisible = true;
            this.lboLog.Size = new System.Drawing.Size(877, 135);
            this.lboLog.TabIndex = 94;
            // 
            // PanelInformationOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TabOptions);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "PanelInformationOptions";
            this.Size = new System.Drawing.Size(896, 180);
            this.TabOptions.ResumeLayout(false);
            this.tabPageRecon.ResumeLayout(false);
            this.tabPageCrawling.ResumeLayout(false);
            this.tabPageFiles.ResumeLayout(false);
            this.panelSearchConfiguration.ResumeLayout(false);
            this.panelSearchConfiguration.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TabControl TabOptions;
        private System.Windows.Forms.TabPage tabPageRecon;
        public System.Windows.Forms.Button btnTechnologyRecon;
        private System.Windows.Forms.TabPage tabPageCrawling;
        public System.Windows.Forms.Button btnAllLinksGoogle;
        public System.Windows.Forms.Button btnAllLinksBing;
        private System.Windows.Forms.TabPage tabPageFiles;
        public System.Windows.Forms.Panel panelSearchConfiguration;
        public System.Windows.Forms.Label lblFilesSearchStatus;
        public System.Windows.Forms.Button tbnSearchFiles;
        public System.Windows.Forms.CheckedListBox checkedListBoxExtensions;
        public System.Windows.Forms.CheckBox chkBing;
        public System.Windows.Forms.CheckBox chkGoogle;
        private System.Windows.Forms.ListBox lboLog;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.Label lblNone;
        private System.Windows.Forms.Label lblAll;
        private System.Windows.Forms.Button btnAllLinksDuckduckgo;
    }
}
