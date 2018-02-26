using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PluginsAPI
{
    public static class Import
    {
        public enum Operation
        {
            // Actions to add data to FOCA's Core
            AddIp = 0,
            AddUrl = 1,
            AddDomain = 2,
            AssociationDomainIp = 3,
            AddContextMenu = 100
        }

        public static event EventHandler ImportEvent;

        public static void ImportEventCaller(ImportObject importObject)
        {
            ImportEvent?.Invoke(importObject, null);
        }
    }

    public class ImportObject
    {
        public object o;
        public Import.Operation operation;

        public ImportObject(Import.Operation operation, object o)
        {
            this.operation = operation;
            this.o = o;
        }
    }

    public static class SharedValues
    {
        public static List<string> FocaEmails { get; set; }
    }

    public class Export
    {
        private readonly List<object> _exportItems;

        public Export()
        {
            _exportItems = new List<object>();
        }

        public void Add(object o)
        {
            _exportItems.Add(o);
        }

        public List<object> Items()
        {
            return _exportItems;
        }
    }
}

namespace PluginsAPI.Elements
{
    public class PluginPanel
    {
        public bool fullWindow;
        public Panel panel;

        public PluginPanel(Panel p, bool fullWindow)
        {
            panel = p;
            this.fullWindow = fullWindow;
        }
    }

    public class PluginToolStripMenuItem
    {
        public ToolStripMenuItem item;

        public PluginToolStripMenuItem(ToolStripMenuItem i)
        {
            item = i;
        }
    }
}

namespace PluginsAPI.Elements.ContextualMenu
{
    public enum keyType
    {
        all = 0,
        backups = 1,
        proxy = 11,
        users = 12,
        zoneTransfer = 13
    }

    public class Global
    {
        public ToolStripMenuItem item;

        public Global(ToolStripMenuItem i)
        {
            item = i;
        }
    }

    public class ShowProjectMenu
    {
        public ToolStripMenuItem item;

        public ShowProjectMenu(ToolStripMenuItem i)
        {
            item = i;
        }
    }

    public class ShowNetworkMenu
    {
        public ToolStripMenuItem item;

        public ShowNetworkMenu(ToolStripMenuItem i)
        {
            item = i;
        }
    }

    public class ShowNetworkClientsMenu
    {
        public ToolStripMenuItem item;

        public ShowNetworkClientsMenu(ToolStripMenuItem i)
        {
            item = i;
        }
    }

    public class ShowNetworkClientsItemMenu
    {
        public ToolStripMenuItem item;

        public ShowNetworkClientsItemMenu(ToolStripMenuItem i)
        {
            item = i;
        }
    }

    public class ShowNetworkServersMenu
    {
        public ToolStripMenuItem item;

        public ShowNetworkServersMenu(ToolStripMenuItem i)
        {
            item = i;
        }
    }

    public class ShowNetworkServersItemMenu
    {
        public ToolStripMenuItem item;

        public ShowNetworkServersItemMenu(ToolStripMenuItem i)
        {
            item = i;
        }
    }

    public class ShowNetworkIpRangeMenu
    {
        public ToolStripMenuItem item;

        public ShowNetworkIpRangeMenu(ToolStripMenuItem i)
        {
            item = i;
        }
    }

    public class ShowNetworkUnlocatedMenu
    {
        public ToolStripMenuItem item;

        public ShowNetworkUnlocatedMenu(ToolStripMenuItem i)
        {
            item = i;
        }
    }

    public class ShowNetworkUnlocatedItemMenu
    {
        public ToolStripMenuItem item;

        public ShowNetworkUnlocatedItemMenu(ToolStripMenuItem i)
        {
            item = i;
        }
    }

    public class ShowDomainsMenu
    {
        public ToolStripMenuItem item;

        public ShowDomainsMenu(ToolStripMenuItem i)
        {
            item = i;
        }
    }

    public class ShowDomainsDomainMenu
    {
        public ToolStripMenuItem item;

        public ShowDomainsDomainMenu(ToolStripMenuItem i)
        {
            item = i;
        }
    }

    public class ShowDomainsDomainItemMenu
    {
        public ToolStripMenuItem item;

        public ShowDomainsDomainItemMenu(ToolStripMenuItem i)
        {
            item = i;
        }
    }

    public class ShowDomainsDomainRelatedDomainsMenu
    {
        public ToolStripMenuItem item;

        public ShowDomainsDomainRelatedDomainsMenu(ToolStripMenuItem i)
        {
            item = i;
        }
    }

    public class ShowDomainsDomainRelatedDomainsItemMenu
    {
        public ToolStripMenuItem item;

        public ShowDomainsDomainRelatedDomainsItemMenu(ToolStripMenuItem i)
        {
            item = i;
        }
    }

    public class ShowMetadataMenu
    {
        public ToolStripMenuItem item;

        public ShowMetadataMenu(ToolStripMenuItem i)
        {
            item = i;
        }
    }
}

namespace PluginsAPI.ImportElements
{
    public class Domain
    {
        public string domain;

        public Domain(string domain)
        {
            this.domain = domain;
        }
    }

    public class URL
    {
        public string url;

        public URL(string url)
        {
            this.url = url;
        }
    }

    public class IP
    {
        public string ip;

        public IP(string ip)
        {
            this.ip = ip;
        }
    }

    public class AssociationDomainIP
    {
        public string domain;
        public string ip;

        public AssociationDomainIP(string domain, string ip)
        {
            this.domain = domain;
            this.ip = ip;
        }
    }

    public class Computer
    {
        public string name;

        public Computer(string name)
        {
            this.name = name;
        }
    }

    public class Project
    {
        public string domain;
        public List<string> lstAlternativeDomains;

        public Project(string domain, List<string> lstAlternativeDomains)
        {
            this.domain = domain;
            this.lstAlternativeDomains = new List<string>();
        }
    }

    public class AddBackUp
    {
        public string file;

        public AddBackUp(string file)
        {
            this.file = file;
        }
    }

    public class AddProxy
    {
        public string ip;
        public int port;

        public AddProxy(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }
    }

    public class AddUser
    {
        public string domain;
        public string user;

        public AddUser(string domain, string user)
        {
            this.domain = domain;
            this.user = user;
        }
    }

    public class AddZoneTransfer
    {
        public string ip;

        public AddZoneTransfer(string ip)
        {
            this.ip = ip;
        }
    }
}