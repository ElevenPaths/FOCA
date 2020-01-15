using FOCA.Analysis.FingerPrinting;
using FOCA.Database.Entities;
using FOCA.ModifiedComponents;
using FOCA.Properties;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PluginsAPI.ImportElements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FOCA.GUI
{
    internal static class Contextual
    {
        internal const string JsonFileFilter = "JSON files (*.json)|*.json";

        private static string GetNodeText(TreeNode tn)
        {
            var sb = new StringBuilder();
            sb.Append("\"" + tn.Text + "\": " + JsonConvert.SerializeObject(
                tn.Nodes, Formatting.Indented,
                settings));
            sb.Append(",");
            return sb.ToString();
        }

        public static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DefaultValueHandling = DefaultValueHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore
        };

        private static string GetNodeTextToExport(TreeNode tn)
        {
            var sb = new StringBuilder();
            sb.Append("{\"" + tn.Text + "\": [");

            for (var i = 0; i < tn.Nodes.Count; i++)
            {
                var n = GetNodeTextToExport(tn.Nodes[i]);
                var hostname = n.Split(new[] { " [" }, StringSplitOptions.None)[0];
                var arr = hostname.ToCharArray();
                arr = Array.FindAll(arr, (c => (char.IsLetterOrDigit(c)
                                                  || char.IsWhiteSpace(c)
                                                  || c == '.' || c == ':' || c == '/')));
                hostname = new string(arr);
                var ip = n.Split(new[] { " [" }, StringSplitOptions.None)[1];
                ip = ip.Remove(ip.Length - 2, 2);
                arr = ip.ToCharArray();
                arr = Array.FindAll(arr, (c => (char.IsLetterOrDigit(c) || c == '.')));
                ip = new string(arr);
                sb.Append("{\"hostname\": \"" + hostname + "\", \"ip\": \"" + ip + "\"},");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]}");

            return sb.ToString();
        }

        private static ThreadSafeList<ComputersItem> GetNodeComputersToDelete(TreeNode tn)
        {
            var lstComputers = new ThreadSafeList<ComputersItem>();

            for (var iNode = 0; iNode < tn.Nodes.Count; iNode++)
            {
                if (tn.Nodes[iNode].Tag is ComputersItem)
                {
                    lstComputers.Add(tn.Nodes[iNode].Tag as ComputersItem);
                }
                else if (tn.Nodes[iNode].Tag.ToString() == "iprange")
                {
                    var lstToAppend = GetNodeComputersToDelete(tn.Nodes[iNode]);
                    foreach (var t in lstToAppend)
                    {
                        lstComputers.Add(t);
                    }
                }
            }

            return lstComputers;
        }

        public static void Global(TreeNode tn, Control sourceControl)
        {
            var tsiCopyClipboard = new ToolStripMenuItem("&Copy to clipboard") { Image = Resources.copytToClipboard };

            tsiCopyClipboard.Click += delegate { Clipboard.SetText(tn.Text); };

            Program.FormMainInstance.contextMenu.Items.Add(tsiCopyClipboard);
            Program.FormMainInstance.contextMenu.Items.Add(new ToolStripSeparator());

#if PLUGINS
            if (Program.FormMainInstance.ManagePluginsApi.lstContextGlobal.Count <= 0) return;
            foreach (
                var tsiPlugin in
                    Program.FormMainInstance.ManagePluginsApi.lstContextGlobal.Select(pluginMenu => pluginMenu.item))
            {
                tsiPlugin.Tag = tn.Tag;
                Program.FormMainInstance.contextMenu.Items.Add(tsiPlugin);
            }
#endif
        }

        public static void ShowProjectMenu(TreeNode tn, Control sourceControl)
        {
            var tsiNewProject = new ToolStripMenuItem("&New project") { Image = Resources.report_add };
            var tsiSave = new ToolStripMenuItem("&Save project") { Image = Resources.save };

            if (string.IsNullOrEmpty(Program.data.Project.Domain))
                tsiSave.Enabled = false;
            else
                tsiSave.Enabled = true;

            tsiNewProject.Click += delegate { Program.FormMainInstance.LoadProjectGui(true); };

            tsiSave.Click +=
                delegate { Program.FormMainInstance.ProjectManager.SaveProject(Program.data.Project.ProjectSaveFile); };

            Program.FormMainInstance.contextMenu.Items.Add(tsiNewProject);
            Program.FormMainInstance.contextMenu.Items.Add(new ToolStripSeparator());
            Program.FormMainInstance.contextMenu.Items.Add(tsiSave);

#if PLUGINS
            if (Program.FormMainInstance.ManagePluginsApi.lstContextShowProjectMenu.Count <= 0) return;
            foreach (var pluginMenu in Program.FormMainInstance.ManagePluginsApi.lstContextShowProjectMenu)
            {
                var tsiPlugin = pluginMenu.item;
                var project = new PluginsAPI.ImportElements.Project(Program.data.Project.Domain, Program.data.Project.AlternativeDomains);
                tsiPlugin.Tag = project;
                Program.FormMainInstance.contextMenu.Items.Add(tsiPlugin);
            }
#endif
        }

        public static void ShowNetworkMenu(TreeNode tn, Control sourceControl)
        {
            var tsiOptions = new ToolStripMenuItem("&Export network") { Image = Resources.exportDomain };

            tsiOptions.Click += delegate
            {
                using (SaveFileDialog sfd = new SaveFileDialog { Filter = JsonFileFilter })
                {

                    if (sfd.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    StringBuilder exportContent = new StringBuilder();
                    foreach (TreeNode node in
                            from TreeNode serverNode in
                                Program.FormMainInstance.TreeView.GetNode(Navigation.Project.Network.Servers.ToNavigationPath()).Nodes
                            from TreeNode childNode in serverNode.Nodes
                            select childNode)
                    {
                        exportContent.Append(JsonConvert.SerializeObject(node, Formatting.Indented, settings));
                    }

                    using (var sw = File.CreateText(sfd.FileName))
                    {
                        sw.WriteLine(exportContent.ToString());
                    }
                }
            };

            Program.FormMainInstance.contextMenu.Items.Add(tsiOptions);
#if PLUGINS
            if (Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkMenu.Count <= 0) return;
            foreach (
                var tsiPlugin in
                    Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkMenu.Select(
                        pluginMenu => pluginMenu.item))
            {
                tsiPlugin.Tag = tn.Text;
                Program.FormMainInstance.contextMenu.Items.Add(tsiPlugin);
            }
#endif
        }

        public static void ShowNetworkClientsMenu(TreeNode tn, Control sourceControl)
        {
            var tsiExportClients = new ToolStripMenuItem("&Export clients") { Image = Resources.exportDomain };
            var tsiAddClients = new ToolStripMenuItem("&Add client") { Image = Resources.add1 };

            tsiExportClients.Click += delegate
            {
                var sfd = new SaveFileDialog { Filter = JsonFileFilter };

                if (sfd.ShowDialog() != DialogResult.OK) return;
                var sb = new StringBuilder();

                sb.Append("{\"Clients\": [");
                foreach (
                    TreeNode tnn in
                        Program.FormMainInstance.TreeView.GetNode(Navigation.Project.Network.Clients.ToNavigationPath()).Nodes)
                    sb.Append("\"" + tnn.Text + "\",");
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]}");
                using (var sw = File.CreateText(sfd.FileName))
                {
                    sw.WriteLine(sb.ToString());
                }
            };

            tsiAddClients.Click += delegate
            {
                var fAddClient = new FormAddClient { StartPosition = FormStartPosition.CenterParent };
                fAddClient.ShowDialog();
            };

            Program.FormMainInstance.contextMenu.Items.Add(tsiExportClients);
            Program.FormMainInstance.contextMenu.Items.Add(new ToolStripSeparator());
            Program.FormMainInstance.contextMenu.Items.Add(tsiAddClients);
