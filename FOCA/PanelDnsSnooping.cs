using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using FOCA.Analysis.DNSCacheSnooping;

namespace FOCA
{
    /// <summary>
    /// Dns Cache Snooping configuration and execution class.
    /// Read more about this attack at http://www.elladodelmal.com/2010/07/foca-dns-cache-snooper.html
    /// </summary>
    public partial class PanelDnsSnooping : UserControl
    {
        private List<string> domains;
        private bool isSnooping;

        public PanelDnsSnooping()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Reads a text file and parses it to get a domains list
        /// </summary>
        /// <params>
        ///     <param name="filename">Name of the file that will be parsed</param>
        /// </params>
        /// <returns>Domains list</returns>
        private List<string> ReadFile(out string filename)
        {
            var res = new List<string>();
            var ofd = new OpenFileDialog();

            if (string.IsNullOrEmpty(txtFilename.Text))
                ofd.InitialDirectory = Application.StartupPath + @"\Analysis\DNSCacheSnooping";
            else
                ofd.InitialDirectory = Path.GetDirectoryName(txtFilename.Text);

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                var sr = new StreamReader(ofd.FileName);
                while (!sr.EndOfStream)
                {
                    var dom = sr.ReadLine();
                    if (string.IsNullOrEmpty(dom) || dom.StartsWith("#"))
                        continue;
                    dom = dom.Split(' ', '#', '\t')[0];
                    res.Add(dom);
                }
                sr.Close();
                filename = ofd.FileName;

                return res;
            }
            filename = string.Empty;
            return null;
        }

        private void btLoadFile_Click(object sender, EventArgs e)
        {
            string filename;
            domains = ReadFile(out filename);
            txtFilename.Text = filename;

            if (domains == null)
            {
                btnDnsSnooping.Enabled = false;
                chkMonitorice.Enabled = chkMonitorice.Checked = false;
            }
            else
            {
                chkMonitorice.Enabled = true;
                btnDnsSnooping.Enabled = true;
                lblStatusValue.Text = txtFilename.Text + @" loaded.";
            }
        }

        /// <summary>
        ///     Launches the snooping action. Tries to get domain names from a DNS server's cache
        /// </summary>
        /// <returns></returns>
        private void Snoop()
        {
            CheckForIllegalCrossThreadCalls = false;

            var dnScache = new DnsCache();
            dnScache.Start += delegate
            {
                chkMonitorice.Enabled = false;
                btnDnsSnooping.Enabled = false;
                lblStatusValue.Text = @"Snooping...";
            };
            dnScache.End += delegate
            {
                isSnooping = false;
                chkMonitorice.Enabled = true;

                btnDnsSnooping.Enabled = !chkMonitorice.Checked;
                lblStatusValue.Text = @"Finished";
            };

            try
            {
                var ipDns = Dns.GetHostAddresses(lstDns.Items[lstDns.SelectedIndex].ToString())[0].ToString();
                var lstCache = dnScache.Exists(ipDns, domains);

                foreach (var t in lstCache)
                {
                    lveCache.Items.Add(t);
                }

                foreach (var t in lstCache)
                {
                    var exists = false;
                    for (var x = 0; x < lveMonitor.Items.Count; x++)
                    {
                        if (lveMonitor.Items[x].Text != t) continue;
                        lveMonitor.Items[x].SubItems[2].Text = DateTime.Now.ToShortTimeString();
                        // Updates last time it was found
                        exists = true;
                        break;
                    }
                    if (exists) continue;
                    lveMonitor.Items.Add(t);
                    lveMonitor.Items[lveMonitor.Items.Count - 1].SubItems.Add(DateTime.Now.ToShortTimeString());
                    lveMonitor.Items[lveMonitor.Items.Count - 1].SubItems.Add(DateTime.Now.ToShortTimeString());
                }
            }
            catch
            {
                isSnooping = false;
            }
            CheckForIllegalCrossThreadCalls = true;
        }

