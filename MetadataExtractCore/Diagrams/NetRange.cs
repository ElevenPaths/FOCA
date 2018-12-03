using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MetadataExtractCore.Diagrams
{
    public class WebClientEx : WebClient
    {
        private CookieContainer _cookieContainer = new CookieContainer();

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).CookieContainer = _cookieContainer;
            }
            return request;
        }
    }

    [Serializable]
    public class NetRange
    {
        public string from;
        public string to;
        public string netname;

        public NetRange()
        {
            // needed to be serializable
        }

        public NetRange(string from, string to, string netname)
        {
            this.from = from;
            this.to = to;
            this.netname = netname;
        }

        public bool IsIpInNetrange(string ip)
        {
            try
            {
                string[] from_octetos = from.Split(new char[] { '.' });
                string[] to_octetos = to.Split(new char[] { '.' });
                string[] ip_octetos = ip.Split(new char[] { '.' });

                if ((int.Parse(ip_octetos[0]) >= int.Parse(from_octetos[0])) && (int.Parse(ip_octetos[0]) <= int.Parse(to_octetos[0])))
                {
                    if ((int.Parse(ip_octetos[1]) >= int.Parse(from_octetos[1])) && (int.Parse(ip_octetos[1]) <= int.Parse(to_octetos[1])))
                    {
                        if ((int.Parse(ip_octetos[2]) >= int.Parse(from_octetos[2])) && (int.Parse(ip_octetos[2]) <= int.Parse(to_octetos[2])))
                        {
                            if ((int.Parse(ip_octetos[3]) >= int.Parse(from_octetos[3])) && (int.Parse(ip_octetos[3]) <= int.Parse(to_octetos[3])))
                                return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
            }
            return false;
        }

        private static NetRange API(string ip)
        {
            try
            {
                // send request to json endpoint
                HttpWebRequest r = (HttpWebRequest)HttpWebRequest.Create("https://stat.ripe.net/data/whois/data.json?resource=" + ip.ToString());
                r.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:54.0) Gecko/20100101 Firefox/54.0";
                r.Headers.Add("Upgrade-Insecure-Requests: 1");
                r.Method = "GET";
                r.Headers.Add(HttpRequestHeader.Cookie, "serverid=www-plone-3; path=/");
                // parse response
                using (HttpWebResponse resp = (HttpWebResponse)r.GetResponse())
                {

                    string response;
                    using (var sr = new StreamReader(resp.GetResponseStream()))
                    {
                        response = sr.ReadToEnd();
                    }

                    // get netrange (if any)
                    JObject json = JObject.Parse(response);

                    string netrange = json["data"]["records"][0][1]["value"].ToString();

                    if (!netrange.Contains("/"))
                        netrange = json["data"]["records"][0][0]["value"].ToString();

                    var ips = NetRange.GetNetRangeIPs(netrange);
                    return new NetRange(ips[0], ips[1], "netrangeObtainedFromRipe");
                }
            }
            catch (Exception)
            {
                // if there wasn't any netrange, return whole 0.0.0.0 to 255.255.255.255
                return null;//new NetRange("0.0.0.0", "255.255.255.255", "ripe_noNetRangeFound");
            }
        }

        private static List<string> GetNetRangeIPs(string netrange)
        {
            List<string> ips = new List<string>(2);
            var direction = netrange.Split('/')[0];
            var bits = Convert.ToInt32(netrange.Split('/')[1]); //Change / for -
            var stringBytes = direction.Split('.');
            IPAddress ip = new IPAddress(new byte[] {
                            Convert.ToByte(Convert.ToDecimal(stringBytes[0])),
                            Convert.ToByte(Convert.ToDecimal(stringBytes[1])),
                            Convert.ToByte(Convert.ToDecimal(stringBytes[2])),
                            Convert.ToByte(Convert.ToDecimal(stringBytes[3]))
                           });

            uint mask = ~(uint.MaxValue >> bits);

            // Convert the IP address to bytes.
            byte[] ipBytes = ip.GetAddressBytes();

            // BitConverter gives bytes in opposite order to GetAddressBytes().
            byte[] maskBytes = BitConverter.GetBytes(mask).Reverse().ToArray();

            byte[] startIPBytes = new byte[ipBytes.Length];
            byte[] endIPBytes = new byte[ipBytes.Length];

            // Calculate the bytes of the start and end IP addresses.
            for (int i = 0; i < ipBytes.Length; i++)
            {
                startIPBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
                endIPBytes[i] = (byte)(ipBytes[i] | ~maskBytes[i]);
            }

            // Convert the bytes to IP addresses.
            IPAddress startIP = new IPAddress(startIPBytes);
            IPAddress endIP = new IPAddress(endIPBytes);

            ips.Add(startIP.ToString());
            ips.Add(endIP.ToString());

            return ips;
        }

        public static NetRange GetNetrange(string ip)
        {
            return API(ip);
        }

        public static CookieContainer GetCookies()
        {
            string responseString = string.Empty;
            ASCIIEncoding encoding = new ASCIIEncoding();
            HttpWebResponse response;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create("http://apps.db.ripe.net/dbweb/search/query.html");
            //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.65 Safari/534.24";
            response = (HttpWebResponse)request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                responseString = reader.ReadToEnd();
            }

            CookieContainer cookieContainer = new CookieContainer();
            foreach (Cookie c in response.Cookies)
            {
                cookieContainer.Add(c);

                //cookieContainer.Add(new Cookie("JSESSIONID", "363FD4891055A3114AE70F908FF7BEEF.node1", "/", "apps.db.ripe.net"));
                //cookieContainer.Add(new Cookie("wdm_last_run", "1314965044617", "/dbweb", "apps.db.ripe.net"));
            }

            response.Close();
            response.GetResponseStream().Close();
            response.GetResponseStream().Dispose();

            response = null;
            request = null;
            encoding = null;
            return cookieContainer;

        }

        private static string DoPost(string url, string parameters)
        {

            string s = parameters;
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = "POST";
            httpWebRequest.Accept = "application/xml,application/xhtml+xml,text/html;q=0.9,text/plain;q=0.8,image/png,*/*;q=0.5";
            httpWebRequest.Referer = "http://apps.db.ripe.net/dbweb/search/query.html";
            httpWebRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, "es-ES");
            httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/4.0; WebMoney Advisor; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Tablet PC 2.0; .NET4.0C; .NET CLR 1.1.4322; .NET4.0E; MALC)";
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";

            httpWebRequest.CookieContainer = GetCookies();
            httpWebRequest.ContentLength = bytes.Length;
            httpWebRequest.Headers.Add(HttpRequestHeader.Pragma, "no-cache");
            httpWebRequest.Timeout = 30000;
            httpWebRequest.AutomaticDecompression = DecompressionMethods.GZip;
            Stream requestStream = httpWebRequest.GetRequestStream();
            requestStream.Write(bytes, 0, bytes.Length);
            requestStream.Close();
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream());
            string document = reader.ReadToEnd().Trim();
            return document;
        }

        private static string DoGet(string url)
        {
            string responseString = string.Empty;
            ASCIIEncoding encoding = new ASCIIEncoding();
            HttpWebResponse response;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            //request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/534.24 (KHTML, like Gecko) Chrome/11.0.696.65 Safari/534.24";
            response = (HttpWebResponse)request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                responseString = reader.ReadToEnd();
            }
            response.Close();
            response.GetResponseStream().Close();
            response.GetResponseStream().Dispose();

            response = null;
            request = null;
            encoding = null;
            return responseString;
        }

        public override string ToString()
        {
            return from + "-" + to + " [" + netname + "]";
        }
    }
}
