namespace FOCA
{
    partial class FormBackupsFuzzer
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormBackupsFuzzer));
            this.lblValidStatusCodes = new System.Windows.Forms.Label();
            this.txtValidCodes = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.panelUrls = new FOCA.PanelUrlsList();
            this.SuspendLayout();
            // 
            // lblValidStatusCodes
            // 
            this.lblValidStatusCodes.AutoSize = true;
            this.lblValidStatusCodes.Location = new System.Drawing.Point(23, 554);
            this.lblValidStatusCodes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblValidStatusCodes.Name = "lblValidStatusCodes";
            this.lblValidStatusCodes.Size = new System.Drawing.Size(267, 20);
            this.lblValidStatusCodes.TabIndex = 1;
            this.lblValidStatusCodes.Text = "HTTP status codes for a valid match:";
            // 
            // txtValidCodes
            // 
            this.txtValidCodes.Location = new System.Drawing.Point(23, 597);
            this.txtValidCodes.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtValidCodes.Name = "txtValidCodes";
            this.txtValidCodes.Size = new System.Drawing.Size(476, 26);
            this.txtValidCodes.TabIndex = 2;
            this.txtValidCodes.Text = "200,206,304,401,403";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(1383, 554);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(224, 74);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // panelUrls
            // 
            this.panelUrls.Location = new System.Drawing.Point(18, 18);
            this.panelUrls.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.panelUrls.Name = "panelUrls";
            this.panelUrls.Size = new System.Drawing.Size(1584, 501);
            this.panelUrls.TabIndex = 0;
            // 
            // FormBackupsFuzzer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1620, 643);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.txtValidCodes);
            this.Controls.Add(this.lblValidStatusCodes);
            this.Controls.Add(this.panelUrls);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormBackupsFuzzer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Backups Fuzzer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PanelUrlsList panelUrls;
        private System.Windows.Forms.Label lblValidStatusCodes;
        private System.Windows.Forms.TextBox txtValidCodes;
        private System.Windows.Forms.Button btnStart;
    }
}