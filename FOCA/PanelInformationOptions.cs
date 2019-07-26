using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using FOCA.Analysis.HttpMap;
using FOCA.Properties;
using FOCA.Searcher;
using FOCA.Threads;

namespace FOCA
{
    /// <summary>
    ///     This panel shows some of the actions that can be performed against a target
    /// </summary>
    public partial class PanelInformationOptions : UserControl
    {
        private PanelInformationOptionsSerializableClass config;
        public DomainsItem domain;

        public PanelInformationOptions()
        {
            InitializeComponent();
        }

        public void LoadDomain(DomainsItem dom)
        {
            domain = dom;
            lblFilesSearchStatus.Text = dom.Domain;
            config = dom.informationOptions;

            ClearValues();
            LoadValuesFromDomain();
        }

        private void ClearValues()
        {
            lboLog.Items.Clear();
        }

        private void LoadValuesFromDomain()
        {
            btnTechnologyRecon.Enabled = domain.map.SearchingTechnology != HttpMap.SearchStatus.Searching;

            var status = (domain.map.SearchingAllLinks != HttpMap.SearchStatus.Searching);
            btnAllLinksGoogle.Enabled = status;
            btnAllLinksBing.Enabled = status;
            btnAllLinksDuckduckgo.Enabled = status;

            foreach (var logRegister in config.log_Log)
                lboLog.Items.Add(logRegister);
            for (var i = 0; i < checkedListBoxExtensions.Items.Count; i++)
                checkedListBoxExtensions.SetItemChecked(i, config.files_Extensions[i]);

            chkBing.Checked = config.files_Bing;
            chkGoogle.Checked = config.files_Google;
        }

        private void SaveValuesFromDomain(object sender = null, EventArgs e = null)
        {
            config.log_Log.Clear();
            foreach (string registroLog in lboLog.Items)
                config.log_Log.Add(registroLog);

            config.files_Extensions.Clear();
            for (var i = 0; i < checkedListBoxExtensions.Items.Count; i++)
            {
                config.files_Extensions.Add(checkedListBoxExtensions.GetItemChecked(i));
            }

            config.files_Bing = chkBing.Checked;
            config.files_Google = chkGoogle.Checked;
        }

        private void BtTechRecog_Click(object sender, EventArgs e)
        {
            domain.AnalyzeTechnology();
            AddLog("Technology recognition");
        }

        private void BtAllLinksGoogle_Click(object sender, EventArgs e)
        {
            domain.map.SearchingAllLinks = HttpMap.SearchStatus.Searching;

            var wsSearch = new GoogleWebSearcher
            {
                SearchAll = true,
                Site = domain.Domain
            };

            wsSearch.SearcherLinkFoundEvent += wsSearch_SearcherLinkFoundEvent;
            wsSearch.SearcherEndEvent += delegate
            {
                domain.map.SearchingAllLinks = HttpMap.SearchStatus.Finished;
                Invoke(new MethodInvoker(delegate
                {
                    Program.LogThis(new Log(Log.ModuleType.Crawling,
                        @"Finishing the links extraction of " + domain.Domain, Log.LogType.debug));
                    Program.FormMainInstance.UpdateBottomPanel(domain);
                }));
            };
            wsSearch.GetCustomLinks("site:" + domain.Domain);
            Program.LogThis(new Log(Log.ModuleType.Crawling, "Extracting links of " + domain.Domain, Log.LogType.debug));
            AddLog("Google crawling");
        }

