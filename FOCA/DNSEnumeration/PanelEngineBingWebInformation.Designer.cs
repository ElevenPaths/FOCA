namespace FOCA
{
    partial class PanelEngineBingWebInformation
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
            this.lblBingWeb = new System.Windows.Forms.Label();
            this.lblBingWebDescription = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // lblBingWeb
            //
            this.lblBingWeb.AutoSize = true;
            this.lblBingWeb.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBingWeb.Location = new System.Drawing.Point(6, 7);
            this.lblBingWeb.Name = "lblBingWeb";
            this.lblBingWeb.Size = new System.Drawing.Size(126, 13);
            this.lblBingWeb.TabIndex = 9;
            this.lblBingWeb.Text = "Bing Web limitations";
            //
            // lblBingWebDescription
            //
            this.lblBingWebDescription.AutoSize = true;
            this.lblBingWebDescription.Location = new System.Drawing.Point(12, 23);
            this.lblBingWebDescription.Name = "lblBingWebDescription";
            this.lblBingWebDescription.Size = new System.Drawing.Size(167, 26);
            this.lblBingWebDescription.TabIndex = 8;
            this.lblBingWebDescription.Text = "-Max 1000 results for each search\r\n-Max 49 words in a search string";
            //
            // PanelEngineBingWebInformation
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblBingWeb);
            this.Controls.Add(this.lblBingWebDescription);
            this.Name = "PanelEngineBingWebInformation";
            this.Size = new System.Drawing.Size(232, 66);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblBingWeb;
        private System.Windows.Forms.Label lblBingWebDescription;
    }
}
