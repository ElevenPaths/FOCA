using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using Heijden.DNS;

namespace FOCA
{
    public class DNSUtil
    {
        public static List<IPAddress> GetHostAddresses(string domain)
        {
            try
            {
                IPAddress[] ips = Dns.GetHostAddresses(domain);
                return new List<IPAddress>(ips);
            }
            catch
            {
                return new List<IPAddress>();
            }
        }

        public static List<IPAddress> GetHostAddresses(Resolver r, string domain, string NSServer)
        {
            List<IPEndPoint> lastNSServers = new List<IPEndPoint>(r.DnsServers);
            try
            {
                r.DnsServer = NSServer;
                List<IPAddress> lst = new List<IPAddress>();
                Response response = r.Query(domain, QType.A, QClass.IN);
                foreach (RecordA rA in response.RecordsA)
                {
                    lst.Add(rA.Address);
                }
                response = r.Query(domain, QType.AAAA, QClass.IN);
                foreach (RecordAAAA rAAAA in response.RecordsAAAA)
                {
                    lst.Add(rAAAA.Address);
                }
                return lst;
            }
            catch
            {
                return new List<IPAddress>();
            }
            finally
            {
                r.DnsServers = lastNSServers.ToArray();
            }
        }

        public static List<string> GetHostNames(Resolver r, string ip)
        {
            return GetHostNames(r, ip, GetLocalNSServer()[0].Address.ToString());
        }

        public static List<string> GetHostNames(Resolver r, string ip, string NSServer)
        {
            List<string> lst = new List<string>();
            //Este mismo resolver es usado en todo el código así que hay que reestablecer los DNSServers anteriores
            List<IPEndPoint> lastNSServers = new List<IPEndPoint>(r.DnsServers);
            try
            {
                r.DnsServer = NSServer;
                Response response = r.Query(Resolver.GetArpaFromIp(IPAddress.Parse(ip)), QType.PTR, QClass.IN);
                foreach (RecordPTR ptr in response.RecordsPTR)
                {
                    string host = RemoveLastPoint(ptr.PTRDNAME);
                    if (host != string.Empty)
                    {
                        lst.Add(host);
                    }
                }
            }
            catch { }
            finally
            {
                r.DnsServers = lastNSServers.ToArray();
            }
            return lst;
        }

        public static bool IsIPv4(String s)
        {
            Regex erIP = new Regex(@"^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$");
            return erIP.IsMatch(s);
        }

        /// <summary>
        /// Dada una cadena elimina el último punto si existe
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static string RemoveLastPoint(string domain)
        {
            return domain.EndsWith(".") ? domain.Substring(0, domain.Length - 1) : domain;
        }

        /// <summary>
        /// Devuelve los 4 bytes de una ipv4
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static byte[] IPToByte(string ip)
        {
            byte[] ipB = new byte[4];
            string[] ipparts = ip.Split('.');
            for (int i = 0; i < 4; i++)
                ipB[i] = Byte.Parse(ipparts[i]);
            return ipB;
        }

        /// <summary>
        /// Dada una ip la incrementa un numero el byte menos significativo
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static IPAddress IncrementIP(IPAddress ip)
        {
            byte[] ipBytes = ip.GetAddressBytes();
            if (ipBytes[3] < 255)
            {
                ipBytes[3]++;

            }
            else if (ipBytes[2] < 255)
            {
                ipBytes[2]++;
                ipBytes[3] = 0;
            }
            else if (ipBytes[1] < 255)
            {
                ipBytes[1]++;
                ipBytes[2] = 0;
                ipBytes[3] = 0;
            }
            else if (ipBytes[0] < 255)
            {
                ipBytes[0]++;
                ipBytes[1] = 0;
                ipBytes[2] = 0;
                ipBytes[3] = 0;
            }
            else
            {
                ipBytes[0] = 0;
                ipBytes[1] = 0;
                ipBytes[2] = 0;
                ipBytes[3] = 0;
            }
            return new IPAddress(ipBytes);
        }

