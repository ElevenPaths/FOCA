using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FOCA.Threads;
using SearcherCore.Searcher.BingAPI;
using System.Net;
using System.Windows.Forms;

namespace FOCA.Searcher
{
    public class BingAPISearcher : WebSearcher
    {
        private readonly string[] supportedFileTypes = new string[4] { "doc", "pdf", "ppt", "xls" };
        public string BingApiKey { get; set; }

        public BingAPISearcher(string key)
        {
            strName = "BingAPI";
            BingApiKey = key;
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
        /// Get custom links
        /// </summary>
        /// <param name="customSearchString"></param>
        public override void GetCustomLinks(string customSearchString)
        {
            if (thrSearchLinks != null && thrSearchLinks.IsAlive) return;

            thrSearchLinks = new Thread(GetCustomLinksAsync)
            {
                Priority     = ThreadPriority.Lowest,
                IsBackground = true
            };
            thrSearchLinks.Start(customSearchString);
        }

        /// <summary>
        /// Get Links Asyc.
        /// </summary>
        private void GetLinksAsync()
        {
            OnSearcherStartEvent(null);
            try
            {
                foreach (var strExtension in Extensions.Where(strExtension => supportedFileTypes.Contains(strExtension.ToLower())))
                {
                    OnSearcherChangeStateEvent(new EventsThreads.ThreadStringEventArgs("Search " + strExtension + " in " + Name));
                    GetBingLinks("site:" + Site + " filetype:" + strExtension);
                }

                OnSearcherEndEvent(new EventsThreads.ThreadEndEventArgs(EventsThreads.ThreadEndEventArgs.EndReasonEnum.NoMoreData));
            }
            catch (ThreadAbortException)
            {
                OnSearcherEndEvent(new EventsThreads.ThreadEndEventArgs(EventsThreads.ThreadEndEventArgs.EndReasonEnum.Stopped));
            }
            catch
            {
                //Error on search
                OnSearcherEndEvent(new EventsThreads.ThreadEndEventArgs(EventsThreads.ThreadEndEventArgs.EndReasonEnum.ErrorFound));
            }
        }

        /// <summary>
        /// Get Custom Links Async.
        /// </summary>
        /// <param name="customSearchString"></param>
        private void GetCustomLinksAsync(object customSearchString)
        {
            OnSearcherStartEvent(null);
            OnSearcherChangeStateEvent(new EventsThreads.ThreadStringEventArgs("Searching links in " + Name + "..."));
            try
            {
                if (GetBingAllLinks((string) customSearchString) == 0)
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
        /// Get bing results.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="moreResults"></param>
        /// <returns></returns>
        private int GetBingResults(string searchString, out bool moreResults)
        {
            var client = new SearchBingApi(BingApiKey);
            List<string> results;
            try
            {
                results = client.Search(searchString);
            }
            catch
            {
                moreResults = false;
                return 0;
            }
            if (results.Count != 0)
                OnSearcherLinkFoundEvent(new EventsThreads.ThreadListDataFoundEventArgs((List<object>)results.Cast<object>().ToList()));
            moreResults = false;
            return results.Count;
        }

        /// <summary>
        /// Return bing links.
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        private int GetBingLinks(string searchString)
        {
            bool dummy;
            return GetBingResults(searchString, out dummy);
        }

        /// <summary>
        /// Get Bing All Links
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        private int GetBingAllLinks(string searchString)
        {
            var totalResults = 0;
            bool moreResults;
            do
            {
                totalResults += GetBingResults(searchString, out moreResults);
            } while (moreResults);
            return totalResults;
        }
    }
}
