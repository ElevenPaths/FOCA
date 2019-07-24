using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SearcherCore.Searcher.Shodan
{

    [DataContract]
    internal class Location
    {
        [DataMember]
        internal object city { get; set; }
        [DataMember]
        internal object region_code { get; set; }
        [DataMember]
        internal object area_code { get; set; }
        [DataMember]
        internal double longitude { get; set; }
        [DataMember]
        internal string country_code3 { get; set; }
        [DataMember]
        internal double latitude { get; set; }
        [DataMember]
        internal object postal_code { get; set; }
        [DataMember]
        internal object dma_code { get; set; }
        [DataMember]
        internal string country_code { get; set; }
        [DataMember]
        internal string country_name { get; set; }
    }

    [DataContract]
    internal class Shodan
    {
        [DataMember]
        internal string module { get; set; }
        [DataMember]
        internal string crawler { get; set; }
    }

    [DataContract]
    internal class Ssh
    {
        [DataMember]
        internal string mac { get; set; }
        [DataMember]
        internal string cipher { get; set; }
        [DataMember]
        internal string type { get; set; }
        [DataMember]
        internal string key { get; set; }
        [DataMember]
        internal string fingerprint { get; set; }
    }

    [DataContract]
    internal class Opts
    {
        [DataMember]
        internal Ssh ssh { get; set; }
    }

    [DataContract]
    internal class Datum
    {
        [DataMember]
        internal string product { get; set; }
        [DataMember]
        internal List<string> hostnames { get; set; }
        [DataMember]
        internal long ip { get; set; }
        [DataMember]
        internal string isp { get; set; }
        [DataMember]
        internal string transport { get; set; }
        [DataMember]
        internal List<string> cpe { get; set; }
        [DataMember]
        internal string data { get; set; }
        [DataMember]
        internal string asn { get; set; }
        [DataMember]
        internal int port { get; set; }
        [DataMember]
        internal string html { get; set; }
        [DataMember]
        internal Location location { get; set; }
        [DataMember]
        internal string timestamp { get; set; }
        [DataMember]
        internal List<string> domains { get; set; }
        [DataMember]
        internal string org { get; set; }
        [DataMember]
        internal string os { get; set; }
        [DataMember]
        internal Shodan _shodan { get; set; }
        [DataMember]
        internal Opts opts { get; set; }
        [DataMember]
        internal string ip_str { get; set; }
        [DataMember]
        internal string info { get; set; }
        [DataMember]
        internal string version { get; set; }
        [DataMember]
        internal string title { get; set; }
    }

    [DataContract]
    internal class ShodanResponse
    {
        internal object region_code { get; set; }
        [DataMember]
        internal long ip { get; set; }
        [DataMember]
        internal object area_code { get; set; }
        [DataMember]
        internal double latitude { get; set; }
        [DataMember]
        internal List<string> hostnames { get; set; }
        [DataMember]
        internal object postal_code { get; set; }
        [DataMember]
        internal object dma_code { get; set; }
        [DataMember]
        internal string country_code { get; set; }
        [DataMember]
        internal string org { get; set; }
        [DataMember]
        internal List<Datum> data { get; set; }
        [DataMember]
        internal string asn { get; set; }
        [DataMember]
        internal object city { get; set; }
        [DataMember]
        internal string isp { get; set; }
        [DataMember]
        internal double longitude { get; set; }
        [DataMember]
        internal string last_update { get; set; }
        [DataMember]
        internal string country_code3 { get; set; }
        [DataMember]
        internal List<string> vulns { get; set; }
        [DataMember]
        internal string country_name { get; set; }
        [DataMember]
        internal string ip_str { get; set; }
        [DataMember]
        internal string os { get; set; }
        [DataMember]
        internal List<int> ports { get; set; }
    }
}