        public static bool IsPrivateIP(IPAddress ip)
        {
            string strIP = ip.ToString();
            return ((strIP.CompareTo("10.0.0.0") >= 0 &&
                     strIP.CompareTo("10.255.255.255") <= 0) ||
                    (strIP.CompareTo("172.16.0.0") >= 0 &&
                     strIP.CompareTo("172.31.255.255") <= 0) ||
                    (strIP.CompareTo("192.168.0.0") >= 0 &&
                     strIP.CompareTo("192.168.255.255") <= 0));
        }

        /// <summary>
        /// Devuelve todos los DNS que funcionen para un determinado dominio
        /// </summary>
        /// <param name="r"></param>
        /// <param name="domain"></param>
        /// <param name="NSServer"></param>
        /// <returns></returns>
        public static List<string> GetNSServer(Resolver r, string domain, string NSServer)
        {
            List<IPEndPoint> lastNSServers = new List<IPEndPoint>(r.DnsServers);
            try
            {
                r.DnsServer = NSServer;
                Response response = r.Query(domain, QType.NS);
                if (response.RecordsNS.Length > 0)
                {
                    List<string> lst = new List<string>();
                    //No es autoritativa, volver a preguntar
                    if (!response.header.AA)
                    {
                        foreach (RecordNS rNS in response.RecordsNS)
                        {
                            if (NSServer != rNS.NSDNAME)
                            {
                                foreach (string ns in GetNSServer(r, domain, rNS.NSDNAME))
                                    if (!lst.Contains(ns, StringComparer.OrdinalIgnoreCase))
                                        lst.Add(ns);
                            }
                            else
                            {
                                lst.Add(NSServer);
                            }
                        }
                    }
                    else
                    {
                        foreach (RecordNS rNS in response.RecordsNS)
                            lst.Add(RemoveLastPoint(rNS.NSDNAME));
                    }
                    //Resuleve los dominios de los DNS
                    List<string> ips = new List<string>();
                    foreach (string ns in lst)
                    {
                        foreach (IPAddress ip in DNSUtil.GetHostAddresses(ns))
                            if (!ips.Contains(ip.ToString()))
                            {
                                if (TestDNS(ip.ToString()))
                                {
                                    ips.Add(ip.ToString());
                                }
                            }
                    }
                    return ips;
                }
                //Hay servidores autoritativos para esta petición
                else if (response.Authorities.Count > 0)
                {
                    try
                    {
                        //Se devuelve el servidor DNS autoritativo
                        if (response.Authorities[0].RECORD is RecordSOA recordSOA)
                        {
                            string dns = RemoveLastPoint(recordSOA.MNAME);
                            if (TestDNS(dns))
                            {
                                List<string> lst = new List<string>();
                                lst.Add(dns);
                                return lst;
                            }
                        }
                        if (response.Authorities[0].RECORD is RecordNS recordNS)
                        {
                            string dns = RemoveLastPoint(recordNS.NSDNAME);
                            if (TestDNS(dns))
                            {
                                List<string> lst = new List<string>();
                                lst.Add(dns);
                                return lst;
                            }
                        }
                    }
                    catch { }
                }
            }
            catch { }
            finally
            {
                r.DnsServers = lastNSServers.ToArray();
            }
            return new List<string>();
        }

