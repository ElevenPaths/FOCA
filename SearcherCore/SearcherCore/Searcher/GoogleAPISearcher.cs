using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FOCA.Threads;
using SearcherCore.Searcher.GoogleAPI;

namespace FOCA.Searcher
{
    public class GoogleAPISearcher: WebSearcher
    {
        public class GoogleAPIResults
        {
            public string Url;
            public string Title;
            public string UrlCache;
        }

        public string GoogleApiKey { get; set; }
        public string GoogleApiCx { get; set; }
        public const int maxResultPerRequest = 8;
        public const int maxResults = 64;

        public int ResultsPerRequest { get; set; }
        public int Offset { get; set; }

        public GoogleAPISearcher()
        {
            strName = "GoogleAPI";
        }

        /// <summary>
        /// Get Links
        /// </summary>
        public override void GetLinks()
        {
            if (thrSearchLinks != null && thrSearchLinks.IsAlive) return;
            thrSearchLinks = new Thread(GetLinksAsync)
            {
                Priority     = ThreadPriority.Lowest,
                IsBackground = true
            };
            thrSearchLinks.Start();
        }

        /// <summary>
        /// Get Custom Links
        /// </summary>
        /// <param name="customSearchString"></param>
        public override void GetCustomLinks(string customSearchString)
        {
            if (thrSearchLinks != null && thrSearchLinks.IsAlive) return;
            thrSearchLinks = new Thread(GetCustomLinksAsync)
            {
                Priority = ThreadPriority.Lowest,
                IsBackground = true
            };
            thrSearchLinks.Start(customSearchString);
        }

        /// <summary>
        /// Get Links Async
        /// </summary>
        private void GetLinksAsync()
        {
            OnSearcherStartEvent(null);
            OnSearcherChangeStateEvent(new EventsThreads.ThreadStringEventArgs("Searching links in " + Name + "..."));
            try
            {
                foreach(var strExtension in Extensions)
                {
                    OnSearcherChangeStateEvent(new EventsThreads.ThreadStringEventArgs("Search " + strExtension + " in " + Name));
                    GetGoogleLinks("site:" + Site + " filetype:" + strExtension);
                }
                OnSearcherEndEvent(new EventsThreads.ThreadEndEventArgs(EventsThreads.ThreadEndEventArgs.EndReasonEnum.NoMoreData));
            }
            catch (ThreadAbortException)
            {
                OnSearcherEndEvent(new EventsThreads.ThreadEndEventArgs(EventsThreads.ThreadEndEventArgs.EndReasonEnum.Stopped));
            }
            catch
            {
                OnSearcherEndEvent(new EventsThreads.ThreadEndEventArgs(EventsThreads.ThreadEndEventArgs.EndReasonEnum.ErrorFound));
            }
        }

        /// <summary>
        /// Get Custom Links Async
        /// </summary>
        /// <param name="customSearchString"></param>
        private void GetCustomLinksAsync(object customSearchString)
        {
            OnSearcherStartEvent(null);
            OnSearcherChangeStateEvent(new EventsThreads.ThreadStringEventArgs("Searching links in " + Name + "..."));
            try
            {
                if (SearchAll)
                    OnSearcherEndEvent(GetGoogleAllLinks((string) customSearchString) > maxResults - 10
                        ? new EventsThreads.ThreadEndEventArgs(
                            EventsThreads.ThreadEndEventArgs.EndReasonEnum.LimitReached)
                        : new EventsThreads.ThreadEndEventArgs(EventsThreads.ThreadEndEventArgs.EndReasonEnum.NoMoreData));
                else
                {
                    GetGoogleLinks((string)customSearchString);
                    OnSearcherEndEvent(new EventsThreads.ThreadEndEventArgs(EventsThreads.ThreadEndEventArgs.EndReasonEnum.NoMoreData));
                }

            }
            catch (ThreadAbortException)
            {
                OnSearcherEndEvent(new EventsThreads.ThreadEndEventArgs(EventsThreads.ThreadEndEventArgs.EndReasonEnum.Stopped));
            }
            catch
            {
                OnSearcherEndEvent(new EventsThreads.ThreadEndEventArgs(EventsThreads.ThreadEndEventArgs.EndReasonEnum.ErrorFound));
            }
        }

        /// <summary>
        /// Get results from Google API
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="moreResults"></param>
        /// <returns></returns>
        private int GetGoogleResults(string searchString, out bool moreResults)
        {
            var client = new SearchGoogleApi(GoogleApiKey, GoogleApiCx);
            OnSearcherLogEvent(new EventsThreads.ThreadStringEventArgs($"[{Name}] Searching q={searchString}"));

            var results = client.RunService(searchString).Cast<object>().ToList();
            moreResults = false;
            if (results.Count == 0)
            {
                OnSearcherLogEvent(new EventsThreads.ThreadStringEventArgs(
                    $"[{strName}] Error in request q={searchString}"));
                return 0;
            }

            OnSearcherLogEvent(new EventsThreads.ThreadStringEventArgs($"[{strName}] Found {results.Count} links"));
            OnSearcherLinkFoundEvent(new EventsThreads.ThreadListDataFoundEventArgs(results));

            return results.Count ;
        }

        /// <summary>
        /// Get Google Links
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        private int GetGoogleLinks(string searchString)
        {
            bool dummy;
            return GetGoogleResults(searchString, out dummy);
        }

        /// <summary>
        /// Get all google links
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        private int GetGoogleAllLinks(string searchString)
        {
            var totalResults = 0;
            bool moreResults;
            do
            {
                totalResults += GetGoogleResults(searchString, out moreResults);
            } while (moreResults);
            return totalResults;
        }
    }
}