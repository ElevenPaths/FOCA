namespace FOCA
{
    partial class FormAssignIp
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
            this.picFOCA = new System.Windows.Forms.PictureBox();
            this.lblAsignIp = new System.Windows.Forms.Label();
            this.lstIps = new System.Windows.Forms.ListBox();
            this.lblIp = new System.Windows.Forms.Label();
            this.txtIp = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picFOCA)).BeginInit();
            this.SuspendLayout();
            // 
            // picFOCA
            // 
            this.picFOCA.Image = global::FOCA.Properties.Resources.newlogo;
            this.picFOCA.Location = new System.Drawing.Point(121, 58);
            this.picFOCA.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picFOCA.Name = "picFOCA";
            this.picFOCA.Size = new System.Drawing.Size(204, 69);
            this.picFOCA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picFOCA.TabIndex = 10;
            this.picFOCA.TabStop = false;
            // 
            // lblAsignIp
            // 
            this.lblAsignIp.AutoSize = true;
            this.lblAsignIp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAsignIp.Location = new System.Drawing.Point(68, 30);
            this.lblAsignIp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAsignIp.Name = "lblAsignIp";
            this.lblAsignIp.Size = new System.Drawing.Size(293, 25);
            this.lblAsignIp.TabIndex = 9;
            this.lblAsignIp.Text = "Assign IP address to a computer";
            // 
            // lstIps
            // 
            this.lstIps.FormattingEnabled = true;
            this.lstIps.ItemHeight = 16;
            this.lstIps.Location = new System.Drawing.Point(43, 183);
            this.lstIps.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstIps.Name = "lstIps";
            this.lstIps.Size = new System.Drawing.Size(333, 84);
            this.lstIps.TabIndex = 11;
            // 
            // lblIp
            // 
            this.lblIp.AutoSize = true;
            this.lblIp.Location = new System.Drawing.Point(44, 151);
            this.lblIp.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblIp.Name = "lblIp";
            this.lblIp.Size = new System.Drawing.Size(20, 17);
            this.lblIp.TabIndex = 12;
            this.lblIp.Text = "IP";
            // 
            // txtIp
            // 
            this.txtIp.Location = new System.Drawing.Point(75, 148);
            this.txtIp.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtIp.Name = "txtIp";
            this.txtIp.Size = new System.Drawing.Size(157, 22);
            this.txtIp.TabIndex = 13;
            this.txtIp.Text = "80.81.106.149";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::FOCA.Properties.Resources.add;
            this.btnAdd.Location = new System.Drawing.Point(257, 148);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(53, 28);
            this.btnAdd.TabIndex = 14;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // btnDel
            // 
            this.btnDel.Image = global::FOCA.Properties.Resources.delete;
            this.btnDel.Location = new System.Drawing.Point(324, 148);
            this.btnDel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(53, 28);
            this.btnDel.TabIndex = 15;
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btDel_Click);
            // 
            // FormAssignIp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 292);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtIp);
            this.Controls.Add(this.lblIp);
            this.Controls.Add(this.lstIps);
            this.Controls.Add(this.picFOCA);
            this.Controls.Add(this.lblAsignIp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "FormAssignIp";
            this.Text = "Assign IP";
            this.Load += new System.EventHandler(this.FormAsignIp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picFOCA)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picFOCA;
        private System.Windows.Forms.Label lblAsignIp;
        private System.Windows.Forms.ListBox lstIps;
        private System.Windows.Forms.Label lblIp;
        private System.Windows.Forms.TextBox txtIp;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDel;
    }
}