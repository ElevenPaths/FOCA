using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using FOCA.Analysis.HttpMap;
using FOCA.ModifiedComponents;
using FOCA.Search;

namespace FOCA
{
    public partial class PanelUrlsList : UserControl
    {
        public string Domain;

        public PanelUrlsList()
        {
            InitializeComponent();
            lstView.ListViewItemSorter = new ListViewColumnSorterValues();
            Domain = string.Empty;
        }

        private void lstView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var listView = sender as ListView;
            if (listView != null)
            {
                var lvwColumnSorter = (ListViewColumnSorterValues) listView.ListViewItemSorter;

                if (e.Column == lvwColumnSorter.SortColumn)
                {
                    lvwColumnSorter.Order = lvwColumnSorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
                }
                else
                {
                    lvwColumnSorter.SortColumn = e.Column;
                    lvwColumnSorter.Order = SortOrder.Ascending;
                }
            }
            var listViewEx = sender as ListViewEx;
            listViewEx?.Sort();
        }

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            exportDataToFileToolStripMenuItem.Enabled = lstView.Items.Count > 0;
            openInBrowserToolStripMenuItem.Enabled = lstView.SelectedItems.Count > 0;
            searchForBackupsToolStripMenuItem.Enabled = lstView.Items.Count > 0;
            viewDownloadedDocumentToolStripMenuItem.Visible = lstView.SelectedItems.Count == 1 &&
                                                              Program.FormMainInstance.panelInformation.tabMap
                                                                  .SelectedTab.Name == "Documents published";
            viewDownloadedDocumentToolStripMenuItem.Enabled = lstView.SelectedItems.Count == 1 &&
                                                              Program.FormMainInstance.panelInformation.tabMap
                                                                  .SelectedTab.Name == "Documents published" &&
                                                              Program.data.files.Items.Any(
                                                                  F =>
                                                                      F.URL.Equals(lstView.SelectedItems[0].Text,
                                                                          StringComparison.OrdinalIgnoreCase) &&
                                                                      F.MetadataExtracted);
        }

        private void viewDownloadedDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Program.data.files.Items.Any(
                F =>
                    string.Equals(F.URL, lstView.SelectedItems[0].Text, StringComparison.OrdinalIgnoreCase) &&
                    F.MetadataExtracted)) return;
            var fi =
                Program.data.files.Items.First(
                    F =>
                        string.Equals(F.URL, lstView.SelectedItems[0].Text, StringComparison.OrdinalIgnoreCase) &&
                        F.MetadataExtracted);

            BringToFront();
            var tnSearched = Program.FormMainInstance.TreeViewMetadataSearchDocument(fi.Path);
            if (tnSearched == null) return;

            if (tnSearched == Program.FormMainInstance.TreeView.SelectedNode)
            {
                Program.FormMainInstance.TreeViewProjectAfterSelect(null,
                    new TreeViewEventArgs(Program.FormMainInstance.TreeView.SelectedNode));
                tnSearched.EnsureVisible();
                tnSearched.ForeColor = SystemColors.ActiveCaption;
            }
            else
            {
                Program.FormMainInstance.TreeView.SelectedNode = tnSearched;
                tnSearched.EnsureVisible();
                tnSearched.ForeColor = SystemColors.ActiveCaption;
            }
        }

        private void exportDataToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sfdExport.ShowDialog() != DialogResult.OK) return;
            var valores = string.Empty;
            foreach (ListViewItem lvi in lstView.Items)
            {
                for (var i = 0; i < lvi.SubItems.Count; i++)
                {
                    var lvsi = lvi.SubItems[i];
                    valores += lvsi.Text + "\t";
                }
                valores += Environment.NewLine;
            }
            File.WriteAllText(sfdExport.FileName, valores);
        }

        private void openInBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var lvi in lstView.SelectedItems.Cast<ListViewItem>().Where(lvi => Uri.IsWellFormedUriString(lvi.Text, UriKind.Absolute)))
            {
                Process.Start(lvi.Text);
                Program.LogThis(new Log(Log.ModuleType.FOCA, $"Opening document {lvi.Text}",
                    Log.LogType.debug));
            }
        }

        private void searchForBackupsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var url = "";

            foreach (var lvi in lstView.SelectedItems.Cast<ListViewItem>().Where(lvi => Uri.IsWellFormedUriString(lvi.Text, UriKind.Absolute)))
            {
                url = lvi.Text;
            }

            var backupSearcher = new FormBackupsFuzzer(url);
            backupSearcher.Show();
        }
    }
}