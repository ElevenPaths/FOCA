using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace FOCA.SubdomainSearcher
{
    class DNSUtilities
    {
        public static Dictionary<string, bool> CheckedDNS = new Dictionary<string, bool>();

        /// <summary>
        /// Función para saber si un dominio existe o no
        /// </summary>
        /// <param name="strServer">Dominio que se quiere descubrir si existe o no</param>
        /// <param name="ips">Parametro de salida con las Ips resueltas para dicho dominio</param>
        /// <returns>Devuelve verdadero si el dominio es resuelto, falso en caso contrario</returns>
        public static bool ExistsDomain(string strServer, out IPAddress[] ips)
        {
            if (String.IsNullOrEmpty(strServer))
            {
                ips = null;
                return false;
            }
            ips = null;
            try
            {
                ips = Dns.GetHostAddresses(strServer);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool ExistDomain(string strServer)
        {
            IPAddress[] ips;
            return ExistsDomain(strServer, out ips);
        }

        private static bool IsPrivateIP(string strIP)
        {
            return ((strIP.CompareTo("10.0.0.0") >= 0 &&
                     strIP.CompareTo("10.255.255.255") <= 0) ||
                    (strIP.CompareTo("172.16.0.0") >= 0 &&
                     strIP.CompareTo("172.31.255.255") <= 0) ||
                    (strIP.CompareTo("192.168.0.0") >= 0 &&
                     strIP.CompareTo("192.168.255.255") <= 0));
        }

        public static bool ReverseLookup(string strIP, out string strName)
        {
            strName = string.Empty;
            IPAddress ip;
            if (IPAddress.TryParse(strIP, out ip))
            {
                if (IsPrivateIP(strIP))
                    return false;
                else
                {
                    try
                    {
                        //Ignora el warning de instrucción obsoleta, la nueva no funciona correctamente
                        #pragma warning disable
                        IPHostEntry iph = Dns.GetHostByAddress(ip);
                        #pragma warning restore
                        strName = iph.HostName;
                        return iph.HostName != string.Empty && iph.HostName != strIP;
                    }
                    catch
                    {
                        strName = string.Empty;
                        return false;
                    }
                }
            }
            else
                return false;
        }

        /// <summary>
        /// Busca si un DNS resuelve todos los dominios, aunque no existan
        /// </summary>
        /// <param name="strServer"></param>
        /// <returns></returns>
        public static bool isDNSAnyCast(string strServer)
        {
            if (CheckedDNS.ContainsKey(strServer))
                return CheckedDNS[strServer];
            bool bExist = ExistDomain("zzzzyyxzzzzyyxzzzzyyx." + strServer);
            //Para evitar comprobar mas de una vez un mismo dns
            if (bExist)
                CheckedDNS.Add(strServer, bExist);
            //Busca un dominio que es improbable que exista
            return bExist;
        }

        /// <summary>
        /// Dado un dominio ej: www.cooperativaCA.com.ar
        /// Extrae el dominio principal cooperativaCA.com.ar
        /// </summary>
        /// <param name="strDomain"></param>
        /// <returns></returns>
        public static string ExtractMainDomain(string strDomain)
        {
            string[] topLevelDomains = new string[] {
                "AERO",
                "BIZ",
                "COM",
                "COOP",
                "INFO",
                "MUSEUM",
                "NAME",
                "NET",
                "ORG",
                "PRO",
                "GOV",
                "GOB",
                "EDU",
                "MIL",
                "INT",
                "TRAVEL",
                "ROOT",
                "CAT",
                "EU",
                "AC.UK",
                "CO.UK"};

            string[] countryDomains = new string[] {
                #region content
                "AD",
                "AE",
                "AF",
                "AG",
                "AI",
                "AL",
                "AM",
                "AN",
                "AO",
                "AQ",
                "AR",
                "AS",
                "AT",
                "AU",
                "AW",
                "AZ",
                "BA",
                "BB",
                "BD",
                "BE",
                "BF",
                "BG",
                "BH",
                "BI",
                "BJ",
                "BM",
                "BN",
                "BO",
                "BR",
                "BS",
                "BT",
                "BV",
                "BW",
                "BY",
                "BZ",
                "CA",
                "CC",
                "CF",
                "CG",
                "CH",
                "CI",
                "CK",
                "CL",
                "CM",
                "CN",
                "CO",
                "CR",
                "CU",
                "CV",
                "CX",
                "CY",
                "CZ",
                "DE",
                "DJ",
                "DK",
                "DM",
                "DO",
                "DZ",
                "EC",
                "EE",
                "EG",
                "EH",
                "ES",
                "ET",
                "FI",
                "FJ",
                "FK",
                "FM",
                "FO",
                "FR",
                "GA",
                "GD",
                "GE",
                "GF",
                "GH",
                "GI",
                "GL",
                "GM",
                "GN",
                "GP",
                "GQ",
                "GR",
                "GT",
                "GU",
                "GW",
                "GY",
                "HK",
                "HM",
                "HN",
                "HR",
                "HT",
                "HU",
                "Id",
                "IE",
                "IL",
                "IN",
                "IO",
                "IQ",
                "IR",
                "IS",
                "IT",
                "JM",
                "JO",
                "JP",
                "KE",
                "KG",
                "KH",
                "KI",
                "KM",
                "KN",
                "KP",
                "KR",
                "KW",
                "KY",
                "KZ",
                "LA",
                "LB",
                "LC",
                "LI",
                "LK",
                "LR",
                "LS",
                "LT",
                "LU",
                "LV",
                "LY",
                "MA",
                "MC",
                "MD",
                "MG",
                "MH",
                "MK",
                "ML",
                "MM",
                "MN",
                "MO",
                "MP",
                "MQ",
                "MR",
                "MS",
                "MT",
                "MU",
                "MV",
                "MW",
                "MX",
                "MY",
                "MZ",
                "NA",
                "NC",
                "NE",
                "NF",
                "NG",
                "NI",
                "NL",
                "NO",
                "NP",
                "NR",
                "NT",
                "NU",
                "NZ",
                "OM",
                "PA",
                "PE",
                "PF",
                "PG",
                "PH",
                "PK",
                "PL",
                "PM",
                "PN",
                "PR",
                "PT",
                "PW",
                "PY",
                "QA",
                "RE",
                "RO",
                "RU",
                "RW",
                "SA",
                "SB",
                "SC",
                "SD",
                "SE",
                "SG",
                "SH",
                "SI",
                "SJ",
                "SK",
                "SL",
                "SM",
                "SN",
                "SO",
                "SR",
                "ST",
                "SU",
                "SV",
                "SY",
                "SZ",
                "TC",
                "TD",
                "TF",
                "TG",
                "TH",
                "TJ",
                "TK",
                "TM",
                "TN",
                "TO",
                "TP",
                "TR",
                "TT",
                "TV",
                "TW",
                "TZ",
                "UA",
                "UG",
                "UK",
                "UM",
                "US",
                "UY",
                "UZ",
                "VA",
                "VC",
                "VE",
                "VG",
                "VI",
                "VN",
                "VU",
                "WF",
                "WS",
                "YE",
                "YU",
                "ZA",
                "ZM",
                "ZR",
                "ZW"
                #endregion
            };
            //Primero verificamos si existe algun dominio de segundo nivel del type com.es, com.ar
            foreach(string strPais in countryDomains)
            {
                foreach(string strTop in topLevelDomains)
                {
                    if (strDomain.ToUpper().EndsWith("." + strTop + "." + strPais))
                    {
                        //Buscamos el punto anterior al dominio
                        int intPosTopDomain = strDomain.ToUpper().LastIndexOf("." + strTop + "." + strPais);
                        if (intPosTopDomain > 0)
                        {
                            int intPosCompany = strDomain.LastIndexOf('.', intPosTopDomain - 1);
                            if (intPosCompany > 0)
                                return strDomain.Substring(intPosCompany + 1);
                            else
                                return strDomain;
                        }
                    }
                }
            }
            //Sino buscamos un dominio superior normal
            foreach (string strTop in topLevelDomains)
            {
                if (strDomain.ToUpper().EndsWith("." + strTop))
                {
                    //Buscamos el punto anterior al dominio
                    int intPosTopDomain = strDomain.ToUpper().LastIndexOf("." + strTop);
                    if (intPosTopDomain > 0)
                    {
                        int intPosCompany = strDomain.LastIndexOf('.', intPosTopDomain - 1);
                        if (intPosCompany > 0)
                            return strDomain.Substring(intPosCompany + 1);
                        else
                            return strDomain;
                    }
                }
            }
            //Y sino un dominio regional
            foreach (string strPais in countryDomains)
            {
                if (strDomain.ToUpper().EndsWith("." + strPais))
                {
                    //Buscamos el punto anterior al dominio
                    int intPosTopDomain = strDomain.ToUpper().LastIndexOf("." + strPais);
                    if (intPosTopDomain > 0)
                    {
                        int intPosCompany = strDomain.LastIndexOf('.', intPosTopDomain - 1);
                        if (intPosCompany > 0)
                            return strDomain.Substring(intPosCompany + 1);
                        else
                            return strDomain;
                    }
                }
            }
            return string.Empty;
        }
    }
}