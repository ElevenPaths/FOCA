using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FOCA.Searcher
{
    public abstract class LinkSearcher : Searcher<string, Uri>
    {
        protected List<string> SupportedExtensions { get; private set; }

        public string UserAgent { get; set; }

        public LinkSearcher(string searcherName, params string[] availableExtensions) : base(searcherName)
        {
            this.SupportedExtensions = new List<string>();
            if (availableExtensions != null && availableExtensions.Length > 0)
            {
                this.SupportedExtensions.AddRange(availableExtensions);
            }
        }

        public virtual Task<int> SearchBySite(CancellationTokenSource cancelToken, string site, params string[] filterExtensions)
        {
            List<string> searchValues = new List<string>();
            if (filterExtensions != null && filterExtensions.Length > 0)
            {
                foreach (string strExtension in this.SupportedExtensions.Where(p => filterExtensions.Contains(p.ToLower())))
                {
                    searchValues.Add($"site:{site} filetype:{strExtension}");
                }
            }
            else
            {
                searchValues.Add($"site:{site}");
            }
            return this.CustomSearch(cancelToken, searchValues.ToArray());
        }
    }
}
