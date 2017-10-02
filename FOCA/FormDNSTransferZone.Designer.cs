namespace FOCA
{
    partial class FormDnsTransferZone
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDnsTransferZone));
            this.btnTransferZone = new System.Windows.Forms.Button();
            this.lblDnsServer = new System.Windows.Forms.Label();
            this.txtDnsServer = new System.Windows.Forms.TextBox();
            this.txtZoneRequested = new System.Windows.Forms.TextBox();
            this.lblZoneTransfer = new System.Windows.Forms.Label();
            this.txtTimeout = new System.Windows.Forms.TextBox();
            this.lblTimeout = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // btnTransferZone
            //
            this.btnTransferZone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTransferZone.Image = global::FOCA.Properties.Resources.chart_organisation;
            this.btnTransferZone.Location = new System.Drawing.Point(51, 150);
            this.btnTransferZone.Name = "btnTransferZone";
            this.btnTransferZone.Size = new System.Drawing.Size(141, 25);
            this.btnTransferZone.TabIndex = 3;
            this.btnTransferZone.Text = "&Try Transfer Zone";
            this.btnTransferZone.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnTransferZone.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTransferZone.UseVisualStyleBackColor = true;
            this.btnTransferZone.Click += new System.EventHandler(this.button1_Click);
            //
            // lblDnsServer
            //
            this.lblDnsServer.AutoSize = true;
            this.lblDnsServer.Location = new System.Drawing.Point(12, 13);
            this.lblDnsServer.Name = "lblDnsServer";
            this.lblDnsServer.Size = new System.Drawing.Size(64, 13);
            this.lblDnsServer.TabIndex = 0;
            this.lblDnsServer.Text = "DNS Server";
            //
            // txtDnsServer
            //
            this.txtDnsServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDnsServer.Location = new System.Drawing.Point(15, 29);
            this.txtDnsServer.Name = "txtDnsServer";
            this.txtDnsServer.Size = new System.Drawing.Size(211, 20);
            this.txtDnsServer.TabIndex = 0;
            //
            // txtZoneRequested
            //
            this.txtZoneRequested.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtZoneRequested.Location = new System.Drawing.Point(15, 76);
            this.txtZoneRequested.Name = "txtZoneRequested";
            this.txtZoneRequested.Size = new System.Drawing.Size(211, 20);
            this.txtZoneRequested.TabIndex = 1;
            //
            // lblZoneTransfer
            //
            this.lblZoneTransfer.AutoSize = true;
            this.lblZoneTransfer.Location = new System.Drawing.Point(12, 60);
            this.lblZoneTransfer.Name = "lblZoneTransfer";
            this.lblZoneTransfer.Size = new System.Drawing.Size(82, 13);
            this.lblZoneTransfer.TabIndex = 2;
            this.lblZoneTransfer.Text = "Zone requested";
            //
            // txtTimeout
            //
            this.txtTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTimeout.Location = new System.Drawing.Point(191, 115);
            this.txtTimeout.Name = "txtTimeout";
            this.txtTimeout.Size = new System.Drawing.Size(35, 20);
            this.txtTimeout.TabIndex = 2;
            //
            // lblTimeout
            //
            this.lblTimeout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTimeout.AutoSize = true;
            this.lblTimeout.Location = new System.Drawing.Point(121, 118);
            this.lblTimeout.Name = "lblTimeout";
            this.lblTimeout.Size = new System.Drawing.Size(64, 13);
            this.lblTimeout.TabIndex = 4;
            this.lblTimeout.Text = "Timeout(ms)";
            //
            // FormDNSTransferZone
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(243, 185);
            this.Controls.Add(this.txtTimeout);
            this.Controls.Add(this.lblTimeout);
            this.Controls.Add(this.txtZoneRequested);
            this.Controls.Add(this.lblZoneTransfer);
            this.Controls.Add(this.txtDnsServer);
            this.Controls.Add(this.lblDnsServer);
            this.Controls.Add(this.btnTransferZone);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(251, 212);
            this.Name = "FormDnsTransferZone";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Try DNS Transfer Zone";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTransferZone;
        private System.Windows.Forms.Label lblDnsServer;
        private System.Windows.Forms.TextBox txtDnsServer;
        private System.Windows.Forms.TextBox txtZoneRequested;
        private System.Windows.Forms.Label lblZoneTransfer;
        private System.Windows.Forms.TextBox txtTimeout;
        private System.Windows.Forms.Label lblTimeout;
    }
}