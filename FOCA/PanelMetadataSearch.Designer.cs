namespace FOCA
{
    partial class PanelMetadataSearch
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
            if (disposing && (components != null) && (Downloads != null))
            {
                Downloads.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelMetadataSearch));
            this.panelSearchConfiguration = new System.Windows.Forms.Panel();
            this.chkDuck = new System.Windows.Forms.CheckBox();
            this.lblAll = new System.Windows.Forms.Label();
            this.lblNone = new System.Windows.Forms.Label();
            this.checkedListBoxExtensions = new System.Windows.Forms.CheckedListBox();
            this.lblExtensions = new System.Windows.Forms.Label();
            this.chkBing = new System.Windows.Forms.CheckBox();
            this.chkGoogle = new System.Windows.Forms.CheckBox();
            this.lblSearchEngines = new System.Windows.Forms.Label();
            this.panelCustomSearch = new System.Windows.Forms.Panel();
            this.linkLabelCustomSearch = new System.Windows.Forms.LinkLabel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.contextMenuStripLinks = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemDownload = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDownloadAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDeleteAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemExtractAllMetadata = new System.Windows.Forms.ToolStripMenuItem();
            this.analyzeMetadataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemAddFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAddFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.addURLsFromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.linkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemOpenInBrowser = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCopyToClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.fbdMain = new System.Windows.Forms.FolderBrowserDialog();
            this.ofdAddFile = new System.Windows.Forms.OpenFileDialog();
            this.btnSearchAll = new System.Windows.Forms.Button();
            this.picFOCA = new System.Windows.Forms.PictureBox();
            this.ofdURLList = new System.Windows.Forms.OpenFileDialog();
            this.imgIcons = new System.Windows.Forms.ImageList(this.components);
            this.listViewDocuments = new FOCA.Search.ListViewEx();
            this.columnHeaderId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderURL = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDownload = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderDownloadDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderAnalyzed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderModifiedDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelSearchConfiguration.SuspendLayout();
            this.panelCustomSearch.SuspendLayout();
            this.contextMenuStripLinks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFOCA)).BeginInit();
            this.SuspendLayout();
            // 
            // panelSearchConfiguration
            // 
            this.panelSearchConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSearchConfiguration.Controls.Add(this.chkDuck);
            this.panelSearchConfiguration.Controls.Add(this.lblAll);
            this.panelSearchConfiguration.Controls.Add(this.lblNone);
            this.panelSearchConfiguration.Controls.Add(this.checkedListBoxExtensions);
            this.panelSearchConfiguration.Controls.Add(this.lblExtensions);
            this.panelSearchConfiguration.Controls.Add(this.chkBing);
            this.panelSearchConfiguration.Controls.Add(this.chkGoogle);
            this.panelSearchConfiguration.Controls.Add(this.lblSearchEngines);
            this.panelSearchConfiguration.Location = new System.Drawing.Point(677, 11);
            this.panelSearchConfiguration.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelSearchConfiguration.Name = "panelSearchConfiguration";
            this.panelSearchConfiguration.Size = new System.Drawing.Size(523, 142);
            this.panelSearchConfiguration.TabIndex = 0;
            // 
            // chkDuck
            // 
            this.chkDuck.AutoSize = true;
            this.chkDuck.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkDuck.Location = new System.Drawing.Point(21, 85);
            this.chkDuck.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkDuck.Name = "chkDuck";
            this.chkDuck.Size = new System.Drawing.Size(113, 24);
            this.chkDuck.TabIndex = 6;
            this.chkDuck.Text = "Duck Duck";
            this.chkDuck.UseVisualStyleBackColor = true;
            // 
            // lblAll
            // 
            this.lblAll.AutoSize = true;
            this.lblAll.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblAll.Location = new System.Drawing.Point(22, 114);
            this.lblAll.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAll.Name = "lblAll";
            this.lblAll.Size = new System.Drawing.Size(26, 20);
            this.lblAll.TabIndex = 5;
            this.lblAll.Text = "All";
            this.lblAll.Click += new System.EventHandler(this.lbAll_Click);
            // 
            // lblNone
            // 
            this.lblNone.AutoSize = true;
            this.lblNone.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblNone.Location = new System.Drawing.Point(68, 114);
            this.lblNone.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNone.Name = "lblNone";
            this.lblNone.Size = new System.Drawing.Size(47, 20);
            this.lblNone.TabIndex = 4;
            this.lblNone.Text = "None";
            this.lblNone.Click += new System.EventHandler(this.lbNone_Click);
            // 
            // checkedListBoxExtensions
            // 
            this.checkedListBoxExtensions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
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
            this.checkedListBoxExtensions.Location = new System.Drawing.Point(172, 26);
            this.checkedListBoxExtensions.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.checkedListBoxExtensions.MultiColumn = true;
            this.checkedListBoxExtensions.Name = "checkedListBoxExtensions";
            this.checkedListBoxExtensions.Size = new System.Drawing.Size(341, 106);
            this.checkedListBoxExtensions.TabIndex = 0;
            // 
            // lblExtensions
            // 
            this.lblExtensions.AutoSize = true;
            this.lblExtensions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblExtensions.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblExtensions.Location = new System.Drawing.Point(168, 6);
            this.lblExtensions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblExtensions.Name = "lblExtensions";
            this.lblExtensions.Size = new System.Drawing.Size(101, 20);
            this.lblExtensions.TabIndex = 3;
            this.lblExtensions.Text = "Extensions";
            // 
            // chkBing
            // 
            this.chkBing.AutoSize = true;
            this.chkBing.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkBing.Location = new System.Drawing.Point(21, 57);
            this.chkBing.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkBing.Name = "chkBing";
            this.chkBing.Size = new System.Drawing.Size(67, 24);
            this.chkBing.TabIndex = 1;
            this.chkBing.Text = "Bing";
            this.chkBing.UseVisualStyleBackColor = true;
            // 
            // chkGoogle
            // 
            this.chkGoogle.AutoSize = true;
            this.chkGoogle.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chkGoogle.ImageKey = "(none)";
            this.chkGoogle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkGoogle.Location = new System.Drawing.Point(21, 31);
            this.chkGoogle.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkGoogle.Name = "chkGoogle";
            this.chkGoogle.Size = new System.Drawing.Size(87, 24);
            this.chkGoogle.TabIndex = 0;
            this.chkGoogle.Text = "Google";
            this.chkGoogle.UseVisualStyleBackColor = true;
            // 
            // lblSearchEngines
            // 
            this.lblSearchEngines.AutoSize = true;
            this.lblSearchEngines.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblSearchEngines.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSearchEngines.Location = new System.Drawing.Point(16, 6);
            this.lblSearchEngines.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSearchEngines.Name = "lblSearchEngines";
            this.lblSearchEngines.Size = new System.Drawing.Size(139, 20);
            this.lblSearchEngines.TabIndex = 0;
            this.lblSearchEngines.Text = "Search engines";
            // 
            // panelCustomSearch
            // 
            this.panelCustomSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCustomSearch.Controls.Add(this.linkLabelCustomSearch);
            this.panelCustomSearch.Controls.Add(this.txtSearch);
            this.panelCustomSearch.Controls.Add(this.btnSearch);
            this.panelCustomSearch.Location = new System.Drawing.Point(12, 163);
            this.panelCustomSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelCustomSearch.Name = "panelCustomSearch";
            this.panelCustomSearch.Size = new System.Drawing.Size(1062, 56);
            this.panelCustomSearch.TabIndex = 21;
            // 
            // linkLabelCustomSearch
            // 
            this.linkLabelCustomSearch.AutoSize = true;
            this.linkLabelCustomSearch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.linkLabelCustomSearch.Location = new System.Drawing.Point(21, 19);
            this.linkLabelCustomSearch.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabelCustomSearch.Name = "linkLabelCustomSearch";
            this.linkLabelCustomSearch.Size = new System.Drawing.Size(116, 20);
            this.linkLabelCustomSearch.TabIndex = 0;
            this.linkLabelCustomSearch.TabStop = true;
            this.linkLabelCustomSearch.Text = "Custom search";
            this.linkLabelCustomSearch.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelCustomSearch_LinkClicked);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(4, 13);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(930, 26);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSearch_KeyPress);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSearch.Location = new System.Drawing.Point(952, 10);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(100, 38);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "&Search";
            this.btnSearch.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // contextMenuStripLinks
            // 
            this.contextMenuStripLinks.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStripLinks.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemDownload,
            this.toolStripMenuItemDownloadAll,
            this.toolStripSeparator4,
            this.toolStripMenuItemDelete,
            this.toolStripMenuItemDeleteAll,
            this.toolStripSeparator5,
            this.toolStripMenuItemExtractAllMetadata,
            this.analyzeMetadataToolStripMenuItem,
            this.toolStripSeparator6,
            this.toolStripMenuItemAddFile,
            this.toolStripMenuItemAddFolder,
            this.addURLsFromFileToolStripMenuItem,
            this.toolStripSeparator7,
            this.linkToolStripMenuItem});
            this.contextMenuStripLinks.Name = "contextMenuStripEnlaces";
            this.contextMenuStripLinks.Size = new System.Drawing.Size(243, 328);
            this.contextMenuStripLinks.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripLinks_Opening);
            // 
            // toolStripMenuItemDownload
            // 
            this.toolStripMenuItemDownload.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemDownload.Image")));
            this.toolStripMenuItemDownload.Name = "toolStripMenuItemDownload";
            this.toolStripMenuItemDownload.Size = new System.Drawing.Size(242, 30);
            this.toolStripMenuItemDownload.Text = "&Download";
            this.toolStripMenuItemDownload.Click += new System.EventHandler(this.toolStripMenuItemDownload_Click);
            // 
            // toolStripMenuItemDownloadAll
            // 
            this.toolStripMenuItemDownloadAll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemDownloadAll.Image")));
            this.toolStripMenuItemDownloadAll.Name = "toolStripMenuItemDownloadAll";
            this.toolStripMenuItemDownloadAll.Size = new System.Drawing.Size(242, 30);
            this.toolStripMenuItemDownloadAll.Text = "&Download All";
            this.toolStripMenuItemDownloadAll.Click += new System.EventHandler(this.toolStripMenuItemDownloadAll_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(239, 6);
            // 
            // toolStripMenuItemDelete
            // 
            this.toolStripMenuItemDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemDelete.Image")));
            this.toolStripMenuItemDelete.Name = "toolStripMenuItemDelete";
            this.toolStripMenuItemDelete.Size = new System.Drawing.Size(242, 30);
            this.toolStripMenuItemDelete.Text = "&Delete";
            this.toolStripMenuItemDelete.Click += new System.EventHandler(this.toolStripMenuItemDelete_Click);
            // 
            // toolStripMenuItemDeleteAll
            // 
            this.toolStripMenuItemDeleteAll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemDeleteAll.Image")));
            this.toolStripMenuItemDeleteAll.Name = "toolStripMenuItemDeleteAll";
            this.toolStripMenuItemDeleteAll.Size = new System.Drawing.Size(242, 30);
            this.toolStripMenuItemDeleteAll.Text = "&Delete All";
            this.toolStripMenuItemDeleteAll.Click += new System.EventHandler(this.toolStripMenuItemDeleteAll_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(239, 6);
            // 
            // toolStripMenuItemExtractAllMetadata
            // 
            this.toolStripMenuItemExtractAllMetadata.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemExtractAllMetadata.Image")));
            this.toolStripMenuItemExtractAllMetadata.Name = "toolStripMenuItemExtractAllMetadata";
            this.toolStripMenuItemExtractAllMetadata.Size = new System.Drawing.Size(242, 30);
            this.toolStripMenuItemExtractAllMetadata.Text = "Extract &Metada";
            this.toolStripMenuItemExtractAllMetadata.Click += new System.EventHandler(this.toolStripMenuItemExtractAllMetadata_Click);
            // 
            // analyzeMetadataToolStripMenuItem
            // 
            this.analyzeMetadataToolStripMenuItem.Enabled = false;
            this.analyzeMetadataToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("analyzeMetadataToolStripMenuItem.Image")));
            this.analyzeMetadataToolStripMenuItem.Name = "analyzeMetadataToolStripMenuItem";
            this.analyzeMetadataToolStripMenuItem.Size = new System.Drawing.Size(242, 30);
            this.analyzeMetadataToolStripMenuItem.Text = "Analyze Metadata";
            this.analyzeMetadataToolStripMenuItem.Click += new System.EventHandler(this.analyzeMetadataToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(239, 6);
            // 
            // toolStripMenuItemAddFile
            // 
            this.toolStripMenuItemAddFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemAddFile.Image")));
            this.toolStripMenuItemAddFile.Name = "toolStripMenuItemAddFile";
            this.toolStripMenuItemAddFile.Size = new System.Drawing.Size(242, 30);
            this.toolStripMenuItemAddFile.Text = "&Add file";
            this.toolStripMenuItemAddFile.Click += new System.EventHandler(this.toolStripMenuItemAddFile_Click);
            // 
            // toolStripMenuItemAddFolder
            // 
            this.toolStripMenuItemAddFolder.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemAddFolder.Image")));
            this.toolStripMenuItemAddFolder.Name = "toolStripMenuItemAddFolder";
            this.toolStripMenuItemAddFolder.Size = new System.Drawing.Size(242, 30);
            this.toolStripMenuItemAddFolder.Text = "&Add folder";
            this.toolStripMenuItemAddFolder.Click += new System.EventHandler(this.toolStripMenuItemAddFolder_Click);
            // 
            // addURLsFromFileToolStripMenuItem
            // 
            this.addURLsFromFileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("addURLsFromFileToolStripMenuItem.Image")));
            this.addURLsFromFileToolStripMenuItem.Name = "addURLsFromFileToolStripMenuItem";
            this.addURLsFromFileToolStripMenuItem.Size = new System.Drawing.Size(242, 30);
            this.addURLsFromFileToolStripMenuItem.Text = "&Add URLs from file";
            this.addURLsFromFileToolStripMenuItem.Click += new System.EventHandler(this.addURLsFromFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(239, 6);
            // 
            // linkToolStripMenuItem
            // 
            this.linkToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemOpenInBrowser,
            this.toolStripMenuItemCopyToClipboard});
            this.linkToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("linkToolStripMenuItem.Image")));
            this.linkToolStripMenuItem.Name = "linkToolStripMenuItem";
            this.linkToolStripMenuItem.Size = new System.Drawing.Size(242, 30);
            this.linkToolStripMenuItem.Text = "&Link";
            // 
            // toolStripMenuItemOpenInBrowser
            // 
            this.toolStripMenuItemOpenInBrowser.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemOpenInBrowser.Image")));
            this.toolStripMenuItemOpenInBrowser.Name = "toolStripMenuItemOpenInBrowser";
            this.toolStripMenuItemOpenInBrowser.Size = new System.Drawing.Size(240, 30);
            this.toolStripMenuItemOpenInBrowser.Text = "&Open in browser";
            this.toolStripMenuItemOpenInBrowser.Click += new System.EventHandler(this.toolStripMenuItemOpenInBrowser_Click);
            // 
            // toolStripMenuItemCopyToClipboard
            // 
            this.toolStripMenuItemCopyToClipboard.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemCopyToClipboard.Image")));
            this.toolStripMenuItemCopyToClipboard.Name = "toolStripMenuItemCopyToClipboard";
            this.toolStripMenuItemCopyToClipboard.Size = new System.Drawing.Size(240, 30);
            this.toolStripMenuItemCopyToClipboard.Text = "&Copy to clipboard";
            this.toolStripMenuItemCopyToClipboard.Click += new System.EventHandler(this.toolStripMenuItemCopyToClipboard_Click);
            // 
            // fbdMain
            // 
            this.fbdMain.Description = "Selecciona la carpeta donde están los documentos de OpenOffice";
            // 
            // ofdAddFile
            // 
            this.ofdAddFile.Filter = resources.GetString("ofdAddFile.Filter");
            this.ofdAddFile.Title = "Select file to extract metadata";
            // 
            // btnSearchAll
            // 
            this.btnSearchAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearchAll.Image = ((System.Drawing.Image)(resources.GetObject("btnSearchAll.Image")));
            this.btnSearchAll.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSearchAll.Location = new System.Drawing.Point(1081, 160);
            this.btnSearchAll.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSearchAll.Name = "btnSearchAll";
            this.btnSearchAll.Size = new System.Drawing.Size(118, 60);
            this.btnSearchAll.TabIndex = 1;
            this.btnSearchAll.Text = "Search All";
            this.btnSearchAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSearchAll.UseVisualStyleBackColor = true;
            this.btnSearchAll.Click += new System.EventHandler(this.btnSearchAll_Click);
            // 
            // picFOCA
            // 
            this.picFOCA.BackColor = System.Drawing.SystemColors.Control;
            this.picFOCA.Image = ((System.Drawing.Image)(resources.GetObject("picFOCA.Image")));
            this.picFOCA.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.picFOCA.Location = new System.Drawing.Point(15, 3);
            this.picFOCA.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picFOCA.Name = "picFOCA";
            this.picFOCA.Size = new System.Drawing.Size(303, 163);
            this.picFOCA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picFOCA.TabIndex = 23;
            this.picFOCA.TabStop = false;
            // 
            // ofdURLList
            // 
            this.ofdURLList.Filter = "All files (*.*)|*.*";
            this.ofdURLList.Title = "Select URLs list file";
            // 
            // imgIcons
            // 
            this.imgIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgIcons.ImageStream")));
            this.imgIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imgIcons.Images.SetKeyName(0, "openoffice.png");
            this.imgIcons.Images.SetKeyName(1, "doc.ico");
            this.imgIcons.Images.SetKeyName(2, "doc.ico");
            this.imgIcons.Images.SetKeyName(3, "report_user.png");
            this.imgIcons.Images.SetKeyName(4, "report_edit.png");
            this.imgIcons.Images.SetKeyName(5, "page_white_stack.png");
            this.imgIcons.Images.SetKeyName(6, "sitemap_color.png");
            this.imgIcons.Images.SetKeyName(7, "doc.ico");
            this.imgIcons.Images.SetKeyName(8, "pdf.png");
            this.imgIcons.Images.SetKeyName(9, "ppt.ico");
            this.imgIcons.Images.SetKeyName(10, "xls.ico");
            this.imgIcons.Images.SetKeyName(11, "OO.png");
            this.imgIcons.Images.SetKeyName(12, "computer.png");
            this.imgIcons.Images.SetKeyName(13, "folder.png");
            this.imgIcons.Images.SetKeyName(14, "group.png");
            this.imgIcons.Images.SetKeyName(15, "printer.png");
            this.imgIcons.Images.SetKeyName(16, "linux.ico");
            this.imgIcons.Images.SetKeyName(17, "paths.ico");
            this.imgIcons.Images.SetKeyName(18, "shares.ico");
            this.imgIcons.Images.SetKeyName(19, "users.ico");
            this.imgIcons.Images.SetKeyName(20, "windows.ico");
            this.imgIcons.Images.SetKeyName(21, "world.png");
            this.imgIcons.Images.SetKeyName(22, "page_white.png");
            this.imgIcons.Images.SetKeyName(23, "email.png");
            this.imgIcons.Images.SetKeyName(24, "date.png");
            this.imgIcons.Images.SetKeyName(25, "page_white_error.png");
            this.imgIcons.Images.SetKeyName(26, "page_white_zip.png");
            this.imgIcons.Images.SetKeyName(27, "page_white_edit.png");
            this.imgIcons.Images.SetKeyName(28, "apple.png");
            this.imgIcons.Images.SetKeyName(29, "wpd.png");
            this.imgIcons.Images.SetKeyName(30, "software-small-icon.png");
            this.imgIcons.Images.SetKeyName(31, "page_white_star.png");
            this.imgIcons.Images.SetKeyName(32, "page_white_picture.png");
            this.imgIcons.Images.SetKeyName(33, "picture_error.png");
            this.imgIcons.Images.SetKeyName(34, "picture.png");
            this.imgIcons.Images.SetKeyName(35, "WindowsVista.png");
            this.imgIcons.Images.SetKeyName(36, "Windows7.png");
            this.imgIcons.Images.SetKeyName(37, "Windows 2000.png");
            this.imgIcons.Images.SetKeyName(38, "Windows2003.png");
            this.imgIcons.Images.SetKeyName(39, "Windows2008.png");
            this.imgIcons.Images.SetKeyName(40, "WindowsXP.png");
            this.imgIcons.Images.SetKeyName(41, "WindowsNT4.0.png");
            this.imgIcons.Images.SetKeyName(42, "folder_link.png");
            this.imgIcons.Images.SetKeyName(43, "group_link.png");
            this.imgIcons.Images.SetKeyName(44, "printer_link.png");
            this.imgIcons.Images.SetKeyName(45, "server.png");
            this.imgIcons.Images.SetKeyName(46, "server_error.png");
            this.imgIcons.Images.SetKeyName(47, "freebsd.png");
            this.imgIcons.Images.SetKeyName(48, "centos.png");
            this.imgIcons.Images.SetKeyName(49, "solaris.png");
            this.imgIcons.Images.SetKeyName(50, "openbsd.png");
            this.imgIcons.Images.SetKeyName(51, "svg.png");
            this.imgIcons.Images.SetKeyName(52, "redhat.png");
            this.imgIcons.Images.SetKeyName(53, "fedora.png");
            this.imgIcons.Images.SetKeyName(54, "mandrake.png");
            this.imgIcons.Images.SetKeyName(55, "mandriva.png");
            this.imgIcons.Images.SetKeyName(56, "suse.png");
            this.imgIcons.Images.SetKeyName(57, "ubuntu.png");
            this.imgIcons.Images.SetKeyName(58, "debian.png");
            this.imgIcons.Images.SetKeyName(59, "network.png");
            this.imgIcons.Images.SetKeyName(60, "eye.png");
            this.imgIcons.Images.SetKeyName(61, "indesign.png");
            // 
            // listViewDocuments
            // 
            this.listViewDocuments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewDocuments.BackColor = System.Drawing.SystemColors.Window;
            this.listViewDocuments.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderId,
            this.columnHeaderType,
            this.columnHeaderURL,
            this.columnHeaderDownload,
            this.columnHeaderDownloadDate,
            this.columnHeaderSize,
            this.columnHeaderAnalyzed,
            this.columnHeaderModifiedDate});
            this.listViewDocuments.ContextMenuStrip = this.contextMenuStripLinks;
            this.listViewDocuments.ForeColor = System.Drawing.SystemColors.WindowText;
            this.listViewDocuments.FullRowSelect = true;
            this.listViewDocuments.GridLines = true;
            this.listViewDocuments.Location = new System.Drawing.Point(12, 228);
            this.listViewDocuments.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listViewDocuments.Name = "listViewDocuments";
            this.listViewDocuments.ShowItemToolTips = true;
            this.listViewDocuments.Size = new System.Drawing.Size(1188, 267);
            this.listViewDocuments.SmallImageList = this.imgIcons;
            this.listViewDocuments.TabIndex = 2;
            this.listViewDocuments.UseCompatibleStateImageBehavior = false;
            this.listViewDocuments.View = System.Windows.Forms.View.Details;
            this.listViewDocuments.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewDocuments_ColumnClick);
            this.listViewDocuments.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listViewDocuments_KeyDown);
            // 
            // columnHeaderId
            // 
            this.columnHeaderId.Text = "Id";
            // 
            // columnHeaderType
            // 
            this.columnHeaderType.Text = "Type";
            this.columnHeaderType.Width = 40;
            // 
            // columnHeaderURL
            // 
            this.columnHeaderURL.Text = "URL";
            this.columnHeaderURL.Width = 290;
            // 
            // columnHeaderDownload
            // 
            this.columnHeaderDownload.Text = "Download";
            // 
            // columnHeaderDownloadDate
            // 
            this.columnHeaderDownloadDate.Text = "Download Date";
            this.columnHeaderDownloadDate.Width = 120;
            // 
            // columnHeaderSize
            // 
            this.columnHeaderSize.Text = "Size";
            this.columnHeaderSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeaderSize.Width = 62;
            // 
            // columnHeaderAnalyzed
            // 
            this.columnHeaderAnalyzed.Text = "Analyzed";
            this.columnHeaderAnalyzed.Width = 55;
            // 
            // columnHeaderModifiedDate
            // 
            this.columnHeaderModifiedDate.Text = "Modified Date";
            this.columnHeaderModifiedDate.Width = 120;
            // 
            // PanelMetadataSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelCustomSearch);
            this.Controls.Add(this.panelSearchConfiguration);
            this.Controls.Add(this.listViewDocuments);
            this.Controls.Add(this.picFOCA);
            this.Controls.Add(this.btnSearchAll);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(952, 508);
            this.Name = "PanelMetadataSearch";
            this.Size = new System.Drawing.Size(1215, 508);
            this.panelSearchConfiguration.ResumeLayout(false);
            this.panelSearchConfiguration.PerformLayout();
            this.panelCustomSearch.ResumeLayout(false);
            this.panelCustomSearch.PerformLayout();
            this.contextMenuStripLinks.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picFOCA)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panelSearchConfiguration;
        public System.Windows.Forms.CheckedListBox checkedListBoxExtensions;
        public System.Windows.Forms.Label lblExtensions;
        public System.Windows.Forms.CheckBox chkBing;
        public System.Windows.Forms.CheckBox chkGoogle;
        private System.Windows.Forms.Label lblSearchEngines;
        public System.Windows.Forms.Panel panelCustomSearch;
        public System.Windows.Forms.LinkLabel linkLabelCustomSearch;
        public System.Windows.Forms.TextBox txtSearch;
        public System.Windows.Forms.Button btnSearch;
        public System.Windows.Forms.Button btnSearchAll;
        public FOCA.Search.ListViewEx listViewDocuments;
        private System.Windows.Forms.ColumnHeader columnHeaderId;
        private System.Windows.Forms.ColumnHeader columnHeaderType;
        private System.Windows.Forms.ColumnHeader columnHeaderURL;
        private System.Windows.Forms.ColumnHeader columnHeaderDownload;
        private System.Windows.Forms.ColumnHeader columnHeaderDownloadDate;
        private System.Windows.Forms.ColumnHeader columnHeaderSize;
        private System.Windows.Forms.ColumnHeader columnHeaderAnalyzed;
        private System.Windows.Forms.ColumnHeader columnHeaderModifiedDate;
        private System.Windows.Forms.PictureBox picFOCA;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripLinks;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDownload;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDownloadAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDelete;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDeleteAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExtractAllMetadata;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAddFile;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAddFolder;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem linkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOpenInBrowser;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemCopyToClipboard;
        private System.Windows.Forms.FolderBrowserDialog fbdMain;
        private System.Windows.Forms.OpenFileDialog ofdAddFile;
        private System.Windows.Forms.ToolStripMenuItem addURLsFromFileToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog ofdURLList;
        public System.Windows.Forms.ImageList imgIcons;
        public System.Windows.Forms.Label lblAll;
        public System.Windows.Forms.Label lblNone;
        private System.Windows.Forms.ToolStripMenuItem analyzeMetadataToolStripMenuItem;
        public System.Windows.Forms.CheckBox chkDuck;
    }
}
