using FOCA.Threads;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace FOCA.Searcher
{
    public class BingWebSearcher : LinkSearcher
    {
        private const int MaxResultPerPage = 100;
        private const int MaxResults = 1000;

        private static string[] supportedFileTypes = new string[] { "doc", "docx", "pdf", "ppt", "pptx", "xls", "xlsx", "rtf", "odt", "ods", "odp" };
        private static readonly Regex bingWebUriRegex = new Regex(@"class=""(?:b_title|b_algo)""><h2><a\s+href=\s*[""]?([^""]*)[""]?\s*", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public enum Language { AnyLanguage, Albanian, Arabic, Bulgarian, Catalan, Chinese_Simplified, Chinese_Traditional, Croatian, Czech, Danish, Dutch, English, Estonian, Finnish, French, German, Greek, Hebrew, Hungarian, Icelandic, Indonesian, Italian, Japanese, Korean, Latvian, Lithuanian, Malay, Norwegian, Persian, Polish, Portuguese_Brazil, Portuguese_Portugal, Romanian, Russian, Serbian_Cyrillic, Slovak, Slovenian, Spanish, Swedish, Thai, Turkish, Ukrainian }
        public Language WriteInLanguage { get; set; }
        public enum Region { AnyRegion, Albania, Algeria, Argentina, Armenia, Australia, Austria, Azerbaijan, Belgium, Bolivia, Bosnia_and_Herzegovina, Brazil, Canada, Chile, Colombia, Commonwealth_of_Puerto_Rico, Costa_Rica, Croatia, Czech_Republic, Denmark, Dominican_Republic, Ecuador, Egypt, El_Salvador, Estonia, Finland, Former_Yugoslav_Republic_of_Macedonia, France, Georgia, Germany, Greece, Guatemala, Honduras, Hong_Kong_SAR, Hungary, Iceland, India, Indonesia, Iran, Iraq, Ireland, Islamic_Republic_of_Pakistan, Israel, Italy, Japan, Jordan, Kenya, Kingdom_of_Bahrain, Korea, Kuwait, Latvia, Lebanon, Libya, Lithuania, Luxembourg, Malaysia, Malta, Mexico, Morocco, Netherlands, New_Zealand, Nicaragua, Norway, Oman, Panama, Paraguay, Peru, Poland, Portugal, Qatar, Republic_of_the_Philippines, Romania, Russia, Saudi_Arabia, Serbia, Singapore, Slovakia, Slovenia, South_Africa, Spain, Sweden, Switzerland, Syria, Taiwan, Thailand, Tunisia, Turkey, UAE, Ukraine, United_Kingdom, United_States, Vietnam, Yemen }
        public Region LocatedInRegion { get; set; }

        public BingWebSearcher() : base("BingWeb", supportedFileTypes)
        {
        }

        protected override int Search(string customSearchString, CancellationToken cancelToken)
        {
            return GetBingAllLinks(customSearchString, cancelToken);
        }

        private int GetBingResults(string searchString, int currentResultPerPage, int currentOffset, CancellationToken cancelToken, out bool moreResults)
        {
            if (WriteInLanguage != Language.AnyLanguage)
                searchString += string.Format(" language:{0}", LanguageToHtmlOption(WriteInLanguage));
            if (LocatedInRegion != Region.AnyRegion)
                searchString += string.Format(" loc:{0}", RegionToHtmlOption(LocatedInRegion));
            OnSearcherLogEvent(new EventsThreads.ThreadStringEventArgs(string.Format("[{0}] Searching first={2} q={1}", Name, searchString, currentOffset + 1)));

            string requestUrl = String.Format("http://www.bing.com/search?first={1}&q={0}", searchString, currentOffset + 1);

            int retries = 0;
            bool error;
            string html = string.Empty;
            do
            {
                error = false;
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
                if (!string.IsNullOrEmpty(UserAgent))
                    request.UserAgent = UserAgent;

                request.Timeout = 5000 + 10000 * retries;
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(new Cookie("SRCHHPGUSR", "ADLT=OFF&NRSLT=" + currentResultPerPage, "/", ".bing.com"));

                request.CookieContainer.Add(new Cookie("MUID", "00000000000000000000000000000000", "/", ".bing.com"));
                try
                {
                    OnSearcherLogEvent(new EventsThreads.ThreadStringEventArgs(string.Format("[{0}] Requesting URL {1}", this.Name, request.RequestUri.ToString())));
                    using (var lector = new StreamReader(((HttpWebResponse)request.GetResponse()).GetResponseStream(), Encoding.UTF8))
                    {
                        html = lector.ReadToEnd();
                    }
                }
                catch
                {
                    error = true;
                    retries++;
                    OnSearcherLogEvent(new EventsThreads.ThreadStringEventArgs(string.Format("[{0}] Error {1} in request {2}", this.Name, retries, request.RequestUri.ToString())));
                }
                cancelToken.ThrowIfCancellationRequested();
            } while (error && retries < 3);

            if (error || retries >= 3)
                throw new Exception(string.Format("[{0}] Error connecting", Name));

            HashSet<Uri> results = new HashSet<Uri>();
            foreach (Match item in bingWebUriRegex.Matches(html))
            {
                if (Uri.TryCreate(System.Web.HttpUtility.UrlPathEncode(item.Result("$1").Replace("&amp;", "&")), UriKind.Absolute, out Uri urlFound))
                {
                    results.Add(urlFound);
                }
                cancelToken.ThrowIfCancellationRequested();
            }

            if (results.Count > 0)
            {
                OnSearcherLinkFoundEvent(new EventsThreads.CollectionFound<Uri>(results));
            }

            moreResults = results.Count > 0;
            return results.Count;
        }

        private int GetBingAllLinks(string searchString, CancellationToken cancelToken)
        {
            int totalResults = 0, currentPage = 0;
            bool moreResults;
            do
            {
                totalResults += GetBingResults(searchString, MaxResultPerPage, currentPage * MaxResultPerPage, cancelToken, out moreResults);
                currentPage++;
                cancelToken.ThrowIfCancellationRequested();
            }
            while (moreResults && currentPage * MaxResultPerPage + MaxResultPerPage <= MaxResults);
            return totalResults;
        }

        public string RegionToHtmlOption(Region r)
        {
            switch (r)
            {
                case Region.Albania: return "AL";
                case Region.Algeria: return "DZ";
                case Region.Argentina: return "AR";
                case Region.Armenia: return "AM";
                case Region.Australia: return "AU";
                case Region.Austria: return "AT";
                case Region.Azerbaijan: return "AZ";
                case Region.Belgium: return "BE";
                case Region.Bolivia: return "BO";
                case Region.Bosnia_and_Herzegovina: return "BA";
                case Region.Brazil: return "BR";
                case Region.Canada: return "CA";
                case Region.Chile: return "CL";
                case Region.Colombia: return "CO";
                case Region.Commonwealth_of_Puerto_Rico: return "PR";
                case Region.Costa_Rica: return "CR";
                case Region.Croatia: return "HR";
                case Region.Czech_Republic: return "CZ";
                case Region.Denmark: return "DK";
                case Region.Dominican_Republic: return "DO";
                case Region.Ecuador: return "EC";
                case Region.Egypt: return "EG";
                case Region.El_Salvador: return "SV";
                case Region.Estonia: return "EE";
                case Region.Finland: return "FI";
                case Region.Former_Yugoslav_Republic_of_Macedonia: return "MK";
                case Region.France: return "FR";
                case Region.Georgia: return "GE";
                case Region.Germany: return "DE";
                case Region.Greece: return "GR";
                case Region.Guatemala: return "GT";
                case Region.Honduras: return "HN";
                case Region.Hong_Kong_SAR: return "HK";
                case Region.Hungary: return "HU";
                case Region.Iceland: return "IS";
                case Region.India: return "IN";
                case Region.Indonesia: return "Id";
                case Region.Iran: return "IR";
                case Region.Iraq: return "IQ";
                case Region.Ireland: return "IE";
                case Region.Islamic_Republic_of_Pakistan: return "PK";
                case Region.Israel: return "IL";
                case Region.Italy: return "IT";
                case Region.Japan: return "JP";
                case Region.Jordan: return "JO";
                case Region.Kenya: return "KE";
                case Region.Kingdom_of_Bahrain: return "BH";
                case Region.Korea: return "KR";
                case Region.Kuwait: return "KW";
                case Region.Latvia: return "LV";
                case Region.Lebanon: return "LB";
                case Region.Libya: return "LY";
                case Region.Lithuania: return "LT";
                case Region.Luxembourg: return "LU";
                case Region.Malaysia: return "MY";
                case Region.Malta: return "MT";
                case Region.Mexico: return "MX";
                case Region.Morocco: return "MA";
                case Region.Netherlands: return "NL";
                case Region.New_Zealand: return "NZ";
                case Region.Nicaragua: return "NI";
                case Region.Norway: return "NO";
                case Region.Oman: return "OM";
                case Region.Panama: return "PA";
                case Region.Paraguay: return "PY";
                case Region.Peru: return "PE";
                case Region.Poland: return "PL";
                case Region.Portugal: return "PT";
                case Region.Qatar: return "QA";
                case Region.Republic_of_the_Philippines: return "PH";
                case Region.Romania: return "RO";
                case Region.Russia: return "RU";
                case Region.Saudi_Arabia: return "SA";
                case Region.Serbia: return "SP";
                case Region.Singapore: return "SG";
                case Region.Slovakia: return "SK";
                case Region.Slovenia: return "SI";
                case Region.South_Africa: return "ZA";
                case Region.Spain: return "ES";
                case Region.Sweden: return "SE";
                case Region.Switzerland: return "CH";
                case Region.Syria: return "SY";
                case Region.Taiwan: return "TW";
                case Region.Thailand: return "TH";
                case Region.Tunisia: return "TN";
                case Region.Turkey: return "TR";
                case Region.UAE: return "AE";
                case Region.Ukraine: return "UA";
                case Region.United_Kingdom: return "GB";
                case Region.United_States: return "US";
                case Region.Vietnam: return "VN";
                case Region.Yemen: return "YE";
                default: return string.Empty;
            }
        }

        public string LanguageToHtmlOption(Language l)
        {
            switch (l)
            {
                case Language.Albanian: return "sq";
                case Language.Arabic: return "ar";
                case Language.Bulgarian: return "bg";
                case Language.Catalan: return "ca";
                case Language.Chinese_Simplified: return "zh_chs";
                case Language.Chinese_Traditional: return "zh_cht";
                case Language.Croatian: return "hr";
                case Language.Czech: return "cs";
                case Language.Danish: return "da";
                case Language.Dutch: return "nl";
                case Language.English: return "en";
                case Language.Estonian: return "et";
                case Language.Finnish: return "fi";
                case Language.French: return "fr";
                case Language.German: return "de";
                case Language.Greek: return "el";
                case Language.Hebrew: return "he";
                case Language.Hungarian: return "hu";
                case Language.Icelandic: return "is";
                case Language.Indonesian: return "id";
                case Language.Italian: return "it";
                case Language.Japanese: return "ja";
                case Language.Korean: return "ko";
                case Language.Latvian: return "lv";
                case Language.Lithuanian: return "lt";
                case Language.Malay: return "ms";
                case Language.Norwegian: return "nb";
                case Language.Persian: return "fa";
                case Language.Polish: return "pl";
                case Language.Portuguese_Brazil: return "pt_br";
                case Language.Portuguese_Portugal: return "pt_pt";
                case Language.Romanian: return "ro";
                case Language.Russian: return "ru";
                case Language.Serbian_Cyrillic: return "sr";
                case Language.Slovak: return "sk";
                case Language.Slovenian: return "sl";
                case Language.Spanish: return "es";
                case Language.Swedish: return "sv";
                case Language.Thai: return "th";
                case Language.Turkish: return "tr";
                case Language.Ukrainian: return "uk";
                default: return string.Empty;
            }
        }
    }
}