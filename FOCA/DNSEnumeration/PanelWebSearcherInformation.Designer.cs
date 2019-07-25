namespace FOCA
{
    partial class PanelWebSearcherInformation
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
            this.cboEngine = new System.Windows.Forms.ComboBox();
            this.lblSearchEngine = new System.Windows.Forms.Label();
            this.lblWebSearcherDescription = new System.Windows.Forms.Label();
            this.lblWebSearcher = new System.Windows.Forms.Label();
            this.panelEngineInformation = new System.Windows.Forms.Panel();
            this.panelEngineBingWebInformation = new FOCA.PanelEngineBingWebInformation();
            this.panelEngineBingAPIInformation = new FOCA.PanelEngineBingAPIInformation();
            this.panelEngineGoogleWebInformation = new FOCA.PanelEngineGoogleWebInformation();
            this.panelEngineGoogleAPIInformation = new FOCA.PanelEngineGoogleAPIInformation();
            this.panelEngineInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboEngine
            // 
            this.cboEngine.FormattingEnabled = true;
            this.cboEngine.Location = new System.Drawing.Point(37, 140);
            this.cboEngine.Margin = new System.Windows.Forms.Padding(4);
            this.cboEngine.Name = "cboEngine";
            this.cboEngine.Size = new System.Drawing.Size(160, 24);
            this.cboEngine.TabIndex = 0;
            this.cboEngine.SelectedValueChanged += new System.EventHandler(this.comboBoxEngine_SelectedValueChanged);
            // 
            // lblSearchEngine
            // 
            this.lblSearchEngine.AutoSize = true;
            this.lblSearchEngine.Location = new System.Drawing.Point(20, 117);
            this.lblSearchEngine.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSearchEngine.Name = "lblSearchEngine";
            this.lblSearchEngine.Size = new System.Drawing.Size(241, 17);
            this.lblSearchEngine.TabIndex = 11;
            this.lblSearchEngine.Text = "Select the web search engine to use:";
            // 
            // lblWebSearcherDescription
            // 
            this.lblWebSearcherDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWebSearcherDescription.Location = new System.Drawing.Point(20, 39);
            this.lblWebSearcherDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWebSearcherDescription.Name = "lblWebSearcherDescription";
            this.lblWebSearcherDescription.Size = new System.Drawing.Size(348, 78);
            this.lblWebSearcherDescription.TabIndex = 8;
            this.lblWebSearcherDescription.Text = "Using a web search engine like Google or Bing the program searches for links pointing to the domain site to identify new subdomains.";
            // 
            // lblWebSearcher
            // 
            this.lblWebSearcher.AutoSize = true;
            this.lblWebSearcher.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWebSearcher.Location = new System.Drawing.Point(7, 11);
            this.lblWebSearcher.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblWebSearcher.Name = "lblWebSearcher";
            this.lblWebSearcher.Size = new System.Drawing.Size(111, 17);
            this.lblWebSearcher.TabIndex = 7;
            this.lblWebSearcher.Text = "Web Searcher";
            // 
            // panelEngineInformation
            // 
            this.panelEngineInformation.Controls.Add(this.panelEngineBingWebInformation);
            this.panelEngineInformation.Controls.Add(this.panelEngineBingAPIInformation);
            this.panelEngineInformation.Controls.Add(this.panelEngineGoogleWebInformation);
            this.panelEngineInformation.Controls.Add(this.panelEngineGoogleAPIInformation);
            this.panelEngineInformation.Location = new System.Drawing.Point(37, 174);
            this.panelEngineInformation.Margin = new System.Windows.Forms.Padding(4);
            this.panelEngineInformation.Name = "panelEngineInformation";
            this.panelEngineInformation.Size = new System.Drawing.Size(331, 81);
            this.panelEngineInformation.TabIndex = 13;
            // 
            // panelEngineBingWebInformation
            // 
            this.panelEngineBingWebInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEngineBingWebInformation.Location = new System.Drawing.Point(0, 0);
            this.panelEngineBingWebInformation.Margin = new System.Windows.Forms.Padding(5);
            this.panelEngineBingWebInformation.Name = "panelEngineBingWebInformation";
            this.panelEngineBingWebInformation.Size = new System.Drawing.Size(331, 81);
            this.panelEngineBingWebInformation.TabIndex = 0;
            // 
            // panelEngineBingAPIInformation
            // 
            this.panelEngineBingAPIInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEngineBingAPIInformation.Location = new System.Drawing.Point(0, 0);
            this.panelEngineBingAPIInformation.Margin = new System.Windows.Forms.Padding(5);
            this.panelEngineBingAPIInformation.Name = "panelEngineBingAPIInformation";
            this.panelEngineBingAPIInformation.Size = new System.Drawing.Size(331, 81);
            this.panelEngineBingAPIInformation.TabIndex = 0;
            // 
            // panelEngineGoogleWebInformation
            // 
            this.panelEngineGoogleWebInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEngineGoogleWebInformation.Location = new System.Drawing.Point(0, 0);
            this.panelEngineGoogleWebInformation.Margin = new System.Windows.Forms.Padding(5);
            this.panelEngineGoogleWebInformation.Name = "panelEngineGoogleWebInformation";
            this.panelEngineGoogleWebInformation.Size = new System.Drawing.Size(331, 81);
            this.panelEngineGoogleWebInformation.TabIndex = 1;
            // 
            // panelEngineGoogleAPIInformation
            // 
            this.panelEngineGoogleAPIInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEngineGoogleAPIInformation.Location = new System.Drawing.Point(0, 0);
            this.panelEngineGoogleAPIInformation.Margin = new System.Windows.Forms.Padding(5);
            this.panelEngineGoogleAPIInformation.Name = "panelEngineGoogleAPIInformation";
            this.panelEngineGoogleAPIInformation.Size = new System.Drawing.Size(331, 81);
            this.panelEngineGoogleAPIInformation.TabIndex = 2;
            // 
            // PanelWebSearcherInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelEngineInformation);
            this.Controls.Add(this.cboEngine);
            this.Controls.Add(this.lblSearchEngine);
            this.Controls.Add(this.lblWebSearcherDescription);
            this.Controls.Add(this.lblWebSearcher);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PanelWebSearcherInformation";
            this.Size = new System.Drawing.Size(392, 277);
            this.Load += new System.EventHandler(this.PanelWebSearcher_Load);
            this.panelEngineInformation.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboEngine;
        private System.Windows.Forms.Label lblSearchEngine;
        private System.Windows.Forms.Label lblWebSearcherDescription;
        private System.Windows.Forms.Label lblWebSearcher;
        private System.Windows.Forms.Panel panelEngineInformation;
        private PanelEngineBingWebInformation panelEngineBingWebInformation;
        private PanelEngineBingAPIInformation panelEngineBingAPIInformation;
        private PanelEngineGoogleWebInformation panelEngineGoogleWebInformation;
        private PanelEngineGoogleAPIInformation panelEngineGoogleAPIInformation;

    }
}
