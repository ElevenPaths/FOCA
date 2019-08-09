using System;
using System.Collections.Generic;
using System.Linq;

namespace FOCA.Database.Controllers
{
    public class ConfigurationController : BaseController<Configuration>
    {
        public void Save(Configuration item)
        {
            try
            {
                using (FocaContextDb context = new FocaContextDb())
                {
                    var config = context.Configurations.FirstOrDefault() ?? new Configuration();

                    config.AvaliableTechExtensions = item.AvaliableTechExtensions;
                    config.BingApiKey = item.BingApiKey;
                    config.DefaultDnsCacheSnooping = item.DefaultDnsCacheSnooping;
                    config.FingerPrintingAllFtp = item.FingerPrintingAllFtp;
                    config.FingerPrintingAllHttp = item.FingerPrintingAllHttp;
                    config.FingerPrintingAllSmtp = item.FingerPrintingAllSmtp;
                    config.FingerPrintingDns = item.FingerPrintingDns;
                    config.GoogleApiCx = item.GoogleApiCx;
                    config.GoogleApiKey = item.GoogleApiKey;
                    config.MaxRecursion = item.MaxRecursion;
                    config.SimultaneousDownloads = item.SimultaneousDownloads;
                    config.ProjectConfigFile = item.ProjectConfigFile;
                    config.ParallelDnsQueries = item.ParallelDnsQueries;
                    config.UseAllDns = item.UseAllDns;
                    config.UseHead = item.UseHead;
                    config.SelectedTechExtensions = item.SelectedTechExtensions;
                    config.SPathsPlugins = item.SPathsPlugins;
                    config.ShodanApiKey = item.ShodanApiKey;

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Configuration GetConfiguration()
        {
            using (FocaContextDb context = new FocaContextDb())
            {
                Configuration config = context.Configurations.FirstOrDefault() ?? LoadDefaultConfig();

                return config;
            }
        }

        private Configuration LoadDefaultConfig()
        {
            var config = new Configuration
            {
                FingerPrintingAllFtp = false,
                FingerPrintingAllHttp = true,
                FingerPrintingAllSmtp = true,
                FingerPrintingDns = true,
                PassiveFingerPrintingHttp = true,
                PasiveFingerPrintingSmtp = true,
                ResolveHost = true,
                UseHead = true,
                DefaultDnsCacheSnooping = "www.google.com",
                GoogleApiKey = string.Empty,
                GoogleApiCx = string.Empty,
                BingApiKey = string.Empty,
                ShodanApiKey = string.Empty,
                NumberOfTasks = 15,
                MaxRecursion = 4,
                ParallelDnsQueries = 4,
                ProjectConfigFile = string.Empty,
                ScanNetranges255 = true,
                SelectedTechExtensions = new List<string>(),
                SimultaneousDownloads = 15,
                SPathsPlugins = string.Empty,
                UseAllDns = true,
                webSearcherEngine = 2
            };

            using (FocaContextDb context = new FocaContextDb())
            {
                context.Configurations.Add(config);
                context.SaveChanges();
            }

            return config;
        }
    }
}
