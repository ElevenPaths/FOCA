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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txtDiarioApiKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDiarioSecret = new System.Windows.Forms.TextBox();
            this.tabPage16.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updSimultaneousTasks)).BeginInit();
            this.tabPage7.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updParallelDNS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.updRecursivity)).BeginInit();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updSimultaneousDownloads)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
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
            this.btnCancel.Location = new System.Drawing.Point(415, 418);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 25);
            this.btnCancel.TabIndex = 1;
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
            this.btnSave.Location = new System.Drawing.Point(19, 418);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(69, 25);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "&Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // tabPage16
            // 
            this.tabPage16.Controls.Add(this.groupBox4);
            this.tabPage16.Controls.Add(this.groupBox3);
            this.tabPage16.Controls.Add(this.groupBox2);
            this.tabPage16.Controls.Add(this.groupBox1);
            this.tabPage16.Controls.Add(this.chkNetrange);
            this.tabPage16.Controls.Add(this.lblSimultaneousTasks);
            this.tabPage16.Controls.Add(this.updSimultaneousTasks);
            this.tabPage16.ImageIndex = 13;
            this.tabPage16.Location = new System.Drawing.Point(4, 23);
            this.tabPage16.Name = "tabPage16";
            this.tabPage16.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage16.Size = new System.Drawing.Size(499, 376);
            this.tabPage16.TabIndex = 8;
            this.tabPage16.Text = "General configuration";
            this.tabPage16.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtGoogleApiKey);
            this.groupBox1.Controls.Add(this.lblGoogleApiKey);
            this.groupBox1.Controls.Add(this.lblGoogleApiCx);
            this.groupBox1.Controls.Add(this.txtGoogleApiCx);
            this.groupBox1.Location = new System.Drawing.Point(13, 71);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(478, 76);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Google Custom Search API";
            // 
            // lblBingApiKey
            // 
            this.lblBingApiKey.AutoSize = true;
            this.lblBingApiKey.Location = new System.Drawing.Point(6, 22);
            this.lblBingApiKey.Name = "lblBingApiKey";
            this.lblBingApiKey.Size = new System.Drawing.Size(47, 13);
            this.lblBingApiKey.TabIndex = 18;
            this.lblBingApiKey.Text = "API key:";
            // 
            // txtBingApiKey
            // 
            this.txtBingApiKey.Location = new System.Drawing.Point(85, 19);
            this.txtBingApiKey.Name = "txtBingApiKey";
            this.txtBingApiKey.Size = new System.Drawing.Size(387, 20);
            this.txtBingApiKey.TabIndex = 0;
            // 
            // lblGoogleApiCx
            // 
            this.lblGoogleApiCx.AutoSize = true;
            this.lblGoogleApiCx.Location = new System.Drawing.Point(6, 48);
            this.lblGoogleApiCx.Name = "lblGoogleApiCx";
            this.lblGoogleApiCx.Size = new System.Drawing.Size(77, 13);
            this.lblGoogleApiCx.TabIndex = 16;
            this.lblGoogleApiCx.Text = "Engine ID (cx):";
            // 
            // txtGoogleApiCx
            // 
            this.txtGoogleApiCx.Location = new System.Drawing.Point(85, 45);
            this.txtGoogleApiCx.Name = "txtGoogleApiCx";
            this.txtGoogleApiCx.Size = new System.Drawing.Size(387, 20);
            this.txtGoogleApiCx.TabIndex = 1;
            // 
            // txtShodanApiKey
            // 
            this.txtShodanApiKey.Location = new System.Drawing.Point(85, 19);
            this.txtShodanApiKey.Name = "txtShodanApiKey";
            this.txtShodanApiKey.Size = new System.Drawing.Size(387, 20);
            this.txtShodanApiKey.TabIndex = 0;
            // 
            // lblShodaApiKey
            // 
            this.lblShodaApiKey.AutoSize = true;
            this.lblShodaApiKey.Location = new System.Drawing.Point(6, 22);
            this.lblShodaApiKey.Name = "lblShodaApiKey";
            this.lblShodaApiKey.Size = new System.Drawing.Size(47, 13);
            this.lblShodaApiKey.TabIndex = 13;
            this.lblShodaApiKey.Text = "API key:";
            // 
            // lblGoogleApiKey
            // 
            this.lblGoogleApiKey.AutoSize = true;
            this.lblGoogleApiKey.Location = new System.Drawing.Point(6, 22);
            this.lblGoogleApiKey.Name = "lblGoogleApiKey";
            this.lblGoogleApiKey.Size = new System.Drawing.Size(47, 13);
            this.lblGoogleApiKey.TabIndex = 12;
            this.lblGoogleApiKey.Text = "API key:";
            // 
            // txtGoogleApiKey
            // 
            this.txtGoogleApiKey.Location = new System.Drawing.Point(85, 19);
            this.txtGoogleApiKey.Name = "txtGoogleApiKey";
            this.txtGoogleApiKey.Size = new System.Drawing.Size(387, 20);
            this.txtGoogleApiKey.TabIndex = 0;
            // 
            // chkNetrange
            // 
            this.chkNetrange.AutoSize = true;
            this.chkNetrange.Location = new System.Drawing.Point(14, 43);
            this.chkNetrange.Name = "chkNetrange";
            this.chkNetrange.Size = new System.Drawing.Size(203, 17);
            this.chkNetrange.TabIndex = 1;
            this.chkNetrange.Text = "Only scan /24 netranges (x.x.x.0-255)";
            this.chkNetrange.UseVisualStyleBackColor = true;
            // 
            // lblSimultaneousTasks
            // 
            this.lblSimultaneousTasks.AutoSize = true;
            this.lblSimultaneousTasks.Location = new System.Drawing.Point(57, 14);
            this.lblSimultaneousTasks.Name = "lblSimultaneousTasks";
            this.lblSimultaneousTasks.Size = new System.Drawing.Size(98, 13);
            this.lblSimultaneousTasks.TabIndex = 9;
            this.lblSimultaneousTasks.Text = "Simultaneous tasks";
            // 
            // updSimultaneousTasks
            // 
            this.updSimultaneousTasks.Location = new System.Drawing.Point(13, 10);
            this.updSimultaneousTasks.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.updSimultaneousTasks.Name = "updSimultaneousTasks";
            this.updSimultaneousTasks.Size = new System.Drawing.Size(39, 20);
            this.updSimultaneousTasks.TabIndex = 0;
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
            this.tabPage7.Location = new System.Drawing.Point(4, 23);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(499, 353);
            this.tabPage7.TabIndex = 6;
            this.tabPage7.Text = "DNS Cache Snooping";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(19, 19);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(109, 13);
            this.label12.TabIndex = 2;
            this.label12.Text = "DNS cache host test:";
            // 
            // tbDefaultDNSCacheSnooping
            // 
            this.tbDefaultDNSCacheSnooping.Location = new System.Drawing.Point(22, 35);
            this.tbDefaultDNSCacheSnooping.Name = "tbDefaultDNSCacheSnooping";
            this.tbDefaultDNSCacheSnooping.Size = new System.Drawing.Size(303, 20);
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
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(499, 353);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "DNS Search";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // lblParallelDescription
            // 
            this.lblParallelDescription.AutoSize = true;
            this.lblParallelDescription.Location = new System.Drawing.Point(9, 137);
            this.lblParallelDescription.Name = "lblParallelDescription";
            this.lblParallelDescription.Size = new System.Drawing.Size(324, 26);
            this.lblParallelDescription.TabIndex = 41;
            this.lblParallelDescription.Text = "To speed up DNS lookups it\'s possible to perform parallel searches.\r\nThis value s" +
    "ets the maximum number of parallel queries.";
            // 
            // lblParallel1
            // 
            this.lblParallel1.AutoSize = true;
            this.lblParallel1.Location = new System.Drawing.Point(51, 113);
            this.lblParallel1.Name = "lblParallel1";
            this.lblParallel1.Size = new System.Drawing.Size(106, 13);
            this.lblParallel1.TabIndex = 40;
            this.lblParallel1.Text = "Parallel DNS Queries";
            // 
            // updParallelDNS
            // 
            this.updParallelDNS.Location = new System.Drawing.Point(9, 109);
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
            this.updParallelDNS.Size = new System.Drawing.Size(35, 20);
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
            this.lblRecursivityDescription.Location = new System.Drawing.Point(6, 77);
            this.lblRecursivityDescription.Name = "lblRecursivityDescription";
            this.lblRecursivityDescription.Size = new System.Drawing.Size(336, 26);
            this.lblRecursivityDescription.TabIndex = 34;
            this.lblRecursivityDescription.Text = "The program tries to reverse lookup each domain or IP address found.\r\nThree level" +
    "s of recursion are recommended.";
            // 
            // lblRecursivity
            // 
            this.lblRecursivity.AutoSize = true;
            this.lblRecursivity.Location = new System.Drawing.Point(51, 55);
            this.lblRecursivity.Name = "lblRecursivity";
            this.lblRecursivity.Size = new System.Drawing.Size(73, 13);
            this.lblRecursivity.TabIndex = 33;
            this.lblRecursivity.Text = "Max recursion";
            // 
            // updRecursivity
            // 
            this.updRecursivity.Location = new System.Drawing.Point(9, 52);
            this.updRecursivity.Name = "updRecursivity";
            this.updRecursivity.Size = new System.Drawing.Size(35, 20);
            this.updRecursivity.TabIndex = 1;
            // 
            // chkUseAllDNS
            // 
            this.chkUseAllDNS.AutoSize = true;
            this.chkUseAllDNS.Location = new System.Drawing.Point(9, 10);
            this.chkUseAllDNS.Name = "chkUseAllDNS";
            this.chkUseAllDNS.Size = new System.Drawing.Size(121, 17);
            this.chkUseAllDNS.TabIndex = 0;
            this.chkUseAllDNS.Text = "Use all DNS servers";
            this.chkUseAllDNS.UseVisualStyleBackColor = true;
            // 
            // lblUseAllDNSDescription
            // 
            this.lblUseAllDNSDescription.AutoSize = true;
            this.lblUseAllDNSDescription.Location = new System.Drawing.Point(8, 32);
            this.lblUseAllDNSDescription.Name = "lblUseAllDNSDescription";
            this.lblUseAllDNSDescription.Size = new System.Drawing.Size(337, 13);
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
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(499, 353);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Metadata";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chkResolveHosts
            // 
            this.chkResolveHosts.AutoSize = true;
            this.chkResolveHosts.Location = new System.Drawing.Point(9, 35);
            this.chkResolveHosts.Name = "chkResolveHosts";
            this.chkResolveHosts.Size = new System.Drawing.Size(227, 17);
            this.chkResolveHosts.TabIndex = 1;
            this.chkResolveHosts.Text = "Resolve host in metadata analysis (Slower)";
            this.chkResolveHosts.UseVisualStyleBackColor = true;
            // 
            // chkHEAD
            // 
            this.chkHEAD.AutoSize = true;
            this.chkHEAD.Location = new System.Drawing.Point(9, 10);
            this.chkHEAD.Name = "chkHEAD";
            this.chkHEAD.Size = new System.Drawing.Size(163, 17);
            this.chkHEAD.TabIndex = 0;
            this.chkHEAD.Text = "Use HEAD to get the file size";
            this.chkHEAD.UseVisualStyleBackColor = true;
            // 
            // updSimultaneousDownloads
            // 
            this.updSimultaneousDownloads.Location = new System.Drawing.Point(9, 64);
            this.updSimultaneousDownloads.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.updSimultaneousDownloads.Name = "updSimultaneousDownloads";
            this.updSimultaneousDownloads.Size = new System.Drawing.Size(39, 20);
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
            this.lblSimultaneousDownloads.Location = new System.Drawing.Point(54, 66);
            this.lblSimultaneousDownloads.Name = "lblSimultaneousDownloads";
            this.lblSimultaneousDownloads.Size = new System.Drawing.Size(124, 13);
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
            this.tabControl1.Location = new System.Drawing.Point(0, 9);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(507, 403);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblShodaApiKey);
            this.groupBox2.Controls.Add(this.txtShodanApiKey);
            this.groupBox2.Location = new System.Drawing.Point(13, 158);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(478, 55);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Shodan";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtBingApiKey);
            this.groupBox3.Controls.Add(this.lblBingApiKey);
            this.groupBox3.Location = new System.Drawing.Point(13, 221);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(478, 55);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Bing Search API";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtDiarioApiKey);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.txtDiarioSecret);
            this.groupBox4.Location = new System.Drawing.Point(15, 282);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(476, 79);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "DIARIO";
            // 
            // txtDiarioApiKey
            // 
            this.txtDiarioApiKey.Location = new System.Drawing.Point(85, 19);
            this.txtDiarioApiKey.Name = "txtDiarioApiKey";
            this.txtDiarioApiKey.Size = new System.Drawing.Size(387, 20);
            this.txtDiarioApiKey.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "API key:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Secret:";
            // 
            // txtDiarioSecret
            // 
            this.txtDiarioSecret.Location = new System.Drawing.Point(85, 45);
            this.txtDiarioSecret.Name = "txtDiarioSecret";
            this.txtDiarioSecret.Size = new System.Drawing.Size(387, 20);
            this.txtDiarioSecret.TabIndex = 1;
            // 
            // FormOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 448);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(367, 373);
            this.Name = "FormOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.FormOptions_Load);
            this.tabPage16.ResumeLayout(false);
            this.tabPage16.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txtDiarioApiKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDiarioSecret;
    }
}