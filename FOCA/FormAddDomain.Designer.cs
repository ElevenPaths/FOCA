namespace FOCA
{
    partial class FormAddDomain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddDomain));
            this.lblAddDomain = new System.Windows.Forms.Label();
            this.txtDomain = new System.Windows.Forms.TextBox();
            this.btnAddDomain = new System.Windows.Forms.Button();
            this.picFOCA = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picFOCA)).BeginInit();
            this.SuspendLayout();
            // 
            // lblAddDomain
            // 
            this.lblAddDomain.AutoSize = true;
            this.lblAddDomain.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddDomain.Location = new System.Drawing.Point(254, 11);
            this.lblAddDomain.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAddDomain.Name = "lblAddDomain";
            this.lblAddDomain.Size = new System.Drawing.Size(193, 29);
            this.lblAddDomain.TabIndex = 7;
            this.lblAddDomain.Text = "Add new domain";
            // 
            // txtDomain
            // 
            this.txtDomain.Location = new System.Drawing.Point(260, 65);
            this.txtDomain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(216, 26);
            this.txtDomain.TabIndex = 14;
            // 
            // btnAddDomain
            // 
            this.btnAddDomain.Image = global::FOCA.Properties.Resources.add1;
            this.btnAddDomain.Location = new System.Drawing.Point(486, 52);
            this.btnAddDomain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAddDomain.Name = "btnAddDomain";
            this.btnAddDomain.Size = new System.Drawing.Size(92, 49);
            this.btnAddDomain.TabIndex = 13;
            this.btnAddDomain.Text = "Add";
            this.btnAddDomain.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddDomain.UseVisualStyleBackColor = true;
            this.btnAddDomain.Click += new System.EventHandler(this.btAddDomain_Click);
            // 
            // picFOCA
            // 
            this.picFOCA.Image = ((System.Drawing.Image)(resources.GetObject("picFOCA.Image")));
            this.picFOCA.Location = new System.Drawing.Point(15, 11);
            this.picFOCA.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picFOCA.Name = "picFOCA";
            this.picFOCA.Size = new System.Drawing.Size(179, 86);
            this.picFOCA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picFOCA.TabIndex = 8;
            this.picFOCA.TabStop = false;
            // 
            // FormAddDomain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 122);
            this.Controls.Add(this.txtDomain);
            this.Controls.Add(this.btnAddDomain);
            this.Controls.Add(this.picFOCA);
            this.Controls.Add(this.lblAddDomain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "FormAddDomain";
            this.Text = "Add new domain";
            ((System.ComponentModel.ISupportInitialize)(this.picFOCA)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddDomain;
        private System.Windows.Forms.PictureBox picFOCA;
        private System.Windows.Forms.Label lblAddDomain;
        private System.Windows.Forms.TextBox txtDomain;
    }
}