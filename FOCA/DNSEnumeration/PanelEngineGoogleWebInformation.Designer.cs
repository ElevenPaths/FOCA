namespace FOCA
{
    partial class PanelEngineGoogleWebInformation
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
            this.lblGoogleWeb = new System.Windows.Forms.Label();
            this.lblGoogleWebDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // lblGoogleWeb
            //
            this.lblGoogleWeb.AutoSize = true;
            this.lblGoogleWeb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGoogleWeb.Location = new System.Drawing.Point(6, 7);
            this.lblGoogleWeb.Name = "lblGoogleWeb";
            this.lblGoogleWeb.Size = new System.Drawing.Size(137, 13);
            this.lblGoogleWeb.TabIndex = 9;
            this.lblGoogleWeb.Text = "Google Web limitations";
            //
            // lblGoogleWebDescription
            //
            this.lblGoogleWebDescription.AutoSize = true;
            this.lblGoogleWebDescription.Location = new System.Drawing.Point(12, 23);
            this.lblGoogleWebDescription.Name = "lblGoogleWebDescription";
            this.lblGoogleWebDescription.Size = new System.Drawing.Size(167, 26);
            this.lblGoogleWebDescription.TabIndex = 8;
            this.lblGoogleWebDescription.Text = "-Max 1000 results for each search\r\n-Max 32 words in a search string";
            //
            // PanelEngineGoogleWebInformation
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblGoogleWeb);
            this.Controls.Add(this.lblGoogleWebDescription);
            this.Name = "PanelEngineGoogleWebInformation";
            this.Size = new System.Drawing.Size(232, 66);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblGoogleWeb;
        private System.Windows.Forms.Label lblGoogleWebDescription;
    }
}
