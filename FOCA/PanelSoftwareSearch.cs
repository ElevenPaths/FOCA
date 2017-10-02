using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using FOCA.Analysis;
using FOCA.Analysis.FingerPrinting;
using FOCA.Analysis.Technology;
using FOCA.Properties;
using FOCA.Threads;

namespace FOCA
{
    public partial class PanelSoftwareSearch : UserControl
    {
        private bool bFpHttp;
        private bool bFpShodan;
        private bool bFpSmtp;
        private bool bSkipToNextSearch; // enable/disable to jump to the next search type
        private bool bTechRecog;
        private ShodanRecognition sr;
        private Thread thrSearch;

        public PanelSoftwareSearch()
        {
            InitializeComponent();
        }

        private void checkedButtonFingerPrintingHTTP_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < panelInformation.Controls.Count; i++)
                panelInformation.Controls[i].Visible = false;
            panelFPrintingHTTP.Visible = true;
            panelFPrintingHTTP.Dock = DockStyle.Fill;
        }

        private void checkedButtonFingerPrintingSMTP_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < panelInformation.Controls.Count; i++)
                panelInformation.Controls[i].Visible = false;

            panelFPrintingSMTP.Visible = true;
            panelFPrintingSMTP.Dock = DockStyle.Fill;
        }

        private void checkedButtonFingerPrintingShodan_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < panelInformation.Controls.Count; i++)
                panelInformation.Controls[i].Visible = false;

            panelFPrintingShodan.Visible = true;
            panelFPrintingShodan.Dock = DockStyle.Fill;
        }

        private void checkedButtonTechRecognition_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < panelInformation.Controls.Count; i++)
                panelInformation.Controls[i].Visible = false;

            panelTechRecognition.Visible = true;
            panelTechRecognition.Dock = DockStyle.Fill;

            clTechExtensions.Items.Clear();
            for (var iTechExtension = 0;
                iTechExtension < Program.cfgCurrent.AvaliableTechExtensions.Length;
                iTechExtension++)
            {
                var techExtension = Program.cfgCurrent.AvaliableTechExtensions[iTechExtension];
                clTechExtensions.Items.Add(techExtension);

                clTechExtensions.SetItemChecked(iTechExtension,
                    Program.cfgCurrent.SelectedTechExtensions.Contains(techExtension));
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (buttonStart.Text == "&Start")
            {
                bFpHttp = cbFingerPrintingHTTP.Checked;
                bFpSmtp = cbFingerPrintingSMTP.Checked;
                bFpShodan = cbFingerprintingShodan.Checked;
                bTechRecog = cbTechRecognition.Checked;

                thrSearch = new Thread(Search) {IsBackground = true};
                thrSearch.Start();

                EnableSkip("Skip");
            }
            else
            {
                Abort();
                DisableSkip("Skip");
            }
        }

        private void Abort()
        {
            if (thrSearch == null) return;
            thrSearch.Abort();
            buttonStart.Text = "&Start";
            buttonStart.Image = Resources.tick;
        }

        private void SearchFpHTTP(List<string> dominios)
        {
            foreach (var domStr in dominios)
            {
                if (CheckToSkip())
                    break;

                var domain = Program.data.GetDomain(domStr);
                var existsFp = false;

                for (var fpI = 0; fpI < domain.fingerPrinting.Count(); fpI++)
                {
                    var fp = domain.fingerPrinting[fpI];

                    if (fp is HTTP)
                        existsFp = true;
                }
                if (existsFp) // do not redo the fingerprinting
                    continue;

                FingerPrinting fprinting = new HTTP(domain.Domain, "/", 80, false);
                domain.fingerPrinting.Add(fprinting);
                fprinting.GetVersion();
                FingerPrintingEventHandler.AsociateFingerPrinting(fprinting, null);
            }
        }

        private void SearchFpSMTP(List<string> dominios)
        {
            foreach (var domStr in dominios)
            {
                if (CheckToSkip())
                    break;

                var domain = Program.data.GetDomain(domStr);
                var existsFp = false;

                for (var fpI = 0; fpI < domain.fingerPrinting.Count(); fpI++)
                {
                    var fp = domain.fingerPrinting[fpI];

                    if (fp is SMTP)
                        existsFp = true;
                }
                if (existsFp) // do not redo the fingerprinting
                    continue;

                FingerPrinting fprinting = new SMTP(domain.Domain, 25);
                domain.fingerPrinting.Add(fprinting);
                fprinting.GetVersion();
                FingerPrintingEventHandler.AsociateFingerPrinting(fprinting, null);
            }
        }

        private void SearchFpShodan()
        {
            if (string.IsNullOrEmpty(Program.cfgCurrent.ShodanApiKey))
            {
                MessageBox.Show(@"Before searching with Shodan's API you must set up a Shodan API Key. You can do it in 'Options > General Config'");
                return;
            }
            if (sr != null)
            {
                MessageBox.Show(@"Already searching in Shodan, please wait", Program.ProgramName,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                sr = new ShodanRecognition(Program.cfgCurrent.ShodanApiKey, Program.data.GetIPs());
                sr.DataFoundEvent += ShodanDataFound;
                sr.LogEvent += ShodanLog;
                sr.EndEvent += delegate { sr = null; };
                sr.StartRecognition();
            }
        }

        private void SearchTechRecog(IReadOnlyList<string> dominios)
        {
            var domsArray = new DomainsItem[dominios.Count];
            for (var i = 0; i < dominios.Count; i++)
                domsArray[i] = Program.data.GetDomain(dominios[i]);

            var handler = new TechnologyAnalysisHandler();

            handler.LinkFound += delegate
            {
                if (CheckToSkip())
                    return;
            };

            handler.EndAnalysisMultipleDomains += delegate
            {
                Program.LogThis(new Log(Log.ModuleType.FingingerPrinting, "Fingerprinting finished", Log.LogType.debug));
                buttonStart.Text = "&Start";
                buttonStart.Image = Resources.tick;
            };

            handler.AnalysisMultiplesDomains(domsArray);
        }

        private void Search()
        {
            try
            {
                Invoke(new MethodInvoker(delegate
                {
                    // show stop button
                    buttonStart.Text = "&Stop";
                    buttonStart.Image = Resources.delete;
                }));

                var numeroDominios = Program.data.GetDomains().Count();
                var listaDominios = new List<string>();

                for (var iDom = 0; iDom < numeroDominios; iDom++)
                {
                    var dominio = Program.data.GetDomains()[iDom];
                    if (dominio.Contains(Program.data.Project.Domain))
                        listaDominios.Add(Program.data.GetDomains()[iDom]);
                    else
                    {
                        listaDominios.AddRange(from domAlternativo in Program.data.Project.AlternativeDomains
                            where dominio.Contains(domAlternativo)
                            select Program.data.GetDomains()[iDom]);
                    }
                }

                if (bFpHttp)
                {
                    Program.LogThis(new Log(Log.ModuleType.FingingerPrinting, "Initializing HTTP fingerprinting",
                        Log.LogType.debug));
                    EnableSkip("Skip");
                    ChangeTextCurrentSearch("HTTP Fingerprinting");
                    SearchFpHTTP(listaDominios);
                    Program.LogThis(new Log(Log.ModuleType.FingingerPrinting, "Finishing HTTP fingerprinting",
                        Log.LogType.debug));
                }
                if (bFpSmtp)
                {
                    Program.LogThis(new Log(Log.ModuleType.FingingerPrinting, "Initializing SMTP fingerprinting",
                        Log.LogType.debug));
                    EnableSkip("Skip");
                    ChangeTextCurrentSearch("SMTP Fingerprinting");
                    SearchFpSMTP(listaDominios);
                    Program.LogThis(new Log(Log.ModuleType.FingingerPrinting, "Finishing SMTP fingerprinting",
                        Log.LogType.debug));
                }
                if (bFpShodan)
                {
                    Program.LogThis(new Log(Log.ModuleType.FingingerPrinting, "Initializing Shodan fingerprinting",
                        Log.LogType.debug));
                    EnableSkip("Skip");
                    ChangeTextCurrentSearch("Shodan Fingerprinting");
                    SearchFpShodan();
                    Program.LogThis(new Log(Log.ModuleType.FingingerPrinting, "Finishing Shodan fingerprinting",
                        Log.LogType.debug));
                }
                if (bTechRecog)
                {
                    Program.LogThis(new Log(Log.ModuleType.FingingerPrinting, "Initializing technology recognition",
                        Log.LogType.debug));
                    EnableSkip("Skip");
                    ChangeTextCurrentSearch("Technology recognition");
                    SearchTechRecog(listaDominios);
                    Program.LogThis(new Log(Log.ModuleType.FingingerPrinting, "Finishing technology recognition",
                        Log.LogType.debug));
                }
            }
            catch (ThreadAbortException)
            {
                const string strMessage = "Fingerprinting aborted";
                Program.LogThis(new Log(Log.ModuleType.FingingerPrinting, strMessage, Log.LogType.debug));
                Program.ChangeStatus(strMessage);
            }
            catch (Exception e)
            {
                var strMessage = $"Error fingerprinting subdomain: {e.Message}";
                Program.LogThis(new Log(Log.ModuleType.FingingerPrinting,
                    $"Error fingerprinting subdomain: {e.Message}", Log.LogType.error));
                Program.ChangeStatus(strMessage);
            }
            finally
            {
                Invoke(new MethodInvoker(delegate
                {
                    buttonStart.Text = "&Start";
                    buttonStart.Image = Resources.tick;
                    ChangeTextCurrentSearch("None");
                    DisableSkip("Skip");
                }));
            }
        }

        private void PanelSoftwareSearch_Load(object sender, EventArgs e)
        {
            panelFPrintingHTTP.Visible = true;
            panelFPrintingHTTP.Dock = DockStyle.Fill;
        }

        private static void ShodanDataFound(object sender, EventsThreads.ThreadListDataFoundEventArgs e)
        {
            try
            {
                if (e?.Data == null || e.Data.Count <= 0 || !(e.Data[0] is ShodanRecognition.ShodanIPInformation))
                    return;
                // just an object is received
                var si = (ShodanRecognition.ShodanIPInformation) e.Data[0];
                Program.LogThis(new Log(Log.ModuleType.ShodanSearch,
                    $"Found IP Information {si.IPAddress}", Log.LogType.low));

                var ei = new ExtendedIPInformation
                {
                    Country = si.Country,
                    ServerBanner = si.ServerBanner
                };
                foreach (var hostName in si.HostNames)
                    Program.data.AddResolution(hostName, si.IPAddress,
                        $"Shodan Hostname [{hostName}]", 0, Program.cfgCurrent, true);
                ei.OS = si.OS;
                ei.ShodanResponse = si.ShodanResponse;
                // add data found in shodan to the IP address
                Program.data.SetIPInformation(si.IPAddress, ei);
                Program.data.GetServersFromIPs();
            }
            catch (Exception ex)
            {
                Program.LogThis(new Log(Log.ModuleType.ShodanSearch,
                    $"Error managing Shodan data returned {ex.Message}", Log.LogType.error));
            }
        }

        private static void ShodanLog(object sender, EventsThreads.ThreadStringEventArgs e)
        {
            Program.LogThis(new Log(Log.ModuleType.ShodanSearch, e.Message, Log.LogType.debug));
        }

        private void btSkip_Click(object sender, EventArgs e)
        {
            bSkipToNextSearch = true;
            btSkip.Enabled = false;
        }

        private bool CheckToSkip()
        {
            return bSkipToNextSearch;
        }

        private void ChangeTextCurrentSearch(string current)
        {
            CheckForIllegalCrossThreadCalls = false;
            btSkip.Enabled = true;
            btSkip.Text = @"Skip";
            lbCurrentSearch.Text = @"Current search: " + current;
            CheckForIllegalCrossThreadCalls = true;
        }

        private void DisableSkip(string text)
        {
            CheckForIllegalCrossThreadCalls = false;
            btSkip.Enabled = false;
            btSkip.Text = text;
            CheckForIllegalCrossThreadCalls = true;
        }

        private void EnableSkip(string text)
        {
            CheckForIllegalCrossThreadCalls = false;
            bSkipToNextSearch = false;
            btSkip.Enabled = true;
            btSkip.Text = text;
            CheckForIllegalCrossThreadCalls = true;
        }

        private void btSelectAll_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < clTechExtensions.Items.Count; i++)
                clTechExtensions.SetItemChecked(i, true);
        }

        private void btUnselectAll_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < clTechExtensions.Items.Count; i++)
                clTechExtensions.SetItemChecked(i, false);
        }

        private void clTechExtensions_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.cfgCurrent.SelectedTechExtensions.Clear();
            for (var iTechExtension = 0; iTechExtension < clTechExtensions.Items.Count; iTechExtension++)
            {
                var extension = clTechExtensions.Items[iTechExtension].ToString();
                if (clTechExtensions.GetItemChecked(iTechExtension))
                    Program.cfgCurrent.SelectedTechExtensions.Add(extension);
            }
        }
    }
}