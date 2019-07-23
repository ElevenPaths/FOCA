namespace FOCA
{
    partial class PanelEngineGoogleAPIInformation
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
            this.lblGoogleApi = new System.Windows.Forms.Label();
            this.lblGoogleApiDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // lblGoogleApi
            //
            this.lblGoogleApi.AutoSize = true;
            this.lblGoogleApi.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGoogleApi.Location = new System.Drawing.Point(6, 7);
            this.lblGoogleApi.Name = "lblGoogleApi";
            this.lblGoogleApi.Size = new System.Drawing.Size(135, 13);
            this.lblGoogleApi.TabIndex = 9;
            this.lblGoogleApi.Text = "Google API limitations";
            //
            // lblGoogleApiDescription
            //
            this.lblGoogleApiDescription.AutoSize = true;
            this.lblGoogleApiDescription.Location = new System.Drawing.Point(12, 23);
            this.lblGoogleApiDescription.Name = "lblGoogleApiDescription";
            this.lblGoogleApiDescription.Size = new System.Drawing.Size(159, 26);
            this.lblGoogleApiDescription.TabIndex = 8;
            this.lblGoogleApiDescription.Text = "-Max 64 results for each search\r\n-Max 32 words in a search string";
            //
            // PanelEngineGoogleAPIInformation
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblGoogleApi);
            this.Controls.Add(this.lblGoogleApiDescription);
            this.Name = "PanelEngineGoogleAPIInformation";
            this.Size = new System.Drawing.Size(232, 66);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblGoogleApi;
        private System.Windows.Forms.Label lblGoogleApiDescription;
    }
}
