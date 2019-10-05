using System;
using System.Linq;
using System.Threading;
using FOCA.ModifiedComponents;
using FOCA.TaskManager;
using PluginsAPI;
using PluginsAPI.Elements.ContextualMenu;
using PluginsAPI.ImportElements;

namespace FOCA.Plugins
{
    /// <summary>
    ///     Manage plugin calls to the API
    /// </summary>
    public class ManagePluginCalls : IDisposable
    {
        public ThreadSafeList<Global> lstContextGlobal;
        public ThreadSafeList<ShowDomainsDomainItemMenu> lstContextShowDomainsDomainItemMenu;
        public ThreadSafeList<ShowDomainsDomainMenu> lstContextShowDomainsDomainMenu;
        public ThreadSafeList<ShowDomainsDomainRelatedDomainsItemMenu> lstContextShowDomainsDomainRelatedDomainsItemMenu;
        public ThreadSafeList<ShowDomainsDomainRelatedDomainsMenu> lstContextShowDomainsDomainRelatedDomainsMenu;
        public ThreadSafeList<ShowDomainsMenu> lstContextShowDomainsMenu;
        public ThreadSafeList<ShowMetadataMenu> lstContextShowMetadataMenu;
        public ThreadSafeList<ShowNetworkClientsItemMenu> lstContextShowNetworkClientsItemMenu;
        public ThreadSafeList<ShowNetworkClientsMenu> lstContextShowNetworkClientsMenu;
        public ThreadSafeList<ShowNetworkIpRangeMenu> lstContextShowNetworkIpRangeMenu;
        public ThreadSafeList<ShowNetworkMenu> lstContextShowNetworkMenu;
        public ThreadSafeList<ShowNetworkServersItemMenu> lstContextShowNetworkServersItemMenu;
        public ThreadSafeList<ShowNetworkServersMenu> lstContextShowNetworkServersMenu;
        public ThreadSafeList<ShowNetworkUnlocatedItemMenu> lstContextShowNetworkUnlocatedItemMenu;
        public ThreadSafeList<ShowNetworkUnlocatedMenu> lstContextShowNetworkUnlocatedMenu;
        public ThreadSafeList<ShowProjectMenu> lstContextShowProjectMenu;

        /// <summary>
        ///     Manage plugin calls to any contextual menu
        /// </summary>
        public ManagePluginCalls()
        {
            lstContextGlobal = new ThreadSafeList<Global>();
            lstContextShowDomainsDomainItemMenu = new ThreadSafeList<ShowDomainsDomainItemMenu>();
            lstContextShowDomainsDomainMenu = new ThreadSafeList<ShowDomainsDomainMenu>();
            lstContextShowDomainsDomainRelatedDomainsItemMenu =
                new ThreadSafeList<ShowDomainsDomainRelatedDomainsItemMenu>();
            lstContextShowDomainsDomainRelatedDomainsMenu = new ThreadSafeList<ShowDomainsDomainRelatedDomainsMenu>();
            lstContextShowDomainsMenu = new ThreadSafeList<ShowDomainsMenu>();
            lstContextShowMetadataMenu = new ThreadSafeList<ShowMetadataMenu>();
            lstContextShowNetworkClientsItemMenu = new ThreadSafeList<ShowNetworkClientsItemMenu>();
            lstContextShowNetworkClientsMenu = new ThreadSafeList<ShowNetworkClientsMenu>();
            lstContextShowNetworkIpRangeMenu = new ThreadSafeList<ShowNetworkIpRangeMenu>();
            lstContextShowNetworkMenu = new ThreadSafeList<ShowNetworkMenu>();
            lstContextShowNetworkServersItemMenu = new ThreadSafeList<ShowNetworkServersItemMenu>();
            lstContextShowNetworkServersMenu = new ThreadSafeList<ShowNetworkServersMenu>();
            lstContextShowNetworkUnlocatedItemMenu = new ThreadSafeList<ShowNetworkUnlocatedItemMenu>();
            lstContextShowNetworkUnlocatedMenu = new ThreadSafeList<ShowNetworkUnlocatedMenu>();
            lstContextShowProjectMenu = new ThreadSafeList<ShowProjectMenu>();
        }

        /// <summary>
        ///     Allows plugins to perform calls
        /// </summary>
        public void EnablePluginCalls()
        {
            Import.ImportEvent += FocaCall;
        }

