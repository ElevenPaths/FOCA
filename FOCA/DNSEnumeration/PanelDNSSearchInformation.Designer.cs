namespace FOCA
{
    partial class PanelDNSSearchInformation
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
            this.lblDnsSearchDescription = new System.Windows.Forms.Label();
            this.lblDnsSearch = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblDnsSearchDescription
            // 
            this.lblDnsSearchDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDnsSearchDescription.Location = new System.Drawing.Point(20, 39);
            this.lblDnsSearchDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDnsSearchDescription.Name = "lblDnsSearchDescription";
            this.lblDnsSearchDescription.Size = new System.Drawing.Size(353, 119);
            this.lblDnsSearchDescription.TabIndex = 12;
            this.lblDnsSearchDescription.Text = "DNS Search performs queries to DNS Servers searching for well-known records. The " +
    "following queries will be done: ";
            // 
            // lblDnsSearch
            // 
            this.lblDnsSearch.AutoSize = true;
            this.lblDnsSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDnsSearch.Location = new System.Drawing.Point(7, 11);
            this.lblDnsSearch.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDnsSearch.Name = "lblDnsSearch";
            this.lblDnsSearch.Size = new System.Drawing.Size(96, 17);
            this.lblDnsSearch.TabIndex = 11;
            this.lblDnsSearch.Text = "DNS Search";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(20, 101);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(353, 71);
            this.label1.TabIndex = 13;
            this.label1.Text = "NS, SOA, Primary.Master, MX, SPF, Domainkeys Records, DKIM Records, SRV Records f" +
    "or VoiP, IM and Active Directory, Kerberos, LDAP and Web Proxy Autodiscovery.";
            // 
            // PanelDNSSearchInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblDnsSearchDescription);
            this.Controls.Add(this.lblDnsSearch);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PanelDNSSearchInformation";
            this.Size = new System.Drawing.Size(392, 302);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDnsSearchDescription;
        private System.Windows.Forms.Label lblDnsSearch;
        private System.Windows.Forms.Label label1;
    }
}
