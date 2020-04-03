using FOCA.Database.Entities;
using FOCA.ModifiedComponents;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace FOCA.GUI
{
    static class UpdateGUI
    {
        private static readonly string[] DoubleDomains = new[] { "gov", "gob", "edu", "mil", "com", "org", "net", "tv", "co" };
        private static TreeViewNoFlickering treeView = Program.FormMainInstance.TreeView;

        public static void Initialize()
        {
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
        }

        public static void UpdateTree(CancellationToken cancelToken)
        {
            if (Program.data == null)
                return;

            try
            {
                treeView.Invoke(new MethodInvoker(delegate
                    {
                        try
                        {
                            treeView.BeginUpdate();
                            NodeNetworkProcess();
                        }
                        catch
                        {
                        }

                        if (Program.data.domains != null)
                        {
                            try
                            {
                                if (treeView.GetNode(Navigation.Project.ToNavigationPath()) != null)
                                {
                                    TreeNode domainNode = treeView.GetNode(Navigation.Project.Domains.ToNavigationPath());
                                    if (domainNode != null && (domainNode.IsExpanded || (domainNode.Nodes.Count == 0 && Program.data.domains.Items.Count > 0)))
                                    {
                                        NodeDomainsProcess(Program.data.GetDomains(), domainNode, cancelToken);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }

                        try
                        {
                            cancelToken.ThrowIfCancellationRequested();
                            NodeMetadataProcess();
                        }
                        catch
                        {
                        }
                        treeView.EndUpdate();
                    }));
            }
            catch
            {
            }
        }

        private static TreeNode GenerateIPRangesNodes(byte[] IPSegments, TreeNode parentNode, int segmentIndex)
        {
            string rangeFormat = String.Empty;
            //Creates format string from 4 IP Address octets like {0}.0.0.0 or {0}.{1}.0.0
            for (int i = 0; i < 4; i++)
            {
                if (i <= segmentIndex)
                {
                    rangeFormat += $"{{{i}}}.";
                }
                else
                {
                    rangeFormat += "0.";
                }
            }
            rangeFormat = rangeFormat.Trim('.');

            string iprange = String.Format(rangeFormat, IPSegments[0], IPSegments[1], IPSegments[2], IPSegments[3]);

            TreeNode tnIP = parentNode.Nodes[iprange];
            if (tnIP == null)
            {
                tnIP = parentNode.Nodes.Insert(Program.FormMainInstance.SearchTextInNodes(parentNode.Nodes, iprange), iprange, iprange);
                tnIP.ContextMenuStrip = Program.FormMainInstance.contextMenu;
                tnIP.ImageIndex = tnIP.SelectedImageIndex = 18;
                tnIP.Tag = "iprange";
            }

            if (segmentIndex >= 3)
            {
                return tnIP;
            }
            else
            {
                return GenerateIPRangesNodes(IPSegments, tnIP, segmentIndex + 1);
            }
        }

        /// <summary>
        /// Process Network nodes.
        /// </summary>
        private static void NodeNetworkProcess()
        {
            if (Program.data == null) return;

            TreeNode tnPCServers = Program.FormMainInstance.TreeView.GetNode(Navigation.Project.Network.ToNavigationPath());
            int nServers = 0;
            if (tnPCServers != null)
            {
                List<ComputersItem> lst = new List<ComputersItem>(Program.data.computers.Items);
                foreach (ComputersItem computer in lst)
                {
                    System.Windows.Forms.Application.DoEvents();

                    TreeNode tn = null;
                    switch (computer.type)
                    {
                        case ComputersItem.Tipo.ClientPC:
                            {
                                tn = tnPCServers.Nodes[Navigation.Project.Network.Clients.Key].Nodes[computer.name];
                                if (tn == null)
                                {
                                    int insertAtIndex = Program.FormMainInstance.SearchTextInNodes(tnPCServers.Nodes[Navigation.Project.Network.Clients.Key].Nodes, computer.name);
                                    tn = tnPCServers.Nodes[Navigation.Project.Network.Clients.Key].Nodes.Insert(insertAtIndex, computer.name, computer.name);
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
                                    TreeNode unknownServers = tnPCServers.Nodes[Navigation.Project.Network.Servers.Key].Nodes[Navigation.Project.Network.Servers.Unknown.Key];
                                    if (unknownServers.Nodes[computer.name] != null)
                                    {
                                        tn = unknownServers.Nodes[computer.name];
                                    }
                                    else
                                    {
                                        int insertAtIndex = Program.FormMainInstance.SearchTextInNodes(unknownServers.Nodes, computer.name);
                                        tn = unknownServers.Nodes.Insert(insertAtIndex, computer.name, computer.name);
                                        tn.ContextMenuStrip = Program.FormMainInstance.contextMenu;
                                    }
                                }
                                else //Como tiene una IP asociada insertar en su rango
                                {
                                    string strIP = Program.data.computerIPs.Items.First(C => C.Computer.name == computer.name).Ip.Ip;

                                    if (Program.data.Project.IsIpInNetrange(strIP) == false)
                                        continue;

                                    bool found = false;
                                    foreach (RelationsItem relation in Program.data.relations.Items.Where(S => S.Ip.Ip == strIP))
                                    {
                                        if (relation.Domain.Domain.Contains(Program.data.Project.Domain))
                                        {
                                            found = true;
                                            break;
                                        }

                                        foreach (string auxDom in Program.data.Project.AlternativeDomains)
                                        {
                                            if (relation.Domain.Domain.Contains(auxDom))
                                            {
                                                found = true;
                                                break;
                                            }
                                        }
                                    }

                                    if (!found && DNSUtil.IsIPv4(strIP))
                                    {
                                        if (Program.data.IsIpInLimitRange(strIP))
                                            found = true;
                                    }

                                    if (!found)
                                        continue;

                                    //Comprobar que existe el rango
                                    if (DNSUtil.IsIPv4(strIP))
                                    {
                                        byte[] IPBytes = DNSUtil.IPToByte(strIP);
                                        int indexFirstSegment = -1;
                                        //IPAddress class A
                                        if (IPBytes[0] >= 1 && IPBytes[0] < 128)
                                        {
                                            indexFirstSegment = 0;
                                        }
                                        //IPAddress class B
                                        else if (IPBytes[0] >= 128 && IPBytes[0] < 192)
                                        {
                                            indexFirstSegment = 1;
                                        }
                                        //IPAddress class C
                                        else if (IPBytes[0] >= 192 && IPBytes[0] < 240)
                                        {
                                            indexFirstSegment = 2;
                                        }
                                        else
                                        {
                                            return;
                                        }
                                        tn = GenerateIPRangesNodes(IPBytes, tnPCServers.Nodes[Navigation.Project.Network.Servers.Key], indexFirstSegment);
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

                    if (tn == null)
                        return;
                    tn.Text = computer.name;
                    tn.Tag = computer;

                    if (computer.os != OperatingSystem.OS.Unknown)
                        tn.ImageIndex = tn.SelectedImageIndex = OperatingSystemUtils.OSToIconNumber(computer.os);
                    else
                    {
                        tn.ImageIndex = tn.SelectedImageIndex = computer.type == ComputersItem.Tipo.ClientPC ? 111 : 45;
                    }

                    foreach (ComputerDomainsItem cdi in Program.data.computerDomains.Items)
                    {
                        if (cdi.Domain == null)
                            continue;

                        if (cdi.Computer == computer)
                        {
                            if (!tn.Nodes.ContainsKey(cdi.Domain.Domain))
                            {
                                TreeNode newTn = new TreeNode(cdi.Domain.Domain);
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

                        TreeNode oldTn = tn.Nodes[cdi.Domain.Domain];

                        if (oldTn == null)
                            continue;

                        oldTn.ImageIndex = OperatingSystemUtils.OSToIconNumber(cdi.Domain.os);
                        oldTn.SelectedImageIndex = oldTn.ImageIndex;

                        OperatingSystem.OS lastOsDom = OperatingSystem.OS.Unknown;
                        bool networkDevice = false;

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
                }
            }

            if (tnPCServers != null)
            {
                for (int i = tnPCServers.Nodes["Clients"].Nodes.Count; i > 0; i--)
                {
                    TreeNode tn = tnPCServers.Nodes["Clients"].Nodes[i - 1];
                    if (!Program.data.computers.Items.Any(C => C.type == ComputersItem.Tipo.ClientPC && C.name.ToLower() == tn.Text.ToLower()))
                    {
                        tn.Remove();
                    }
                }

                Program.FormMainInstance.DeleteNodesServers(tnPCServers.Nodes["Servers"].Nodes);
                tnPCServers.Nodes["Clients"].Text = string.Format("Clients ({0})", tnPCServers.Nodes["Clients"].Nodes.Count);
                tnPCServers.Nodes["Servers"].Text = string.Format("Servers ({0})", nServers);
            }
        }

        /// <summary>
        /// Process node Domains.
        /// </summary>
        /// <param name="domains"></param>
        /// <param name="tnDomains"></param>
        private static void NodeDomainsProcess(List<string> domains, TreeNode tnDomains, CancellationToken cancelToken)
        {
            if (!String.IsNullOrWhiteSpace(Program.data.Project.Domain))
            {
                try
                {
                    foreach (string domain in domains)
                    {
                        System.Windows.Forms.Application.DoEvents();
                        cancelToken.ThrowIfCancellationRequested();

                        if (Program.data.IsDomainOrAlternative(domain))
                        {
                            string[] domainParts = domain.Split('.');

                            if (domainParts.Length >= 2)
                            {
                                string mainDomain = domainParts[domainParts.Length - 2] + "." + domainParts[domainParts.Length - 1];

                                TreeNode tnDomain = null;
                                if (tnDomains.Nodes[mainDomain] != null)
                                    tnDomain = tnDomains.Nodes[mainDomain];
                                else
                                {
                                    tnDomain = tnDomains.Nodes.Insert(Program.FormMainInstance.SearchTextInNodes(tnDomains.Nodes, mainDomain), mainDomain, mainDomain);
                                    tnDomain.ImageIndex = tnDomain.SelectedImageIndex = 107;
                                }
                                tnDomain.ContextMenuStrip = Program.FormMainInstance.contextMenu;

                                foreach (TreeNode tnNewDomain in from altDom in Program.data.Project.AlternativeDomains where tnDomains.Nodes[altDom] == null select tnDomains.Nodes.Insert(Program.FormMainInstance.SearchTextInNodes(tnDomains.Nodes, altDom), altDom, altDom))
                                {
                                    tnNewDomain.ImageIndex = tnNewDomain.SelectedImageIndex = 107;
                                }

                                bool dominioDoble = false;
                                for (int i = 0; i < DoubleDomains.Length; i++)
                                {
                                    if (domainParts[domainParts.Length - 2] == DoubleDomains[i])
                                    {
                                        if (domainParts.Length == 2)
                                            break;
                                        mainDomain = domainParts[domainParts.Length - 3] + "." + mainDomain;
                                        dominioDoble = true;
                                        break;
                                    }
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

                                    if (tnDomain.Nodes[mainDomain] != null)
                                    {
                                        tnDomain = tnDomain.Nodes[mainDomain];

                                        tnDomain.ContextMenuStrip = Program.FormMainInstance.contextMenu;
                                        DomainsItem itemDominio = Program.data.GetDomain(mainDomain);

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
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        /// <summary>
        /// Process metadata nodes.
        /// </summary>
        private static void NodeMetadataProcess()
        {
            TreeNode summaryNode = Program.FormMainInstance.TreeView.GetNode(Navigation.Project.DocumentAnalysis.MetadataSummary.ToNavigationPath());

            foreach (TreeNode itemNode in summaryNode.Nodes)
            {
                int separatorIndex = itemNode.Text.Trim().LastIndexOf(' ');
                string itemText;
                if (separatorIndex > 0)
                {
                    itemText = itemNode.Text.Substring(0, separatorIndex);
                }
                else
                {
                    itemText = itemNode.Text;
                }

                itemNode.Text = String.Format("{0} ({1})", itemText, ((ICollection)itemNode.Tag).Count);
            }
        }

        /// <summary>
        /// Create metadata nodes.
        /// </summary>
        private static void CreateDocumentAnalysisNodes()
        {
            if (treeView.GetNode(Navigation.Project.DocumentAnalysis.ToNavigationPath()).Nodes.Count == 0)
            {
                TreeNode nodeDocumentAnalysis = treeView.GetNode(Navigation.Project.DocumentAnalysis.ToNavigationPath());

                nodeDocumentAnalysis.Nodes.Clear();
                TreeNode tn_documents = nodeDocumentAnalysis.Nodes.Add(Navigation.Project.DocumentAnalysis.Files.Key, "Files (0/0)");
                tn_documents.ImageIndex =
                tn_documents.SelectedImageIndex = 114;

                TreeNode tn_data = nodeDocumentAnalysis.Nodes.Add(Navigation.Project.DocumentAnalysis.MetadataSummary.Key, "Metadata Summary");
                tn_data.ImageIndex =
                tn_data.SelectedImageIndex = 115;

                TreeNode tn_users = tn_data.Nodes.Add(Navigation.Project.DocumentAnalysis.MetadataSummary.Users.Key, "Users (0)");
                tn_users.ImageIndex =
                tn_users.SelectedImageIndex = 116;
                ConcurrentBag<UserItem> users = new ConcurrentBag<UserItem>();
                tn_users.Tag = users;

                TreeNode tn_folders = tn_data.Nodes.Add(Navigation.Project.DocumentAnalysis.MetadataSummary.Folders.Key, "Folders (0)");
                tn_folders.ImageIndex =
                tn_folders.SelectedImageIndex = 117;
                var folders = new ConcurrentBag<PathsItem>();
                tn_folders.Tag = folders;

                TreeNode tn_printers = tn_data.Nodes.Add(Navigation.Project.DocumentAnalysis.MetadataSummary.Printers.Key, "Printers (0)");
                tn_printers.ImageIndex =
                tn_printers.SelectedImageIndex = 118;
                var printers = new ConcurrentBag<PrintersItem>();
                tn_printers.Tag = printers;

                TreeNode tn_software = tn_data.Nodes.Add(Navigation.Project.DocumentAnalysis.MetadataSummary.Software.Key, "Software (0)");
                tn_software.ImageIndex =
                tn_software.SelectedImageIndex = 119;
                var software = new ConcurrentBag<ApplicationsItem>();
                tn_software.Tag = software;

                TreeNode tn_emails = tn_data.Nodes.Add(Navigation.Project.DocumentAnalysis.MetadataSummary.Emails.Key, "Emails (0)");
                tn_emails.ImageIndex =
                tn_emails.SelectedImageIndex = 120;
                var emails = new ConcurrentBag<EmailsItem>();
                tn_emails.Tag = emails;

                TreeNode tn_operatingsystems = tn_data.Nodes.Add(Navigation.Project.DocumentAnalysis.MetadataSummary.OperatingSystems.Key, "Operating Systems (0)");
                tn_operatingsystems.ImageIndex =
                tn_operatingsystems.SelectedImageIndex = 20;
                var operatingsystems = new ConcurrentBag<string>();
                tn_operatingsystems.Tag = operatingsystems;

                TreeNode tn_passwords = tn_data.Nodes.Add(Navigation.Project.DocumentAnalysis.MetadataSummary.Passwords.Key, "Passwords (0)");
                tn_passwords.ImageIndex =
                tn_passwords.SelectedImageIndex = 121;
                var passwords = new ConcurrentBag<PasswordsItem>();
                tn_passwords.Tag = passwords;

                TreeNode tn_servers = tn_data.Nodes.Add(Navigation.Project.DocumentAnalysis.MetadataSummary.Servers.Key, "Servers (0)");
                tn_servers.ImageIndex =
                tn_servers.SelectedImageIndex = 112;
                var servers = new ConcurrentBag<ServersItem>();
                tn_servers.Tag = servers;

                tn_data = nodeDocumentAnalysis.Nodes.Add(Navigation.Project.DocumentAnalysis.MalwareSummary.Key, "Malware Summary (DIARIO)");
                tn_data.ImageIndex =
                tn_data.SelectedImageIndex = 123;
            }
        }

        /// <summary>
        /// Create Pc Server nodes.
        /// </summary>
        static private void CreatePcServersNodes()
        {
            TreeNode networkNode = treeView.GetNode(Navigation.Project.Network.ToNavigationPath());

            TreeNode tn = networkNode.Nodes.Add(Navigation.Project.Network.Clients.Key, "Clients");
            tn.ImageIndex = tn.SelectedImageIndex = 111;

            tn = networkNode.Nodes.Add(Navigation.Project.Network.Servers.Key, "Servers");
            tn.ImageIndex = tn.SelectedImageIndex = 112;

            tn = tn.Nodes.Add(Navigation.Project.Network.Servers.Unknown.Key, "Unknown Servers");
            tn.ImageIndex = tn.SelectedImageIndex = 113;
        }

        /// <summary>
        /// Create main nodes.
        /// </summary>
        private static void CreateMainNodes()
        {
            TreeNode tnProject, tn;
            if (!treeView.Nodes.ContainsKey(Navigation.Project.Key))
            {
                tnProject = treeView.Nodes.Add(Navigation.Project.Key, Program.data.Project.ProjectName);
                tnProject.ImageIndex = tnProject.SelectedImageIndex = 107;
                tnProject.Tag = Program.data.Project.ProjectName;

                tn = tnProject.Nodes.Add(Navigation.Project.Network.Key, "Network");
                tn.ImageIndex = tn.SelectedImageIndex = 110;
                CreatePcServersNodes();

                tn = tnProject.Nodes.Add(Navigation.Project.Domains.Key, "Domains");
                tn.ImageIndex = tn.SelectedImageIndex = 108;

                tn = tnProject.Nodes.Add(Navigation.Project.DocumentAnalysis.Key, "Document Analysis");
                tn.ImageIndex = tn.SelectedImageIndex = 109;
                CreateDocumentAnalysisNodes();

                Program.FormMainInstance.TreeView.Nodes[Navigation.Project.Key].Expand();
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
