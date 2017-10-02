namespace FOCA
{
    partial class FormDocumentsSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDocumentsSearch));
            this.lstDocumentsFound = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            //
            // lstDocumentsFound
            //
            this.lstDocumentsFound.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstDocumentsFound.FormattingEnabled = true;
            this.lstDocumentsFound.Location = new System.Drawing.Point(12, 12);
            this.lstDocumentsFound.Name = "lstDocumentsFound";
            this.lstDocumentsFound.Size = new System.Drawing.Size(392, 173);
            this.lstDocumentsFound.TabIndex = 0;
            this.lstDocumentsFound.SelectedValueChanged += new System.EventHandler(this.listBoxFound_SelectedValueChanged);
            //
            // FormDocumentsSearch
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 212);
            this.Controls.Add(this.lstDocumentsFound);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDocumentsSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Documents found";
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.ListBox lstDocumentsFound;
    }
}