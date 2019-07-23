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
            this.lblSearchIpBingDescription.AutoSize = true;
            this.lblSearchIpBingDescription.Location = new System.Drawing.Point(15, 32);
            this.lblSearchIpBingDescription.Name = "lblSearchIpBingDescription";
            this.lblSearchIpBingDescription.Size = new System.Drawing.Size(279, 39);
            this.lblSearchIpBingDescription.TabIndex = 16;
            this.lblSearchIpBingDescription.Text = "Bing allows searching for links located in a particular IP address.\r\nThis functionality " +
    "can be used to find domains that\r\nshare IP Address.";
            //
            // lblSearchIpBing
            //
            this.lblSearchIpBing.AutoSize = true;
            this.lblSearchIpBing.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchIpBing.Location = new System.Drawing.Point(5, 9);
            this.lblSearchIpBing.Name = "lblSearchIpBing";
            this.lblSearchIpBing.Size = new System.Drawing.Size(106, 13);
            this.lblSearchIpBing.TabIndex = 15;
            this.lblSearchIpBing.Text = "Search IP in Bing";
            //
            // cboEngine
            //
            this.cboEngine.FormattingEnabled = true;
            this.cboEngine.Location = new System.Drawing.Point(28, 114);
            this.cboEngine.Name = "cboEngine";
            this.cboEngine.Size = new System.Drawing.Size(121, 21);
            this.cboEngine.TabIndex = 0;
            this.cboEngine.SelectedValueChanged += new System.EventHandler(this.comboBoxEngine_SelectedValueChanged);
            //
            // lblSearchEngine
            //
            this.lblSearchEngine.AutoSize = true;
            this.lblSearchEngine.Location = new System.Drawing.Point(15, 95);
            this.lblSearchEngine.Name = "lblSearchEngine";
            this.lblSearchEngine.Size = new System.Drawing.Size(183, 13);
            this.lblSearchEngine.TabIndex = 19;
            this.lblSearchEngine.Text = "Select the web search engine to use:";
            //
            // panelEngineInformation
            //
            this.panelEngineInformation.Controls.Add(this.panelEngineBingWebInformation);
            this.panelEngineInformation.Controls.Add(this.panelEngineBingAPIInformation);
            this.panelEngineInformation.Location = new System.Drawing.Point(28, 141);
            this.panelEngineInformation.Name = "panelEngineInformation";
            this.panelEngineInformation.Size = new System.Drawing.Size(248, 66);
            this.panelEngineInformation.TabIndex = 21;
            //
            // panelEngineBingWebInformation
            //
            this.panelEngineBingWebInformation.Location = new System.Drawing.Point(0, 0);
            this.panelEngineBingWebInformation.Name = "panelEngineBingWebInformation";
            this.panelEngineBingWebInformation.Size = new System.Drawing.Size(232, 66);
            this.panelEngineBingWebInformation.TabIndex = 0;
            //
            // panelEngineBingAPIInformation
            //
            this.panelEngineBingAPIInformation.Location = new System.Drawing.Point(0, 0);
            this.panelEngineBingAPIInformation.Name = "panelEngineBingAPIInformation";
            this.panelEngineBingAPIInformation.Size = new System.Drawing.Size(232, 66);
            this.panelEngineBingAPIInformation.TabIndex = 0;
            //
            // PanelSearchIPBing
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelEngineInformation);
            this.Controls.Add(this.cboEngine);
            this.Controls.Add(this.lblSearchEngine);
            this.Controls.Add(this.lblSearchIpBingDescription);
            this.Controls.Add(this.lblSearchIpBing);
            this.Name = "PanelSearchIPBing";
            this.Size = new System.Drawing.Size(294, 225);
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
