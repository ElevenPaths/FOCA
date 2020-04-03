using FOCA.Database.Entities;
using FOCA.Properties;
using FOCA.Searcher;
using FOCA.Threads;
using Heijden.DNS;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FOCA
{
    public partial class PanelDNSSearch : UserControl
    {
        private bool bSearchCommonNames;
        private bool bSearchIPBing;
        private bool bSearchShodan;
        private bool bSearchWeb;
        private bool bSearchWithAllDNS;
        private bool bSkipToNextSearch;
        private string CommonNamesFileName;
        private int MaxRecursion;
        private CancellationTokenSource searchCancelToken;

        private Resolver Resolve;

        private PanelWebSearcherInformation.Engine searchEngine;
        private PanelSearchIPBing.Engine searchIPEngine;
        private string strDomain;
        private Thread thrSearch;

        /// <summary>
        /// Setup panel
        /// </summary>
        public PanelDNSSearch()
        {
            InitializeComponent();

            label2.Top = 10;
            panel2.Height += panel2.Top - 29;
            panel2.Top = 29;
        }

        /// <summary>
        /// Load resolver
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelDNSSearch_Load(object sender, EventArgs e)
        {
            if (Program.DesignMode()) return;
            Resolve = new Resolver
            {
                TimeOut = 1000
            };

            Program.data.SetResolver(Resolve);
            if (!Program.DesignMode())
            {
                Program.data.OnLog +=
                    delegate (object s, EventsThreads.ThreadStringEventArgs ev)
                    {
                        Program.LogThis(new Log(Log.ModuleType.DNS, ev.Message, Log.LogType.debug));
                    };
            }
        }

        /// <summary>
        /// Set components visibility
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedButton_Click(object sender, EventArgs e)
        {
            panelWebSearcher.Visible = sender == checkedButtonWebSearcher;
            panelDNSSearchInformation.Visible = sender == checkedButtonDNSSearch;
            panelTryTransferZone.Visible = sender == checkedButtonTransferZone;
            panelTryCommonNames.Visible = sender == checkedButtonNamesSearch;
            panelSearchIPBing.Visible = sender == checkedButtonIPBing;
        }

        /// <summary>
        /// Launch DNS reconnaissance tasks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (buttonStart.Text == "&Start")
            {
                if (cbDictionarySearch.Checked)
                {
                    if (string.IsNullOrEmpty(textBoxCommonNames.Text) == false && !File.Exists(textBoxCommonNames.Text))
                    {
                        MessageBox.Show(
                            @"Invalid file in 'Names Search'. The file '" + textBoxCommonNames.Text +
                            @"' doesn't exist.",
                            @"Names Search - Invalid file name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                bSearchWithAllDNS = Program.cfgCurrent.UseAllDns;
                strDomain = Program.data.Project.Domain;

                bSearchWeb = cbWebSearch.Checked;

                //DuckDuck
                if (rbWSDuckDuckGo.Checked)
                    searchEngine = PanelWebSearcherInformation.Engine.DuckDuckGoWeb;
                //Bing
                else if (rbWSBing.Checked)
                {
                    searchEngine = PanelWebSearcherInformation.Engine.BingAPI;
                    if (string.IsNullOrEmpty(Program.cfgCurrent.BingApiKey))
                    {
                        var result = MessageBox.Show(
                            @"Before searching with Bing's API you must set up a Bing API Key. You can do it in 'Options > General Config', do you continue with Web option?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        searchEngine = PanelWebSearcherInformation.Engine.BingWeb;

                        if (result == DialogResult.No)
                            return;
                    }
                }
                //Google
                else if (rbWSGoogle.Checked)
                {
                    searchEngine = PanelWebSearcherInformation.Engine.GoogleAPI;
                    if (string.IsNullOrEmpty(Program.cfgCurrent.GoogleApiKey) && string.IsNullOrEmpty(Program.cfgCurrent.GoogleApiCx))
                    {
                        var result = MessageBox.Show(
                            @"Before searching with Google API you must set up a Google API Key and Google Custom Search CX. You can do it in 'Options > General Config', do you continue with Web option? ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        searchEngine = PanelWebSearcherInformation.Engine.GoogleWeb;

                        if (result == DialogResult.No)
                            return;
                    }
                }

                bSearchShodan = cbShodan.Checked;
                bSearchCommonNames = cbDictionarySearch.Checked;
                CommonNamesFileName = textBoxCommonNames.Text;
                bSearchIPBing = cbIpBing.Checked;
                if (bSearchIPBing && this.searchIPEngine == PanelSearchIPBing.Engine.BingAPI && String.IsNullOrWhiteSpace(Program.cfgCurrent.BingApiKey))
                {
                    MessageBox.Show("The Bing api key is not configured. You have to provide an api key, or use IP Bing Web instead of IP Bing API", "Bing API key not configured", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MaxRecursion = Program.cfgCurrent.MaxRecursion;
                thrSearch = new Thread(Search)
                {
                    IsBackground = true,
                    Priority = ThreadPriority.BelowNormal
                };
                thrSearch.Start();
            }
            else
            {
                Abort();
                DisableSkip("Skip");
            }

            bSkipToNextSearch = false;
        }

        /// <summary>
        /// Star search for subdomains
        /// </summary>
        private void Search()
        {
            try
            {
                Invoke(new MethodInvoker(delegate
                {
                    DisableSkip("Skip");

                    buttonStart.Text = "&Stop";
                    buttonStart.Image = Resources.delete;
                }));

                var message = $"Searching subdomains of {strDomain}";
                Program.LogThis(new Log(Log.ModuleType.FOCA, message, Log.LogType.debug));
                Program.ChangeStatus(message);

                ChangeTextCurrentSearch("[initializing] Adding main domain information");
                DisableSkip("Skip");

                Program.data.AddDomain(strDomain, $"Added main domain [{strDomain}]", MaxRecursion,
                    Program.cfgCurrent);
                EnableSkip("Skip");

                if (bSearchWeb)
                {
                    EnableSkip("Skip");
                    ChangeTextCurrentSearch("Web Searcher");
                    SearchWeb();
                }

                if (bSearchCommonNames)
                {
                    EnableSkip("Skip");
                    ChangeTextCurrentSearch("Dictionary Search");
                    SearchCommonNames();
                }

                if (bSearchIPBing)
                {
                    EnableSkip("Skip to next IP");
                    ChangeTextCurrentSearch("IP Bing");
                    SearchIpBing();
                }
                if (bSearchShodan)
                {
                    EnableSkip("Skip Shodan");
                    ChangeTextCurrentSearch("Shodan Search");
                    SearchShodan();
                }

                ChangeTextCurrentSearch("");
                message = $"Subdomains search for {strDomain} finished";
                Program.LogThis(new Log(Log.ModuleType.FOCA, message, Log.LogType.debug));
                Program.ChangeStatus(message);
            }
            catch (ThreadAbortException)
            {
                const string strMessage = "Domain search aborted";
                Program.LogThis(new Log(Log.ModuleType.FOCA, strMessage, Log.LogType.debug));
                Program.ChangeStatus(strMessage);
            }
            catch (Exception e)
            {
                var strMessage = $"Error searching subdomain: {e.Message}";
                Program.LogThis(new Log(Log.ModuleType.DNSSearch, strMessage, Log.LogType.error));
                Program.ChangeStatus(strMessage);
            }
            finally
            {
                this.searchCancelToken?.Cancel();
                Invoke(new MethodInvoker(delegate
                {
                    buttonStart.Text = "&Start";
                    buttonStart.Image = Resources.tick;

                    ChangeTextCurrentSearch("None");
                    DisableSkip("Skip");
                }));
            }
        }

        /// <summary>
        /// Abort subdomains search tasks
        /// </summary>
        public void Abort()
        {
            if (thrSearch == null) return;
            thrSearch.Abort();
            ChangeTextCurrentSearch("");
            DisableSkip("Skip");
            this.searchCancelToken?.Cancel();
        }

        /// <summary>
        /// Custom logger for web searchers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void WebSearcherLogEvent(object sender, EventsThreads.ThreadStringEventArgs e)
        {
            Program.LogThis(new Log(Log.ModuleType.WebSearch, e.Message, Log.LogType.debug));
        }

        /// <summary>
        /// max length of the request is 2058 including the GET and HTTP/1.1 words
        /// we limit the string to 1900 to leave space for offset, count and filters
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        private bool CheckTotalLength(int len)
        {
            var res = len > 1900;
            if (res)
            {
                Program.LogThis(new Log(Log.ModuleType.WebSearch,
                    $"[{PanelWebSearcherInformation.EngineToString(searchEngine)}] Reached limit of 1900 characters in search string.",
                    Log.LogType.debug));
            }

            return res;
        }

        /// <summary>
        /// Logs that length limit was reached
        /// </summary>
        /// <param name="excludeSubdomains"></param>
        private void LogLimit(string excludeSubdomains)
        {
            Program.LogThis(new Log(Log.ModuleType.WebSearch,
                $"[{PanelWebSearcherInformation.EngineToString(searchEngine)}] Reached limit of 32 words in search string, the next subdomains can't be used to exclude them: {excludeSubdomains}",
                Log.LogType.debug));
        }

        /// <summary>
        /// Adds and logs a subdomain discovery event
        /// </summary>
        /// <param name="strHost">Discovered subdomain</param>
        private void AddAndLogSubdomainDiscover(string strHost)
        {
            Program.data.AddDomain(strHost,
                string.Format("WebSearch, {1} [{0}]", strHost,
                    PanelWebSearcherInformation.EngineToString(searchEngine)),
                MaxRecursion, Program.cfgCurrent);

            Program.LogThis(new Log(Log.ModuleType.WebSearch,
                $"[{PanelWebSearcherInformation.EngineToString(searchEngine)}] Found subdomain {strHost}",
                Log.LogType.debug));
        }

        private void CaptureSearchResults(object sender, EventsThreads.CollectionFound<Uri> e)
        {
            try
            {
                foreach (var group in e.Data.GroupBy(p => p.Host))
                {
                    CancelIfSkipRequested();
                    try
                    {
                        AddAndLogSubdomainDiscover(group.Key);

                        DomainsItem domain = Program.data.GetDomain(group.Key);
                        foreach (Uri url in group)
                        {
                            domain.map.AddUrl(url.ToString());
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch (InvalidCastException)
            {
            }
        }

        /// <summary>
        /// Perform web search based on the user's choice
        /// </summary>
        private void SearchWeb()
        {
            string message = $"Searching subdomains for {strDomain} in {PanelWebSearcherInformation.EngineToString(searchEngine)}";
            Program.LogThis(new Log(Log.ModuleType.WebSearch, message, Log.LogType.debug));
            Program.ChangeStatus(message);

            LinkSearcher searcher = null;
            switch (searchEngine)
            {
                case PanelWebSearcherInformation.Engine.GoogleWeb:
                    searcher = new GoogleWebSearcher
                    {
                        cSafeSearch = GoogleWebSearcher.SafeSearch.off,
                        FirstSeen = GoogleWebSearcher.FirstSeenGoogle.AnyTime,
                        LocatedInRegion = GoogleWebSearcher.Region.AnyRegion,
                        WriteInLanguage = GoogleWebSearcher.Language.AnyLanguage
                    };
                    break;
                case PanelWebSearcherInformation.Engine.GoogleAPI:
                    searcher = new GoogleAPISearcher(Program.cfgCurrent.GoogleApiKey, Program.cfgCurrent.GoogleApiCx);
                    break;
                case PanelWebSearcherInformation.Engine.BingWeb:
                    searcher = new BingWebSearcher
                    {
                        LocatedInRegion = BingWebSearcher.Region.AnyRegion,
                        WriteInLanguage = BingWebSearcher.Language.AnyLanguage
                    };
                    break;
                case PanelWebSearcherInformation.Engine.BingAPI:
                    searcher = new BingAPISearcher(Program.cfgCurrent.BingApiKey);
                    break;
                case PanelWebSearcherInformation.Engine.DuckDuckGoWeb:
                    searcher = new DuckduckgoWebSearcher();
                    break;
            }

            try
            {
                searcher.ItemsFoundEvent += CaptureSearchResults;
                searcher.SearcherLogEvent += WebSearcherLogEvent;
                CancelIfSkipRequested();

                searcher.SearchBySite(this.searchCancelToken, strDomain)
                    .ContinueWith((e) => SearchEngineFinish(e, searcher.Name, Log.ModuleType.WebSearch))
                    .Wait();
            }
            catch (OperationCanceledException)
            {
            }
        }

        /// <summary>
        /// Search subdomains using wordlists
        /// </summary>
        private void SearchCommonNames()
        {
            var message = $"Searching subdomains of {strDomain} using common DNS names";
            Program.LogThis(new Log(Log.ModuleType.DNSCommonNames, message, Log.LogType.debug));
            Program.ChangeStatus(message);

            var names = new List<string>();
            try
            {
                names.AddRange(File.ReadAllLines(CommonNamesFileName));
            }
            catch
            {
                Program.LogThis(new Log(Log.ModuleType.DNSCommonNames,
                    $"Error opening file: {CommonNamesFileName}", Log.LogType.error));
                return;
            }

            List<string> nsServerList = new List<string>();
            foreach (var item in Resolve.DnsServers)
            {
                nsServerList.AddRange(DNSUtil.GetNSServer(Resolve, strDomain, item.Address.ToString()));
            }

            foreach (var nsServer in nsServerList)
            {
                if (DNSUtil.IsDNSAnyCast(Resolve, nsServer, strDomain))
                    Program.LogThis(new Log(Log.ModuleType.DNSCommonNames,
                        $"DNS server is Anycast, not used: {nsServer}", Log.LogType.debug));
                else
                {
                    var op = Partitioner.Create(names);
                    var po = new ParallelOptions();
                    if (Program.cfgCurrent.ParallelDnsQueries != 0)
                        po.MaxDegreeOfParallelism = Program.cfgCurrent.ParallelDnsQueries;

                    try
                    {
                        Parallel.ForEach(op, po, delegate (string name)
                        {
                            CancelIfSkipRequested();

                            var subdomain = $"{name}.{strDomain}";
                            Program.LogThis(new Log(Log.ModuleType.DNSCommonNames,
                                string.Format("[{0}] Trying resolve subdomain: {1} with NameServer {0}", nsServer, subdomain),
                                Log.LogType.debug));

                            foreach (var ip in DNSUtil.GetHostAddresses(Resolve, subdomain, nsServer))
                            {
                                Program.LogThis(new Log(Log.ModuleType.DNSCommonNames,
                                    $"[{nsServer}] Found subdomain {subdomain}", Log.LogType.medium));

                                CancelIfSkipRequested();
                                try
                                {
                                    Program.data.AddResolution(subdomain, ip.ToString(),
                                        $"Common Names [{subdomain}]", MaxRecursion, Program.cfgCurrent,
                                        true);
                                }
                                catch (Exception)
                                {
                                }
                            }
                        });
                    }
                    catch (AggregateException)
                    { }
                    catch (OperationCanceledException)
                    {
                    }

                    if (!bSearchWithAllDNS)
                        break;
                }
            }
        }

        /// <summary>
        /// Log an event of Bing's IP searcher
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IpBingSearcherLogEvent(object sender, EventsThreads.ThreadStringEventArgs e)
        {
            Program.LogThis(new Log(Log.ModuleType.IPBingSearch, e.Message, Log.LogType.debug));
        }

        /// <summary>
        /// Search an IP address in Bing
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool SearchIpBingSingleIp(string ip)
        {
            CancelIfSkipRequested();

            if (ip.Contains("\\") || ip.Contains("/") || !DNSUtil.IsIPv4(ip))
                return false;

            var message = $"[{PanelSearchIPBing.EngineToString(searchIPEngine)}] Searching domains in IP {ip}";
            Program.LogThis(new Log(Log.ModuleType.IPBingSearch, message, Log.LogType.debug));
            Program.ChangeStatus(message);
            LinkSearcher searcher = null;
            try
            {
                switch (searchIPEngine)
                {
                    case PanelSearchIPBing.Engine.BingWeb:
                        searcher = new BingWebSearcher
                        {
                            LocatedInRegion = BingWebSearcher.Region.AnyRegion,
                            WriteInLanguage = BingWebSearcher.Language.AnyLanguage
                        };
                        break;
                    case PanelSearchIPBing.Engine.BingAPI:
                        searcher = new BingAPISearcher(Program.cfgCurrent.BingApiKey);
                        break;
                }
                SearchIP(ip, searcher);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private void SearchIP(string ip, LinkSearcher searcher)
        {
            ConcurrentBag<string> domainsFound = new ConcurrentBag<string>();
            searcher.ItemsFoundEvent +=
                delegate (object sender, EventsThreads.CollectionFound<Uri> e)
                {
                    var op = Partitioner.Create(e.Data);
                    var po = new ParallelOptions();
                    if (Program.cfgCurrent.ParallelDnsQueries != 0)
                        po.MaxDegreeOfParallelism = Program.cfgCurrent.ParallelDnsQueries;
                    try
                    {
                        Parallel.ForEach(op, po, delegate (Uri url, ParallelLoopState loopState)
                        {
                            CancelIfSkipRequested();
                            try
                            {
                                if (domainsFound.Any(d => d.ToLower() == url.Host.ToLower()))
                                {
                                    return;
                                }
                                domainsFound.Add(url.Host);

                                bool isInBaseDomain = false;
                                try
                                {
                                    if (Dns.GetHostAddresses(url.Host).Any(IP => IP.ToString() == ip))
                                        isInBaseDomain = true;
                                }
                                catch
                                {
                                }

                                if (!isInBaseDomain)
                                    return;

                                string source = $"{Program.data.GetIpSource(ip)} > {searcher.Name} [{url.Host}]";
                                Program.data.AddResolution(url.Host, ip, source, MaxRecursion, Program.cfgCurrent, true);

                                Program.LogThis(new Log(Log.ModuleType.IPBingSearch,
                                    $"[{PanelSearchIPBing.EngineToString(searchIPEngine)}] Found domain {url.Host} in IP {ip}",
                                    Log.LogType.medium));
                            }
                            catch
                            {
                            }
                        });
                    }
                    catch (AggregateException)
                    {
                    }
                };
            searcher.SearcherLogEvent += IpBingSearcherLogEvent;

            searcher.CustomSearch(this.searchCancelToken, $"ip:{ip}")
                .ContinueWith((e) => SearchEngineFinish(e, searcher.Name, Log.ModuleType.IPBingSearch))
                .Wait();
        }

        /// <summary>
        /// Search for IP addresses on Bing
        /// </summary>
        private void SearchIpBing()
        {
            try
            {
                var ips = Program.data.GetIPs();
                foreach (var ip in ips)
                {
                    SearchIpBingSingleIp(ip);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Search for a range of IP addresses in Shodan
        /// </summary>
        private void SearchShodan()
        {
            List<string> ranges = Program.data.GetIPs().Select(x => x.Remove(x.LastIndexOf('.') + 1)).Distinct().ToList();

            List<IPAddress> lstIpsInRanges = new List<IPAddress>();

            foreach (var t in ranges)
            {
                for (var i = 0; i < 254; i++)
                {
                    string targetIp = t + i;
                    if (!String.IsNullOrWhiteSpace(Program.data.GetIp(targetIp).Ip) && IPAddress.TryParse(targetIp, out IPAddress currentIP))
                        lstIpsInRanges.Add(currentIP);
                }
            }
            SearchIpInShodan(lstIpsInRanges);
        }

        public void SearchIpInShodan(List<IPAddress> lstIPs)
        {
            if (string.IsNullOrEmpty(Program.cfgCurrent.ShodanApiKey))
            {
                MessageBox.Show(@"Before searching with Shodan's API you must set up a Shodan API Key. You can do it in 'Options > General Config'");
                return;
            }

            Invoke(new MethodInvoker(delegate
            {
                const string strMessage = "Searching IPs in Shodan";
                Program.FormMainInstance.toolStripStatusLabelLeft.Text = strMessage;
                Program.LogThis(new Log(Log.ModuleType.ShodanSearch, strMessage, Log.LogType.debug));
            }));

            ShodanSearcher shodanSearcher = new ShodanSearcher(Program.cfgCurrent.ShodanApiKey);
            shodanSearcher.ItemsFoundEvent += Program.FormMainInstance.ShodanDataFound;
            shodanSearcher.SearcherLogEvent += Program.FormMainInstance.ShodanLog;

            shodanSearcher.CustomSearch(this.searchCancelToken, lstIPs)
                .ContinueWith((e) => SearchEngineFinish(e, shodanSearcher.Name, Log.ModuleType.ShodanSearch))
                .Wait();
        }

        private void SearchEngineFinish(Task<int> previousTask, string engineName, Log.ModuleType module)
        {
            string logMessage = String.Empty;
            Log.LogType logType = Log.LogType.medium;
            if (previousTask.IsCanceled)
            {
                logMessage = $"{engineName} search aborted!!";
            }
            else if (previousTask.IsFaulted)
            {
                logMessage = $"An error has ocurred on {engineName}: {String.Join(Environment.NewLine, (previousTask.Exception)?.InnerException.Message)}.";
                logType = Log.LogType.error;
            }
            else if (previousTask.IsCompleted)
            {
                logMessage = $"{engineName} search finished successfully!!";
            }

            Program.LogThis(new Log(module, logMessage, logType));
        }

        /// <summary>
        /// Check to skip.
        /// </summary>
        /// <returns></returns>
        private void CancelIfSkipRequested()
        {
            if (bSkipToNextSearch)
            {
                this.searchCancelToken.Cancel();
                this.searchCancelToken.Token.ThrowIfCancellationRequested();
            }
        }

        /// <summary>
        /// Change text current search.
        /// </summary>
        /// <param name="current"></param>
        private void ChangeTextCurrentSearch(string current)
        {
            CheckForIllegalCrossThreadCalls = false;
            btSkip.Enabled = true;
            lbCurrentSearch.Text = @"Current search: " + current;
            CheckForIllegalCrossThreadCalls = true;
        }

        /// <summary>
        /// Disable Skip.
        /// </summary>
        /// <param name="text"></param>
        private void DisableSkip(string text)
        {
            CheckForIllegalCrossThreadCalls = false;
            btSkip.Enabled = false;
            btSkip.Text = text;
            CheckForIllegalCrossThreadCalls = true;
        }

        /// <summary>
        /// Enable Skip
        /// </summary>
        /// <param name="text"></param>
        private void EnableSkip(string text)
        {
            CheckForIllegalCrossThreadCalls = false;
            bSkipToNextSearch = false;
            btSkip.Enabled = true;
            btSkip.Text = text;
            CheckForIllegalCrossThreadCalls = true;
            if (this.searchCancelToken == null || this.searchCancelToken.IsCancellationRequested)
            {
                this.searchCancelToken = new CancellationTokenSource();
            }
        }

        private void btSkip_Click(object sender, EventArgs e)
        {
            bSkipToNextSearch = true;
            btSkip.Enabled = false;
        }

        private void MouseOver(object sender, MouseEventArgs e)
        {
            panelWebSearcher.Visible = sender == checkedButtonWebSearcher;
            panelDNSSearchInformation.Visible = sender == checkedButtonDNSSearch;
            panelTryTransferZone.Visible = sender == checkedButtonTransferZone;
            panelTryCommonNames.Visible = sender == checkedButtonNamesSearch;
            panelSearchIPBing.Visible = sender == checkedButtonIPBing;
            panelShodan.Visible = sender == checkedButtonShodan;
        }

        /// <summary>
        /// Load wordlist button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btLoad_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = @"All Files(*.*)|*.*",
                InitialDirectory = string.IsNullOrEmpty(textBoxCommonNames.Text)
                    ? $@"{Application.StartupPath}\DNSDictionary\"
                    : Path.GetDirectoryName(textBoxCommonNames.Text)
            };


            if (openFileDialog.ShowDialog() == DialogResult.OK)
                textBoxCommonNames.Text = openFileDialog.FileName;
        }

        private void WsEngineChanged(object sender, EventArgs e)
        {
            if (rbWSGoogle.Checked)
            {
                lbWsTitle.Text = @"Google Web  limitations";
                lbWs1.Text = @"-Max 1000 results for each search";
                lbWs2.Text = @"-Max 32 words in a search string";
            }
            else if (rbWSDuckDuckGo.Checked)
            {
                lbWsTitle.Text = @"DuckDuckGo  limitations";
                lbWs1.Text = @"-Max 64 results for each search";
                lbWs2.Text = @"-Max 32 words in a search string";
            }
            else if (rbWSBing.Checked)
            {
                lbWsTitle.Text = @"Bing Web  limitations";
                lbWs1.Text = @"-Max 1000 results for each search";
                lbWs2.Text = @"-Max 49 words in a search string";
            }
        }

        private void IpBingEngineChanged(object sender, EventArgs e)
        {
            if (rbIPBingAPI.Checked)
            {
                lbIpBingTitle.Text = @"Bing API  limitations";
                this.searchIPEngine = PanelSearchIPBing.Engine.BingAPI;
            }
            else if (rbIPBingWeb.Checked)
            {
                lbIpBingTitle.Text = @"Bing Web  limitations";
                this.searchIPEngine = PanelSearchIPBing.Engine.BingWeb;
            }

            lbIpBing1.Text = @"-Max 1000 results for each search";
            lbIpBing2.Text = @"-Max 49 words in a search string";
        }
    }
}