        /// <summary>
        /// Devuelve el primer DNS que funcione para un dominio dado
        /// </summary>
        /// <param name="r"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static string GetNSServer(Resolver r, string domain)
        {
            try
            {
                Response response = r.Query(domain, QType.NS);
                if (response.RecordsNS.Length > 0)
                {
                    //Se busca hasta encontrar el DNS autoritativo
                    if (response.header.AA == false)
                    {
                        foreach (RecordNS rNS in response.RecordsNS)
                        {
                            List<string> NSServers = GetNSServer(r, domain, rNS.NSDNAME);
                            if (NSServers.Count > 0)
                            {
                                foreach (string dns in NSServers)
                                {

                                    //Se devuelve el primero de ellos
                                    return DNSUtil.RemoveLastPoint(dns);
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (RecordNS rNS in response.RecordsNS)
                        {
                            //Si no es una respuesta autoritativa volver a preguntar
                            string dns = RemoveLastPoint(rNS.NSDNAME);
                            if (TestDNS(dns))
                                return dns;
                        }
                    }
                }
                //Hay servidores autoritativos para esta petición
                else if (response.Authorities.Count > 0)
                {
                    try
                    {
                        //Se devuelve el servidor DNS autoritativo
                        if ((response.Authorities[0]).RECORD is RecordSOA recordSOA)
                        {
                            string dns = RemoveLastPoint(recordSOA.MNAME);
                            if (TestDNS(dns))
                                return dns;
                        }
                        if ((response.Authorities[0]).RECORD is RecordNS recordNS)
                        {
                            string dns = RemoveLastPoint(recordNS.NSDNAME);
                            if (TestDNS(dns))
                                return dns;
                        }
                    }
                    catch { }
                }
            }
            catch { }
            //Devuelve el primer DNS local que funcione...
            foreach (IPEndPoint dns in GetLocalNSServer())
                if (TestDNS(dns.Address.ToString()))
                    return dns.Address.ToString();
            //Si ninguno a funcionado devolver el primero...
            return GetLocalNSServer()[0].Address.ToString();
        }

        /// <summary>
        /// Comprueba si un servidor DNS está funcionando
        /// </summary>
        /// <param name="DNSServer"></param>
        /// <returns></returns>
        public static bool TestDNS(string DNSServer)
        {
            Resolver resolver = new Resolver();
            resolver.DnsServer = DNSServer;
            //Response response = resolver.Query("test", QType.ANY);
            Response response = resolver.Query("google.com", QType.NS);
            return response.Error != "Timeout Error";
        }

        public static bool TestDNSCustom(string DNSServer, string host)
        {
            Resolver resolver = new Resolver();
            resolver.DnsServer = DNSServer;
            Response response = resolver.Query(host, QType.A);
            return response.Error != "Timeout Error";
        }

        /// <summary>
        /// Busca si un DNS resuelve todos los dominios, aunque no existan
        /// </summary>
        /// <param name="strServer"></param>
        /// <returns></returns>
        public static bool IsDNSAnyCast(Resolver r, string DNSServer, string strDomain)
        {
            List<IPEndPoint> lastNSServers = new List<IPEndPoint>(r.DnsServers);
            try
            {
                r.DnsServer = DNSServer;
                Response res = r.Query("zzzzyyxzzzzyyxzzzzyyx." + strDomain, QType.A);
                return res.Answers.Count > 0;
            }
            catch
            {
                //Si hay un error, n se sabe si es anycast o no, devuelve false por defecto
                return false;
            }
            finally
            {
                r.DnsServers = lastNSServers.ToArray();
            }
        }

        /// <summary>
        /// Devuelve los servidores DNS que se tengan configurados localmente
        /// </summary>
        /// <returns></returns>
        public static List<IPEndPoint> GetLocalNSServer()
        {
            List<IPEndPoint> NSServers = new List<IPEndPoint>();
            //Busca los servidores DNS locales
            foreach (IPEndPoint ip in Resolver.GetDnsServers())
            {
                //Por el momento ignoramos IPv6...
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    NSServers.Add(new IPEndPoint(ip.Address, 53));
            }
            //Si no encuentra servidores DNS locales usa los servidores DNS de OpenDNS
            if (NSServers.Count == 0)
            {
                foreach (IPEndPoint ip in Resolver.DefaultDnsServers)
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        NSServers.Add(new IPEndPoint(ip.Address, 53));
            }
            return NSServers;
        }
    }
}
