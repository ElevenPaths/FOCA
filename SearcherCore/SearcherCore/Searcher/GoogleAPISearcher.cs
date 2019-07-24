using FOCA.Threads;
using SearcherCore.Searcher.GoogleAPI;
using System;
using System.Collections.Generic;
using System.Threading;

namespace FOCA.Searcher
{
    public class GoogleAPISearcher : LinkSearcher
    {
        private const int MaxResultPerRequest = 8;
        private const int MaxResults = 64;
        private static readonly string[] supportedFileTypes = new string[] { "doc", "ppt", "pps", "xls", "docx", "pptx", "ppsx", "xlsx", "sxw", "odt", "ods", "odg", "odp", "pdf", "rtf" };

        public string GoogleApiKey { get; }
        public string GoogleApiCx { get; }

        public GoogleAPISearcher(string apiKey, string apiCx) : base("GoogleAPI", supportedFileTypes)
        {
            this.GoogleApiKey = apiKey;
            this.GoogleApiCx = apiCx;
        }

        protected override int Search(string customSearchString, CancellationToken cancelToken)
        {
            return this.GetGoogleAllLinks(customSearchString, cancelToken);
        }

        private int GetGoogleResults(string searchString, CancellationToken cancelToken, out bool moreResults)
        {
            var client = new SearchGoogleApi(GoogleApiKey, GoogleApiCx);
            OnSearcherLogEvent(new EventsThreads.ThreadStringEventArgs($"[{Name}] Searching q={searchString}"));

            ICollection<Uri> results = client.RunService(searchString, cancelToken);
            moreResults = false;
            if (results.Count > 0)
            {
                OnSearcherLinkFoundEvent(new EventsThreads.CollectionFound<Uri>(results));
            }

            return results.Count;
        }

        private int GetGoogleAllLinks(string searchString, CancellationToken cancelToken)
        {
            var totalResults = 0;
            bool moreResults;
            do
            {
                totalResults += GetGoogleResults(searchString, cancelToken, out moreResults);
                cancelToken.ThrowIfCancellationRequested();
            } while (moreResults);
            return totalResults;
        }
    }
}