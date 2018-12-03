using FOCA.ModifiedComponents;
using MetadataExtractCore.Diagrams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FOCA.GUI
{
    static class UpdateGUI
    {
        public enum TreeViewKeys
        {
            KProject = 0,
            KPCServers = 1,
            KDomains = 2,
            KMetadata = 3
        }

        static TreeViewNoFlickering treeView = Program.FormMainInstance.TreeView;

        static public void realUpdateTree()
        {
            List<string> domains = null;

            try
            {
                treeView.Invoke(new MethodInvoker(delegate
                    {
                        try
                        {
                            CreateMainNodes();
                        }
                        catch
                        {
                        }
                    }));
            }
            catch
            {
            }

            if (Program.data == null)
                return;

            try
            {
                treeView.Invoke(new MethodInvoker(delegate
                    {
                        try
                        {
                            NodeNetworkProcess();
                        }
                        catch
                        {
                        }
                    }));
            }
            catch
            {
            }

            if (Program.data.domains != null)
            {
                try
                {
                    treeView.Invoke(new MethodInvoker(delegate
                    {
                        try
                        {
                            if (treeView.Nodes[TreeViewKeys.KProject.ToString()] != null)
                            {
                                if (treeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KDomains.ToString()] != null)
                                {
                                    if ((treeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KDomains.ToString()].IsExpanded))
                                    {
                                        if (domains == null)
                                            domains = Program.data.GetDomains();

                                        NodeDomainsProcess(domains, treeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KDomains.ToString()]);
                                    }
                                    else
                                    {
                                        if ((treeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KDomains.ToString()].Nodes.Count == 0) &&
                                            (Program.data.domains.Items.Count > 0))
                                        {
                                            if (domains == null)
                                                domains = Program.data.GetDomains();

                                            NodeDomainsProcess(domains, treeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KDomains.ToString()]);
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }));
                }
                catch
                {
                }
            }

            try
            {
                treeView.Invoke(new MethodInvoker(delegate
                    {
                        try
                        {
                            NodeMetadataProcess();
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

        /// <summary>
        /// Process Network nodes.
        /// </summary>
        static private void NodeNetworkProcess()
        {
            if (Program.data == null) return;

            var nServers = 0;

            var tnPCServers = Program.FormMainInstance.TreeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KPCServers.ToString()];

            if (tnPCServers != null)
            {
                var lst = new ThreadSafeList<ComputersItem>(Program.data.computers.Items);
                foreach (var computer in lst)
                {
                    Application.DoEvents();

                    TreeNode tn = null;
                    switch (computer.type)
                    {
                        case ComputersItem.Tipo.ClientPC:
                            {
                                tn = tnPCServers.Nodes["Clients"].Nodes[computer.name];
                                if (tn == null)
                                {
                                    var insertAtIndex = Program.FormMainInstance.SearchTextInNodes(tnPCServers.Nodes["Clients"].Nodes, computer.name);
                                    tn = tnPCServers.Nodes["Clients"].Nodes.Insert(insertAtIndex, computer.name, computer.name);
                                    tn.ContextMenuStrip = Program.FormMainInstance.contextMenu;
                                }
                                else
                                {
                                    tn.ContextMenuStrip = Program.FormMainInstance.contextMenu;
                                }
                            }
                            break;
                        case ComputersItem.Tipo.Server:
                            {
                                //Si no tiene una IP asociada añadir al nodo de servidores desconocidos
                                if (!Program.data.computerIPs.Items.Any(C => C.Computer.name == computer.name))
                                {
                                    Object x = Program.FormMainInstance.TreeView;
                                    if (tnPCServers.Nodes["Servers"].Nodes["Unlocated Servers"].Nodes[computer.name] != null)
                                        tn = tnPCServers.Nodes["Servers"].Nodes["Unlocated Servers"].Nodes[computer.name];
                                    else
                                    {
                                        int insertAtIndex = Program.FormMainInstance.SearchTextInNodes(tnPCServers.Nodes["Servers"].Nodes["Unlocated Servers"].Nodes, computer.name);
                                        tn = tnPCServers.Nodes["Servers"].Nodes["Unlocated Servers"].Nodes.Insert(insertAtIndex, computer.name, computer.name);
                                        tn.ContextMenuStrip = Program.FormMainInstance.contextMenu;
                                    }
                                    if (tn == null)
                                        return;
                                }
                                else //Como tiene una IP asociada insertar en su rango
                                {
                                    string strIP = Program.data.computerIPs.Items.First(C => C.Computer.name == computer.name).Ip.Ip;

                                    if (Program.data.Project.IsIpInNetrange(strIP) == false)
                                        continue;

                                    bool found = false;
                                    foreach (RelationsItem relation in Program.data.relations.Items.Where(S => S.Ip.Ip == strIP))
                                    {
                                        if (relation.Domain.Domain.Contains(Program.data.Project.Domain) == true)
                                            found = true;

                                        foreach (string auxDom in Program.data.Project.AlternativeDomains)
                                        {
                                            if (relation.Domain.Domain.Contains(auxDom))
                                                found = true;
                                        }
                                    }

                                    if ((found == false) && (DNSUtil.IsIPv4(strIP)))
                                    {
                                        if (Program.data.IsIpInLimitRange(strIP))
                                            found = true;
                                    }

                                    if (found == false)
                                        continue;


                                    //Comprobar que existe el rango
                                    if (DNSUtil.IsIPv4(strIP))
                                    {
                                        byte[] IPBytes = DNSUtil.IPToByte(strIP);
                                        //Dependiendo de la calse de Ip que sea tiene que dibujarla a una profundidad u otra
                                        //IP de clase A
                                        if (IPBytes[0] >= 1 && IPBytes[0] < 128)
                                        {
                                            TreeNode tnIP = null;
                                            string iprange = string.Format("{0}.0.0.0", IPBytes[0]);

                                            if (tnPCServers.Nodes["Servers"].Nodes[iprange] != null)
                                                tnIP = tnPCServers.Nodes["Servers"].Nodes[iprange];
                                            else
                                            {
                                                tnIP = tnPCServers.Nodes["Servers"].Nodes.Insert(Program.FormMainInstance.SearchTextInNodes(tnPCServers.Nodes["Servers"].Nodes, iprange), iprange, iprange);
                                                tnIP.ContextMenuStrip = Program.FormMainInstance.contextMenu;
                                                tnIP.ImageIndex = tnIP.SelectedImageIndex = 18;
                                                tnIP.Tag = "iprange";
                                            }
                                            if (tnIP == null)
                                                return;
                                            iprange = string.Format("{0}.{1}.0.0", IPBytes[0], IPBytes[1]);
                                            if (tnIP.Nodes[iprange] != null)
                                                tnIP = tnIP.Nodes[iprange];
                                            else
                                            {
                                                tnIP = tnIP.Nodes.Insert(Program.FormMainInstance.SearchTextInNodes(tnIP.Nodes, iprange), iprange, iprange);
                                                tnIP.ContextMenuStrip = Program.FormMainInstance.contextMenu;
                                                tnIP.ImageIndex = tnIP.SelectedImageIndex = 18;
                                                tnIP.Tag = "iprange";
                                            }
                                            if (tnIP == null)
                                                return;
                                            iprange = string.Format("{0}.{1}.{2}.0", IPBytes[0], IPBytes[1], IPBytes[2]);
                                            if (tnIP.Nodes[iprange] != null)
                                                tnIP = tnIP.Nodes[iprange];
                                            else
                                            {
                                                tnIP = tnIP.Nodes.Insert(Program.FormMainInstance.SearchTextInNodes(tnIP.Nodes, iprange), iprange, iprange);
                                                tnIP.ContextMenuStrip = Program.FormMainInstance.contextMenu;
                                                tnIP.ImageIndex = tnIP.SelectedImageIndex = 18;
                                                tnIP.Tag = "iprange";
                                            }
                                            iprange = string.Format("{0}.{1}.{2}.{3}", IPBytes[0], IPBytes[1], IPBytes[2], IPBytes[3]);
                                            if (tnIP.Nodes[iprange] != null)
                                                tn = tnIP.Nodes[iprange];
                                            else
                                            {
                                                tn = tnIP.Nodes.Insert(Program.FormMainInstance.SearchTextInNodes(tnIP.Nodes, iprange), iprange, computer.name);
                                                tn.ContextMenuStrip = Program.FormMainInstance.contextMenu;
                                            }
                                            if (tn == null)
                                                return;
                                        }
                                        //IP de clase B
                                        else if (IPBytes[0] >= 128 && IPBytes[0] < 192)
                                        {
                                            TreeNode tnIP = null;
                                            string iprange = string.Format("{0}.{1}.0.0", IPBytes[0], IPBytes[1]);
                                            if (tnPCServers.Nodes["Servers"].Nodes[iprange] != null)
                                                tnIP = tnPCServers.Nodes["Servers"].Nodes[iprange];
                                            else
                                            {
                                                tnIP = tnPCServers.Nodes["Servers"].Nodes.Insert(Program.FormMainInstance.SearchTextInNodes(tnPCServers.Nodes["Servers"].Nodes, iprange), iprange, iprange);
                                                tnIP.ContextMenuStrip = Program.FormMainInstance.contextMenu;
                                                tnIP.ImageIndex = tnIP.SelectedImageIndex = 18;
                                                tnIP.Tag = "iprange";
                                            }
                                            if (tnIP == null)
                                                return;
                                            iprange = string.Format("{0}.{1}.{2}.0", IPBytes[0], IPBytes[1], IPBytes[2]);
                                            if (tnIP.Nodes[iprange] != null)
                                                tnIP = tnIP.Nodes[iprange];
                                            else
                                            {
                                                tnIP = tnIP.Nodes.Insert(Program.FormMainInstance.SearchTextInNodes(tnIP.Nodes, iprange), iprange, iprange);
                                                tnIP.ContextMenuStrip = Program.FormMainInstance.contextMenu;
                                                tnIP.ImageIndex = tnIP.SelectedImageIndex = 18;
                                                tnIP.Tag = "iprange";
                                            }
                                            iprange = string.Format("{0}.{1}.{2}.{3}", IPBytes[0], IPBytes[1], IPBytes[2], IPBytes[3]);
                                            if (tnIP.Nodes[iprange] != null)
                                                tn = tnIP.Nodes[iprange];
                                            else
                                            {
                                                tn = tnIP.Nodes.Insert(Program.FormMainInstance.SearchTextInNodes(tnIP.Nodes, iprange), iprange, computer.name);
                                                tn.ContextMenuStrip = Program.FormMainInstance.contextMenu;
                                            }
                                        }
                                        //IP de clase C
                                        else if (IPBytes[0] >= 192 && IPBytes[0] < 240)
                                        {
                                            TreeNode tnIP = null;
                                            string iprange = string.Format("{0}.{1}.{2}.0", IPBytes[0], IPBytes[1], IPBytes[2]);
                                            if (tnPCServers.Nodes["Servers"].Nodes[iprange] != null)
                                                tnIP = tnPCServers.Nodes["Servers"].Nodes[iprange];
                                            else
                                            {
                                                tnIP = tnPCServers.Nodes["Servers"].Nodes.Insert(Program.FormMainInstance.SearchTextInNodes(tnPCServers.Nodes["Servers"].Nodes, iprange), iprange, iprange);
                                                tnIP.ContextMenuStrip = Program.FormMainInstance.contextMenu;
                                                tnIP.ImageIndex = tnIP.SelectedImageIndex = 18;
                                                tnIP.Tag = "iprange";
                                            }
                                            if (tnIP == null)
                                                return;
                                            iprange = string.Format("{0}.{1}.{2}.{3}", IPBytes[0], IPBytes[1], IPBytes[2], IPBytes[3]);
                                            if (tnIP.Nodes[iprange] != null)
                                                tn = tnIP.Nodes[iprange];
                                            else
                                            {
                                                tn = tnIP.Nodes.Insert(Program.FormMainInstance.SearchTextInNodes(tnIP.Nodes, iprange), iprange, computer.name);
                                                tn.ContextMenuStrip = Program.FormMainInstance.contextMenu;
                                            }
                                        }

                                        nServers++;
                                    }
                                    else
                                    {
                                        //ipv6
                                    }
                                }

                            }
                            break;
                    }

                    Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(delegate
                    {
                        if (tn == null)
                            return;
                        tn.Tag = computer;


                        if (computer.os != OperatingSystem.OS.Unknown)
                            tn.ImageIndex = tn.SelectedImageIndex = OperatingSystemUtils.OSToIconNumber(computer.os);
                        else
                        {
                            tn.ImageIndex = tn.SelectedImageIndex = computer.type == ComputersItem.Tipo.ClientPC ? 111 : 45;
                        }

                        var computersDomain = Program.data.computerDomains.Items;

                        foreach (var cdi in computersDomain.Where(x => x.Computer == computer))
                        {
                            if (cdi.Domain == null)
                                continue;

                            if (!(tn.Nodes.ContainsKey(cdi.Domain.Domain)))
                            {
                                var newTn = new TreeNode(cdi.Domain.Domain);
                                newTn.Text = cdi.Domain.Domain;
                                newTn.Name = cdi.Domain.Domain;
                                newTn.Tag = cdi;

                                if (cdi.Domain.os == OperatingSystem.OS.Unknown)
                                    newTn.SelectedImageIndex = newTn.ImageIndex = 90;
                                else
                                    newTn.SelectedImageIndex = newTn.ImageIndex = OperatingSystemUtils.OSToIconNumber(cdi.Domain.os);

                                tn.Nodes.Add(newTn);
                            }
                        }

                        foreach (var cdi in Program.data.computerDomains.Items)
                        {
                            if (cdi.Domain == null)
                                return;

                            var oldTn = tn.Nodes[cdi.Domain.Domain];

                            if (oldTn == null)
                                continue;

                            if (cdi.Domain == null)
                                continue;

                            oldTn.ImageIndex = OperatingSystemUtils.OSToIconNumber(cdi.Domain.os);
                            oldTn.SelectedImageIndex = oldTn.ImageIndex;

                            OperatingSystem.OS lastOsDom = OperatingSystem.OS.Unknown;
                            var networkDevice = false;

                            lastOsDom = OperatingSystem.OS.Unknown;
                            foreach (ComputerDomainsItem cdiDom in from TreeNode tnDom in tn.Nodes where tnDom.Tag is ComputerDomainsItem select (ComputerDomainsItem)tnDom.Tag)
                            {
                                if ((cdiDom.Domain.os != lastOsDom) && (cdiDom.Domain.os != OperatingSystem.OS.Unknown) && (lastOsDom != OperatingSystem.OS.Unknown))
                                {
                                    networkDevice = true;
                                    break;
                                }
                                if (cdiDom.Domain.os != OperatingSystem.OS.Unknown)
                                    lastOsDom = cdiDom.Domain.os;
                            }

                            if (networkDevice)
                                tn.ImageIndex = tn.SelectedImageIndex = 100;
                            else
                                if (lastOsDom != OperatingSystem.OS.Unknown)
                                tn.ImageIndex = tn.SelectedImageIndex = OperatingSystemUtils.OSToIconNumber(lastOsDom);
                        }

                        if (computer.Users.Items.Count > 0)
                        {
                            TreeNode tnUsers;
                            if (tn.Nodes["Users"] == null)
                            {
                                tnUsers = tn.Nodes.Add("Users", "Users");
                                tnUsers.ImageIndex = tnUsers.SelectedImageIndex = 14;
                            }
                            else
                                tnUsers = tn.Nodes["Users"];
                            tnUsers.Tag = computer.Users;
                        }
                        else if (tn.Nodes["Users"] != null)
                            tn.Nodes["Users"].Remove();

                        if (computer.Description.Items.Count > 0)
                        {
                            TreeNode tnDescription;
                            if (tn.Nodes["Description"] == null)
                                tnDescription = tn.Nodes.Add("Description", "Description");
                            else
                                tnDescription = tn.Nodes["Description"];
                            tnDescription.Tag = computer.Description;
                        }
                        else if (tn.Nodes["Description"] != null)
                            tn.Nodes["Description"].Remove();

                        if (computer.RemotePasswords.Items.Count > 0)
                        {
                            TreeNode tnPasswords;
                            if (tn.Nodes["Passwords"] == null)
                            {
                                tnPasswords = tn.Nodes.Add("Passwords", "Passwords");
                                tnPasswords.ImageIndex = tnPasswords.SelectedImageIndex = 121;
                            }
                            else
                                tnPasswords = tn.Nodes["Passwords"];
                            tnPasswords.Tag = computer.RemotePasswords;
                        }
                        else if (tn.Nodes["Passwords"] != null)
                            tn.Nodes["Passwords"].Remove();


                        if (computer.Folders.Items.Count > 0)
                        {
                            TreeNode tnFolders;
                            if (tn.Nodes["Folders"] == null)
                            {
                                tnFolders = tn.Nodes.Add("Folders", "Folders");
                                tnFolders.ImageIndex = tnFolders.SelectedImageIndex = 117;
                            }
                            else
                                tnFolders = tn.Nodes["Folders"];
                            tnFolders.Tag = computer.Folders;
                        }
                        else if (tn.Nodes["Folders"] != null)
                            tn.Nodes["Folders"].Remove();
                        if (computer.Printers.Items.Count > 0)
                        {
                            TreeNode tnPrinters;
                            if (tn.Nodes["Printers"] == null)
                            {
                                tnPrinters = tn.Nodes.Add("Printers", "Printers");
                                tnPrinters.ImageIndex = tnPrinters.SelectedImageIndex = 118;
                            }
                            else
                                tnPrinters = tn.Nodes["Printers"];
                            tnPrinters.Tag = computer.Printers;
                        }
                        else if (tn.Nodes["Printers"] != null)
                            tn.Nodes["Printers"].Remove();
                        if (computer.RemoteUsers.Items.Count > 0)
                        {
                            TreeNode tnRemoteUsers;
                            if (tn.Nodes["Users with access"] == null)
                            {
                                tnRemoteUsers = tn.Nodes.Add("Users with access", "Users with access");
                                tnRemoteUsers.ImageIndex = tnRemoteUsers.SelectedImageIndex = 43;
                            }
                            else
                                tnRemoteUsers = tn.Nodes["Users with access"];
                            tnRemoteUsers.Tag = computer.RemoteUsers;
                        }
                        else if (tn.Nodes["Users with access"] != null)
                            tn.Nodes["Users with access"].Remove();
                        if (computer.RemoteFolders.Items.Count > 0)
                        {
                            TreeNode tnRemoteFolders;
                            if (tn.Nodes["Remote Folders"] == null)
                            {
                                tnRemoteFolders = tn.Nodes.Add("Remote Folders", "Remote Folders");
                                tnRemoteFolders.ImageIndex = tnRemoteFolders.SelectedImageIndex = 42;
                            }
                            else
                                tnRemoteFolders = tn.Nodes["Remote Folders"];
                            tnRemoteFolders.Tag = computer.RemoteFolders;
                        }
                        else if (tn.Nodes["Remote Folders"] != null)
                            tn.Nodes["Remote Folders"].Remove();
                        if (computer.RemotePrinters.Items.Count > 0)
                        {
                            TreeNode tnRemotePrinters;
                            if (tn.Nodes["Remote Printers"] == null)
                            {
                                tnRemotePrinters = tn.Nodes.Add("Remote Printers", "Remote Printers");
                                tnRemotePrinters.ImageIndex = tnRemotePrinters.SelectedImageIndex = 44;
                            }
                            else
                                tnRemotePrinters = tn.Nodes["Remote Printers"];
                            tnRemotePrinters.Tag = computer.RemotePrinters;
                        }
                        else if (tn.Nodes["Remote Printers"] != null)
                            tn.Nodes["Remote Printers"].Remove();
                    }));
                }
            }

            if (tnPCServers != null)
            {
                for (var i = tnPCServers.Nodes["Clients"].Nodes.Count; i > 0; i--)
                {
                    var tn = tnPCServers.Nodes["Clients"].Nodes[i - 1];
                    if (!Program.data.computers.Items.Any(C => C.type == ComputersItem.Tipo.ClientPC && C.name.ToLower() == tn.Text.ToLower()))
                    {
                        Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(delegate
                        {
                            tn.Remove();
                        }));
                    }
                }

                Program.FormMainInstance.DeleteNodesServers(tnPCServers.Nodes["Servers"].Nodes);
                Program.FormMainInstance.TreeView.Invoke(new MethodInvoker(delegate
                {
                    tnPCServers.Nodes["Clients"].Text = string.Format("Clients ({0})", tnPCServers.Nodes["Clients"].Nodes.Count);
                }));
                tnPCServers.Nodes["Servers"].Text = string.Format("Servers ({0})", nServers);

            }
        }

        /// <summary>
        /// Process node Domains.
        /// </summary>
        /// <param name="domains"></param>
        /// <param name="tnDomains"></param>
        static private void NodeDomainsProcess(List<string> domains, TreeNode tnDomains)
        {
            try
            {
                foreach (var domain in domains)
                {
                    Application.DoEvents();

                    if ((Program.data.Project.Domain != null && domain.Contains(Program.data.Project.Domain)) || (Program.data.Project.AlternativeDomains.Contains(domain)))
                    {
                        string[] domainParts = domain.Split('.');

                        if (domainParts.Length <= 1)
                            continue;

                        if (domainParts.Length >= 2)
                        {
                            var mainDomain = domainParts[domainParts.Length - 2] + "." + domainParts[domainParts.Length - 1];

                            TreeNode tnDomain = null;

                            var dominioDoble = false;

                            var doubleDomains = new[] { "gov", "gob", "edu", "mil", "com", "org", "net", "tv", "co" };
                            for (int i = 0; i < doubleDomains.Length; i++)
                            {
                                if (domainParts[domainParts.Length - 2] == doubleDomains[i])
                                {
                                    if (domainParts.Length == 2)
                                        break;
                                    mainDomain = domainParts[domainParts.Length - 3] + "." + mainDomain;
                                    dominioDoble = true;
                                    break;
                                }
                            }

                            if (Program.data.IsDomainOrAlternative(domain))
                            {
                                if (tnDomains.Nodes[mainDomain] != null)
                                    tnDomain = tnDomains.Nodes[mainDomain];
                                else
                                {
                                    tnDomain = tnDomains.Nodes.Insert(Program.FormMainInstance.SearchTextInNodes(tnDomains.Nodes, mainDomain), mainDomain, mainDomain);
                                    tnDomain.ImageIndex = tnDomain.SelectedImageIndex = 107;
                                }
                                tnDomain.ContextMenuStrip = Program.FormMainInstance.contextMenu;
                            }

                            foreach (TreeNode tnNewDomain in from altDom in Program.data.Project.AlternativeDomains where tnDomains.Nodes[altDom] == null select tnDomains.Nodes.Insert(Program.FormMainInstance.SearchTextInNodes(tnDomains.Nodes, altDom), altDom, altDom))
                            {
                                tnNewDomain.ImageIndex = tnNewDomain.SelectedImageIndex = 107;
                            }

                            for (var i = 3; i <= domainParts.Length; i++)
                            {
                                if ((dominioDoble) & (i == 3))
                                    i = 4;
                                if (dominioDoble)
                                {
                                    if (domainParts.Length == 3)
                                        continue;
                                }

                                mainDomain = domainParts[domainParts.Length - i] + "." + mainDomain;
                                if (tnDomain == null)
                                    continue;

                                if (tnDomain.Nodes[mainDomain] != null)
                                {
                                    tnDomain = tnDomain.Nodes[mainDomain];

                                    tnDomain.ContextMenuStrip = Program.FormMainInstance.contextMenu;
                                    var itemDominio = Program.data.GetDomain(mainDomain);

                                    if (itemDominio == null)
                                        continue;

                                    tnDomain.ImageIndex = tnDomain.SelectedImageIndex = 111;//OperatingSystemUtils.OSToIconNumber(itemDominio.os);//45;
                                    tnDomain.Tag = itemDominio;
                                }
                                else
                                {
                                    if ((!mainDomain.Contains(Program.data.Project.Domain)) &&
                                        (!Program.data.Project.AlternativeDomains.Contains(mainDomain))) continue;
                                    tnDomain = tnDomain.Nodes.Insert(Program.FormMainInstance.SearchTextInNodes(tnDomain.Nodes, mainDomain), mainDomain, mainDomain);
                                    tnDomain.ContextMenuStrip = Program.FormMainInstance.contextMenu;
                                    var itemDominio = Program.data.GetDomain(mainDomain);

                                    if (itemDominio == null)
                                        continue;

                                    tnDomain.ImageIndex = tnDomain.SelectedImageIndex = 111;//OperatingSystemUtils.OSToIconNumber(itemDominio.os);//45;
                                    tnDomain.Tag = itemDominio;
                                }
                            }
                        }
                    }
                    else
                    {

                        var domainParts = domain.Split('.');

                        if (domainParts.Length <= 1)
                            continue;

                        if (domainParts.Length >= 2)
                        {
                            var mainDomain = domainParts[domainParts.Length - 2] + "." + domainParts[domainParts.Length - 1];

                            TreeNode tnDomain = null;

                            string[] doubleDomains = { "gov", "gob", "edu", "mil", "com", "org", "net", "tv", "co" };

                            if (doubleDomains.Where(t => domainParts[domainParts.Length - 2] == t).TakeWhile(t => domainParts.Length != 2).Any())
                            {
                                mainDomain = domainParts[domainParts.Length - 3] + "." + mainDomain;
                            }

                            tnDomain.ContextMenuStrip = Program.FormMainInstance.contextMenu;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Process metadata nodes.
        /// </summary>
        static private void NodeMetadataProcess()
        {
            var users = (List<UserItem>)Program.FormMainInstance.TreeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Users"].Tag;

            var printers = (List<PrintersItem>)Program.FormMainInstance.TreeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Printers"].Tag;

            var folders = (List<PathsItem>)Program.FormMainInstance.TreeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Folders"].Tag;

            var software = (List<ApplicationsItem>)Program.FormMainInstance.TreeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Software"].Tag;

            var emails = (List<EmailsItem>)Program.FormMainInstance.TreeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Emails"].Tag;

            var operatingsystems = (List<string>)Program.FormMainInstance.TreeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Operating Systems"].Tag;

            var passwords = (List<PasswordsItem>)Program.FormMainInstance.TreeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Passwords"].Tag;

            var servers = (List<ServersItem>)Program.FormMainInstance.TreeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Servers"].Tag;

            Program.FormMainInstance.TreeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Users"].Text = string.Format("Users ({0})", users.Count);
            Program.FormMainInstance.TreeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Printers"].Text = string.Format("Printers ({0})", printers.Count);
            Program.FormMainInstance.TreeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Folders"].Text = string.Format("Folders ({0})", folders.Count);
            Program.FormMainInstance.TreeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Software"].Text = string.Format("Software ({0})", software.Count);
            Program.FormMainInstance.TreeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Emails"].Text = string.Format("Emails ({0})", emails.Count);
            Program.FormMainInstance.TreeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Operating Systems"].Text = string.Format("Operating Systems ({0})", operatingsystems.Count);
            Program.FormMainInstance.TreeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Passwords"].Text = string.Format("Passwords ({0})", passwords.Count);
            Program.FormMainInstance.TreeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KMetadata.ToString()].Nodes["Metadata Summary"].Nodes["Servers"].Text = string.Format("Servers ({0})", servers.Count);
        }

        /// <summary>
        /// Create metadata nodes.
        /// </summary>
        static private void CreateMetadataNodes()
        {
            if (treeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KMetadata.ToString()].Nodes.Count == 0)
            {
                TreeNode treeViewMetadata = treeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KMetadata.ToString()];
                treeViewMetadata.Nodes.Clear();
                TreeNode tn_documents = treeViewMetadata.Nodes.Add("Documents", "Documents (0/0)");
                tn_documents.ImageIndex =
                tn_documents.SelectedImageIndex = 114;
                TreeNode tn_data = treeViewMetadata.Nodes.Add("Metadata Summary", "Metadata Summary");
                tn_data.ImageIndex =
                tn_data.SelectedImageIndex = 115;

                TreeNode tn_users = tn_data.Nodes.Add("Users", "Users");
                tn_users.ImageIndex =
                tn_users.SelectedImageIndex = 116;
                List<UserItem> users = new List<UserItem>();
                tn_users.Tag = users;

                TreeNode tn_folders = tn_data.Nodes.Add("Folders", "Folders");
                tn_folders.ImageIndex =
                tn_folders.SelectedImageIndex = 117;
                var folders = new List<PathsItem>();
                tn_folders.Tag = folders;

                TreeNode tn_printers = tn_data.Nodes.Add("Printers", "Printers");
                tn_printers.ImageIndex =
                tn_printers.SelectedImageIndex = 118;
                var printers = new List<PrintersItem>();
                tn_printers.Tag = printers;

                TreeNode tn_software = tn_data.Nodes.Add("Software", "Software");
                tn_software.ImageIndex =
                tn_software.SelectedImageIndex = 119;
                var software = new List<ApplicationsItem>();
                tn_software.Tag = software;

                TreeNode tn_emails = tn_data.Nodes.Add("Emails", "Emails");
                tn_emails.ImageIndex =
                tn_emails.SelectedImageIndex = 120;
                var emails = new List<EmailsItem>();
                tn_emails.Tag = emails;

                TreeNode tn_operatingsystems = tn_data.Nodes.Add("Operating Systems", "Operating Systems");
                tn_operatingsystems.ImageIndex =
                tn_operatingsystems.SelectedImageIndex = 20;
                var operatingsystems = new List<string>();
                tn_operatingsystems.Tag = operatingsystems;

                TreeNode tn_passwords = tn_data.Nodes.Add("Passwords", "Passwords");
                tn_passwords.ImageIndex =
                tn_passwords.SelectedImageIndex = 121;
                var passwords = new List<PasswordsItem>();
                tn_passwords.Tag = passwords;

                TreeNode tn_servers = tn_data.Nodes.Add("Servers", "Servers");
                tn_servers.ImageIndex =
                tn_servers.SelectedImageIndex = 112;
                var servers = new List<ServersItem>();
                tn_servers.Tag = servers;
            }
        }

        /// <summary>
        /// Create Pc Server nodes.
        /// </summary>
        static private void CreatePcServersNodes()
        {
            TreeNode tn;
            tn = treeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KPCServers.ToString()].Nodes.Add("Clients", "Clients");
            tn.ImageIndex = tn.SelectedImageIndex = 111;
            tn = treeView.Nodes[TreeViewKeys.KProject.ToString()].Nodes[TreeViewKeys.KPCServers.ToString()].Nodes.Add("Servers", "Servers");
            tn.ImageIndex = tn.SelectedImageIndex = 112;
            tn = tn.Nodes.Add("Unlocated Servers", "Unlocated Servers");
            tn.ImageIndex = tn.SelectedImageIndex = 113;
        }

        /// <summary>
        /// Create main nodes.
        /// </summary>
        static private void CreateMainNodes()
        {
            TreeNode tnProject, tn;

            if (!treeView.Nodes.ContainsKey(TreeViewKeys.KProject.ToString()))
            {
                tnProject = treeView.Nodes.Add(TreeViewKeys.KProject.ToString(), Program.data.Project.ProjectName);
                tnProject.ImageIndex = tnProject.SelectedImageIndex = 107;
                tnProject.Tag = Program.data.Project.ProjectName;

                tn = tnProject.Nodes.Add(TreeViewKeys.KPCServers.ToString(), "Network");
                tn.ImageIndex = tn.SelectedImageIndex = 110;
                CreatePcServersNodes();

                tn = tnProject.Nodes.Add(TreeViewKeys.KDomains.ToString(), "Domains");
                tn.ImageIndex = tn.SelectedImageIndex = 108;

                tn = tnProject.Nodes.Add(TreeViewKeys.KMetadata.ToString(), "Metadata");
                tn.ImageIndex = tn.SelectedImageIndex = 109;
                CreateMetadataNodes();

                Program.FormMainInstance.TreeView.Nodes[TreeViewKeys.KProject.ToString()].Expand();
            }
        }

        /// <summary>
        /// Reset nodes.
        /// </summary>
        public static void Reset()
        {
            treeView.ResetText();
            treeView.Nodes.Clear();
            CreateMainNodes();
        }
    }

}
