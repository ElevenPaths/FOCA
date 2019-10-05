using FOCA.Analysis.FingerPrinting;
using FOCA.ModifiedComponents;
using FOCA.Plugins;
using FOCA.TaskManager;
using FOCA.Threads;
using Heijden.DNS;
using MetadataExtractCore.Diagrams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FOCA.Database.Entities
{
    [Serializable]
    public class Data : IDisposable
    {
        public PluginsAPI.Export PluginExport;

        public Project Project;

        public Ficheros files;

        [NonSerialized]
        public Tasker tasker;

        public IPs Ips;

        public Domains domains;

        public Relations relations;

        public Computers computers;

        public ComputerIPs computerIPs;

        public ComputerDomains computerDomains;

        public event EventHandler OnChange;

        public event EventHandler<EventsThreads.ThreadStringEventArgs> OnLog;

        public event EventHandler NewDomainByHTTPServer;

        public event EventHandler NewDomainByMXServer;

        public event EventHandler NewDomainByFTPServer;

        public event EventHandler NewDomainByDNSServer;

#if PLUGINS
        [NonSerialized]
        public PluginList plugins;
#endif
        private Resolver resolver;

        public ThreadSafeList<Limits> lstLimits;

        public Data()
        {
            PluginExport = new PluginsAPI.Export();
            Project = new Project();
            files = new Ficheros();
            Ips = new IPs();
            domains = new Domains();
            relations = new Relations();
            computers = new Computers();
            computerIPs = new ComputerIPs();
            computerDomains = new ComputerDomains();
            resolver = new Resolver();
#if PLUGINS
            plugins = new PluginList();
#endif
            lstLimits = new ThreadSafeList<Limits>();
        }

        public void Clear()
        {
            files.Items.Clear();
            Ips.Items.Clear();
            domains.Items.Clear();
            relations.Items.Clear();
            computers.Items.Clear();
            computerIPs.Items.Clear();
            computerDomains.Items.Clear();

            if (tasker == null)
            {
                tasker = new Tasker();
                tasker.AsociaEventosTareas();
            }

            tasker.RemoveAllTasks();
        }

        public Limits GetLimitFromIp(string ip)
        {
            var r = ip.Split(new char[] { '.' })[0] + "." +
                        ip.Split(new char[] { '.' })[1] + "." +
                        ip.Split(new char[] { '.' })[2];

            return (from t in lstLimits let result = t.Range == r where result select t).FirstOrDefault();
        }

        public void AddLimit(Limits limit)
        {
            var found = false;

            foreach (var t in lstLimits.Where(t => t.Range == limit.Range))
            {
                found = true;
            }

            if (found == false)
                lstLimits.Add(limit);
        }

        public bool IsIpInLimitRange(string ip)
        {
            var result = false;

            if (DNSUtil.IsIPv4(ip))
            {
                foreach (var limit in lstLimits)
                {
                    result = limit.IsInRangeLimit(ip);

                    if (result)
                        return true;
                }
            }

            return false;
        }

        public void SetResolver(Resolver r)
        {
            resolver = r;
        }

        /// <summary>
        /// Add domain if this not exist in the list.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="source"></param>
        /// <param name="maxRecursion"></param>
        /// <param name="cfgCurrent"></param>
        public void AddDomain(string domain, string source, int maxRecursion, Configuration cfgCurrent)
        {
            domain = domain.Trim();

            if (domains.Items.Any(S => S.Domain.ToLower() == domain.ToLower())) return;

            var dItem = new DomainsItem(domain, source);
            domains.Items.Add(dItem);
#if PLUGINS
            Thread tPluginOnDomain = new Thread(new ParameterizedThreadStart(Program.data.plugins.OnNewDomain));
            tPluginOnDomain.IsBackground = true;
            object[] oDomain = new object[] { new object[] { domain } };
            tPluginOnDomain.Start(oDomain);
#endif
            var domainParts = domain.Split('.');
            var currentdomain = domainParts[domainParts.Length - 1];

            for (var i = 2; i < domainParts.Length; i++)
            {
                currentdomain = domainParts[domainParts.Length - i] + "." + currentdomain;

                AddDomain(currentdomain, string.Format("{0} > Inferred by {2} [{1}]", GetDomainSource(domain), currentdomain, domain), maxRecursion - 1, cfgCurrent);
            }

            if (maxRecursion <= 0)
            {
                OnChangeEvent(null);
                return;
            }

            //OnLog(null, new EventsThreads.ThreadStringEventArgs(string.Format("Resolving domain: {0}", domain)));

            var listIpsOfDomain = DNSUtil.GetHostAddresses(domain);

            if (listIpsOfDomain.Count == 0)
            {
                var computer = new ComputersItem();
                computer.type = ComputersItem.Tipo.Server;
                computer.name = domain;
                computer.NotOS = true;
                computer.os = OperatingSystem.OS.Unknown;
                if (!computers.Items.Any(S => S.name == domain))
                    computers.Items.Add(computer);
            }

            foreach (var IP in listIpsOfDomain)
            {
                if (Program.data.IsMainDomainOrAlternative(domain))
                {
                    var limit = Program.data.GetLimitFromIp(IP.ToString());

                    if (limit == null)
                        Program.data.AddLimit(new Limits(IP.ToString()));
                    else
                    {
                        var lastOct = int.Parse(IP.ToString().Split(new char[] { '.' })[3]);

                        if (lastOct < limit.Lower)
                            limit.Lower = lastOct;
                        else if (lastOct > limit.Higher)
                            limit.Higher = lastOct;
                    }
                }

                AddResolution(domain, IP.ToString(), string.Format("{0} > DNS resolution [{1}]", GetDomainSource(domain), IP.ToString()), maxRecursion - 1, Program.cfgCurrent, false);
            }

            // Fingerprinting HTTP
            if (cfgCurrent.PassiveFingerPrintingHttp && cfgCurrent.FingerPrintingAllHttp)
            {
                if (NewDomainByHTTPServer != null)
                    NewDomainByHTTPServer(dItem, null);
            }
            else if ((cfgCurrent.PassiveFingerPrintingHttp) && (source.ToLower() == "documents search" || source.ToLower().Contains("websearch") || source.ToLower().Contains("bing ip search") || source.ToLower().Contains("technologyrecognition") || source.ToLower().Contains("fingerprinting") || source.ToLower().Contains("certificate fingerprinting")))
            {
                if (NewDomainByHTTPServer != null)
                    NewDomainByHTTPServer(dItem, null);
            }
            // Fingerprinting SMTP
            if (cfgCurrent.PasiveFingerPrintingSmtp && cfgCurrent.FingerPrintingAllSmtp)
            {
                if (NewDomainByMXServer != null)
                    NewDomainByMXServer(dItem, null);
            }

            else if ((cfgCurrent.PasiveFingerPrintingSmtp) && (source.ToLower().Contains("mx server")))
            {
                if (NewDomainByMXServer != null)
                    NewDomainByMXServer(dItem, null);
            }

            // Fingerprinting FTP
            if (cfgCurrent.FingerPrintingAllFtp)
            {
                if (NewDomainByFTPServer != null)
                    NewDomainByFTPServer(dItem, null);
            }

            OnChangeEvent(null);
        }

        /// <summary>
        /// Return domain.
        /// </summary>
        /// <param name="subdomain"></param>
        /// <returns></returns>
        public string GetDomainFromSubdomain(string subdomain)
        {
            string[] domSplited = subdomain.Split(new char[] { '.' });
            return domSplited[domSplited.Length - 2] + "." + domSplited[domSplited.Length - 1];
        }

        public List<RelationsItem> GetRelationsOfIP(string ip)
        {
            var r = relations.Items.Where(R => R.Ip.ToString() == ip).ToList<RelationsItem>();
            return r;
        }

        public DomainsItem GetDomain(string domain)
        {
            domain = domain.Trim();
            return domains.Items.FirstOrDefault(D => D.Domain.ToLower() == domain.ToLower());
        }

        public List<string> GetDomains()
        {
            return domains.Items.Select(D => D.Domain).ToList<string>();
        }

        public ComputerDomains GetComputerDomainsFromDomainsItem(DomainsItem dominio)
        {
            var lstComputers = new ComputerDomains();

            var po = new ParallelOptions();
            if (Program.cfgCurrent.ParallelDnsQueries != 0)
                po.MaxDegreeOfParallelism = Program.cfgCurrent.ParallelDnsQueries;
            Parallel.ForEach(Program.data.computerDomains.Items, S =>
            {
                if ((S.Domain != null) && (S.Domain.Domain == dominio.Domain))
                    lstComputers.Items.Add(S);
            });
            return lstComputers;
        }

        public IPsItem GetIpsItemFromDomainItem(DomainsItem dominio)
        {
            IPsItem ip = null;

            var po = new ParallelOptions();
            if (Program.cfgCurrent.ParallelDnsQueries != 0)
                po.MaxDegreeOfParallelism = Program.cfgCurrent.ParallelDnsQueries;
            Parallel.For(0, Program.data.computerDomains.Items.Count, po, delegate (int i)
                {
                    if (dominio == computerDomains.Items[i].Domain)
                    {
                        for (int c = 0; c < computerIPs.Items.Count; c++)
                        {
                            if (computerIPs.Items[c].Computer.name == computerDomains.Items[i].Computer.name)
                            {
                                ip = computerIPs.Items[c].Ip;
                                return;
                            }
                        }
                    }
                });

            return ip;
        }

        public IPsItem GetComputerIpFromDomainItem(string dominio)
        {
            return GetIpsItemFromDomainItem(Program.data.GetDomain(dominio));
        }

        public string GetDomainSource(string domain)
        {
            domain = domain.Trim();
            DomainsItem d = null;
            try
            {
                d = domains.Items.First(S => S.Domain.ToLower() == domain.ToLower());
            }
            catch { }
            return d != null ? d.Source : string.Empty;
        }

        public void AddIP(string ip, string source, int MaxRecursion)
        {
            AddIP(ip, source, null, MaxRecursion, true);
        }

        public static bool isPrivateIP(IPsItem IPv4)
        {
            return (isPrivateIP(IPv4.Ip));
        }

        public static bool isPublicIP(IPsItem IPv4)
        {
            return (isPublicIP(IPv4.Ip));
        }

        public static bool isPrivateIP(string IPv4)
        {
            return (!isPublicIP(IPv4));
        }

        public static bool isIPv6(string ip)
        {
            if (ParseIPV6(ip) != null)
                return true;
            return false;
        }

        /// <summary>
        /// Return Type of Ip.
        /// </summary>
        /// <param name="ipValue"></param>
        /// <returns>AddressFamily</returns>
        public static AddressFamily GetIpType(string ipValue)
        {
            IPAddress address;

            var type = AddressFamily.Unknown;

            if (!IPAddress.TryParse(ipValue, out address)) return type;

            switch (address.AddressFamily)
            {
                case AddressFamily.InterNetwork:
                    type = AddressFamily.InterNetwork;
                    break;
                case AddressFamily.InterNetworkV6:
                    type = AddressFamily.InterNetworkV6;
                    break;
                case AddressFamily.Unknown:
                    return type;
            }

            return type;
        }

        public static string ParseIPV6(string ip)
        {
            if (ip.StartsWith(":"))
                ip = "0" + ip;
            if (ip.EndsWith(":"))
                ip = ip + "0";

            var sbIp = new StringBuilder();
            for (var bloque = 0; bloque < ip.Split(new char[] { ':' }).Length; bloque++)
            {
                var sBloque = ip.Split(new char[] { ':' })[bloque];

                if (sBloque.Length == 0)
                {
                    sBloque = "[PADDING]" + sBloque;
                }
                else
                {
                    int nBytesPadding = 4 - sBloque.Length;
                    for (int padding = 0; padding < nBytesPadding; padding++)
                    {
                        sBloque = "0" + sBloque;
                    }
                }

                sbIp.Append(sBloque);
                if (bloque < ip.Split(new char[] { ':' }).Length - 1)
                    sbIp.Append(":");
            }

            var nBloquesReales = 8;
            var nBloquesPadding = nBloquesReales - ip.Split(new char[] { ':' }).Length;

            if (nBloquesPadding >= 0)
            {
                var sbPadding = new StringBuilder();
                for (var i = 0; i <= nBloquesPadding; i++)
                {
                    sbPadding.Append("0000");
                    if (i < nBloquesPadding)
                        sbPadding.Append(":");
                }
                sbIp.Replace("[PADDING]", sbPadding.ToString());
            }

            if (sbIp.ToString().Split(new char[] { ':' }).Length == 8 && sbIp.ToString().Length == 39)
                return sbIp.ToString();
            return null;
        }

        public static bool isPublicIP(string IPv4)
        {
            try
            {
                var primerOcteto = int.Parse(IPv4.Split(new char[] { '.' })[0]);
                var segundoOcteto = int.Parse(IPv4.Split(new char[] { '.' })[1]);

                if (primerOcteto == 10)
                    return false;
                if ((primerOcteto == 192) && (segundoOcteto == 168))
                    return false;
                if ((primerOcteto == 172) && (segundoOcteto >= 16) && (segundoOcteto <= 31))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Add Ip.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="source"></param>
        /// <param name="domainSource"></param>
        public void AddIP(string ip, string source, string domainSource, int MaxRecursion, bool doptr)
        {
            ip = ip.Trim();

            if (isIPv6(ip))
                ip = ParseIPV6(ip);

            if (!Ips.Items.Any(I => I.Ip.ToLower() == ip.ToLower()))
            {
                if (isPublicIP(ip))
                {
                    var isInNetrange = Project.IsIpInNetrange(ip);

                    if (!isInNetrange)
                    {
                        var host = string.Empty;
                        try
                        {
                            host = Dns.GetHostEntry(ip).HostName;

                            if (Program.data.Project.LstNetRange.Count == 0)
                            {
                                if (Program.data.Project.Domain != null)
                                {
                                    if (!IsMainDomainOrAlternative(host))
                                    {
                                        if (Program.data.Project.AlternativeDomains.Select(S => host.Contains(S.ToString())).Count() == 0)
                                        {
                                            string[] arrDom = host.Split(new char[] { '.' });
                                            if (arrDom.Length > 1)
                                            {
                                                string auxFinalDom = arrDom[arrDom.Length - 2] + "." + arrDom[arrDom.Length - 1];
                                                Program.data.Project.AlternativeDomains.Add(auxFinalDom);
                                                Program.LogThis(new Log(Log.ModuleType.FOCA, "IP address associated to " + Program.data.Project.Domain + " belongs to a Netrange of " + auxFinalDom + ". It is going to be added as an alternative domain.", Log.LogType.low));
                                            }

                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                        }

                        if (IsMainDomainOrAlternative(host))
                        {
                            var netrange = Project.GetNetrange(ip);

                            if (netrange != null)
                            {
                                Project.LstNetRange.Add(netrange);
#if PLUGINS
                                Thread tPluginOnNetrange = new Thread(new ParameterizedThreadStart(Program.data.plugins.OnNewNetrange));
                                tPluginOnNetrange.IsBackground = true;
                                object[] oNetRange = new object[] { new object[] { netrange.from, netrange.to } };
                                tPluginOnNetrange.Start(oNetRange);
#endif

                                if (!Program.cfgCurrent.ScanNetranges255 || Project.GetIpsOfNetrange(netrange) <= 255)
                                {
                                    List<string> lstIps = netrange.GenerateIpsOfNetrange();
                                    Program.LogThis(new Log(Log.ModuleType.IPRangeSearch, "Netrange with " + lstIps.Count.ToString() + " IPs", Log.LogType.low));
                                    Thread tAddIps = new Thread(new ParameterizedThreadStart(AddIpListAsync));
                                    tAddIps.IsBackground = true;
                                    tAddIps.Priority = ThreadPriority.Lowest;
                                    tAddIps.Start(lstIps);
                                }
                            }
                        }
                    }
                }

                var ipItem = new IPsItem(ip, source);
                Ips.Items.Add(ipItem);

                // OnNewIP
#if PLUGINS
                Thread tPluginOnIP = new Thread(new ParameterizedThreadStart(Program.data.plugins.OnNewIP));
                tPluginOnIP.IsBackground = true;

                object[] oIP = new object[] { new object[] { ip } };
                tPluginOnIP.Start(oIP);
#endif
                if (MaxRecursion <= 0)
                {
                    OnChangeEvent(null);
                    return;
                }

                List<string> domains;
                if (doptr)
                {
                    if (domainSource != null)
                    {
                        if (Program.cfgCurrent.UseAllDns)
                        {
                            domains = new List<string>();
                            List<string> dnsServers = DNSUtil.GetNSServer(resolver, domainSource, DNSUtil.GetLocalNSServer().First().ToString());

                            foreach (string dns in dnsServers)
                            {
                                OnLog(null, new EventsThreads.ThreadStringEventArgs(string.Format("Making reverse resolution to IP: {0} Using DNS server: {1}", ip, dns)));

                                foreach (var domain in DNSUtil.GetHostNames(resolver, ip, dns).Where(domain => !domains.Contains(domain)))
                                {
                                    domains.Add(domain);
                                }
                            }
                        }
                        else
                        {
                            var dnsserver = DNSUtil.GetNSServer(resolver, domainSource);
                            OnLog(null, new EventsThreads.ThreadStringEventArgs(string.Format("Making reverse resolution to IP: {0} Using DNS server: {1}", ip, dnsserver)));
                            domains = DNSUtil.GetHostNames(resolver, ip, dnsserver);
                        }
                    }
                    else
                        domains = DNSUtil.GetHostNames(resolver, ip);
                    foreach (var domain in domains)
                        AddResolution(domain, ip, string.Format("{0} > DNS reverse resolution [{1}]", GetIpSource(ip), domain), MaxRecursion - 1, Program.cfgCurrent, true);
                }
                OnChangeEvent(null);
            }
        }

        private void AddIpListAsync(object lstIpsObject)
        {
            var lstIps = (List<string>)lstIpsObject;
            Thread tNewIp;


            object[] paramsAddNewIp;

            foreach (string ipNR in lstIps)
            {
                paramsAddNewIp = new object[] { ipNR, "Netrange", Program.cfgCurrent.MaxRecursion };
                tNewIp = new Thread(AddIpAsync);
                TaskFOCA taskFoca = new TaskFOCA(tNewIp, paramsAddNewIp, "Add new IP [" + ipNR + "]");
                tasker.AddTask(taskFoca);
            }

        }

        private static void AddIpAsync(object paramsNewIp)
        {
            object[] objArray = (object[])paramsNewIp;

            var ip = objArray[0].ToString();
            var description = objArray[1].ToString();
            var maxRecursion = int.Parse(objArray[2].ToString());

            Program.data.AddIP(ip, description, maxRecursion);
            Program.LogThis(new Log(Log.ModuleType.IPRangeSearch, "New IP (" + ip + ") found by netrange.", Log.LogType.low));
        }

        /// <summary>
        /// Return IpItem by ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public IPsItem GetIp(string ip)
        {
            ip = ip.Trim();

            if (GetIpType(ip) == AddressFamily.InterNetworkV6)
                ip = IPAddress.Parse(ip).ToString();

            IPsItem res;
            try
            {
                res = Ips.Items.FirstOrDefault(x => x.Ip == ip);
                if (res == null)
                {
                    res = new IPsItem("", "");
                }
            }
            catch
            {
                res = new IPsItem("", "");
            }
            return res;
        }

        public void SetIPInformation(string ip, ExtendedIPInformation info)
        {
            var i = GetIp(ip);
            if (i != null)
            {
                i.Information = info;
            }
        }

        public ExtendedIPInformation GetIPInformation(string ip)
        {
            var i = GetIp(ip);
            return i?.Information;
        }

        /// <summary>
        /// Devuelve TODAS las Ips
        /// </summary>
        /// <returns></returns>
        public ThreadSafeList<string> GetIPs()
        {
            return new ThreadSafeList<string>(Ips.Items.Select(s => s.Ip));
        }

        /// <summary>
        /// Validate domain if is alternative or not
        /// </summary>
        /// <param name="dom"></param>
        /// <returns></returns>
        public bool IsDomainOrAlternative(string dom)
        {
            if (Program.data.Project.Domain == null)
                return false;

            if (Program.data.Project.Domain == "*")
                return true;

            if (dom.EndsWith(Program.data.Project.Domain))
            {
                if (dom.Length == Program.data.Project.Domain.Length)
                    return true;//dominio.com

                if (dom.EndsWith("." + Program.data.Project.Domain))
                    return true;
            }

            foreach (var alternativo in Project.AlternativeDomains)
            {
                if (dom.Length == alternativo.Length)
                    return true;//dominio.com

                if (dom.EndsWith("." + alternativo))
                    return true;
            }
            return false;
        }

        public string GetIpSource(string ip)
        {
            ip = ip.Trim();
            var lstIP = new ThreadSafeList<IPsItem>(Ips.Items.Where(I => I.Ip.ToLower() == ip.ToLower()));
            return lstIP.Count != 0 ? lstIP[0].Source : null;
        }

        public void AddResolution(string domain, string IP, string source, int MaxRecursion, Configuration cfgCurrent, bool doptr)
        {
            domain = domain.Trim();
            IP = IP.Trim();

            AddDomain(domain, source, MaxRecursion, cfgCurrent);

            AddIP(IP, source, domain, MaxRecursion, doptr);

            if (!relations.Items.Any(R => R.Domain.Domain.ToLower() == domain.ToLower() &&
                               R.Ip.Ip.ToLower() == IP.ToLower()))
            {
                var domAsoci = GetDomain(domain);
                var ipAsoci = GetIp(IP);
                relations.Items.Add(new RelationsItem(domAsoci, ipAsoci, source));
#if PLUGINS
                var tPluginOnRelation = new Thread(new ParameterizedThreadStart(Program.data.plugins.OnNewRelation));
                tPluginOnRelation.IsBackground = true;
                var oAsoci = new object[] { new object[] { ipAsoci.Ip, domAsoci.Domain } };
                tPluginOnRelation.Start(oAsoci);
#endif

                GetServersFromIPs();
                OnChangeEvent(null);
            }
        }

        public ThreadSafeList<IPsItem> GetResolutionIPs(string domain)
        {
            domain = domain.Trim().ToLower();

            var lstIPs = new ThreadSafeList<string>(relations.Items.Where(W => W.Domain.Domain.ToLower() == domain.ToLower()).Select(S => S.Ip.Ip));
            return new ThreadSafeList<IPsItem>(Ips.Items.Where(I => lstIPs.Contains(I.Ip)));
        }

        public ThreadSafeList<DomainsItem> GetResolutionDomains(string ip)
        {
            ip = ip.Trim().ToLower();
            var lstDominios = new ThreadSafeList<string>(relations.Items.Where(W => W.Ip.Ip.ToLower() == ip.ToLower()).Select(S => S.Domain.Domain));

            return new ThreadSafeList<DomainsItem>(domains.Items.Where(D => lstDominios.Contains(D.Domain)));
        }

        /// <summary>
        ///
        /// </summary>
        public void GetServersFromIPs()
        {
            var lstDomains = new List<string>();
            if (!string.IsNullOrEmpty(Project.Domain))
                lstDomains.Add(Project.Domain);
            lstDomains.AddRange(Project.AlternativeDomains);

            var lstIPs = new ThreadSafeList<IPsItem>(Ips.Items.Where(IP => Project.Domain == "*" || relations.Items.Any(R => R.Ip.Ip == IP.Ip && lstDomains.Any(D => R.Domain.Domain.ToLower().EndsWith(D.ToLower())))));

            var po = new ParallelOptions();
            if (Program.cfgCurrent.ParallelDnsQueries != 0)
                po.MaxDegreeOfParallelism = Program.cfgCurrent.ParallelDnsQueries;
            Parallel.ForEach(lstIPs, ip =>
                {
                    try
                    {
                        ComputersItem ci;

                        if (computerIPs.Items.Any(C => C.Ip.Ip == ip.Ip))
                        {
                            ci = computerIPs.Items.First(C => C.Ip.Ip == ip.Ip).Computer;
                        }

                        else
                        {
                            if (computers.Items.Any(C => relations.Items.Where(R => R.Ip.Ip == ip.Ip).Select(D => D.Domain).Any(D => string.Equals(D.Domain, C.name, StringComparison.OrdinalIgnoreCase))))
                            {
                                ci = computers.Items.First(C => relations.Items.Where(R => R.Ip.Ip == ip.Ip).Select(D => D.Domain).Any(D => string.Equals(D.Domain, C.name, StringComparison.OrdinalIgnoreCase)));
                                ci.name = string.Format("{0} [{1}]", ci.name, ip.Ip);
                                computerIPs.Items.Add(new ComputerIPsItem(ci, ip, ip.Source));
                            }

                            else
                            {
                                ci = new ComputersItem();
                                computers.Items.Add(ci);
                                ci.type = ComputersItem.Tipo.Server;
                                ci.os = OperatingSystem.OS.Unknown;

                                var strFirstDomain = string.Empty;

                                try
                                {
                                    strFirstDomain = relations.Items.First(R => R.Ip.Ip == ip.Ip && lstDomains.Any(D => R.Domain.Domain.ToLower().EndsWith(D.ToLower()))).Domain.Domain;
                                }
                                catch
                                {
                                    strFirstDomain = "*";
                                }

                                ci.name = string.Format("{0} [{1}]", strFirstDomain, ip.Ip);

                                computerIPs.Items.Add(new ComputerIPsItem(ci, ip, ip.Source));
                            }
                        }

                        foreach (DomainsItem di in relations.Items.Where(R => R.Ip.Ip == ip.Ip).Select(D => D.Domain))
                        {
                            if (!computerDomains.Items.Any(C => C.Computer.name == ci.name && C.Domain.Domain == di.Domain))
                                computerDomains.Items.Add(new ComputerDomainsItem(ci, di, di.Source));

                            for (var fpI = 0; fpI < di.fingerPrinting.Count; fpI++)
                            {
                                var fp = di.fingerPrinting[fpI];

                                if ((fp.os != OperatingSystem.OS.Unknown))
                                {
                                    ci.os = fp.os;
                                }

                                foreach (var software in BannerAnalysis.GetSoftwareFromBanner(fp.Version).Where(software => !ci.Software.Items.Any(A => A.Name.ToLower() == software.ToLower())))
                                {
                                    ci.Software.Items.Add(new ApplicationsItem(software, string.Format("{0} FingerPrinting Banner: {1}", di.Domain, fp.Version)));
                                }
                            }
                        }
                        if (ip.Information != null)
                        {
                            if (!string.IsNullOrEmpty(ip.Information.OS))
                            {
                                var os = OperatingSystemUtils.StringToOS(ip.Information.OS);

                                if (ci.os == OperatingSystem.OS.Unknown && os != OperatingSystem.OS.Unknown)
                                    ci.os = os;
                            }

                            if (!string.IsNullOrEmpty(ip.Information.ServerBanner))
                            {
                                var os = OperatingSystemUtils.StringToOS(ip.Information.ServerBanner);
                                if (ci.os == OperatingSystem.OS.Unknown && os != OperatingSystem.OS.Unknown)
                                    ci.os = os;

                                foreach (var software in BannerAnalysis.GetSoftwareFromBanner(ip.Information.ServerBanner).Where(software => !ci.Software.Items.Any(A => A.Name.ToLower() == software.ToLower())))
                                {
                                    ci.Software.Items.Add(new ApplicationsItem(software, string.Format("{0} Shodan Banner: {1}", ip, ip.Information.ServerBanner)));
                                }
                            }
                        }
                    }
                    catch
                    {
                    }
                });
            OnChangeEvent(null);
        }

        public bool IsMainDomainOrAlternative(string host)
        {
            if (Program.data.Project.Domain == null)
                return false;

            if (Program.data.Project.Domain == "*")
                return true;

            if (((Program.data.Project.Domain != null) && (host.EndsWith("." + Program.data.Project.Domain) || Program.data.Project.Domain == host) ||
                            Program.data.Project.AlternativeDomains.Where<string>(D => host.EndsWith(D)).Count() > 0))
            {
                return true;
            }

            return false;
        }

        protected void OnChangeEvent(EventArgs e)
        {
            OnChange?.Invoke(this, e);
        }

        protected void OnLogEvent(EventsThreads.ThreadStringEventArgs e)
        {
            OnLog?.Invoke(this, e);
        }

        #region fingerprinting
        public bool IsAssignedHttpEvents()
        {
            return NewDomainByHTTPServer != null;
        }

        public bool IsAssignedFtpEvents()
        {
            return NewDomainByFTPServer != null;
        }

        public bool IsAssignedSmtpEvents()
        {
            return NewDomainByMXServer != null;
        }

        public bool IsAssignedDnsEvents()
        {
            return NewDomainByDNSServer != null;
        }

        public void FingerPrintDns(DomainsItem dom)
        {
            NewDomainByDNSServer?.Invoke(dom, null);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Project.Dispose();
                    tasker.Dispose();
                    relations.Dispose();
                    computers.Dispose();
                    computerIPs.Dispose();
                    files.Dispose();
                    Ips.Dispose();
                    computerDomains.Dispose();
                    lstLimits.Dispose();
                    domains.Dispose();
                    plugins.Dispose();
                }

                Project = null;
                tasker = null;
                computerDomains = null;
                lstLimits = null;
                domains = null;
                relations = null;
                computers = null;
                files = null;
                Ips = null;
                plugins = null;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        #endregion
    }
}
