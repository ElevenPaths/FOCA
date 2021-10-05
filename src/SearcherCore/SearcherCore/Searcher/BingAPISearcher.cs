using FOCA.Threads;
using SearcherCore.Searcher.BingAPI;
using System;
using System.Collections.Generic;
using System.Threading;

namespace FOCA.Searcher
{
    public class BingAPISearcher : LinkSearcher
    {
        private static readonly string[] supportedFileTypes = new string[] { "doc", "docx", "pdf", "ppt", "pptx", "xls", "xlsx", "rtf", "odt", "ods", "odp" };
        private const int MaxResults = 1000;

        public string BingApiKey { get; }

        public BingAPISearcher(string key) : base("BingAPI", supportedFileTypes)
        {
            BingApiKey = key;
        }

        protected override int Search(string customSearchString, CancellationToken cancelToken)
        {
            return GetBingAllLinks(customSearchString, cancelToken);
        }

        private int GetBingAllLinks(string searchString, CancellationToken cancelToken)
        {
            int totalResults = 0;
            bool moreResults;
            SearchBingApi client = new SearchBingApi(BingApiKey);
            int pageNumber = 0;
            do
            {
                try
                {
                    List<Uri> results = client.Search(searchString, pageNumber, cancelToken, out moreResults);
                    totalResults += results.Count;

                    if (results.Count > 0)
                        OnSearcherLinkFoundEvent(new EventsThreads.CollectionFound<Uri>(results));
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (InvalidOperationException)
                {
                    throw;
                }
                catch
                {
                    moreResults = false;
                }

                pageNumber++;
                cancelToken.ThrowIfCancellationRequested();
            } while (moreResults && totalResults < MaxResults);

            return totalResults;
        }
    }
}
