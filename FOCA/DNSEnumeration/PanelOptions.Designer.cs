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
            this.lblAllDnsDescription.Location = new System.Drawing.Point(17, 66);
            this.lblAllDnsDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAllDnsDescription.Name = "lblAllDnsDescription";
            this.lblAllDnsDescription.Size = new System.Drawing.Size(363, 42);
            this.lblAllDnsDescription.TabIndex = 19;
            this.lblAllDnsDescription.Text = "When the program makes a DNS query it can use all DNS servers found. It\'s slow and often redundant.";
            // 
            // lblOptions
            // 
            this.lblOptions.AutoSize = true;
            this.lblOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOptions.Location = new System.Drawing.Point(7, 11);
            this.lblOptions.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOptions.Name = "lblOptions";
            this.lblOptions.Size = new System.Drawing.Size(64, 17);
            this.lblOptions.TabIndex = 18;
            this.lblOptions.Text = "Options";
            // 
            // chkUseAllDns
            // 
            this.chkUseAllDns.AutoSize = true;
            this.chkUseAllDns.Location = new System.Drawing.Point(19, 42);
            this.chkUseAllDns.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkUseAllDns.Name = "chkUseAllDns";
            this.chkUseAllDns.Size = new System.Drawing.Size(157, 21);
            this.chkUseAllDns.TabIndex = 0;
            this.chkUseAllDns.Text = "Use all DNS servers";
            this.chkUseAllDns.UseVisualStyleBackColor = true;
            // 
            // updMaxRecursivity
            // 
            this.updMaxRecursivity.Location = new System.Drawing.Point(19, 128);
            this.updMaxRecursivity.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.updMaxRecursivity.Name = "updMaxRecursivity";
            this.updMaxRecursivity.Size = new System.Drawing.Size(47, 22);
            this.updMaxRecursivity.TabIndex = 1;
            // 
            // lblMaxRecursivity
            // 
            this.lblMaxRecursivity.AutoSize = true;
            this.lblMaxRecursivity.Location = new System.Drawing.Point(75, 132);
            this.lblMaxRecursivity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaxRecursivity.Name = "lblMaxRecursivity";
            this.lblMaxRecursivity.Size = new System.Drawing.Size(96, 17);
            this.lblMaxRecursivity.TabIndex = 23;
            this.lblMaxRecursivity.Text = "Max recursion";
            // 
            // lblMaxRecursivityDescription
            // 
            this.lblMaxRecursivityDescription.Location = new System.Drawing.Point(17, 164);
            this.lblMaxRecursivityDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaxRecursivityDescription.Name = "lblMaxRecursivityDescription";
            this.lblMaxRecursivityDescription.Size = new System.Drawing.Size(363, 105);
            this.lblMaxRecursivityDescription.TabIndex = 24;
            this.lblMaxRecursivityDescription.Text = resources.GetString("lblMaxRecursivityDescription.Text");
            // 
            // PanelOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblMaxRecursivityDescription);
            this.Controls.Add(this.lblMaxRecursivity);
            this.Controls.Add(this.updMaxRecursivity);
            this.Controls.Add(this.chkUseAllDns);
            this.Controls.Add(this.lblAllDnsDescription);
            this.Controls.Add(this.lblOptions);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PanelOptions";
            this.Size = new System.Drawing.Size(392, 277);
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