        private void wsSearch_SearcherLinkFoundEvent(object sender, EventsThreads.CollectionFound<Uri> e)
        {
            foreach (Uri url in e.Data)
            {
                try
                {
                    try
                    {
                        var fileWithMetadata =
                            Program.FormMainInstance.panelMetadataSearch.checkedListBoxExtensions.Items.Cast<string>()
                                .Any(checkedListbox => url.ToString().EndsWith(checkedListbox));

                        if (fileWithMetadata)
                        {
                            var fi = new FilesITem
                            {
                                Ext = Path.GetExtension(url.AbsolutePath).ToLower(),
                                URL = url.ToString(),
                                Downloaded = false,
                                Processed = false,
                                Date = DateTime.MinValue,
                                ModifiedDate = DateTime.MinValue,
                                Path = string.Empty,
                                Size = -1
                            };
                            Program.data.files.Items.Add(fi);
                            Program.FormMainInstance.treeViewMetadata_UpdateDocumentsNumber();
                            var lvi = Program.FormMainInstance.panelMetadataSearch.listViewDocuments_Update(fi);
                            Program.FormMainInstance.panelMetadataSearch.HttpSizeDaemonInst.AddURL(fi);
                        }
                    }
                    catch (Exception)
                    {
                    }
                    // add the url to the files list
                    var d = Program.data.GetDomain(url.Host);
                    d.map.AddUrl(url.ToString());

                    if (d.techAnalysis.domain == null)
                        d.techAnalysis.domain = d.Domain;

                    var listaUrl = new List<Uri> {url};
                    d.techAnalysis.eventLinkFoundDetailed(null,
                        new EventsThreads.CollectionFound<Uri>(listaUrl));
                }
                catch
                {
                }
            }
        }

        private void btAllLinksBing_Click(object sender, EventArgs e)
        {
            domain.map.SearchingAllLinks = HttpMap.SearchStatus.Searching;

            var wsSearch = new BingWebSearcher
            {
                SearchAll = true,
                Site = domain.Domain
            };

            wsSearch.SearcherLinkFoundEvent += wsSearch_SearcherLinkFoundEvent;
            wsSearch.SearcherEndEvent += delegate
            {
                domain.map.SearchingAllLinks = HttpMap.SearchStatus.Finished;
                Invoke(new MethodInvoker(delegate
                {
                    Program.LogThis(new Log(Log.ModuleType.Crawling,
                        "Finishing the links extraction of " + domain.Domain, Log.LogType.debug));
                    Program.FormMainInstance.UpdateBottomPanel(domain);
                }));
            };
            wsSearch.GetCustomLinks("site:" + domain.Domain);
            Program.LogThis(new Log(Log.ModuleType.Crawling, "Extracting links of " + domain.Domain, Log.LogType.debug));
            AddLog("Bing crawling");
        }

