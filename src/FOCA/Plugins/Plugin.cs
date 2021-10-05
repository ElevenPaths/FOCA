using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using PluginsAPI;
using PluginsAPI.Elements;
using PluginsAPI.Elements.ContextualMenu;

namespace FOCA.Plugins
{
    public class Plugin
    {
        public int Id { get; set; }
        public string description { get; set; }
        // general description of the plugin
        public string name { get; set; }
        // namespace where the plugin is stored
        public string _namespace { get; set; }
        // internal plugin properties (uses reflexion)
        public Assembly assembly;
        // path where the plugin is stored
        public string path { get; set; }
        // instance of the plugin
        public object pluginInstance { get; set; }
        // type of the plugin
        public Type pluginType;

        public Plugin()
        {
                
        }

        /// <summary>
        /// Plugin constructor
        /// </summary>
        /// <param name="path">Path where the plugin is stored</param>
        public Plugin(string path)
        {
            description = string.Empty;
            name = string.Empty;
            _namespace = string.Empty;
            this.path = path;

            try
            {
                assembly = Assembly.LoadFile(path);

                _namespace = GetNameSpace();

                pluginType = assembly.GetType(_namespace + ".Plugin");
                pluginInstance = Activator.CreateInstance(pluginType);

                PropertyInfo pInfo;

                pInfo = pluginType.GetProperty("name");
                name = (string) pInfo.GetValue(pluginInstance, null);


                Export export;
                pInfo = pluginType.GetProperty("exportItems");
                export = (Export) pInfo.GetValue(pluginInstance, null);

                pInfo = pluginType.GetProperty("description");
                description = (string) pInfo.GetValue(pluginInstance, null);

                ImportElements(export);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtain the namespace of a plugin
        /// </summary>
        /// <returns></returns>
        public string GetNameSpace()
        {
            return assembly.GetTypes().Select(t => t.ToString().Split('.')[0]).FirstOrDefault();
        }

        private static void ImportElements(Export elements)
        {
            foreach (var o in elements.Items())
            {
                // Plugin's main panel
                var panel = o as PluginPanel;
                if (panel != null)
                {
                    var p = panel;
                    p.panel.Dock = DockStyle.Fill;
                    if (p.fullWindow)
                        Program.FormMainInstance.Controls.Add(p.panel);
                    else
                        Program.FormMainInstance.splitContainerMain.Panel2.Controls.Add(p.panel);
                }
                // add it to the plugins tab
                else
                {
                    var item = o as PluginToolStripMenuItem;
                    if (item != null)
                    {
                        var t = item;
                        Program.FormMainInstance.pluginsToolStripMenuItem.DropDownItems.Add(t.item);
                    }
                    // contextual menu
                    else if (o is Global)
                        Program.FormMainInstance.ManagePluginsApi.lstContextGlobal.Add((Global) o);
                    else if (o is ShowDomainsDomainItemMenu)
                        Program.FormMainInstance.ManagePluginsApi.lstContextShowDomainsDomainItemMenu.Add(
                            (ShowDomainsDomainItemMenu) o);
                    else if (o is ShowDomainsDomainMenu)
                        Program.FormMainInstance.ManagePluginsApi.lstContextShowDomainsDomainMenu.Add(
                            (ShowDomainsDomainMenu) o);
                    else if (o is ShowDomainsDomainRelatedDomainsItemMenu)
                        Program.FormMainInstance.ManagePluginsApi.lstContextShowDomainsDomainRelatedDomainsItemMenu.Add(
                            (ShowDomainsDomainRelatedDomainsItemMenu) o);
                    else if (o is ShowDomainsDomainRelatedDomainsMenu)
                        Program.FormMainInstance.ManagePluginsApi.lstContextShowDomainsDomainRelatedDomainsMenu.Add(
                            (ShowDomainsDomainRelatedDomainsMenu) o);
                    else if (o is ShowDomainsMenu)
                        Program.FormMainInstance.ManagePluginsApi.lstContextShowDomainsMenu.Add((ShowDomainsMenu) o);
                    else if (o is ShowMetadataMenu)
                        Program.FormMainInstance.ManagePluginsApi.lstContextShowMetadataMenu.Add((ShowMetadataMenu) o);
                    else if (o is ShowNetworkClientsItemMenu)
                        Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkClientsItemMenu.Add(
                            (ShowNetworkClientsItemMenu) o);
                    else if (o is ShowNetworkClientsMenu)
                        Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkClientsMenu.Add(
                            (ShowNetworkClientsMenu) o);
                    else if (o is ShowNetworkIpRangeMenu)
                        Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkIpRangeMenu.Add(
                            (ShowNetworkIpRangeMenu) o);
                    else if (o is ShowNetworkMenu)
                        Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkMenu.Add((ShowNetworkMenu) o);
                    else if (o is ShowNetworkServersItemMenu)
                        Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkServersItemMenu.Add(
                            (ShowNetworkServersItemMenu) o);
                    else if (o is ShowNetworkServersMenu)
                        Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkServersMenu.Add(
                            (ShowNetworkServersMenu) o);
                    else if (o is ShowNetworkUnlocatedItemMenu)
                        Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkUnlocatedItemMenu.Add(
                            (ShowNetworkUnlocatedItemMenu) o);
                    else if (o is ShowNetworkUnlocatedMenu)
                        Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkUnlocatedMenu.Add(
                            (ShowNetworkUnlocatedMenu) o);
                    else if (o is ShowProjectMenu)
                        Program.FormMainInstance.ManagePluginsApi.lstContextShowProjectMenu.Add((ShowProjectMenu) o);
                }
            }
        }

        public override string ToString()
        {
            return name + " [" + description + "]";
        }
    }
}