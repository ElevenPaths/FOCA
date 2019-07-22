using FOCA.Analysis.FingerPrinting;
using FOCA.ModifiedComponents;
using FOCA.Threads;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;

namespace FOCA.Analysis
{
    public class ShodanRecognition
    {
        private readonly ThreadSafeList<string> lstIPs;
        private readonly string ShodanAPIKey;

        /// <summary>
        ///     default constructor
        /// </summary>
        public ShodanRecognition(string secretKey)
        {
            ShodanAPIKey = secretKey;
        }

        /// <summary>
        ///     constructor, se le pasa una lista de IPs que será sobre las que consultará a Shodan
        /// </summary>
        /// <param name="lstIPs"></param>
        public ShodanRecognition(string secretKey, ThreadSafeList<string> lstIPs)
        {
            ShodanAPIKey = secretKey;
            this.lstIPs = lstIPs;
        }

        /// <summary>
        ///     Inicia el reconocimiento de IPs asincronamente
        /// </summary>
        public void StartRecognition()
        {
            //To run asynchronous
            if (thrSearchShodan != null && thrSearchShodan.IsAlive) return;
            thrSearchShodan = new Thread(Start)
            {
                Priority = ThreadPriority.Lowest,
                IsBackground = true
            };
            thrSearchShodan.Start();
        }

        /// <summary>
        ///     Inicia el reconocimiento de IPs sin hacerlo asincronamente
        /// </summary>
        public void StartRecognitionNoAsync()
        {
            Start();
        }

        /// <summary>
        ///     Inicia el reconocimiento de IPs por netranges sin hacerlo asincronamente
        /// </summary>
        public void StartRecognitionNetRangeNoAsync(MetadataExtractCore.ThreadSafeList<NetRange> lstNetranges)
        {
            StartNetRanges(lstNetranges);
        }

        private void StartNetRanges(MetadataExtractCore.ThreadSafeList<NetRange> lstNetranges)
        {
            try
            {
                OnStartEvent(null);
                OnLogEvent(new EventsThreads.ThreadStringEventArgs("Starting Shodan search by netrange"));

                foreach (var nr in lstNetranges)
                {
                    GetShodanInformation(nr);
                }
            }
            catch (ThreadAbortException)
            {
                OnLogEvent(new EventsThreads.ThreadStringEventArgs("Shodan search aborted"));
                OnEndEvent(new EventsThreads.ThreadEndEventArgs(EventsThreads.ThreadEndEventArgs.EndReasonEnum.Stopped));
            }
            catch (Exception e)
            {
                OnLogEvent(new EventsThreads.ThreadStringEventArgs($"Shodan search aborted error: {e.Message}"));
                OnEndEvent(
                    new EventsThreads.ThreadEndEventArgs(EventsThreads.ThreadEndEventArgs.EndReasonEnum.ErrorFound));
            }
        }

        private void Start()
        {
            try
            {
                //ejecutamos el evento de inicio
                OnStartEvent(null);
                OnLogEvent(new EventsThreads.ThreadStringEventArgs(
                    $"Starting Shodan search in {lstIPs.Count} IP Addresses"));

                foreach (string ip in lstIPs.Where(p => Functions.IsIP(p)))
                {
                    ShodanIPInformation ipInfo = GetShodanInformation(ip);
                    if (ipInfo != null)
                    {
                        OnDataFoundEvent(new EventsThreads.CollectionFound<ShodanIPInformation>(new[] { ipInfo }));
                    }
                }

                OnLogEvent(new EventsThreads.ThreadStringEventArgs("Shodan search finished"));
                OnEndEvent(
                    new EventsThreads.ThreadEndEventArgs(EventsThreads.ThreadEndEventArgs.EndReasonEnum.NoMoreData));
            }
            catch (ThreadAbortException)
            {
                OnLogEvent(new EventsThreads.ThreadStringEventArgs("Shodan search aborted"));
                OnEndEvent(new EventsThreads.ThreadEndEventArgs(EventsThreads.ThreadEndEventArgs.EndReasonEnum.Stopped));
            }
            catch (Exception e)
            {
                OnLogEvent(new EventsThreads.ThreadStringEventArgs($"Shodan search aborted error: {e.Message}"));
                OnEndEvent(
                    new EventsThreads.ThreadEndEventArgs(EventsThreads.ThreadEndEventArgs.EndReasonEnum.ErrorFound));
            }
        }

