namespace FOCA
{
    partial class PanelTryCommonNames
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
            this.lblSelectFile = new System.Windows.Forms.Label();
            this.lblCommonDnsNamesDescription = new System.Windows.Forms.Label();
            this.lblCommonDnsNames = new System.Windows.Forms.Label();
            this.txtCommonNamesPath = new System.Windows.Forms.TextBox();
            this.btnBrowseFile = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            //
            // lblSelectFile
            //
            this.lblSelectFile.AutoSize = true;
            this.lblSelectFile.Location = new System.Drawing.Point(15, 74);
            this.lblSelectFile.Name = "lblSelectFile";
            this.lblSelectFile.Size = new System.Drawing.Size(71, 13);
            this.lblSelectFile.TabIndex = 28;
            this.lblSelectFile.Text = "Select the file";
            //
            // lblCommonDnsNamesDescription
            //
            this.lblCommonDnsNamesDescription.AutoSize = true;
            this.lblCommonDnsNamesDescription.Location = new System.Drawing.Point(15, 32);
            this.lblCommonDnsNamesDescription.Name = "lblCommonDnsNamesDescription";
            this.lblCommonDnsNamesDescription.Size = new System.Drawing.Size(274, 26);
            this.lblCommonDnsNamesDescription.TabIndex = 26;
            this.lblCommonDnsNamesDescription.Text = "The program uses a common DNS names list to find new\r\nsubdomains. This list is th" +
    "e same used by Fierce tool.";
            //
            // lblCommonDnsNames
            //
            this.lblCommonDnsNames.AutoSize = true;
            this.lblCommonDnsNames.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCommonDnsNames.Location = new System.Drawing.Point(5, 9);
            this.lblCommonDnsNames.Name = "lblCommonDnsNames";
            this.lblCommonDnsNames.Size = new System.Drawing.Size(145, 13);
            this.lblCommonDnsNames.TabIndex = 25;
            this.lblCommonDnsNames.Text = "Try common DNS names";
            //
            // txtCommonNamesPath
            //
            this.txtCommonNamesPath.Location = new System.Drawing.Point(18, 94);
            this.txtCommonNamesPath.Name = "txtCommonNamesPath";
            this.txtCommonNamesPath.Size = new System.Drawing.Size(177, 20);
            this.txtCommonNamesPath.TabIndex = 0;
            //
            // btnBrowseFile
            //
            this.btnBrowseFile.Image = global::FOCA.Properties.Resources.magnifier;
            this.btnBrowseFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBrowseFile.Location = new System.Drawing.Point(201, 92);
            this.btnBrowseFile.Name = "btnBrowseFile";
            this.btnBrowseFile.Size = new System.Drawing.Size(83, 23);
            this.btnBrowseFile.TabIndex = 1;
            this.btnBrowseFile.Text = "&Browse";
            this.btnBrowseFile.UseVisualStyleBackColor = true;
            this.btnBrowseFile.Click += new System.EventHandler(this.button1_Click);
            //
            // openFileDialog
            //
            this.openFileDialog.Filter = "All Files(*.*)|*.*";
            //
            // PanelTryCommonNames
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnBrowseFile);
            this.Controls.Add(this.txtCommonNamesPath);
            this.Controls.Add(this.lblSelectFile);
            this.Controls.Add(this.lblCommonDnsNamesDescription);
            this.Controls.Add(this.lblCommonDnsNames);
            this.Name = "PanelTryCommonNames";
            this.Size = new System.Drawing.Size(294, 225);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSelectFile;
        private System.Windows.Forms.Label lblCommonDnsNamesDescription;
        private System.Windows.Forms.Label lblCommonDnsNames;
        private System.Windows.Forms.Button btnBrowseFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        public System.Windows.Forms.TextBox txtCommonNamesPath;
    }
}
