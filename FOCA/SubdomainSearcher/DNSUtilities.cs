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
    }
}