using FOCA.Threads;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace FOCA.Searcher
{
    public class DuckduckgoWebSearcher : WebSearcher
    {
        private const int MAX_PAGES = 3;
        private const string userAgent = "Mozilla/5.0 (Linux; U; Android 4.1.1; en-gb; Build/KLP) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Safari/534.30";
        private static readonly string[] supportedFileTypes = new string[] { "pdf", "txt", "doc", "docx", "xls", "xlsx", "ppt", "pptx", "rdp", "ica", "html", "htm" };

        public DuckduckgoWebSearcher()
        {
            strName = "DuckDuckGoWeb";
        }

        private HashSet<string> Query(string searchTerms)
        {
            HashSet<string> results = null;
            try
            {


                string response = SendInitialRequest(searchTerms);
                results = ParseResponse(response);
                HashSet<string> pageResults = null;
                int currentPage = 0;
                do
                {
                    string requestParams = GetPostData(response);
                    response = MoreResults(searchTerms, requestParams);
                    pageResults = ParseResponse(response);
                    results.UnionWith(pageResults);
                    currentPage++;
                } while (pageResults != null && pageResults.Count > 0 && currentPage < MAX_PAGES);
            }
            catch (WebException e)
            {
                if (e.Response is HttpWebResponse && ((HttpWebResponse)e.Response).StatusCode == HttpStatusCode.Forbidden)
                {
                    OnSearcherEndEvent(new EventsThreads.ThreadEndEventArgs(EventsThreads.ThreadEndEventArgs.EndReasonEnum.LimitReached));
                    return new HashSet<string>();
                }
                else
                {
                    throw;
                }
            }
            return results;
        }

        /// <summary>
        /// Send Initial Request.
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        private string SendInitialRequest(string searchString)
        {
            var responseText = string.Empty;

            var request = (HttpWebRequest)WebRequest.Create(string.Format("https://duckduckgo.com/html/?q={0}&t=h_", System.Web.HttpUtility.UrlEncode(searchString)));
            request.UserAgent = userAgent;
            request.Referer = "https://duckduckgo.com/";
            var response = (HttpWebResponse)request.GetResponse();
            responseText = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();
            return responseText;
        }

        /// <summary>
        /// Return more results.
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="postParameters"></param>
        /// <returns></returns>
        private string MoreResults(string searchString, string postParameters)
        {
            string responseString = String.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("https://duckduckgo.com/html/?q={0}&t=h_", System.Web.HttpUtility.UrlEncode(searchString)));
            request.Method = "POST";
            request.Headers.Add("Origin: https://duckduckgo.com");
            request.Referer = "https://duckduckgo.com/";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = userAgent;
            request.ContentLength = postParameters.Length;
            request.Headers.Add("Cache-Control: max-age=0");
            request.Headers.Add("Upgrade-Insecure-Requests: 1");
            byte[] data = Encoding.ASCII.GetBytes(postParameters);
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();
            }

            return responseString;
        }


        /// <summary>
        /// Parse Response.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private HashSet<string> ParseResponse(string response)
        {
            HashSet<string> res = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response);

            var links = doc.DocumentNode.SelectNodes(string.Format("//a[contains(@class,'{0}')]", "result__a"));
            if (links == null) return res;

            res.UnionWith(links.Select(link => link.Attributes["href"].Value.Split(new string[] { "uddg=" }, StringSplitOptions.None)).Select(parts => Uri.UnescapeDataString(parts[parts.Length - 1])));
            return res;
        }

        /// <summary>
        /// Get post data.
        /// </summary>
        /// <param name="response"></param>
        private string GetPostData(string response)
        {
            string postParams = String.Empty;
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response);

            var inputs = doc.DocumentNode.SelectNodes("//input[@type=\"hidden\"]");
            foreach (var f in inputs)
            {
                var value = f.Attributes["value"].Value;
                value = System.Web.HttpUtility.UrlEncode(value);
                postParams += f.Attributes["name"].Value + "=" + value + "&";
            }
            postParams.Remove(postParams.Length - 1);
            return postParams;
        }

        private void GetCustomLinksAsync(object customSearchString)
        {
            OnSearcherStartEvent(null);
            try
            {
                SearchAndReport((string)customSearchString);
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

        private void SearchAndReport(string searchValue)
        {
            OnSearcherChangeStateEvent(new EventsThreads.ThreadStringEventArgs($"Searching links in {this.Name} using '{searchValue}' ...."));
            HashSet<string> results = Query(searchValue);
            List<object> lstCurrentResults = results.Cast<object>().ToList();

            OnSearcherLogEvent(new EventsThreads.ThreadStringEventArgs(String.Format("[{0}] Found {1} links", strName, lstCurrentResults.Count)));
            OnSearcherLinkFoundEvent(new EventsThreads.ThreadListDataFoundEventArgs(lstCurrentResults));
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
                Priority = ThreadPriority.Lowest,
                IsBackground = true
            };
            thrSearchLinks.Start(customSearchString);
        }

        /// <summary>
        /// Get links
        /// </summary>
        public override void GetLinks()
        {
            if (thrSearchLinks != null && thrSearchLinks.IsAlive) return;

            thrSearchLinks = new Thread(GetLinksAsync)
            {
                Priority = ThreadPriority.Lowest,
                IsBackground = true
            };
            thrSearchLinks.Start();
        }

        /// <summary>
        /// Get Links async
        /// </summary>
        private void GetLinksAsync()
        {
            OnSearcherStartEvent(null);
            try
            {
                foreach (var strExtension in Extensions.Where(strExtension => supportedFileTypes.Contains(strExtension.ToLower())))
                {
                    OnSearcherChangeStateEvent(new EventsThreads.ThreadStringEventArgs("Search " + strExtension + " in " + Name));

                    switch (strExtension.ToLower())
                    {
                        case "ica":
                            SearchAndReport("site:" + Site + " filetype:txt \"initialprogram\"");
                            break;
                        case "rdp":
                            SearchAndReport("site:" + Site + " filetype:txt \"full address:s:\"");
                            break;
                        default:
                            SearchAndReport("site:" + Site + " filetype:" + strExtension);
                            break;
                    }
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
    }
}
