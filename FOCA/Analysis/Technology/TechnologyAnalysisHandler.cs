using FOCA.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FOCA.Analysis.Technology
{
    public class TechnologyAnalysisHandler
    {
        public event EventHandler EndAnalysisMultipleDomains;
        public event EventHandler LinkFound;

        DomainsItem[] domains = null;
        int indice = 0;

        public TechnologyAnalysisHandler()
        {
        }

        public void AnalysisMultiplesDomains(DomainsItem[] domains)
        {
            if (domains.Length == 0)
                return;

            this.domains = domains;
            indice = 0;
            AnalyzeDomain(domains[indice]);
        }

        private void AnalyzeDomain(DomainsItem domain)
        {
            indice++;
            domain.techAnalysis.LinkFound += delegate
            {
                if (this.LinkFound != null)
                    LinkFound(null, null);
            };

            domain.techAnalysis.EndAnalysis += new EventHandler(techAnalysis_EndAnalysis);
            {
                domain.AnalyzeTechnology();
            }
        }

        void techAnalysis_EndAnalysis(object sender, EventArgs e)
        {
            if (indice == domains.Length)
            {
                if (EndAnalysisMultipleDomains != null)
                    EndAnalysisMultipleDomains(null, null);
                return;
            }

            AnalyzeDomain(domains[indice]);
        }
    }
}
