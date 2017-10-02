namespace FOCA
{
    partial class FormAddIpRange
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddIpRange));
            this.txtOct4From = new System.Windows.Forms.TextBox();
            this.txtOct3From = new System.Windows.Forms.TextBox();
            this.txtOct2From = new System.Windows.Forms.TextBox();
            this.txtOct1From = new System.Windows.Forms.TextBox();
            this.lblAddIpRange = new System.Windows.Forms.Label();
            this.btnAddIp = new System.Windows.Forms.Button();
            this.picFOCA = new System.Windows.Forms.PictureBox();
            this.lblFrom = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.txtOct4To = new System.Windows.Forms.TextBox();
            this.txtOct3To = new System.Windows.Forms.TextBox();
            this.txtOct2To = new System.Windows.Forms.TextBox();
            this.txtOct1To = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picFOCA)).BeginInit();
            this.SuspendLayout();
            // 
            // txtOct4From
            // 
            this.txtOct4From.Location = new System.Drawing.Point(194, 175);
            this.txtOct4From.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtOct4From.Name = "txtOct4From";
            this.txtOct4From.Size = new System.Drawing.Size(46, 26);
            this.txtOct4From.TabIndex = 12;
            this.txtOct4From.Text = "0";
            this.txtOct4From.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtOct3From
            // 
            this.txtOct3From.Location = new System.Drawing.Point(136, 175);
            this.txtOct3From.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtOct3From.Name = "txtOct3From";
            this.txtOct3From.Size = new System.Drawing.Size(46, 26);
            this.txtOct3From.TabIndex = 11;
            this.txtOct3From.Text = "0";
            this.txtOct3From.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtOct2From
            // 
            this.txtOct2From.Location = new System.Drawing.Point(80, 175);
            this.txtOct2From.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtOct2From.Name = "txtOct2From";
            this.txtOct2From.Size = new System.Drawing.Size(46, 26);
            this.txtOct2From.TabIndex = 10;
            this.txtOct2From.Text = "168";
            this.txtOct2From.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtOct1From
            // 
            this.txtOct1From.Location = new System.Drawing.Point(22, 175);
            this.txtOct1From.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtOct1From.Name = "txtOct1From";
            this.txtOct1From.Size = new System.Drawing.Size(46, 26);
            this.txtOct1From.TabIndex = 9;
            this.txtOct1From.Text = "192";
            this.txtOct1From.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblAddIpRange
            // 
            this.lblAddIpRange.AutoSize = true;
            this.lblAddIpRange.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddIpRange.Location = new System.Drawing.Point(18, 14);
            this.lblAddIpRange.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAddIpRange.Name = "lblAddIpRange";
            this.lblAddIpRange.Size = new System.Drawing.Size(152, 29);
            this.lblAddIpRange.TabIndex = 7;
            this.lblAddIpRange.Text = "Add IP range";
            // 
            // btnAddIp
            // 
            this.btnAddIp.Image = global::FOCA.Properties.Resources.add1;
            this.btnAddIp.Location = new System.Drawing.Point(93, 308);
            this.btnAddIp.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAddIp.Name = "btnAddIp";
            this.btnAddIp.Size = new System.Drawing.Size(92, 49);
            this.btnAddIp.TabIndex = 13;
            this.btnAddIp.Text = "Add";
            this.btnAddIp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddIp.UseVisualStyleBackColor = true;
            this.btnAddIp.Click += new System.EventHandler(this.btAddIp_Click);
            // 
            // picFOCA
            // 
            this.picFOCA.Image = ((System.Drawing.Image)(resources.GetObject("picFOCA.Image")));
            this.picFOCA.Location = new System.Drawing.Point(53, 49);
            this.picFOCA.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picFOCA.Name = "picFOCA";
            this.picFOCA.Size = new System.Drawing.Size(164, 86);
            this.picFOCA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picFOCA.TabIndex = 8;
            this.picFOCA.TabStop = false;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFrom.Location = new System.Drawing.Point(18, 140);
            this.lblFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(70, 29);
            this.lblFrom.TabIndex = 14;
            this.lblFrom.Text = "From";
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTo.Location = new System.Drawing.Point(18, 211);
            this.lblTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(43, 29);
            this.lblTo.TabIndex = 15;
            this.lblTo.Text = "To";
            // 
            // txtOct4To
            // 
            this.txtOct4To.Location = new System.Drawing.Point(194, 249);
            this.txtOct4To.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtOct4To.MaxLength = 3;
            this.txtOct4To.Name = "txtOct4To";
            this.txtOct4To.Size = new System.Drawing.Size(46, 26);
            this.txtOct4To.TabIndex = 19;
            this.txtOct4To.Text = "255";
            this.txtOct4To.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtOct3To
            // 
            this.txtOct3To.Location = new System.Drawing.Point(136, 249);
            this.txtOct3To.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtOct3To.MaxLength = 3;
            this.txtOct3To.Name = "txtOct3To";
            this.txtOct3To.Size = new System.Drawing.Size(46, 26);
            this.txtOct3To.TabIndex = 18;
            this.txtOct3To.Text = "0";
            this.txtOct3To.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtOct2To
            // 
            this.txtOct2To.Location = new System.Drawing.Point(80, 249);
            this.txtOct2To.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtOct2To.MaxLength = 3;
            this.txtOct2To.Name = "txtOct2To";
            this.txtOct2To.Size = new System.Drawing.Size(46, 26);
            this.txtOct2To.TabIndex = 17;
            this.txtOct2To.Text = "168";
            this.txtOct2To.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtOct1To
            // 
            this.txtOct1To.Location = new System.Drawing.Point(22, 249);
            this.txtOct1To.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtOct1To.MaxLength = 3;
            this.txtOct1To.Name = "txtOct1To";
            this.txtOct1To.Size = new System.Drawing.Size(46, 26);
            this.txtOct1To.TabIndex = 16;
            this.txtOct1To.Text = "192";
            this.txtOct1To.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FormAddIpRange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(270, 375);
            this.Controls.Add(this.txtOct4To);
            this.Controls.Add(this.txtOct3To);
            this.Controls.Add(this.txtOct2To);
            this.Controls.Add(this.txtOct1To);
            this.Controls.Add(this.lblTo);
            this.Controls.Add(this.lblFrom);
            this.Controls.Add(this.btnAddIp);
            this.Controls.Add(this.txtOct4From);
            this.Controls.Add(this.txtOct3From);
            this.Controls.Add(this.txtOct2From);
            this.Controls.Add(this.txtOct1From);
            this.Controls.Add(this.picFOCA);
            this.Controls.Add(this.lblAddIpRange);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "FormAddIpRange";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add IP range";
            ((System.ComponentModel.ISupportInitialize)(this.picFOCA)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddIp;
        private System.Windows.Forms.TextBox txtOct4From;
        private System.Windows.Forms.TextBox txtOct3From;
        private System.Windows.Forms.TextBox txtOct2From;
        private System.Windows.Forms.TextBox txtOct1From;
        private System.Windows.Forms.PictureBox picFOCA;
        private System.Windows.Forms.Label lblAddIpRange;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.TextBox txtOct4To;
        private System.Windows.Forms.TextBox txtOct3To;
        private System.Windows.Forms.TextBox txtOct2To;
        private System.Windows.Forms.TextBox txtOct1To;
    }
}