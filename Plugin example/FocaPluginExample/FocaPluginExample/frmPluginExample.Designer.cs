namespace FocaPluginExample
{
    partial class FrmPluginExample
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
            this.panel = new System.Windows.Forms.Panel();
            this.btnSendFoca = new System.Windows.Forms.Button();
            this.lblDomain = new System.Windows.Forms.Label();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.txtDomain = new System.Windows.Forms.TextBox();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.Controls.Add(this.btnSendFoca);
            this.panel.Controls.Add(this.lblDomain);
            this.panel.Controls.Add(this.lblWelcome);
            this.panel.Controls.Add(this.txtDomain);
            this.panel.Location = new System.Drawing.Point(12, 12);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(695, 364);
            this.panel.TabIndex = 0;
            // 
            // btnSendFoca
            // 
            this.btnSendFoca.Location = new System.Drawing.Point(102, 197);
            this.btnSendFoca.Name = "btnSendFoca";
            this.btnSendFoca.Size = new System.Drawing.Size(121, 32);
            this.btnSendFoca.TabIndex = 3;
            this.btnSendFoca.Text = "Send to Foca";
            this.btnSendFoca.UseVisualStyleBackColor = true;
            this.btnSendFoca.Click += new System.EventHandler(this.btnSendFoca_Click);
            // 
            // lblDomain
            // 
            this.lblDomain.AutoSize = true;
            this.lblDomain.Location = new System.Drawing.Point(98, 138);
            this.lblDomain.Name = "lblDomain";
            this.lblDomain.Size = new System.Drawing.Size(64, 20);
            this.lblDomain.TabIndex = 2;
            this.lblDomain.Text = "Domain";
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.Location = new System.Drawing.Point(98, 69);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(245, 20);
            this.lblWelcome.TabIndex = 1;
            this.lblWelcome.Text = "Welcome to Foca Plugin Example";
            // 
            // txtDomain
            // 
            this.txtDomain.Location = new System.Drawing.Point(168, 135);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(462, 26);
            this.txtDomain.TabIndex = 0;
            // 
            // FrmPluginExample
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(719, 388);
            this.Controls.Add(this.panel);
            this.Name = "FrmPluginExample";
            this.Text = "Foca plugin example";
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label lblDomain;
        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.TextBox txtDomain;
        private System.Windows.Forms.Button btnSendFoca;
    }
}