        /// <summary>
        ///     Identifies a call and performs it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FocaCall(object sender, EventArgs e)
        {
            var iObject = (ImportObject)sender;

            switch (iObject.operation)
            {
                case Import.Operation.AssociationDomainIp:
                    {
                        AddAssociationDomainIp((AssociationDomainIP)iObject.o);
                    }
                    break;
                case Import.Operation.AddDomain:
                    {
                        AddDomain(iObject.o.ToString());
                    }
                    break;
                case Import.Operation.AddIp:
                    {
                        AddIp(iObject.o.ToString());
                    }
                    break;
                case Import.Operation.AddUrl:
                    {
                        var url = iObject.o.ToString();
                        var error = false;
                        try
                        {
                            var uri = new Uri(url);
                            var domain = Program.data.GetDomain(uri.Host);
                            if (domain == null)
                            {
                                Program.data.AddDomain(uri.Host, "Plugins", Program.cfgCurrent.MaxRecursion,
                                    Program.cfgCurrent);
                            }
                            else
                            {
                                domain = Program.data.GetDomain(uri.Host);
                                if (domain == null)
                                    error = true;
                            }
                        }
                        catch
                        {
                            error = true;
                        }

                        if (!error)
                        {
                            var tAddUrl = new Thread(AddUrlAsync);
                            var taskAddUrl = new TaskFOCA(tAddUrl, iObject.o, "[Plugin] Add url: " + url);
                            Program.data.tasker.AddTask(taskAddUrl);
                        }
                    }
                    break;

                case Import.Operation.AddContextMenu:
                    {
                        switch (iObject.o)
                        {
                            case Global global:
                                lstContextGlobal.Add(global);
                                break;
                            case ShowDomainsDomainItemMenu showDomainsDomainItem:
                                lstContextShowDomainsDomainItemMenu.Add(showDomainsDomainItem);
                                break;
                            case ShowDomainsDomainMenu showDomainsDomainMenu:
                                lstContextShowDomainsDomainMenu.Add(showDomainsDomainMenu);
                                break;
                            case ShowDomainsDomainRelatedDomainsItemMenu showDomainsDomainRelatedDomainsItemMenu:
                                lstContextShowDomainsDomainRelatedDomainsItemMenu.Add(showDomainsDomainRelatedDomainsItemMenu);
                                break;
                            case ShowDomainsDomainRelatedDomainsMenu showDomainsDomainRelatedDomainsMenu:
                                lstContextShowDomainsDomainRelatedDomainsMenu.Add(showDomainsDomainRelatedDomainsMenu);
                                break;
                            case ShowDomainsMenu showDomainsMenu:
                                lstContextShowDomainsMenu.Add(showDomainsMenu);
                                break;
                            case ShowMetadataMenu showMetadataMenu:
                                lstContextShowMetadataMenu.Add(showMetadataMenu);
                                break;
                            case ShowNetworkClientsItemMenu showNetworkClientsItemMenu:
                                lstContextShowNetworkClientsItemMenu.Add(showNetworkClientsItemMenu);
                                break;
                            case ShowNetworkClientsMenu showNetworkClientsMenu:
                                lstContextShowNetworkClientsMenu.Add(showNetworkClientsMenu);
                                break;
                            case ShowNetworkIpRangeMenu showNetworkIpRangeMenu:
                                lstContextShowNetworkIpRangeMenu.Add(showNetworkIpRangeMenu);
                                break;
                            case ShowNetworkMenu showNetworkMenu:
                                lstContextShowNetworkMenu.Add(showNetworkMenu);
                                break;
                            case ShowNetworkServersItemMenu showNetworkServersItemMenu:
                                lstContextShowNetworkServersItemMenu.Add(showNetworkServersItemMenu);
                                break;
                            case ShowNetworkServersMenu showNetworkServersMenu:
                                lstContextShowNetworkServersMenu.Add(showNetworkServersMenu);
                                break;
                            case ShowNetworkUnlocatedItemMenu showNetworkUnlocatedItemMenu:
                                lstContextShowNetworkUnlocatedItemMenu.Add(showNetworkUnlocatedItemMenu);
                                break;
                            case ShowNetworkUnlocatedMenu showNetworkUnlocatedMenu:
                                lstContextShowNetworkUnlocatedMenu.Add(showNetworkUnlocatedMenu);
                                break;
                            case ShowProjectMenu showProjectMenu:
                                lstContextShowProjectMenu.Add(showProjectMenu);
                                break;
                        }
                    }
                    break;
            }
        }

