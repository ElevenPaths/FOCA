using FOCA.Database.Entities;
using FOCA.Properties;
using FOCA.Search;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FOCA
{
    /// <summary>
    ///     This panel describes and shows information about a target host. This information includes:
    ///     - Hostname
    ///     - Operating System (if it was identified)
    ///     - Running software (if any was found)
    ///     - Language
    ///     - IP Address
    /// </summary>
    public partial class PanelInformation : UserControl
    {
        private static ListViewItem last;
        private bool hiddenPanelBotom;
        public Thread tSearch;

        public PanelInformation()
        {
            InitializeComponent();
            picFOCA.Visible = false;
            splitPanel.Height += splitPanel.Top - 11;
            splitPanel.Top = 11;
        }

        private void ResizePanel()
        {
            if (hiddenPanelBotom)
                splitPanel.SplitterDistance = splitPanel.Size.Height - 57;
        }

        private void btHide_Click(object sender, EventArgs e)
        {
            if (hiddenPanelBotom)
            {
                hiddenPanelBotom = false;
                btnHide.Text = @"Minimize";
                splitPanel.SplitterDistance = (splitPanel.Size.Height / 3);
                btnHide.Image = Resources.arrowDown;

                for (var i = 0; i < splitPanel.Panel2.Controls.Count; i++)
                {
                    if (
                        !string.Equals(splitPanel.Panel2.Controls[i].ToString(), "FOCA.PanelInformationOptions",
                            StringComparison.CurrentCultureIgnoreCase))
                        continue;
                    var pio = (PanelInformationOptions)splitPanel.Panel2.Controls[i];
                    pio.Visible = true;
                    return;
                }
            }
            else
            {
                hiddenPanelBotom = true;
                ResizePanel();
                btnHide.Text = @"Maximize";
                btnHide.Image = Resources.arrowUp;

                for (var i = 0; i < splitPanel.Panel2.Controls.Count; i++)
                {
                    if (
                        !string.Equals(splitPanel.Panel2.Controls[i].ToString(), "FOCA.PanelInformationOptions",
                            StringComparison.CurrentCultureIgnoreCase))
                        continue;
                    var pio = (PanelInformationOptions)splitPanel.Panel2.Controls[i];
                    pio.Visible = false;
                    return;
                }
            }
        }

        private void splitPanel_Panel2_Resize(object sender, EventArgs e)
        {
            ResizePanel();
        }

        private void PanelInformation_SizeChanged(object sender, EventArgs e)
        {
            ResizePanel();
        }

        private void splitPanel_Panel1_Resize(object sender, EventArgs e)
        {
            ResizePanel();
        }

        private void listViewInformation_Click(object sender, EventArgs e)
        {
            if (lvwInformation.SelectedItems.Count != 1) return;
            var textoSeleccionado = lvwInformation.SelectedItems[0].Text;

            if (Program.data.GetDomain(textoSeleccionado.Split(':')[0]) == null) return;
            splitPanel.Panel2Collapsed = false;
            var dom = textoSeleccionado.Split(':')[0];
            Program.FormMainInstance.UpdateBottomPanel(Program.data.GetDomain(dom));
            panelInformationOptions.LoadDomain(Program.data.GetDomain(dom));
        }

        private void PanelInformation_Load(object sender, EventArgs e)
        {
            splitPanel.SplitterDistance = (splitPanel.Size.Height / 3);
        }

        private void openUrlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvwInformation.SelectedItems.Count != 1 || lvwInformation.SelectedItems[0].SubItems.Count <= 0) return;
            var columnValue = lvwInformation.SelectedItems[0].SubItems[0].Text + " " +
                              lvwInformation.SelectedItems[0].SubItems[1].Text;
            var urls = columnValue.Split(' ');
            var found = false;

            foreach (var url in urls)
            {
                try
                {
                    var parsedUrl = url;
                    if (parsedUrl.ToUpper().Equals("URL"))
                    {
                        parsedUrl = columnValue.Substring(4, columnValue.Length - 4);
                    }
                    Process.Start(parsedUrl);
                    found = true;
                    return;
                }
                catch
                {
                }
            }

            if (found) return;
            foreach (var url in urls)
            {
                var urlDom = url.Split(':')[0];
                var puerto = 80;
                try
                {
                    puerto = int.Parse(url.Split(':')[1]);
                }
                catch
                {
                }

                if (Program.data.GetDomain(urlDom) == null) continue;

                var pre = (puerto == 443) ? "https://" : "http://";
                Process.Start(pre + urlDom + ":" + puerto);
                return;
            }
        }

        private void contextMenuStripExport_Opened(object sender, EventArgs e)
        {
            try
            {
                contextMenuStripExport.Items["searchDocumentsWhereAppearsValueToolStripMenuItem"].Visible =
                    lvwInformation.Groups.Count > 0 &&
                    (lvwInformation.Groups[0].Header.StartsWith("All users found") ||
                     lvwInformation.Groups[0].Header.StartsWith("All folders found") ||
                     lvwInformation.Groups[0].Header.StartsWith("All printers found") ||
                     lvwInformation.Groups[0].Header.StartsWith("All software found") ||
                     lvwInformation.Groups[0].Header.StartsWith("All emails found") ||
                     lvwInformation.Groups[0].Header.StartsWith("All operating systems found"));
                contextMenuStripExport.Items["searchDocumentsWhereAppearsValueToolStripMenuItem"].Enabled =
                    lvwInformation.SelectedItems.Count == 1 &&
                    lvwInformation.SelectedItems[0].Text != @"No users found" &&
                    lvwInformation.SelectedItems[0].Text != @"No folders found" &&
                    lvwInformation.SelectedItems[0].Text != @"No printers found" &&
                    lvwInformation.SelectedItems[0].Text != @"No software found" &&
                    lvwInformation.SelectedItems[0].Text != @"No emails found" &&
                    lvwInformation.SelectedItems[0].Text != @"No operating systems found";
            }
            catch (Exception)
            {
                contextMenuStripExport.Items["searchDocumentsWhereAppearsValueToolStripMenuItem"].Visible = false;
            }
        }

        #region Components events

        private void listViewInformacion_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            var lvwColumnSorterV = (ListViewColumnSorterValues)lvwInformation.Tag;
            if (e.Column == lvwColumnSorterV.SortColumn)
            {
                lvwColumnSorterV.Order = lvwColumnSorterV.Order == SortOrder.Ascending
                    ? SortOrder.Descending
                    : SortOrder.Ascending;
            }
            else
            {
                lvwColumnSorterV.SortColumn = e.Column;
                lvwColumnSorterV.Order = SortOrder.Ascending;
            }
            lvwInformation.Sort();
        }

        private void listViewInformation_MouseMove(object sender, MouseEventArgs e)
        {
            var lstView = (ListView)sender;
            var lv = lstView.GetItemAt(e.X, e.Y);
            if (lv == last) return;
            last = lv;
            if (string.IsNullOrEmpty((string)lv?.Tag))
                toolTip.SetToolTip(lstView, string.Empty);
            else
            {
                toolTip.SetToolTip(lstView, (string)lv.Tag);
            }
        }

        #endregion

        #region context menu export events

        private void toolStripMenuItemExport_Click(object sender, EventArgs e)
        {
            if (sfdExport.ShowDialog() != DialogResult.OK) return;
            StringBuilder output = new StringBuilder();
            if (lvwInformation.Groups.Count > 0)
            {
                foreach (ListViewGroup lvg in lvwInformation.Groups)
                {
                    output.AppendLine(lvg.Header + ":");
                    foreach (ListViewItem lvi in lvg.Items)
                    {
                        for (var i = 0; i < lvi.SubItems.Count; i++)
                        {
                            output.Append(lvi.SubItems[i].Text + "\t");
                        }
                        output.AppendLine();
                    }
                }
            }
            else
            {
                foreach (ListViewItem lvi in lvwInformation.Items)
                {
                    for (var i = 0; i < lvi.SubItems.Count; i++)
                    {
                        output.Append(lvi.SubItems[i].Text + "\t");
                    }
                    output.AppendLine();
                }
            }
            File.WriteAllText(sfdExport.FileName, output.ToString());
        }

        private void searchDocumentsWhereAppearsValueToolStripMenuItem_Click(object sender, EventArgs eArgs)
        {
            string valueToSearch = lvwInformation?.SelectedItems[0]?.SubItems[1]?.Text?.Trim();
            if (lvwInformation.Groups.Count == 0 || String.IsNullOrWhiteSpace(valueToSearch))
                return;

            FormDocumentsSearch formSearchInstance = new FormDocumentsSearch(ParentForm)
            {
                Text = $"Documents found with value '{valueToSearch}'"
            };

            Func<FilesItem, bool> searchPredicate = null;

            if (lvwInformation.Groups[0].Header.StartsWith("All users found"))
            {
                searchPredicate = p => p.Metadata?.FoundUsers?.Items.Any(q => String.Equals(q.Name, valueToSearch, StringComparison.OrdinalIgnoreCase)) ?? false;
            }
            else if (lvwInformation.Groups[0].Header.StartsWith("All folders found"))
            {
                searchPredicate = p => p.Metadata?.FoundPaths?.Items.Any(q => String.Equals(q.Path, valueToSearch, StringComparison.OrdinalIgnoreCase)) ?? false;
            }
            else if (lvwInformation.Groups[0].Header.StartsWith("All printers found"))
            {
                searchPredicate = p => p.Metadata?.FoundPrinters?.Items.Any(q => String.Equals(q.Printer, valueToSearch, StringComparison.OrdinalIgnoreCase)) ?? false;
            }
            else if (lvwInformation.Groups[0].Header.StartsWith("All software found"))
            {
                searchPredicate = p => p.Metadata?.FoundMetaData?.Applications?.Items.Any(q => String.Equals(q.Name, valueToSearch, StringComparison.OrdinalIgnoreCase)) ?? false;
            }
            else if (lvwInformation.Groups[0].Header.StartsWith("All emails found"))
            {
                searchPredicate = p => p.Metadata?.FoundEmails?.Items.Any(q => String.Equals(q.Mail, valueToSearch, StringComparison.OrdinalIgnoreCase)) ?? false;
            }
            else if (lvwInformation.Groups[0].Header.StartsWith("All operating systems found"))
            {
                searchPredicate = p => p.Metadata?.FoundMetaData?.OperativeSystem?.Equals(valueToSearch, StringComparison.OrdinalIgnoreCase) ?? false;
            }
            else
            {
                searchPredicate = p => false;
            }

            string[] foundElements = Program.data.files.Items.Where(searchPredicate).Select(p => p.Path).ToArray();
            formSearchInstance.lstDocumentsFound.Items.AddRange(new ListBox.ObjectCollection(formSearchInstance.lstDocumentsFound, foundElements));

            formSearchInstance.Show();
        }

        #endregion
    }
}