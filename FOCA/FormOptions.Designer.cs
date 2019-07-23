namespace FOCA
{
    partial class FormOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOptions));
            this.imageListOptions = new System.Windows.Forms.ImageList(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.tabPage16 = new System.Windows.Forms.TabPage();
            this.lblBingApiKey = new System.Windows.Forms.Label();
            this.txtBingApiKey = new System.Windows.Forms.TextBox();
            this.lblGoogleApiCx = new System.Windows.Forms.Label();
            this.txtGoogleApiCx = new System.Windows.Forms.TextBox();
            this.txtShodanApiKey = new System.Windows.Forms.TextBox();
            this.lblShodaApiKey = new System.Windows.Forms.Label();
            this.lblGoogleApiKey = new System.Windows.Forms.Label();
            this.txtGoogleApiKey = new System.Windows.Forms.TextBox();
            this.chkNetrange = new System.Windows.Forms.CheckBox();
            this.lblSimultaneousTasks = new System.Windows.Forms.Label();
            this.updSimultaneousTasks = new System.Windows.Forms.NumericUpDown();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.tbDefaultDNSCacheSnooping = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lblParallelDescription = new System.Windows.Forms.Label();
            this.lblParallel1 = new System.Windows.Forms.Label();
            this.updParallelDNS = new System.Windows.Forms.NumericUpDown();
            this.lblRecursivityDescription = new System.Windows.Forms.Label();
            this.lblRecursivity = new System.Windows.Forms.Label();
            this.updRecursivity = new System.Windows.Forms.NumericUpDown();
            this.chkUseAllDNS = new System.Windows.Forms.CheckBox();
            this.lblUseAllDNSDescription = new System.Windows.Forms.Label();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chkResolveHosts = new System.Windows.Forms.CheckBox();
            this.chkHEAD = new System.Windows.Forms.CheckBox();
            this.updSimultaneousDownloads = new System.Windows.Forms.NumericUpDown();
            this.lblSimultaneousDownloads = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage16.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updSimultaneousTasks)).BeginInit();
            this.tabPage7.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updParallelDNS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updRecursivity)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updSimultaneousDownloads)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListOptions
            // 
            this.imageListOptions.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListOptions.ImageStream")));
            this.imageListOptions.TransparentColor = System.Drawing.Color.Red;
            this.imageListOptions.Images.SetKeyName(0, "page_white_code.png");
            this.imageListOptions.Images.SetKeyName(1, "drive_network.png");
            this.imageListOptions.Images.SetKeyName(2, "chart_organisation.png");
            this.imageListOptions.Images.SetKeyName(3, "fingerprint.png");
            this.imageListOptions.Images.SetKeyName(4, "tech.png");
            this.imageListOptions.Images.SetKeyName(5, "folder.png");
            this.imageListOptions.Images.SetKeyName(6, "world_search.png");
            this.imageListOptions.Images.SetKeyName(7, "bomb-icon.png");
            this.imageListOptions.Images.SetKeyName(8, "Configuration1.png");
            this.imageListOptions.Images.SetKeyName(9, "arbol.gif");
            this.imageListOptions.Images.SetKeyName(10, "Metadata.png");
            this.imageListOptions.Images.SetKeyName(11, "DNS Search.png");
            this.imageListOptions.Images.SetKeyName(12, "DNScacheSnooping.png");
            this.imageListOptions.Images.SetKeyName(13, "GeneralConfig.png");
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.Location = new System.Drawing.Point(622, 608);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(124, 38);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Close";
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(28, 608);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(104, 38);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "&Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // tabPage16
            // 
            this.tabPage16.Controls.Add(this.lblBingApiKey);
            this.tabPage16.Controls.Add(this.txtBingApiKey);
            this.tabPage16.Controls.Add(this.lblGoogleApiCx);
            this.tabPage16.Controls.Add(this.txtGoogleApiCx);
            this.tabPage16.Controls.Add(this.txtShodanApiKey);
            this.tabPage16.Controls.Add(this.lblShodaApiKey);
            this.tabPage16.Controls.Add(this.lblGoogleApiKey);
            this.tabPage16.Controls.Add(this.txtGoogleApiKey);
            this.tabPage16.Controls.Add(this.chkNetrange);
            this.tabPage16.Controls.Add(this.lblSimultaneousTasks);
            this.tabPage16.Controls.Add(this.updSimultaneousTasks);
            this.tabPage16.ImageIndex = 13;
            this.tabPage16.Location = new System.Drawing.Point(4, 29);
            this.tabPage16.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage16.Name = "tabPage16";
            this.tabPage16.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage16.Size = new System.Drawing.Size(752, 552);
            this.tabPage16.TabIndex = 8;
            this.tabPage16.Text = "General configuration";
            this.tabPage16.UseVisualStyleBackColor = true;
            // 
            // lblBingApiKey
            // 
            this.lblBingApiKey.AutoSize = true;
            this.lblBingApiKey.Location = new System.Drawing.Point(16, 325);
            this.lblBingApiKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBingApiKey.Name = "lblBingApiKey";
            this.lblBingApiKey.Size = new System.Drawing.Size(107, 20);
            this.lblBingApiKey.TabIndex = 18;
            this.lblBingApiKey.Text = "Bing API KEY";
            // 
            // txtBingApiKey
            // 
            this.txtBingApiKey.Location = new System.Drawing.Point(20, 352);
            this.txtBingApiKey.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtBingApiKey.Name = "txtBingApiKey";
            this.txtBingApiKey.Size = new System.Drawing.Size(544, 26);
            this.txtBingApiKey.TabIndex = 17;
            // 
            // lblGoogleApiCx
            // 
            this.lblGoogleApiCx.AutoSize = true;
            this.lblGoogleApiCx.Location = new System.Drawing.Point(16, 180);
            this.lblGoogleApiCx.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGoogleApiCx.Name = "lblGoogleApiCx";
            this.lblGoogleApiCx.Size = new System.Drawing.Size(205, 20);
            this.lblGoogleApiCx.TabIndex = 16;
            this.lblGoogleApiCx.Text = "Google Custom Search CX:";
            // 
            // txtGoogleApiCx
            // 
            this.txtGoogleApiCx.Location = new System.Drawing.Point(20, 208);
            this.txtGoogleApiCx.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtGoogleApiCx.Name = "txtGoogleApiCx";
            this.txtGoogleApiCx.Size = new System.Drawing.Size(544, 26);
            this.txtGoogleApiCx.TabIndex = 15;
            // 
            // txtShodanApiKey
            // 
            this.txtShodanApiKey.Location = new System.Drawing.Point(20, 277);
            this.txtShodanApiKey.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtShodanApiKey.Name = "txtShodanApiKey";
            this.txtShodanApiKey.Size = new System.Drawing.Size(544, 26);
            this.txtShodanApiKey.TabIndex = 14;
            // 
            // lblShodaApiKey
            // 
            this.lblShodaApiKey.AutoSize = true;
            this.lblShodaApiKey.Location = new System.Drawing.Point(18, 252);
            this.lblShodaApiKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblShodaApiKey.Name = "lblShodaApiKey";
            this.lblShodaApiKey.Size = new System.Drawing.Size(135, 20);
            this.lblShodaApiKey.TabIndex = 13;
            this.lblShodaApiKey.Text = "Shodan API KEY:";
            // 
            // lblGoogleApiKey
            // 
            this.lblGoogleApiKey.AutoSize = true;
            this.lblGoogleApiKey.Location = new System.Drawing.Point(16, 111);
            this.lblGoogleApiKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGoogleApiKey.Name = "lblGoogleApiKey";
            this.lblGoogleApiKey.Size = new System.Drawing.Size(131, 20);
            this.lblGoogleApiKey.TabIndex = 12;
            this.lblGoogleApiKey.Text = "Google API KEY:";
            // 
            // txtGoogleApiKey
            // 
            this.txtGoogleApiKey.Location = new System.Drawing.Point(20, 138);
            this.txtGoogleApiKey.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtGoogleApiKey.Name = "txtGoogleApiKey";
            this.txtGoogleApiKey.Size = new System.Drawing.Size(544, 26);
            this.txtGoogleApiKey.TabIndex = 11;
            // 
            // chkNetrange
            // 
            this.chkNetrange.AutoSize = true;
            this.chkNetrange.Location = new System.Drawing.Point(21, 66);
            this.chkNetrange.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkNetrange.Name = "chkNetrange";
            this.chkNetrange.Size = new System.Drawing.Size(294, 24);
            this.chkNetrange.TabIndex = 10;
            this.chkNetrange.Text = "Only scan /24 netranges (x.x.x.0-255)";
            this.chkNetrange.UseVisualStyleBackColor = true;
            // 
            // lblSimultaneousTasks
            // 
            this.lblSimultaneousTasks.AutoSize = true;
            this.lblSimultaneousTasks.Location = new System.Drawing.Point(86, 22);
            this.lblSimultaneousTasks.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSimultaneousTasks.Name = "lblSimultaneousTasks";
            this.lblSimultaneousTasks.Size = new System.Drawing.Size(148, 20);
            this.lblSimultaneousTasks.TabIndex = 9;
            this.lblSimultaneousTasks.Text = "Simultaneous tasks";
            // 
            // updSimultaneousTasks
            // 
            this.updSimultaneousTasks.Location = new System.Drawing.Point(20, 15);
            this.updSimultaneousTasks.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.updSimultaneousTasks.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.updSimultaneousTasks.Name = "updSimultaneousTasks";
            this.updSimultaneousTasks.Size = new System.Drawing.Size(58, 26);
            this.updSimultaneousTasks.TabIndex = 3;
            this.updSimultaneousTasks.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.label12);
            this.tabPage7.Controls.Add(this.tbDefaultDNSCacheSnooping);
            this.tabPage7.ImageIndex = 12;
            this.tabPage7.Location = new System.Drawing.Point(4, 29);
            this.tabPage7.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage7.Size = new System.Drawing.Size(752, 552);
            this.tabPage7.TabIndex = 6;
            this.tabPage7.Text = "DNS Cache Snooping";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(28, 29);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(160, 20);
            this.label12.TabIndex = 2;
            this.label12.Text = "DNS cache host test:";
            // 
            // tbDefaultDNSCacheSnooping
            // 
            this.tbDefaultDNSCacheSnooping.Location = new System.Drawing.Point(33, 54);
            this.tbDefaultDNSCacheSnooping.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbDefaultDNSCacheSnooping.Name = "tbDefaultDNSCacheSnooping";
            this.tbDefaultDNSCacheSnooping.Size = new System.Drawing.Size(452, 26);
            this.tbDefaultDNSCacheSnooping.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.lblParallelDescription);
            this.tabPage3.Controls.Add(this.lblParallel1);
            this.tabPage3.Controls.Add(this.updParallelDNS);
            this.tabPage3.Controls.Add(this.lblRecursivityDescription);
            this.tabPage3.Controls.Add(this.lblRecursivity);
            this.tabPage3.Controls.Add(this.updRecursivity);
            this.tabPage3.Controls.Add(this.chkUseAllDNS);
            this.tabPage3.Controls.Add(this.lblUseAllDNSDescription);
            this.tabPage3.ImageIndex = 11;
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage3.Size = new System.Drawing.Size(752, 552);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "DNS Search";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lblParallelDescription
            // 
            this.lblParallelDescription.AutoSize = true;
            this.lblParallelDescription.Location = new System.Drawing.Point(14, 211);
            this.lblParallelDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblParallelDescription.Name = "lblParallelDescription";
            this.lblParallelDescription.Size = new System.Drawing.Size(483, 60);
            this.lblParallelDescription.TabIndex = 41;
            this.lblParallelDescription.Text = "To speed up DNS lookups it\'s possible to perform parallel searches.\r\nThis value sets the maximum number of parallel queries.";
            // 
            // lblParallel1
            // 
            this.lblParallel1.AutoSize = true;
            this.lblParallel1.Location = new System.Drawing.Point(76, 174);
            this.lblParallel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblParallel1.Name = "lblParallel1";
            this.lblParallel1.Size = new System.Drawing.Size(157, 20);
            this.lblParallel1.TabIndex = 40;
            this.lblParallel1.Text = "Parallel DNS Queries";
            // 
            // updParallelDNS
            // 
            this.updParallelDNS.Location = new System.Drawing.Point(14, 168);
            this.updParallelDNS.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.updParallelDNS.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.updParallelDNS.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.updParallelDNS.Name = "updParallelDNS";
            this.updParallelDNS.Size = new System.Drawing.Size(52, 26);
            this.updParallelDNS.TabIndex = 39;
            this.updParallelDNS.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblRecursivityDescription
            // 
            this.lblRecursivityDescription.AutoSize = true;
            this.lblRecursivityDescription.Location = new System.Drawing.Point(9, 118);
            this.lblRecursivityDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRecursivityDescription.Name = "lblRecursivityDescription";
            this.lblRecursivityDescription.Size = new System.Drawing.Size(500, 40);
            this.lblRecursivityDescription.TabIndex = 34;
            this.lblRecursivityDescription.Text = "The program tries to reverse lookup each domain or IP address found.\r\nThree level" +
    "s of recursion are recommended.";
            // 
            // lblRecursivity
            // 
            this.lblRecursivity.AutoSize = true;
            this.lblRecursivity.Location = new System.Drawing.Point(76, 85);
            this.lblRecursivity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRecursivity.Name = "lblRecursivity";
            this.lblRecursivity.Size = new System.Drawing.Size(111, 20);
            this.lblRecursivity.TabIndex = 33;
            this.lblRecursivity.Text = "Max recursion";
            // 
            // updRecursivity
            // 
            this.updRecursivity.Location = new System.Drawing.Point(14, 80);
            this.updRecursivity.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.updRecursivity.Name = "updRecursivity";
            this.updRecursivity.Size = new System.Drawing.Size(52, 26);
            this.updRecursivity.TabIndex = 1;
            // 
            // chkUseAllDNS
            // 
            this.chkUseAllDNS.AutoSize = true;
            this.chkUseAllDNS.Location = new System.Drawing.Point(14, 15);
            this.chkUseAllDNS.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkUseAllDNS.Name = "chkUseAllDNS";
            this.chkUseAllDNS.Size = new System.Drawing.Size(176, 24);
            this.chkUseAllDNS.TabIndex = 0;
            this.chkUseAllDNS.Text = "Use all DNS servers";
            this.chkUseAllDNS.UseVisualStyleBackColor = true;
            // 
            // lblUseAllDNSDescription
            // 
            this.lblUseAllDNSDescription.AutoSize = true;
            this.lblUseAllDNSDescription.Location = new System.Drawing.Point(12, 49);
            this.lblUseAllDNSDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUseAllDNSDescription.Name = "lblUseAllDNSDescription";
            this.lblUseAllDNSDescription.Size = new System.Drawing.Size(496, 20);
            this.lblUseAllDNSDescription.TabIndex = 29;
            this.lblUseAllDNSDescription.Text = "When FOCA launches a DNS query, it uses all available DNS servers.";
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chkResolveHosts);
            this.tabPage1.Controls.Add(this.chkHEAD);
            this.tabPage1.Controls.Add(this.updSimultaneousDownloads);
            this.tabPage1.Controls.Add(this.lblSimultaneousDownloads);
            this.tabPage1.ImageIndex = 10;
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Size = new System.Drawing.Size(752, 552);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Metadata";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chkResolveHosts
            // 
            this.chkResolveHosts.AutoSize = true;
            this.chkResolveHosts.Location = new System.Drawing.Point(14, 54);
            this.chkResolveHosts.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkResolveHosts.Name = "chkResolveHosts";
            this.chkResolveHosts.Size = new System.Drawing.Size(337, 24);
            this.chkResolveHosts.TabIndex = 1;
            this.chkResolveHosts.Text = "Resolve host in metadata analysis (Slower)";
            this.chkResolveHosts.UseVisualStyleBackColor = true;
            // 
            // chkHEAD
            // 
            this.chkHEAD.AutoSize = true;
            this.chkHEAD.Location = new System.Drawing.Point(14, 15);
            this.chkHEAD.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkHEAD.Name = "chkHEAD";
            this.chkHEAD.Size = new System.Drawing.Size(242, 24);
            this.chkHEAD.TabIndex = 0;
            this.chkHEAD.Text = "Use HEAD to get the file size";
            this.chkHEAD.UseVisualStyleBackColor = true;
            // 
            // updSimultaneousDownloads
            // 
            this.updSimultaneousDownloads.Location = new System.Drawing.Point(14, 98);
            this.updSimultaneousDownloads.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.updSimultaneousDownloads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.updSimultaneousDownloads.Name = "updSimultaneousDownloads";
            this.updSimultaneousDownloads.Size = new System.Drawing.Size(58, 26);
            this.updSimultaneousDownloads.TabIndex = 2;
            this.updSimultaneousDownloads.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.updSimultaneousDownloads.Validating += new System.ComponentModel.CancelEventHandler(this.nudSimultaneousDownloads_Validating);
            // 
            // lblSimultaneousDownloads
            // 
            this.lblSimultaneousDownloads.AutoSize = true;
            this.lblSimultaneousDownloads.Location = new System.Drawing.Point(81, 102);
            this.lblSimultaneousDownloads.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSimultaneousDownloads.Name = "lblSimultaneousDownloads";
            this.lblSimultaneousDownloads.Size = new System.Drawing.Size(186, 20);
            this.lblSimultaneousDownloads.TabIndex = 8;
            this.lblSimultaneousDownloads.Text = "Simultaneous downloads";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Controls.Add(this.tabPage16);
            this.tabControl1.ImageList = this.imageListOptions;
            this.tabControl1.Location = new System.Drawing.Point(0, 14);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(760, 585);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 0;
            // 
            // FormOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 654);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(542, 553);
            this.Name = "FormOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.FormOptions_Load);
            this.tabPage16.ResumeLayout(false);
            this.tabPage16.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updSimultaneousTasks)).EndInit();
            this.tabPage7.ResumeLayout(false);
            this.tabPage7.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updParallelDNS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.updRecursivity)).EndInit();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updSimultaneousDownloads)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ImageList imageListOptions;
        private System.Windows.Forms.TabPage tabPage16;
        private System.Windows.Forms.Label lblGoogleApiKey;
        private System.Windows.Forms.TextBox txtGoogleApiKey;
        private System.Windows.Forms.CheckBox chkNetrange;
        private System.Windows.Forms.Label lblSimultaneousTasks;
        private System.Windows.Forms.NumericUpDown updSimultaneousTasks;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbDefaultDNSCacheSnooping;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label lblParallelDescription;
        private System.Windows.Forms.Label lblParallel1;
        private System.Windows.Forms.NumericUpDown updParallelDNS;
        private System.Windows.Forms.Label lblRecursivityDescription;
        private System.Windows.Forms.Label lblRecursivity;
        private System.Windows.Forms.NumericUpDown updRecursivity;
        private System.Windows.Forms.CheckBox chkUseAllDNS;
        private System.Windows.Forms.Label lblUseAllDNSDescription;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox chkResolveHosts;
        private System.Windows.Forms.CheckBox chkHEAD;
        private System.Windows.Forms.NumericUpDown updSimultaneousDownloads;
        private System.Windows.Forms.Label lblSimultaneousDownloads;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TextBox txtShodanApiKey;
        private System.Windows.Forms.Label lblShodaApiKey;
        private System.Windows.Forms.Label lblGoogleApiCx;
        private System.Windows.Forms.TextBox txtGoogleApiCx;
        private System.Windows.Forms.Label lblBingApiKey;
        private System.Windows.Forms.TextBox txtBingApiKey;
    }
}