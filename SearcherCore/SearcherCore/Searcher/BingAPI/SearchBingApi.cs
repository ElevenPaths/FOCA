using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SearcherCore.Searcher.BingAPI
{
    public class SearchBingApi
    {
        public delegate void StatusUpdateHandler(object sender, string e);

        private readonly string _secretKey;
        private readonly RestClient client;

        public SearchBingApi(string secretKey)
        {
            // V5.0 of the API
            client = new RestClient("https://api.cognitive.microsoft.com/bing/v5.0");
            // secret key to authenticate requests
            _secretKey = secretKey;
        }

        public event StatusUpdateHandler SearcherLinkFoundEvent;

        /// <summary>
        /// Query Bing API
        /// </summary>
        /// <param name="q">query</param>
        /// <returns></returns>
        public List<Uri> Search(string q)
        {
            List<Uri> results = new List<Uri>();
            int offset = 0;
            // URL of the requests. Count = 50 because it's the max allowed value
            RestRequest request = new RestRequest($"search?count=50&safeSearch=Off&textFormat=Raw&offset={offset}&q={q}", Method.GET);
            // Request header which sends the private key to the server
            request.AddHeader("Ocp-Apim-Subscription-Key", _secretKey);
            var queryResult = client.Execute<List<string>>(request).Data[0];
            JToken token = JObject.Parse(queryResult);

            try
            {
                do
                {
                    var webpages = (JArray)token["webPages"]["value"];
                    foreach (string displayUrl in webpages.Select(link => ((string)link.SelectToken("displayUrl"))))
                    {
                        if (Uri.TryCreate(displayUrl, UriKind.Absolute, out Uri url))
                        {
                            results.Add(url);
                            UpdateStatus(displayUrl);
                        }
                    }
                    offset += 50;
                } while (offset < (int)token.SelectToken("webPages").SelectToken("totalEstimatedMatches") / 1000);
            }
            catch
            {
                throw new Exception("Error while trying to get results");
            }

            return results;
        }

        private void UpdateStatus(string status)
        {
            SearcherLinkFoundEvent?.Invoke(this, status);
        }
    }
}