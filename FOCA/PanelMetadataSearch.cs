using FOCA.Database.Entities;
using FOCA.Properties;
using FOCA.Search;
using FOCA.Searcher;
using FOCA.SubdomainSearcher;
using FOCA.Threads;
using MetadataExtractCore.Analysis;
using MetadataExtractCore.Diagrams;
using MetadataExtractCore.Extractors;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static FOCA.Utilites.Functions;

namespace FOCA
{
    public partial class PanelMetadataSearch : UserControl
    {
        /// <summary>
        /// Threads used for download, extract and analyze the metadata
        /// </summary>
        private Thread Metadata, Analysis;

        private CancellationTokenSource searchCancelToken;

        private ConcurrentQueue<Download> downloadQueue;
        private ConcurrentDictionary<string, Download> downloadingFiles;
        private int downloadedFileCount;
        private CancellationTokenSource downloadTaskToken;
        private CancellationTokenSource downloadDelayToken;

        // Thread that runs in background and obtains size of files from given URLs
        public HTTPSizeDaemon HttpSizeDaemonInst;

        public PanelMetadataSearch()
        {
            InitializeComponent();
            this.downloadQueue = new ConcurrentQueue<Download>();
            this.downloadingFiles = new ConcurrentDictionary<string, Download>(StringComparer.OrdinalIgnoreCase);
            this.HttpSizeDaemonInst = new HTTPSizeDaemon();

            this.downloadTaskToken = new CancellationTokenSource();
            this.downloadDelayToken = new CancellationTokenSource();

            Task.Factory.StartNew(() => { ProcessDownloadQueue(); }, TaskCreationOptions.LongRunning);
        }

        /// <summary>
        ///     Handle event for found URLs
        /// </summary>
        /// <param name="o"></param>
        private void ProcessUrls(object o)
        {
            List<Uri> lstUrLs = o as List<Uri>;
            if (lstUrLs != null)
            {
                HandleLinkFoundEvent(null, new EventsThreads.CollectionFound<Uri>(lstUrLs));
            }
        }

        /// <summary>
        ///     Handle event for adding URLs from a file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addURLsFromFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ofdURLList.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            List<Uri> lstUrLs = new List<Uri>();
            foreach (string line in File.ReadAllLines(ofdURLList.FileName))
            {
                if (Uri.TryCreate(line, UriKind.Absolute, out Uri currentUrl))
                {
                    lstUrLs.Add(currentUrl);
                }
            }

            if (lstUrLs.Count > 0)
            {
                new Thread(ProcessUrls) { IsBackground = true }.Start(lstUrLs);
            }
        }

