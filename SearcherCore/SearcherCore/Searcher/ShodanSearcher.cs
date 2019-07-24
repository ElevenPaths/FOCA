using FOCA.Threads;
using SearcherCore.Searcher.Shodan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace FOCA.Searcher
{
    public class ShodanSearcher : Searcher<IEnumerable<IPAddress>, ShodanIPInformation>
    {

        private static readonly Regex ServerHeaderExpression = new Regex("^Server: (?<serverValue>.*)$", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        private string apiKey;

        public ShodanSearcher(string apiKey) : base("Shodan")
        {
            if (String.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentNullException(nameof(apiKey));

            this.apiKey = apiKey;
        }

        protected override int Search(IEnumerable<IPAddress> searchValues, CancellationToken cancelToken)
        {
            int resultCount = 0;

            foreach (IPAddress ip in searchValues)
            {
                ShodanIPInformation ipInfo = GetShodanInformation(ip.ToString(), cancelToken);
                if (ipInfo != null)
                {
                    resultCount++;
                    OnSearcherLinkFoundEvent(new EventsThreads.CollectionFound<ShodanIPInformation>(new[] { ipInfo }));
                }
                cancelToken.ThrowIfCancellationRequested();
            }

            return resultCount;
        }

        private ShodanIPInformation GetShodanInformation(string strIPAddress, CancellationToken cancelToken)
        {
            string json = MakeShodanRequestIP(strIPAddress, cancelToken);
            if (!String.IsNullOrWhiteSpace(json))
            {
                List<ShodanIPInformation> lstSIPinfo = ParseJsonShodan(json);
                ShodanIPInformation SIPinfo = lstSIPinfo.FirstOrDefault(p => p.IPAddress == strIPAddress);

                return SIPinfo;
            }
            else
            {
                return null;
            }
        }

        private List<ShodanIPInformation> ParseJsonShodan(string jsonResponse)
        {
            List<ShodanIPInformation> lstShodan = new List<ShodanIPInformation>();
            try
            {
                ShodanResponse result;
                using (MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(jsonResponse)))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ShodanResponse));
                    result = (ShodanResponse)serializer.ReadObject(ms);
                }

                if (result != null)
                {
                    if (result.data != null && result.data.Count > 0)
                    {
                        foreach (Datum m in result.data)
                        {
                            ShodanIPInformation shodanInfo = new ShodanIPInformation
                            {
                                Country = result.country_name,
                                IPAddress = m.ip_str,
                                OS = m.os,
                                HostNames = m.hostnames
                            };

                            if (!String.IsNullOrEmpty(m.data) && m._shodan.module.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                            {
                                Match serverHeader = ServerHeaderExpression.Match(m.data);
                                if (serverHeader.Success)
                                {
                                    shodanInfo.ServerBanner = serverHeader.Groups["serverValue"].Value.Trim();
                                }
                            }

                            lstShodan.Add(shodanInfo);
                        }
                    }
                    else
                    {
                        ShodanIPInformation shodanInfo = new ShodanIPInformation
                        {
                            Country = result.country_name,
                            IPAddress = result.ip_str,
                            OS = result.os
                        };

                        shodanInfo.HostNames.AddRange(result.hostnames);

                        lstShodan.Add(shodanInfo);
                    }
                }
            }
            catch
            {
                OnSearcherLogEvent(new EventsThreads.ThreadStringEventArgs("Couldn't parse Shodan JSON response"));
            }
            return lstShodan;
        }

        private string MakeShodanRequestIP(string searchString, CancellationToken cancelToken)
        {
            string noApiKeyUrl = $"https://api.shodan.io/shodan/host/{searchString}";
            string requestUrl = noApiKeyUrl + $"?key={apiKey}";
            bool error;
            var retries = 0;
            string json = String.Empty;
            do
            {
                error = false;
                HttpWebRequest request = HttpWebRequest.CreateHttp(requestUrl);
                try
                {
                    OnSearcherLogEvent(new EventsThreads.ThreadStringEventArgs($"Requesting URL {noApiKeyUrl}"));
                    HttpWebResponse response = null;
                    try
                    {
                        response = request.GetResponse() as HttpWebResponse;
                        using (Stream st = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(st, Encoding.UTF8))
                            {
                                json = reader.ReadToEnd();
                            }
                        }
                    }
                    catch
                    {
                        if (response?.StatusCode == HttpStatusCode.NotFound)
                        {
                            OnSearcherLogEvent(new EventsThreads.ThreadStringEventArgs($"Shodan hasn't got information about {searchString}"));
                            return "";
                        }
                        OnSearcherLogEvent(new EventsThreads.ThreadStringEventArgs($"Error requesting {noApiKeyUrl}"));
                        return "";
                    }

                }
                catch
                {
                    error = true;
                    retries++;
                    OnSearcherLogEvent(new EventsThreads.ThreadStringEventArgs(String.Format("Error {0} in request {1}", retries, noApiKeyUrl)));
                }
                cancelToken.ThrowIfCancellationRequested();
            } while (error && retries < 3);
            if (error || retries >= 3)
                throw new Exception(string.Format("Error requesting {0}", noApiKeyUrl));
            return json;
        }
    }

    public class ShodanIPInformation
    {
        public string Country;
        public List<string> HostNames = new List<string>();
        public string IPAddress;
        public string OS;
        public string ServerBanner;
    }
}
