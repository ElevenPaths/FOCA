namespace FOCA
{
    partial class PanelInformation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelInformation));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Name",
            "PC_DEP-02-14"}, -1, System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F));
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Operating System",
            "Windows XP"}, -1, System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F));
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "Software",
            "Microsoft Office 2007"}, -1, System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F));
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "Language",
            "English"}, -1, System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F));
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "IP",
            "192.168.1.54"}, -1, System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F));
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStripExport = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.sfdExport = new System.Windows.Forms.SaveFileDialog();
            this.splitPanel = new System.Windows.Forms.SplitContainer();
            this.lbDomain = new System.Windows.Forms.Label();
            this.tabMap = new System.Windows.Forms.TabControl();
            this.toolStripMenuItemExport = new System.Windows.Forms.ToolStripMenuItem();
            this.searchDocumentsWhereAppearsValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openUrlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnHide = new System.Windows.Forms.Button();
            this.picFOCA = new System.Windows.Forms.PictureBox();
            this.lvwInformation = new FOCA.Search.ListViewEx();
            this.columnHeaderAttribute = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelInformationOptions = new FOCA.PanelInformationOptions();
            this.contextMenuStripExport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitPanel)).BeginInit();
            this.splitPanel.Panel1.SuspendLayout();
            this.splitPanel.Panel2.SuspendLayout();
            this.splitPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFOCA)).BeginInit();
            this.SuspendLayout();
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 30000;
            this.toolTip.BackColor = System.Drawing.Color.LightGray;
            this.toolTip.InitialDelay = 500;
            this.toolTip.IsBalloon = true;
            this.toolTip.ReshowDelay = 200;
            // 
            // contextMenuStripExport
            // 
            this.contextMenuStripExport.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStripExport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemExport,
            this.searchDocumentsWhereAppearsValueToolStripMenuItem,
            this.openUrlToolStripMenuItem});
            this.contextMenuStripExport.Name = "contextMenuStripExport";
            this.contextMenuStripExport.Size = new System.Drawing.Size(440, 94);
            this.contextMenuStripExport.Opened += new System.EventHandler(this.contextMenuStripExport_Opened);
            // 
            // sfdExport
            // 
            this.sfdExport.DefaultExt = "txt";
            this.sfdExport.Filter = "Text Files|*.txt|All Files|*.*";
            // 
            // splitPanel
            // 
            this.splitPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitPanel.Location = new System.Drawing.Point(16, 189);
            this.splitPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitPanel.Name = "splitPanel";
            this.splitPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitPanel.Panel1
            // 
            this.splitPanel.Panel1.Controls.Add(this.lvwInformation);
            this.splitPanel.Panel1.Resize += new System.EventHandler(this.splitPanel_Panel1_Resize);
            this.splitPanel.Panel1MinSize = 0;
            // 
            // splitPanel.Panel2
            // 
            this.splitPanel.Panel2.Controls.Add(this.btnHide);
            this.splitPanel.Panel2.Controls.Add(this.lbDomain);
            this.splitPanel.Panel2.Controls.Add(this.tabMap);
            this.splitPanel.Panel2.Controls.Add(this.panelInformationOptions);
            this.splitPanel.Panel2.Resize += new System.EventHandler(this.splitPanel_Panel2_Resize);
            this.splitPanel.Panel2MinSize = 0;
            this.splitPanel.Size = new System.Drawing.Size(1410, 431);
            this.splitPanel.SplitterDistance = 198;
            this.splitPanel.SplitterWidth = 6;
            this.splitPanel.TabIndex = 84;
            // 
            // lbDomain
            // 
            this.lbDomain.AutoSize = true;
            this.lbDomain.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.lbDomain.Location = new System.Drawing.Point(3, 162);
            this.lbDomain.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDomain.Name = "lbDomain";
            this.lbDomain.Size = new System.Drawing.Size(72, 20);
            this.lbDomain.TabIndex = 88;
            this.lbDomain.Text = "Domain: ";
            // 
            // tabMap
            // 
            this.tabMap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabMap.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabMap.Location = new System.Drawing.Point(0, 183);
            this.tabMap.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabMap.Multiline = true;
            this.tabMap.Name = "tabMap";
            this.tabMap.SelectedIndex = 0;
            this.tabMap.Size = new System.Drawing.Size(1408, 46);
            this.tabMap.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabMap.TabIndex = 83;
            // 
            // toolStripMenuItemExport
            // 
            this.toolStripMenuItemExport.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItemExport.Image")));
            this.toolStripMenuItemExport.Name = "toolStripMenuItemExport";
            this.toolStripMenuItemExport.Size = new System.Drawing.Size(439, 30);
            this.toolStripMenuItemExport.Text = "&Export  data to file";
            this.toolStripMenuItemExport.Click += new System.EventHandler(this.toolStripMenuItemExport_Click);
            // 
            // searchDocumentsWhereAppearsValueToolStripMenuItem
            // 
            this.searchDocumentsWhereAppearsValueToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("searchDocumentsWhereAppearsValueToolStripMenuItem.Image")));
            this.searchDocumentsWhereAppearsValueToolStripMenuItem.Name = "searchDocumentsWhereAppearsValueToolStripMenuItem";
            this.searchDocumentsWhereAppearsValueToolStripMenuItem.Size = new System.Drawing.Size(439, 30);
            this.searchDocumentsWhereAppearsValueToolStripMenuItem.Text = "&Search Documents where appears this value";
            this.searchDocumentsWhereAppearsValueToolStripMenuItem.Click += new System.EventHandler(this.searchDocumentsWhereAppearsValueToolStripMenuItem_Click);
            // 
            // openUrlToolStripMenuItem
            // 
            this.openUrlToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openUrlToolStripMenuItem.Image")));
            this.openUrlToolStripMenuItem.Name = "openUrlToolStripMenuItem";
            this.openUrlToolStripMenuItem.Size = new System.Drawing.Size(439, 30);
            this.openUrlToolStripMenuItem.Text = "Open URL";
            this.openUrlToolStripMenuItem.Click += new System.EventHandler(this.openUrlToolStripMenuItem_Click);
            // 
            // btnHide
            // 
            this.btnHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHide.Image = ((System.Drawing.Image)(resources.GetObject("btnHide.Image")));
            this.btnHide.Location = new System.Drawing.Point(1257, 8);
            this.btnHide.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(144, 63);
            this.btnHide.TabIndex = 87;
            this.btnHide.Text = "Minimize";
            this.btnHide.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Click += new System.EventHandler(this.btHide_Click);
            // 
            // picFOCA
            // 
            this.picFOCA.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picFOCA.BackColor = System.Drawing.SystemColors.Control;
            this.picFOCA.Image = global::FOCA.Properties.Resources.newlogo;
            this.picFOCA.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.picFOCA.Location = new System.Drawing.Point(300, 12);
            this.picFOCA.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picFOCA.Name = "picFOCA";
            this.picFOCA.Size = new System.Drawing.Size(392, 154);
            this.picFOCA.TabIndex = 86;
            this.picFOCA.TabStop = false;
            // 
            // lvwInformation
            // 
            this.lvwInformation.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderAttribute,
            this.columnHeaderValue});
            this.lvwInformation.ContextMenuStrip = this.contextMenuStripExport;
            this.lvwInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwInformation.FullRowSelect = true;
            this.lvwInformation.GridLines = true;
            this.lvwInformation.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5});
            this.lvwInformation.Location = new System.Drawing.Point(0, 0);
            this.lvwInformation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lvwInformation.Name = "lvwInformation";
            this.lvwInformation.Size = new System.Drawing.Size(1408, 196);
            this.lvwInformation.TabIndex = 0;
            this.lvwInformation.UseCompatibleStateImageBehavior = false;
            this.lvwInformation.View = System.Windows.Forms.View.Details;
            this.lvwInformation.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listViewInformacion_ColumnClick);
            this.lvwInformation.Click += new System.EventHandler(this.listViewInformation_Click);
            this.lvwInformation.MouseMove += new System.Windows.Forms.MouseEventHandler(this.listViewInformation_MouseMove);
            // 
            // columnHeaderAttribute
            // 
            this.columnHeaderAttribute.Text = "Attribute";
            this.columnHeaderAttribute.Width = 242;
            // 
            // columnHeaderValue
            // 
            this.columnHeaderValue.Text = "Value";
            this.columnHeaderValue.Width = 400;
            // 
            // panelInformationOptions
            // 
            this.panelInformationOptions.Location = new System.Drawing.Point(4, 8);
            this.panelInformationOptions.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.panelInformationOptions.Name = "panelInformationOptions";
            this.panelInformationOptions.Size = new System.Drawing.Size(874, 149);
            this.panelInformationOptions.TabIndex = 89;
            // 
            // PanelInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitPanel);
            this.Controls.Add(this.picFOCA);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "PanelInformation";
            this.Size = new System.Drawing.Size(1444, 625);
            this.Load += new System.EventHandler(this.PanelInformation_Load);
            this.SizeChanged += new System.EventHandler(this.PanelInformation_SizeChanged);
            this.contextMenuStripExport.ResumeLayout(false);
            this.splitPanel.Panel1.ResumeLayout(false);
            this.splitPanel.Panel2.ResumeLayout(false);
            this.splitPanel.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitPanel)).EndInit();
            this.splitPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picFOCA)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.SaveFileDialog sfdExport;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExport;
        private System.Windows.Forms.ToolStripMenuItem searchDocumentsWhereAppearsValueToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader columnHeaderAttribute;
        private System.Windows.Forms.ColumnHeader columnHeaderValue;
        public FOCA.Search.ListViewEx lvwInformation;
        public System.Windows.Forms.TabControl tabMap;
        public System.Windows.Forms.SplitContainer splitPanel;
        private System.Windows.Forms.Button btnHide;
        public System.Windows.Forms.Label lbDomain;
        public System.Windows.Forms.ContextMenuStrip contextMenuStripExport;
        private System.Windows.Forms.PictureBox picFOCA;
        public PanelInformationOptions panelInformationOptions;
        private System.Windows.Forms.ToolStripMenuItem openUrlToolStripMenuItem;
    }
}