        /// <summary>
        ///     Search for all filetypes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbAll_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < checkedListBoxExtensions.Items.Count; i++)
                checkedListBoxExtensions.SetItemChecked(i, true);
        }

        /// <summary>
        ///     Do not select any filetype
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbNone_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < checkedListBoxExtensions.Items.Count; i++)
                checkedListBoxExtensions.SetItemChecked(i, false);
        }

        /// <summary>
        ///     Analyze metadata of documents
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void analyzeMetadataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var t = new Thread(AnalyzeMetadata)
            {
                IsBackground = true,
                Priority = ThreadPriority.BelowNormal
            };
            t.Start();
        }

        #region contextMenuStrip events

        /// <summary>
        ///     Event used for enabling/disabling context menu items when they're displayed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStripLinks_Opening(object sender, CancelEventArgs e)
        {
            bool defaultEnabledValue = Program.FormMainInstance.programState != FormMain.ProgramState.ExtractingMetadata && Program.FormMainInstance.programState != FormMain.ProgramState.Searching;

            IEnumerable<ToolStripItem> allMenuItems = this.contextMenuStripLinks.Items.OfType<ToolStripItem>();
            foreach (ToolStripItem menuItem in allMenuItems)
            {
                menuItem.Enabled = defaultEnabledValue;
            }

            if (defaultEnabledValue)
            {
                ListView lv = (ListView)((ContextMenuStrip)sender).SourceControl;
                int downloadingItemCount =
                    (from ListViewItem lvi in lv.SelectedItems from d in this.downloadQueue where d.Lvi.Index == lvi.Index select lvi).Count() +
                    (from ListViewItem lvi in lv.SelectedItems from d in this.downloadingFiles.Values where d.Lvi.Index == lvi.Index select lvi).Count();

                switch (downloadingItemCount)
                {
                    case 0:
                        this.toolStripMenuItemDownload.Text = "&Download";
                        this.toolStripMenuItemDownload.Image = Resources.page_white_go;
                        break;
                    case 1:
                        this.toolStripMenuItemDownload.Text = "&Stop Download";
                        this.toolStripMenuItemDownload.Image = Resources.page_white_go_stop;
                        break;
                    default: // > 1
                        this.toolStripMenuItemDownload.Text = "&Stop Downloads";
                        this.toolStripMenuItemDownload.Image = Resources.page_white_go_stop;
                        break;
                }

                {//Selected items
                    IEnumerable<FilesItem> selectedFiles = (from ListViewItem lvi in lv.SelectedItems where lvi.Tag != null select (FilesItem)lvi.Tag);
                    bool someFileDownloadedAndNotProcessed = selectedFiles.Any(fi => fi.Downloaded && !fi.Processed && fi.Size > 0);
                    bool someFilePendingToDownload = selectedFiles.Any(fi => !fi.Downloaded || !File.Exists(fi.Path));

                    this.toolStripMenuItemExtractMetadata.Enabled = someFileDownloadedAndNotProcessed;
                    this.toolStripMenuItemDownload.Enabled = someFilePendingToDownload;
                    this.toolStripMenuItemDelete.Enabled = selectedFiles.Any();

                    this.toolStripMenuItemLinks.Enabled = selectedFiles.Any();
                }

                {//All items
                    IEnumerable<FilesItem> allFiles = (from ListViewItem lvi in lv.Items where lvi.Tag != null select (FilesItem)lvi.Tag);
                    bool someFileDownloadedAndNotProcessed = allFiles.Any(fi => fi.Downloaded && !fi.Processed && fi.Size > 0);
                    bool someFilePendingToDownload = allFiles.Any(fi => !fi.Downloaded || !File.Exists(fi.Path));
                    bool someFileDownloadedAndProcessed = (from ListViewItem lvi in lv.Items where lvi.Tag != null select (FilesItem)lvi.Tag).Any(p => p.Downloaded && p.Processed);

                    this.toolStripMenuItemDownloadAll.Enabled = someFilePendingToDownload;
                    this.toolStripMenuItemExtractAll.Enabled = someFileDownloadedAndNotProcessed;
                    this.toolStripMenuItemDeleteAll.Enabled = allFiles.Any();
                    this.toolStripMenuItemStopAll.Visible = downloadQueue.Count > 0 || this.downloadingFiles.Count > 0;

                    //Validate if the thread for Analysis is running.
                    this.toolStripMenuItemAnalyzeAll.Enabled = someFileDownloadedAndProcessed && (Program.FormMainInstance.panelMetadataSearch.Analysis == null || !Program.FormMainInstance.panelMetadataSearch.Analysis.IsAlive);
                }
            }
        }


        private void stopAllMenuItem_Click(object sender, EventArgs e)
        {
            StopAllDownloads();
        }

        /// <summary>
        ///     Handle file download button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemDownload_Click(object sender, EventArgs e)
        {
            ToolStripDropDownItem toolStripDropDownItem = sender as ToolStripDropDownItem;
            if (toolStripDropDownItem != null)
            {
                if (toolStripDropDownItem.Text == "&Download")
                {
                    string directory = Program.data.Project.FolderToDownload;
                    if (!directory.EndsWith("\\"))
                        directory += "\\";
                    List<ListViewItem> urls = Program.FormMainInstance.panelMetadataSearch.listViewDocuments.SelectedItems.Cast<ListViewItem>().ToList();
                    this.EnqueueFilestoDownload(urls, directory);
                }
                else if (toolStripDropDownItem.Text.StartsWith("&Stop Download"))
                {
                    // Stop all selected items
                    List<string> selectedUrls = Program.FormMainInstance.panelMetadataSearch.listViewDocuments.SelectedItems.OfType<ListViewItem>().Select(p => p.Tag as FilesItem).Where(p => p != null).Select(p => p.URL).ToList();

                    List<Download> filesToCancel = new List<Download>();
                    filesToCancel.AddRange(this.downloadingFiles.Where(p => selectedUrls.Contains(p.Key, StringComparer.OrdinalIgnoreCase)).Select(p => p.Value));
                    filesToCancel.AddRange(this.downloadQueue.Where(p => selectedUrls.Contains(p.DownloadUrl, StringComparer.OrdinalIgnoreCase)));

                    foreach (Download item in filesToCancel)
                    {
                        this.StopFileDownload(item);
                    }
                }
            }
        }

        /// <summary>
        ///     Handle file download all files button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void toolStripMenuItemDownloadAll_Click(object sender, EventArgs e)
        {
            string directory = Program.data.Project.FolderToDownload;
            if (!directory.EndsWith("\\"))
                directory += "\\";
            List<ListViewItem> urls = Program.FormMainInstance.panelMetadataSearch.listViewDocuments.Items.Cast<ListViewItem>().ToList();
            this.EnqueueFilestoDownload(urls, directory);
        }

        /// <summary>
        ///     Handle file deletion from documents list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void toolStripMenuItemDelete_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in Program.FormMainInstance.panelMetadataSearch.listViewDocuments.SelectedItems)
            {
                var fi = (FilesItem)lvi.Tag;
                if (fi != null)
                {
                    Program.data.files.Items.Remove(fi);
                    RemoveFileFromTreeNode(fi);
                    Program.LogThis(new Log(Log.ModuleType.MetadataSearch,
                        $"Removed document: {fi.URL}", Log.LogType.debug));
                }
                lvi.Remove();
            }
            Program.FormMainInstance.treeViewMetadata_UpdateDocumentsNumber();
        }

        /// <summary>
        ///     Handle all files deletion from documents list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemDeleteAll_Click(object sender, EventArgs e)
        {
            foreach (
                var fi in from ListViewItem lvi in Program.FormMainInstance.panelMetadataSearch.listViewDocuments.Items
                          select (FilesItem)lvi.Tag
                    into fi
                          where fi != null
                          select fi)
            {
                RemoveFileFromTreeNode(fi);
                Program.LogThis(new Log(Log.ModuleType.MetadataSearch,
                    $"Removed document: {fi.URL}", Log.LogType.debug));
            }
            Program.data.files.Items.Clear();
            Program.FormMainInstance.panelMetadataSearch.listViewDocuments.Items.Clear();
            Program.FormMainInstance.treeViewMetadata_UpdateDocumentsNumber();
        }

        /// <summary>
        ///     Remove file from files tree
        /// </summary>
        /// <param name="fi"></param>
        private void RemoveFileFromTreeNode(FilesItem fi)
        {
            if (fi != null && fi.Processed)
            {
                Program.FormMainInstance.TreeViewMetadataSearchDocument(fi.Path)?.Remove();
            }
        }


        /// <summary>
        ///     Handle extrect metadata from all files button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void toolStripMenuItemExtractMetadata_Click(object sender, EventArgs e)
        {
            LaunchMetadataExtractorThread(Program.FormMainInstance.panelMetadataSearch.listViewDocuments.SelectedItems.Cast<ListViewItem>().ToList());
        }

        private void extractAllMetadataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LaunchMetadataExtractorThread(Program.FormMainInstance.panelMetadataSearch.listViewDocuments.Items.Cast<ListViewItem>().ToList());
        }

        /// <summary>
        ///     Handle add local file to documents tree action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemAddFile_Click(object sender, EventArgs e)
        {
            if (ofdAddFile.ShowDialog() == DialogResult.OK)
                AddFile(ofdAddFile.FileName);
        }

        /// <summary>
        ///     Handle add all files from a local folder to documents tree action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemAddFolder_Click(object sender, EventArgs e)
        {
            if (fbdMain.ShowDialog() != DialogResult.OK) return;
            var files = Directory.GetFiles(fbdMain.SelectedPath);
            var lvcs = (ListViewColumnSorter)listViewDocuments.ListViewItemSorter;
            listViewDocuments.ListViewItemSorter = null;
            foreach (var file in files)
                AddFile(file);
            listViewDocuments.ListViewItemSorter = lvcs;
        }

        /// <summary>
        ///     Event called to show selected URLs in browser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemOpenInBrowser_Click(object sender, EventArgs e)
        {
            foreach (
                var lvi in
                    listViewDocuments.SelectedItems.Cast<ListViewItem>()
                        .Where(lvi => Uri.IsWellFormedUriString(lvi.SubItems[2].Text, UriKind.Absolute)))
            {
                Process.Start(lvi.SubItems[2].Text);
                Program.LogThis(new Log(Log.ModuleType.MetadataSearch,
                    $"Opening document {lvi.SubItems[2].Text}", Log.LogType.debug));
            }
        }

        /// <summary>
        ///     Event called to copy selected items into clipboard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripMenuItemCopyToClipboard_Click(object sender, EventArgs e)
        {
            var links = string.Empty;
            for (var i = 0; i < listViewDocuments.SelectedItems.Count; i++)
            {
                //Add newlines
                links += i != 0 ? Environment.NewLine : string.Empty;
                links += listViewDocuments.SelectedItems[i].SubItems[2].Text;
            }
            Clipboard.SetText(links);
            Program.LogThis(new Log(Log.ModuleType.MetadataSearch,
                $"Copied to clipboard {listViewDocuments.SelectedItems.Count} URL documents",
                Log.LogType.debug));
        }

        /// <summary>
        ///     Add file from path to the project
        ///     A file locally added is tagged with the "Downloaded" attribute as true
        /// </summary>
        /// <param name="path"></param>
        public void AddFile(string path)
        {
            try
            {
                string extension = System.IO.Path.GetExtension(path)?.ToLower();
                if (extension == null)
                    return;
                // verify that it's a well-known extension, if not, guess filetype
                if (!DocumentExtractor.IsSupportedExtension(extension))
                {
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        extension = MetadataExtractCore.Utilities.Functions.GetFormatFile(fs);
                    }
                    if (!String.IsNullOrWhiteSpace(extension))
                    {
                        Program.LogThis(new Log(Log.ModuleType.MetadataSearch, $"Unknown extension {path}, found filetype {extension}", Log.LogType.medium));
                        string newFile = GetNotExistsPath(path + extension);
                        File.Move(path, newFile);
                        Program.LogThis(new Log(Log.ModuleType.MetadataSearch, $"File moved {path} to {newFile}", Log.LogType.medium));
                        path = newFile;
                    }
                }
                FilesItem fi = new FilesItem
                {
                    Ext = extension,
                    URL = path,
                    Date = DateTime.Now,
                    Size = (int)new FileInfo(path).Length,
                    Downloaded = true,
                    Processed = false,
                    Path = path
                };
                Program.data.files.Items.Add(fi);
                listViewDocuments_Update(fi);
                Program.FormMainInstance.treeViewMetadata_UpdateDocumentsNumber();

                Program.LogThis(new Log(Log.ModuleType.MetadataSearch, $"Added document {fi.Path}", Log.LogType.debug));
            }
            catch (Exception e)
            {
                Program.LogThis(new Log(Log.ModuleType.MetadataSearch, $"The file {path} could not be added: {e.ToString()}", Log.LogType.debug));
            }
        }

        #endregion

        #region listViewDocuments events

        /// <summary>
        ///     Represents a document or link into the ListViewDocuments and creates a new item for it
        /// </summary>
        /// <param name="fi"></param>
        public ListViewItem listViewDocuments_Update(FilesItem fi)
        {
            // search for the corresponding listViewDocument
            ListViewItem lviCurrent = null;
            try
            {
                listViewDocuments.Invoke(new MethodInvoker(() =>
                {
                    ListViewItem.ListViewSubItem lviExtension;
                    ListViewItem.ListViewSubItem lviUrl;
                    ListViewItem.ListViewSubItem lviDownloaded;
                    ListViewItem.ListViewSubItem lviDownloadedDate;
                    ListViewItem.ListViewSubItem lviSize;
                    ListViewItem.ListViewSubItem lviAnalyzed;
                    ListViewItem.ListViewSubItem lviModifedDate;

                    lviCurrent = listViewDocuments.Items.Cast<ListViewItem>().Where(lvi => (FilesItem)lvi.Tag == fi).FirstOrDefault();

                    // new item
                    if (lviCurrent == null)
                    {
                        lviCurrent = listViewDocuments.Items.Add(listViewDocuments.Items.Count.ToString());
                        lviCurrent.Tag = fi;
                        lviCurrent.UseItemStyleForSubItems = false;
                        lviCurrent.ImageIndex = Program.FormMainInstance.GetImageToExtension(fi.Ext);
                        lviExtension = lviCurrent.SubItems.Add(string.Empty);
                        lviUrl = lviCurrent.SubItems.Add(string.Empty);
                        lviDownloaded = lviCurrent.SubItems.Add(string.Empty);
                        lviDownloadedDate = lviCurrent.SubItems.Add(string.Empty);
                        lviSize = lviCurrent.SubItems.Add(string.Empty);
                        lviAnalyzed = lviCurrent.SubItems.Add(string.Empty);
                        lviModifedDate = lviCurrent.SubItems.Add(string.Empty);
                    }
                    // already existing item
                    else
                    {
                        lviExtension = lviCurrent.SubItems[1];
                        lviUrl = lviCurrent.SubItems[2];
                        lviDownloaded = lviCurrent.SubItems[3];
                        lviDownloadedDate = lviCurrent.SubItems[4];
                        lviSize = lviCurrent.SubItems[5];
                        lviAnalyzed = lviCurrent.SubItems[6];
                        lviModifedDate = lviCurrent.SubItems[7];
                        //ImageIndex of unknown file
                        if (lviCurrent.ImageIndex == 22)
                        {
                            lviCurrent.ImageIndex = Program.FormMainInstance.GetImageToExtension(fi.Ext);
                        }
                    }
                    // represent the item into the ListView
                    lviExtension.Text = fi.Ext.TrimStart('.');
                    lviUrl.Text = fi.URL;

                    lviDownloaded.Font = new Font(Font.FontFamily, 14);
                    lviDownloaded.Text = fi.Downloaded ? "•" : "×";
                    lviDownloaded.ForeColor = fi.Downloaded ? Color.Green : Color.Red;
                    lviDownloadedDate.Text = fi.Date == DateTime.MinValue ? "-" : fi.Date.ToString(CultureInfo.InvariantCulture);

                    lviSize.Text = fi.Size > -1 ? GetFileSizeAsString(fi.Size) : "-";

                    lviAnalyzed.Font = new Font(Font.FontFamily, 14);
                    lviAnalyzed.Text = fi.Processed ? "•" : "×";
                    lviAnalyzed.ForeColor = fi.Processed ? Color.Green : Color.Red;
                    lviModifedDate.Text = fi.ModifiedDate == DateTime.MinValue ? "-" : fi.ModifiedDate.ToString(CultureInfo.InvariantCulture);
                }));
            }
            catch (Exception ex)
            {
                Program.LogThis(new Log(Log.ModuleType.MetadataSearch,
                    $"Error filling ListViewItemDocuments: {ex.Message}", Log.LogType.error));
            }
            return lviCurrent;
        }

        /// <summary>
        ///     Handle documents list view order change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewDocuments_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var lvwColumnSorter = (ListViewColumnSorter)listViewDocuments.Tag;
            if (e.Column == lvwColumnSorter.SortColumn)
            {
                lvwColumnSorter.Order = lvwColumnSorter.Order == SortOrder.Ascending
                    ? SortOrder.Descending
                    : SortOrder.Ascending;
            }
            else
            {
                lvwColumnSorter.SortColumn = e.Column;
                lvwColumnSorter.Order = SortOrder.Ascending;
            }
            listViewDocuments.Sort();
        }

        /// <summary>
        ///     Hadle key down action when placed in the documents list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewDocuments_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 46)
                toolStripMenuItemDelete_Click(null, null);
        }

        #endregion

        #region Form components events

        /// <summary>
        ///     Handle custom search click. Make text visible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelCustomSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panelCustomSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Visible = true;
            linkLabelCustomSearch.Visible = false;
            btnSearch.Visible = true;
        }

        /// <summary>
        ///     Handle any key press action when placed in the documents list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter) && btnSearch.Enabled)
                btnSearch_Click(btnSearch, null);
        }

        /// <summary>
        ///     Handle search button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            string customSearchValue = txtSearch.Text;
            if (button != null && button.Text == "&Stop")
            {
                this.searchCancelToken?.Cancel();
            }
            else if (String.IsNullOrWhiteSpace(customSearchValue))
            {
                MessageBox.Show(@"Please, insert a term for searching", @"Insert a term", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (!chkGoogle.Checked && !chkBing.Checked && !chkDuck.Checked)
            {
                MessageBox.Show(@"Select a search engine, please.", @"Select a search engine", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (this.searchCancelToken != null)
            {
                MessageBox.Show(@"Already searching, please wait", @"Please, wait", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Invoke(new MethodInvoker(() =>
                {
                    btnSearch.Text = "&Stop";
                    btnSearch.Image = Resources.magnifier_stop;
                    checkedListBoxExtensions.Enabled = panelSearchConfiguration.Enabled = false;
                }));

                Func<LinkSearcher, Task<int>> searchFunc = (s) => s.CustomSearch(this.searchCancelToken, customSearchValue);
                this.Search(CreateSelectedSearchers(), searchFunc);
            }
        }

        /// <summary>
        ///     Handle search using all available engines button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchAll_Click(object sender, EventArgs e)
        {
            try
            {
                var button = sender as Button;
                if (button != null && button.Text == "&Stop")
                {
                    this.searchCancelToken?.Cancel();
                    Program.LogThis(new Log(Log.ModuleType.MetadataSearch, "Stopping documents search", Log.LogType.debug));
                }
                else if (!chkGoogle.Checked && !chkBing.Checked && !chkDuck.Checked)
                {
                    MessageBox.Show(@"Select a search engine please.", @"Select a search engine", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (this.searchCancelToken != null)
                {
                    MessageBox.Show(@"Already searching, please wait", @"Please, wait", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (checkedListBoxExtensions.CheckedIndices.Count == 0)
                        MessageBox.Show(@"Select at least one extension please.", @"Select an extension", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        List<string> selectedExtensions = new List<string>();
                        foreach (int i in checkedListBoxExtensions.CheckedIndices)
                        {
                            var strExt = (string)checkedListBoxExtensions.Items[i];
                            // some extensions are marked as '*', delete them
                            strExt = strExt.Replace("*", String.Empty);
                            selectedExtensions.Add(strExt);
                        }

                        if (selectedExtensions.Count > 0)
                        {
                            Invoke(new MethodInvoker(() =>
                            {
                                btnSearchAll.Text = "&Stop";
                                btnSearchAll.Image = Resources.world_search_stop;
                                checkedListBoxExtensions.Enabled = panelSearchConfiguration.Enabled = false;
                            }));

                            Func<LinkSearcher, Task<int>> searchFunc = (s) => s.SearchBySite(this.searchCancelToken, Program.data.Project.Domain, selectedExtensions.ToArray());
                            this.Search(CreateSelectedSearchers(), searchFunc);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region Metadata functions

        private void LaunchMetadataExtractorThread(List<ListViewItem> items)
        {
            if (Metadata != null && Metadata.IsAlive)
            {
                MessageBox.Show("Already extracting metadata", "Please wait", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                List<FilesItem> files = items.Select(p => p.Tag as FilesItem)
                                            .Where(p => p != null && p.Downloaded && p.Size > 0 && !p.Processed)
                                            .ToList();
                Metadata = new Thread(ExtractMetadata)
                {
                    // avoid CPU overload
                    Priority = ThreadPriority.Lowest,
                    IsBackground = true
                };
                Metadata.Start(files);
            }
        }

        /// <summary>
        ///     Extract metadasta from file
        /// </summary>
        /// <param name="o">List which contains the documents from which the tool is going to extract metadata information</param>
        private void ExtractMetadata(object filesItemList)
        {
            TreeNode itemsTree = Program.FormMainInstance.TreeView.GetNode(GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.ToNavigationPath());

            if (Program.data.Project.Id == 0)
            {
                itemsTree.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.Users.Key].Tag = new ConcurrentBag<UserItem>();
                itemsTree.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.Printers.Key].Tag = new ConcurrentBag<PrintersItem>();
                itemsTree.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.Folders.Key].Tag = new ConcurrentBag<PathsItem>();
                itemsTree.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.Software.Key].Tag = new ConcurrentBag<ApplicationsItem>();
                itemsTree.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.Emails.Key].Tag = new ConcurrentBag<EmailsItem>();
                itemsTree.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.OperatingSystems.Key].Tag = new ConcurrentBag<string>();
                itemsTree.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.Passwords.Key].Tag = new ConcurrentBag<PasswordsItem>();
                itemsTree.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.Servers.Key].Tag = new ConcurrentBag<ServersItem>();
            }

            var users = (ConcurrentBag<UserItem>)itemsTree.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.Users.Key].Tag;
            var printers = (ConcurrentBag<PrintersItem>)itemsTree.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.Printers.Key].Tag;
            var folders = (ConcurrentBag<PathsItem>)itemsTree.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.Folders.Key].Tag;
            var software = (ConcurrentBag<ApplicationsItem>)itemsTree.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.Software.Key].Tag;
            var emails = (ConcurrentBag<EmailsItem>)itemsTree.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.Emails.Key].Tag;
            var operatingsystems = (ConcurrentBag<string>)itemsTree.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.OperatingSystems.Key].Tag;
            var passwords = (ConcurrentBag<PasswordsItem>)itemsTree.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.Passwords.Key].Tag;
            var servers = (ConcurrentBag<ServersItem>)itemsTree.Nodes[GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.Servers.Key].Tag;

            try
            {
                List<FilesItem> files = filesItemList as List<FilesItem>;
                if (files == null)
                {
                    return;
                }
                else
                {
                    List<FilesItem> filesToExtract = files.Where(p => p != null && p.Downloaded && p.Size > 0 && !p.Processed).ToList();
                    Program.LogThis(new Log(Log.ModuleType.MetadataSearch,
                        $"Starting metadata extraction of {filesToExtract.Count} documents", Log.LogType.debug));
                    if (filesToExtract.Count > 0)
                    {
                        Invoke(new MethodInvoker(() =>
                        {
                            // Disable interface elements while metadata search is running
                            btnSearch.Enabled = false;
                            btnSearchAll.Enabled = false;
                            Program.FormMainInstance.programState = FormMain.ProgramState.ExtractingMetadata;
                            Program.FormMainInstance.SetItemsMenu(null, null);
                            Program.FormMainInstance.toolStripDropDownButtonStop.Enabled = true;
                        }));
                        int extractedFileCount = 0; // counter

                        int chunkSize = Environment.ProcessorCount;
                        List<List<FilesItem>> chunkedFiles = new List<List<FilesItem>>();
                        for (int i = 0; i < filesToExtract.Count; i += chunkSize)
                            chunkedFiles.Add(filesToExtract.GetRange(i, Math.Min(chunkSize, filesToExtract.Count - i)));

                        ParallelOptions po = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount };
                        foreach (List<FilesItem> fileChunk in chunkedFiles)
                        {
                            Invoke(new MethodInvoker(delegate
                            {
                                Program.FormMainInstance.toolStripProgressBarDownload.Value = extractedFileCount * 100 / filesToExtract.Count;
                                Program.FormMainInstance.toolStripStatusLabelLeft.Text = $"Extracting metadata from {extractedFileCount} / {filesToExtract.Count} documents";
                                Program.FormMainInstance.ReportProgress(extractedFileCount, filesToExtract.Count);
                            }));

                            Parallel.ForEach(fileChunk, po, currentFile =>
                            {
                                currentFile.Processed = true;
                                FileMetadata foundMetadata = null;
                                if (!String.IsNullOrWhiteSpace(currentFile.Ext))
                                {
                                    try
                                    {
                                        using (var fs = new FileStream(currentFile.Path, FileMode.Open, FileAccess.Read))
                                        {
                                            using (DocumentExtractor doc = DocumentExtractor.Create(currentFile.Ext, fs))
                                            {
                                                // Analyze file and extract metadata
                                                foundMetadata = doc.AnalyzeFile();
                                                Program.LogThis(new Log(Log.ModuleType.MetadataSearch, $"Document metadata extracted: {currentFile.Path}", Log.LogType.low));
                                                if (foundMetadata.HasMetadata())
                                                {
                                                    // fi.Metadata contains the file's metadata so that other parts can also use them
                                                    currentFile.Metadata = new MetaExtractor(foundMetadata);

                                                    // Extract primary metadata from the document
                                                    ExtractGenericMetadata(currentFile.Metadata, users, passwords, servers,
                                                            folders, printers, emails, software, operatingsystems);
                                                    // if any date has been found, use it for the 'Last modified' field into the ListView
                                                    if (currentFile.Metadata.FoundDates.ModificationDateSpecified)
                                                        currentFile.ModifiedDate = currentFile.Metadata.FoundDates.ModificationDate;
                                                    else if (currentFile.Metadata.FoundDates.CreationDateSpecified)
                                                        currentFile.ModifiedDate = currentFile.Metadata.FoundDates.CreationDate;
                                                    // if there're no older versions, just the existing ones in OpenOffice, extract the metadata summary from them
                                                    if (foundMetadata.OldVersions.Count > 0)
                                                    {
                                                        foreach (var oldVersion in foundMetadata.OldVersions)
                                                        {
                                                            // Add every version's information to the summary
                                                            this.ExtractGenericMetadata(new MetaExtractor(oldVersion.Metadata), users, passwords, servers, folders, printers, emails, software, operatingsystems);
                                                        }
                                                    }
                                                }


                                            }
                                        }

                                    }
                                    catch
                                    {
                                    }
                                }

                                Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(delegate
                                {
                                    TreeNode tnFile = Program.FormMainInstance.TreeViewMetadataAddDocument(currentFile);
                                    tnFile.Tag = currentFile;
                                    tnFile.Nodes.Clear();
                                    tnFile.ImageIndex = tnFile.SelectedImageIndex = Program.FormMainInstance.GetImageToExtension(currentFile.Ext);
                                    if (currentFile.Metadata != null)
                                    {
                                        this.AddDocumentNodes(currentFile.Metadata, tnFile, foundMetadata);
                                    }
                                }));

                                Interlocked.Increment(ref extractedFileCount);
                            });

                            Invoke(new MethodInvoker(delegate
                            {
                                foreach (FilesItem item in fileChunk)
                                {
                                    listViewDocuments_Update(item);
                                }
                            }));
                        }

                        Invoke(new MethodInvoker(delegate
                        {
                            const string strMessage = "All documents were analyzed";
                            Program.LogThis(new Log(Log.ModuleType.MetadataSearch, strMessage, Log.LogType.debug));
                            Program.FormMainInstance.ChangeStatus(strMessage);

                            Program.FormMainInstance.TreeView.GetNode(GUI.Navigation.Project.DocumentAnalysis.ToNavigationPath()).Expand();
                            Program.FormMainInstance.TreeView.GetNode(GUI.Navigation.Project.DocumentAnalysis.MetadataSummary.ToNavigationPath()).Expand();
                        }));
                    }
                    else
                    {
                        Invoke(new MethodInvoker(delegate
                        {
                            const string strMessage = "No documents to analyze metadata";
                            Program.LogThis(new Log(Log.ModuleType.MetadataSearch, strMessage, Log.LogType.debug));
                            Program.FormMainInstance.ChangeStatus(strMessage);
                        }));
                    }
                }

            }
            catch (ThreadAbortException)
            {
                Invoke(new MethodInvoker(delegate
                {
                    const string strMessage = "Metadata extraction was aborted!";
                    Program.LogThis(new Log(Log.ModuleType.MetadataSearch, strMessage, Log.LogType.debug));
                    Program.FormMainInstance.ChangeStatus(strMessage);
                }));
            }
            finally
            {
                Invoke(new MethodInvoker(delegate
                {
                    Program.FormMainInstance.ReportProgress(0, 100);
                    Program.FormMainInstance.toolStripProgressBarDownload.Value = 0;
                    Program.FormMainInstance.toolStripDropDownButtonStop.Enabled = false;

                    if (emails.Count != 0)
                    {
                        List<string> emailsValue = emails.Select(x => x.Mail).ToList();
                        PluginsAPI.SharedValues.FocaEmails = emailsValue;
                    }
                    else
                        PluginsAPI.SharedValues.FocaEmails = new List<string>();

                    // enable interface elements which were disabled previously
                    btnSearchAll.Enabled = Program.data.Project.ProjectState != Project.ProjectStates.Uninitialized;
                    btnSearch.Enabled = true;
                    Program.FormMainInstance.programState = FormMain.ProgramState.Normal;
                    Metadata = null;
                }));
            }
        }

        /// <summary>
        ///     Obtain primary metadata from a document to elaborate a summary
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="users"></param>
        /// <param name="servers"></param>
        /// <param name="folders"></param>
        /// <param name="printers"></param>
        /// <param name="emails"></param>
        /// <param name="passwords"></param>
        /// <param name="software"></param>
        /// <param name="operatingsystems"></param>
        internal void ExtractGenericMetadata(MetaExtractor doc, ConcurrentBag<UserItem> users,
            ConcurrentBag<PasswordsItem> passwords, ConcurrentBag<ServersItem> servers,
            ConcurrentBag<PathsItem> folders, ConcurrentBag<PrintersItem> printers,
            ConcurrentBag<EmailsItem> emails, ConcurrentBag<ApplicationsItem> software,
            ConcurrentBag<string> operatingsystems)
        {
            Servers s = doc.FoundServers;
            foreach (var si in s.Items.Where(si => si.Name.Trim().Length > 1))
                if (servers.Count(x => x.Name == si.Name.Trim()) == 0)
                    servers.Add(si);
            //else
            //    servers[si.Name.Trim()]++;

            Users u = doc.FoundUsers;
            foreach (var ui in u.Items.Where(ui => ui.Name.Trim().Length > 1))
                if (users.Count(x => x.Name == ui.Name.Trim()) == 0)
                    users.Add(ui);

            Passwords p = doc.FoundPasswords;
            foreach (var pi in p.Items.Where(pi => pi.Password.Trim().Length > 1))
                if (passwords.Count(x => x.Password == pi.Password.Trim()) == 0)
                    passwords.Add(pi);
            //else
            //    passwords[pi.Password.Trim()]++;

            var r = doc.FoundPaths;
            foreach (var ri in r.Items.Where(ri => ri.Path.Trim().Length > 1))
                if (folders.Count(x => x.Path == ri.Path.Trim()) == 0)
                    folders.Add(ri);
            //else
            //    folders[ri.Path.Trim()]++;

            Printers im = doc.FoundPrinters;
            foreach (var ii in im.Items.Where(ii => ii.Printer.Trim().Length > 1))
                if (printers.Count(x => x.Printer == ii.Printer.Trim()) == 0)
                    printers.Add(ii);
            //else
            //    printers[ii.Printer.Trim()]++;

            Emails e = doc.FoundEmails;
            foreach (var ei in e.Items.Where(ei => ei.Mail.Trim().Length > 1))
                if (emails.Count(x => x.Mail == ei.Mail.Trim()) == 0)
                    emails.Add(ei);
            //else
            //    emails[ei.Mail.Trim()]++;

            foreach (string strSoftware in
                    doc.FoundMetaData.Applications.Items.Select(aplicacion => aplicacion.Name)
                        .Where(strSoftware => strSoftware.Trim().Length > 1))
            {
                if (software.Count(x => x.Name == strSoftware.Trim()) == 0)
                    software.Add(new ApplicationsItem(strSoftware));
            }

            string strOs = doc.FoundMetaData.OperativeSystem;
            if (string.IsNullOrEmpty(strOs)) return;
            strOs = strOs.Trim();
            if (operatingsystems.Count(i => i == strOs) == 0)
                operatingsystems.Add(strOs);
        }

        /// <summary>
        ///     Add needed nodes to treeViewMetadata to show document's metadata
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="trnParentNode">Parent node to which information will be added</param>
        internal void AddDocumentNodes(MetaExtractor doc, TreeNode trnParentNode, FileMetadata originalMetadata = null)
        {
            List<Action> methodsToInvoke = new List<Action>();
            if (doc.FoundUsers.Items.Count != 0)
            {
                methodsToInvoke.Add(new Action(() =>
                {
                    var tnUsers = trnParentNode.Nodes.Add("Users", "Users", 14, 14);
                    tnUsers.Tag = doc.FoundUsers;
                }));
            }
            if (doc.FoundPasswords.Items.Count != 0)
            {
                methodsToInvoke.Add(new Action(() =>
                {
                    var tnPasswords = trnParentNode.Nodes.Add("Passwords", "Passwords", 75, 75);
                    tnPasswords.Tag = doc.FoundPasswords;
                }));
            }
            if (doc.FoundServers.Items.Count != 0)
            {
                methodsToInvoke.Add(new Action(() =>
                {
                    var tnServers = trnParentNode.Nodes.Add("Servers", "Servers", 45, 45);
                    tnServers.Tag = doc.FoundServers;
                }));
            }
            if (doc.FoundPaths.Items.Count != 0)
            {
                methodsToInvoke.Add(new Action(() =>
                {
                    var tnFolders = trnParentNode.Nodes.Add("Folders", "Folders", 13, 13);
                    tnFolders.Tag = doc.FoundPaths;
                }));
            }
            if (doc.FoundPrinters.Items.Count != 0)
            {
                methodsToInvoke.Add(new Action(() =>
                {
                    var tnPrinters = trnParentNode.Nodes.Add("Printers", "Printers", 15, 15);
                    tnPrinters.Tag = doc.FoundPrinters;
                }));
            }
            if (doc.FoundEmails.Items.Count != 0)
            {
                methodsToInvoke.Add(new Action(() =>
                {
                    var tnEmails = trnParentNode.Nodes.Add("Emails", "Emails", 23, 23);
                    tnEmails.Tag = doc.FoundEmails;
                }));
            }
            if (doc.FoundDates.CreationDateSpecified || doc.FoundDates.DatePrintingSpecified ||
                doc.FoundDates.ModificationDateSpecified)
            {
                methodsToInvoke.Add(new Action(() =>
                {
                    var tnDates = trnParentNode.Nodes.Add("Dates", "Dates", 24, 24);
                    tnDates.Tag = doc.FoundDates;
                }));
            }

            if (doc.FoundMetaData != null && doc.FoundMetaData.Applications.Items.Count > 0)
            {
                methodsToInvoke.Add(new Action(() =>
                {
                    var tnSoftware = trnParentNode.Nodes.Add("Software", "Software", 30, 30);
                    tnSoftware.Tag = doc.FoundMetaData.Applications;
                }));
            }

            var m = doc.FoundMetaData;

            if (!string.IsNullOrEmpty(m.Subject) ||
                !string.IsNullOrEmpty(m.DataBase) ||
                !string.IsNullOrEmpty(m.Category) ||
                !string.IsNullOrEmpty(m.Codification) ||
                !string.IsNullOrEmpty(m.Comments) ||
                !string.IsNullOrEmpty(m.Company) ||
                !string.IsNullOrEmpty(m.Description) ||
                !string.IsNullOrEmpty(m.Statistic) ||
                !string.IsNullOrEmpty(m.Language) ||
                !string.IsNullOrEmpty(m.UserInfo) ||
                m.VersionNumber != 0 ||
                !string.IsNullOrEmpty(m.Keywords) ||
                !string.IsNullOrEmpty(m.Template) ||
                !string.IsNullOrEmpty(m.OperativeSystem) ||
                m.EditTime != 0 ||
                !string.IsNullOrEmpty(m.Title) ||
                !string.IsNullOrEmpty(m.Model))
            {
                methodsToInvoke.Add(new Action(() =>
                {
                    var tnMetadata = trnParentNode.Nodes.Add("Other Metadata", "Other Metadata", 25, 25);
                    tnMetadata.Tag = doc.FoundMetaData;
                }));
            }
            if (doc.FoundOldVersions.Items.Count != 0)
            {
                methodsToInvoke.Add(new Action(() =>
                {
                    var tnOld = trnParentNode.Nodes.Add("Old versions", "Old versions", 26, 26);
                    tnOld.Tag = doc.FoundOldVersions;
                }));
            }
            if (doc.FoundHistory.Items.Count != 0)
            {
                methodsToInvoke.Add(new Action(() =>
                {
                    var tnOld = trnParentNode.Nodes.Add("History", "History", 31, 31);
                    tnOld.Tag = doc.FoundHistory;
                }));
            }

            if (originalMetadata != null)
            {
                if (originalMetadata.Makernotes.Count > 0 || originalMetadata.Thumbnail != null)
                {
                    methodsToInvoke.Add(new Action(() =>
                    {
                        var tnExif = trnParentNode.Nodes.Add("EXIF", "EXIF", 33, 33);
                        tnExif.Tag = originalMetadata;
                    }));
                }

                if (originalMetadata.EmbeddedImages.Count > 0)
                {
                    methodsToInvoke.Add(new Action(delegate
                    {
                        IEnumerable<KeyValuePair<string, FileMetadata>> filesWithExif = originalMetadata.EmbeddedImages.Where(p => p.Value.Makernotes.Count > 0 || p.Value.Thumbnail != null);
                        if (filesWithExif.Any())
                        {
                            TreeNode tnExifRoot = trnParentNode.Nodes.Add("EXIF in pictures", "EXIF in pictures", 32, 32);
                            foreach (KeyValuePair<string, FileMetadata> dicPicture in filesWithExif)
                            {
                                TreeNode tnExifPic = tnExifRoot.Nodes.Add(System.IO.Path.GetFileName(dicPicture.Key), System.IO.Path.GetFileName(dicPicture.Key), 34, 34);
                                TreeNode tnExif = tnExifPic.Nodes.Add("EXIF", "EXIF", 33, 33);
                                tnExif.Tag = dicPicture.Value;
                            }
                        }
                    }));

                }

                if (originalMetadata.OldVersions.Count > 0)
                {
                    methodsToInvoke.Add(new Action(() =>
                    {
                        var tnOldVersionsRoot = trnParentNode.Nodes["Old versions"];
                        foreach (var oldVersion in originalMetadata.OldVersions)
                        {
                            var tnOldVersion = tnOldVersionsRoot.Nodes.Add(oldVersion.Value, oldVersion.Value);
                            tnOldVersion.ImageIndex =
                                tnOldVersion.SelectedImageIndex = tnOldVersion.Parent.Parent.SelectedImageIndex;
                            AddDocumentNodes(new MetaExtractor(oldVersion.Metadata), tnOldVersion, oldVersion.Metadata);
                        }
                    }));
                }

                if (originalMetadata.GPS != null)
                {
                    methodsToInvoke.Add(new Action(() =>
                    {
                        TreeNode gps = trnParentNode.Nodes.Add("GPS", "GPS", 122, 122);
                        gps.Tag = originalMetadata;
                    }));
                }
            }

            Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(() =>
            {
                foreach (var item in methodsToInvoke)
                {
                    item.Invoke();
                }
            }));
        }

        #endregion

        #region Analyze Metadata Functions

        /// <summary>
        ///     Analyze metadata of the current project files
        /// </summary>
        public void AnalyzeMetadata()
        {
            if (Program.FormMainInstance.TreeView.GetNode(GUI.Navigation.Project.Network.ToNavigationPath()) == null)
            {
                MessageBox.Show(@"You need a project before analyzing metadata", "", MessageBoxButtons.OK);
                return;
            }

            try
            {
                Invoke(new MethodInvoker(() =>
                {
                    Program.FormMainInstance.toolStripStatusLabelLeft.Text = @"Identifying computers...";
                    Program.FormMainInstance.toolStripDropDownButtonStop.Enabled = true;
                }));

                GetPCsAndServers();
                if (Program.data.computers.Items.Any(c => c.type == ComputersItem.Tipo.ClientPC))
                {
                    // group by username match
                    GroupClientNodes();
                }
                IdentifyOsSoftware();
                Invoke(
                    new MethodInvoker(
                        () => Program.FormMainInstance.toolStripStatusLabelLeft.Text = @"Metadata analyzed"));
            }
            catch (ThreadAbortException)
            {
                Invoke(
                    new MethodInvoker(
                        () => { Program.FormMainInstance.toolStripStatusLabelLeft.Text = @"Metadata analysis aborted"; }));
            }
            finally
            {
                Invoke(new MethodInvoker(() =>
                {
                    Program.FormMainInstance.toolStripDropDownButtonStop.Enabled = false;
                    Program.FormMainInstance.TreeView.GetNode(GUI.Navigation.Project.Network.ToNavigationPath()).Expand();
                    Program.FormMainInstance.TreeView.GetNode(GUI.Navigation.Project.Network.Clients.ToNavigationPath()).Expand();
                    Program.FormMainInstance.TreeView.GetNode(GUI.Navigation.Project.Network.Servers.ToNavigationPath()).Expand();
                    Program.FormMainInstance.TreeView.GetNode(GUI.Navigation.Project.Network.Servers.Unknown.ToNavigationPath()).Expand();
                    Analysis = null;
                }));
            }
        }

        /// <summary>
        ///     Identify operating system and software installed in each machine
        /// </summary>
        private void IdentifyOsSoftware()
        {
            foreach (var ci in Program.data.computers.Items)
            {
                var so = ci.os;
                if (so == OperatingSystem.OS.Unknown && !ci.NotOS)
                {
                    if (ci.type == ComputersItem.Tipo.ClientPC && ci.SourceDocuments.Any())
                    {
                        var strSourceDocument = ci.SourceDocuments[0];
                        var fi = Program.data.files.Items.First(f => f.URL == strSourceDocument);
                        if (fi?.Metadata?.FoundMetaData != null)
                        {
                            var m = fi.Metadata.FoundMetaData;
                            if (m.OperativeSystem != null)
                            {
                                so = OperatingSystemUtils.StringToOS(m.OperativeSystem);
                            }
                            if ((so == OperatingSystem.OS.Unknown) && m.Applications.Items.Count > 0)
                            {
                                var os = OperatingSystemUtils.SoftwareToOS(m.Applications);
                                if (os != OperatingSystem.OS.Unknown)
                                    so = os;
                            }
                        }
                    }
                }
                if (so == OperatingSystem.OS.Windows || so == OperatingSystem.OS.Unknown)
                {
                    // if X:\users\ is found --> it's vista or higher
                    // if X:\Documents and Settings\ --> XP or lower
                    // if X:\ --> Windows
                    if (ci.Folders.Items.Count > 0)
                    {
                        int hitsWindows = 0, hitsWindowsXp = 0, hitsWindowsVista = 0, hitsWindowsNt4 = 0;
                        foreach (
                            var ri in
                                ci.Folders.Items.Where(ri => ri.IsComputerFolder)
                                    .Where(ri => char.IsLetter(ri.Path[0]) && ri.Path[1] == ':' && ri.Path[2] == '\\'))
                        {
                            hitsWindows++;
                            if (ri.Path.ToLower().IndexOf("documents and settings", StringComparison.Ordinal) == 3 ||
                                ri.Path.ToLower().IndexOf("dokumente und einstellungen", StringComparison.Ordinal) == 3)
                                hitsWindowsXp++;
                            else if (ri.Path.ToLower().IndexOf("users", StringComparison.Ordinal) == 3 &&
                                     ri.Path.ToLower().IndexOf("usuarios", StringComparison.Ordinal) == 3)
                                hitsWindowsVista++;
                            else if (ri.Path.ToLower().IndexOf("winnt", StringComparison.Ordinal) == 3)
                                hitsWindowsNt4++;
                        }
                        if (hitsWindowsNt4 > 0 && hitsWindowsNt4 > hitsWindowsXp && hitsWindowsNt4 > hitsWindowsVista)
                            so = OperatingSystem.OS.WindowsNT40;
                        else if (hitsWindowsXp > 0 && hitsWindowsXp > hitsWindowsVista && hitsWindowsXp >= hitsWindowsNt4)
                            so = OperatingSystem.OS.WindowsXP;
                        else if (hitsWindowsVista > 0 && hitsWindowsVista >= hitsWindowsXp &&
                                 hitsWindowsVista >= hitsWindowsNt4)
                            so = OperatingSystem.OS.WindowsVista;
                        else if (hitsWindows > 0)
                            so = OperatingSystem.OS.Windows;
                    }
                }
                ci.os = so;

                #region identify installed software

                if (ci.NotOS || ci.type != ComputersItem.Tipo.ClientPC) continue;
                foreach (var document in ci.SourceDocuments)
                {
                    var strSourceDocument = document;
                    FilesItem fi = Program.data.files.Items.FirstOrDefault(f => f.URL == strSourceDocument);
                    if (fi?.Metadata?.FoundMetaData == null || fi.Metadata.FoundMetaData.Applications.Items.Count <= 0)
                        continue;
                    foreach (var aplicacion in from aplicacion in fi.Metadata.FoundMetaData.Applications.Items
                                               let strSoftware = aplicacion.Name
                                               where !ci.Software.Items.Any(a => a.Name == strSoftware)
                                               select aplicacion)
                    {
                        ci.Software.Items.Add(aplicacion);
                    }
                }

                #endregion
            }
        }

        /// <summary>
        ///     Analyze all documents and create machines with users and paths,
        ///     also servers each time a shared path is found
        /// </summary>
        private void GetPCsAndServers()
        {
            var unIdentifiedComputer = 0;
            // for each document, a new machine is generated
            foreach (var fi in Program.data.files.Items)
            {
                // only use those files where metadata was found
                if (fi.Metadata == null) continue;
                var machineUsers = new ConcurrentBag<UserItem>(fi.Metadata.FoundUsers.Items.Where(u => u.IsComputerUser));
                var machinePaths = new ConcurrentBag<PathsItem>(fi.Metadata.FoundPaths.Items.Where(r => r.IsComputerFolder));
                var servers = new ConcurrentBag<ServersItem>(fi.Metadata.FoundServers.Items);
                var passwords = new ConcurrentBag<PasswordsItem>(fi.Metadata.FoundPasswords.Items);

                if (servers.Count > 0 || machineUsers.Count > 0 || machinePaths.Count > 0 ||
                    fi.Metadata.FoundPrinters.Items.Count > 0)
                {
                    var ciClient = new ComputersItem();
                    // add the machine to current data
                    Program.data.computers.Items.Add(ciClient);
                    ciClient.type = ComputersItem.Tipo.ClientPC;
                    ciClient.SourceDocuments.Add(fi.URL);
                    ciClient.SourceDocuments.Add(fi.Path);
                    ciClient.os = OperatingSystem.OS.Unknown;
                    if (machineUsers.Count > 0)
                    {
                        foreach (var t in machineUsers)
                        {
                            ciClient.Users.Items.Add(t);
                        }
                        var strNodeName = $"PC_{ciClient.Users.Items[0].Name}";
                        try
                        {
                            foreach (var c in Program.data.computers.Items)
                            {
                                if (c.name != null)
                                {
                                    if (string.Equals(c.name, strNodeName, StringComparison.CurrentCultureIgnoreCase))
                                    {
                                        strNodeName = $"PC_Unknown{++unIdentifiedComputer}";
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                        ciClient.name = strNodeName;
                    }
                    else // no username known, use a generic one
                    {
                        var strNodeName = "PC_Unknown0";
                        if (
                            Program.data.computers.Items.Any(
                                c => string.Equals(c.name, strNodeName, StringComparison.CurrentCultureIgnoreCase)))
                            strNodeName = $"PC_Unknown{++unIdentifiedComputer}";
                        ciClient.name = strNodeName;
                    }

                    #region identify servers in the 'servers' module of metadata (example: rdp metadata)

                    if (servers.Count > 0)
                    {
                        foreach (var si in servers)
                        {
                            ComputersItem ciServer;
                            // if an existing server was found, add the documents from where it was extracted
                            if (
                                Program.data.computers.Items.Any(
                                    c => string.Equals(c.name, si.Name, StringComparison.CurrentCultureIgnoreCase)))
                            {
                                var si1 = si;
                                ciServer =
                                    Program.data.computers.Items.First(
                                        c => string.Equals(c.name, si1.Name, StringComparison.CurrentCultureIgnoreCase));
                                if (!ciServer.SourceDocuments.Contains(fi.URL))
                                    ciServer.SourceDocuments.Add(fi.URL);
                            }
                            else // if it doesn't exist, create it and add the information
                            {
                                ciServer = new ComputersItem();
                                Program.data.computers.Items.Add(ciServer);
                                ciServer.name = si.Name;
                                ciServer.type = ComputersItem.Tipo.Server;
                                ciServer.NameChangedEvent += NameChangedFunction;
                                ciServer.SourceDocuments.Add(fi.URL);
                                ciServer.os = OperatingSystem.OS.Unknown;
                                ResolveHost(si.Name, ref ciServer);
                                try
                                {
                                    var ip = Dns.GetHostEntry(si.Name).AddressList[0].ToString();
                                    Program.data.AddIP(ip, "Metadata extraction", Program.cfgCurrent.MaxRecursion);
                                    Program.data.computerIPs.Items.Add(new ComputerIPsItem(ciServer,
                                        Program.data.GetIp(ip), "Metadata extraction"));

                                    Program.data.AddResolution(si.Name, ip, @"Metadata extraction",
                                        Program.cfgCurrent.MaxRecursion, Program.cfgCurrent, true);
                                }
                                catch
                                {
                                }
                                if (machineUsers.Count > 0)
                                {
                                    foreach (var ui in machineUsers)
                                        ciServer.RemoteUsers.AddUniqueItem(ui.Name, false);
                                }
                                if (passwords.Count <= 0) continue;
                                foreach (var pi in passwords)
                                    ciServer.RemotePasswords.AddUniqueItem(pi);
                            }
                        }
                    }

                    #endregion

                    #region Identify servers with shared routes

                    // client folders, example: C:\Documents and settings\
                    foreach (var ri in machinePaths)
                    {
                        string server;
                        // it isn't a local path, it's a shared folder route, so, let's add a server
                        if (IsServerPath(ri.Path, out server))
                        {
                            // add it only if the 'server' variable matches the current domain or it's alternative domains
                            var found = false;
                            if (Program.data.Project.Domain != null)
                            {
                                if (Program.data.Project.AlternativeDomains.Any(y => y.Contains(server)) ||
                                    server.Contains(Program.data.Project.Domain))
                                    found = true;
                            }

                            IPAddress ipAddress;
                            // if it's an IP address, it must be added to the list
                            if (IPAddress.TryParse(server, out ipAddress))
                                found = true;
                            else
                            {
                                // if it's a hostname which doesn't resolve to an IP, add it to the list
                                try
                                {
                                    // here an internet connection is needed because if there's no path to the internet,
                                    // when the DNS request for a host is done, it will return that it doesn't have IP address
                                    if (Dns.GetHostAddresses(server).Length == 0)
                                        found = true;
                                }
                                catch
                                {
                                    found = true;
                                }
                            }

                            if (!found)
                                continue;

                            ComputersItem ciServer;
                            // if an already existing server is found, add the documents
                            if (
                                Program.data.computers.Items.Any(
                                    c => string.Equals(c.name, server, StringComparison.CurrentCultureIgnoreCase)))
                            {
                                ciServer =
                                    Program.data.computers.Items.First(
                                        c => string.Equals(c.name, server, StringComparison.CurrentCultureIgnoreCase));
                                if (!ciServer.SourceDocuments.Contains(fi.URL))
                                    ciServer.SourceDocuments.Add(fi.URL);
                            }
                            else
                            {
                                ciServer = new ComputersItem();
                                Program.data.computers.Items.Add(ciServer);
                                ciServer.type = ComputersItem.Tipo.Server;
                                ciServer.NameChangedEvent += NameChangedFunction;
                                ciServer.SourceDocuments.Add(fi.URL);
                                ciServer.os = OperatingSystem.OS.Unknown;
                                if (ri.Path.StartsWith("\\\\"))
                                    ciServer.os = OperatingSystem.OS.Windows;
                                ResolveHost(server, ref ciServer);

                                try
                                {
                                    var ip = Dns.GetHostEntry(server).AddressList[0].ToString();
                                    Program.data.AddIP(ip, "Metadata extraction", Program.cfgCurrent.MaxRecursion);
                                    Program.data.computerIPs.Items.Add(new ComputerIPsItem(ciServer,
                                        Program.data.GetIp(ip), "Metadata extraction"));
                                    Program.data.AddResolution(server, ip, "Metadata extraction",
                                        Program.cfgCurrent.MaxRecursion, Program.cfgCurrent, true);
                                }
                                catch
                                {
                                }
                            }
                            // if the server's name is shorter than the original route, it means that it was a shared folder, example: \\Server\C\Windows\
                            if (ri.Path.Length > server.Length)
                            {
                                if (ri.Path.IndexOf(server, StringComparison.Ordinal) > 0 &&
                                    ri.Path.IndexOf(server, StringComparison.Ordinal) + server.Length + 1 <
                                    ri.Path.Length)
                                {
                                    var folder =
                                        ri.Path.Substring(ri.Path.IndexOf(server, StringComparison.Ordinal) +
                                                          server.Length);
                                    var rii = ciServer.Folders.AddUniqueItem(folder, true);
                                    // add remote users for this folder those who are users from the machine which had access to it
                                    if (rii != null)
                                    {
                                        foreach (var ui in machineUsers)
                                        {
                                            rii.RemoteUsers.AddUniqueItem(ui.Name, false);
                                        }
                                    }
                                }
                            }
                            // add to the server user's who have access to it
                            if (machineUsers.Count > 0)
                            {
                                foreach (var ui in machineUsers)
                                {
                                    ciServer.RemoteUsers.AddUniqueItem(ui.Name, false);
                                }
                            }

                            // add passwords to the server
                            if (passwords.Count > 0)
                            {
                                foreach (var pi in passwords)
                                {
                                    ciServer.RemotePasswords.AddUniqueItem(pi);
                                }
                            }

                            // Add to the user the remote folder to which he has access
                            ciClient.RemoteFolders.AddUniqueItem(ri.Path, false);
                        }
                        else
                        {
                            ciClient.Folders.AddUniqueItem(ri.Path, ri.IsComputerFolder);
                        }
                    }

                    #endregion

                    #region Identify severs with shared printers

                    // iterate over printers looking for servers
                    if (fi.Metadata.FoundPrinters.Items.Count > 0)
                    {
                        foreach (
                            var p in fi.Metadata.FoundPrinters.Items.Where(ii => !string.IsNullOrEmpty(ii.Printer)))
                        {
                            string server;
                            // it isn't a local printer, it's a printer in a shared server, so let's add a new server
                            if (IsServerPath(p.Printer, out server))
                            {
                                ComputersItem ciServer;
                                if (
                                    Program.data.computers.Items.Any(
                                        c => string.Equals(c.name, server, StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    // if an already existing server is found, add different printers
                                    ciServer =
                                        Program.data.computers.Items.First(
                                            c =>
                                                string.Equals(c.name, server, StringComparison.CurrentCultureIgnoreCase));
                                    if (!ciServer.SourceDocuments.Contains(fi.URL))
                                        ciServer.SourceDocuments.Add(fi.URL);
                                }
                                else // new server node
                                {
                                    ciServer = new ComputersItem();
                                    Program.data.computers.Items.Add(ciServer);
                                    ciServer.type = ComputersItem.Tipo.Server;
                                    ciServer.os = OperatingSystem.OS.Unknown;
                                    ciServer.SourceDocuments.Add(fi.URL);
                                    ResolveHost(server, ref ciServer);
                                }
                                if (p.Printer.Length > server.Length)
                                {
                                    var printerName =
                                        p.Printer.Substring(p.Printer.IndexOf(server, StringComparison.Ordinal) +
                                                            server.Length);
                                    var iitem = ciServer.Printers.AddUniqueItem(printerName.Trim());
                                    // add printer's remote users
                                    if (iitem != null)
                                    {
                                        foreach (var ui in machineUsers)
                                        {
                                            iitem.RemoteUsers.AddUniqueItem(ui.Name, false);
                                        }
                                    }
                                }
                                // add to the servers the users who have access to it
                                if (machineUsers.Count > 0)
                                {
                                    foreach (var ui in machineUsers)
                                    {
                                        ciServer.RemoteUsers.AddUniqueItem(ui.Name, false);
                                    }
                                }
                                // add the remote printer to the user
                                ciClient.RemotePrinters.AddUniqueItem(p.Printer);
                            }
                            else
                            {
                                ciClient.Printers.AddUniqueItem(p.Printer);
                            }
                        }
                    }

                    #endregion
                }

                // iterate over the history to add new machines and folders to the existing user's machines
                if (fi.Metadata.FoundHistory.Items.Count <= 0) continue;
                foreach (var hi in fi.Metadata.FoundHistory.Items)
                {
                    hi.Author = hi.Author?.Trim() ?? string.Empty;
                    hi.Path = hi.Path?.Trim() ?? string.Empty;
                    if (string.IsNullOrEmpty(hi.Path)) continue;
                    string server;
                    // it's a folder from a shared path so we add a new server
                    if (IsServerPath(hi.Path, out server))
                    {
                        ComputersItem ciServer;
                        if (
                            Program.data.computers.Items.Any(
                                c => string.Equals(c.name, server, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            ciServer =
                                Program.data.computers.Items.First(
                                    c => string.Equals(c.name, server, StringComparison.CurrentCultureIgnoreCase));
                            if (!ciServer.SourceDocuments.Contains(fi.URL))
                                ciServer.SourceDocuments.Add(fi.URL);
                        }
                        else
                        {
                            ciServer = new ComputersItem();
                            Program.data.computers.Items.Add(ciServer);
                            ciServer.type = ComputersItem.Tipo.Server;
                            ciServer.os = OperatingSystem.OS.Windows;
                            ciServer.SourceDocuments.Add(fi.URL);
                            ResolveHost(server, ref ciServer);
                        }
                        // if the found server's length is greater than the original route, that means that there's an external route like \\Server\\route
                        if (hi.Path.Length > server.Length)
                        {
                            // if the server isn't at the beggining or at the end, it means that it's a valid route
                            if (hi.Path.IndexOf(server, StringComparison.Ordinal) > 0 &&
                                hi.Path.IndexOf(server, StringComparison.Ordinal) + server.Length + 1 < hi.Path.Length)
                            {
                                var folder =
                                    hi.Path.Substring(hi.Path.IndexOf(server, StringComparison.Ordinal) + server.Length);
                                // The router is from the server itself
                                var ri = ciServer.Folders.AddUniqueItem(folder, true);
                                if (ri != null && Users.IsValidUser(hi.Author))
                                {
                                    ri.RemoteUsers.AddUniqueItem(hi.Author, false);
                                }
                            }
                        }
                        if (!Users.IsValidUser(hi.Author)) continue;
                        // it isn't an user from the remote host
                        ciServer.RemoteUsers.AddUniqueItem(hi.Author, false);

                        var ciClient = new ComputersItem();
                        Program.data.computers.Items.Add(ciClient);
                        ciClient.type = ComputersItem.Tipo.ClientPC;
                        ciClient.SourceDocuments.Add(fi.URL);
                        ciClient.os = OperatingSystem.OS.Unknown;
                        ciClient.NotOS = true;
                        ciClient.Users.AddUniqueItem(hi.Author, true);
                        var strNodeName = $"PC_{hi.Author}";
                        if (Program.data.computers.Items.Any(c => c.name?.ToLower() == strNodeName.ToLower()))
                            strNodeName = $"PC_Unknown{++unIdentifiedComputer}";
                        ciClient.name = strNodeName;
                        ciClient.RemoteFolders.AddUniqueItem(hi.Path, false);
                    }
                    else
                    {
                        if (!Users.IsValidUser(hi.Author) || string.IsNullOrEmpty(hi.Path) ||
                            !PathAnalysis.IsValidPath(hi.Path.Trim())) continue;
                        // if it isn't a server (\\Server\) but it has an author, add it as personal computer
                        var ciClient = new ComputersItem();
                        Program.data.computers.Items.Add(ciClient);
                        ciClient.type = ComputersItem.Tipo.ClientPC;
                        ciClient.SourceDocuments.Add(fi.URL);
                        ciClient.os = OperatingSystem.OS.Unknown;
                        ciClient.NotOS = true;

                        var strNodeName = $"PC_{hi.Author}";
                        if (
                            Program.data.computers.Items.Any(
                                c => string.Equals(c.name, strNodeName, StringComparison.CurrentCultureIgnoreCase)))
                            strNodeName = $"PC_Unknown{++unIdentifiedComputer}";
                        ciClient.name = strNodeName;
                        ciClient.Users.AddUniqueItem(hi.Author, true);
                        // Search for users into the path
                        var strUser = PathAnalysis.ExtractUserFromPath(hi.Path);
                        if (!string.IsNullOrEmpty(strUser))
                            ciClient.Users.AddUniqueItem(strUser, true);
                        ciClient.Folders.AddUniqueItem(hi.Path, true);
                    }
                }
            }
        }

        /// <summary>
        ///     Group client nodes matching by username
        /// </summary>
        private void GroupClientNodes()
        {
            var clientComputers =
                new List<ComputersItem>(Program.data.computers.Items.Where(c => c.type == ComputersItem.Tipo.ClientPC));
            var index = 0;
            var ciCurrent = clientComputers[index];
            while (true)
            {
                var match = false;
                var index1 = index;
                Invoke(new MethodInvoker(() =>
                {
                    if (
                        Program.FormMainInstance.toolStripStatusLabelLeft.Text.StartsWith(
                            "Grouping computers from node " + index1))
                    {
                        if (Program.FormMainInstance.toolStripStatusLabelLeft.Text.Length <
                            ("Grouping computers from node " + index1).Length + 3)
                            Program.FormMainInstance.toolStripStatusLabelLeft.Text += @".";
                        else
                            Program.FormMainInstance.toolStripStatusLabelLeft.Text = @"Grouping computers from node " +
                                                                                     index1;
                    }
                    else
                        Program.FormMainInstance.toolStripStatusLabelLeft.Text = @"Grouping computers from node " +
                                                                                 index1;
                }));
                if (ciCurrent.Users.Items.Count > 0)
                {
                    // For each user found in the current node
                    foreach (var ui in ciCurrent.Users.Items.Where(ui => ui.IsComputerUser))
                    {
                        // search in the next nodes for a match
                        for (var j = index + 1; j < clientComputers.Count; j++)
                        {
                            if (clientComputers[j].Users.Items.Count <= 0) continue;
                            if (
                                clientComputers[j].Users.Items.Where(ui2 => ui2.IsComputerUser)
                                    .Any(
                                        ui2 =>
                                            string.Equals(ui2.Name.Trim(), ui.Name.Trim(),
                                                StringComparison.CurrentCultureIgnoreCase)))
                            {
                                JoinComputers(ciCurrent, clientComputers[j]);
                                clientComputers.Remove(clientComputers[j]);
                                match = true;
                            }
                            if (match) break;
                        }
                        if (match) break;
                    }
                }
                // if the machine has no users but it has routes, search for machines in those routes
                else if (ciCurrent.Folders.Items.Count > 0)
                {
                    // foreach folder found in those routes
                    foreach (var ri in ciCurrent.Folders.Items.Where(ri => ri.IsComputerFolder))
                    {
                        for (var j = index + 1; j < clientComputers.Count; j++)
                        {
                            if (clientComputers[j].Folders.Items.Count <= 0) continue;
                            if (
                                clientComputers[j].Folders.Items.Where(ri2 => ri2.IsComputerFolder)
                                    .Any(
                                        ri2 =>
                                            ri2.Path.Trim().ToLower().Contains(ri.Path.Trim().ToLower()) ||
                                            ri.Path.Trim().ToLower().Contains(ri2.Path.Trim().ToLower())))
                            {
                                JoinComputers(ciCurrent, clientComputers[j]);
                                clientComputers.Remove(clientComputers[j]);
                                match = true;
                            }
                            if (match) break;
                        }
                        if (match) break;
                    }
                }
                // if the machine has nor users nor routes but it has printers,
                // look for machines which also have those printers and join them
                else if (ciCurrent.Printers.Items.Count > 0)
                {
                    foreach (var ii in ciCurrent.Printers.Items)
                    {
                        for (var j = index + 1; j < clientComputers.Count; j++)
                        {
                            if (clientComputers[j].Printers.Items.Count <= 0) continue;
                            if (
                                clientComputers[j].Printers.Items.Any(
                                    ii2 =>
                                        string.Equals(ii2.Printer.Trim(), ii.Printer.Trim(),
                                            StringComparison.CurrentCultureIgnoreCase)))
                            {
                                JoinComputers(ciCurrent, clientComputers[j]);
                                clientComputers.Remove(clientComputers[j]);
                                match = true;
                            }
                            if (match) break;
                        }
                        if (match) break;
                    }
                }
                // if the machine has nor users nor routers nor printers, but it has remote routes,
                // search for machines which share those routes and join them
                else if (ciCurrent.RemoteFolders.Items.Count > 0)
                {
                    foreach (var ri in ciCurrent.RemoteFolders.Items)
                    {
                        for (var j = index + 1; j < clientComputers.Count; j++)
                        {
                            if (clientComputers[j].RemoteFolders.Items.Count <= 0) continue;
                            if (
                                clientComputers[j].RemoteFolders.Items.Any(
                                    ri2 =>
                                        string.Equals(ri2.Path.Trim(), ri.Path.Trim(),
                                            StringComparison.CurrentCultureIgnoreCase)))
                            {
                                JoinComputers(ciCurrent, clientComputers[j]);
                                clientComputers.Remove(clientComputers[j]);
                                match = true;
                            }
                            if (match) break;
                        }
                        if (match) break;
                    }
                }
                // if the machine has nor users, nor routes, nor printers nor remote routes,
                // but it has remote printers, search for machines which share those remote
                // printers and join them
                else if (ciCurrent.RemotePrinters.Items.Count > 0)
                {
                    foreach (var ii in ciCurrent.RemotePrinters.Items)
                    {
                        for (var j = index + 1; j < clientComputers.Count; j++)
                        {
                            if (clientComputers[j].RemotePrinters.Items.Count <= 0) continue;
                            if (
                                clientComputers[j].RemotePrinters.Items.Any(
                                    ii2 =>
                                        string.Equals(ii2.Printer.Trim(), ii.Printer.Trim(),
                                            StringComparison.CurrentCultureIgnoreCase)))
                            {
                                JoinComputers(ciCurrent, clientComputers[j]);
                                clientComputers.Remove(clientComputers[j]);
                                match = true;
                            }
                            if (match) break;
                        }
                        if (match) break;
                    }
                }

                // if there isn't a node where a match happens, go to the next one
                if (match) continue;
                if (index + 1 >= clientComputers.Count)
                    break;
                index++;
                ciCurrent = clientComputers[index];
            }
        }

        /// <summary>
        /// Joins the information of 2 computers into the first one and removes the last one
        /// </summary>
        /// <param name="ci1"></param>
        /// <param name="ci2"></param>
        private static void JoinComputers(ComputersItem ci1, ComputersItem ci2)
        {
            if (ci2.SourceDocuments.Count > 0 && !ci1.SourceDocuments.Contains(ci2.SourceDocuments[0]))
            {
                ci1.SourceDocuments.Add(ci2.SourceDocuments[0]);
            }
            // join operating systems
            if (ci1.os != OperatingSystem.OS.Unknown && ci2.os == OperatingSystem.OS.Unknown)
            {
                ci1.os = ci2.os;
            }
            // join users, folders and printers
            if (ci1.Users.Items.Count > 0 && ci2.Users.Items.Count > 0)
            {
                foreach (
                    var ui3 in
                        ci2.Users.Items.Where(
                            ui3 => !ci1.Users.Items.Contains(ui3, new CaseInsensitiveUserItemComparer())))
                {
                    ci1.Users.Items.Add(ui3);
                }
            }
            if (ci2.Folders.Items.Count > 0)
            {
                foreach (
                    var ri3 in
                        ci2.Folders.Items.Where(
                            ri3 => !ci1.Folders.Items.Contains(ri3, new CaseInsensitiveFolderItemComparer<PathsItem>()))
                    )
                {
                    ci1.Folders.Items.Add(ri3);
                }
            }
            if (ci2.RemoteFolders.Items.Count > 0)
            {
                foreach (
                    var ri3 in
                        ci2.RemoteFolders.Items.Where(
                            ri3 =>
                                !ci1.RemoteFolders.Items.Contains(ri3,
                                    new CaseInsensitiveFolderItemComparer<PathsItem>())))
                {
                    ci1.RemoteFolders.Items.Add(ri3);
                }
            }
            if (ci2.Printers.Items.Count > 0)
            {
                foreach (
                    var ii3 in
                        ci2.Printers.Items.Where(
                            ii3 =>
                                !ci1.Printers.Items.Contains(ii3, new CaseInsensitivePrinterItemComparer<PrintersItem>()))
                    )
                {
                    ci1.Printers.Items.Add(ii3);
                }
            }
            if (ci2.RemotePrinters.Items.Count > 0)
            {
                foreach (
                    var ii3 in
                        ci2.RemotePrinters.Items.Where(
                            ii3 =>
                                !ci1.RemotePrinters.Items.Contains(ii3,
                                    new CaseInsensitivePrinterItemComparer<PrintersItem>())))
                {
                    ci1.RemotePrinters.Items.Add(ii3);
                }
            }
            // remove the second computer, it's information is now stored into the first one
            Program.data.computers.Items.Remove(ci2);
        }

        /// <summary>
        /// For each named server, search for possible new resolutions
        /// </summary>
        public void ReanalyzeServers()
        {
            Invoke(new MethodInvoker(() =>
            {
                const string strMessage = "Resolving unknown servers...";
                Program.FormMainInstance.toolStripStatusLabelLeft.Text = strMessage;
                Program.LogThis(new Log(Log.ModuleType.FOCA, strMessage, Log.LogType.debug));
            }));

            var lstCi =
                new List<ComputersItem>(
                    Program.data.computers.Items.Where(
                        c => c.type == ComputersItem.Tipo.Server && c.name.IndexOf('.') < 0));
            foreach (var t in lstCi)
            {
                var ci = t;
                ResolveHost(ci.name, ref ci);
            }
            Invoke(new MethodInvoker(() =>
            {
                const string strMessage = "Reanalysis finished!";
                Program.FormMainInstance.toolStripStatusLabelLeft.Text = strMessage;
                Program.LogThis(new Log(Log.ModuleType.FOCA, strMessage, Log.LogType.debug));
            }));
            // obtain machines for each IP
            Program.data.GetServersFromIPs();
        }

        /// <summary>
        ///     Resolve directly and reversely a server's address
        /// </summary>
        /// <param name="server"></param>
        /// <param name="ci"></param>
        public void ResolveHost(string server, ref ComputersItem ci)
        {
            var strSource = ci.SourceDocuments.Count > 0
                ? $"Metadata extraction [{ci.SourceDocuments[0]}]"
                : "Metadata extraction";
            if (IsIP(server))
            {
                var strIp = server;
                Program.data.AddIP(strIp, strSource, Program.data.Project.Domain, Program.cfgCurrent.MaxRecursion, true);
                Program.data.computerIPs.Items.Add(new ComputerIPsItem(ci, Program.data.GetIp(strIp), strSource));
                // check if it was possible to reverse resolve the server's IP
                if (Program.data.GetResolutionDomains(server).Count > 0)
                {
                    var domains = Program.data.GetResolutionDomains(strIp);
                    ci.name = domains[0].Domain;
                    foreach (var domain in domains)
                    {
                        Program.data.computerDomains.Items.Add(new ComputerDomainsItem(ci, domain, strSource));
                    }
                }
                else
                {
                    ci.name = server;
                }
            }
            else
            {
                if (
                    Program.data.computerDomains.Items.Any(
                        c =>
                            c.Domain != null &&
                            string.Equals(c.Domain.Domain, server, StringComparison.OrdinalIgnoreCase)))
                {
                    // a machine assigned to that domain already exists, remove the new one
                    Program.data.computers.Items.Remove(ci);
                    // use the one that already exists
                    ci =
                        Program.data.computerDomains.Items.First(
                            c => string.Equals(c.Domain.Domain, server, StringComparison.OrdinalIgnoreCase)).Computer;
                }
                else
                {
                    ci.name = server;
                    var lstDomains = new List<string>();
                    if (!string.IsNullOrEmpty(Program.data.Project.Domain))
                        lstDomains.Add(Program.data.Project.Domain);
                    lstDomains.AddRange(Program.data.Project.AlternativeDomains);
                    // add all subdomains
                    lstDomains.AddRange(
                        Program.data.domains.Items.Where(
                            d =>
                                lstDomains.Any(
                                    d2 => d.Domain != d2 && d.Domain.EndsWith(d2, StringComparison.OrdinalIgnoreCase)))
                            .Select(d => d.Domain));
                    // if it isn't a domain, add the configuration's domain and alternative domains
                    if (server.IndexOf('.') < 0 && lstDomains.Count > 0)
                    {
                        if (!Program.cfgCurrent.ResolveHost) return;
                        foreach (var strDomain in lstDomains)
                        {
                            IPAddress[] ips;
                            Program.LogThis(new Log(Log.ModuleType.FOCA,
                                $"Trying resolve {server + "." + strDomain}", Log.LogType.debug));
                            if (DNSUtilities.isDNSAnyCast(strDomain) ||
                                !DNSUtilities.ExistsDomain(server + "." + strDomain, out ips)) continue;
                            ci.name = $"{server}.{strDomain}";

                            // add the domain to the DNS enumeration data
                            Program.data.AddDomain(ci.name, strSource, Program.cfgCurrent.MaxRecursion,
                                Program.cfgCurrent);
                            Program.data.computerDomains.Items.Add(new ComputerDomainsItem(ci,
                                Program.data.GetDomain(ci.name), strSource));
                            foreach (var ip in ips)
                            {
                                Program.data.AddIP(ip.ToString(), strSource,
                                    Program.cfgCurrent.ResolveHost ? 2 : 0);
                                Program.data.computerIPs.Items.Add(new ComputerIPsItem(ci,
                                    Program.data.GetIp(ip.ToString()), strSource));
                            }
                            break;
                        }
                    }
                    else
                    {
                        // add the domain to the DNS enumeration data
                        Program.data.AddDomain(server, strSource,
                            Program.cfgCurrent.ResolveHost ? Program.cfgCurrent.MaxRecursion : 0, Program.cfgCurrent);

                        IPAddress[] ips;
                        if (!Program.cfgCurrent.ResolveHost || DNSUtilities.isDNSAnyCast(server) ||
                            !DNSUtilities.ExistsDomain(server, out ips)) return;
                        foreach (var ip in ips)
                        {
                            Program.data.AddIP(ip.ToString(), strSource, Program.cfgCurrent.MaxRecursion);
                            Program.data.computerIPs.Items.Add(new ComputerIPsItem(ci,
                                Program.data.GetIp(ip.ToString()), strSource));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// If a machine is renamed, we must rename the node that represents it
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public void NameChangedFunction(object o, DoubleStringEventArgs e)
        {
            if (e.oldString != e.newString)
                RenameNode(e.oldString, e.newString);
        }

        /// <summary>
        /// Rename a machine, if a machine with the new name already exists, join their information
        /// </summary>
        /// <param name="strOldName"></param>
        /// <param name="strNewName"></param>
        private static void RenameNode(string strOldName, string strNewName)
        {
            if (Program.data.computers.Items.All(c => c.name != strNewName)) return;
            var ciOld = Program.data.computers.Items.First(c => c.name == strOldName);
            var ciNew = Program.data.computers.Items.First(c => c.name == strNewName);
            if (ciOld != null && ciNew != null)
            {
                // join users
                foreach (var ui in ciOld.Users.Items)
                {
                    ciNew.Users.AddUniqueItem(ui.Name, ui.IsComputerUser);
                }
                // join remote users
                foreach (var ui in ciOld.RemoteUsers.Items)
                {
                    ciNew.RemoteUsers.AddUniqueItem(ui.Name, ui.IsComputerUser);
                }
                // join routes
                foreach (var ri in ciOld.Folders.Items)
                {
                    ciNew.Folders.AddUniqueItem(ri.Path, ri.IsComputerFolder);
                }
                // join remote routes
                foreach (var ri in ciOld.RemoteFolders.Items)
                {
                    ciNew.RemoteFolders.AddUniqueItem(ri.Path, ri.IsComputerFolder);
                }
                // join printers
                foreach (var ii in ciOld.Printers.Items)
                {
                    ciNew.Printers.AddUniqueItem(ii.Printer);
                }
                // join remote printers
                foreach (var ii in ciOld.RemotePrinters.Items)
                {
                    ciNew.RemotePrinters.AddUniqueItem(ii.Printer);
                }
                // join identified software
                foreach (var aplicacion in from aplicacion in ciOld.Software.Items
                                           let any = ciNew.Software.Items.Any(a => a.Name.ToLower().Equals(aplicacion.Name.ToLower()))
                                           where !any
                                           select aplicacion)
                {
                    ciNew.Software.Items.Add(aplicacion);
                }
                // join source documents
                foreach (
                    var strDocument in
                        ciOld.SourceDocuments.Where(
                            strDocument =>
                                !ciNew.SourceDocuments.Contains(strDocument.ToLower(), StringComparer.OrdinalIgnoreCase))
                    )
                {
                    ciNew.SourceDocuments.Add(strDocument);
                }
                // join domains
                foreach (var cdi in Program.data.computerDomains.Items.Where(c => c.Computer.name == ciOld.name))
                {
                    if (Program.data.computerDomains.Items.All(c => c.Domain.Domain != cdi.Domain.Domain))
                    {
                        Program.data.computerDomains.Items.Add(new ComputerDomainsItem(ciNew, cdi.Domain, cdi.Source));
                    }
                    Program.data.computerDomains.Items.Remove(cdi);
                }
                // join IP addresses
                foreach (var cii in Program.data.computerIPs.Items.Where(c => c.Computer.name == ciOld.name))
                {
                    if (Program.data.computerIPs.Items.All(c => c.Ip.Ip != cii.Ip.Ip))
                    {
                        Program.data.computerIPs.Items.Add(new ComputerIPsItem(ciNew, cii.Ip, cii.Source));
                    }
                    Program.data.computerIPs.Items.Remove(cii);
                }
            }
            // remove the machine with the old name
            Program.data.computers.Items.Remove(ciOld);
        }

        #endregion

        #region WebSearch functions

        private void Search(IEnumerable<LinkSearcher> engines, Func<LinkSearcher, Task<int>> searchFunc)
        {
            try
            {
                this.searchCancelToken?.Cancel();
                this.searchCancelToken = new CancellationTokenSource();
                List<Task> searchTasks = new List<Task>();

                foreach (LinkSearcher currentSearcher in engines)
                {
                    searchTasks.Add(SearchInEngine(currentSearcher, searchFunc));
                }

                Task.WhenAll(searchTasks)
                    .ContinueWith((e) =>
                    {
                        Invoke(new MethodInvoker(() =>
                        {
                            btnSearchAll.Text = "&Search All";
                            btnSearchAll.Image = Resources.world_search;
                            btnSearch.Text = "&Search";
                            btnSearch.Image = Resources.magnifier;
                            checkedListBoxExtensions.Enabled = panelSearchConfiguration.Enabled = true;
                        }));
                        Program.FormMainInstance.ChangeStatus("All searchers have finished");
                        this.searchCancelToken.Dispose();
                        this.searchCancelToken = null;
                    });
            }
            catch
            {

            }
        }

        private IEnumerable<LinkSearcher> CreateSelectedSearchers()
        {
            List<LinkSearcher> selectedSearchers = new List<LinkSearcher>();
            if (chkGoogle.Checked)
            {
                if (String.IsNullOrWhiteSpace(Program.cfgCurrent.GoogleApiKey) || String.IsNullOrWhiteSpace(Program.cfgCurrent.GoogleApiCx))
                {
                    selectedSearchers.Add(new GoogleWebSearcher());

                }
                else
                {
                    selectedSearchers.Add(new GoogleAPISearcher(Program.cfgCurrent.GoogleApiKey, Program.cfgCurrent.GoogleApiCx));
                }
            }

            if (chkBing.Checked)
            {
                if (String.IsNullOrWhiteSpace(Program.cfgCurrent.BingApiKey))
                {
                    selectedSearchers.Add(new BingWebSearcher());
                }
                else
                {
                    selectedSearchers.Add(new BingAPISearcher(Program.cfgCurrent.BingApiKey));
                }
            }

            if (chkDuck.Checked)
            {
                selectedSearchers.Add(new DuckduckgoWebSearcher());
            }

            return selectedSearchers;
        }

        private Task<int> SearchInEngine(LinkSearcher wsSearch, Func<LinkSearcher, Task<int>> searchFunc)
        {
            wsSearch.SearcherChangeStateEvent += HandleChangeStateEvent;
            wsSearch.ItemsFoundEvent += HandleLinkFoundEvent;
            wsSearch.SearcherLogEvent += WebSearcherLogEvent;

            return searchFunc(wsSearch)
                .ContinueWith<int>((state) =>
                {
                    int resultCount = 0;
                    string message = String.Empty;
                    Log.LogType logType = Log.LogType.medium;
                    if (state.IsCanceled)
                    {
                        message = $"{wsSearch.Name} search aborted!!";
                    }
                    else if (state.IsFaulted)
                    {
                        message = $"An error has ocurred on {wsSearch.Name}: {String.Join(Environment.NewLine, (state.Exception as AggregateException)?.InnerException.Message)}.";
                        logType = Log.LogType.error;
                    }
                    else if (state.IsCompleted)
                    {
                        resultCount = state.Result;
                        message = $"{wsSearch.Name} search finished successfully!! Total found result count: {resultCount}";
                    }

                    Program.LogThis(new Log(Log.ModuleType.MetadataSearch, message, logType));
                    return resultCount;
                });
        }

        /// <summary>
        /// Handle searcher logging events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void WebSearcherLogEvent(object sender, EventsThreads.ThreadStringEventArgs e)
        {
            Program.LogThis(new Log(Log.ModuleType.MetadataSearch, $"Web Search, {e.Message}", Log.LogType.debug));
        }

        /// <summary>
        /// Handle searcher state changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleChangeStateEvent(object sender, EventsThreads.ThreadStringEventArgs e)
        {
            Invoke(new MethodInvoker(() =>
            {
                if (Program.FormMainInstance != null)
                    Program.FormMainInstance.toolStripStatusLabelLeft.Text = e.Message;
                Program.LogThis(new Log(Log.ModuleType.MetadataSearch, e.Message, Log.LogType.debug));
            }));
        }

        /// <summary>
        ///     Event used to handle found links events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void HandleLinkFoundEvent(object sender, EventsThreads.CollectionFound<Uri> e)
        {
            string source = String.Empty;
            if (sender is LinkSearcher ls)
            {
                source = ls.Name;
            }
            else
            {
                source = "Manually added";
            }

            Program.LogThis(new Log(Log.ModuleType.MetadataSearch, $"{source}: Found {e.Data.Count} links", Log.LogType.debug));

            foreach (Uri link in e.Data)
            {
                this.searchCancelToken?.Token.ThrowIfCancellationRequested();
                try
                {
                    if (Program.data.files.Items.Count(li => string.Equals(li.URL, link.ToString(), StringComparison.CurrentCultureIgnoreCase)) > 0)
                        continue;

                    DomainsItem currentDomain = Program.data.GetDomain(link.Host);
                    if (currentDomain == null)
                    {
                        Program.data.AddDomain(link.Host, source, Program.cfgCurrent.MaxRecursion, Program.cfgCurrent);
                        currentDomain = Program.data.GetDomain(link.Host);
                    }

                    currentDomain.map.AddUrl(link.ToString());

                    if (currentDomain.techAnalysis.domain == null)
                        currentDomain.techAnalysis.domain = currentDomain.Domain;

                    currentDomain.techAnalysis.eventLinkFoundDetailed(null, new EventsThreads.CollectionFound<Uri>(new List<Uri> { link }));
                }
                catch
                {
                }

                Invoke(new MethodInvoker(() =>
                {
                    var fi = new FilesItem
                    {
                        Ext = System.IO.Path.GetExtension(link.AbsolutePath).ToLower(),
                        URL = link.ToString(),
                        Downloaded = false,
                        Processed = false,
                        Date = DateTime.MinValue,
                        ModifiedDate = DateTime.MinValue,
                        Path = string.Empty,
                        Size = -1
                    };
                    Program.data.files.Items.Add(fi);
                    Program.FormMainInstance.treeViewMetadata_UpdateDocumentsNumber();
                    listViewDocuments_Update(fi);
                    HttpSizeDaemonInst.AddURL(fi);
                }));
                // add the domain from the found link to the project's domains
                if (Program.data.Project.ProjectState == Project.ProjectStates.Uninitialized)
                    continue;
                Program.data.AddDomain(link.Host, "Documents search", 0, Program.cfgCurrent);
                // add the URL to the domain's map
                Program.data.GetDomain(link.Host).map.AddDocument(link.ToString());
            }
        }

        #endregion

        #region Functions to download files

        /// <summary>
        ///     Event launched each time a download's progress changes
        /// </summary>
        /// <param name="o"></param>
        /// <param name="dpce"></param>
        private void DownloadProgressChanged(object o, DownloadProgressChangedEventArgs dpce)
        {
            Invoke(new MethodInvoker(() =>
            {
                Download file = ((DownloadableUri)((TaskCompletionSource<object>)dpce.UserState).Task.AsyncState).File;
                file.Pbar.Value = dpce.ProgressPercentage;
                file.DownloadStatus = Download.Status.InProgress;
            }));
        }

        /// <summary>
        ///     Event launched each time a download is completed
        /// </summary>
        /// <param name="o"></param>
        /// <param name="completedArgs"></param>
        private void DownloadProgressCompleted(object o, AsyncCompletedEventArgs completedArgs)
        {
            Download file = ((DownloadableUri)((TaskCompletionSource<object>)completedArgs.UserState).Task.AsyncState).File;
            file.CaClient?.Dispose();

            if (completedArgs.Cancelled)
            {
                Invoke(new MethodInvoker(() =>
                {
                    File.Delete(file.PhysicalPath);
                    FilesItem fi = (FilesItem)file.Lvi.Tag;
                    if (fi == null) return;
                    fi.Downloaded = false;
                    listViewDocuments.RemoveEmbeddedControl(file.Pbar);
                    this.downloadingFiles.TryRemove(file.DownloadUrl, out Download value);
                    this.UpdateProgressDownloadControls();
                }));

                if (file.Retries < 3 && !file.IsCanceled)
                {
                    file.Retries++;
                    this.downloadQueue.Enqueue(file);
                    this.downloadDelayToken.Cancel();
                    Program.LogThis(new Log(Log.ModuleType.MetadataSearch, "Retrying download " + file.DownloadUrl, Log.LogType.debug));
                }
                else
                {
                    file.IsCanceled = true;
                    Invoke(new MethodInvoker(() =>
                    {
                        this.downloadedFileCount++;
                        FilesItem fi = (FilesItem)file.Lvi.Tag;
                        if (fi == null) return;
                        Program.LogThis(new Log(Log.ModuleType.MetadataSearch, $"Document download has been cancelled. Too many retries: {fi.URL}", Log.LogType.debug));
                    }));
                }
            }
            else // successful download
            {
                Invoke(new MethodInvoker(() =>
                {
                    ListViewItem lvi = file.Lvi;
                    listViewDocuments.RemoveEmbeddedControl(file.Pbar);
                    FilesItem fi = (FilesItem)lvi.Tag;
                    if (fi == null) return;
                    fi.Downloaded = true;
                    fi.Date = DateTime.Now;
                    fi.Size = (int)new FileInfo(file.PhysicalPath).Length;
                    bool unknownExtension = String.IsNullOrWhiteSpace(fi.Ext) || !DocumentExtractor.IsSupportedExtension(fi.Ext);

                    if (unknownExtension)
                    {
                        using (var fs = new FileStream(fi.Path, FileMode.Open, FileAccess.Read))
                        {
                            fi.Ext = MetadataExtractCore.Utilities.Functions.GetFormatFile(fs);
                        }

                        if (!string.IsNullOrEmpty(lvi.SubItems[1].Text)) // rename the file
                        {
                            string newFile = GetNotExistsPath(fi.Path + "." + lvi.SubItems[1].Text);
                            File.Move(fi.Path, newFile);
                            fi.Path = newFile;
                            fi.Ext = System.IO.Path.GetExtension(fi.Path);
                        }
                    }

                    listViewDocuments_Update(fi);
                    this.downloadedFileCount++;
                    Program.LogThis(new Log(Log.ModuleType.MetadataSearch, $"Downloaded document: {fi.URL}", Log.LogType.low));
                    this.downloadingFiles.TryRemove(file.DownloadUrl, out Download value);
                    this.UpdateProgressDownloadControls();
                }));
            }
        }

        private void UpdateProgressDownloadControls()
        {
            int totalFiles = this.downloadQueue.Count + this.downloadingFiles.Count + this.downloadedFileCount;
            if (this.downloadQueue.IsEmpty && this.downloadingFiles.IsEmpty)
            {
                Program.FormMainInstance.ReportProgress(0, 100);
                Program.FormMainInstance.toolStripProgressBarDownload.Value = 0;
                Program.FormMainInstance.toolStripStatusLabelLeft.Text = @"All documents have been downloaded";
                Program.LogThis(new Log(Log.ModuleType.MetadataSearch, "All documents have been downloaded", Log.LogType.debug));

                if (this.IsHandleCreated)
                {
                    Invoke(new MethodInvoker(() => { Program.FormMainInstance.toolStripDropDownButtonStop.Enabled = false; }));
                }
            }
            else if (totalFiles > 0)
            {
                Program.FormMainInstance.ReportProgress(this.downloadedFileCount, totalFiles);
                Program.FormMainInstance.toolStripProgressBarDownload.Value = this.downloadedFileCount * 100 / totalFiles;
                Program.FormMainInstance.toolStripStatusLabelLeft.Text = $"Downloading {this.downloadedFileCount}/{totalFiles}";
            }
        }

        private void EnqueueFilestoDownload(List<ListViewItem> urls, string directory)
        {
            if (!Directory.Exists(directory))
            {
                MessageBox.Show("The selected folder does not exist, please check the project configuration - " + directory, "Message for User", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (this.downloadQueue.Count + this.downloadingFiles.Count == 0)
            {
                this.downloadedFileCount = 0;
            }

            bool filesAdded = false;
            foreach (ListViewItem url in urls)
            {
                FilesItem currentFile = (FilesItem)url.Tag;
                if (!currentFile.Downloaded || !File.Exists(currentFile.Path))
                {
                    Download fileDownload = new Download()
                    {
                        DownloadStatus = Download.Status.Enqueued,
                        PhysicalPath = directory,
                        DownloadUrl = url.SubItems[2].Text,
                        Lvi = url,
                        IsCanceled = false
                    };
                    downloadQueue.Enqueue(fileDownload);
                    filesAdded = true;
                }
            }

            if (filesAdded)
            {
                Invoke(new MethodInvoker(() => { Program.FormMainInstance.toolStripDropDownButtonStop.Enabled = true; }));
                this.UpdateProgressDownloadControls();
                this.downloadDelayToken.Cancel();
            }
        }

        private void StopAllDownloads()
        {
            while (!this.downloadQueue.IsEmpty)
            {
                if (this.downloadQueue.TryDequeue(out Download value))
                {
                    this.StopFileDownload(value);
                }
            }

            foreach (var item in this.downloadingFiles)
            {
                this.StopFileDownload(item.Value);
            }

            this.UpdateProgressDownloadControls();
        }

        private void StopFileDownload(Download file)
        {
            file.CaClient?.CancelAsync();
            file.IsCanceled = true;
        }

        /// <summary>
        /// Download files from the queue
        /// </summary>
        private void ProcessDownloadQueue()
        {
            try
            {
                do
                {
                    while (this.downloadingFiles.Count < Program.cfgCurrent.SimultaneousDownloads && downloadQueue.TryDequeue(out Download currentDownload))
                    {
                        FilesItem fi = currentDownload.Lvi.Tag as FilesItem;
                        if (!currentDownload.IsCanceled && fi != null && !fi.Downloaded)
                        {
                            currentDownload.DownloadStatus = Download.Status.Downloading;
                            string fileName = System.IO.Path.GetFileName(new Uri(currentDownload.Lvi.SubItems[2].Text).AbsolutePath);

                            //Delete incorrect filename characters
                            for (var j = 0; fileName.IndexOfAny(MyInvalidPathChars) != -1; j++)
                                fileName = fileName.Replace(MyInvalidPathChars[j], ' ');
                            string downloadPath = GetNotExistsPath(currentDownload.PhysicalPath + fileName);

                            File.Create(downloadPath).Close();

                            fi.Path = downloadPath;
                            currentDownload.PhysicalPath = downloadPath;

                            Invoke(new MethodInvoker(() =>
                            {
                                ProgressBar pb = new ProgressBar
                                {
                                    Top = -1000,
                                    Left = -1000,
                                    ForeColor = Color.Green,
                                    Value = 0
                                };
                                currentDownload.Pbar = pb;
                                listViewDocuments.AddEmbeddedControl(pb, 3, currentDownload.Lvi.Index);
                                listViewDocuments.Refresh();
                            }));
                            currentDownload.CaClient = new CookieAwareWebClient();
                            currentDownload.CaClient.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8");

                            #region moodle requires cookies to download files from it

                            if (currentDownload.DownloadUrl.Contains("/moodle/"))
                            {
                                Program.LogThis(new Log(Log.ModuleType.MetadataSearch, "Moodle application found, adding cookies to download document", Log.LogType.debug));
                                var cc = new CookieContainer();
                                HttpWebRequest req;
                                HttpWebResponse resp;
                                var location = currentDownload.DownloadUrl;
                                do
                                {
                                    req = (HttpWebRequest)WebRequest.Create(location);
                                    req.AllowAutoRedirect = false;
                                    req.CookieContainer = cc;
                                    try
                                    {
                                        resp = (HttpWebResponse)req.GetResponse();
                                    }
                                    catch
                                    {
                                        break;
                                    }
                                    resp.Close();
                                    cc.Add(resp.Cookies);
                                    location = resp.Headers["Location"];
                                } while (resp.StatusCode == HttpStatusCode.SeeOther &&
                                         location != req.RequestUri.ToString());
                                currentDownload.CaClient.m_container = cc;
                            }

                            #endregion

                            currentDownload.CaClient.DownloadFileCompleted += DownloadProgressCompleted;
                            currentDownload.CaClient.DownloadProgressChanged += DownloadProgressChanged;

                            this.downloadingFiles.AddOrUpdate(currentDownload.DownloadUrl, currentDownload, (k, v) => { return v; });

                            CancellationToken cancelT = new CancellationTokenSource(TimeSpan.FromSeconds(10 + currentDownload.Retries * 10)).Token;
                            cancelT.Register(() => currentDownload.CaClient.CancelAsync(), true);
                            currentDownload.CaClient.DownloadFileTaskAsync(new DownloadableUri(currentDownload), currentDownload.PhysicalPath);
                        }
                    }

                    this.downloadDelayToken = new CancellationTokenSource();

                    if (this.downloadQueue.IsEmpty)
                    {
                        try
                        {
                            Task.Delay(TimeSpan.FromSeconds(20), this.downloadDelayToken.Token).Wait();
                        }
                        catch (AggregateException)
                        {
                        }
                    }
                    else
                    {
                        Task.Delay(TimeSpan.FromSeconds(1)).Wait();
                    }

                } while (!this.downloadTaskToken.IsCancellationRequested);
            }
            catch
            {
            }
        }
        #endregion

        public void Abort()
        {
            this.searchCancelToken?.Cancel();
            this.Metadata?.Abort();
            this.Analysis?.Abort();
            this.StopAllDownloads();
        }
    }

    public class Download
    {
        public enum Status
        {
            Enqueued,
            Downloading,
            InProgress
        };

        public CookieAwareWebClient CaClient;
        public Status DownloadStatus;
        public string DownloadUrl;
        public ListViewItem Lvi;
        public ProgressBar Pbar;
        public string PhysicalPath;
        public int Retries;
        public bool IsCanceled;
    }

    public class DownloadableUri : Uri
    {
        public Download File { get; }

        public DownloadableUri(Download downloadFile) : base(downloadFile.DownloadUrl)
        {
            this.File = downloadFile;
        }
    }
}
