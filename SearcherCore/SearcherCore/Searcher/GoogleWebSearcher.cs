using FOCA.Threads;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace FOCA.Searcher
{
    public class GoogleWebSearcher : LinkSearcher
    {
        private const string IPv4Google = "https://ipv4.google.com";
        private const int MaxResultPerPage = 100;
        private const int MaxResults = 1000;

        private static readonly Regex googleWebUriRegex = new Regex("q=(?<url>https?:\\/\\/[^>]+)&amp;sa", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public enum SafeSearch { off, moderate, active };
        public SafeSearch cSafeSearch { get; set; }

        public enum Language { AnyLanguage, Afrikaans, Arabic, Armenian, Belarusian, Bulgarian, Catalan, Chinese_Simplified, Chinese_Traditional, Croatian, Czech, Danish, Dutch, English, Esperanto, Estonian, Filipino, Finnish, French, German, Greek, Hebrew, Hungarian, Icelandic, Indonesian, Italian, Japanese, Korean, Latvian, Lithuanian, Norwegian, Persian, Polish, Portuguese, Romanian, Russian, Serbian, Slovak, Slovenian, Spanish, Swahili, Swedish, Thai, Turkish, Ukrainian, Vietnamese }
        public Language WriteInLanguage { get; set; }

        public enum Region { AnyRegion, Afghanistan, Albania, Algeria, American_Samoa, Andorra, Angola, Anguilla, Antarctica, Antigua_and_Barbuda, Argentina, Armenia, Aruba, Australia, Austria, Azerbaijan, Bahamas, Bahrain, Bangladesh, Barbados, Belarus, Belgium, Belize, Benin, Bermuda, Bhutan, Bolivia, Bosnia_and_Herzegovina, Botswana, Bouvet_Island, Brazil, British_Indian_Ocean_Territory, British_Virgin_Islands, Brunei, Bulgaria, Burkina_Faso, Burundi, Cambodia, Cameroon, Canada, Cape_Verde, Cayman_Islands, Central_African_Republic, Chad, Chile, China, Christmas_Island, Cocos_Keeling_Islands, Colombia, Comoros, Congo_DRC, Congo_Republic, Cook_Islands, Costa_Rica, Côte_d_Ivoire, Croatia, Cuba, Cyprus, Czech_Republic, Denmark, Djibouti, Dominica, Dominican_Republic, Ecuador, Egypt, El_Salvador, Equatorial_Guinea, Eritrea, Estonia, Ethiopia, Falkland_Islands_Islas_Malvinas, Faroe_Islands, Fiji, Finland, France, French_Guiana, French_Polynesia, French_Southern_Territories, Gabon, Gambia, Georgia, Germany, Ghana, Gibraltar, Greece, Greenland, Grenada, Guadeloupe, Guam, Guatemala, Guinea, Guinea_Bissau, Guyana, Haiti, Heard_Island_and_McDonald_Islands, Honduras, Hong_Kong, Hungary, Iceland, India, Indonesia, Iran, Iraq, Ireland, Israel, Italy, Jamaica, Japan, Jordan, Kazakhstan, Kenya, Kiribati, Kuwait, Kyrgyzstan, Laos, Latvia, Lebanon, Lesotho, Liberia, Libya, Liechtenstein, Lithuania, Luxembourg, Macau, Macedonia_FYROM, Madagascar, Malawi, Malaysia, Maldives, Mali, Malta, Marshall_Islands, Martinique, Mauritania, Mauritius, Mayotte, Mexico, Micronesia, Moldova, Monaco, Mongolia, Montserrat, Morocco, Mozambique, Myanmar_Burma, Namibia, Nauru, Nepal, Netherlands, Netherlands_Antilles, New_Caledonia, New_Zealand, Nicaragua, Niger, Nigeria, Niue, Norfolk_Island, North_Korea, Northern_Mariana_Islands, Norway, Oman, Pakistan, Palau, Palestinian_Territories, Panama, Papua_New_Guinea, Paraguay, Peru, Philippines, Pitcairn_Islands, Poland, Portugal, Puerto_Rico, Qatar, Réunion, Romania, Russia, Rwanda, Saint_Helena, Saint_Kitts_and_Nevis, Saint_Lucia, Saint_Pierre_and_Miquelon, Saint_Vincent_and_the_Grenadines, Samoa, San_Marino, São_Tomé_and_Príncipe, Saudi_Arabia, Senegal, Serbia, Seychelles, Sierra_Leone, Singapore, Slovakia, Slovenia, Solomon_Islands, Somalia, South_Africa, South_Georgia_and_the_South_Sandwich_Islands, South_Korea, Spain, Sri_Lanka, Sudan, Suriname, Svalbard_and_Jan_Mayen, Swaziland, Sweden, Switzerland, Syria, Taiwan, Tajikistan, Tanzania, Thailand, Togo, Tokelau, Tonga, Trinidad_and_Tobago, Tunisia, Turkey, Turkmenistan, Turks_and_Caicos_Islands, Tuvalu, US_Minor_Outlying_Islands, US_Virgin_Islands, Uganda, Ukraine, United_Arab_Emirates, United_Kingdom, United_States, Uruguay, Uzbekistan, Vanuatu, Vatican_City, Venezuela, Vietnam, Wallis_and_Futuna, Western_Sahara, Yemen, Zambia, Zimbabwe }
        public Region LocatedInRegion { get; set; }

        public enum FirstSeenGoogle { AnyTime, past24Hours, pastWeek, pastMonth, pastYear };
        public FirstSeenGoogle FirstSeen { get; set; }

        private static readonly string[] supportedFileTypes = new string[] { "doc", "ppt", "pps", "xls", "docx", "pptx", "ppsx", "xlsx", "sxw", "odt", "ods", "odg", "odp", "pdf", "rtf" };

        public GoogleWebSearcher() : base("GoogleWeb", supportedFileTypes)
        {
        }

        protected override int Search(string customSearchString, CancellationToken cancelToken)
        {
            return GetGoogleAllLinks(customSearchString, cancelToken);
        }

        private int GetGoogleResults(string searchString, int currentResultPerPage, int currentOffset, CancellationToken cancelToken, out bool moreResults)
        {
            HttpWebRequest request;
            byte retries = 0;
            bool error;
            HttpWebResponse response = null;
            string referer = string.Empty;
            OnSearcherLogEvent(new EventsThreads.ThreadStringEventArgs(string.Format("[{0}] Searching first={2} q={1}", Name, searchString, currentOffset)));

            StringBuilder urlBuilder = new StringBuilder(String.Format("https://www.google.com/search?start={1}&num={0}&filter=0&q={2}", currentResultPerPage, currentOffset, HttpUtility.UrlEncode(searchString, Encoding.UTF8)));

            if (cSafeSearch != SafeSearch.off)
                urlBuilder.Append("&safe=" + SafeSearchToString(cSafeSearch));
            if (LocatedInRegion != Region.AnyRegion)
                urlBuilder.Append("&cr=" + RegionToHtmlOption(LocatedInRegion));
            if (FirstSeen != FirstSeenGoogle.AnyTime)
                urlBuilder.Append("&as_qdr=" + FirstSeenToHtmlOption(FirstSeen));
            if (WriteInLanguage != Language.AnyLanguage)
                urlBuilder.Append("&lr=" + LanguageToHtmlOption(WriteInLanguage));

            do
            {
                var intTimeOut = 5000 + 10000 * retries;
                error = false;
                request = (HttpWebRequest)HttpWebRequest.Create(urlBuilder.ToString());
                request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                request.Method = "GET";
                request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                request.KeepAlive = true;

                if (!string.IsNullOrEmpty(UserAgent))
                    request.UserAgent = UserAgent;

                if (!string.IsNullOrEmpty(referer))
                    request.Referer = referer;

                request.Timeout = intTimeOut;

                try
                {
                    OnSearcherLogEvent(new EventsThreads.ThreadStringEventArgs(string.Format("[{0}] Requesting URL {1}", this.Name, request.RequestUri)));
                    response = (HttpWebResponse)request.GetResponse();

                }
                catch (WebException we)
                {
                    if (we.Response != null && we.Response is HttpWebResponse)
                    {
                        HttpWebResponse exceptionResponse = (HttpWebResponse)we.Response;
                        //Too many request, reCaptcha
                        if ((int)exceptionResponse.StatusCode == 429)
                        {
                            OnSearcherLogEvent(new EventsThreads.ThreadStringEventArgs("Too many requests to GoogleWeb engine. Please use the API instead."));
                            retries = 3;
                            throw;
                        }
                        else
                        {
                            OnSearcherLogEvent(new EventsThreads.ThreadStringEventArgs(string.Format("[{0}] Error {1} in request {2}", this.Name, retries, request.RequestUri.ToString())));
                        }
                    }
                    else
                    {
                        OnSearcherLogEvent(new EventsThreads.ThreadStringEventArgs(string.Format("[{0}] Error {1} in request {2}", this.Name, retries, request.RequestUri.ToString())));
                    }
                    error = true;
                    retries++;
                }
                catch (Exception)
                {
                    error = true;
                    retries++;
                    OnSearcherLogEvent(new EventsThreads.ThreadStringEventArgs(string.Format("[{0}] Error {1} in request {2}", this.Name, retries, request.RequestUri.ToString())));
                }
                cancelToken.ThrowIfCancellationRequested();
            } while (error && retries < 3);

            if (error || retries >= 3)
                throw new Exception(string.Format("[{0}] Error connecting", Name));
            string html = null;
            using (var lector = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                html = lector.ReadToEnd();
            }
            response.Close();
            HashSet<Uri> lstCurrentResults = new HashSet<Uri>();
            foreach (Match m in googleWebUriRegex.Matches(html))
            {
                string link = m.Groups["url"].Value;
                if (link.Contains("%"))
                    link = (HttpUtility.UrlDecode(link));
                if (Uri.TryCreate(link, UriKind.Absolute, out Uri foundUri) && !foundUri.Host.Contains("google.com"))
                {
                    lstCurrentResults.Add(foundUri);
                }
                cancelToken.ThrowIfCancellationRequested();
            }

            OnSearcherLogEvent(new EventsThreads.ThreadStringEventArgs(string.Format("[{0}] Found {1} links", this.Name, lstCurrentResults.Count)));
            OnSearcherLinkFoundEvent(new EventsThreads.CollectionFound<Uri>(lstCurrentResults));

            moreResults = html.Contains($"start={currentOffset + currentResultPerPage}&amp;");

            return lstCurrentResults.Count;
        }

        private int GetGoogleAllLinks(string SearchString, CancellationToken cancelToken)
        {
            int totalResults = 0, currentPage = 0;
            bool moreResults;
            do
            {
                totalResults += GetGoogleResults(SearchString, MaxResultPerPage, currentPage * MaxResultPerPage, cancelToken, out moreResults);
                currentPage++;
                CancelToken.ThrowIfCancellationRequested();
            } while (moreResults);
            return totalResults;
        }

        private string RegionToHtmlOption(Region r)
        {
            switch (r)
            {
                case Region.Afghanistan: return "countryAF";
                #region 150 options
                case Region.Albania: return "countryAL";
                case Region.Algeria: return "countryDZ";
                case Region.American_Samoa: return "countryAS";
                case Region.Andorra: return "countryAD";
                case Region.Angola: return "countryAO";
                case Region.Anguilla: return "countryAI";
                case Region.Antarctica: return "countryAQ";
                case Region.Antigua_and_Barbuda: return "countryAG";
                case Region.Argentina: return "countryAR";
                case Region.Armenia: return "countryAM";
                case Region.Aruba: return "countryAW";
                case Region.Australia: return "countryAU";
                case Region.Austria: return "countryAT";
                case Region.Azerbaijan: return "countryAZ";
                case Region.Bahamas: return "countryBS";
                case Region.Bahrain: return "countryBH";
                case Region.Bangladesh: return "countryBD";
                case Region.Barbados: return "countryBB";
                case Region.Belarus: return "countryBY";
                case Region.Belgium: return "countryBE";
                case Region.Belize: return "countryBZ";
                case Region.Benin: return "countryBJ";
                case Region.Bermuda: return "countryBM";
                case Region.Bhutan: return "countryBT";
                case Region.Bolivia: return "countryBO";
                case Region.Bosnia_and_Herzegovina: return "countryBA";
                case Region.Botswana: return "countryBW";
                case Region.Bouvet_Island: return "countryBV";
                case Region.Brazil: return "countryBR";
                case Region.British_Indian_Ocean_Territory: return "countryIO";
                case Region.British_Virgin_Islands: return "countryVG";
                case Region.Brunei: return "countryBN";
                case Region.Bulgaria: return "countryBG";
                case Region.Burkina_Faso: return "countryBF";
                case Region.Burundi: return "countryBI";
                case Region.Cambodia: return "countryKH";
                case Region.Cameroon: return "countryCM";
                case Region.Canada: return "countryCA";
                case Region.Cape_Verde: return "countryCV";
                case Region.Cayman_Islands: return "countryKY";
                case Region.Central_African_Republic: return "countryCF";
                case Region.Chad: return "countryTD";
                case Region.Chile: return "countryCL";
                case Region.China: return "countryCN";
                case Region.Christmas_Island: return "countryCX";
                case Region.Cocos_Keeling_Islands: return "countryCC";
                case Region.Colombia: return "countryCO";
                case Region.Comoros: return "countryKM";
                case Region.Congo_DRC: return "countryCD";
                case Region.Congo_Republic: return "countryCG";
                case Region.Cook_Islands: return "countryCK";
                case Region.Costa_Rica: return "countryCR";
                case Region.Côte_d_Ivoire: return "countryCI";
                case Region.Croatia: return "countryHR";
                case Region.Cuba: return "countryCU";
                case Region.Cyprus: return "countryCY";
                case Region.Czech_Republic: return "countryCZ";
                case Region.Denmark: return "countryDK";
                case Region.Djibouti: return "countryDJ";
                case Region.Dominica: return "countryDM";
                case Region.Dominican_Republic: return "countryDO";
                case Region.Ecuador: return "countryEC";
                case Region.Egypt: return "countryEG";
                case Region.El_Salvador: return "countrySV";
                case Region.Equatorial_Guinea: return "countryGQ";
                case Region.Eritrea: return "countryER";
                case Region.Estonia: return "countryEE";
                case Region.Ethiopia: return "countryET";
                case Region.Falkland_Islands_Islas_Malvinas: return "countryFK";
                case Region.Faroe_Islands: return "countryFO";
                case Region.Fiji: return "countryFJ";
                case Region.Finland: return "countryFI";
                case Region.France: return "countryFR";
                case Region.French_Guiana: return "countryGF";
                case Region.French_Polynesia: return "countryPF";
                case Region.French_Southern_Territories: return "countryTF";
                case Region.Gabon: return "countryGA";
                case Region.Gambia: return "countryGM";
                case Region.Georgia: return "countryGE";
                case Region.Germany: return "countryDE";
                case Region.Ghana: return "countryGH";
                case Region.Gibraltar: return "countryGI";
                case Region.Greece: return "countryGR";
                case Region.Greenland: return "countryGL";
                case Region.Grenada: return "countryGD";
                case Region.Guadeloupe: return "countryGP";
                case Region.Guam: return "countryGU";
                case Region.Guatemala: return "countryGT";
                case Region.Guinea: return "countryGN";
                case Region.Guinea_Bissau: return "countryGW";
                case Region.Guyana: return "countryGY";
                case Region.Haiti: return "countryHT";
                case Region.Heard_Island_and_McDonald_Islands: return "countryHM";
                case Region.Honduras: return "countryHN";
                case Region.Hong_Kong: return "countryHK";
                case Region.Hungary: return "countryHU";
                case Region.Iceland: return "countryIS";
                case Region.India: return "countryIN";
                case Region.Indonesia: return "countryID";
                case Region.Iran: return "countryIR";
                case Region.Iraq: return "countryIQ";
                case Region.Ireland: return "countryIE";
                case Region.Israel: return "countryIL";
                case Region.Italy: return "countryIT";
                case Region.Jamaica: return "countryJM";
                case Region.Japan: return "countryJP";
                case Region.Jordan: return "countryJO";
                case Region.Kazakhstan: return "countryKZ";
                case Region.Kenya: return "countryKE";
                case Region.Kiribati: return "countryKI";
                case Region.Kuwait: return "countryKW";
                case Region.Kyrgyzstan: return "countryKG";
                case Region.Laos: return "countryLA";
                case Region.Latvia: return "countryLV";
                case Region.Lebanon: return "countryLB";
                case Region.Lesotho: return "countryLS";
                case Region.Liberia: return "countryLR";
                case Region.Libya: return "countryLY";
                case Region.Liechtenstein: return "countryLI";
                case Region.Lithuania: return "countryLT";
                case Region.Luxembourg: return "countryLU";
                case Region.Macau: return "countryMO";
                case Region.Macedonia_FYROM: return "countryMK";
                case Region.Madagascar: return "countryMG";
                case Region.Malawi: return "countryMW";
                case Region.Malaysia: return "countryMY";
                case Region.Maldives: return "countryMV";
                case Region.Mali: return "countryML";
                case Region.Malta: return "countryMT";
                case Region.Marshall_Islands: return "countryMH";
                case Region.Martinique: return "countryMQ";
                case Region.Mauritania: return "countryMR";
                case Region.Mauritius: return "countryMU";
                case Region.Mayotte: return "countryYT";
                case Region.Mexico: return "countryMX";
                case Region.Micronesia: return "countryFM";
                case Region.Moldova: return "countryMD";
                case Region.Monaco: return "countryMC";
                case Region.Mongolia: return "countryMN";
                case Region.Montserrat: return "countryMS";
                case Region.Morocco: return "countryMA";
                case Region.Mozambique: return "countryMZ";
                case Region.Myanmar_Burma: return "countryMM";
                case Region.Namibia: return "countryNA";
                case Region.Nauru: return "countryNR";
                case Region.Nepal: return "countryNP";
                case Region.Netherlands: return "countryNL";
                case Region.Netherlands_Antilles: return "countryAN";
                case Region.New_Caledonia: return "countryNC";
                case Region.New_Zealand: return "countryNZ";
                case Region.Nicaragua: return "countryNI";
                case Region.Niger: return "countryNE";
                case Region.Nigeria: return "countryNG";
                case Region.Niue: return "countryNU";
                case Region.Norfolk_Island: return "countryNF";
                case Region.North_Korea: return "countryKP";
                case Region.Northern_Mariana_Islands: return "countryMP";
                case Region.Norway: return "countryNO";
                case Region.Oman: return "countryOM";
                case Region.Pakistan: return "countryPK";
                case Region.Palau: return "countryPW";
                case Region.Palestinian_Territories: return "countryPS";
                case Region.Panama: return "countryPA";
                case Region.Papua_New_Guinea: return "countryPG";
                case Region.Paraguay: return "countryPY";
                case Region.Peru: return "countryPE";
                case Region.Philippines: return "countryPH";
                case Region.Pitcairn_Islands: return "countryPN";
                case Region.Poland: return "countryPL";
                case Region.Portugal: return "countryPT";
                case Region.Puerto_Rico: return "countryPR";
                case Region.Qatar: return "countryQA";
                case Region.Réunion: return "countryRE";
                case Region.Romania: return "countryRO";
                case Region.Russia: return "countryRU";
                case Region.Rwanda: return "countryRW";
                case Region.Saint_Helena: return "countrySH";
                case Region.Saint_Kitts_and_Nevis: return "countryKN";
                case Region.Saint_Lucia: return "countryLC";
                case Region.Saint_Pierre_and_Miquelon: return "countryPM";
                case Region.Saint_Vincent_and_the_Grenadines: return "countryVC";
                case Region.Samoa: return "countryWS";
                case Region.San_Marino: return "countrySM";
                case Region.São_Tomé_and_Príncipe: return "countryST";
                case Region.Saudi_Arabia: return "countrySA";
                case Region.Senegal: return "countrySN";
                case Region.Serbia: return "countryYU";
                case Region.Seychelles: return "countrySC";
                case Region.Sierra_Leone: return "countrySL";
                case Region.Singapore: return "countrySG";
                case Region.Slovakia: return "countrySK";
                case Region.Slovenia: return "countrySI";
                case Region.Solomon_Islands: return "countrySB";
                case Region.Somalia: return "countrySO";
                case Region.South_Africa: return "countryZA";
                case Region.South_Georgia_and_the_South_Sandwich_Islands: return "countryGS";
                case Region.South_Korea: return "countryKR";
                case Region.Spain: return "countryES";
                case Region.Sri_Lanka: return "countryLK";
                case Region.Sudan: return "countrySD";
                case Region.Suriname: return "countrySR";
                case Region.Svalbard_and_Jan_Mayen: return "countrySJ";
                case Region.Swaziland: return "countrySZ";
                case Region.Sweden: return "countrySE";
                case Region.Switzerland: return "countryCH";
                case Region.Syria: return "countrySY";
                case Region.Taiwan: return "countryTW";
                case Region.Tajikistan: return "countryTJ";
                case Region.Tanzania: return "countryTZ";
                case Region.Thailand: return "countryTH";
                case Region.Togo: return "countryTG";
                case Region.Tokelau: return "countryTK";
                case Region.Tonga: return "countryTO";
                case Region.Trinidad_and_Tobago: return "countryTT";
                case Region.Tunisia: return "countryTN";
                case Region.Turkey: return "countryTR";
                case Region.Turkmenistan: return "countryTM";
                case Region.Turks_and_Caicos_Islands: return "countryTC";
                case Region.Tuvalu: return "countryTV";
                case Region.US_Minor_Outlying_Islands: return "countryUM";
                case Region.US_Virgin_Islands: return "countryVI";
                case Region.Uganda: return "countryUG";
                case Region.Ukraine: return "countryUA";
                case Region.United_Arab_Emirates: return "countryAE";
                case Region.United_Kingdom: return "countryGB";
                case Region.United_States: return "countryUS";
                case Region.Uruguay: return "countryUY";
                case Region.Uzbekistan: return "countryUZ";
                case Region.Vanuatu: return "countryVU";
                case Region.Vatican_City: return "countryVA";
                case Region.Venezuela: return "countryVE";
                case Region.Vietnam: return "countryVN";
                case Region.Wallis_and_Futuna: return "countryWF";
                case Region.Western_Sahara: return "countryEH";
                case Region.Yemen: return "countryYE";
                case Region.Zambia: return "countryZM";
                case Region.Zimbabwe: return "countryZW";
                default: return string.Empty;
                    #endregion
            }
        }

        private string LanguageToHtmlOption(Language l)
        {
            switch (l)
            {
                case Language.Afrikaans: return "lang_af";
                case Language.Arabic: return "lang_ar";
                case Language.Armenian: return "lang_hy";
                case Language.Belarusian: return "lang_be";
                case Language.Bulgarian: return "lang_bg";
                case Language.Catalan: return "lang_ca";
                case Language.Chinese_Simplified: return "lang_zh-CN";
                case Language.Chinese_Traditional: return "lang_zh-TW";
                case Language.Croatian: return "lang_hr";
                case Language.Czech: return "lang_cs";
                case Language.Danish: return "lang_da";
                case Language.Dutch: return "lang_nl";
                case Language.English: return "lang_en";
                case Language.Esperanto: return "lang_eo";
                case Language.Estonian: return "lang_et";
                case Language.Filipino: return "lang_tl";
                case Language.Finnish: return "lang_fi";
                case Language.French: return "lang_fr";
                case Language.German: return "lang_de";
                case Language.Greek: return "lang_el";
                case Language.Hebrew: return "lang_iw";
                case Language.Hungarian: return "lang_hu";
                case Language.Icelandic: return "lang_is";
                case Language.Indonesian: return "lang_id";
                case Language.Italian: return "lang_it";
                case Language.Japanese: return "lang_ja";
                case Language.Korean: return "lang_ko";
                case Language.Latvian: return "lang_lv";
                case Language.Lithuanian: return "lang_lt";
                case Language.Norwegian: return "lang_no";
                case Language.Persian: return "lang_fa";
                case Language.Polish: return "lang_pl";
                case Language.Portuguese: return "lang_pt";
                case Language.Romanian: return "lang_ro";
                case Language.Russian: return "lang_ru";
                case Language.Serbian: return "lang_sr";
                case Language.Slovak: return "lang_sk";
                case Language.Slovenian: return "lang_sl";
                case Language.Spanish: return "lang_es";
                case Language.Swahili: return "lang_sw";
                case Language.Swedish: return "lang_sv";
                case Language.Thai: return "lang_th";
                case Language.Turkish: return "lang_tr";
                case Language.Ukrainian: return "lang_uk";
                case Language.Vietnamese: return "lang_vi";
                default: return string.Empty;
            }
        }

        private string FirstSeenToHtmlOption(FirstSeenGoogle f)
        {
            switch (f)
            {
                case FirstSeenGoogle.past24Hours: return "d";
                case FirstSeenGoogle.pastWeek: return "w";
                case FirstSeenGoogle.pastMonth: return "m";
                case FirstSeenGoogle.pastYear: return "y";
                default: return string.Empty;
            }
        }

        private string SafeSearchToString(SafeSearch s)
        {
            switch (s)
            {
                case SafeSearch.active:
                    return "active";
                case SafeSearch.moderate:
                    return "moderate";
                case SafeSearch.off:
                    return "off";
                default:
                    return string.Empty;
            }
        }
    }
}
