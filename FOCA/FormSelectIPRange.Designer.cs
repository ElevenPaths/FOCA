namespace FOCA
{
    partial class FormSelectIpRange
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectIpRange));
            this.lblStart = new System.Windows.Forms.Label();
            this.lblEnd = new System.Windows.Forms.Label();
            this.txtStart = new System.Windows.Forms.TextBox();
            this.txtEnd = new System.Windows.Forms.TextBox();
            this.chkInclude = new System.Windows.Forms.CheckBox();
            this.btnScan = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // lblStart
            //
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point(10, 9);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(83, 13);
            this.lblStart.TabIndex = 0;
            this.lblStart.Text = "Start IP Address";
            //
            // lblEnd
            //
            this.lblEnd.AutoSize = true;
            this.lblEnd.Location = new System.Drawing.Point(10, 57);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(80, 13);
            this.lblEnd.TabIndex = 1;
            this.lblEnd.Text = "End IP Address";
            //
            // txtStart
            //
            this.txtStart.Location = new System.Drawing.Point(13, 25);
            this.txtStart.Name = "txtStart";
            this.txtStart.Size = new System.Drawing.Size(129, 20);
            this.txtStart.TabIndex = 2;
            //
            // txtEnd
            //
            this.txtEnd.Location = new System.Drawing.Point(13, 73);
            this.txtEnd.Name = "txtEnd";
            this.txtEnd.Size = new System.Drawing.Size(129, 20);
            this.txtEnd.TabIndex = 3;
            //
            // chkInclude
            //
            this.chkInclude.AutoSize = true;
            this.chkInclude.Location = new System.Drawing.Point(13, 108);
            this.chkInclude.Name = "chkInclude";
            this.chkInclude.Size = new System.Drawing.Size(184, 17);
            this.chkInclude.TabIndex = 4;
            this.chkInclude.Text = "Include alive host in network map";
            this.chkInclude.UseVisualStyleBackColor = true;
            //
            // btnScan
            //
            this.btnScan.Image = global::FOCA.Properties.Resources.drive_network_magnifier;
            this.btnScan.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnScan.Location = new System.Drawing.Point(67, 131);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(75, 28);
            this.btnScan.TabIndex = 5;
            this.btnScan.Text = "&Scan";
            this.btnScan.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            //
            // FormSelectIPRange
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(199, 171);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.chkInclude);
            this.Controls.Add(this.txtEnd);
            this.Controls.Add(this.txtStart);
            this.Controls.Add(this.lblEnd);
            this.Controls.Add(this.lblStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSelectIpRange";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select IP Range";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.TextBox txtStart;
        private System.Windows.Forms.TextBox txtEnd;
        private System.Windows.Forms.CheckBox chkInclude;
        private System.Windows.Forms.Button btnScan;
    }
}