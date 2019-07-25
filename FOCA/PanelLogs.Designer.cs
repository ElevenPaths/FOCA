using System.Drawing;

namespace FOCA
{
    partial class PanelLogs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelLogs));
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.buttonAutoScroll = new System.Windows.Forms.Button();
            this.buttonSaveToFile = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.gbCriticidad = new System.Windows.Forms.GroupBox();
            this.lbUnCheckAllCriti = new System.Windows.Forms.Label();
            this.lbCheckAllCriti = new System.Windows.Forms.Label();
            this.cblbCritically = new System.Windows.Forms.CheckedListBox();
            this.gbModulos = new System.Windows.Forms.GroupBox();
            this.lbUncheckAllModule = new System.Windows.Forms.Label();
            this.lbCheckAllModule = new System.Windows.Forms.Label();
            this.cblbModules = new System.Windows.Forms.CheckedListBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.panelFilter = new System.Windows.Forms.Panel();
            this.btShow = new System.Windows.Forms.Button();
            this.PanelInfo = new System.Windows.Forms.Panel();
            this.listViewLog = new FOCA.Search.ListViewEx();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.gbCriticidad.SuspendLayout();
            this.gbModulos.SuspendLayout();
            this.panelFilter.SuspendLayout();
            this.PanelInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Text File|*.txt";
            // 
            // buttonAutoScroll
            // 
            this.buttonAutoScroll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAutoScroll.Image = ((System.Drawing.Image)(resources.GetObject("buttonAutoScroll.Image")));
            this.buttonAutoScroll.Location = new System.Drawing.Point(121, 154);
            this.buttonAutoScroll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonAutoScroll.Name = "buttonAutoScroll";
            this.buttonAutoScroll.Size = new System.Drawing.Size(199, 30);
            this.buttonAutoScroll.TabIndex = 1;
            this.buttonAutoScroll.Text = "&Deactivate AutoScroll";
            this.buttonAutoScroll.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonAutoScroll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonAutoScroll.UseVisualStyleBackColor = true;
            this.buttonAutoScroll.Click += new System.EventHandler(this.buttonAutoScroll_Click);
            // 
            // buttonSaveToFile
            // 
            this.buttonSaveToFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveToFile.Image = ((System.Drawing.Image)(resources.GetObject("buttonSaveToFile.Image")));
            this.buttonSaveToFile.Location = new System.Drawing.Point(630, 154);
            this.buttonSaveToFile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSaveToFile.Name = "buttonSaveToFile";
            this.buttonSaveToFile.Size = new System.Drawing.Size(144, 30);
            this.buttonSaveToFile.TabIndex = 3;
            this.buttonSaveToFile.Text = "&Save log to File";
            this.buttonSaveToFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonSaveToFile.UseVisualStyleBackColor = true;
            this.buttonSaveToFile.Click += new System.EventHandler(this.buttonSaveToFile_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonClear.Image = ((System.Drawing.Image)(resources.GetObject("buttonClear.Image")));
            this.buttonClear.Location = new System.Drawing.Point(336, 154);
            this.buttonClear.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(87, 30);
            this.buttonClear.TabIndex = 2;
            this.buttonClear.Text = "&Clear";
            this.buttonClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // gbCriticidad
            // 
            this.gbCriticidad.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbCriticidad.Controls.Add(this.lbUnCheckAllCriti);
            this.gbCriticidad.Controls.Add(this.lbCheckAllCriti);
            this.gbCriticidad.Controls.Add(this.cblbCritically);
            this.gbCriticidad.Location = new System.Drawing.Point(23, 4);
            this.gbCriticidad.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbCriticidad.Name = "gbCriticidad";
            this.gbCriticidad.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbCriticidad.Size = new System.Drawing.Size(160, 180);
            this.gbCriticidad.TabIndex = 93;
            this.gbCriticidad.TabStop = false;
            this.gbCriticidad.Text = "Severity filter";
            // 
            // lbUnCheckAllCriti
            // 
            this.lbUnCheckAllCriti.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbUnCheckAllCriti.ForeColor = System.Drawing.Color.Blue;
            this.lbUnCheckAllCriti.Location = new System.Drawing.Point(4, 158);
            this.lbUnCheckAllCriti.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbUnCheckAllCriti.Name = "lbUnCheckAllCriti";
            this.lbUnCheckAllCriti.Size = new System.Drawing.Size(152, 16);
            this.lbUnCheckAllCriti.TabIndex = 4;
            this.lbUnCheckAllCriti.Text = "Uncheck all";
            this.lbUnCheckAllCriti.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbUnCheckAllCriti.Click += new System.EventHandler(this.lbUnCheckAllCriti_Click);
            this.lbUnCheckAllCriti.MouseEnter += new System.EventHandler(this.Colorea);
            this.lbUnCheckAllCriti.MouseLeave += new System.EventHandler(this.Descolorea);
            // 
            // lbCheckAllCriti
            // 
            this.lbCheckAllCriti.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCheckAllCriti.ForeColor = System.Drawing.Color.Blue;
            this.lbCheckAllCriti.Location = new System.Drawing.Point(4, 142);
            this.lbCheckAllCriti.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbCheckAllCriti.Name = "lbCheckAllCriti";
            this.lbCheckAllCriti.Size = new System.Drawing.Size(152, 16);
            this.lbCheckAllCriti.TabIndex = 3;
            this.lbCheckAllCriti.Text = "Check all";
            this.lbCheckAllCriti.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbCheckAllCriti.Click += new System.EventHandler(this.lbCheckAllCriti_Click);
            this.lbCheckAllCriti.MouseEnter += new System.EventHandler(this.Colorea);
            this.lbCheckAllCriti.MouseLeave += new System.EventHandler(this.Descolorea);
            // 
            // cblbCritically
            // 
            this.cblbCritically.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cblbCritically.CheckOnClick = true;
            this.cblbCritically.FormattingEnabled = true;
            this.cblbCritically.Location = new System.Drawing.Point(4, 20);
            this.cblbCritically.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cblbCritically.Name = "cblbCritically";
            this.cblbCritically.Size = new System.Drawing.Size(151, 89);
            this.cblbCritically.TabIndex = 1;
            this.cblbCritically.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cblbModules_ItemCheck);
            // 
            // gbModulos
            // 
            this.gbModulos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gbModulos.Controls.Add(this.lbUncheckAllModule);
            this.gbModulos.Controls.Add(this.lbCheckAllModule);
            this.gbModulos.Controls.Add(this.cblbModules);
            this.gbModulos.Location = new System.Drawing.Point(185, 4);
            this.gbModulos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbModulos.Name = "gbModulos";
            this.gbModulos.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.gbModulos.Size = new System.Drawing.Size(160, 180);
            this.gbModulos.TabIndex = 94;
            this.gbModulos.TabStop = false;
            this.gbModulos.Text = "Module filter";
            // 
            // lbUncheckAllModule
            // 
            this.lbUncheckAllModule.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbUncheckAllModule.ForeColor = System.Drawing.Color.Blue;
            this.lbUncheckAllModule.Location = new System.Drawing.Point(5, 158);
            this.lbUncheckAllModule.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbUncheckAllModule.Name = "lbUncheckAllModule";
            this.lbUncheckAllModule.Size = new System.Drawing.Size(151, 16);
            this.lbUncheckAllModule.TabIndex = 2;
            this.lbUncheckAllModule.Text = "Uncheck all";
            this.lbUncheckAllModule.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbUncheckAllModule.Click += new System.EventHandler(this.lbUncheckAllModule_Click);
            this.lbUncheckAllModule.MouseEnter += new System.EventHandler(this.Colorea);
            this.lbUncheckAllModule.MouseLeave += new System.EventHandler(this.Descolorea);
            // 
            // lbCheckAllModule
            // 
            this.lbCheckAllModule.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCheckAllModule.ForeColor = System.Drawing.Color.Blue;
            this.lbCheckAllModule.Location = new System.Drawing.Point(7, 142);
            this.lbCheckAllModule.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbCheckAllModule.Name = "lbCheckAllModule";
            this.lbCheckAllModule.Size = new System.Drawing.Size(146, 16);
            this.lbCheckAllModule.TabIndex = 1;
            this.lbCheckAllModule.Text = "Check all";
            this.lbCheckAllModule.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lbCheckAllModule.Click += new System.EventHandler(this.lbCheckAllModule_Click);
            this.lbCheckAllModule.MouseEnter += new System.EventHandler(this.Colorea);
            this.lbCheckAllModule.MouseLeave += new System.EventHandler(this.Descolorea);
            // 
            // cblbModules
            // 
            this.cblbModules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cblbModules.CheckOnClick = true;
            this.cblbModules.FormattingEnabled = true;
            this.cblbModules.Location = new System.Drawing.Point(4, 20);
            this.cblbModules.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cblbModules.Name = "cblbModules";
            this.cblbModules.Size = new System.Drawing.Size(151, 89);
            this.cblbModules.Sorted = true;
            this.cblbModules.TabIndex = 0;
            this.cblbModules.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.cblbModules_ItemCheck);
            // 
            // panelFilter
            // 
            this.panelFilter.Controls.Add(this.gbCriticidad);
            this.panelFilter.Controls.Add(this.gbModulos);
            this.panelFilter.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelFilter.Location = new System.Drawing.Point(0, 0);
            this.panelFilter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Size = new System.Drawing.Size(354, 187);
            this.panelFilter.TabIndex = 95;
            // 
            // btShow
            // 
            this.btShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btShow.Image = ((System.Drawing.Image)(resources.GetObject("btShow.Image")));
            this.btShow.Location = new System.Drawing.Point(18, 154);
            this.btShow.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btShow.Name = "btShow";
            this.btShow.Size = new System.Drawing.Size(95, 30);
            this.btShow.TabIndex = 95;
            this.btShow.Text = "Settings";
            this.btShow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btShow.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btShow.UseVisualStyleBackColor = true;
            this.btShow.Click += new System.EventHandler(this.btShow_Click);
            // 
            // PanelInfo
            // 
            this.PanelInfo.Controls.Add(this.btShow);
            this.PanelInfo.Controls.Add(this.listViewLog);
            this.PanelInfo.Controls.Add(this.buttonAutoScroll);
            this.PanelInfo.Controls.Add(this.buttonSaveToFile);
            this.PanelInfo.Controls.Add(this.buttonClear);
            this.PanelInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelInfo.Location = new System.Drawing.Point(354, 0);
            this.PanelInfo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.PanelInfo.Name = "PanelInfo";
            this.PanelInfo.Size = new System.Drawing.Size(779, 187);
            this.PanelInfo.TabIndex = 96;
            // 
            // listViewLog
            // 
            this.listViewLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewLog.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.listViewLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader2});
            this.listViewLog.FullRowSelect = true;
            this.listViewLog.GridLines = true;
            this.listViewLog.LabelWrap = false;
            this.listViewLog.Location = new System.Drawing.Point(18, 0);
            this.listViewLog.Margin = new System.Windows.Forms.Padding(4);
            this.listViewLog.Name = "listViewLog";
            this.listViewLog.Size = new System.Drawing.Size(746, 146);
            this.listViewLog.TabIndex = 0;
            this.listViewLog.UseCompatibleStateImageBehavior = false;
            this.listViewLog.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Time";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Source";
            this.columnHeader3.Width = 101;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Severity";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Message";
            this.columnHeader2.Width = 500;
            // 
            // PanelLogs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PanelInfo);
            this.Controls.Add(this.panelFilter);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PanelLogs";
            this.Size = new System.Drawing.Size(1133, 187);
            this.Load += new System.EventHandler(this.PanelLogs_Load);
            this.VisibleChanged += new System.EventHandler(this.PanelLogs_VisibleChanged);
            this.gbCriticidad.ResumeLayout(false);
            this.gbModulos.ResumeLayout(false);
            this.panelFilter.ResumeLayout(false);
            this.PanelInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSaveToFile;
        private System.Windows.Forms.Button buttonClear;
        private FOCA.Search.ListViewEx listViewLog;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button buttonAutoScroll;
        private System.Windows.Forms.GroupBox gbCriticidad;
        private System.Windows.Forms.GroupBox gbModulos;
        private System.Windows.Forms.CheckedListBox cblbModules;
        private System.Windows.Forms.CheckedListBox cblbCritically;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Label lbUncheckAllModule;
        private System.Windows.Forms.Label lbCheckAllModule;
        private System.Windows.Forms.Label lbUnCheckAllCriti;
        private System.Windows.Forms.Label lbCheckAllCriti;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Panel panelFilter;
        private System.Windows.Forms.Button btShow;
        private System.Windows.Forms.Panel PanelInfo;
    }
}
