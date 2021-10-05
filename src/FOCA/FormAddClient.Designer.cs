namespace FOCA
{
    partial class FormAddClient
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAddClient));
            this.picFOCA = new System.Windows.Forms.PictureBox();
            this.lblAddPcName = new System.Windows.Forms.Label();
            this.txtPcName = new System.Windows.Forms.TextBox();
            this.btnAddDomain = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picFOCA)).BeginInit();
            this.SuspendLayout();
            // 
            // picFOCA
            // 
            this.picFOCA.Image = ((System.Drawing.Image)(resources.GetObject("picFOCA.Image")));
            this.picFOCA.Location = new System.Drawing.Point(27, 9);
            this.picFOCA.Name = "picFOCA";
            this.picFOCA.Size = new System.Drawing.Size(109, 56);
            this.picFOCA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picFOCA.TabIndex = 2;
            this.picFOCA.TabStop = false;
            // 
            // lblAddPcName
            // 
            this.lblAddPcName.AutoSize = true;
            this.lblAddPcName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAddPcName.Location = new System.Drawing.Point(144, 22);
            this.lblAddPcName.Name = "lblAddPcName";
            this.lblAddPcName.Size = new System.Drawing.Size(107, 20);
            this.lblAddPcName.TabIndex = 3;
            this.lblAddPcName.Text = "Add PC name";
            // 
            // txtPcName
            // 
            this.txtPcName.Location = new System.Drawing.Point(148, 45);
            this.txtPcName.Name = "txtPcName";
            this.txtPcName.Size = new System.Drawing.Size(141, 20);
            this.txtPcName.TabIndex = 4;
            // 
            // btnAddDomain
            // 
            this.btnAddDomain.Image = global::FOCA.Properties.Resources.add1;
            this.btnAddDomain.Location = new System.Drawing.Point(303, 28);
            this.btnAddDomain.Name = "btnAddDomain";
            this.btnAddDomain.Size = new System.Drawing.Size(110, 42);
            this.btnAddDomain.TabIndex = 14;
            this.btnAddDomain.Text = "Add";
            this.btnAddDomain.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddDomain.UseVisualStyleBackColor = true;
            this.btnAddDomain.Click += new System.EventHandler(this.btAddDomain_Click);
            // 
            // FormAddClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 85);
            this.Controls.Add(this.btnAddDomain);
            this.Controls.Add(this.txtPcName);
            this.Controls.Add(this.lblAddPcName);
            this.Controls.Add(this.picFOCA);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormAddClient";
            this.Text = "Add PC Client";
            ((System.ComponentModel.ISupportInitialize)(this.picFOCA)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picFOCA;
        private System.Windows.Forms.Label lblAddPcName;
        private System.Windows.Forms.TextBox txtPcName;
        private System.Windows.Forms.Button btnAddDomain;
    }
}