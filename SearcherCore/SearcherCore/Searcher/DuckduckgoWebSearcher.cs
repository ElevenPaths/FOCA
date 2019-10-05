using FOCA.Threads;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace FOCA.Searcher
{
    public class DuckduckgoWebSearcher : LinkSearcher
    {
        private const int MAX_PAGES = 3;
        private const string userAgent = "Mozilla/5.0 (Linux; U; Android 4.1.1; en-gb; Build/KLP) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Safari/534.30";
        private static readonly string[] supportedFileTypes = new string[] { "pdf", "doc", "docx", "xls", "xlsx", "ppt", "pptx" };

        public DuckduckgoWebSearcher() : base("DuckDuckGoWeb", supportedFileTypes)
        {
        }

        private int Query(string searchTerms, CancellationToken cancelToken)
        {
            int totalResults = 0;
            cancelToken.ThrowIfCancellationRequested();
            string response = SendInitialRequest(searchTerms);
            HashSet<Uri> pageResults = null;
            pageResults = ParseResponse(response, cancelToken);
            if (pageResults.Count > 0)
            {
                totalResults += pageResults.Count;
                OnSearcherLinkFoundEvent(new EventsThreads.CollectionFound<Uri>(pageResults));
            }
            int currentPage = 0;
            do
            {
                string requestParams = GetPostData(response);
                response = MoreResults(searchTerms, requestParams);
                pageResults = ParseResponse(response, cancelToken);
                if (pageResults.Count > 0)
                {
                    totalResults += pageResults.Count;
                    OnSearcherLinkFoundEvent(new EventsThreads.CollectionFound<Uri>(pageResults));
                }
                currentPage++;
                cancelToken.ThrowIfCancellationRequested();
            } while (pageResults != null && pageResults.Count > 0 && currentPage < MAX_PAGES);
            return totalResults;
        }

        private string SendInitialRequest(string searchString)
        {
            HttpWebRequest request = HttpWebRequest.CreateHttp(string.Format("https://duckduckgo.com/html/?q={0}&t=h_", System.Web.HttpUtility.UrlEncode(searchString)));
            request.UserAgent = userAgent;
            request.Referer = "https://duckduckgo.com/";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader responseReader = new StreamReader(response.GetResponseStream()))
            {
                return responseReader.ReadToEnd();
            }
        }

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
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }

            return responseString;
        }

        private HashSet<Uri> ParseResponse(string response, CancellationToken cancelToken)
        {
            HashSet<Uri> res = new HashSet<Uri>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response);

            var links = doc.DocumentNode.SelectNodes(string.Format("//a[contains(@class,'{0}')]", "result__a"));
            if (links == null) return res;

            foreach (var item in links)
            {
                var parts = item.Attributes["href"].Value.Split(new string[] { "uddg=" }, StringSplitOptions.None);
                if (Uri.TryCreate(Uri.UnescapeDataString(parts[parts.Length - 1]), UriKind.Absolute, out Uri urlFound))
                {
                    res.Add(urlFound);
                }
                cancelToken.ThrowIfCancellationRequested();
            }

            return res;
        }

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
            postParams = postParams.Remove(postParams.Length - 1);
            return postParams;
        }

        protected override int Search(string customSearchString, CancellationToken cancelToken)
        {
            return this.Query(customSearchString, cancelToken);
        }
    }
}
