
using Heijden.DNS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Threading.Tasks.Parallel;

namespace FOCA.Analysis.DNSCacheSnooping
{
    class DnsCache
    {
        public event EventHandler Start;
        public event EventHandler Found;
        public event EventHandler End;

        /// <summary>
        /// Returns the DNS servers list of a domain
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetDnsList(string domain)
        {
            Resolver r = new Resolver("8.8.8.8");
            return r.Query(domain, Heijden.DNS.QType.NS).Answers.Select(a => a.RECORD.ToString().Trim('.'));
        }

        /// <summary>
        /// Checks if a list of domains exists in the cache of a DNS server
        /// </summary>
        /// <param name="dnsIp">IP of the DNS server</param>
        /// <param name="query">List of domains that FOCA will look for in the cache</param>
        /// <returns>List of domains that exist in the cache</returns>
        public List<string> Exists(string dnsIp, List<string> query)
        {
            Start?.Invoke(dnsIp, null);
            var domains = new List<string>();

            var po = new ParallelOptions();
            if (Program.cfgCurrent != null && Program.cfgCurrent.ParallelDnsQueries != 0)
                po.MaxDegreeOfParallelism = Program.cfgCurrent.ParallelDnsQueries;
            For(0, query.Count, po, delegate (int i)
            {
                if (ExistsInDnsServerCache(dnsIp, query[i]))
                    domains.Add(query[i]);
            }
            );

            End?.Invoke(dnsIp, null);

            return domains;
        }

        /// <summary>
        /// Checks if a domain exists in the DNS server's cache
        /// </summary>
        /// <params>
        /// <param name="dnsIp">DNS server's IP address</param>
        /// <param name="query">Query to be executed</param>
        /// </params>
        /// <returns>True if it exists</returns>
        private bool ExistsInDnsServerCache(string dnsIp, string query)
        {
            var r = new Heijden.DNS.Resolver(dnsIp) { Recursion = false };
            if (r.Query(query, Heijden.DNS.QType.A, Heijden.DNS.QClass.IN).Answers.Count > 0)
            {
                Found?.Invoke(query, null);
                return true;
            }
            return false;
        }
    }
}
