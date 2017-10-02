using System;
using System.Collections.Generic;
using System.Linq;

namespace FOCA.Controllers
{
    public class ConfigurationController : BaseController
    {
        public void Save(Configuration item)
        {
            try
            {
                using (var db = new FocaContextDb())
                {
                    var Config = db.Configurations.FirstOrDefault() ?? new Configuration();

                    Config.AvaliableTechExtensions = item.AvaliableTechExtensions;
                    Config.BingApiKey = item.BingApiKey;
                    Config.DefaultDnsCacheSnooping = item.DefaultDnsCacheSnooping;
                    Config.FingerPrintingAllFtp = item.FingerPrintingAllFtp;
                    Config.FingerPrintingAllHttp = item.FingerPrintingAllHttp;
                    Config.FingerPrintingAllSmtp = item.FingerPrintingAllSmtp;
                    Config.FingerPrintingDns = item.FingerPrintingDns;
                    Config.GoogleApiCx = item.GoogleApiCx;
                    Config.GoogleApiKey = item.GoogleApiKey;
                    Config.MaxRecursion = item.MaxRecursion;
                    Config.SimultaneousDownloads = item.SimultaneousDownloads;
                    Config.ProjectConfigFile = item.ProjectConfigFile;
                    Config.ParallelDnsQueries = item.ParallelDnsQueries;
                    Config.UseAllDns = item.UseAllDns;
                    Config.UseHead = item.UseHead;
                    Config.SelectedTechExtensions = item.SelectedTechExtensions;
                    Config.SPathsPlugins = item.SPathsPlugins;
                    Config.ShodanApiKey = item.ShodanApiKey;

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public Configuration GetConfiguration()
        {
            Configuration Config;

            Config = CurrentContextDb.Configurations.FirstOrDefault() ?? LoadDefaultConfig();

            return Config;
        }

        private Configuration LoadDefaultConfig()
        {
            var config = new Configuration
            {
                FingerPrintingAllFtp      = false,
                FingerPrintingAllHttp     = true,
                FingerPrintingAllSmtp     = true,
                FingerPrintingDns         = true,
                PassiveFingerPrintingHttp = true,
                PasiveFingerPrintingSmtp  = true,
                ResolveHost               = true,
                UseHead                   = true,
                DefaultDnsCacheSnooping   = "www.google.com",
                GoogleApiKey              = string.Empty,
                GoogleApiCx               = string.Empty,
                BingApiKey                = string.Empty,
                ShodanApiKey              = string.Empty,
                NumberOfTasks             = 15,
                MaxRecursion              = 4,
                ParallelDnsQueries        = 4,
                ProjectConfigFile         = string.Empty,
                ScanNetranges255          = true,
                SelectedTechExtensions    = new List<string>(),
                SimultaneousDownloads     = 15,
                SPathsPlugins             = string.Empty,
                UseAllDns                 = true,
                webSearcherEngine         = 2
            };


            CurrentContextDb.Configurations.Add(config);
            CurrentContextDb.SaveChanges();

            return config;
        }
    }
}
