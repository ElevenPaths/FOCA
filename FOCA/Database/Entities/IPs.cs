using FOCA.Analysis.DNSCacheSnooping;
using FOCA.ModifiedComponents;
using FOCA.Utilites;
using System;

namespace FOCA.Database.Entities
{
    [Serializable]
    public class IPs : IDisposable
    {
        private ThreadSafeList<IPsItem> itemsField;

        public IPs()
        {
            itemsField = new ThreadSafeList<IPsItem>();
        }

        public ThreadSafeList<IPsItem> Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    itemsField.Dispose();
                }

                itemsField = null;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

    [Serializable]
    public class ExtendedIPInformation
    {
        public int Id { get; set; }
        public string OS { get; set; }
        public string Country { get; set; }
        public string ServerBanner { get; set; }
        public string ShodanResponse { get; set; }
    }

    [Serializable]
    public class IPsItem : BaseItem
    {
        public int Id { get; set; }
        public string Ip { get; set; }
        public string Source { get; set; }
        public virtual ExtendedIPInformation Information { get; set; }

        public bool activeDNS { get; set; } = false;

        public bool ZoneTransfer { get; set; } = false;

        public IPsItem() : base()
        {
        }

        public IPsItem(string ip, string source) : base()
        {
            this.Ip = ip;
            this.Source = source;
        }

        public NetRange GetNetrange()
        {
            NetRange netrange;
            for (int i = 0; i < Program.data.Project.LstNetRange.Count; i++)
            {
                netrange = Program.data.Project.LstNetRange[i];

                if (netrange.IsIpInNetrange(this.Ip))
                    return netrange;
            }
            return null;
        }

        public override string ToString()
        {
            return Ip;
        }
    }


    class DNSThread
    {
        public string dnsIp, host;
        public DnsCache dnsCache;

        public DNSThread(string dnsIp, string host, DnsCache dnsCache)
        {
            this.dnsIp = dnsIp;
            this.host = host;
            this.dnsCache = dnsCache;
        }
    }
}