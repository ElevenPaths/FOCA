using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using HtmlAgilityPack;
using FOCA.Threads;

namespace FOCA.Searcher
{
    public class DuckduckgoWebSearcher : WebSearcher
    {
        private string userAgent = "Mozilla/5.0 (Linux; U; Android 4.1.1; en-gb; Build/KLP) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Safari/534.30";
        private string postParams = string.Empty; 
        private List<string> results; 
        private string[] supportedFileTypes = new string[] { "html", "htm", "pdf", "txt", "doc", "docx", "xls", "xlsx", "ppt", "pptx" };

        public DuckduckgoWebSearcher()
        {
            strName = "DuckduckgoWeb";
        }


        public int Query(string searchTerms)
        {
            var response = SendInitialRequest(searchTerms);
            
            results = ParseResponse(response);
            try
            {
                GetPostData(response);
                response = MoreResults(searchTerms, postParams);
                results.AddRange(ParseResponse(response));
            }
            catch (Exception)
            {
                OnSearcherEndEvent(new EventsThreads.ThreadEndEventArgs(EventsThreads.ThreadEndEventArgs.EndReasonEnum.ErrorFound));
            }
            return results.Count;
        }

        /// <summary>
        /// Send Initial Request.
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        private string SendInitialRequest(string searchString)
        {
            var responseText = string.Empty;

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(string.Format("https://duckduckgo.com/html/?q={0}&t=h_", System.Web.HttpUtility.UrlEncode(searchString)));
                request.UserAgent = userAgent;
                request.Referer = "https://duckduckgo.com/";
                var response = (HttpWebResponse)request.GetResponse();
                responseText = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();
            } catch (Exception ex)
            {
                OnSearcherEndEvent(new EventsThreads.ThreadEndEventArgs(EventsThreads.ThreadEndEventArgs.EndReasonEnum.ErrorFound));
                throw ex;
            }
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
            var responseString = string.Empty;
            var request = (HttpWebRequest)WebRequest.Create(string.Format("https://duckduckgo.com/html/?q={0}&t=h_", System.Web.HttpUtility.UrlEncode(searchString)));
            request.Method = "POST";
            request.Headers.Add("Origin: https://duckduckgo.com");
            request.Referer = "https://duckduckgo.com/";    
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = userAgent;
            request.ContentLength = postParameters.Length;
            request.Headers.Add("Cache-Control: max-age=0");
            request.Headers.Add("Upgrade-Insecure-Requests: 1");
            
            var data = Encoding.ASCII.GetBytes(postParameters);
            try
            {
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
                var response = (HttpWebResponse)request.GetResponse();
                responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();
            } catch
            {
                OnSearcherEndEvent(new EventsThreads.ThreadEndEventArgs(EventsThreads.ThreadEndEventArgs.EndReasonEnum.ErrorFound));
            }
            return responseString;
        }

       
        /// <summary>
        /// Parse Response.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private List<string> ParseResponse(string response)
        {
            var res = new List<string>();
            var doc = new HtmlDocument();
            doc.LoadHtml(response);

            var links = doc.DocumentNode.SelectNodes(string.Format("//a[contains(@class,'{0}')]", "result__a"));
            if (links == null) return res;

            res.AddRange(links.Select(link => link.Attributes["href"].Value.Split(new string[] {"uddg="}, StringSplitOptions.None)).Select(parts => Uri.UnescapeDataString(parts[parts.Length - 1])));
            return res;
        }

        /// <summary>
        /// Get post data.
        /// </summary>
        /// <param name="response"></param>
        private void GetPostData(string response)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(response);

            var inputs = doc.DocumentNode.SelectNodes(string.Format("//input[@type={0}]", "\"hidden\""));
            foreach (var f in inputs)
            {
                var value = f.Attributes["value"].Value;
                value = System.Web.HttpUtility.UrlEncode(value);
                postParams += f.Attributes["name"].Value + "=" + value + "&";
            }
            postParams.Remove(postParams.Length - 1);
        }

        /// <summary>
        /// Get custom links
        /// </summary>
        /// <param name="customSearchString"></param>
        public override void GetCustomLinks(string customSearchString)
        {
            
            OnSearcherStartEvent(null);
            OnSearcherChangeStateEvent(new EventsThreads.ThreadStringEventArgs("Searching links in " + Name + "..."));
            string valueQuery = string.Empty;
            var completeQuery = customSearchString.Replace("site:","site=");
            int count = 0;
            string endQuery = string.Empty;

            foreach (var ext in Extensions)
            {
                if (count == 0 && supportedFileTypes.Contains(ext.ToLower()))
                {
                    count++;
                    valueQuery += "filetype:(" + ext + "+OR+";
                }
                else if (supportedFileTypes.Contains(ext.ToLower()))
                    valueQuery += ext + "+OR+";

            }
            if (valueQuery.Length > 0)
            {
                endQuery = valueQuery.Remove(valueQuery.Length - 4);
                endQuery += ")" + completeQuery;
                GetDuckduckgoResults(endQuery);
            }
            else
                GetDuckduckgoResults(completeQuery);
           
            OnSearcherEndEvent(new EventsThreads.ThreadEndEventArgs(EventsThreads.ThreadEndEventArgs.EndReasonEnum.NoMoreData));
        }

        /// <summary>
        /// Get Duck go results.
        /// </summary>
        /// <param name="searchTerms"></param>
        /// <returns></returns>
        private int GetDuckduckgoResults(string searchTerms)
        {
            OnSearcherChangeStateEvent(new EventsThreads.ThreadStringEventArgs("Search in " + Name));
            Query(searchTerms);
            var lstCurrentResults = new List<object>();
            lstCurrentResults = results.Cast<object>().ToList();

            OnSearcherLogEvent(new EventsThreads.ThreadStringEventArgs(String.Format("[{0}] Found {1} links", strName, lstCurrentResults.Count)));
            OnSearcherLinkFoundEvent(new EventsThreads.ThreadListDataFoundEventArgs(lstCurrentResults));
            OnSearcherEndEvent(new EventsThreads.ThreadEndEventArgs(EventsThreads.ThreadEndEventArgs.EndReasonEnum.NoMoreData));

            return lstCurrentResults.Count;
        }

        /// <summary>
        /// Get links
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
                            GetCustomLinks("site:" + Site + " f:txt \"initialprogram\"");
                            break;
                        case "rdp":
                            GetCustomLinks("site:" + Site + " f:txt \"full address:s:\"");
                            break;
                        default:
                            GetCustomLinks("site:" + Site);
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
