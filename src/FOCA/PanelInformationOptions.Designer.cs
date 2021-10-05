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
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.lboLog = new System.Windows.Forms.ListBox();
            this.TabOptions.SuspendLayout();
            this.tabPageRecon.SuspendLayout();
            this.tabPageCrawling.SuspendLayout();
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
            this.TabOptions.Controls.Add(this.tabPage6);
            this.TabOptions.Location = new System.Drawing.Point(5, 3);
            this.TabOptions.Name = "TabOptions";
            this.TabOptions.SelectedIndex = 0;
            this.TabOptions.Size = new System.Drawing.Size(590, 111);
            this.TabOptions.TabIndex = 93;
            // 
            // tabPageRecon
            // 
            this.tabPageRecon.Controls.Add(this.btnTechnologyRecon);
            this.tabPageRecon.Location = new System.Drawing.Point(4, 25);
            this.tabPageRecon.Name = "tabPageRecon";
            this.tabPageRecon.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPageRecon.Size = new System.Drawing.Size(582, 82);
            this.tabPageRecon.TabIndex = 0;
            this.tabPageRecon.Text = "Technology recognition";
            this.tabPageRecon.UseVisualStyleBackColor = true;
            // 
            // btnTechnologyRecon
            // 
            this.btnTechnologyRecon.Enabled = false;
            this.btnTechnologyRecon.Image = ((System.Drawing.Image)(resources.GetObject("btnTechnologyRecon.Image")));
            this.btnTechnologyRecon.Location = new System.Drawing.Point(6, 6);
            this.btnTechnologyRecon.Name = "btnTechnologyRecon";
            this.btnTechnologyRecon.Size = new System.Drawing.Size(106, 41);
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
            this.tabPageCrawling.Location = new System.Drawing.Point(4, 25);
            this.tabPageCrawling.Name = "tabPageCrawling";
            this.tabPageCrawling.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabPageCrawling.Size = new System.Drawing.Size(582, 82);
            this.tabPageCrawling.TabIndex = 1;
            this.tabPageCrawling.Text = "Crawling";
            this.tabPageCrawling.UseVisualStyleBackColor = true;
            // 
            // btnAllLinksDuckduckgo
            // 
            this.btnAllLinksDuckduckgo.Image = ((System.Drawing.Image)(resources.GetObject("btnAllLinksDuckduckgo.Image")));
            this.btnAllLinksDuckduckgo.Location = new System.Drawing.Point(231, 7);
            this.btnAllLinksDuckduckgo.Name = "btnAllLinksDuckduckgo";
            this.btnAllLinksDuckduckgo.Size = new System.Drawing.Size(136, 41);
            this.btnAllLinksDuckduckgo.TabIndex = 91;
            this.btnAllLinksDuckduckgo.Text = "DuckDuckGo crawling";
            this.btnAllLinksDuckduckgo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAllLinksDuckduckgo.UseVisualStyleBackColor = true;
            this.btnAllLinksDuckduckgo.Click += new System.EventHandler(this.btAllLinksDuckduckGo_Click);
            // 
            // btnAllLinksGoogle
            // 
            this.btnAllLinksGoogle.Enabled = false;
            this.btnAllLinksGoogle.Image = ((System.Drawing.Image)(resources.GetObject("btnAllLinksGoogle.Image")));
            this.btnAllLinksGoogle.Location = new System.Drawing.Point(6, 6);
            this.btnAllLinksGoogle.Name = "btnAllLinksGoogle";
            this.btnAllLinksGoogle.Size = new System.Drawing.Size(106, 41);
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
            this.btnAllLinksBing.Location = new System.Drawing.Point(118, 6);
            this.btnAllLinksBing.Name = "btnAllLinksBing";
            this.btnAllLinksBing.Size = new System.Drawing.Size(106, 41);
            this.btnAllLinksBing.TabIndex = 90;
            this.btnAllLinksBing.Text = "Bing crawling";
            this.btnAllLinksBing.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAllLinksBing.UseVisualStyleBackColor = true;
            this.btnAllLinksBing.Click += new System.EventHandler(this.btAllLinksBing_Click);
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.lboLog);
            this.tabPage6.Location = new System.Drawing.Point(4, 25);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Size = new System.Drawing.Size(582, 82);
            this.tabPage6.TabIndex = 5;
            this.tabPage6.Text = "Log";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // lboLog
            // 
            this.lboLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lboLog.FormattingEnabled = true;
            this.lboLog.Location = new System.Drawing.Point(0, 0);
            this.lboLog.Name = "lboLog";
            this.lboLog.ScrollAlwaysVisible = true;
            this.lboLog.Size = new System.Drawing.Size(582, 82);
            this.lboLog.TabIndex = 94;
            // 
            // PanelInformationOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.TabOptions);
            this.Name = "PanelInformationOptions";
            this.Size = new System.Drawing.Size(597, 117);
            this.TabOptions.ResumeLayout(false);
            this.tabPageRecon.ResumeLayout(false);
            this.tabPageCrawling.ResumeLayout(false);
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
        private System.Windows.Forms.ListBox lboLog;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.Button btnAllLinksDuckduckgo;
    }
}