        /// <summary>
        ///     Dada una IP devuelve la información que obtiene el búscador Shodan de ella
        /// </summary>
        /// <param name="strIPAddress"></param>
        /// <returns></returns>
        private ShodanIPInformation GetShodanInformation(NetRange netRange)
        {
            var json = MakeShodanRequestNet(netRange.from + "/24");
            //Parsea el HTML y obtiene los datos de respuesta
            var lstSiPinfo = ParseJsonShodan(json);
            return null;
        }

        /// <summary>
        ///     Dada una IP devuelve la información que obtiene el búscador Shodan de ella
        /// </summary>
        /// <param name="strIPAddress"></param>
        /// <returns></returns>
        private ShodanIPInformation GetShodanInformation(string strIPAddress)
        {
            //Obtiene el HTML de la petición
            string json = MakeShodanRequestIP(strIPAddress);
            //Parsea el HTML y obtiene los datos de respuesta
            List<ShodanIPInformation> lstSIPinfo = ParseJsonShodan(json);
            //Filtra los datos para que se quede solo con los de la ip buscada
            ShodanIPInformation SIPinfo = lstSIPinfo.FirstOrDefault(p => p.IPAddress == strIPAddress);

            //Damos prioridad al fprinting de shodan al del modulo fingerprinting/http.cs
            if (SIPinfo != null && String.IsNullOrWhiteSpace(SIPinfo.OS))
            {
                SIPinfo.OS = HTTP.GetOsFromBanner(SIPinfo.ServerBanner).ToString();
            }

            return SIPinfo;
        }

        private string MakeShodanRequestNet(string searchString)
        {
            var url =
                string.Format(
                    "https://api.shodan.io/shodan/host/search?key={1}&query={0}&facets=net",
                    searchString, ShodanAPIKey);
            var urlCensored = $"https://api.shodan.io/shodan/host/search?query={searchString}&facets=net";
            bool error;
            var retries = 0;
            var html = string.Empty;
            do
            {
                error = false;
                var request = (HttpWebRequest)WebRequest.Create(url);
                try
                {
                    OnLogEvent(new EventsThreads.ThreadStringEventArgs($"Requesting URL {urlCensored}"));
                    HttpWebResponse response = null;
                    try
                    {
                        response = (HttpWebResponse)request.GetResponse();
                    }
                    catch
                    {
                        if (response?.StatusCode == HttpStatusCode.NotFound)
                        {
                            OnLogEvent(
                                new EventsThreads.ThreadStringEventArgs(
                                    $"Shodan hasn't information about {searchString}"));
                            return "";
                        }
                        OnLogEvent(new EventsThreads.ThreadStringEventArgs($"Error requesting {urlCensored}"));
                        return "";
                    }
                    var st = response.GetResponseStream();
                    if (st == null)
                        return "";
                    var lector = new StreamReader(st, Encoding.UTF8);
                    html = lector.ReadToEnd();
                    lector.Close();
                    response.Close();
                }
                catch
                {
                    error = true;
                    retries++;
                    OnLogEvent(
                        new EventsThreads.ThreadStringEventArgs(string.Format("Error {0} in request {1}", retries,
                            urlCensored)));
                }
            } while (error && retries < 3);
            if (error || retries >= 3)
                throw new Exception(string.Format("Error requesting {0}", urlCensored));
            return html;
        }

        private string MakeShodanRequestIP(string searchString)
        {
            var url =
                $"https://api.shodan.io/shodan/host/{searchString}?key={ShodanAPIKey}";
            var urlCensored = $"https://api.shodan.io/shodan/host/{searchString}";
            bool error;
            var retries = 0;
            var html = string.Empty;
            do
            {
                error = false;
                var request = (HttpWebRequest)WebRequest.Create(url);
                try
                {
                    OnLogEvent(new EventsThreads.ThreadStringEventArgs($"Requesting URL {urlCensored}"));
                    HttpWebResponse response = null;
                    try
                    {
                        response = (HttpWebResponse)request.GetResponse();
                    }
                    catch
                    {
                        if (response?.StatusCode == HttpStatusCode.NotFound)
                        {
                            OnLogEvent(
                                new EventsThreads.ThreadStringEventArgs(
                                    $"Shodan hasn't information about {searchString}"));
                            return "";
                        }
                        OnLogEvent(new EventsThreads.ThreadStringEventArgs($"Error requesting {urlCensored}"));
                        return "";
                    }
                    var st = response.GetResponseStream();
                    if (st == null)
                        return "";
                    var lector = new StreamReader(st, Encoding.UTF8);
                    html = lector.ReadToEnd();
                    lector.Close();
                    response.Close();
                }
                catch
                {
                    error = true;
                    retries++;
                    OnLogEvent(
                        new EventsThreads.ThreadStringEventArgs(string.Format("Error {0} in request {1}", retries,
                            urlCensored)));
                }
            } while (error && retries < 3);
            if (error || retries >= 3)
                throw new Exception(string.Format("Error requesting {0}", urlCensored));
            return html;
        }

