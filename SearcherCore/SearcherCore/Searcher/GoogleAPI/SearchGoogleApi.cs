using Google.Apis.Customsearch.v1;
using Google.Apis.Customsearch.v1.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SearcherCore.Searcher.GoogleAPI
{
    /// <summary>
    ///     A set of methods for executing searches.
    /// </summary>
    public class SearchGoogleApi
    {
        private const int ResultsPerPage = 10;
        private const int MaxPages = 10;

        public string CX { get; }

        public string ApiKey { get; }

        public SearchGoogleApi(string key, string cx)
        {
            ApiKey = key;
            CX = cx;
        }

        private CseResource.ListRequest BuildRequest(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
                return null;
            var service = new CustomsearchService(new BaseClientService.Initializer
            {
                ApplicationName = "Foca",
                ApiKey = ApiKey
            });

            var listRequest = service.Cse.List(" ");

            listRequest.Cx = CX;
            listRequest.Safe = 0;
            listRequest.Hq = searchString;
            listRequest.Num = ResultsPerPage;

            return listRequest;
        }

        public ICollection<Uri> RunService(string searchString, CancellationToken cancelToken)
        {
            CseResource.ListRequest listRequest = BuildRequest(searchString);
            IList<Result> paging;
            HashSet<Uri> urls = new HashSet<Uri>();
            int pageNumber = 0;
            do
            {
                listRequest.Start = pageNumber * ResultsPerPage + 1;
                try
                {
                    paging = listRequest.Execute().Items;
                    if (paging != null)
                    {
                        foreach (var item in paging)
                        {
                            if (Uri.TryCreate(item.Link, UriKind.Absolute, out Uri urlFound))
                            {
                                urls.Add(urlFound);
                            }
                            cancelToken.ThrowIfCancellationRequested();
                        }
                    }
                    pageNumber++;
                }
                catch (Google.GoogleApiException e)
                {
                    if (e.Error != null && e.Error.Errors != null && e.Error.Errors.Any())
                    {
                        if (e.Error.Errors.Any(p => "keyInvalid".Equals(p.Reason, StringComparison.OrdinalIgnoreCase)))
                        {
                            throw new ArgumentException("The provided API key is invalid", nameof(ApiKey));
                        }
                        else if (e.Error.Errors.Any(p => "dailyLimitExceeded".Equals(p.Reason, StringComparison.OrdinalIgnoreCase)))
                        {
                            throw new InvalidOperationException("Daily quota exceeded");
                        }
                    }
                    throw;
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch
                {
                    paging = null;
                }
                cancelToken.ThrowIfCancellationRequested();
            } while (paging != null && pageNumber < MaxPages);
            return urls;
        }
    }
}