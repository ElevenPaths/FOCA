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
            this.lblTransferZoneDescription.Location = new System.Drawing.Point(20, 39);
            this.lblTransferZoneDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTransferZoneDescription.Name = "lblTransferZoneDescription";
            this.lblTransferZoneDescription.Size = new System.Drawing.Size(344, 82);
            this.lblTransferZoneDescription.TabIndex = 29;
            this.lblTransferZoneDescription.Text = "The program tries to make a DNS Transfer Zone to find new subdomains. This action may be illegal in some countries.";
            // 
            // lblTryTransferZone
            // 
            this.lblTryTransferZone.AutoSize = true;
            this.lblTryTransferZone.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTryTransferZone.Location = new System.Drawing.Point(7, 11);
            this.lblTryTransferZone.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTryTransferZone.Name = "lblTryTransferZone";
            this.lblTryTransferZone.Size = new System.Drawing.Size(146, 17);
            this.lblTryTransferZone.TabIndex = 28;
            this.lblTryTransferZone.Text = "Try Transfer  Zone";
            // 
            // PanelTryTransferZone
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTransferZoneDescription);
            this.Controls.Add(this.lblTryTransferZone);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PanelTryTransferZone";
            this.Size = new System.Drawing.Size(392, 277);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblTransferZoneDescription;
        private System.Windows.Forms.Label lblTryTransferZone;
    }
}