        /// <summary>
        ///     Create a relationship between a domain and an IP address
        /// </summary>
        /// <param name="associationDomainIp"></param>
        private static void AddAssociationDomainIp(AssociationDomainIP associationDomainIp)
        {
            // don't add it if it already existed
            if (
                Program.data.relations.Items.Any(
                    I => I.Domain.Domain == associationDomainIp.domain && I.Ip.Ip == associationDomainIp.ip))
            {
                return;
            }

            // add the IP address to the project if it did not already exist
            if (Program.data.GetIp(associationDomainIp.ip) == null)
                Program.data.AddIP(associationDomainIp.ip, "Plugins", Program.cfgCurrent.MaxRecursion);
            var ipItem = Program.data.GetIp(associationDomainIp.ip);
            if (ipItem == null)
                return;

            // Check if the domain exists
            if (Program.data.GetDomain(associationDomainIp.domain) == null)
                Program.data.AddDomain(associationDomainIp.domain, "Plugins", Program.cfgCurrent.MaxRecursion,
                    Program.cfgCurrent);
            var domainItem = Program.data.GetDomain(associationDomainIp.domain);
            if (domainItem == null)
                return;

            Program.data.AddResolution(domainItem.Domain, ipItem.ToString(), "Plugins", Program.cfgCurrent.MaxRecursion,
                Program.cfgCurrent, true);
        }

        /// <summary>
        ///     Allows a plugin to add domains to the project
        /// </summary>
        /// <param name="domain"></param>
        private static void AddDomain(string domain)
        {
            if (Program.data.GetDomain(domain) == null)
                Program.data.AddDomain(domain, "Plugins", 0, Program.cfgCurrent);
        }

        /// <summary>
        ///     Allows a plugin to add IP addresses to the project
        /// </summary>
        /// <param name="ip"></param>
        private static void AddIp(string ip)
        {
            if (Program.data.GetIp(ip) == null)
                Program.data.AddIP(ip, "Plugins", 0);
        }

        /// <summary>
        ///     Asynchronous URLs add action
        /// </summary>
        /// <param name="url"></param>
        private static void AddUrlAsync(object url)
        {
            AddUrl(url.ToString());
        }

        /// <summary>
        ///     Allows a plugin to add URLs to the project
        /// </summary>
        /// <param name="url"></param>
        private static void AddUrl(string url)
        {
            var uri = new Uri(url);
            var domain = Program.data.GetDomain(uri.Host);

            if (domain == null)
            {
                Program.data.AddDomain(uri.Host, "Plugins", Program.cfgCurrent.MaxRecursion, Program.cfgCurrent);
            }
            else
            {
                domain = Program.data.GetDomain(uri.Host);
                // If URL could not be added, return
                if (domain == null)
                    return;
            }
            if (domain == null) return;
            domain.map.AddUrl(url);

            if (Program.data.relations.Items.Any(r => r.Domain.Domain == domain.Domain && r.Ip != null))
                return;
            var listIpsOfDomain = DNSUtil.GetHostAddresses(domain.Domain);
            foreach (var ip in listIpsOfDomain)
            {
                Program.data.AddResolution(domain.Domain, ip.ToString(), "Plugins", Program.cfgCurrent.MaxRecursion,
                    Program.cfgCurrent, true);
            }
        }

        #region IDisposable Support

        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue) return;
            if (disposing)
            {
                lstContextGlobal.Dispose();
                lstContextShowDomainsDomainItemMenu.Dispose();
                lstContextShowDomainsDomainMenu.Dispose();
                lstContextShowDomainsDomainRelatedDomainsItemMenu.Dispose();
                lstContextShowDomainsDomainRelatedDomainsMenu.Dispose();
                lstContextShowDomainsMenu.Dispose();
                lstContextShowMetadataMenu.Dispose();
                lstContextShowNetworkClientsItemMenu.Dispose();
                lstContextShowNetworkClientsMenu.Dispose();
                lstContextShowNetworkIpRangeMenu.Dispose();
                lstContextShowNetworkMenu.Dispose();
                lstContextShowNetworkServersItemMenu.Dispose();
                lstContextShowNetworkServersMenu.Dispose();
                lstContextShowNetworkUnlocatedItemMenu.Dispose();
                lstContextShowNetworkUnlocatedMenu.Dispose();
                lstContextShowProjectMenu.Dispose();
            }

            lstContextGlobal = null;
            lstContextShowDomainsDomainItemMenu = null;
            lstContextShowDomainsDomainMenu = null;
            lstContextShowDomainsDomainRelatedDomainsItemMenu = null;
            lstContextShowDomainsDomainRelatedDomainsMenu = null;
            lstContextShowDomainsMenu = null;
            lstContextShowMetadataMenu = null;
            lstContextShowNetworkClientsItemMenu = null;
            lstContextShowNetworkClientsMenu = null;
            lstContextShowNetworkIpRangeMenu = null;
            lstContextShowNetworkMenu = null;
            lstContextShowNetworkServersItemMenu = null;
            lstContextShowNetworkServersMenu = null;
            lstContextShowNetworkUnlocatedItemMenu = null;
            lstContextShowNetworkUnlocatedMenu = null;
            lstContextShowProjectMenu = null;

            disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
