using FOCA.Analysis.HttpMap;
using FOCA.Database.Entities;
using FOCA.Searcher;
using FOCA.Threads;
using MetadataExtractCore.Extractors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

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
        }

        private void SaveValuesFromDomain(object sender = null, EventArgs e = null)
        {
            config.log_Log.Clear();
            foreach (string registroLog in lboLog.Items)
                config.log_Log.Add(registroLog);
        }

        private void BtTechRecog_Click(object sender, EventArgs e)
        {
            domain.AnalyzeTechnology();
            AddLog("Technology recognition");
        }

        private void BtAllLinksGoogle_Click(object sender, EventArgs e)
        {
            this.SearchLinks(new GoogleWebSearcher());
        }

        private void wsSearch_SearcherLinkFoundEvent(object sender, EventsThreads.CollectionFound<Uri> e)
        {
            foreach (Uri url in e.Data)
            {
                try
                {
                    try
                    {
                        string fileExtension = Path.GetExtension(url.AbsolutePath).ToLowerInvariant();

                        if (!String.IsNullOrWhiteSpace(fileExtension) && DocumentExtractor.IsSupportedExtension(fileExtension))
                        {
                            var fi = new FilesItem
                            {
                                Ext = fileExtension,
                                URL = url.ToString(),
                                Downloaded = false,
                                MetadataExtracted = false,
                                Date = DateTime.MinValue,
                                ModifiedDate = DateTime.MinValue,
                                Path = string.Empty,
                                Size = -1
                            };
                            Program.data.files.Items.Add(fi);
                            Program.FormMainInstance.treeViewMetadata_UpdateDocumentsNumber();
                            Program.FormMainInstance.panelMetadataSearch.listViewDocuments_Update(fi);
                            Program.FormMainInstance.panelMetadataSearch.HttpSizeDaemonInst.AddURL(fi);
                        }
                    }
                    catch (Exception)
                    {
                    }
                    // add the url to the files list
                    DomainsItem domain = Program.data.GetDomain(url.Host);
                    if (domain == null)
                    {
                        Program.data.AddDomain(url.Host, "Crawling", Program.cfgCurrent.MaxRecursion, Program.cfgCurrent);
                        Program.LogThis(new Log(Log.ModuleType.Crawling, "Domain found: " + url.Host, Log.LogType.medium));
                        domain = Program.data.GetDomain(url.Host);
                    }

                    domain.map.AddUrl(url.ToString());
                    if (domain.techAnalysis.domain == null)
                    {
                        domain.techAnalysis.domain = domain.Domain;
                    }
                    domain.techAnalysis.eventLinkFoundDetailed(null, new EventsThreads.CollectionFound<Uri>(new List<Uri> { url }));
                }
                catch
                {
                }
            }
        }

        private void btAllLinksBing_Click(object sender, EventArgs e)
        {
            this.SearchLinks(new BingWebSearcher());
        }

        private void btAllLinksDuckduckGo_Click(object sender, EventArgs e)
        {
            this.SearchLinks(new DuckduckgoWebSearcher());
        }

        private void SearchLinks(LinkSearcher searcherEngine)
        {
            domain.map.SearchingAllLinks = HttpMap.SearchStatus.Searching;

            searcherEngine.ItemsFoundEvent += wsSearch_SearcherLinkFoundEvent;
            searcherEngine.SearchBySite(new System.Threading.CancellationTokenSource(), domain.Domain)
                .ContinueWith((e) =>
                {
                    domain.map.SearchingAllLinks = HttpMap.SearchStatus.Finished;
                    Invoke(new MethodInvoker(delegate
                    {
                        Program.LogThis(new Log(Log.ModuleType.Crawling, @"Finishing the links extraction of " + domain.Domain, Log.LogType.debug));
                        Program.FormMainInstance.UpdateBottomPanel(domain);
                    }));
                });
            Program.LogThis(new Log(Log.ModuleType.Crawling, "Extracting links of " + domain.Domain, Log.LogType.debug));
            AddLog($"Searching links with {searcherEngine.Name}");
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
    }
}