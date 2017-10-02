namespace FOCA
{
       partial class PanelUrlsList
    {
        /// <summary>
        /// Variable del diseÃ±ador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estÃ©n utilizando.
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

        #region Código generado por el DiseÃ±ador de componentes

        /// <summary>
        /// MÃ©todo necesario para admitir el DiseÃ±ador. No se puede modificar
        /// el contenido del mÃ©todo con el editor de cÃ³digo.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportDataToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewDownloadedDocumentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.openInBrowserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.searchForBackupsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dumpDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sfdExport = new System.Windows.Forms.SaveFileDialog();
            this.lstView = new FOCA.Search.ListViewEx();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            //
            // contextMenuStrip
            //
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[]
            {
                this.exportDataToFileToolStripMenuItem,
                this.viewDownloadedDocumentToolStripMenuItem,
                this.toolStripSeparator1,
                this.openInBrowserToolStripMenuItem,
                this.toolStripSeparator2,
                this.searchForBackupsToolStripMenuItem,
                this.dumpDatabaseToolStripMenuItem
            });
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(227, 214);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            //
            // exportDataToFileToolStripMenuItem
            //
            this.exportDataToFileToolStripMenuItem.Image = global::FOCA.Properties.Resources.page_white_edit;
            this.exportDataToFileToolStripMenuItem.Name = "exportDataToFileToolStripMenuItem";
            this.exportDataToFileToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.exportDataToFileToolStripMenuItem.Text = "&Export  data to file";
            this.exportDataToFileToolStripMenuItem.Click += new System.EventHandler(this.exportDataToFileToolStripMenuItem_Click);
            //
            // viewDownloadedDocumentToolStripMenuItem
            //
            this.viewDownloadedDocumentToolStripMenuItem.Image = global::FOCA.Properties.Resources.page_white_find;
            this.viewDownloadedDocumentToolStripMenuItem.Name = "viewDownloadedDocumentToolStripMenuItem";
            this.viewDownloadedDocumentToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.viewDownloadedDocumentToolStripMenuItem.Text = "&View downloaded document";
            this.viewDownloadedDocumentToolStripMenuItem.Click += new System.EventHandler(this.viewDownloadedDocumentToolStripMenuItem_Click);
            //
            // toolStripSeparator1
            //
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(223, 6);
            //
            // openInBrowserToolStripMenuItem
            //
            this.openInBrowserToolStripMenuItem.Image = global::FOCA.Properties.Resources.world_link;
            this.openInBrowserToolStripMenuItem.Name = "openInBrowserToolStripMenuItem";
            this.openInBrowserToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.openInBrowserToolStripMenuItem.Text = "&Open in browser";
            this.openInBrowserToolStripMenuItem.Click += new System.EventHandler(this.openInBrowserToolStripMenuItem_Click);
            //
            // toolStripSeparator2
            //
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(223, 6);
            //
            // searchForBackupsToolStripMenuItem
            //
            this.searchForBackupsToolStripMenuItem.Image = global::FOCA.Properties.Resources.eye;
            this.searchForBackupsToolStripMenuItem.Name = "searchForBackupsToolStripMenuItem";
            this.searchForBackupsToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.searchForBackupsToolStripMenuItem.Text = "&Search for backups";
            this.searchForBackupsToolStripMenuItem.Click += new System.EventHandler(this.searchForBackupsToolStripMenuItem_Click);
            //
            // dumpDatabaseToolStripMenuItem
            //
            this.dumpDatabaseToolStripMenuItem.Image = global::FOCA.Properties.Resources.database_lightning;
            this.dumpDatabaseToolStripMenuItem.Name = "dumpDatabaseToolStripMenuItem";
            this.dumpDatabaseToolStripMenuItem.Size = new System.Drawing.Size(226, 22);
            this.dumpDatabaseToolStripMenuItem.Text = "Dump database";
            this.dumpDatabaseToolStripMenuItem.Visible = false;
            //
            // sfdExport
            //
            this.sfdExport.DefaultExt = "txt";
            this.sfdExport.Filter = "Text files|*.txt|All files|*.*";
            //
            // lstView
            //
            this.lstView.ContextMenuStrip = this.contextMenuStrip;
            this.lstView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstView.FullRowSelect = true;
            this.lstView.GridLines = true;
            this.lstView.Location = new System.Drawing.Point(0, 0);
            this.lstView.Name = "lstView";
            this.lstView.Size = new System.Drawing.Size(475, 224);
            this.lstView.TabIndex = 0;
            this.lstView.UseCompatibleStateImageBehavior = false;
            this.lstView.View = System.Windows.Forms.View.Details;
            this.lstView.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lstView_ColumnClick);
            //
            // PanelUrlsList
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lstView);
            this.Name = "PanelUrlsList";
            this.Size = new System.Drawing.Size(475, 224);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public FOCA.Search.ListViewEx lstView;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem exportDataToFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewDownloadedDocumentToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog sfdExport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem openInBrowserToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem searchForBackupsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dumpDatabaseToolStripMenuItem;
    }

}
