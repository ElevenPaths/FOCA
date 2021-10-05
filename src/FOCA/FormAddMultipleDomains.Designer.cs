namespace FOCA
{
    partial class FormAddMultipleDomains
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddMultipleDomains));
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.lblAddMultipleHosts = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Image = global::FOCA.Properties.Resources.add1;
            this.btnSelectFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSelectFile.Location = new System.Drawing.Point(146, 41);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSelectFile.Size = new System.Drawing.Size(104, 36);
            this.btnSelectFile.TabIndex = 0;
            this.btnSelectFile.Text = "Select file";
            this.btnSelectFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblAddMultipleHosts
            // 
            this.lblAddMultipleHosts.AutoSize = true;
            this.lblAddMultipleHosts.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblAddMultipleHosts.Location = new System.Drawing.Point(12, 9);
            this.lblAddMultipleHosts.Name = "lblAddMultipleHosts";
            this.lblAddMultipleHosts.Size = new System.Drawing.Size(406, 20);
            this.lblAddMultipleHosts.TabIndex = 1;
            this.lblAddMultipleHosts.Text = "Select the file which contains the hosts list (one per line):";
            // 
            // FormAddMultipleDomains
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 89);
            this.Controls.Add(this.lblAddMultipleHosts);
            this.Controls.Add(this.btnSelectFile);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormAddMultipleDomains";
            this.Text = "Add multiple hosts";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Label lblAddMultipleHosts;
    }
}