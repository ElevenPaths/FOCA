using FOCA.Database.Controllers;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FOCA
{
    public partial class FormOptions : Form
    {
        public FormOptions()
        {
            InitializeComponent();
        }

        private void FormOptions_Load(object sender, EventArgs e)
        {
            var config = Program.cfgCurrent;

            updSimultaneousDownloads.Value = config.SimultaneousDownloads;
            chkHEAD.Checked = config.UseHead;
            chkResolveHosts.Checked = config.ResolveHost;
            chkUseAllDNS.Checked = config.UseAllDns;
            updRecursivity.Value = config.MaxRecursion;
            tbDefaultDNSCacheSnooping.Text = config.DefaultDnsCacheSnooping;
            updParallelDNS.Value = config.ParallelDnsQueries;
            txtGoogleApiKey.Text = config.GoogleApiKey;
            txtGoogleApiCx.Text = config.GoogleApiCx;
            txtShodanApiKey.Text = config.ShodanApiKey;
            txtBingApiKey.Text = config.BingApiKey;
            chkNetrange.Checked = config.ScanNetranges255;
            updSimultaneousTasks.Value = config.NumberOfTasks;
            txtDiarioApiKey.Text = config.DiarioAPIKey;
            txtDiarioSecret.Text = config.DiarioAPISecret;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            var config = new Configuration();

            config.SimultaneousDownloads = int.Parse(updSimultaneousDownloads.Value.ToString());
            config.UseHead = chkHEAD.Checked;
            config.UseAllDns = chkUseAllDNS.Checked;
            config.ResolveHost = chkResolveHosts.Checked;
            config.MaxRecursion = int.Parse(updRecursivity.Value.ToString());
            config.DefaultDnsCacheSnooping = tbDefaultDNSCacheSnooping.Text;
            config.ParallelDnsQueries = int.Parse(updParallelDNS.Value.ToString());
            config.GoogleApiKey = txtGoogleApiKey.Text;
            config.GoogleApiCx = txtGoogleApiCx.Text;
            config.ShodanApiKey = txtShodanApiKey.Text;
            config.BingApiKey = txtBingApiKey.Text;
            config.ScanNetranges255 = chkNetrange.Checked;
            config.NumberOfTasks = int.Parse(updSimultaneousTasks.Value.ToString());
            config.DiarioAPIKey = txtDiarioApiKey.Text;
            config.DiarioAPISecret = txtDiarioSecret.Text;

            new ConfigurationController().Save(config);

            Program.cfgCurrent = config;

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void nudSimultaneousDownloads_Validating(object sender, CancelEventArgs e)
        {
            var numericUpDown = sender as NumericUpDown;
            if (numericUpDown != null && (numericUpDown.Value < 0 || numericUpDown.Value > 30))
            {
                MessageBox.Show(@"Please insert a value in 'Simultaneous downloads' between 1 and 30", @"Invalid data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
        }

        public void MoveToTab(int tab)
        {
            tabControl1.SelectTab(tab);
        }
    }
}
