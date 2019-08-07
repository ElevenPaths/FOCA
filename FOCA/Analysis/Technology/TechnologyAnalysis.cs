using FOCA.Database.Entities;
using FOCA.Threads;
using System;
using System.Collections.Generic;

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
        public List<Technology> listaTech = null;
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
            wsSearch = new FOCA.Searcher.GoogleWebSearcher();
            listaTech = new List<Technology>();
            foreach (string extension in Program.cfgCurrent.SelectedTechExtensions)
                listaTech.Add(new Technology(extension));
        }

        public List<Technology> GetListTech()
        {
            return listaTech;
        }

        public void DetailledSearch(DomainsItem domain)
        {
            this.domain = domain.Domain;
            StartSearch(domain);
        }

        private void StartSearch(DomainsItem domain)
        {
            LoadNewTechList();

            wsSearch = new FOCA.Searcher.GoogleWebSearcher();
            wsSearch.SearchAll = true;
            wsSearch.Site = domain.Domain;
            wsSearch.SearcherLinkFoundEvent += new EventHandler<EventsThreads.CollectionFound<Uri>>(eventLinkFoundDetailed);
            wsSearch.SearcherEndEvent += new EventHandler<EventsThreads.ThreadEndEventArgs>(EndSearch);
            foreach (Technology tech in listaTech)
                wsSearch.AddExtension(tech.extension);
            Program.LogThis(new Log(Log.ModuleType.TechnologyRecognition, "Starting technology recognition in " + domain.Domain, Log.LogType.debug));
            wsSearch.GetLinks();
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

        private void EndSearch(object sender, FOCA.Threads.EventsThreads.ThreadEndEventArgs e)
        {
            Program.LogThis(new Log(Log.ModuleType.TechnologyRecognition, "Finishing technology recognition in " + domain + ". Reason: " + e.EndReason.ToString(), Log.LogType.debug));
            if (this.EndAnalysis != null)
                EndAnalysis(domain, null);
        }
    }
}