        /// <summary>
        ///     Dado un documento Json obtiene los resultados dados por Shodan
        /// </summary>
        /// <param name="JSON"></param>
        /// <returns>Devuelve una lista ya que shodan no da un solo resultado, da varios</returns>
        private List<ShodanIPInformation> ParseJsonShodan(string JSON)
        {
            List<ShodanIPInformation> lstShodan = new List<ShodanIPInformation>();
            try
            {
                ShodanJson result;
                using (MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(JSON)))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(ShodanJson));
                    result = (ShodanJson)serializer.ReadObject(ms);
                }
                if (result != null && result.matches != null)
                {
                    foreach (var m in result.matches)
                    {
                        ShodanIPInformation si = new ShodanIPInformation
                        {
                            Country = m.country,
                            IPAddress = m.ip,
                            OS = m.os
                        };
                        si.HostNames.AddRange(m.hostnames);
                        var dummy = 0;
                        si.ServerBanner = Functions.SearchBetweenDelimiters(m.data, "Server: ", "\r", ref dummy, StringComparison.InvariantCulture);
                        si.ShodanResponse = JSON;
                        lstShodan.Add(si);
                    }
                }
            }
            catch
            {
                Program.LogThis(new Log(Log.ModuleType.ShodanSearch, "Couldn't parse Shodan JSON response",
                    Log.LogType.debug));
            }
            return lstShodan;
        }

        private ShodanIPInformation FilterShodanResults(IEnumerable<ShodanIPInformation> lstSIPinfo, string strIPAddress)
        {
            foreach (var si in lstSIPinfo.Where(si => si.IPAddress == strIPAddress))
            {
                return si;
            }
            //No se ha encontrado la ip
            return new ShodanIPInformation();
        }

        //Datos que devuelve Shodan de una búsqueda
        public class ShodanIPInformation
        {
            public string Country;
            public List<string> HostNames = new List<string>();
            public string IPAddress;
            public string OS;
            public string ServerBanner;
            public string ShodanResponse;
        }

        /// <summary>
        ///     Estructura de una clase json devuelta por el servidor web
        /// </summary>
        [DataContract]
        public class ShodanJson
        {
            [DataMember] public match[] matches;
            [DataMember] public int total;

            [DataContract]
            public class match
            {
                [DataMember] public string country;
                [DataMember] public string data;
                [DataMember] public string[] hostnames;
                [DataMember] public string ip;
                [DataMember] public string os;
                [DataMember] public string updated;
            }
        }

        #region Variables y funciones para el manejo de hilos

        //Hilo usado para realizar las búsquedas asíncronamente
        private Thread thrSearchShodan;

        /// <summary>
        ///     Aborta el reconocimiento
        /// </summary>
        public void Abort()
        {
            if (thrSearchShodan != null && thrSearchShodan.IsAlive)
                thrSearchShodan.Abort();
        }

        /// <summary>
        ///     Une el hilo actual al del reconocimiento
        /// </summary>
        public void Join()
        {
            thrSearchShodan.Join();
        }

        #endregion

        #region Funciones y variables necesarias para usar eventos

        /// <summary>
        ///     Event raised when a start the search
        /// </summary>
        public event EventHandler StartEvent;

        /// <summary>
        ///     Event raised when a new link or group of links is found
        /// </summary>
        public event EventHandler<EventsThreads.ThreadEndEventArgs> EndEvent;

        /// <summary>
        ///     Event raised when a new link or group of links is found..
        /// </summary>
        public event EventHandler<EventsThreads.CollectionFound<ShodanIPInformation>> DataFoundEvent;

        /// <summary>
        ///     Evento lanzado cuando hay algo importante que logear
        /// </summary>
        public event EventHandler<EventsThreads.ThreadStringEventArgs> LogEvent;

        protected void OnStartEvent(EventArgs e)
        {
            StartEvent?.Invoke(this, e);
        }

        protected void OnEndEvent(EventsThreads.ThreadEndEventArgs e)
        {
            EndEvent?.Invoke(this, e);
        }

        protected void OnDataFoundEvent(EventsThreads.CollectionFound<ShodanIPInformation> e)
        {
            //MS say: Copy the handle to avoid a race condition.
            DataFoundEvent?.Invoke(this, e);
        }

        protected void OnLogEvent(EventsThreads.ThreadStringEventArgs e)
        {
            LogEvent?.Invoke(this, e);
        }

        #endregion
    }
}