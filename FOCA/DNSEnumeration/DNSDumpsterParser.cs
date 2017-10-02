using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FOCA
{
    /*
     * ALERT: this class needs the NuGet package named HtmlAgilityPack
     *
     * Search for subdomains using DNSdumpster web service
     * In order to be able to use the service, we need to get the anti-CSRF token
     * 1st request obtains it
     * 2nd request obtains the subdomains
     *
     * Usage:
     *  - DNSDumpsterParser p = new DNSDumpsterParser("example.com");
     *  - var subdomains = p.getSubdomains();
     *
     * NOTE: HTML parsing, so it has a really high time complexity
     */
    public class DNSDumpsterParser
    {
        string url;
        string token;

        public DNSDumpsterParser(string url)
        {
            this.url = url;
        }

        /*
         *  Send initial request in order to obtain a valid anti CSRF token
         */
        private string sendInitialRequest()
        {
            HttpWebRequest r = (HttpWebRequest)HttpWebRequest.Create("https://dnsdumpster.com");
            r.Method = "GET";
            String resp_text = String.Empty;
            using (HttpWebResponse resp = (HttpWebResponse)r.GetResponse())
            {
                using (var sr = new StreamReader(resp.GetResponseStream()))
                {
                    resp_text = sr.ReadToEnd();
                }
            }

            return resp_text;
        }

        /*
         * Token comes like this: <input type='hidden' name='csrfmiddlewaretoken' value='foo' />
         */
        private string findToken(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            var token = doc.DocumentNode.SelectSingleNode("//input[@type='hidden' and @name='csrfmiddlewaretoken']").Attributes["value"].Value;
            return token;
        }

        private void getToken()
        {
            var resp_text = sendInitialRequest();
            token = findToken(resp_text);
        }


        private string doSearchAction()
        {
            var request = (HttpWebRequest)WebRequest.Create("https://dnsdumpster.com/");
            request.Method = "POST";
            var postData = "csrfmiddlewaretoken=" + this.token + "&targetip=" + this.url;
            var data = Encoding.ASCII.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;
            request.Headers.Add(HttpRequestHeader.Cookie, "csrftoken=" + this.token);
            request.Referer = "https://dnsdumpster.com";
            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            return responseString;
        }

        public List<string> getSubdomains()
        {
            getToken();
            string resp = doSearchAction();
            return parseFinalResponse(resp);
        }

        private List<string> parseFinalResponse(string html)
        {
            List<string> subs = new List<string>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            // get all elements with class="table-responsive"
            var tables = doc.DocumentNode.SelectNodes("//div[@class='table-responsive']");
            // the one we care about is the last one (4th)
            var subdomainsContainer = tables[3];
            var trs = subdomainsContainer.SelectSingleNode("//table").SelectNodes("//tr");
            foreach(var n in trs)
            {
                var tds = n.SelectNodes("//td[@class=\"col-md-4\"]/text()");
                foreach (var t in tds)
                {
                    var i = t.OuterHtml.Split(new string[] { "<br>" }, StringSplitOptions.None)[0];
                    if (i.Contains(this.url))
                    {
                        bool exists = subs.Any(x => x == i);
                        if (!exists)
                        {
                            subs.Add(i);
                        }
                    }
                }
            }
            return subs;
        }
    }
}