        private void btDnsSnooping_Click(object sender, EventArgs e)
        {
            lveCache.Items.Clear();
            if (lstDns.SelectedIndex == -1)
            {
                MessageBox.Show(null, @"Must select a DNS server", @"FOCA", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            if (domains == null)
            {
                MessageBox.Show(null, @"Error, file not valid.", @"FOCA", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            var t = new Thread(Snoop) {IsBackground = true};
            t.Start();
        }

        private void btGetDNS_Click(object sender, EventArgs e)
        {
            chkMonitorice.Enabled = chkMonitorice.Checked = false;
            lveMonitor.Items.Clear();
            lveCache.Items.Clear();

            if (string.IsNullOrEmpty(txtDomain.Text))
            {
                MessageBox.Show(null, @"Invalid domain", @"FOCA", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            lstDns.Items.Clear();

            lblStatusValue.Text = @"Obtaining DNS servers of " + txtDomain.Text + ".";
            try
            {
                var dnss = DnsCache.GetDnsList(txtDomain.Text);
                foreach (var t in dnss)
                    lstDns.Items.Add(t);
                lblStatusValue.Text = @"Obtained DNS servers of " + txtDomain.Text + ".";
            }
            catch
            {
                MessageBox.Show(null, @"Invalid domain", @"FOCA", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblStatusValue.Text = @"Error obtaining DNS servers of " + txtDomain.Text + ".";
            }
        }

        private void PanelDnsSnooping_Load(object sender, EventArgs e)
        {
            lveCache.Columns.Add("Host").Width = lveCache.Width - 5;
            lveMonitor.Columns.Add("Host").Width = lveCache.Width - 205;
            lveMonitor.Columns.Add("First").Width = 100;
            lveMonitor.Columns.Add("Last").Width = 100;

            lveCache.Groups.Add(new ListViewGroup("cache", "Cache"));
            lveMonitor.Groups.Add(new ListViewGroup("monitorice", "Monitorice"));
        }

        private void lbDns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstDns.SelectedIndex != -1)
            {
                if (!string.IsNullOrEmpty(txtFilename.Text))
                    chkMonitorice.Enabled = true;
            }
            else
                chkMonitorice.Enabled = chkMonitorice.Checked = false;
        }

        private void cbMonitorice_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkMonitorice.Checked)
            {
                btnDnsSnooping.Enabled = btnGetDNS.Enabled = btnLoadFile.Enabled = true;
                timerSnoop.Enabled = false;
                timerSnoop.Stop();
            }
            else
            {
                if (lstDns.SelectedIndex == -1)
                {
                    MessageBox.Show(@"First you must select a DNS server");
                    chkMonitorice.Checked = false;
                    return;
                }

                if (string.IsNullOrEmpty(txtFilename.Text))
                {
                    MessageBox.Show(@"First you must select a domain's file");
                    chkMonitorice.Checked = false;
                    return;
                }

                btnDnsSnooping.Enabled = btnGetDNS.Enabled = btnLoadFile.Enabled = false;

                timerSnoop.Enabled = true;
                timerSnoop.Start();
                timerSnoop_Tick(null, null);
            }
        }

        private void timerSnoop_Tick(object sender, EventArgs e)
        {
            if (isSnooping)
                return;

            lveCache.Items.Clear();
            var t = new Thread(Snoop) {IsBackground = true};
            isSnooping = true;
            t.Start();
        }

        /// <summary>
        ///     Loads project's domain name
        /// </summary>
        /// <returns></returns>
        public void LoadProjectConfig()
        {
            if (string.IsNullOrEmpty(txtDomain.Text))
                txtDomain.Text = Program.data.Project.Domain;
        }

        /// <summary>
        ///     Updates project's domain name
        /// </summary>
        /// <returns></returns>
        public void ChangeDomain(string domain)
        {
            txtDomain.Text = domain;
        }
    }
}