        private void btAllLinksDuckduckGo_Click(object sender, EventArgs e)
        {
            domain.map.SearchingAllLinks = HttpMap.SearchStatus.Searching;

            var wsSearch = new DuckduckgoWebSearcher
            {
                SearchAll = true,
                Site = domain.Domain
            };

            wsSearch.SearcherLinkFoundEvent += wsSearch_SearcherLinkFoundEvent;
            wsSearch.SearcherEndEvent += delegate
            {
                domain.map.SearchingAllLinks = HttpMap.SearchStatus.Finished;
                Invoke(new MethodInvoker(delegate
                {
                    Program.LogThis(new Log(Log.ModuleType.Crawling,
                        "Finishing the links extraction of " + domain.Domain, Log.LogType.debug));
                    Program.FormMainInstance.UpdateBottomPanel(domain);
                }));
            };
            wsSearch.GetCustomLinks("site:" + domain.Domain);
            Program.LogThis(new Log(Log.ModuleType.Crawling, "Extracting links of " + domain.Domain, Log.LogType.debug));
            AddLog("DuckDuckGo crawling");
        }

        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            if (button != null &&
                (button.Text == "&Stop" && Program.FormMainInstance.panelMetadataSearch.CurrentSearch != null))
            {
                Program.FormMainInstance.panelMetadataSearch.CurrentSearch.Abort();
                Program.LogThis(new Log(Log.ModuleType.MetadataSearch, "Stopping Documents search", Log.LogType.debug));
            }
            else if (!chkGoogle.Checked && !chkBing.Checked)
            {
                MessageBox.Show(@"Select a search engine please.", @"Select a search engine", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else if (Program.FormMainInstance.panelMetadataSearch.CurrentSearch != null)
            {
                MessageBox.Show(@"It's already searching Documents!", @"Please wait", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            else
            {
                var button1 = sender as Button;
                if (button1 != null && ((Program.FormMainInstance.panelMetadataSearch.CurrentSearch != null) &&
                                        (button1.Text == "&Search")))
                {
                }
                else
                {
                    //Check if at least an extension is selected
                    if (checkedListBoxExtensions.CheckedIndices.Count == 0)
                        MessageBox.Show(@"Select a extension please.", @"Select a extension", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    else
                    {
                        Program.FormMainInstance.panelMetadataSearch.CurrentSearch = new Thread(SearchAll);
                        Program.FormMainInstance.panelMetadataSearch.CurrentSearch.IsBackground = true;
                        Program.FormMainInstance.panelMetadataSearch.CurrentSearch.Start();

                        AddLog("Search for Files (metadata)");
                    }
                }
            }
        }

        private void SearchAll()
        {
            try
            {
                var google = chkGoogle.Checked;
                var bing = chkBing.Checked;

                if (google)
                    SearchEventsGeneric(new GoogleWebSearcher());
                if (bing)
                    SearchEventsGeneric(new BingWebSearcher());
            }
            catch (ThreadAbortException)
            {
                Invoke(new MethodInvoker(delegate
                {
                    const string strMessage = "Aborted document search!";
                    Program.LogThis(new Log(Log.ModuleType.MetadataSearch, strMessage, Log.LogType.debug));
                    Program.FormMainInstance.ChangeStatus(strMessage);
                }));
            }
            finally
            {
                Program.FormMainInstance.panelMetadataSearch.CurrentSearch = null;
            }
        }

        private void SearchEventsGeneric(WebSearcher wsSearch)
        {
            try
            {
                foreach (int i in checkedListBoxExtensions.CheckedIndices)
                {
                    var strExt = checkedListBoxExtensions.Items[i] as string;
                    // remove '*' marked extensions
                    if (strExt == null) continue;
                    strExt = strExt.Replace("*", string.Empty);
                    wsSearch.AddExtension(strExt);
                }
                wsSearch.Site = domain.Domain;

                wsSearch.SearcherLinkFoundEvent += Program.FormMainInstance.panelMetadataSearch.HandleLinkFoundEvent;
                wsSearch.SearcherLogEvent += Program.FormMainInstance.panelMetadataSearch.WebSearcherLogEvent;
                wsSearch.SearcherStartEvent += HandleSearchStartEvent;
                wsSearch.SearcherEndEvent += HandleCustomSearchEndEvent;

                wsSearch.GetLinks();
                wsSearch.Join();
            }
            catch (ThreadAbortException)
            {
                wsSearch.Abort();
            }
        }

        public void HandleCustomSearchEndEvent(object sender, EventsThreads.ThreadEndEventArgs e)
        {
            Invoke(new MethodInvoker(delegate
            {
                Program.FormMainInstance.programState = FormMain.ProgramState.Normal;
                checkedListBoxExtensions.Enabled = true;
                lblAll.Enabled = true;
                lblNone.Enabled = true;
                tbnSearchFiles.Text = "&Search";
                tbnSearchFiles.Image = Resources.magnifier;
                lblFilesSearchStatus.Text = @"Finished";
                AddLog("Search of documents finished [" + domain.Domain + "]");
            }));
        }

        public void HandleSearchStartEvent(object sender, EventArgs e)
        {
            Invoke(new MethodInvoker(delegate
            {
                lblAll.Enabled = false;
                lblNone.Enabled = false;
                tbnSearchFiles.Text = "&Stop";
                tbnSearchFiles.Image = Resources.world_search_stop;
                checkedListBoxExtensions.Enabled = false;
                lblFilesSearchStatus.Text = @"Searching";
                AddLog("Starting search of documents [" + domain.Domain + "]");
            }));
        }

        private void AddLog(string log)
        {
            Invoke(new MethodInvoker(delegate
            {
                lboLog.Items.Add(DateTime.Now.ToShortTimeString() + " - " + log);
                lboLog.SelectedIndex = lboLog.Items.Count - 1;

                SaveValuesFromDomain();
            }));
        }

        private void lbAll_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < checkedListBoxExtensions.Items.Count; i++)
                checkedListBoxExtensions.SetItemChecked(i, true);
        }

        private void lbNone_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < checkedListBoxExtensions.Items.Count; i++)
                checkedListBoxExtensions.SetItemChecked(i, false);
        }
    }
}