using FOCA.Analysis;
using FOCA.Database.Entities;
using FOCA.Properties;
using FOCA.Searcher;
using FOCA.Threads;
using FOCA.Utilites;
using Heijden.DNS;
using MetadataExtractCore;
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
                TimeOut = 1000,
                SearchWithAllDNS = Program.cfgCurrent.UseAllDns
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
            WebSearcher searcher = (WebSearcher)sender;
            foreach (var group in e.Data.GroupBy(p => p.Host))
            {
                if (CheckToSkip())
                    searcher.Abort();

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

        /// <summary>
        /// Perform web search based on the user's choice
        /// </summary>
        private void SearchWeb()
        {
            string message = $"Searching subdomains for {strDomain} in {PanelWebSearcherInformation.EngineToString(searchEngine)}";
            Program.LogThis(new Log(Log.ModuleType.WebSearch, message, Log.LogType.debug));
            Program.ChangeStatus(message);

            WebSearcher searcher = null;
            switch (searchEngine)
            {
                case PanelWebSearcherInformation.Engine.GoogleWeb:
                    searcher = new GoogleWebSearcher
                    {
                        SearchAll = true,
                        cSafeSearch = GoogleWebSearcher.SafeSearch.off,
                        FirstSeen = GoogleWebSearcher.FirstSeenGoogle.AnyTime,
                        LocatedInRegion = GoogleWebSearcher.Region.AnyRegion,
                        WriteInLanguage = GoogleWebSearcher.Language.AnyLanguage
                    };
                    break;
                case PanelWebSearcherInformation.Engine.GoogleAPI:
                    searcher = new GoogleAPISearcher(Program.cfgCurrent.GoogleApiKey, Program.cfgCurrent.GoogleApiCx)
                    {
                        SearchAll = true
                    };
                    break;
                case PanelWebSearcherInformation.Engine.BingWeb:
                    searcher = new BingWebSearcher
                    {
                        LocatedInRegion = BingWebSearcher.Region.AnyRegion,
                        SearchAll = true,
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
                searcher.SearcherLinkFoundEvent += CaptureSearchResults;
                searcher.SearcherLogEvent += WebSearcherLogEvent;
                string strSearchString = $"site:{strDomain}";
                if (CheckToSkip())
                    return;

                searcher.GetCustomLinks(strSearchString);
                searcher.Join();
            }
            catch (ThreadAbortException)
            {
            }
            finally
            {
                searcher?.Abort();
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

            var nsServerList = DNSUtil.GetNSServer(Resolve, strDomain, Resolve.DnsServers[0].Address.ToString());

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

                    Parallel.ForEach(op, po, delegate (string name)
                    {
                        if (CheckToSkip())
                            return;

                        var subdomain = $"{name}.{strDomain}";
                        Program.LogThis(new Log(Log.ModuleType.DNSCommonNames,
                            string.Format("[{0}] Trying resolve subdomain: {1} with NameServer {0}", nsServer, subdomain),
                            Log.LogType.debug));

                        foreach (var ip in DNSUtil.GetHostAddresses(Resolve, subdomain, nsServer))
                        {
                            Program.LogThis(new Log(Log.ModuleType.DNSCommonNames,
                                $"[{nsServer}] Found subdomain {subdomain}", Log.LogType.medium));
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
                    }
                        );
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
            if (CheckToSkip())
            {
                EnableSkip("Skip to next IP");
                return false;
            }

            if (ip.Contains("\\") || ip.Contains("/") || !DNSUtil.IsIPv4(ip))
                return false;

            var message = $"[{PanelSearchIPBing.EngineToString(searchIPEngine)}] Searching domains in IP {ip}";
            Program.LogThis(new Log(Log.ModuleType.IPBingSearch, message, Log.LogType.debug));
            Program.ChangeStatus(message);
            List<string> currentResults;

            switch (searchIPEngine)
            {
                case PanelSearchIPBing.Engine.BingWeb:
                    try
                    {
                        var bingSearcher = new BingWebSearcher
                        {
                            LocatedInRegion = BingWebSearcher.Region.AnyRegion,
                            SearchAll = true,
                            WriteInLanguage = BingWebSearcher.Language.AnyLanguage
                        };

                        currentResults = new List<string>();

                        SerchLinkWebBingEvent(ip, bingSearcher, currentResults);
                        break;
                    }
                    catch
                    {
                        break;
                    }
                case PanelSearchIPBing.Engine.BingAPI:
                    var bingSearcherApi = new BingAPISearcher(Program.cfgCurrent.BingApiKey);
                    currentResults = new List<string>();
                    SerchLinkApiBingEvent(ip, bingSearcherApi, currentResults);
                    break;
            }

            return true;
        }

        private void SerchLinkApiBingEvent(string ip, BingAPISearcher bingSearcherApi, List<string> currentResults)
        {
            bingSearcherApi.SearcherLinkFoundEvent +=
                delegate (object sender, EventsThreads.CollectionFound<Uri> e)
                {
                    var op = Partitioner.Create(e.Data);
                    var po = new ParallelOptions();
                    if (Program.cfgCurrent.ParallelDnsQueries != 0)
                        po.MaxDegreeOfParallelism = Program.cfgCurrent.ParallelDnsQueries;
                    Parallel.ForEach(op, po, delegate (Uri url, ParallelLoopState loopState)
                    {
                        if (CheckToSkip())
                            loopState.Stop();
                        try
                        {
                            if (currentResults.Any(d => string.Equals(d, url.Host, StringComparison.CurrentCultureIgnoreCase)))
                                return;
                            currentResults.Add(url.Host);
                            var source = $"{Program.data.GetIpSource(ip)} > Bing IP Search [{url.Host}]";
                            Program.data.AddResolution(url.Host, ip, source, MaxRecursion,
                                Program.cfgCurrent, true);

                            Program.LogThis(new Log(Log.ModuleType.IPBingSearch,
                                $"[{PanelSearchIPBing.EngineToString(searchIPEngine)}] Found domain {url.Host} in IP {ip}",
                                Log.LogType.medium));
                        }
                        catch
                        {
                        }
                    });
                };
            bingSearcherApi.SearcherLogEvent += IpBingSearcherLogEvent;
            bingSearcherApi.GetCustomLinks($"ip:{ip}");
            bingSearcherApi.Join();
        }

        /// <summary>
        /// Event for search link found.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="bingSearcher"></param>
        /// <param name="currentResults"></param>
        private void SerchLinkWebBingEvent(string ip, BingWebSearcher bingSearcher, List<string> currentResults)
        {
            bingSearcher.SearcherLinkFoundEvent +=
                delegate (object sender, EventsThreads.CollectionFound<Uri> e)
                {
                    var op = Partitioner.Create(e.Data);
                    var po = new ParallelOptions();
                    if (Program.cfgCurrent.ParallelDnsQueries != 0)
                        po.MaxDegreeOfParallelism = Program.cfgCurrent.ParallelDnsQueries;
                    Parallel.ForEach(op, po, delegate (Uri url, ParallelLoopState loopState)
                    {
                        if (CheckToSkip())
                            loopState.Stop();
                        try
                        {
                            if (currentResults.Any(d => d.ToLower() == url.Host.ToLower())) return;
                            currentResults.Add(url.Host);

                            var isInBaseDomain = false;
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

                            var source = $"{Program.data.GetIpSource(ip)} > Bing IP Search [{url.Host}]";
                            Program.data.AddResolution(url.Host, ip, source, MaxRecursion,
                                Program.cfgCurrent, true);

                            Program.LogThis(new Log(Log.ModuleType.IPBingSearch,
                                $"[{PanelSearchIPBing.EngineToString(searchIPEngine)}] Found domain {url.Host} in IP {ip}",
                                Log.LogType.medium));
                        }
                        catch
                        {
                        }
                    });
                };
            bingSearcher.SearcherLogEvent += IpBingSearcherLogEvent;
            bingSearcher.GetCustomLinks($"ip:{ip}");
            bingSearcher.Join();
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
            var ranges = new ModifiedComponents.ThreadSafeList<string>();
            var ips = Program.data.GetIPs().Select(x => x.Remove(x.LastIndexOf('.') + 1)).Distinct().ToList();

            ips.ForEach(x => ranges.Add(x));

            var lstIpsInRanges = new ModifiedComponents.ThreadSafeList<string>();

            foreach (var t in ranges)
            {
                for (var i = 0; i < 254; i++)
                {
                    var targetIp = t + i;
                    if (Program.data.GetIp(targetIp).Ip != "")
                        lstIpsInRanges.Add(targetIp);
                }
            }
            SearchIpInShodanNoAsync(lstIpsInRanges);
        }

        /// <summary>
        /// Search a netrange in Shodan
        /// </summary>
        /// <param name="lstNetRange"></param>
        public void SearchNetrangeShodanNoAsync(ThreadSafeList<NetRange> lstNetRange)
        {
            Invoke(new MethodInvoker(delegate
            {
                const string strMessage = "Searching IPs in Shodan by netrange";
                Program.FormMainInstance.toolStripStatusLabelLeft.Text = strMessage;
                Program.LogThis(new Log(Log.ModuleType.ShodanSearch, strMessage, Log.LogType.debug));
            }));
            if (string.IsNullOrEmpty(Program.cfgCurrent.ShodanApiKey))
            {
                MessageBox.Show(@"Before searching with Shodan's API you must set up a Shodan API Key. You can do it in 'Options > General Config'");
                return;
            }
            var sr = new ShodanRecognition(Program.cfgCurrent.ShodanApiKey);
            sr.StartRecognitionNetRangeNoAsync(lstNetRange);
        }

        /// <summary>
        /// Search an IP in Shodan
        /// </summary>
        /// <param name="oThreadSafeListIp"></param>
        public void SearchIpInShodanNoAsync(object oThreadSafeListIp)
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

            var lstIPs = (oThreadSafeListIp as ModifiedComponents.ThreadSafeList<string>);

            var shodanItem = new ShodanRecognition(Program.cfgCurrent.ShodanApiKey, lstIPs);
            shodanItem.DataFoundEvent += Program.FormMainInstance.ShodanDataFound;
            shodanItem.LogEvent += Program.FormMainInstance.ShodanLog;
            shodanItem.EndEvent += delegate
            {
                const string strMessage = "Finish Searching IPs in Shodan";
                Program.FormMainInstance.toolStripStatusLabelLeft.Text = strMessage;
                Program.LogThis(new Log(Log.ModuleType.Shodan, strMessage, Log.LogType.debug));
                shodanItem = null;
            };

            shodanItem.StartRecognitionNoAsync();
        }

        /// <summary>
        /// Check to skip.
        /// </summary>
        /// <returns></returns>
        private bool CheckToSkip()
        {
            return bSkipToNextSearch;
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
                lbIpBingTitle.Text = @"Bing API  limitations";

            else if (rbIPBingWeb.Checked)
                lbIpBingTitle.Text = @"Bing Web  limitations";

            lbIpBing1.Text = @"-Max 1000 results for each search";
            lbIpBing2.Text = @"-Max 49 words in a search string";
        }
    }
}