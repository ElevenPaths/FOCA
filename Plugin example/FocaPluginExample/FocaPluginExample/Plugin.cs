using PluginsAPI;
using PluginsAPI.Elements;
using PluginsAPI.Elements.ContextualMenu;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace FocaPluginExample
{
    public class Plugin
    {
        private string _name = "Foca Plugin Example";
        private string _description = "Description Foca Plugin Example";
        private FrmPluginExample main = new FrmPluginExample();
        
        public static FrmPluginExample mainForm;
        private Export export;
        public static bool end;

        public Export exportItems
        {
            get { return this.export; }
        }

        public string name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        public string description
        {
            get { return this._description; }
            set { this._description = value; }
        }

        public Plugin()
        {
            if (Plugin.mainForm == null)
                Plugin.mainForm = new FrmPluginExample();
            this.export = new Export();
            PluginPanel pluginPanel = new PluginPanel(this.main.panel, false);
            ToolStripMenuItem toolStripMenuItem1 = new ToolStripMenuItem(this._name);
            toolStripMenuItem1.Image = Properties.Resources.foca_img;
            PluginToolStripMenuItem toolStripMenuItem2 = new PluginToolStripMenuItem(toolStripMenuItem1);
            toolStripMenuItem1.Click += (EventHandler)((param0, param1) =>
            {
                this.main.panel.BringToFront();
                this.main.panel.Visible = true;
            });
            this.export.Add((object)pluginPanel);
            this.export.Add((object)toolStripMenuItem2);
            this.AssociateContextualMenu(this.export);
        }

        private void AssociateContextualMenu(Export export)
        {
            //ShowVulnerabilitiesVulnerabilitieMenuItem vulnerabilitieMenuItem =
            //    new ShowVulnerabilitiesVulnerabilitieMenuItem(this.main.sVNExtractorToolStripMenuItem, (keyType) 14);
            //export.Add((object) vulnerabilitieMenuItem);
        }
    }
}
