using FOCA.Plugins;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace FOCA
{
    public partial class FormPlugins : Form
    {
        public FormPlugins()
        {
            InitializeComponent();
            RenderPlugins();
        }

        private void btLoadPlugin_Click(object sender, EventArgs e)
        {
            ShowLoadPluginGUI();
            UpdateAndSave();
        }

        /// <summary>
        ///     Show form to select the plugin from file system
        /// </summary>
        private void ShowLoadPluginGUI()
        {
            var ofd = new OpenFileDialog();

            if (ofd.ShowDialog() != DialogResult.OK) return;
            try
            {
                var exist = Program.data.plugins.lstPlugins.Count(x => x.path == ofd.FileName);
                if (exist == 0)
                {
                    var p = new Plugin(ofd.FileName);
                    Program.data.plugins.AddPlugin(p);
                }
                else
                    MessageBox.Show("The plugin you are trying to add already exists.", "Foca Open Source");


            }
            catch (Exception)
            {
                MessageBox.Show("Error importing plugin.");
            }
        }

        /// <summary>
        /// Save current plugins configuration in FOCA configuration
        /// </summary>
        private void SaveConfig()
        {
            var pluginsPaths = new string[Program.data.plugins.Count()];

            Program.cfgCurrent.SPathsPlugins = string.Empty;
            for (var i = 0; i < pluginsPaths.Length; i++)
            {
                Program.cfgCurrent.SPathsPlugins += Program.data.plugins.GetPlugin(i).path;
                if (i + 1 < pluginsPaths.Length)
                    Program.cfgCurrent.SPathsPlugins += "|";
            }
        }

        /// <summary>
        ///     Show currently loaded plugins
        /// </summary>
        public void RenderPlugins()
        {
            var lstPlugins = Program.data.plugins.GetPluginsLoaded();
            this.lstPlugins.Items.Clear();
            foreach (var plg in lstPlugins)
            {
                this.lstPlugins.Items.Add(plg);
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLoadPluginGUI();
            UpdateAndSave();
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var plugin = lstPlugins.Items[lstPlugins.SelectedIndex] as Plugin;
            Program.data.plugins.RemovePlugin(plugin);
            UpdateAndSave();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            removeToolStripMenuItem.Enabled = lstPlugins.SelectedIndex != -1;
        }

        /// <summary>
        /// Render the plugins and save the configuration
        /// </summary>
        private void UpdateAndSave()
        {
            RenderPlugins();
            SaveConfig();
        }
    }
}