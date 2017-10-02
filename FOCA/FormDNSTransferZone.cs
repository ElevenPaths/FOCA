using System;
using System.Windows.Forms;

namespace FOCA
{
    public partial class FormDnsTransferZone : Form
    {
        private readonly ParametersDnsTransferZone currentParameters;

        public FormDnsTransferZone(Form frmParent, ParametersDnsTransferZone currentParameters)
        {
            InitializeComponent();
            Left = frmParent.Left + (frmParent.Width/2) - Width/2;
            Top = frmParent.Top + (frmParent.Height/2) - Height/2;
            this.currentParameters = currentParameters;
        }

        /// <summary>
        ///     Set DNS Transfer Zone configuration
        /// </summary>
        /// <param name="dns"></param>
        /// <param name="timeout"></param>
        public void SetConfig(string dns, int timeout)
        {
            txtDnsServer.Text =
                txtZoneRequested.Text = dns;
            txtTimeout.Text = timeout.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            currentParameters.DnsServer = txtDnsServer.Text;
            currentParameters.ZoneRequested = txtZoneRequested.Text;
            int intTemp;
            if (!int.TryParse(txtTimeout.Text, out intTemp))
            {
                MessageBox.Show(@"Insert a valid Timeout", @"Invalid value", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                txtTimeout.Focus();
                return;
            }
            currentParameters.Timeout = intTemp;
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        ///     Parameters needed in order to trigger the DNS Zone Transfer
        /// </summary>
        public class ParametersDnsTransferZone
        {
            public string DnsServer;
            public int Timeout;
            public string ZoneRequested;
        }
    }
}