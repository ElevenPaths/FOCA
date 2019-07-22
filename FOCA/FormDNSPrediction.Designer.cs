namespace FOCA
{
    partial class FormDNSPrediction
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDNSPrediction));
            this.lblPattern = new System.Windows.Forms.Label();
            this.lblVariables = new System.Windows.Forms.Label();
            this.listViewPatterns = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.lblNVariants = new System.Windows.Forms.Label();
            this.lblVariants = new System.Windows.Forms.Label();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.rtbPattern = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // lblPattern
            // 
            this.lblPattern.AutoSize = true;
            this.lblPattern.Location = new System.Drawing.Point(11, 9);
            this.lblPattern.Name = "lblPattern";
            this.lblPattern.Size = new System.Drawing.Size(41, 13);
            this.lblPattern.TabIndex = 0;
            this.lblPattern.Text = "Pattern";
            // 
            // lblVariables
            // 
            this.lblVariables.AutoSize = true;
            this.lblVariables.Location = new System.Drawing.Point(11, 58);
            this.lblVariables.Name = "lblVariables";
            this.lblVariables.Size = new System.Drawing.Size(50, 13);
            this.lblVariables.TabIndex = 2;
            this.lblVariables.Text = "Variables";
            // 
            // listViewPatterns
            // 
            this.listViewPatterns.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewPatterns.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewPatterns.FullRowSelect = true;
            this.listViewPatterns.GridLines = true;
            this.listViewPatterns.Location = new System.Drawing.Point(14, 74);
            this.listViewPatterns.Name = "listViewPatterns";
            this.listViewPatterns.Size = new System.Drawing.Size(230, 103);
            this.listViewPatterns.TabIndex = 1;
            this.listViewPatterns.UseCompatibleStateImageBehavior = false;
            this.listViewPatterns.View = System.Windows.Forms.View.Details;
            this.listViewPatterns.SelectedIndexChanged += new System.EventHandler(this.listViewPatterns_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Variable";
            this.columnHeader1.Width = 105;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Values";
            this.columnHeader2.Width = 105;
            // 
            // lblNVariants
            // 
            this.lblNVariants.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblNVariants.AutoSize = true;
            this.lblNVariants.Location = new System.Drawing.Point(11, 191);
            this.lblNVariants.Name = "lblNVariants";
            this.lblNVariants.Size = new System.Drawing.Size(66, 13);
            this.lblNVariants.TabIndex = 4;
            this.lblNVariants.Text = "No. of variants: ";
            // 
            // lblVariants
            // 
            this.lblVariants.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblVariants.AutoSize = true;
            this.lblVariants.Location = new System.Drawing.Point(93, 191);
            this.lblVariants.Name = "lblVariants";
            this.lblVariants.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblVariants.Size = new System.Drawing.Size(13, 13);
            this.lblVariants.TabIndex = 5;
            this.lblVariants.Text = "1";
            // 
            // btnScan
            // 
            this.btnScan.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScan.Image = global::FOCA.Properties.Resources.magnifier;
            this.btnScan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnScan.Location = new System.Drawing.Point(100, 217);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(89, 23);
            this.btnScan.TabIndex = 4;
            this.btnScan.Text = "&Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.Location = new System.Drawing.Point(254, 119);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(26, 26);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Enabled = false;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.Location = new System.Drawing.Point(254, 151);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(26, 26);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // rtbPattern
            // 
            this.rtbPattern.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbPattern.DetectUrls = false;
            this.rtbPattern.Location = new System.Drawing.Point(14, 25);
            this.rtbPattern.Multiline = false;
            this.rtbPattern.Name = "rtbPattern";
            this.rtbPattern.Size = new System.Drawing.Size(262, 20);
            this.rtbPattern.TabIndex = 0;
            this.rtbPattern.Text = "";
            this.rtbPattern.TextChanged += new System.EventHandler(this.rtbPattern_TextChanged);
            // 
            // FormDNSPrediction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(288, 252);
            this.Controls.Add(this.rtbPattern);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.lblVariants);
            this.Controls.Add(this.lblNVariants);
            this.Controls.Add(this.listViewPatterns);
            this.Controls.Add(this.lblVariables);
            this.Controls.Add(this.lblPattern);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(296, 279);
            this.Name = "FormDNSPrediction";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "DNS Prediction";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPattern;
        private System.Windows.Forms.Label lblVariables;
        private System.Windows.Forms.ListView listViewPatterns;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label lblNVariants;
        private System.Windows.Forms.Label lblVariants;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.RichTextBox rtbPattern;
    }
}