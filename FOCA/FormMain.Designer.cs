using System.Windows.Forms;
using FOCA;
using System.Reflection;

namespace FOCA
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null) && (ManagePluginsApi != null))
            {
                ManagePluginsApi.Dispose();
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.imgIcons = new System.Windows.Forms.ImageList(this.components);
            this.menuStripMain = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemProject = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemNewProject = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemSaveProject = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemClose = new System.Windows.Forms.ToolStripMenuItem();
            this.pluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadUnloadPluginsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.dNSSnoopingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemOptions = new System.Windows.Forms.ToolStripMenuItem();
            this.taskListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMarket = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripNetwork = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemViewDocumentsUsedFor = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripDocuments = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openDocumentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.removeDocumentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sfdSaveProject = new System.Windows.Forms.SaveFileDialog();
            this.ofdOpenProject = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolStripProgressBarDownload = new System.Windows.Forms.ToolStripProgressBar();
            this.statusStripMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelLeft = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripDropDownButtonStop = new System.Windows.Forms.ToolStripDropDownButton();
            this.sfdExport = new System.Windows.Forms.SaveFileDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.TreeView = new FOCA.ModifiedComponents.TreeViewNoFlickering();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.PanelIntroduccion = new System.Windows.Forms.Panel();
            this.panelProject = new FOCA.PanelProject();
            this.panelDnsSnooping = new FOCA.PanelDnsSnooping();
            this.panelTasks = new FOCA.PanelTasks();
            this.panelMetadataSearch = new FOCA.PanelMetadataSearch();
            this.panelDNSSearch = new FOCA.PanelDNSSearch();
            this.panelInformation = new FOCA.PanelInformation();
            this.pictureBoxAdvert = new System.Windows.Forms.PictureBox();
            this.panelLogs = new FOCA.PanelLogs();
            this.menuStripMain.SuspendLayout();
            this.contextMenuStripNetwork.SuspendLayout();
            this.contextMenuStripDocuments.SuspendLayout();
            this.statusStripMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAdvert)).BeginInit();
            this.SuspendLayout();
            // 
            // imgIcons
            // 
            this.imgIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgIcons.ImageStream")));
            this.imgIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imgIcons.Images.SetKeyName(0, "openoffice.png");
            this.imgIcons.Images.SetKeyName(1, "doc.ico");
            this.imgIcons.Images.SetKeyName(2, "doc.ico");
            this.imgIcons.Images.SetKeyName(3, "report_user.png");
            this.imgIcons.Images.SetKeyName(4, "report_edit.png");
            this.imgIcons.Images.SetKeyName(5, "page_white_stack.png");
            this.imgIcons.Images.SetKeyName(6, "sitemap_color.png");
            this.imgIcons.Images.SetKeyName(7, "doc.ico");
            this.imgIcons.Images.SetKeyName(8, "pdf.png");
            this.imgIcons.Images.SetKeyName(9, "ppt.ico");
            this.imgIcons.Images.SetKeyName(10, "xls.ico");
            this.imgIcons.Images.SetKeyName(11, "OO.png");
            this.imgIcons.Images.SetKeyName(12, "computer.png");
            this.imgIcons.Images.SetKeyName(13, "folder.png");
            this.imgIcons.Images.SetKeyName(14, "group.png");
            this.imgIcons.Images.SetKeyName(15, "printer.png");
            this.imgIcons.Images.SetKeyName(16, "linux.ico");
            this.imgIcons.Images.SetKeyName(17, "paths.ico");
            this.imgIcons.Images.SetKeyName(18, "shares.ico");
            this.imgIcons.Images.SetKeyName(19, "users.ico");
            this.imgIcons.Images.SetKeyName(20, "windows.ico");
            this.imgIcons.Images.SetKeyName(21, "world.png");
            this.imgIcons.Images.SetKeyName(22, "page_white.png");
            this.imgIcons.Images.SetKeyName(23, "email.png");
            this.imgIcons.Images.SetKeyName(24, "date.png");
            this.imgIcons.Images.SetKeyName(25, "page_white_error.png");
            this.imgIcons.Images.SetKeyName(26, "page_white_zip.png");
            this.imgIcons.Images.SetKeyName(27, "page_white_edit.png");
            this.imgIcons.Images.SetKeyName(28, "apple.png");
            this.imgIcons.Images.SetKeyName(29, "wpd.png");
            this.imgIcons.Images.SetKeyName(30, "software-small-icon.png");
            this.imgIcons.Images.SetKeyName(31, "page_white_star.png");
            this.imgIcons.Images.SetKeyName(32, "page_white_picture.png");
            this.imgIcons.Images.SetKeyName(33, "picture_error.png");
            this.imgIcons.Images.SetKeyName(34, "picture.png");
            this.imgIcons.Images.SetKeyName(35, "WindowsVista.png");
            this.imgIcons.Images.SetKeyName(36, "Windows7.png");
            this.imgIcons.Images.SetKeyName(37, "Windows 2000.png");
            this.imgIcons.Images.SetKeyName(38, "Windows2003.png");
            this.imgIcons.Images.SetKeyName(39, "Windows2008.png");
            this.imgIcons.Images.SetKeyName(40, "WindowsXP.png");
            this.imgIcons.Images.SetKeyName(41, "WindowsNT4.0.png");
            this.imgIcons.Images.SetKeyName(42, "folder_link.png");
            this.imgIcons.Images.SetKeyName(43, "group_link.png");
            this.imgIcons.Images.SetKeyName(44, "printer_link.png");
            this.imgIcons.Images.SetKeyName(45, "server.png");
            this.imgIcons.Images.SetKeyName(46, "server_error.png");
            this.imgIcons.Images.SetKeyName(47, "freebsd.png");
            this.imgIcons.Images.SetKeyName(48, "centos.png");
            this.imgIcons.Images.SetKeyName(49, "solaris.png");
            this.imgIcons.Images.SetKeyName(50, "openbsd.png");
            this.imgIcons.Images.SetKeyName(51, "svg.png");
            this.imgIcons.Images.SetKeyName(52, "redhat.png");
            this.imgIcons.Images.SetKeyName(53, "fedora.png");
            this.imgIcons.Images.SetKeyName(54, "mandrake.png");
            this.imgIcons.Images.SetKeyName(55, "mandriva.png");
            this.imgIcons.Images.SetKeyName(56, "suse.png");
            this.imgIcons.Images.SetKeyName(57, "ubuntu.png");
            this.imgIcons.Images.SetKeyName(58, "debian.png");
            this.imgIcons.Images.SetKeyName(59, "network.png");
            this.imgIcons.Images.SetKeyName(60, "eye.png");
            this.imgIcons.Images.SetKeyName(61, "indesign.png");
            this.imgIcons.Images.SetKeyName(62, "http.png");
            this.imgIcons.Images.SetKeyName(63, "https.png");
            this.imgIcons.Images.SetKeyName(64, "dns.png");
            this.imgIcons.Images.SetKeyName(65, "FTP.png");
            this.imgIcons.Images.SetKeyName(66, "icono_google.gif");
            this.imgIcons.Images.SetKeyName(67, "method.png");
            this.imgIcons.Images.SetKeyName(68, "proxy.png");
            this.imgIcons.Images.SetKeyName(69, "terminal.png");
            this.imgIcons.Images.SetKeyName(70, "vpn.png");
            this.imgIcons.Images.SetKeyName(71, "balanza.gif");
            this.imgIcons.Images.SetKeyName(72, "firewall_16.gif");
            this.imgIcons.Images.SetKeyName(73, "Ips.png");
            this.imgIcons.Images.SetKeyName(74, "voip.gif");
            this.imgIcons.Images.SetKeyName(75, "password.gif");
            this.imgIcons.Images.SetKeyName(76, "rdp.gif");
            this.imgIcons.Images.SetKeyName(77, "ldap.png");
            this.imgIcons.Images.SetKeyName(78, "skull.gif");
            this.imgIcons.Images.SetKeyName(79, "proxy.png");
            this.imgIcons.Images.SetKeyName(80, "ldap.png");
            this.imgIcons.Images.SetKeyName(81, "kerberos.png");
            this.imgIcons.Images.SetKeyName(82, "finger.png");
            this.imgIcons.Images.SetKeyName(83, "firewall.png");
            this.imgIcons.Images.SetKeyName(84, "active direcory.PNG");
            this.imgIcons.Images.SetKeyName(85, "bombica.jpg");
            this.imgIcons.Images.SetKeyName(86, "add.png");
            this.imgIcons.Images.SetKeyName(87, "delete.png");
            this.imgIcons.Images.SetKeyName(88, "bing.png");
            this.imgIcons.Images.SetKeyName(89, "shodan.png");
            this.imgIcons.Images.SetKeyName(90, "server.png");
            this.imgIcons.Images.SetKeyName(91, "computer.png");
            this.imgIcons.Images.SetKeyName(92, "");
            this.imgIcons.Images.SetKeyName(93, "report.png");
            this.imgIcons.Images.SetKeyName(94, "fingerprint.png");
            this.imgIcons.Images.SetKeyName(95, "report_add.png");
            this.imgIcons.Images.SetKeyName(96, "report_disk.png");
            this.imgIcons.Images.SetKeyName(97, "report_magnify.png");
            this.imgIcons.Images.SetKeyName(98, "leaks.png");
            this.imgIcons.Images.SetKeyName(99, "ico_computer.gif");
            this.imgIcons.Images.SetKeyName(100, "IC191465.gif");
            this.imgIcons.Images.SetKeyName(101, "choices.gif");
            this.imgIcons.Images.SetKeyName(102, "svn.gif");
            this.imgIcons.Images.SetKeyName(103, "fruta.gif");
            this.imgIcons.Images.SetKeyName(104, "arbol.gif");
            this.imgIcons.Images.SetKeyName(105, "php.gif");
            this.imgIcons.Images.SetKeyName(106, "net.iis.small.png");
            this.imgIcons.Images.SetKeyName(107, "no_project.png");
            this.imgIcons.Images.SetKeyName(108, "domains.png");
            this.imgIcons.Images.SetKeyName(109, "metadata.png");
            this.imgIcons.Images.SetKeyName(110, "network.png");
            this.imgIcons.Images.SetKeyName(111, "Clients.png");
            this.imgIcons.Images.SetKeyName(112, "Servers.png");
            this.imgIcons.Images.SetKeyName(113, "UnlocatedServers.png");
            this.imgIcons.Images.SetKeyName(114, "documents.png");
            this.imgIcons.Images.SetKeyName(115, "MetadaSummary.png");
            this.imgIcons.Images.SetKeyName(116, "Users.png");
            this.imgIcons.Images.SetKeyName(117, "Folders.png");
            this.imgIcons.Images.SetKeyName(118, "Printers.png");
            this.imgIcons.Images.SetKeyName(119, "Software.png");
            this.imgIcons.Images.SetKeyName(120, "Emails.png");
            this.imgIcons.Images.SetKeyName(121, "Passwords.png");
            this.imgIcons.Images.SetKeyName(122, "iconfinder_advantage_nearby_1034361.png");
            // 
            // menuStripMain
            // 
            this.menuStripMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemProject,
            this.pluginsToolStripMenuItem,
            this.toolStripMenuItemOptions,
            this.taskListToolStripMenuItem,
            this.toolStripMenuItemAbout,
            this.toolStripMenuItemMarket});
            resources.ApplyResources(this.menuStripMain, "menuStripMain");
            this.menuStripMain.Name = "menuStripMain";
            this.menuStripMain.MenuActivate += new System.EventHandler(this.SetItemsMenu);
            // 
            // toolStripMenuItemProject
            // 
            this.toolStripMenuItemProject.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemNewProject,
            this.toolStripSeparator1,
            this.toolStripMenuItemSaveProject,
            this.toolStripSeparator3,
            this.toolStripMenuItemClose});
            resources.ApplyResources(this.toolStripMenuItemProject, "toolStripMenuItemProject");
            this.toolStripMenuItemProject.Name = "toolStripMenuItemProject";
            // 
            // toolStripMenuItemNewProject
            // 
            resources.ApplyResources(this.toolStripMenuItemNewProject, "toolStripMenuItemNewProject");
            this.toolStripMenuItemNewProject.Name = "toolStripMenuItemNewProject";
            this.toolStripMenuItemNewProject.Click += new System.EventHandler(this.toolStripMenuItemNewProject_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripMenuItemSaveProject
            // 
            resources.ApplyResources(this.toolStripMenuItemSaveProject, "toolStripMenuItemSaveProject");
            this.toolStripMenuItemSaveProject.Image = global::FOCA.Properties.Resources.save;
            this.toolStripMenuItemSaveProject.Name = "toolStripMenuItemSaveProject";
            this.toolStripMenuItemSaveProject.Click += new System.EventHandler(this.toolStripMenuItemSaveProject_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // toolStripMenuItemClose
            // 
            resources.ApplyResources(this.toolStripMenuItemClose, "toolStripMenuItemClose");
            this.toolStripMenuItemClose.Name = "toolStripMenuItemClose";
            this.toolStripMenuItemClose.Click += new System.EventHandler(this.toolStripMenuItemClose_Click);
            // 
            // pluginsToolStripMenuItem
            // 
            this.pluginsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadUnloadPluginsToolStripMenuItem,
            this.toolStripSeparator4,
            this.dNSSnoopingToolStripMenuItem});
            resources.ApplyResources(this.pluginsToolStripMenuItem, "pluginsToolStripMenuItem");
            this.pluginsToolStripMenuItem.Name = "pluginsToolStripMenuItem";
            // 
            // loadUnloadPluginsToolStripMenuItem
            // 
            resources.ApplyResources(this.loadUnloadPluginsToolStripMenuItem, "loadUnloadPluginsToolStripMenuItem");
            this.loadUnloadPluginsToolStripMenuItem.Name = "loadUnloadPluginsToolStripMenuItem";
            this.loadUnloadPluginsToolStripMenuItem.Click += new System.EventHandler(this.loadUnloadPluginsToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // dNSSnoopingToolStripMenuItem
            // 
            resources.ApplyResources(this.dNSSnoopingToolStripMenuItem, "dNSSnoopingToolStripMenuItem");
            this.dNSSnoopingToolStripMenuItem.Name = "dNSSnoopingToolStripMenuItem";
            this.dNSSnoopingToolStripMenuItem.Click += new System.EventHandler(this.dNSSnoopingToolStripMenuItem_Click_1);
            // 
            // toolStripMenuItemOptions
            // 
            resources.ApplyResources(this.toolStripMenuItemOptions, "toolStripMenuItemOptions");
            this.toolStripMenuItemOptions.Name = "toolStripMenuItemOptions";
            this.toolStripMenuItemOptions.Click += new System.EventHandler(this.toolStripMenuItemOptions_Click);
            // 
            // taskListToolStripMenuItem
            // 
            resources.ApplyResources(this.taskListToolStripMenuItem, "taskListToolStripMenuItem");
            this.taskListToolStripMenuItem.Name = "taskListToolStripMenuItem";
            this.taskListToolStripMenuItem.Click += new System.EventHandler(this.taskListToolStripMenuItem_Click);
            // 
            // toolStripMenuItemAbout
            // 
            resources.ApplyResources(this.toolStripMenuItemAbout, "toolStripMenuItemAbout");
            this.toolStripMenuItemAbout.Name = "toolStripMenuItemAbout";
            this.toolStripMenuItemAbout.Click += new System.EventHandler(this.toolStripMenuItemAbout_Click);
            // 
            // toolStripMenuItemMarket
            // 
            resources.ApplyResources(this.toolStripMenuItemMarket, "toolStripMenuItemMarket");
            this.toolStripMenuItemMarket.Name = "toolStripMenuItemMarket";
            this.toolStripMenuItemMarket.Click += new System.EventHandler(this.toolStripMenuItemMarket_Click);
            // 
            // contextMenuStripNetwork
            // 
            this.contextMenuStripNetwork.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStripNetwork.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemViewDocumentsUsedFor});
            this.contextMenuStripNetwork.Name = "contextMenuStripSearch";
            resources.ApplyResources(this.contextMenuStripNetwork, "contextMenuStripNetwork");
            this.contextMenuStripNetwork.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripNetwork_Opening);
            // 
            // toolStripMenuItemViewDocumentsUsedFor
            // 
            resources.ApplyResources(this.toolStripMenuItemViewDocumentsUsedFor, "toolStripMenuItemViewDocumentsUsedFor");
            this.toolStripMenuItemViewDocumentsUsedFor.Name = "toolStripMenuItemViewDocumentsUsedFor";
            // 
            // contextMenuStripDocuments
            // 
            this.contextMenuStripDocuments.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStripDocuments.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDocumentToolStripMenuItem,
            this.toolStripSeparator8,
            this.removeDocumentToolStripMenuItem});
            this.contextMenuStripDocuments.Name = "contextMenuStripNodes";
            resources.ApplyResources(this.contextMenuStripDocuments, "contextMenuStripDocuments");
            // 
            // openDocumentToolStripMenuItem
            // 
            resources.ApplyResources(this.openDocumentToolStripMenuItem, "openDocumentToolStripMenuItem");
            this.openDocumentToolStripMenuItem.Name = "openDocumentToolStripMenuItem";
            this.openDocumentToolStripMenuItem.Click += new System.EventHandler(this.openDocumentToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // removeDocumentToolStripMenuItem
            // 
            resources.ApplyResources(this.removeDocumentToolStripMenuItem, "removeDocumentToolStripMenuItem");
            this.removeDocumentToolStripMenuItem.Name = "removeDocumentToolStripMenuItem";
            this.removeDocumentToolStripMenuItem.Click += new System.EventHandler(this.removeDocumentToolStripMenuItem_Click);
            // 
            // sfdSaveProject
            // 
            this.sfdSaveProject.DefaultExt = "FOCA";
            resources.ApplyResources(this.sfdSaveProject, "sfdSaveProject");
            // 
            // ofdOpenProject
            // 
            resources.ApplyResources(this.ofdOpenProject, "ofdOpenProject");
            // 
            // toolStripProgressBarDownload
            // 
            this.toolStripProgressBarDownload.Name = "toolStripProgressBarDownload";
            resources.ApplyResources(this.toolStripProgressBarDownload, "toolStripProgressBarDownload");
            this.toolStripProgressBarDownload.Step = 1;
            this.toolStripProgressBarDownload.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // statusStripMain
            // 
            this.statusStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStripMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelLeft,
            this.toolStripProgressBarDownload,
            this.toolStripDropDownButtonStop});
            resources.ApplyResources(this.statusStripMain, "statusStripMain");
            this.statusStripMain.Name = "statusStripMain";
            this.statusStripMain.Stretch = false;
            // 
            // toolStripStatusLabelLeft
            // 
            resources.ApplyResources(this.toolStripStatusLabelLeft, "toolStripStatusLabelLeft");
            this.toolStripStatusLabelLeft.Name = "toolStripStatusLabelLeft";
            // 
            // toolStripDropDownButtonStop
            // 
            this.toolStripDropDownButtonStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripDropDownButtonStop, "toolStripDropDownButtonStop");
            this.toolStripDropDownButtonStop.Name = "toolStripDropDownButtonStop";
            this.toolStripDropDownButtonStop.ShowDropDownArrow = false;
            this.toolStripDropDownButtonStop.Click += new System.EventHandler(this.toolStripDropDownButtonAbort_Click);
            // 
            // sfdExport
            // 
            this.sfdExport.DefaultExt = "txt";
            resources.ApplyResources(this.sfdExport, "sfdExport");
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainerMain);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelLogs);
            // 
            // splitContainerMain
            // 
            resources.ApplyResources(this.splitContainerMain, "splitContainerMain");
            this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.TreeView);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.PanelIntroduccion);
            this.splitContainerMain.Panel2.Controls.Add(this.panelProject);
            this.splitContainerMain.Panel2.Controls.Add(this.panelDnsSnooping);
            this.splitContainerMain.Panel2.Controls.Add(this.panelTasks);
            this.splitContainerMain.Panel2.Controls.Add(this.panelMetadataSearch);
            this.splitContainerMain.Panel2.Controls.Add(this.panelDNSSearch);
            this.splitContainerMain.Panel2.Controls.Add(this.panelInformation);
            this.splitContainerMain.Panel2.Controls.Add(this.pictureBoxAdvert);
            this.splitContainerMain.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainerMain_Paint);
            this.splitContainerMain.Resize += new System.EventHandler(this.splitContainerMain_Resize);
            // 
            // TreeView
            // 
            this.TreeView.ContextMenuStrip = this.contextMenu;
            resources.ApplyResources(this.TreeView, "TreeView");
            this.TreeView.ImageList = this.imgIcons;
            this.TreeView.Name = "TreeView";
            this.TreeView.Tag = "";
            this.TreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.TreeView_AfterSelect);
            this.TreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeView_NodeMouseClick_1);
            // 
            // contextMenu
            // 
            this.contextMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenu.Name = "contextMenu";
            resources.ApplyResources(this.contextMenu, "contextMenu");
            this.contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenu_Opening);
            // 
            // PanelIntroduccion
            // 
            resources.ApplyResources(this.PanelIntroduccion, "PanelIntroduccion");
            this.PanelIntroduccion.Name = "PanelIntroduccion";
            // 
            // panelProject
            // 
            this.panelProject.AlternativeDomains = "";
            resources.ApplyResources(this.panelProject, "panelProject");
            this.panelProject.DomainWebsite = "";
            this.panelProject.FolderDocuments = "";
            this.panelProject.Name = "panelProject";
            this.panelProject.Notes = "";
            this.panelProject.ProjectName = "";
            // 
            // panelDnsSnooping
            // 
            resources.ApplyResources(this.panelDnsSnooping, "panelDnsSnooping");
            this.panelDnsSnooping.Name = "panelDnsSnooping";
            // 
            // panelTasks
            // 
            resources.ApplyResources(this.panelTasks, "panelTasks");
            this.panelTasks.Name = "panelTasks";
            // 
            // panelMetadataSearch
            // 
            resources.ApplyResources(this.panelMetadataSearch, "panelMetadataSearch");
            this.panelMetadataSearch.Name = "panelMetadataSearch";
            // 
            // panelDNSSearch
            // 
            resources.ApplyResources(this.panelDNSSearch, "panelDNSSearch");
            this.panelDNSSearch.Name = "panelDNSSearch";
            // 
            // panelInformation
            // 
            resources.ApplyResources(this.panelInformation, "panelInformation");
            this.panelInformation.Name = "panelInformation";
            // 
            // pictureBoxAdvert
            // 
            resources.ApplyResources(this.pictureBoxAdvert, "pictureBoxAdvert");
            this.pictureBoxAdvert.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBoxAdvert.Name = "pictureBoxAdvert";
            this.pictureBoxAdvert.TabStop = false;
            // 
            // panelLogs
            // 
            resources.ApplyResources(this.panelLogs, "panelLogs");
            this.panelLogs.Name = "panelLogs";
            // 
            // FormMain
            // 
            this.AllowDrop = true;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStripMain);
            this.Controls.Add(this.menuStripMain);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStripMain;
            this.Name = "FormMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formMain_FormClosing);
            this.Load += new System.EventHandler(this.formMain_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormMain_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormMain_DragEnter);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyUp);
            this.menuStripMain.ResumeLayout(false);
            this.menuStripMain.PerformLayout();
            this.contextMenuStripNetwork.ResumeLayout(false);
            this.contextMenuStripDocuments.ResumeLayout(false);
            this.statusStripMain.ResumeLayout(false);
            this.statusStripMain.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAdvert)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.ToolStripMenuItem toolStripMenuItemProject;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAbout;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemClose;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemNewProject;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOptions;
        public System.Windows.Forms.ContextMenuStrip contextMenuStripDocuments;
        private System.Windows.Forms.ToolStripMenuItem removeDocumentToolStripMenuItem;
        public System.Windows.Forms.ContextMenuStrip contextMenuStripNetwork;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemViewDocumentsUsedFor;
        private System.Windows.Forms.ToolStripMenuItem openDocumentToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        public System.Windows.Forms.SaveFileDialog sfdSaveProject;
        public System.Windows.Forms.PictureBox pictureBoxAdvert;
        private System.Windows.Forms.ToolTip toolTip1;
        public PanelMetadataSearch panelMetadataSearch;
        public PanelInformation panelInformation;
        public PanelLogs panelLogs;
        public PanelDNSSearch panelDNSSearch;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelLeft;
        public System.Windows.Forms.ToolStripProgressBar toolStripProgressBarDownload;
        public System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonStop;
        private System.Windows.Forms.StatusStrip statusStripMain;
        private System.Windows.Forms.SaveFileDialog sfdExport;
        public ImageList imgIcons;
        private ToolStripMenuItem taskListToolStripMenuItem;
        private PanelTasks panelTasks;
        public FOCA.ModifiedComponents.TreeViewNoFlickering TreeView;
        private SplitContainer splitContainer1;
        public ContextMenuStrip contextMenu;
        private Panel PanelIntroduccion;
        public OpenFileDialog ofdOpenProject;
        public PanelProject panelProject;
        public PanelDnsSnooping panelDnsSnooping;
        public MenuStrip menuStripMain;
        public SplitContainer splitContainerMain;
        private ToolStripMenuItem loadUnloadPluginsToolStripMenuItem;
        public ToolStripMenuItem pluginsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem dNSSnoopingToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItemMarket;
        private ToolStripMenuItem toolStripMenuItemSaveProject;
    }
}

