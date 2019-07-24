using FOCA.Database.Entities;
using FOCA.Threads;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FOCA.Analysis.Technology
{
    [Serializable]
    public class Technology
    {
        public string extension;
        public List<string> urls;

        public Technology()
        {
        }

        public Technology(string extension)
        {
            this.extension = extension;

            urls = new List<string>();
        }

        public void AddURL(string url)
        {
            urls.Add(url);
        }

        public List<string> GetURLs()
        {
            return urls;
        }
    }

    [Serializable]
    public class TechnologyAnalysis
    {
        public List<Technology> SelectedTechnologies { get; private set; }

        [NonSerialized]
        private FOCA.Searcher.GoogleWebSearcher wsSearch;
        private static string source = "TechnologyRecognition";

        public string domain;

        public event EventHandler EndAnalysis;
        public event EventHandler LinkFound;

        public TechnologyAnalysis()
        {
            LoadNewTechList();
        }

        private void LoadNewTechList()
        {
            this.SelectedTechnologies = new List<Technology>();
            foreach (string extension in Program.cfgCurrent.SelectedTechExtensions)
                this.SelectedTechnologies.Add(new Technology(extension));
        }

        public void DetailedSearch(DomainsItem domain)
        {
            this.domain = domain.Domain;
            StartSearch(domain);
        }

        private void StartSearch(DomainsItem domain)
        {
            LoadNewTechList();

            wsSearch = new FOCA.Searcher.GoogleWebSearcher();
            wsSearch.ItemsFoundEvent += new EventHandler<EventsThreads.CollectionFound<Uri>>(eventLinkFoundDetailed);
            Program.LogThis(new Log(Log.ModuleType.TechnologyRecognition, "Starting technology recognition in " + domain.Domain, Log.LogType.debug));
            wsSearch.SearchBySite(new CancellationTokenSource(), domain.Domain, this.SelectedTechnologies.Select(p => p.extension).ToArray())
                .ContinueWith((e) =>
                {
                    Program.LogThis(new Log(Log.ModuleType.TechnologyRecognition, "Finishing technology recognition in " + domain, Log.LogType.debug));
                    if (this.EndAnalysis != null)
                        EndAnalysis(domain, null);
                });
        }

        public void eventLinkFoundDetailed(object sender, FOCA.Threads.EventsThreads.CollectionFound<Uri> e)
        {
            foreach (Uri url in e.Data)
            {
                if (LinkFound != null)
                    LinkFound(url.ToString(), null);

                /*  Este if, newdomain=null se da cuando por ejemplo se hace una busqueda de tecnologia
                 *  sobre DOMINIO.COM y aparecen resultados de subdominio1.DOMINO.COM... Así que se agrega
                 *  subdominio1.DOMINIO.com y se le agregan las URLs y tecnologias que se han encontrado
                 */
                DomainsItem NewDomain = Program.data.GetDomain(url.Host);
                if (NewDomain == null)
                {
                    Program.data.AddDomain(url.Host, source, Program.cfgCurrent.MaxRecursion, Program.cfgCurrent);
                    Program.LogThis(new Log(Log.ModuleType.TechnologyRecognition, "Domain found: " + url.Host, Log.LogType.medium));
                    NewDomain = Program.data.GetDomain(url.Host);
                    NewDomain.map.AddUrl(url.ToString());

                }
                /* Si el dominio de la URL coincide con el dominio que se esta tratando, se agrega y se extraen los directorio */
                else if (url.Host == this.domain)
                {
                    Program.data.GetDomain(domain).map.AddUrl(url.ToString());
                }
            }
        }
    }
}
