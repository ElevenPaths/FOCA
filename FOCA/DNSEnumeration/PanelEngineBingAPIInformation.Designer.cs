namespace FOCA
{
    partial class PanelEngineBingAPIInformation
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
            this.lblBingApi = new System.Windows.Forms.Label();
            this.lblBingApiDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // lblBingApi
            //
            this.lblBingApi.AutoSize = true;
            this.lblBingApi.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBingApi.Location = new System.Drawing.Point(6, 7);
            this.lblBingApi.Name = "lblBingApi";
            this.lblBingApi.Size = new System.Drawing.Size(120, 13);
            this.lblBingApi.TabIndex = 5;
            this.lblBingApi.Text = "Bing API  limitations";
            //
            // lblBingApiDescription
            //
            this.lblBingApiDescription.AutoSize = true;
            this.lblBingApiDescription.Location = new System.Drawing.Point(12, 23);
            this.lblBingApiDescription.Name = "lblBingApiDescription";
            this.lblBingApiDescription.Size = new System.Drawing.Size(167, 26);
            this.lblBingApiDescription.TabIndex = 4;
            this.lblBingApiDescription.Text = "-Max 1000 results for each search\r\n-Max 49 words in a search string\r\n";
            //
            // PanelEngineBingAPIInformation
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblBingApi);
            this.Controls.Add(this.lblBingApiDescription);
            this.Name = "PanelEngineBingAPIInformation";
            this.Size = new System.Drawing.Size(232, 66);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBingApi;
        private System.Windows.Forms.Label lblBingApiDescription;
    }
}
