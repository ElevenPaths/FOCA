namespace FOCA
{
    partial class PanelSearchIPBing
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
            this.lblSearchIpBingDescription = new System.Windows.Forms.Label();
            this.lblSearchIpBing = new System.Windows.Forms.Label();
            this.cboEngine = new System.Windows.Forms.ComboBox();
            this.lblSearchEngine = new System.Windows.Forms.Label();
            this.panelEngineInformation = new System.Windows.Forms.Panel();
            this.panelEngineBingWebInformation = new FOCA.PanelEngineBingWebInformation();
            this.panelEngineBingAPIInformation = new FOCA.PanelEngineBingAPIInformation();
            this.panelEngineInformation.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSearchIpBingDescription
            // 
            this.lblSearchIpBingDescription.Location = new System.Drawing.Point(20, 39);
            this.lblSearchIpBingDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSearchIpBingDescription.Name = "lblSearchIpBingDescription";
            this.lblSearchIpBingDescription.Size = new System.Drawing.Size(348, 63);
            this.lblSearchIpBingDescription.TabIndex = 16;
            this.lblSearchIpBingDescription.Text = "Bing allows searching for links located in a particular IP address. This functionality can be used to find domains that share IP Address.";
            // 
            // lblSearchIpBing
            // 
            this.lblSearchIpBing.AutoSize = true;
            this.lblSearchIpBing.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchIpBing.Location = new System.Drawing.Point(7, 11);
            this.lblSearchIpBing.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSearchIpBing.Name = "lblSearchIpBing";
            this.lblSearchIpBing.Size = new System.Drawing.Size(133, 17);
            this.lblSearchIpBing.TabIndex = 15;
            this.lblSearchIpBing.Text = "Search IP in Bing";
            // 
            // cboEngine
            // 
            this.cboEngine.FormattingEnabled = true;
            this.cboEngine.Location = new System.Drawing.Point(37, 140);
            this.cboEngine.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.lblSearchEngine.TabIndex = 19;
            this.lblSearchEngine.Text = "Select the web search engine to use:";
            // 
            // panelEngineInformation
            // 
            this.panelEngineInformation.Controls.Add(this.panelEngineBingWebInformation);
            this.panelEngineInformation.Controls.Add(this.panelEngineBingAPIInformation);
            this.panelEngineInformation.Location = new System.Drawing.Point(37, 174);
            this.panelEngineInformation.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelEngineInformation.Name = "panelEngineInformation";
            this.panelEngineInformation.Size = new System.Drawing.Size(331, 81);
            this.panelEngineInformation.TabIndex = 21;
            // 
            // panelEngineBingWebInformation
            // 
            this.panelEngineBingWebInformation.Location = new System.Drawing.Point(0, 0);
            this.panelEngineBingWebInformation.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.panelEngineBingWebInformation.Name = "panelEngineBingWebInformation";
            this.panelEngineBingWebInformation.Size = new System.Drawing.Size(309, 81);
            this.panelEngineBingWebInformation.TabIndex = 0;
            // 
            // panelEngineBingAPIInformation
            // 
            this.panelEngineBingAPIInformation.Location = new System.Drawing.Point(0, 0);
            this.panelEngineBingAPIInformation.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.panelEngineBingAPIInformation.Name = "panelEngineBingAPIInformation";
            this.panelEngineBingAPIInformation.Size = new System.Drawing.Size(309, 81);
            this.panelEngineBingAPIInformation.TabIndex = 0;
            // 
            // PanelSearchIPBing
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelEngineInformation);
            this.Controls.Add(this.cboEngine);
            this.Controls.Add(this.lblSearchEngine);
            this.Controls.Add(this.lblSearchIpBingDescription);
            this.Controls.Add(this.lblSearchIpBing);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PanelSearchIPBing";
            this.Size = new System.Drawing.Size(392, 277);
            this.Load += new System.EventHandler(this.PanelSearchIPBing_Load);
            this.panelEngineInformation.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblSearchIpBingDescription;
        private System.Windows.Forms.Label lblSearchIpBing;
        private System.Windows.Forms.ComboBox cboEngine;
        private System.Windows.Forms.Label lblSearchEngine;
        private System.Windows.Forms.Panel panelEngineInformation;
        private PanelEngineBingWebInformation panelEngineBingWebInformation;
        private PanelEngineBingAPIInformation panelEngineBingAPIInformation;
    }
}
