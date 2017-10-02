using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FOCA
{
    /// <summary>
    /// This panel shows logs and their available configuration to the user
    /// </summary>
    public partial class PanelLogs : UserControl
    {
        private readonly List<Log> lstLog = new List<Log>();
        public bool ActivePanel = false;
        private bool bAutoScroll = true;
        private bool minimized;

        public PanelLogs()
        {
            InitializeComponent();
            LoadDefaultFilters();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            lstLog.Clear();
            listViewLog.Items.Clear();
        }

        private void buttonSaveToFile_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() != DialogResult.OK) return;
            var filename = saveFileDialog1.FileName;
            var sb = new StringBuilder();
            foreach (ListViewItem lvi in listViewLog.Items)
            {
                foreach (ListViewItem.ListViewSubItem sub in lvi.SubItems)
                {
                    sb.Append($"{sub.Text}\t");
                }
                sb.AppendLine();
            }
            File.AppendAllText(filename, sb.ToString());
        }

        /// <summary>
        /// Add a log to the logs collection
        /// </summary>
        /// <param name="log"></param>
        internal void LogThis(Log log)
        {
            try
            {
                listViewLog.Invoke(new MethodInvoker(delegate
                {
                    try
                    {
                        lstLog.Add(log);

                        if (CheckFilters(log))
                            ShowLogLine(log);
                    }
                    catch
                    {
                    }
                }));
            }
            catch
            {
            }
        }

        private bool CheckFilters(Log log)
        {
            return (from string logItemName in cblbModules.CheckedItems
                where logItemName == log.module.ToString()
                from string logCritically in cblbCritically.CheckedItems
                select logCritically).Any(logCritically => logCritically == log.type.ToString());
        }

        /// <summary>
        /// Show a log from the logs collection
        /// </summary>
        /// <param name="log"></param>
        private void ShowLogLine(Log log)
        {
            lock (listViewLog.Items)
            {
                var lvi = listViewLog.Items.Add(log.time);
                lvi.SubItems.Add(log.module.ToString());
                lvi.SubItems.Add(log.type.ToString());
                lvi.SubItems.Add(log.text);
                if (bAutoScroll)
                    lvi.EnsureVisible();
            }
        }

        /// <summary>
        /// Enable or disable logs autoscroll
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAutoScroll_Click(object sender, EventArgs e)
        {
            buttonAutoScroll.Text = (buttonAutoScroll.Text.Equals("Activate AutoScroll"))
                ? "Deactivate AutoScroll"
                : "Activate AutoScroll";
            bAutoScroll = !bAutoScroll;
        }

        /// <summary>
        ///     Ensure that autoscroll is done when showing the listview for the first time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelLogs_VisibleChanged(object sender, EventArgs e)
        {
            if (!bAutoScroll || listViewLog.Items.Count <= 0) return;
            listViewLog.EnsureVisible(listViewLog.Items.Count - 1);
            listViewLog.EnsureVisible(listViewLog.Items.Count - 1);
        }

        private void PanelLogs_Load(object sender, EventArgs e)
        {
            btShow_Click(null, null);
        }

        /// <summary>
        /// Restore logs configuration to default values
        /// </summary>
        public void LoadDefaultFilters()
        {
            foreach (var name in Enum.GetNames(typeof (Log.ModuleType)))
                cblbModules.Items.Add(name, CheckState.Checked);

            foreach (var name in Enum.GetNames(typeof (Log.LogType)))
            {
                if (name != null && (name == Log.LogType.debug.ToString() || name == Log.LogType.low.ToString()))
                    cblbCritically.Items.Add(name, CheckState.Unchecked);
                else if (name != null) cblbCritically.Items.Add(name, CheckState.Checked);
            }
        }

        private void cblbModules_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            cblbModules.Enabled = false;
            cblbCritically.Enabled = false;

            listViewLog.BeginUpdate();
            listViewLog.Items.Clear();

            foreach (var log in lstLog)
            {
                if (sender == cblbModules)
                {
                    if (ModuleFiltered(log, e) || CriticallyFiltered(log, null)) continue;
                    var lvi = new ListViewItem(log.time);
                    lvi.SubItems.Add(log.module.ToString());
                    lvi.SubItems.Add(log.type.ToString());
                    lvi.SubItems.Add(log.text);
                    listViewLog.Items.Add(lvi);
                }
                else if (sender == cblbCritically)
                {
                    if (ModuleFiltered(log, null) || CriticallyFiltered(log, e)) continue;
                    var lvi = new ListViewItem(log.time);
                    lvi.SubItems.Add(log.module.ToString());
                    lvi.SubItems.Add(log.type.ToString());
                    lvi.SubItems.Add(log.text);
                    listViewLog.Items.Add(lvi);
                }
            }
            listViewLog.EndUpdate();

            cblbModules.Enabled = true;
            cblbCritically.Enabled = true;
        }

        private bool CriticallyFiltered(Log log, ItemCheckEventArgs e)
        {
            if (e == null)
                return
                    (from object t in cblbCritically.CheckedItems select t.ToString()).All(
                        crit => log.type.ToString() != crit);
            var criticidadActual = cblbCritically.Items[e.Index].ToString();
            if (criticidadActual == log.type.ToString())
            {
                return e.NewValue != CheckState.Checked;
            }

            return
                (from object t in cblbCritically.CheckedItems select t.ToString()).All(
                    crit => log.type.ToString() != crit);
        }

        private bool ModuleFiltered(Log log, ItemCheckEventArgs e)
        {
            if (e == null)
                return
                    (from object t in cblbModules.CheckedItems select t.ToString()).All(
                        modulo => log.module.ToString() != modulo);
            var moduloActual = cblbModules.Items[e.Index].ToString();
            if (moduloActual != log.module.ToString())
                return
                    (from object t in cblbModules.CheckedItems select t.ToString()).All(
                        modulo => log.module.ToString() != modulo);
            return e.NewValue != CheckState.Checked;
        }

        private void lbCheckAllModule_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < cblbModules.Items.Count; i++)
                cblbModules.SetItemChecked(i, true);
        }

        private void lbUncheckAllModule_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < cblbModules.Items.Count; i++)
                cblbModules.SetItemChecked(i, false);
        }

        private void lbCheckAllCriti_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < cblbCritically.Items.Count; i++)
                cblbCritically.SetItemChecked(i, true);
        }

        private void lbUnCheckAllCriti_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < cblbCritically.Items.Count; i++)
                cblbCritically.SetItemChecked(i, false);
        }

        private void Colorea(object sender, EventArgs e)
        {
            var l = (Label) sender;
            l.ForeColor = Color.Red;
        }

        private void Descolorea(object sender, EventArgs e)
        {
            var l = (Label) sender;
            l.ForeColor = Color.Blue;
        }

        private void btShow_Click(object sender, EventArgs e)
        {
            if (!minimized)
            {
                panelFilter.Width = 0;
                gbCriticidad.Visible = false;
                gbModulos.Visible = false;

                PanelInfo.Location = new Point(0, PanelInfo.Location.Y);
                minimized = !minimized;
            }
            else
            {
                panelFilter.Width = 265;
                gbCriticidad.Visible = true;
                gbModulos.Visible = true;
                minimized = !minimized;
            }
        }
    }
}