namespace FOCA
{
    partial class PanelSoftwareSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelSoftwareSearch));
            this.lblSearchType = new System.Windows.Forms.Label();
            this.panelSearchTypes = new System.Windows.Forms.Panel();
            this.cbTechRecognition = new FOCA.ModifiedComponents.CheckedButton();
            this.cbFingerPrintingHTTP = new FOCA.ModifiedComponents.CheckedButton();
            this.cbFingerPrintingSMTP = new FOCA.ModifiedComponents.CheckedButton();
            this.cbFingerprintingShodan = new FOCA.ModifiedComponents.CheckedButton();
            this.panelInformation = new System.Windows.Forms.Panel();
            this.panelTechRecognition = new System.Windows.Forms.Panel();
            this.btnUnselectAll = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.lblTechSearch = new System.Windows.Forms.Label();
            this.clTechExtensions = new System.Windows.Forms.CheckedListBox();
            this.lblReconDescription = new System.Windows.Forms.Label();
            this.lblTechRecon = new System.Windows.Forms.Label();
            this.panelFPrintingShodan = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panelFPrintingSMTP = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panelFPrintingHTTP = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lbCurrentSearch = new System.Windows.Forms.Label();
            this.btSkip = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.picFOCA = new System.Windows.Forms.PictureBox();
            this.panelSearchTypes.SuspendLayout();
            this.panelInformation.SuspendLayout();
            this.panelTechRecognition.SuspendLayout();
            this.panelFPrintingShodan.SuspendLayout();
            this.panelFPrintingSMTP.SuspendLayout();
            this.panelFPrintingHTTP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFOCA)).BeginInit();
            this.SuspendLayout();
            //
            // lblSearchType
            //
            this.lblSearchType.AutoSize = true;
            this.lblSearchType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchType.Location = new System.Drawing.Point(7, 107);
            this.lblSearchType.Name = "lblSearchType";
            this.lblSearchType.Size = new System.Drawing.Size(113, 13);
            this.lblSearchType.TabIndex = 78;
            this.lblSearchType.Text = "Select search type";
            //
            // panelSearchTypes
            //
            this.panelSearchTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelSearchTypes.Controls.Add(this.cbTechRecognition);
            this.panelSearchTypes.Controls.Add(this.cbFingerPrintingHTTP);
            this.panelSearchTypes.Controls.Add(this.cbFingerPrintingSMTP);
            this.panelSearchTypes.Controls.Add(this.cbFingerprintingShodan);
            this.panelSearchTypes.Controls.Add(this.panelInformation);
            this.panelSearchTypes.Location = new System.Drawing.Point(10, 123);
            this.panelSearchTypes.Name = "panelSearchTypes";
            this.panelSearchTypes.Size = new System.Drawing.Size(553, 263);
            this.panelSearchTypes.TabIndex = 81;
            //
            // cbTechRecognition
            //
            this.cbTechRecognition.BackColor = System.Drawing.SystemColors.Control;
            this.cbTechRecognition.BackColorChecked = System.Drawing.SystemColors.Control;
            this.cbTechRecognition.BackColorUnchecked = System.Drawing.SystemColors.Control;
            this.cbTechRecognition.Checked = true;
            this.cbTechRecognition.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbTechRecognition.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbTechRecognition.Location = new System.Drawing.Point(5, 120);
            this.cbTechRecognition.Name = "cbTechRecognition";
            this.cbTechRecognition.Size = new System.Drawing.Size(108, 29);
            this.cbTechRecognition.TabIndex = 20;
            this.cbTechRecognition.Text = "Tech Recognition";
            this.cbTechRecognition.UseVisualStyleBackColor = false;
            this.cbTechRecognition.Click += new System.EventHandler(this.checkedButtonTechRecognition_Click);
            //
            // cbFingerPrintingHTTP
            //
            this.cbFingerPrintingHTTP.BackColor = System.Drawing.SystemColors.Control;
            this.cbFingerPrintingHTTP.BackColorChecked = System.Drawing.SystemColors.Control;
            this.cbFingerPrintingHTTP.BackColorUnchecked = System.Drawing.SystemColors.Control;
            this.cbFingerPrintingHTTP.Checked = true;
            this.cbFingerPrintingHTTP.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbFingerPrintingHTTP.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbFingerPrintingHTTP.Location = new System.Drawing.Point(5, 0);
            this.cbFingerPrintingHTTP.Name = "cbFingerPrintingHTTP";
            this.cbFingerPrintingHTTP.Size = new System.Drawing.Size(110, 30);
            this.cbFingerPrintingHTTP.TabIndex = 19;
            this.cbFingerPrintingHTTP.Text = "FPrinting HTTP";
            this.cbFingerPrintingHTTP.UseVisualStyleBackColor = false;
            this.cbFingerPrintingHTTP.Click += new System.EventHandler(this.checkedButtonFingerPrintingHTTP_Click);
            //
            // cbFingerPrintingSMTP
            //
            this.cbFingerPrintingSMTP.BackColor = System.Drawing.SystemColors.Control;
            this.cbFingerPrintingSMTP.BackColorChecked = System.Drawing.SystemColors.Control;
            this.cbFingerPrintingSMTP.BackColorUnchecked = System.Drawing.SystemColors.Control;
            this.cbFingerPrintingSMTP.Checked = true;
            this.cbFingerPrintingSMTP.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbFingerPrintingSMTP.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbFingerPrintingSMTP.Location = new System.Drawing.Point(5, 40);
            this.cbFingerPrintingSMTP.Name = "cbFingerPrintingSMTP";
            this.cbFingerPrintingSMTP.Size = new System.Drawing.Size(108, 29);
            this.cbFingerPrintingSMTP.TabIndex = 18;
            this.cbFingerPrintingSMTP.Text = "FPrinting SMTP";
            this.cbFingerPrintingSMTP.UseVisualStyleBackColor = false;
            this.cbFingerPrintingSMTP.Click += new System.EventHandler(this.checkedButtonFingerPrintingSMTP_Click);
            //
            // cbFingerprintingShodan
            //
            this.cbFingerprintingShodan.BackColor = System.Drawing.SystemColors.Control;
            this.cbFingerprintingShodan.BackColorChecked = System.Drawing.SystemColors.Control;
            this.cbFingerprintingShodan.BackColorUnchecked = System.Drawing.SystemColors.Control;
            this.cbFingerprintingShodan.Checked = true;
            this.cbFingerprintingShodan.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbFingerprintingShodan.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cbFingerprintingShodan.Location = new System.Drawing.Point(5, 80);
            this.cbFingerprintingShodan.Name = "cbFingerprintingShodan";
            this.cbFingerprintingShodan.Size = new System.Drawing.Size(108, 29);
            this.cbFingerprintingShodan.TabIndex = 17;
            this.cbFingerprintingShodan.Text = "Shodan fingerprinting";
            this.cbFingerprintingShodan.UseVisualStyleBackColor = false;
            this.cbFingerprintingShodan.Click += new System.EventHandler(this.checkedButtonFingerPrintingShodan_Click);
            //
            // panelInformation
            //
            this.panelInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelInformation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelInformation.Controls.Add(this.panelTechRecognition);
            this.panelInformation.Controls.Add(this.panelFPrintingShodan);
            this.panelInformation.Controls.Add(this.panelFPrintingSMTP);
            this.panelInformation.Controls.Add(this.panelFPrintingHTTP);
            this.panelInformation.Location = new System.Drawing.Point(123, 0);
            this.panelInformation.Name = "panelInformation";
            this.panelInformation.Size = new System.Drawing.Size(430, 263);
            this.panelInformation.TabIndex = 12;
            //
            // panelTechRecognition
            //
            this.panelTechRecognition.Controls.Add(this.btnUnselectAll);
            this.panelTechRecognition.Controls.Add(this.btnSelectAll);
            this.panelTechRecognition.Controls.Add(this.lblTechSearch);
            this.panelTechRecognition.Controls.Add(this.clTechExtensions);
            this.panelTechRecognition.Controls.Add(this.lblReconDescription);
            this.panelTechRecognition.Controls.Add(this.lblTechRecon);
            this.panelTechRecognition.Location = new System.Drawing.Point(34, 24);
            this.panelTechRecognition.Name = "panelTechRecognition";
            this.panelTechRecognition.Size = new System.Drawing.Size(396, 240);
            this.panelTechRecognition.TabIndex = 3;
            this.panelTechRecognition.Visible = false;
            //
            // btnUnselectAll
            //
            this.btnUnselectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUnselectAll.Image = global::FOCA.Properties.Resources.delete;
            this.btnUnselectAll.Location = new System.Drawing.Point(284, 183);
            this.btnUnselectAll.Name = "btnUnselectAll";
            this.btnUnselectAll.Size = new System.Drawing.Size(94, 25);
            this.btnUnselectAll.TabIndex = 7;
            this.btnUnselectAll.Text = "&Uncheck all";
            this.btnUnselectAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUnselectAll.UseVisualStyleBackColor = true;
            this.btnUnselectAll.Click += new System.EventHandler(this.btUnselectAll_Click);
            //
            // btnSelectAll
            //
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectAll.Image = global::FOCA.Properties.Resources.tick;
            this.btnSelectAll.Location = new System.Drawing.Point(184, 182);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(94, 27);
            this.btnSelectAll.TabIndex = 6;
            this.btnSelectAll.Text = "&Select all";
            this.btnSelectAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btSelectAll_Click);
            //
            // lblTechSearch
            //
            this.lblTechSearch.AutoSize = true;
            this.lblTechSearch.Location = new System.Drawing.Point(19, 87);
            this.lblTechSearch.Name = "lblTechSearch";
            this.lblTechSearch.Size = new System.Drawing.Size(160, 13);
            this.lblTechSearch.TabIndex = 5;
            this.lblTechSearch.Text = "Technologies to be searched for";
            //
            // clTechExtensions
            //
            this.clTechExtensions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clTechExtensions.CheckOnClick = true;
            this.clTechExtensions.FormattingEnabled = true;
            this.clTechExtensions.Location = new System.Drawing.Point(22, 113);
            this.clTechExtensions.MultiColumn = true;
            this.clTechExtensions.Name = "clTechExtensions";
            this.clTechExtensions.Size = new System.Drawing.Size(356, 64);
            this.clTechExtensions.TabIndex = 4;
            this.clTechExtensions.SelectedIndexChanged += new System.EventHandler(this.clTechExtensions_SelectedIndexChanged);
            //
            // lblReconDescription
            //
            this.lblReconDescription.AutoSize = true;
            this.lblReconDescription.Location = new System.Drawing.Point(15, 40);
            this.lblReconDescription.Name = "lblReconDescription";
            this.lblReconDescription.Size = new System.Drawing.Size(165, 26);
            this.lblReconDescription.TabIndex = 1;
            this.lblReconDescription.Text = "Technology recognition based on\r\n        Google searches";
            //
            // lblTechRecon
            //
            this.lblTechRecon.AutoSize = true;
            this.lblTechRecon.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTechRecon.Location = new System.Drawing.Point(12, 13);
            this.lblTechRecon.Name = "lblTechRecon";
            this.lblTechRecon.Size = new System.Drawing.Size(145, 13);
            this.lblTechRecon.TabIndex = 0;
            this.lblTechRecon.Text = "Technology Recognition";
            //
            // panelFPrintingShodan
            //
            this.panelFPrintingShodan.Controls.Add(this.label15);
            this.panelFPrintingShodan.Controls.Add(this.label14);
            this.panelFPrintingShodan.Controls.Add(this.label4);
            this.panelFPrintingShodan.Location = new System.Drawing.Point(16, 155);
            this.panelFPrintingShodan.Name = "panelFPrintingShodan";
            this.panelFPrintingShodan.Size = new System.Drawing.Size(200, 109);
            this.panelFPrintingShodan.TabIndex = 2;
            this.panelFPrintingShodan.Visible = false;
            //
            // label15
            //
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(48, 63);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(74, 13);
            this.label15.TabIndex = 3;
            this.label15.Text = "Server banner";
            //
            // label14
            //
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(15, 40);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(169, 13);
            this.label14.TabIndex = 2;
            this.label14.Text = "Web server fingerprinting based on";
            //
            // label4
            //
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Shodan fingerprinting";
            //
            // panelFPrintingSMTP
            //
            this.panelFPrintingSMTP.Controls.Add(this.label13);
            this.panelFPrintingSMTP.Controls.Add(this.label9);
            this.panelFPrintingSMTP.Controls.Add(this.label3);
            this.panelFPrintingSMTP.Location = new System.Drawing.Point(222, 10);
            this.panelFPrintingSMTP.Name = "panelFPrintingSMTP";
            this.panelFPrintingSMTP.Size = new System.Drawing.Size(208, 139);
            this.panelFPrintingSMTP.TabIndex = 1;
            this.panelFPrintingSMTP.Visible = false;
            //
            // label13
            //
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(15, 38);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(147, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "SMTP fingerprinting based on";
            //
            // label9
            //
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(54, 60);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Server banner";
            //
            // label3
            //
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "SMTP fingerprinting";
            //
            // panelFPrintingHTTP
            //
            this.panelFPrintingHTTP.Controls.Add(this.label12);
            this.panelFPrintingHTTP.Controls.Add(this.label11);
            this.panelFPrintingHTTP.Controls.Add(this.label10);
            this.panelFPrintingHTTP.Controls.Add(this.label8);
            this.panelFPrintingHTTP.Controls.Add(this.label1);
            this.panelFPrintingHTTP.Location = new System.Drawing.Point(16, 10);
            this.panelFPrintingHTTP.Name = "panelFPrintingHTTP";
            this.panelFPrintingHTTP.Size = new System.Drawing.Size(200, 139);
            this.panelFPrintingHTTP.TabIndex = 0;
            this.panelFPrintingHTTP.Visible = false;
            //
            // label12
            //
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(48, 101);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(93, 13);
            this.label12.TabIndex = 4;
            this.label12.Text = "ASPX\'s responses";
            //
            // label11
            //
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(49, 81);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(94, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "404 message error";
            //
            // label10
            //
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(48, 60);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "Server banner";
            //
            // label8
            //
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 38);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(169, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Web server fingerprinting based on";
            //
            // label1
            //
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "HTTP fingerprinting";
            //
            // lbCurrentSearch
            //
            this.lbCurrentSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lbCurrentSearch.AutoSize = true;
            this.lbCurrentSearch.Location = new System.Drawing.Point(17, 406);
            this.lbCurrentSearch.Name = "lbCurrentSearch";
            this.lbCurrentSearch.Size = new System.Drawing.Size(108, 13);
            this.lbCurrentSearch.TabIndex = 83;
            this.lbCurrentSearch.Text = "Current search: None";
            //
            // btSkip
            //
            this.btSkip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btSkip.Enabled = false;
            this.btSkip.Image = global::FOCA.Properties.Resources.skip;
            this.btSkip.Location = new System.Drawing.Point(397, 395);
            this.btSkip.Name = "btSkip";
            this.btSkip.Size = new System.Drawing.Size(80, 35);
            this.btSkip.TabIndex = 0;
            this.btSkip.Text = "S&kip";
            this.btSkip.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btSkip.UseVisualStyleBackColor = true;
            this.btSkip.Click += new System.EventHandler(this.btSkip_Click);
            //
            // buttonStart
            //
            this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStart.Image = ((System.Drawing.Image)(resources.GetObject("buttonStart.Image")));
            this.buttonStart.Location = new System.Drawing.Point(483, 395);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(80, 35);
            this.buttonStart.TabIndex = 82;
            this.buttonStart.Text = "&Start";
            this.buttonStart.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonStart.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            //
            // picFOCA
            //
            this.picFOCA.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.picFOCA.BackColor = System.Drawing.SystemColors.Control;
            this.picFOCA.Image = global::FOCA.Properties.Resources.newlogo;
            this.picFOCA.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.picFOCA.Location = new System.Drawing.Point(3, 4);
            this.picFOCA.Name = "picFOCA";
            this.picFOCA.Size = new System.Drawing.Size(261, 100);
            this.picFOCA.TabIndex = 84;
            this.picFOCA.TabStop = false;
            //
            // PanelSoftwareSearch
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btSkip);
            this.Controls.Add(this.lbCurrentSearch);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.panelSearchTypes);
            this.Controls.Add(this.lblSearchType);
            this.Controls.Add(this.picFOCA);
            this.Name = "PanelSoftwareSearch";
            this.Size = new System.Drawing.Size(575, 442);
            this.Load += new System.EventHandler(this.PanelSoftwareSearch_Load);
            this.panelSearchTypes.ResumeLayout(false);
            this.panelInformation.ResumeLayout(false);
            this.panelTechRecognition.ResumeLayout(false);
            this.panelTechRecognition.PerformLayout();
            this.panelFPrintingShodan.ResumeLayout(false);
            this.panelFPrintingShodan.PerformLayout();
            this.panelFPrintingSMTP.ResumeLayout(false);
            this.panelFPrintingSMTP.PerformLayout();
            this.panelFPrintingHTTP.ResumeLayout(false);
            this.panelFPrintingHTTP.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picFOCA)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSearchType;
        private System.Windows.Forms.Panel panelSearchTypes;
        public FOCA.ModifiedComponents.CheckedButton cbFingerPrintingHTTP;
        public FOCA.ModifiedComponents.CheckedButton cbFingerPrintingSMTP;
        public FOCA.ModifiedComponents.CheckedButton cbFingerprintingShodan;
        private System.Windows.Forms.Panel panelInformation;
        public FOCA.ModifiedComponents.CheckedButton cbTechRecognition;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Panel panelTechRecognition;
        private System.Windows.Forms.Panel panelFPrintingShodan;
        private System.Windows.Forms.Panel panelFPrintingSMTP;
        private System.Windows.Forms.Panel panelFPrintingHTTP;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTechRecon;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btSkip;
        private System.Windows.Forms.Label lbCurrentSearch;
        private System.Windows.Forms.Label lblReconDescription;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnUnselectAll;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Label lblTechSearch;
        private System.Windows.Forms.CheckedListBox clTechExtensions;
        private System.Windows.Forms.PictureBox picFOCA;
    }
}
