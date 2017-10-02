using System;
using System.Net;
using System.Windows.Forms;

namespace FOCA
{
    public partial class FormSelectIpRange : Form
    {
        public bool IncludeInNetworkMap;
        public IPAddress IpStart, IpEnd;

        public FormSelectIpRange()
        {
            InitializeComponent();
        }

        public FormSelectIpRange(string ipStart, string ipEnd) : this()
        {
            txtStart.Text = ipStart;
            txtEnd.Text = ipEnd;
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            IPAddress ipStart, ipEnd;
            if (!IPAddress.TryParse(txtStart.Text, out ipStart))
            {
                MessageBox.Show($@"Invalid IP Address {txtStart.Text}", Application.ProductName, MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            if (!IPAddress.TryParse(txtEnd.Text, out ipEnd))
            {
                MessageBox.Show($@"Invalid IP Address {txtEnd.Text}", Application.ProductName, MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }
            IpStart = ipStart;
            IpEnd = ipEnd;
            IncludeInNetworkMap = chkInclude.Checked;
            DialogResult = DialogResult.OK;
        }
    }
}