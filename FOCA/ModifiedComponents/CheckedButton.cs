using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FOCA.ModifiedComponents
{
    public class CheckedButton : Button
    {
        private bool Checked_;
        public bool Checked
        {
            set
            {
                Checked_ = value;
                if (Checked_)
                {
                    this.BackColor = BackColorChecked;
                    this.ForeColor = Color.FromName("ControlText");
                }
                else
                {
                    this.BackColor = BackColorUnchecked;
                    this.ForeColor = Color.FromName("ControlDark");
                }
            }
            get
            {
                return Checked_;
            }
        }

        public Color BackColorUnchecked { get; set; }

        public Color BackColorChecked { get; set; }

        public CheckedButton() : base() { }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Checked = !Checked;
        }
    }
}
