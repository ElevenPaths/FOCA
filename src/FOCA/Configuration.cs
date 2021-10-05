using FOCA.Analysis.FingerPrinting;
using FOCA.Database.Controllers;
using FOCA.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FOCA
{
    /// <summary>
    ///     This class contains configuration variables for FOCA
    /// </summary>
    [Serializable]
    public class Configuration
    {
        public int Id { get; set; }

        /// <summary>
        ///     Extensions to look for
        /// </summary>
        public string[] AvaliableTechExtensions { get; set; }

        /// <summary>
        ///     API key needed to use Bing Search API
        /// </summary>
        public string BingApiKey { get; set; }

        public string DuckDuckKey { get; set; }

        /// <summary>
        ///     Set default host for DNS Cache Snooping tests
        /// </summary>
        public string DefaultDnsCacheSnooping { get; set; }

        /// <summary>
        ///     Set to true to fingerprint all FTP servers that are found
        /// </summary>
        public bool FingerPrintingAllFtp { get; set; }

        /// <summary>
        ///     Set to true to fingerprint all HTTP servers that are found
        /// </summary>
        public bool FingerPrintingAllHttp { get; set; }

        /// <summary>
        ///     Set to true to fingerprint all SMTP servers that are found
        /// </summary>
        public bool FingerPrintingAllSmtp { get; set; }

        /// <summary>
        ///     Set to true to fingerprint all DNS servers that are found
        /// </summary>
        public bool FingerPrintingDns { get; set; }

        /// <summary>
        ///     This variable will store the Google Custom Search CX that will be used during the search process
        /// </summary>
        public string GoogleApiCx { get; set; }

        /// <summary>
        ///     This variable will store the Google API Key that will be used during the search process
        /// </summary>
        public string GoogleApiKey { get; set; }

        public string DiarioAPIKey { get; set; }

        public string DiarioAPISecret { get; set; }

        /// <summary>
        ///     Limit max recursivity level during search process
        /// </summary>
        public int MaxRecursion { get; set; }

        /// <summary>
        ///     Limit max number of simultaneous tasks
        /// </summary>
        public int NumberOfTasks { get; set; }

        /// <summary>
        ///     Limit max number of simultaneous DNS queries
        /// </summary>
        public int ParallelDnsQueries { get; set; }

        /// <summary>
        ///     Set to true to perform passive SMTP fingerprinting tasks against all SMTP servers found
        /// </summary>
        public bool PasiveFingerPrintingSmtp { get; set; }

        /// <summary>
        ///     Set to true to perform passive HTTP fingerprinting tasks against all HTTP servers found
        /// </summary>
        public bool PassiveFingerPrintingHttp { get; set; }

        /// <summary>
        ///     Set project configuration filename (JSON)
        /// </summary>
        public string ProjectConfigFile { get; set; }

        /// <summary>
        ///     Set to true to resolve hosts
        /// </summary>
        public bool ResolveHost { get; set; }

        /// <summary>
        ///     Set to true to limit scans to those netranges
        /// </summary>
        public bool ScanNetranges255 { get; set; }

        /// <summary>
        ///     List of technology extensions that FOCA will look for
        /// </summary>
        public List<string> SelectedTechExtensions { get; set; }

        /// <summary>
        ///     API key needed to use Shodan API
        /// </summary>
        public string ShodanApiKey { get; set; }

        /// <summary>
        ///     Set max number of simultaneous downloads (useful if your connection is slow)
        /// </summary>
        public int SimultaneousDownloads { get; set; }

        /// <summary>
        ///     Set paths of plugins, separator character is '|'
        /// </summary>
        public string SPathsPlugins { get; set; }

        /// <summary>
        ///     Set to true if you want to resolve hosts using all available DNS
        /// </summary>
        public bool UseAllDns { get; set; }

        /// <summary>
        ///     Set to true if you want to retrieve files size using a HEAD request before downloading them
        /// </summary>
        public bool UseHead { get; set; }

        /// <summary>
        ///     Set default web searcher engine. Possible values are:
        ///     0: Google Web,
        ///     1: Google API,
        ///     2: Bing Web,
        ///     3: Bing API
        /// </summary>
        public int webSearcherEngine { get; set; }

        /// <summary>
        ///     Default constructor. If you want to modify default configuration for FOCA, edit this method.
        /// </summary>
        public Configuration()
        {
            AvaliableTechExtensions = new[]
            {
                "asp", "aspx", "asmx", "do", "php", "nsf", "jsp", "swf", "exe", "pl", "cfm", "cgi", "lasso", "tpl",
                "dna",
                "jserv", "servlet", "jsf", "ssjs", "ssi", "xsp", "app", "jws", "srv"
            };
            FingerPrintingAllFtp = false;
            FingerPrintingAllHttp = true;
            FingerPrintingAllSmtp = true;
            FingerPrintingDns = true;
            PassiveFingerPrintingHttp = true;
            PasiveFingerPrintingSmtp = true;
            ResolveHost = true;
            UseHead = true;
            DefaultDnsCacheSnooping = "www.google.com";
            GoogleApiKey = "";
            GoogleApiCx = "";
            NumberOfTasks = 15;
            MaxRecursion = 4;
            ParallelDnsQueries = 4;
            ProjectConfigFile = "";
            ScanNetranges255 = true;
            SelectedTechExtensions = new List<string>();
            SimultaneousDownloads = 15;
            SPathsPlugins = "";
            UseAllDns = true;
            webSearcherEngine = 2;
        }

        /// <summary>
        /// Load configuration from file. If there's no open project, file will be 'default.json'
        /// </summary>
        public void LoadConfiguration()
        {
            var config = new ConfigurationController().GetConfiguration();

            Program.cfgCurrent = config;

#if PLUGINS
            var arPluginsPaths = SPathsPlugins.Split('|');
            foreach (var t in arPluginsPaths.Where(t => !string.IsNullOrEmpty(t)))
            {
                try
                {
                    Program.data.plugins.AddPlugin(new Plugin(t));
                }
                catch (Exception)
                {
                    SPathsPlugins = string.Empty;
                    MessageBox.Show(@"Plugin '" + t + @"' not found. Restoring plugin list as default.");
                    break;
                }
            }
#endif
            LoadFingerprintingConfiguration(Program.cfgCurrent.PassiveFingerPrintingHttp,
                Program.cfgCurrent.PasiveFingerPrintingSmtp, Program.cfgCurrent.FingerPrintingAllFtp,
                Program.cfgCurrent.FingerPrintingDns);

            Program.LogThis(new Log(Log.ModuleType.FOCA, "Loaded config", Log.LogType.debug));
        }


        #region fingerprintingEvents

        /// <summary>
        ///     Load fingerprinting configuration
        /// </summary>
        /// <param name="enableHttp"></param>
        /// <param name="enableSmtp"></param>
        /// <param name="enableFtp"></param>
        /// <param name="enableDns"></param>
        private static void LoadFingerprintingConfiguration(bool enableHttp, bool enableSmtp, bool enableFtp,
            bool enableDns)
        {
            if (!Program.data.IsAssignedHttpEvents() && enableHttp)
                Program.data.NewDomainByHTTPServer += FingerPrintingEventHandler.data_NewWebDomain;
            else if (Program.data.IsAssignedHttpEvents() && !enableHttp)
                Program.data.NewDomainByHTTPServer -= FingerPrintingEventHandler.data_NewWebDomain;

            if (!Program.data.IsAssignedSmtpEvents() && enableSmtp)
                Program.data.NewDomainByMXServer += FingerPrintingEventHandler.data_NewMXDomain;
            else if (Program.data.IsAssignedSmtpEvents() && !enableSmtp)
                Program.data.NewDomainByMXServer -= FingerPrintingEventHandler.data_NewMXDomain;

            if (!Program.data.IsAssignedFtpEvents() && enableFtp)
                Program.data.NewDomainByFTPServer += FingerPrintingEventHandler.data_NewFTPDomain;
            else if (Program.data.IsAssignedFtpEvents() && !enableFtp)
                Program.data.NewDomainByFTPServer -= FingerPrintingEventHandler.data_NewFTPDomain;

            if (!Program.data.IsAssignedDnsEvents() && enableDns)
                Program.data.NewDomainByDNSServer += FingerPrintingEventHandler.data_NewDNSDomain;
            else if (Program.data.IsAssignedDnsEvents() && !enableDns)
                Program.data.NewDomainByDNSServer -= FingerPrintingEventHandler.data_NewDNSDomain;
        }

        #endregion
    }
}