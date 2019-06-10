using System.Collections.Generic;
using System.Linq;
using FOCA.Searcher;
using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;

namespace SearcherCore.Searcher.GoogleAPI
{
    /// <summary>
    ///     A set of methods for executing searches.
    /// </summary>
    public class SearchGoogleApi
    {
        public delegate void StatusUpdateHandler(object sender, string e);

        private readonly string CX;
        public string API_KEY;

        public SearchGoogleApi(string key, string cx)
        {
            API_KEY = key;
            CX = cx;
        }

        public event StatusUpdateHandler SearcherLinkFoundEvent;

        private CseResource.ListRequest BuildRequest(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
                return null;
            var service = new CustomsearchService(new BaseClientService.Initializer
            {
                ApplicationName = "Foca",
                ApiKey = API_KEY
            });

            var listRequest = service.Cse.List(" ");

            listRequest.Cx = CX;
            listRequest.Safe = 0;
            listRequest.Hq = searchString;

            return listRequest;
        }

        public List<string> RunService(string searchString)
        {
            var listRequest = BuildRequest(searchString);
            IList<Result> paging = new List<Result>();
            var urls = new List<string>();
            var count = 0;
            while (paging != null)
            {
                listRequest.Start = count*10 + 1;
                try
                {
                    paging = listRequest.Execute().Items;
                    if (paging != null)
                    {
                        urls.AddRange(
                            paging.Select(
                                item => item.Link));
                        foreach (var item in paging)
                        {
                            UpdateStatus(item.Link);
                        }
                    }
                    count++;
                }
                catch
                {
                    paging = null;
                }
            }
            return urls;
        }

        private void UpdateStatus(string status)
        {
            SearcherLinkFoundEvent?.Invoke(this, status);
        }
    }
}