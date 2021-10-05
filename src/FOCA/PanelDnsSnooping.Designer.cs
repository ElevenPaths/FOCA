namespace FOCA
{
    partial class PanelDnsSnooping
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelDnsSnooping));
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.lblFile = new System.Windows.Forms.Label();
            this.lblDns = new System.Windows.Forms.Label();
            this.txtDomain = new System.Windows.Forms.TextBox();
            this.lblDomain = new System.Windows.Forms.Label();
            this.lstDns = new System.Windows.Forms.ListBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblStatusValue = new System.Windows.Forms.Label();
            this.lblCache = new System.Windows.Forms.Label();
            this.chkMonitorice = new System.Windows.Forms.CheckBox();
            this.lblMonitor = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lveCache = new FOCA.Search.ListViewEx();
            this.lveMonitor = new FOCA.Search.ListViewEx();
            this.timerSnoop = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDnsSnooping = new System.Windows.Forms.Button();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.btnGetDNS = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFilename
            // 
            this.txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilename.Location = new System.Drawing.Point(93, 57);
            this.txtFilename.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.ReadOnly = true;
            this.txtFilename.Size = new System.Drawing.Size(786, 26);
            this.txtFilename.TabIndex = 12;
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(18, 62);
            this.lblFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(34, 20);
            this.lblFile.TabIndex = 11;
            this.lblFile.Text = "File";
            // 
            // lblDns
            // 
            this.lblDns.AutoSize = true;
            this.lblDns.Location = new System.Drawing.Point(18, 103);
            this.lblDns.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDns.Name = "lblDns";
            this.lblDns.Size = new System.Drawing.Size(38, 20);
            this.lblDns.TabIndex = 7;
            this.lblDns.Text = "DNS";
            // 
            // txtDomain
            // 
            this.txtDomain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDomain.Location = new System.Drawing.Point(93, 15);
            this.txtDomain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(786, 26);
            this.txtDomain.TabIndex = 14;
            // 
            // lblDomain
            // 
            this.lblDomain.AutoSize = true;
            this.lblDomain.Location = new System.Drawing.Point(18, 20);
            this.lblDomain.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDomain.Name = "lblDomain";
            this.lblDomain.Size = new System.Drawing.Size(64, 20);
            this.lblDomain.TabIndex = 15;
            this.lblDomain.Text = "Domain";
            // 
            // lstDns
            // 
            this.lstDns.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstDns.FormattingEnabled = true;
            this.lstDns.ItemHeight = 20;
            this.lstDns.Location = new System.Drawing.Point(93, 98);
            this.lstDns.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lstDns.Name = "lstDns";
            this.lstDns.Size = new System.Drawing.Size(786, 84);
            this.lstDns.TabIndex = 16;
            this.lstDns.SelectedIndexChanged += new System.EventHandler(this.lbDns_SelectedIndexChanged);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(58, 474);
            this.lblStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(60, 20);
            this.lblStatus.TabIndex = 18;
            this.lblStatus.Text = "Status:";
            // 
            // lblStatusValue
            // 
            this.lblStatusValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatusValue.AutoSize = true;
            this.lblStatusValue.Location = new System.Drawing.Point(124, 474);
            this.lblStatusValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblStatusValue.Name = "lblStatusValue";
            this.lblStatusValue.Size = new System.Drawing.Size(63, 20);
            this.lblStatusValue.TabIndex = 19;
            this.lblStatusValue.Text = "<none>";
            // 
            // lblCache
            // 
            this.lblCache.AutoSize = true;
            this.lblCache.Location = new System.Drawing.Point(18, 15);
            this.lblCache.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCache.Name = "lblCache";
            this.lblCache.Size = new System.Drawing.Size(55, 20);
            this.lblCache.TabIndex = 20;
            this.lblCache.Text = "Cache";
            // 
            // chkMonitorice
            // 
            this.chkMonitorice.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkMonitorice.AutoSize = true;
            this.chkMonitorice.Enabled = false;
            this.chkMonitorice.Location = new System.Drawing.Point(92, 92);
            this.chkMonitorice.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkMonitorice.Name = "chkMonitorice";
            this.chkMonitorice.Size = new System.Drawing.Size(215, 24);
            this.chkMonitorice.TabIndex = 22;
            this.chkMonitorice.Text = "Monitor each 2 minutes.";
            this.chkMonitorice.UseVisualStyleBackColor = true;
            this.chkMonitorice.CheckedChanged += new System.EventHandler(this.cbMonitorice_CheckedChanged);
            // 
            // lblMonitor
            // 
            this.lblMonitor.AutoSize = true;
            this.lblMonitor.Location = new System.Drawing.Point(18, 5);
            this.lblMonitor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMonitor.Name = "lblMonitor";
            this.lblMonitor.Size = new System.Drawing.Size(62, 20);
            this.lblMonitor.TabIndex = 26;
            this.lblMonitor.Text = "Monitor";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(36, 205);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lveCache);
            this.splitContainer1.Panel1.Controls.Add(this.lblCache);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lveMonitor);
            this.splitContainer1.Panel2.Controls.Add(this.chkMonitorice);
            this.splitContainer1.Panel2.Controls.Add(this.lblMonitor);
            this.splitContainer1.Size = new System.Drawing.Size(1131, 240);
            this.splitContainer1.SplitterDistance = 118;
            this.splitContainer1.SplitterWidth = 6;
            this.splitContainer1.TabIndex = 27;
            // 
            // lveCache
            // 
            this.lveCache.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lveCache.GridLines = true;
            this.lveCache.Location = new System.Drawing.Point(93, 15);
            this.lveCache.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lveCache.Name = "lveCache";
            this.lveCache.Size = new System.Drawing.Size(1018, 82);
            this.lveCache.TabIndex = 24;
            this.lveCache.UseCompatibleStateImageBehavior = false;
            this.lveCache.View = System.Windows.Forms.View.Details;
            // 
            // lveMonitor
            // 
            this.lveMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lveMonitor.GridLines = true;
            this.lveMonitor.Location = new System.Drawing.Point(92, 5);
            this.lveMonitor.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lveMonitor.Name = "lveMonitor";
            this.lveMonitor.Size = new System.Drawing.Size(1020, 82);
            this.lveMonitor.TabIndex = 25;
            this.lveMonitor.UseCompatibleStateImageBehavior = false;
            this.lveMonitor.View = System.Windows.Forms.View.Details;
            // 
            // timerSnoop
            // 
            this.timerSnoop.Interval = 120000;
            this.timerSnoop.Tick += new System.EventHandler(this.timerSnoop_Tick);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.lstDns);
            this.panel1.Controls.Add(this.lblDns);
            this.panel1.Controls.Add(this.btnDnsSnooping);
            this.panel1.Controls.Add(this.lblFile);
            this.panel1.Controls.Add(this.txtFilename);
            this.panel1.Controls.Add(this.btnLoadFile);
            this.panel1.Controls.Add(this.btnGetDNS);
            this.panel1.Controls.Add(this.txtDomain);
            this.panel1.Controls.Add(this.lblDomain);
            this.panel1.Location = new System.Drawing.Point(36, 5);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1131, 191);
            this.panel1.TabIndex = 85;
            // 
            // btnDnsSnooping
            // 
            this.btnDnsSnooping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDnsSnooping.Enabled = false;
            this.btnDnsSnooping.Image = ((System.Drawing.Image)(resources.GetObject("btnDnsSnooping.Image")));
            this.btnDnsSnooping.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDnsSnooping.Location = new System.Drawing.Point(896, 100);
            this.btnDnsSnooping.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnDnsSnooping.Name = "btnDnsSnooping";
            this.btnDnsSnooping.Size = new System.Drawing.Size(218, 37);
            this.btnDnsSnooping.TabIndex = 10;
            this.btnDnsSnooping.Text = "Snoop DNS";
            this.btnDnsSnooping.UseVisualStyleBackColor = true;
            this.btnDnsSnooping.Click += new System.EventHandler(this.btDnsSnooping_Click);
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadFile.Image = ((System.Drawing.Image)(resources.GetObject("btnLoadFile.Image")));
            this.btnLoadFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLoadFile.Location = new System.Drawing.Point(896, 55);
            this.btnLoadFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(218, 37);
            this.btnLoadFile.TabIndex = 13;
            this.btnLoadFile.Text = "Load file";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.btLoadFile_Click);
            // 
            // btnGetDNS
            // 
            this.btnGetDNS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetDNS.Image = ((System.Drawing.Image)(resources.GetObject("btnGetDNS.Image")));
            this.btnGetDNS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGetDNS.Location = new System.Drawing.Point(896, 11);
            this.btnGetDNS.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGetDNS.Name = "btnGetDNS";
            this.btnGetDNS.Size = new System.Drawing.Size(218, 37);
            this.btnGetDNS.TabIndex = 17;
            this.btnGetDNS.Text = "Obtain DNS servers";
            this.btnGetDNS.UseVisualStyleBackColor = true;
            this.btnGetDNS.Click += new System.EventHandler(this.btGetDNS_Click);
            // 
            // PanelDnsSnooping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.lblStatusValue);
            this.Controls.Add(this.lblStatus);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "PanelDnsSnooping";
            this.Size = new System.Drawing.Size(1197, 512);
            this.Load += new System.EventHandler(this.PanelDnsSnooping_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.TextBox txtFilename;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.Label lblDns;
        private System.Windows.Forms.TextBox txtDomain;
        private System.Windows.Forms.Label lblDomain;
        private System.Windows.Forms.Button btnGetDNS;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblStatusValue;
        private System.Windows.Forms.Label lblCache;
        private System.Windows.Forms.CheckBox chkMonitorice;
        public FOCA.Search.ListViewEx lveCache;
        public FOCA.Search.ListViewEx lveMonitor;
        private System.Windows.Forms.Label lblMonitor;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Timer timerSnoop;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.ListBox lstDns;
        public System.Windows.Forms.Button btnDnsSnooping;
    }
}
