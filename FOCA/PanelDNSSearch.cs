using FOCA.Analysis;
using FOCA.Properties;
using FOCA.Searcher;
using FOCA.Threads;
using Heijden.DNS;
using MetadataExtractCore;
using MetadataExtractCore.Diagrams;
using SearcherCore.Searcher.BingAPI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
                if (rbWSDuckDuck.Checked)
                    searchEngine = PanelWebSearcherInformation.Engine.DuckDuckWeb;
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
        /// Setup Web Search
        /// </summary>
        private void SetUpWebSearch()
        {
            var message =
                $"Searching subdomains for {strDomain} in {PanelWebSearcherInformation.EngineToString(searchEngine)}";
            Program.LogThis(new Log(Log.ModuleType.WebSearch, message, Log.LogType.debug));
            Program.ChangeStatus(message);
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
        /// Checks thread's end reason and logs if the reason was LimitReached
        /// </summary>
        /// <param name="endReason"></param>
        /// <param name="currentResults"></param>
        /// <param name="strSearchString"></param>
        /// <param name="wordsCountLimit"></param>
        private void CheckEndReason(EventsThreads.ThreadEndEventArgs.EndReasonEnum endReason,
            IList<string> currentResults, string strSearchString, int wordsCountLimit)
        {
            if (strSearchString == null) return;
            if (endReason != EventsThreads.ThreadEndEventArgs.EndReasonEnum.LimitReached) return;
            var newSearchString = new StringBuilder($"site:{strDomain}");

            var wordsCounter = 1;
            foreach (var item in currentResults)
            {
                var strExcludeSite = $" -site:{item}";
                if (CheckTotalLength(newSearchString.Length + strExcludeSite.Length))
                    break;

                newSearchString.Append(strExcludeSite);
                if (++wordsCounter != wordsCountLimit) continue;
                var excludeSubDomains = string.Empty;
                var currentIndex = currentResults.IndexOf(item);
                for (var i = currentIndex; i < currentResults.Count; i++)
                    excludeSubDomains += currentResults[i] + " ";
                LogLimit(excludeSubDomains);
                break;
            }
            strSearchString = newSearchString.ToString();
            Program.LogThis(new Log(Log.ModuleType.WebSearch,
                $"[{PanelWebSearcherInformation.EngineToString(searchEngine)}] Searching again with restricted sites, search string: {strSearchString}",
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

        /// <summary>
        /// Perform the DNS search through Google Web
        /// </summary>
        private void GoogleWebSearch()
        {
            GoogleWebSearcher googleSearcher = null;
            try
            {
                googleSearcher = new GoogleWebSearcher
                {
                    SearchAll = true,
                    cSafeSearch = GoogleWebSearcher.SafeSearch.off,
                    FirstSeen = GoogleWebSearcher.FirstSeenGoogle.AnyTime,
                    LocatedInRegion = GoogleWebSearcher.Region.AnyRegion,
                    WriteInLanguage = GoogleWebSearcher.Language.AnyLanguage
                };

                var currentResults = new List<string>();

                googleSearcher.SearcherLinkFoundEvent +=
                    delegate (object sender, EventsThreads.ThreadListDataFoundEventArgs e)
                    {
                        var searcher = (GoogleWebSearcher)sender;
                        foreach (var item in e.Data)
                        {
                            if (CheckToSkip())
                                searcher.Abort();
                            try
                            {
                                if (item == null)
                                    continue;
                                var url = new Uri((string)item);
                                var strHost = url.Host;
                                if (
                                    currentResults.All(
                                        d =>
                                            !string.Equals(d, strHost, StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    currentResults.Add(strHost);
                                    AddAndLogSubdomainDiscover(strHost);
                                }
                                var domain = Program.data.GetDomain(url.Host);
                                domain.map.AddUrl(url.ToString());
                            }
                            catch
                            {
                            }
                        }
                    };

                googleSearcher.SearcherLogEvent += WebSearcherLogEvent;
                var endReason = EventsThreads.ThreadEndEventArgs.EndReasonEnum.ErrorFound;
                googleSearcher.SearcherEndEvent +=
                    delegate (object o, EventsThreads.ThreadEndEventArgs e) { endReason = e.EndReason; };
                var strSearchString = $"site:{strDomain}";
                int resultsNumber;
                do
                {
                    if (CheckToSkip())
                        return;

                    resultsNumber = currentResults.Count;
                    googleSearcher.GetCustomLinks(strSearchString);
                    googleSearcher.Join();

                    CheckEndReason(endReason, currentResults, strSearchString, 32);
                } while (endReason == EventsThreads.ThreadEndEventArgs.EndReasonEnum.LimitReached &&
                         resultsNumber != currentResults.Count);
            }
            catch (ThreadAbortException)
            {
            }
            finally
            {
                googleSearcher?.Abort();
            }
        }

        /// <summary>
        ///     Perform the search through Google API
        /// </summary>
        private void GoogleApiSearch()
        {
            GoogleAPISearcher googleApiSearcher = null;
            try
            {
                googleApiSearcher = new GoogleAPISearcher
                {
                    GoogleApiKey = Program.cfgCurrent.GoogleApiKey,
                    GoogleApiCx = Program.cfgCurrent.GoogleApiCx,
                    SearchAll = true
                };

                var currentResults = new List<string>();
                googleApiSearcher.SearcherLinkFoundEvent +=
                    delegate (object sender, EventsThreads.ThreadListDataFoundEventArgs e)
                    {
                        var searcher = (GoogleAPISearcher)sender;

                        foreach (var item in e.Data)
                        {
                            if (CheckToSkip())
                                searcher.Abort();

                            try
                            {
                                var gr = (GoogleAPISearcher.GoogleAPIResults)item;
                                var url = new Uri(gr.Url);
                                var strHost = url.Host;
                                if (
                                    currentResults.All(
                                        D => !string.Equals(D, strHost, StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    currentResults.Add(strHost);
                                    AddAndLogSubdomainDiscover(strHost);
                                }
                                var domain = Program.data.GetDomain(url.Host);
                                domain.map.AddUrl(url.ToString());
                            }
                            catch
                            {
                            }
                        }
                    };
                googleApiSearcher.SearcherLogEvent += WebSearcherLogEvent;
                var endReason = EventsThreads.ThreadEndEventArgs.EndReasonEnum.ErrorFound;
                googleApiSearcher.SearcherEndEvent +=
                    delegate (object o, EventsThreads.ThreadEndEventArgs e) { endReason = e.EndReason; };
                var strSearchString = $"site:{strDomain}";
                int resultsNumber;
                do
                {
                    if (CheckToSkip())
                        return;

                    resultsNumber = currentResults.Count;
                    googleApiSearcher.GetCustomLinks(strSearchString);
                    googleApiSearcher.Join();

                    CheckEndReason(endReason, currentResults, strSearchString, 32);
                } while (endReason == EventsThreads.ThreadEndEventArgs.EndReasonEnum.LimitReached &&
                         resultsNumber != currentResults.Count);
            }
            catch (ThreadAbortException)
            {
            }
            finally
            {
                googleApiSearcher?.Abort();
            }
        }

        /// <summary>
        /// Perform the search through DuckDuckGo Web
        /// </summary>
        private void DuckDuckWebSearch()
        {
            DuckduckgoWebSearcher duckSearcher = null;

            try
            {
                duckSearcher = new DuckduckgoWebSearcher();

                var currentResults = new List<string>();
                duckSearcher.SearcherLinkFoundEvent +=
                    delegate (object sender, EventsThreads.ThreadListDataFoundEventArgs e)
                    {
                        var searcher = (DuckduckgoWebSearcher)sender;
                        foreach (var item in e.Data)
                        {
                            if (CheckToSkip())
                                searcher.Abort();

                            try
                            {
                                var url = new Uri((string)item);
                                var strHost = url.Host;
                                if (currentResults.All(d => !string.Equals(d, strHost, StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    currentResults.Add(strHost);
                                    AddAndLogSubdomainDiscover(strHost);
                                }

                                var domain = Program.data.GetDomain(url.Host);
                                domain.map.AddUrl(url.ToString());
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    };

                duckSearcher.SearcherLogEvent += WebSearcherLogEvent;
                var endReason = EventsThreads.ThreadEndEventArgs.EndReasonEnum.ErrorFound;
                duckSearcher.SearcherEndEvent +=
                    delegate (object o, EventsThreads.ThreadEndEventArgs e) { endReason = e.EndReason; };
                var strSearchString = $"site:{strDomain}";
                int nroResultados;

                do
                {
                    if (CheckToSkip())
                        return;

                    nroResultados = currentResults.Count;
                    duckSearcher.GetCustomLinks(strSearchString);
                    duckSearcher.Join();

                    CheckEndReason(endReason, currentResults, strSearchString, 49);
                } while (endReason == EventsThreads.ThreadEndEventArgs.EndReasonEnum.LimitReached &&
                         nroResultados != currentResults.Count);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                duckSearcher?.Abort();
            }
        }


        /// <summary>
        /// Perform the search through Bing Web
        /// </summary>
        private void BingWebSearch()
        {
            BingWebSearcher bingSearcher = null;
            try
            {
                bingSearcher = new BingWebSearcher
                {
                    LocatedInRegion = BingWebSearcher.Region.AnyRegion,
                    SearchAll = true,
                    WriteInLanguage = BingWebSearcher.Language.AnyLanguage
                };

                var currentResults = new List<string>();
                bingSearcher.SearcherLinkFoundEvent +=
                    delegate (object sender, EventsThreads.ThreadListDataFoundEventArgs e)
                    {
                        var searcher = (BingWebSearcher)sender;

                        foreach (var item in e.Data)
                        {
                            if (CheckToSkip())
                                searcher.Abort();

                            try
                            {
                                var url = new Uri((string)item);
                                var strHost = url.Host;
                                if (
                                    currentResults.All(
                                        D => !string.Equals(D, strHost, StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    currentResults.Add(strHost);
                                    AddAndLogSubdomainDiscover(strHost);
                                }
                                var domain = Program.data.GetDomain(url.Host);
                                domain.map.AddUrl(url.ToString());
                            }
                            catch
                            {
                            }
                        }
                    };

                bingSearcher.SearcherLogEvent += WebSearcherLogEvent;
                var endReason = EventsThreads.ThreadEndEventArgs.EndReasonEnum.ErrorFound;
                bingSearcher.SearcherEndEvent +=
                    delegate (object o, EventsThreads.ThreadEndEventArgs e) { endReason = e.EndReason; };
                var strSearchString = $"site:{strDomain}";
                int nroResultados;
                do
                {
                    if (CheckToSkip())
                        return;

                    nroResultados = currentResults.Count;
                    bingSearcher.GetCustomLinks(strSearchString);
                    bingSearcher.Join();

                    CheckEndReason(endReason, currentResults, strSearchString, 49);
                } while (endReason == EventsThreads.ThreadEndEventArgs.EndReasonEnum.LimitReached &&
                         nroResultados != currentResults.Count);
            }
            catch (ThreadAbortException)
            {
            }
            finally
            {
                bingSearcher?.Abort();
            }
        }

        /// <summary>
        /// Perform the search through Bing API
        /// </summary>
        private void BingApiSearch()
        {
            BingAPISearcher bingSearcherApi = null;
            try
            {
                bingSearcherApi = new BingAPISearcher(Program.cfgCurrent.BingApiKey);

                var currentResults = new List<string>();
                bingSearcherApi.SearcherLinkFoundEvent +=
                    delegate (object sender, EventsThreads.ThreadListDataFoundEventArgs e)
                    {
                        var searcher = (BingAPISearcher)sender;

                        foreach (var item in e.Data)
                        {
                            if (CheckToSkip())
                                searcher.Abort();

                            try
                            {
                                var br = (BingApiResult)item;
                                var url = new Uri(br.Url);
                                var strHost = url.Host;
                                if (
                                    currentResults.All(
                                        D => !string.Equals(D, strHost, StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    currentResults.Add(strHost);
                                    AddAndLogSubdomainDiscover(strHost);
                                }
                                var domain = Program.data.GetDomain(url.Host);
                                domain.map.AddUrl(url.ToString());
                            }
                            catch
                            {
                            }
                        }
                    };
                bingSearcherApi.SearcherLogEvent += WebSearcherLogEvent;
                var endReason = EventsThreads.ThreadEndEventArgs.EndReasonEnum.ErrorFound;
                bingSearcherApi.SearcherEndEvent +=
                    delegate (object o, EventsThreads.ThreadEndEventArgs e) { endReason = e.EndReason; };
                var strSearchString = $"site:{strDomain}";
                int nroResultados;
                do
                {
                    if (CheckToSkip())
                        return;

                    nroResultados = currentResults.Count;
                    bingSearcherApi.GetCustomLinks(strSearchString);
                    bingSearcherApi.Join();

                    CheckEndReason(endReason, currentResults, strSearchString, 49);
                } while (endReason == EventsThreads.ThreadEndEventArgs.EndReasonEnum.LimitReached &&
                         nroResultados != currentResults.Count);
            }
            catch (ThreadAbortException)
            {
            }
            finally
            {
                bingSearcherApi?.Abort();
            }
        }

        /// <summary>
        /// Perform web search based on the user's choice
        /// </summary>
        private void SearchWeb()
        {
            SetUpWebSearch();

            switch (searchEngine)
            {
                case PanelWebSearcherInformation.Engine.GoogleWeb:
                    GoogleWebSearch();
                    break;
                case PanelWebSearcherInformation.Engine.GoogleAPI:
                    GoogleApiSearch();
                    break;
                case PanelWebSearcherInformation.Engine.BingWeb:
                    BingWebSearch();
                    break;
                case PanelWebSearcherInformation.Engine.BingAPI:
                    BingApiSearch();
                    break;
                case PanelWebSearcherInformation.Engine.DuckDuckWeb:
                    DuckDuckWebSearch();
                    break;
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
                delegate (object sender, EventsThreads.ThreadListDataFoundEventArgs e)
                {
                    var op = Partitioner.Create(e.Data);
                    var po = new ParallelOptions();
                    if (Program.cfgCurrent.ParallelDnsQueries != 0)
                        po.MaxDegreeOfParallelism = Program.cfgCurrent.ParallelDnsQueries;
                    Parallel.ForEach(op, po, delegate (object item, ParallelLoopState loopState)
                    {
                        if (CheckToSkip())
                            loopState.Stop();
                        try
                        {
                            var br = (BingApiResult)item;
                            var url = new Uri(br.Url);
                            if (
                                currentResults.Any(d => string.Equals(d, url.Host, StringComparison.CurrentCultureIgnoreCase)))
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
                    }
                        );
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
                delegate (object sender, EventsThreads.ThreadListDataFoundEventArgs e)
                {
                    var op = Partitioner.Create(e.Data);
                    var po = new ParallelOptions();
                    if (Program.cfgCurrent.ParallelDnsQueries != 0)
                        po.MaxDegreeOfParallelism = Program.cfgCurrent.ParallelDnsQueries;
                    Parallel.ForEach(op, po, delegate (object item, ParallelLoopState loopState)
                    {
                        if (CheckToSkip())
                            loopState.Stop();
                        try
                        {
                            var url = new Uri((string)item);
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
                    }
                        );
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
            else if (rbWSDuckDuck.Checked)
            {
                lbWsTitle.Text = @"DuckDuck  limitations";
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