namespace FOCA
{
    partial class PanelOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelOptions));
            this.lblAllDnsDescription = new System.Windows.Forms.Label();
            this.lblOptions = new System.Windows.Forms.Label();
            this.chkUseAllDns = new System.Windows.Forms.CheckBox();
            this.updMaxRecursivity = new System.Windows.Forms.NumericUpDown();
            this.lblMaxRecursivity = new System.Windows.Forms.Label();
            this.lblMaxRecursivityDescription = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.updMaxRecursivity)).BeginInit();
            this.SuspendLayout();
            //
            // lblAllDnsDescription
            //
            this.lblAllDnsDescription.AutoSize = true;
            this.lblAllDnsDescription.Location = new System.Drawing.Point(13, 54);
            this.lblAllDnsDescription.Name = "lblAllDnsDescription";
            this.lblAllDnsDescription.Size = new System.Drawing.Size(273, 26);
            this.lblAllDnsDescription.TabIndex = 19;
            this.lblAllDnsDescription.Text = "When the program makes a DNS query it can use all the\r\ndns servers found. It\'s sl" +
    "ow and often redundant.";
            //
            // lblOptions
            //
            this.lblOptions.AutoSize = true;
            this.lblOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOptions.Location = new System.Drawing.Point(5, 9);
            this.lblOptions.Name = "lblOptions";
            this.lblOptions.Size = new System.Drawing.Size(50, 13);
            this.lblOptions.TabIndex = 18;
            this.lblOptions.Text = "Options";
            //
            // chkUseAllDns
            //
            this.chkUseAllDns.AutoSize = true;
            this.chkUseAllDns.Location = new System.Drawing.Point(14, 34);
            this.chkUseAllDns.Name = "chkUseAllDns";
            this.chkUseAllDns.Size = new System.Drawing.Size(121, 17);
            this.chkUseAllDns.TabIndex = 0;
            this.chkUseAllDns.Text = "Use all DNS servers";
            this.chkUseAllDns.UseVisualStyleBackColor = true;
            //
            // updMaxRecursivity
            //
            this.updMaxRecursivity.Location = new System.Drawing.Point(14, 104);
            this.updMaxRecursivity.Name = "updMaxRecursivity";
            this.updMaxRecursivity.Size = new System.Drawing.Size(35, 20);
            this.updMaxRecursivity.TabIndex = 1;
            //
            // lblMaxRecursivity
            //
            this.lblMaxRecursivity.AutoSize = true;
            this.lblMaxRecursivity.Location = new System.Drawing.Point(56, 107);
            this.lblMaxRecursivity.Name = "lblMaxRecursivity";
            this.lblMaxRecursivity.Size = new System.Drawing.Size(77, 13);
            this.lblMaxRecursivity.TabIndex = 23;
            this.lblMaxRecursivity.Text = "Max recursion";
            //
            // lblMaxRecursivityDescription
            //
            this.lblMaxRecursivityDescription.AutoSize = true;
            this.lblMaxRecursivityDescription.Location = new System.Drawing.Point(13, 133);
            this.lblMaxRecursivityDescription.Name = "lblMaxRecursivityDescription";
            this.lblMaxRecursivityDescription.Size = new System.Drawing.Size(279, 65);
            this.lblMaxRecursivityDescription.TabIndex = 24;
            this.lblMaxRecursivityDescription.Text = resources.GetString("lblMaxRecursivityDescription.Text");
            //
            // PanelOptions
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblMaxRecursivityDescription);
            this.Controls.Add(this.lblMaxRecursivity);
            this.Controls.Add(this.updMaxRecursivity);
            this.Controls.Add(this.chkUseAllDns);
            this.Controls.Add(this.lblAllDnsDescription);
            this.Controls.Add(this.lblOptions);
            this.Name = "PanelOptions";
            this.Size = new System.Drawing.Size(294, 225);
            ((System.ComponentModel.ISupportInitialize)(this.updMaxRecursivity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblAllDnsDescription;
        private System.Windows.Forms.Label lblOptions;
        private System.Windows.Forms.CheckBox chkUseAllDns;
        private System.Windows.Forms.NumericUpDown updMaxRecursivity;
        private System.Windows.Forms.Label lblMaxRecursivity;
        private System.Windows.Forms.Label lblMaxRecursivityDescription;
    }
}
