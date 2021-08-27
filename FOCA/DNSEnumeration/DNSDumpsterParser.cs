using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace FOCA
{
    public static class DNSDumpsterParser
    {
        public static ICollection<string> GetSubdomains(string domain)
        {
            try
            {
                string csrfToken = ReadCsrfToken();
                HttpWebRequest request = HttpWebRequest.CreateHttp("https://dnsdumpster.com/");
                request.Method = "POST";
                string postData = $"user=free&csrfmiddlewaretoken={csrfToken}&targetip={domain}";
                byte[] data = Encoding.ASCII.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Headers.Add(HttpRequestHeader.Cookie, $"csrftoken={csrfToken}");
                request.Referer = "https://dnsdumpster.com";
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader responseStream = new StreamReader(response.GetResponseStream()))
                    {
                        return ParseSubdomainResponse(domain, responseStream.ReadToEnd());
                    }
                }
            }
            catch (Exception e)
            {
                Program.LogThis(new Log(Log.ModuleType.DNSSearch, $"DNSDumpster query has failed: {e.Message}", Log.LogType.error));
                return new List<string>();
            }
        }

        //Token comes like this: <input type='hidden' name='csrfmiddlewaretoken' value='foo' />
        private static string ReadCsrfToken()
        {
            HttpWebRequest r = HttpWebRequest.CreateHttp("https://dnsdumpster.com");
            string response = String.Empty;
            using (WebResponse resp = r.GetResponse())
            {
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    response = sr.ReadToEnd();
                }
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(response);
            return doc.DocumentNode.SelectSingleNode("//input[@type='hidden' and @name='csrfmiddlewaretoken']").Attributes["value"].Value;
        }

        private static ICollection<string> ParseSubdomainResponse(string domain, string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            // get all elements with class="table-responsive"
            HtmlNodeCollection tables = doc.DocumentNode.SelectNodes("//div[@class='table-responsive']");
            // the one we care about is the last one (4th)
            HtmlNode subdomainsContainer = tables[3];
            HtmlNodeCollection trs = subdomainsContainer.SelectSingleNode("//table").SelectNodes("//tr");

            HashSet<string> subDomains = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var n in trs)
            {
                HtmlNodeCollection tds = n.SelectNodes("//td[@class=\"col-md-4\"]/text()");
                foreach (var t in tds)
                {
                    string i = t.OuterHtml.Split(new string[] { "<br>" }, StringSplitOptions.None)[0];
                    if (i.EndsWith(domain))
                    {
                        subDomains.Add(i);
                    }
                }
            }
            return subDomains;
        }
    }
}