#if PLUGINS

            if (Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkClientsMenu.Count <= 0) return;
            foreach (
                var tsiPlugin in
                    Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkClientsMenu.Select(
                        pluginMenu => pluginMenu.item))
            {
                tsiPlugin.Tag = tn.Text;
                Program.FormMainInstance.contextMenu.Items.Add(tsiPlugin);
            }
#endif
        }

        public static void ShowNetworkClientsItemMenu(TreeNode tn, Control sourceControl)
        {
            var computer = (ComputersItem)tn.Tag;

            var tsiExportClient = new ToolStripMenuItem("Export client") { Image = Resources.report };
            var tsiRemoveClient = new ToolStripMenuItem("Remove Client") { Image = Resources.delete };
            var tsiReferingDocuments = new ToolStripMenuItem("Referring documents") { Image = Resources.link };
            var tsiModifyInformation = new ToolStripMenuItem("&Modify information") { Image = Resources.page_white_edit };
            var tsiModifySoftware = new ToolStripMenuItem("Add Software") { Image = Resources.tech };
            var tsiModifyUser = new ToolStripMenuItem("Add User") { Image = Resources.group };
            var tsiModifyDescription = new ToolStripMenuItem("Add Description") { Image = Resources.Scheduled_tasks };

            tsiModifyInformation.DropDownItems.Add(tsiModifySoftware);
            tsiModifyInformation.DropDownItems.Add(tsiModifyUser);
            tsiModifyInformation.DropDownItems.Add(tsiModifyDescription);

            tsiExportClient.Click += delegate
            {
                var sfd = new SaveFileDialog { Filter = JsonFileFilter };

                if (sfd.ShowDialog() != DialogResult.OK) return;
                var sb = new StringBuilder();
                sb.Append("{\"client\": " + JsonConvert.SerializeObject(tn.Text, Formatting.Indented, settings) + "}");
                using (var sw = File.CreateText(sfd.FileName))
                {
                    sw.WriteLine(sb.ToString());
                }
            };
            tsiRemoveClient.Click += delegate
            {
                Program.data.computers.Items.Remove(computer);
            };
            tsiModifySoftware.Click += delegate
            {
                var fModDat = new FormModifyData(computer.Software) { StartPosition = FormStartPosition.CenterParent };
                fModDat.ShowDialog();
            };
            tsiModifyUser.Click += delegate
            {
                var fModDat = new FormModifyData(computer.Users) { StartPosition = FormStartPosition.CenterParent };
                fModDat.ShowDialog();
            };
            tsiModifyDescription.Click += delegate
            {
                var fModData = new FormModifyData(computer.Description) { StartPosition = FormStartPosition.CenterParent };
                fModData.ShowDialog();
            };
            tsiReferingDocuments.Click += delegate { Program.FormMainInstance.ViewDocumentsUsedFor(computer); };

            Program.FormMainInstance.contextMenu.Items.Add(tsiExportClient);
            Program.FormMainInstance.contextMenu.Items.Add(tsiRemoveClient);
            Program.FormMainInstance.contextMenu.Items.Add(tsiModifyInformation);
            Program.FormMainInstance.contextMenu.Items.Add(tsiReferingDocuments);
#if PLUGINS

            if (Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkClientsItemMenu.Count <= 0) return;
            foreach (
                var tsiPlugin in
                    Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkClientsItemMenu.Select(
                        pluginMenu => pluginMenu.item))
            {
                tsiPlugin.Tag = tn.Tag;
                Program.FormMainInstance.contextMenu.Items.Add(tsiPlugin);
            }
#endif
        }

        public static void ShowNetworkServersMenu(TreeNode tn, Control sourceControl)
        {
            var tsiExport = new ToolStripMenuItem("&Export servers") { Image = Resources.exportDomain };
            var tsiAddElements = new ToolStripMenuItem("&Add elements") { Image = Resources.add1 };
            var tsiAddIP = new ToolStripMenuItem("&IP") { Image = Resources.computer };
            var tsiAddDomain = new ToolStripMenuItem("&Hostname") { Image = Resources.server };

            tsiAddElements.DropDownItems.Add(tsiAddIP);
            tsiAddElements.DropDownItems.Add(tsiAddDomain);

            tsiExport.Click += delegate
            {
                var sfd = new SaveFileDialog { Filter = JsonFileFilter };
                if (sfd.ShowDialog() != DialogResult.OK) return;
                var sb = new StringBuilder();
                sb.Append("{\"Servers\": [");
                foreach (
                    var node in
                        Program.FormMainInstance.TreeView.GetNode(Navigation.Project.Network.Servers.ToNavigationPath()).Nodes)
                {
                    sb.Append(GetNodeText((TreeNode)node) + ",");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                using (var sw = File.CreateText(sfd.FileName))
                {
                    sw.WriteLine(sb.ToString());
                }
            };

            tsiAddIP.Click += delegate
            {
                var fAddIp = new FormAddIp { StartPosition = FormStartPosition.CenterParent };
                fAddIp.ShowDialog();
            };

            tsiAddDomain.Click += delegate
            {
                var fAddDom = new FormAddDomain { StartPosition = FormStartPosition.CenterParent };
                fAddDom.ShowDialog();
            };

            Program.FormMainInstance.contextMenu.Items.Add(tsiExport);
            Program.FormMainInstance.contextMenu.Items.Add(new ToolStripSeparator());
            Program.FormMainInstance.contextMenu.Items.Add(tsiAddElements);

#if PLUGINS
            if (Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkServersMenu.Count <= 0) return;
            foreach (
                var tsiPlugin in
                    Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkServersMenu.Select(
                        pluginMenu => pluginMenu.item))
            {
                tsiPlugin.Tag = tn.Text;
                Program.FormMainInstance.contextMenu.Items.Add(tsiPlugin);
            }
#endif
        }

        public static void ShowNetworkServersItemMenu(TreeNode tn, Control sourceControl)
        {
            var computer = (ComputersItem)tn.Tag;

            var tsiExport = new ToolStripMenuItem("&Export server") { Image = Resources.exportDomain };
            var tsiRemove = new ToolStripMenuItem("&Remove server") { Image = Resources.delete };
            var tsiModInfo = new ToolStripMenuItem("&Add") { Image = Resources.add1 };
            var tsiModUser = new ToolStripMenuItem("&Users") { Image = Resources.group };
            var tsiModSoft = new ToolStripMenuItem("&Software") { Image = Resources.computer };
            var tsiModDesc = new ToolStripMenuItem("&Description") { Image = Resources.report_magnify };
            var tsiModIP = new ToolStripMenuItem("&IP") { Image = Resources.magnifier };

            tsiModInfo.DropDownItems.Add(tsiModUser);
            tsiModInfo.DropDownItems.Add(tsiModSoft);
            tsiModInfo.DropDownItems.Add(tsiModDesc);
            tsiModInfo.DropDownItems.Add(tsiModIP);

            var tsiFinger = new ToolStripMenuItem("&Fingerprinting") { Image = Resources.fingerprint };
            var tsiFingerHTTP = new ToolStripMenuItem("&HTTP") { Image = Resources.http };
            var tsiFingerDNS = new ToolStripMenuItem("&DNS") { Image = Resources.computer };
            var tsiFingerSMTP = new ToolStripMenuItem("&SMTP") { Image = Resources.email };
            var tsiFingerFTP = new ToolStripMenuItem("&FTP") { Image = Resources.folder };
            var tsiFingerAll = new ToolStripMenuItem("&All") { Image = Resources.door_open };

            tsiFinger.DropDownItems.Add(tsiFingerHTTP);
            tsiFinger.DropDownItems.Add(tsiFingerDNS);
            tsiFinger.DropDownItems.Add(tsiFingerSMTP);
            tsiFinger.DropDownItems.Add(tsiFingerFTP);
            tsiFinger.DropDownItems.Add(tsiFingerAll);

            tsiExport.Click += delegate
            {
                var sfd = new SaveFileDialog { Filter = JsonFileFilter };
                if (sfd.ShowDialog() != DialogResult.OK) return;
                using (var sw = File.CreateText(sfd.FileName))
                {
                    var hostname = tn.Text.Split(new[] { " [" }, StringSplitOptions.None)[0];
                    var ip = tn.Text.Split(new[] { " [" }, StringSplitOptions.None)[1];
                    ip = ip.Remove(ip.Length - 1, 1);
                    sw.WriteLine("{\"hostname\": \"" + hostname + "\", \"ip\": \"" + ip + "\"}");
                }
            };

            tsiRemove.Click += delegate
            {
                ComputerDomainsItem lastComp = null;

                foreach (var compDomItem in Program.data.computerDomains.Items.Where(C => C.Computer == computer))
                {
                    if (compDomItem.Computer != null)
                        Program.data.computers.Items.Remove(compDomItem.Computer);

                    if (compDomItem.Domain != null)
                    {
                        RelationsItem lastRel = null;
                        foreach (
                            var relationItem in Program.data.relations.Items.Where(r => r.Domain == compDomItem.Domain))
                        {
                            if (relationItem.Ip != null)
                                Program.data.Ips.Items.Remove(relationItem.Ip);

                            if (relationItem.Domain != null)
                                Program.data.domains.Items.Remove(relationItem.Domain);
                            //Program.data.relations.Items.Remove(relationItem);
                            lastRel = relationItem;
                        }
                        if (lastRel != null)
                            Program.data.relations.Items.Remove(lastRel);

                        if (compDomItem.Domain != null)
                            Program.data.domains.Items.Remove(compDomItem.Domain);
                        //Program.data.computerDomains.Items.Remove(compDomItem);
                    }

                    lastComp = compDomItem;
                }

                if (lastComp != null)
                    Program.data.computerDomains.Items.Remove(lastComp);

                Program.data.computers.Items.Remove(computer);
            };
            tsiModUser.Click += delegate
            {
                var fModData = new FormModifyData(computer.Users) { StartPosition = FormStartPosition.CenterParent };
                fModData.ShowDialog();
            };
            tsiModSoft.Click += delegate
            {
                var fModData = new FormModifyData(computer.Software) { StartPosition = FormStartPosition.CenterParent };
                fModData.ShowDialog();
            };
            tsiModDesc.Click += delegate
            {
                var fModData = new FormModifyData(computer.Description) { StartPosition = FormStartPosition.CenterParent };
                fModData.ShowDialog();
            };

            tsiModIP.Click += delegate
            {
                var fAsigIp = new FormAssignIp(computer) { StartPosition = FormStartPosition.CenterParent };
                fAsigIp.ShowDialog();
            };

            tsiFingerHTTP.Click += delegate
            {
                foreach (
                    var relation in
                        Program.data.computerIPs.Items.Where(ipItem => ipItem.Computer == computer)
                            .SelectMany(
                                ipItem => Program.data.relations.Items.Where(relation => relation.Ip.Ip == ipItem.Ip.Ip))
                    )
                {
                    FingerPrintingEventHandler.data_NewWebDomain(relation.Domain, null);
                }
            };
            tsiFingerDNS.Click += delegate
            {
                foreach (
                    var relation in
                        Program.data.computerIPs.Items.Where(ipItem => ipItem.Computer == computer)
                            .SelectMany(
                                ipItem => Program.data.relations.Items.Where(relation => relation.Ip.Ip == ipItem.Ip.Ip))
                    )
                {
                    FingerPrintingEventHandler.data_NewDNSDomain(relation.Domain, null);
                }
            };
            tsiFingerSMTP.Click += delegate
            {
                foreach (
                    var relation in
                        Program.data.computerIPs.Items.Where(ipItem => ipItem.Computer == computer)
                            .SelectMany(
                                ipItem => Program.data.relations.Items.Where(relation => relation.Ip.Ip == ipItem.Ip.Ip))
                    )
                {
                    FingerPrintingEventHandler.data_NewMXDomain(relation.Domain, null);
                }
            };
            tsiFingerFTP.Click += delegate
            {
                foreach (
                    var relation in
                        Program.data.computerIPs.Items.Where(ipItem => ipItem.Computer == computer)
                            .SelectMany(
                                ipItem => Program.data.relations.Items.Where(relation => relation.Ip.Ip == ipItem.Ip.Ip))
                    )
                {
                    FingerPrintingEventHandler.data_NewFTPDomain(relation.Domain, null);
                }
            };
            tsiFingerAll.Click += delegate
            {
                foreach (
                    var relation in
                        Program.data.computerIPs.Items.Where(ipItem => ipItem.Computer == computer)
                            .SelectMany(
                                ipItem => Program.data.relations.Items.Where(relation => relation.Ip.Ip == ipItem.Ip.Ip))
                    )
                {
                    FingerPrintingEventHandler.data_NewFTPDomain(relation.Domain, null);
                    // fingerprinting ftp
                    FingerPrintingEventHandler.data_NewDNSDomain(relation.Domain, null);
                    // fingerprinting dns
                    FingerPrintingEventHandler.data_NewFTPDomain(relation.Domain, null);
                    // fingerprinting ftp
                    FingerPrintingEventHandler.data_NewWebDomain(relation.Domain, null);
                }
            };

            Program.FormMainInstance.contextMenu.Items.Add(tsiExport);
            Program.FormMainInstance.contextMenu.Items.Add(new ToolStripSeparator());
            Program.FormMainInstance.contextMenu.Items.Add(tsiRemove);
            Program.FormMainInstance.contextMenu.Items.Add(tsiModInfo);
            Program.FormMainInstance.contextMenu.Items.Add(tsiFinger);
#if PLUGINS

            if (Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkServersItemMenu.Count <= 0) return;
            foreach (
                var pluginMenu in Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkServersItemMenu)
            {
                var tsiPlugin = pluginMenu.item;
                var computerPlugin = new Computer(computer.name);
                tsiPlugin.Tag = computerPlugin;
                Program.FormMainInstance.contextMenu.Items.Add(tsiPlugin);
            }
#endif
        }

        public static void ShowNetworkIpRangeMenu(TreeNode tn, Control sourceControl)
        {
            var tsiExport = new ToolStripMenuItem("&Export segment") { Image = Resources.exportDomain };
            var tsiRemove = new ToolStripMenuItem("&Remove segment") { Image = Resources.delete };
            var tsiScan = new ToolStripMenuItem("&Scan segment") { Image = Resources.scan };
            var tsiPing = new ToolStripMenuItem("&Ping Sweep") { Image = Resources.computer };
            var tsiBingIP = new ToolStripMenuItem("&Bing IP") { Image = Resources.bing };
            var tsiShodan = new ToolStripMenuItem("&Shodan") { Image = Resources.shodan };

            tsiScan.DropDownItems.Add(tsiPing);
            tsiScan.DropDownItems.Add(tsiBingIP);
            tsiScan.DropDownItems.Add(tsiShodan);

            if (tn.Nodes.Count > 0)
            {
                if (tn.Nodes[0].Tag.ToString() == "iprange")
                    tsiScan.Enabled = false;
            }

            var ipRange = tn.Text;

            tsiExport.Click += delegate
            {
                var sfd = new SaveFileDialog { Filter = JsonFileFilter };

                if (sfd.ShowDialog() != DialogResult.OK) return;
                var tnToExport = tn;
                using (var sw = File.CreateText(sfd.FileName))
                {
                    var sb = new StringBuilder(GetNodeTextToExport(tnToExport));
                    sw.WriteLine(sb.ToString());
                }
            };

            tsiRemove.Click += delegate
            {
                var tnToDelete = tn;
                var lstComputers = GetNodeComputersToDelete(tnToDelete);

                foreach (var comp in lstComputers)
                    Program.data.computers.Items.Remove(comp);
            };

            tsiPing.Click += delegate
            {
                var strBaseIP = ipRange.Remove(ipRange.LastIndexOf('.') + 1);

                if (Program.FormMainInstance.ScannThread != null && Program.FormMainInstance.ScannThread.IsAlive)
                    MessageBox.Show("Another thread is working please wait!", "Please wait", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                else
                {
                    var frmIP = new FormSelectIpRange(strBaseIP + "0", strBaseIP + "255");
                    if (frmIP.ShowDialog() != DialogResult.OK) return;
                    Program.FormMainInstance.ScannThread = new Thread(Program.FormMainInstance.ScanIpRangeIcmp)
                    {
                        IsBackground = true
                    };
                    Program.FormMainInstance.ScannThread.Start(new object[]
                    {frmIP.IpStart, frmIP.IpEnd, frmIP.IncludeInNetworkMap});
                }
            };

            tsiBingIP.Click += delegate
            {
                var strBaseIP = ipRange.Remove(ipRange.LastIndexOf('.') + 1);

                if (Program.FormMainInstance.ScannThread != null && Program.FormMainInstance.ScannThread.IsAlive)
                    MessageBox.Show("Another thread is working please wait!", "Please wait", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                else
                {
                    Program.FormMainInstance.ScannThread = new Thread(Program.FormMainInstance.ScanIpRangeBing)
                    {
                        IsBackground = true
                    };
                    Program.FormMainInstance.ScannThread.Start(strBaseIP);
                }
            };

            tsiShodan.Click += delegate
            {
                var strBaseIP = ipRange.Remove(ipRange.LastIndexOf('.') + 1);

                if (Program.FormMainInstance.ScannThread != null && Program.FormMainInstance.ScannThread.IsAlive)
                    MessageBox.Show("Another thread is working please wait!", "Please wait", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                else
                {
                    Program.FormMainInstance.toolStripStatusLabelLeft.Text = "Searching domains in Shodan";
                    Program.FormMainInstance.ScannThread = new Thread(Program.FormMainInstance.ScanIpRangeShodan)
                    {
                        IsBackground = true
                    };
                    Program.FormMainInstance.ScannThread.Start(strBaseIP);
                }
            };

            Program.FormMainInstance.contextMenu.Items.Add(tsiExport);
            Program.FormMainInstance.contextMenu.Items.Add(new ToolStripSeparator());
            Program.FormMainInstance.contextMenu.Items.Add(tsiRemove);
            Program.FormMainInstance.contextMenu.Items.Add(tsiScan);
#if PLUGINS

            if (Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkIpRangeMenu.Count <= 0) return;
            foreach (
                var tsiPlugin in
                    Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkIpRangeMenu.Select(
                        pluginMenu => pluginMenu.item))
            {
                tsiPlugin.Tag = tn.Text;
                Program.FormMainInstance.contextMenu.Items.Add(tsiPlugin);
            }
#endif
        }

        public static void ShowNetworkUnlocatedMenu(TreeNode tn, Control sourceControl)
        {
            var tsiExport = new ToolStripMenuItem("&Export unknown servers") { Image = Resources.exportDomain };
            var tsiRemoveAll = new ToolStripMenuItem("&Remove all") { Image = Resources.delete };

            tsiExport.Click += delegate
            {
                using (var sfd = new SaveFileDialog { Filter = JsonFileFilter })
                {
                    if (sfd.ShowDialog() != DialogResult.OK) return;

                    StringBuilder sb = new StringBuilder();
                    sb.Append("{\"Unknown servers\": [");
                    foreach (TreeNode serverNode in
                            Program.FormMainInstance.TreeView.GetNode(Navigation.Project.Network.Servers.Unknown.ToNavigationPath()).Nodes)
                        sb.Append(JsonConvert.SerializeObject(serverNode.Text, Formatting.Indented, settings) + ",");
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append("]}");
                    using (var sw = File.CreateText(sfd.FileName))
                    {
                        sw.WriteLine(sb.ToString());
                    }
                }
            };

            tsiRemoveAll.Click += delegate
            {
                foreach (
                    var computer in
                        Program.data.computers.Items.Where(c => c.type == ComputersItem.Tipo.Server).ToList())
                {
                    var isRelationshipIP = false;
                    foreach (var compIp in Program.data.computerIPs.Items.Where(compIp => compIp.Computer == computer))
                    {
                        isRelationshipIP = true;
                    }
                    if (!isRelationshipIP)
                        Program.data.computers.Items.Remove(computer);
                }
            };

            Program.FormMainInstance.contextMenu.Items.Add(tsiExport);
            Program.FormMainInstance.contextMenu.Items.Add(new ToolStripSeparator());
            Program.FormMainInstance.contextMenu.Items.Add(tsiRemoveAll);
#if PLUGINS

            if (Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkUnlocatedMenu.Count <= 0) return;
            foreach (
                var tsiPlugin in
                    Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkUnlocatedMenu.Select(
                        pluginMenu => pluginMenu.item))
            {
                tsiPlugin.Tag = tn.Text;
                Program.FormMainInstance.contextMenu.Items.Add(tsiPlugin);
            }
#endif
        }

        public static void ShowNetworkUnlocatedItemMenu(TreeNode tn, Control sourceControl)
        {
            var computer = (ComputersItem)tn.Tag;

            var tsiExport = new ToolStripMenuItem("&Export server") { Image = Resources.exportDomain };
            var tsiRemove = new ToolStripMenuItem("&Remove") { Image = Resources.delete };
            var tsiAdd = new ToolStripMenuItem("Add") { Image = Resources.add1 };
            var tsiAddUser = new ToolStripMenuItem("User") { Image = Resources.group };
            var tsiAddSoftware = new ToolStripMenuItem("Software") { Image = Resources.tech };
            var tsiAddDescription = new ToolStripMenuItem("Description") { Image = Resources.report_magnify };
            var tsiAddIP = new ToolStripMenuItem("IP") { Image = Resources.computer };

            tsiAdd.DropDownItems.Add(tsiAddUser);
            tsiAdd.DropDownItems.Add(tsiAddSoftware);
            tsiAdd.DropDownItems.Add(tsiAddDescription);
            tsiAdd.DropDownItems.Add(tsiAddIP);

            tsiExport.Click += delegate
            {
                var sfd = new SaveFileDialog { Filter = JsonFileFilter };
                if (sfd.ShowDialog() != DialogResult.OK) return;
                var sb = new StringBuilder();
                sb.Append("{\"Name\": " + JsonConvert.SerializeObject(tn.Text, Formatting.Indented, settings) + "}");
                using (var sw = File.CreateText(sfd.FileName))
                {
                    sw.WriteLine(sb.ToString());
                }
            };

            tsiRemove.Click += delegate { Program.data.computers.Items.Remove(computer); };
            tsiAddUser.Click += delegate
            {
                var fModData = new FormModifyData(computer.Users) { StartPosition = FormStartPosition.CenterParent };
                fModData.ShowDialog();
            };

            tsiAddSoftware.Click += delegate
            {
                var fModData = new FormModifyData(computer.Software) { StartPosition = FormStartPosition.CenterParent };
                fModData.ShowDialog();
            };

            tsiAddDescription.Click += delegate
            {
                var fModData = new FormModifyData(computer.Description) { StartPosition = FormStartPosition.CenterParent };
                fModData.ShowDialog();
            };

            tsiAddIP.Click += delegate
            {
                var fAsigIp = new FormAssignIp(computer) { StartPosition = FormStartPosition.CenterParent };
                fAsigIp.ShowDialog();
            };

            Program.FormMainInstance.contextMenu.Items.Add(tsiExport);
            Program.FormMainInstance.contextMenu.Items.Add(new ToolStripSeparator());
            Program.FormMainInstance.contextMenu.Items.Add(tsiRemove);
            Program.FormMainInstance.contextMenu.Items.Add(tsiAdd);
#if PLUGINS

            if (Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkUnlocatedItemMenu.Count <= 0) return;
            foreach (
                var tsiPlugin in
                    Program.FormMainInstance.ManagePluginsApi.lstContextShowNetworkUnlocatedItemMenu.Select(
                        pluginMenu => pluginMenu.item))
            {
                tsiPlugin.Tag = new Computer(computer.name);
                Program.FormMainInstance.contextMenu.Items.Add(tsiPlugin);
            }
#endif
        }

        public static void ShowDomainsMenu(TreeNode tn, Control sourceControl)
        {
            var tsiExport = new ToolStripMenuItem("&Export all domains") { Image = Resources.exportDomain };
            var tsiAddHostname = new ToolStripMenuItem("&Add hostname") { Image = Resources.add1 };
            var tsiAddMultipleHostnames = new ToolStripMenuItem("&Add multiple hostnames")
            {
                Image = Resources.add1
            };


            tsiExport.Click += delegate
            {
                var sfd = new SaveFileDialog { Filter = JsonFileFilter };

                if (sfd.ShowDialog() != DialogResult.OK) return;
                var contents = "";
                try
                {
                    var domains = Program.data.domains;
                    contents = JsonConvert.SerializeObject(domains, Formatting.Indented, settings);
                }
                catch (Exception)
                {
                    Program.LogThis(new Log(Log.ModuleType.FOCA, "Couldn't export \"Domains\" tree to a JSON file", Log.LogType.error));
                }
                using (var sw = File.CreateText(sfd.FileName))
                {
                    sw.WriteLine(contents);
                }
            };

            tsiAddHostname.Click += delegate
            {
                var fAddDom = new FormAddDomain { StartPosition = FormStartPosition.CenterParent };
                fAddDom.ShowDialog();
            };

            tsiAddMultipleHostnames.Click += delegate
            {
                var fAddMultipleDomains = new FormAddMultipleDomains { StartPosition = FormStartPosition.CenterParent };
                fAddMultipleDomains.ShowDialog();
            };

            Program.FormMainInstance.contextMenu.Items.Add(tsiExport);
            Program.FormMainInstance.contextMenu.Items.Add(new ToolStripSeparator());
            Program.FormMainInstance.contextMenu.Items.Add(tsiAddHostname);
            Program.FormMainInstance.contextMenu.Items.Add(tsiAddMultipleHostnames);
#if PLUGINS
            if (Program.FormMainInstance.ManagePluginsApi.lstContextShowDomainsMenu.Count <= 0) return;
            foreach (
                var tsiPlugin in
                    Program.FormMainInstance.ManagePluginsApi.lstContextShowDomainsMenu.Select(
                        pluginMenu => pluginMenu.item))
            {
                tsiPlugin.Tag = tn.Text;
                Program.FormMainInstance.contextMenu.Items.Add(tsiPlugin);
            }
#endif
        }

        public static void ShowDomainsDomainMenu(TreeNode tn, Control sourceControl)
        {
            var tsiExport = new ToolStripMenuItem("&Export domain") { Image = Resources.exportDomain };
            var tsiDelDomain = new ToolStripMenuItem("&Remove") { Image = Resources.delete1 };
            var tsiAddFromDNSDumpster = new ToolStripMenuItem("&Search for subdomains in dnsdumpster.com")
            {
                Image = Resources.add1
            };
            var tsiAddFromAltDNSOutput = new ToolStripMenuItem("&Import subdomains from altdns output")
            {
                Image = Resources.add1
            };

            if (Program.data.Project.Domain != null)
            {
                if (tn.Text == Program.data.Project.Domain)
                    tsiDelDomain.Enabled = false;
            }

            tsiExport.Click += delegate
            {
                var sfd = new SaveFileDialog { Filter = JsonFileFilter };

                if (sfd.ShowDialog() != DialogResult.OK) return;
                var contents = "";
                foreach (var d in Program.data.domains.Items.Where(d => d.Domain == tn.Text))
                {
                    var item = d;
                    try
                    {
                        contents = JsonConvert.SerializeObject(item, Formatting.Indented, settings);
                    }
                    catch (Exception)
                    {
                        Program.LogThis(new Log(Log.ModuleType.FOCA, $"Couldn't export domain {d.Domain} to a JSON file", Log.LogType.high));
                    }
                }
                using (var sw = File.CreateText(sfd.FileName))
                {
                    sw.WriteLine(contents);
                }
            };

            tsiDelDomain.Click += delegate
            {
                Program.data.Project.AlternativeDomains.Remove(tn.Text);
                tn.Remove();
            };

            tsiAddFromDNSDumpster.Click += delegate
            {
                var p = new DNSDumpsterParser(Program.data.Project.Domain);
                var subs = p.getSubdomains();
                foreach (
                    var t in
                        subs.Select(
                            sub =>
                                new Thread(
                                    () =>
                                        Program.data.AddDomain(sub, "Subdomain obtained from dnsdumpster.com",
                                            Program.cfgCurrent.MaxRecursion, Program.cfgCurrent))))
                {
                    t.Start();
                }
            };

            tsiAddFromAltDNSOutput.Click += delegate
            {
                var f = new FormAddAltDnsParser { StartPosition = FormStartPosition.CenterParent };
                f.ShowDialog();
            };

            Program.FormMainInstance.contextMenu.Items.Add(tsiExport);
            Program.FormMainInstance.contextMenu.Items.Add(tsiAddFromDNSDumpster);
            Program.FormMainInstance.contextMenu.Items.Add(tsiAddFromAltDNSOutput);
            Program.FormMainInstance.contextMenu.Items.Add(new ToolStripSeparator());
            Program.FormMainInstance.contextMenu.Items.Add(tsiDelDomain);
#if PLUGINS
            if (Program.FormMainInstance.ManagePluginsApi.lstContextShowDomainsDomainMenu.Count <= 0) return;
            foreach (
                var tsiPlugin in
                    Program.FormMainInstance.ManagePluginsApi.lstContextShowDomainsDomainMenu.Select(
                        pluginMenu => pluginMenu.item))
            {
                tsiPlugin.Tag = tn.Text;
                Program.FormMainInstance.contextMenu.Items.Add(tsiPlugin);
            }
#endif
        }

        public static void ShowDomainsDomainItemMenu(TreeNode tn, Control sourceControl)
        {
            var domainItem = (DomainsItem)tn.Tag;
            var tsiExport = new ToolStripMenuItem("&Export domain") { Image = Resources.exportDomain };
            var tsiDelDomain = new ToolStripMenuItem("&Remove") { Image = Resources.delete1 };
            var tsiDNSPrediction = new ToolStripMenuItem("&DNS Prediction")
            {
                Image = Resources.database_lightning
            };

            var tsiOpenBrowser = new ToolStripMenuItem("&Open in browser") { Image = Resources.openUrl };
            tsiOpenBrowser.Click += delegate
            {
                try
                {
                    var key = "htmlfile\\shell\\open\\command";
                    var registryKey = Registry.ClassesRoot.OpenSubKey(key, false);
                    var browserPath = registryKey?.GetValue(null, null).ToString().Split('"')[1];
                    Process.Start(browserPath, "http://" + domainItem.Domain);
                }
                catch
                {
                }
            };

            tsiDNSPrediction.Click +=
                delegate
                {
                    if (Program.FormMainInstance.ScannThread != null && Program.FormMainInstance.ScannThread.IsAlive)
                        MessageBox.Show("Another thread is working please wait!", "Please wait", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    else
                    {
                        var results = new List<string>();
                        Form frm = new FormDNSPrediction(Program.FormMainInstance, results, domainItem.Domain);
                        if (frm.ShowDialog() != DialogResult.OK) return;
                        Program.FormMainInstance.ScannThread = new Thread(Program.FormMainInstance.TryDomains)
                        {
                            IsBackground = true
                        };
                        Program.FormMainInstance.ScannThread.Start(results);
                    }
                };


            var tsiFinger = new ToolStripMenuItem("&Fingerprinting") { Image = Resources.fingerprint };
            var tsiFingerHTTP = new ToolStripMenuItem("&HTTP") { Image = Resources.http };
            var tsiFingerDNS = new ToolStripMenuItem("&DNS") { Image = Resources.computer };
            var tsiFingerSMTP = new ToolStripMenuItem("&SMTP") { Image = Resources.email };
            var tsiFingerFTP = new ToolStripMenuItem("&FTP") { Image = Resources.folder };
            var tsiFingerAll = new ToolStripMenuItem("&All") { Image = Resources.door_open };

            tsiFinger.DropDownItems.Add(tsiFingerHTTP);
            tsiFinger.DropDownItems.Add(tsiFingerDNS);
            tsiFinger.DropDownItems.Add(tsiFingerSMTP);
            tsiFinger.DropDownItems.Add(tsiFingerFTP);
            tsiFinger.DropDownItems.Add(tsiFingerAll);


            if (Program.data.Project.Domain != null)
            {
                if (tn.Text == Program.data.Project.Domain)
                    tsiDelDomain.Enabled = false;
            }

            tsiExport.Click += delegate
            {
                var sfd = new SaveFileDialog { Filter = JsonFileFilter };

                if (sfd.ShowDialog() != DialogResult.OK) return;
                var contents = "";
                foreach (var d in Program.data.domains.Items.Where(d => d.Domain == tn.Text))
                {
                    var item = d;
                    try
                    {
                        contents = JsonConvert.SerializeObject(item, Formatting.Indented, settings);
                    }
                    catch (Exception)
                    {
                        Program.LogThis(new Log(Log.ModuleType.FOCA, $"Couldn't export domain {d.Domain} to a JSON file", Log.LogType.high));
                    }
                }
                using (var sw = File.CreateText(sfd.FileName))
                {
                    sw.WriteLine(contents);
                }
            };

            tsiDelDomain.Click += delegate
            {
                Program.data.domains.Items.Remove(domainItem);
                tn.Remove();
            };

            tsiFingerHTTP.Click +=
                delegate
                {
                    FingerPrintingEventHandler.data_NewWebDomain(domainItem, null); // fingerprinting http
                };

            tsiFingerDNS.Click +=
                delegate
                {
                    FingerPrintingEventHandler.data_NewDNSDomain(domainItem, null); // fingerprinting dns
                };
            tsiFingerSMTP.Click +=
                delegate
                {
                    FingerPrintingEventHandler.data_NewMXDomain(domainItem, null); // fingerprinting mail
                };
            tsiFingerFTP.Click +=
                delegate
                {
                    FingerPrintingEventHandler.data_NewFTPDomain(domainItem, null); // fingerprinting ftp
                };
            tsiFingerAll.Click += delegate
            {
                FingerPrintingEventHandler.data_NewWebDomain(domainItem, null); // fingerprinting http
                FingerPrintingEventHandler.data_NewDNSDomain(domainItem, null); // fingerprinting dns
                FingerPrintingEventHandler.data_NewMXDomain(domainItem, null); // fingerprinting mail
                FingerPrintingEventHandler.data_NewFTPDomain(domainItem, null); // fingerprinting ftp
            };

            Program.FormMainInstance.contextMenu.Items.Add(tsiExport);
            Program.FormMainInstance.contextMenu.Items.Add(new ToolStripSeparator());
            Program.FormMainInstance.contextMenu.Items.Add(tsiDelDomain);
            Program.FormMainInstance.contextMenu.Items.Add(tsiOpenBrowser);
            Program.FormMainInstance.contextMenu.Items.Add(tsiDNSPrediction);
            Program.FormMainInstance.contextMenu.Items.Add(tsiFinger);
#if PLUGINS
            if (Program.FormMainInstance.ManagePluginsApi.lstContextShowDomainsDomainItemMenu.Count <= 0) return;
            foreach (
                var tsiPlugin in
                    Program.FormMainInstance.ManagePluginsApi.lstContextShowDomainsDomainItemMenu.Select(
                        pluginMenu => pluginMenu.item))
            {
                tsiPlugin.Tag = new Domain(domainItem.Domain);
                Program.FormMainInstance.contextMenu.Items.Add(tsiPlugin);
            }
#endif
        }

        public static void ShowDomainsDomainRelatedDomainsMenu(TreeNode tn, Control sourceControl)
        {
            var tsiExport = new ToolStripMenuItem("&Export") { Image = Resources.exportDomain };
            var tsiAddAlternative = new ToolStripMenuItem("&Add as alternative domain")
            {
                Image = Resources.add1
            };

            tsiExport.Click += delegate
            {
                // ToDo
            };

            tsiAddAlternative.Click += delegate
            {
                Program.data.Project.AlternativeDomains.Add(tn.Text);
                tn.Remove();
            };

            Program.FormMainInstance.contextMenu.Items.Add(tsiExport);
            Program.FormMainInstance.contextMenu.Items.Add(new ToolStripSeparator());
            Program.FormMainInstance.contextMenu.Items.Add(tsiAddAlternative);

#if PLUGINS
            if (Program.FormMainInstance.ManagePluginsApi.lstContextShowDomainsDomainRelatedDomainsMenu.Count <= 0)
                return;
            foreach (
                var tsiPlugin in
                    Program.FormMainInstance.ManagePluginsApi.lstContextShowDomainsDomainRelatedDomainsMenu.Select(
                        pluginMenu => pluginMenu.item))
            {
                tsiPlugin.Tag = tn.Text;
                Program.FormMainInstance.contextMenu.Items.Add(tsiPlugin);
            }
#endif
        }

        public static void ShowDomainsDomainRelatedDomainsItemMenu(TreeNode tn, Control sourceControl)
        {
            var tsiExport = new ToolStripMenuItem("&Export") { Image = Resources.exportDomain };

            tsiExport.Click += delegate
            {
                var sfd = new SaveFileDialog();
                if (sfd.ShowDialog() != DialogResult.OK) return;
                var tnToExport = tn;
                using (var sw = File.CreateText(sfd.FileName))
                {
                    var sb = new StringBuilder(GetNodeTextToExport(tnToExport));
                    sw.WriteLine(sb.ToString());
                }
            };

            Program.FormMainInstance.contextMenu.Items.Add(tsiExport);
#if PLUGINS
            if (Program.FormMainInstance.ManagePluginsApi.lstContextShowDomainsDomainRelatedDomainsItemMenu.Count <= 0)
                return;
            foreach (
                var tsiPlugin in
                    Program.FormMainInstance.ManagePluginsApi.lstContextShowDomainsDomainRelatedDomainsItemMenu.Select(
                        pluginMenu => pluginMenu.item))
            {
                tsiPlugin.Tag = tn.Text;
                Program.FormMainInstance.contextMenu.Items.Add(tsiPlugin);
            }
#endif
        }

        public static void ShowMetadataMenu(TreeNode tn, Control sourceControl)
        {
            var tsiExport = new ToolStripMenuItem("&Export") { Image = Resources.exportDomain };
            var tsiAnalyze = new ToolStripMenuItem("&Analyze") { Image = Resources.analyzeMetadata };

            tsiExport.Click += delegate
            {
                var sfd = new SaveFileDialog { Filter = JsonFileFilter };

                if (sfd.ShowDialog() != DialogResult.OK) return;
                var contents = "";
                var sb = new StringBuilder();
                try
                {
                    sb.Append(JsonConvert.SerializeObject(Program.data.files, Formatting.Indented, settings));
                }
                catch (Exception)
                {
                    Program.LogThis(new Log(Log.ModuleType.FOCA, "Couldn't export \"Metadata\" tree to a JSON file", Log.LogType.error));
                }
                contents = sb.ToString();
                using (var sw = File.CreateText(sfd.FileName))
                {
                    sw.WriteLine(contents);
                }
            };

            tsiAnalyze.Click += delegate { Program.FormMainInstance.panelMetadataSearch.AnalyzeMetadata(); };

            Program.FormMainInstance.contextMenu.Items.Add(tsiExport);
            Program.FormMainInstance.contextMenu.Items.Add(new ToolStripSeparator());
            Program.FormMainInstance.contextMenu.Items.Add(tsiAnalyze);
        }
    }
}