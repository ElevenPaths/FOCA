namespace FOCA
{
    partial class PanelTryTransferZone
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
            this.lblTransferZoneDescription = new System.Windows.Forms.Label();
            this.lblTryTransferZone = new System.Windows.Forms.Label();
            this.SuspendLayout();
            //
            // lblTransferZoneDescription
            //
            this.lblTransferZoneDescription.AutoSize = true;
            this.lblTransferZoneDescription.Location = new System.Drawing.Point(15, 32);
            this.lblTransferZoneDescription.Name = "lblTransferZoneDescription";
            this.lblTransferZoneDescription.Size = new System.Drawing.Size(267, 52);
            this.lblTransferZoneDescription.TabIndex = 29;
            this.lblTransferZoneDescription.Text = "The program tries to make a DNS Transfer Zone to find\r\nnew subdomains.\r\n\r\nThis ac" +
    "tion may be illegal in some countries.";
            //
            // lblTryTransferZone
            //
            this.lblTryTransferZone.AutoSize = true;
            this.lblTryTransferZone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTryTransferZone.Location = new System.Drawing.Point(5, 9);
            this.lblTryTransferZone.Name = "lblTryTransferZone";
            this.lblTryTransferZone.Size = new System.Drawing.Size(113, 13);
            this.lblTryTransferZone.TabIndex = 28;
            this.lblTryTransferZone.Text = "Try Transfer  Zone";
            //
            // PanelTryTransferZone
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTransferZoneDescription);
            this.Controls.Add(this.lblTryTransferZone);
            this.Name = "PanelTryTransferZone";
            this.Size = new System.Drawing.Size(294, 225);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblTransferZoneDescription;
        private System.Windows.Forms.Label lblTryTransferZone;
    }
}
