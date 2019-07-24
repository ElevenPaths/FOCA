using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SearcherCore.Searcher.BingAPI
{
    public class SearchBingApi
    {
        private readonly string _secretKey;
        private readonly RestClient client;
        private const int ResultCountPerPage = 50;

        public SearchBingApi(string secretKey)
        {
            // V7.0 of the API
            client = new RestClient("https://api.cognitive.microsoft.com/bing/v7.0");
            // secret key to authenticate requests
            _secretKey = secretKey;
        }

        public List<Uri> Search(string q, int pageNumber, CancellationToken cancelToken, out bool moreResults)
        {
            List<Uri> results = new List<Uri>();
            int currentOffset = ResultCountPerPage * pageNumber;
            RestRequest request = new RestRequest($"search?count={ResultCountPerPage}&safeSearch=Off&textFormat=Raw&offset={currentOffset}&q={q}", Method.GET);
            // Request header which sends the private key to the server
            request.AddHeader("Ocp-Apim-Subscription-Key", _secretKey);
            IRestResponse<List<string>> queryResult = client.ExecuteTaskAsync<List<string>>(request, cancelToken).Result;
            if (queryResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JToken token = JObject.Parse(queryResult.Data[0]);

                var webpages = (JArray)token["webPages"]["value"];
                foreach (string foundUrl in webpages.Select(link => ((string)link.SelectToken("url"))))
                {
                    if (Uri.TryCreate(foundUrl, UriKind.Absolute, out Uri url))
                    {
                        results.Add(url);
                    }
                    currentOffset++;
                }

                moreResults = ((int)token.SelectToken("webPages").SelectToken("totalEstimatedMatches")) > currentOffset;
                return results;
            }
            else
            {
                throw new InvalidOperationException($"The request could not be completed. Please check your API key. Response status: {queryResult.StatusDescription}");
            }
        }
    }
}