using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace FOCA.Utilites
{
    [Serializable]
    public class NetRange
    {
        public string from;
        public string to;
        public string netname;

        public NetRange()
        {
            // needed to be serializable
        }

        public NetRange(string from, string to, string netname)
        {
            this.from = from;
            this.to = to;
            this.netname = netname;
        }

        public bool IsIpInNetrange(string ip)
        {
            try
            {
                string[] from_octetos = from.Split(new char[] { '.' });
                string[] to_octetos = to.Split(new char[] { '.' });
                string[] ip_octetos = ip.Split(new char[] { '.' });

                if ((int.Parse(ip_octetos[0]) >= int.Parse(from_octetos[0])) && (int.Parse(ip_octetos[0]) <= int.Parse(to_octetos[0])))
                {
                    if ((int.Parse(ip_octetos[1]) >= int.Parse(from_octetos[1])) && (int.Parse(ip_octetos[1]) <= int.Parse(to_octetos[1])))
                    {
                        if ((int.Parse(ip_octetos[2]) >= int.Parse(from_octetos[2])) && (int.Parse(ip_octetos[2]) <= int.Parse(to_octetos[2])))
                        {
                            if ((int.Parse(ip_octetos[3]) >= int.Parse(from_octetos[3])) && (int.Parse(ip_octetos[3]) <= int.Parse(to_octetos[3])))
                                return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public List<string> GenerateIpsOfNetrange()
        {
            List<string> lstIps = new List<string>();
            try
            {
                string[] fromSplitted = this.from.Split('.');
                int[] rangeFrom =
                {
                    int.Parse(fromSplitted[0]),
                    int.Parse(fromSplitted[1]),
                    int.Parse(fromSplitted[2]),
                    int.Parse(fromSplitted[3])
                };

                string[] toSplitted = this.to.Split('.');
                int[] rangeTo =
                {
                    int.Parse(toSplitted[0]),
                    int.Parse(toSplitted[1]),
                    int.Parse(toSplitted[2]),
                    int.Parse(toSplitted[3])
                };

                while (!rangeFrom.SequenceEqual(rangeTo))
                {
                    var ip = rangeFrom[0] + "." + rangeFrom[1] + "." + rangeFrom[2] + "." + rangeFrom[3];
                    lstIps.Add(ip);

                    if (rangeFrom[3] == 255)
                    {
                        rangeFrom[3] = 0;

                        if (rangeFrom[2] == 255)
                        {
                            rangeFrom[2] = 0;
                            if (rangeFrom[1] == 255)
                            {
                                rangeFrom[1] = 0;
                                rangeFrom[0]++;
                                if (rangeFrom[0] == 225 + 1)
                                    break;
                            }
                            else
                                rangeFrom[1]++;
                        }
                        else
                            rangeFrom[2]++;
                    }
                    else
                        rangeFrom[3]++;
                }

                return lstIps;
            }
            catch
            {
                return lstIps;
            }

        }

        private static NetRange API(string ip)
        {
            try
            {
                // send request to json endpoint
                HttpWebRequest ripeRequest = HttpWebRequest.CreateHttp("https://stat.ripe.net/data/whois/data.json?resource=" + ip);
                ripeRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:54.0) Gecko/20100101 Firefox/54.0";
                ripeRequest.Headers.Add("Upgrade-Insecure-Requests: 1");
                ripeRequest.Method = "GET";
                ripeRequest.Headers.Add(HttpRequestHeader.Cookie, "serverid=www-plone-3; path=/");
                // parse response
                using (HttpWebResponse resp = (HttpWebResponse)ripeRequest.GetResponse())
                {
                    string response;
                    using (var sr = new StreamReader(resp.GetResponseStream()))
                    {
                        response = sr.ReadToEnd();
                    }

                    // get netrange (if any)
                    JObject json = JObject.Parse(response);

                    string netrange = json["data"]["records"][0][1]["value"].ToString();

                    if (!netrange.Contains("/"))
                        netrange = json["data"]["records"][0][0]["value"].ToString();

                    var ips = NetRange.GetNetRangeIPs(netrange);
                    return new NetRange(ips[0], ips[1], "netrangeObtainedFromRipe");
                }
            }
            catch (Exception)
            {
                // if there wasn't any netrange, return whole 0.0.0.0 to 255.255.255.255
                return null;//new NetRange("0.0.0.0", "255.255.255.255", "ripe_noNetRangeFound");
            }
        }

        private static List<string> GetNetRangeIPs(string netrange)
        {
            List<string> ips = new List<string>(2);
            var direction = netrange.Split('/')[0];
            var bits = Convert.ToInt32(netrange.Split('/')[1]); //Change / for -
            var stringBytes = direction.Split('.');
            IPAddress ip = new IPAddress(new byte[] {
                            Convert.ToByte(Convert.ToDecimal(stringBytes[0])),
                            Convert.ToByte(Convert.ToDecimal(stringBytes[1])),
                            Convert.ToByte(Convert.ToDecimal(stringBytes[2])),
                            Convert.ToByte(Convert.ToDecimal(stringBytes[3]))
                           });

            uint mask = ~(uint.MaxValue >> bits);

            // Convert the IP address to bytes.
            byte[] ipBytes = ip.GetAddressBytes();

            // BitConverter gives bytes in opposite order to GetAddressBytes().
            byte[] maskBytes = BitConverter.GetBytes(mask).Reverse().ToArray();

            byte[] startIPBytes = new byte[ipBytes.Length];
            byte[] endIPBytes = new byte[ipBytes.Length];

            // Calculate the bytes of the start and end IP addresses.
            for (int i = 0; i < ipBytes.Length; i++)
            {
                startIPBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
                endIPBytes[i] = (byte)(ipBytes[i] | ~maskBytes[i]);
            }

            // Convert the bytes to IP addresses.
            IPAddress startIP = new IPAddress(startIPBytes);
            IPAddress endIP = new IPAddress(endIPBytes);

            ips.Add(startIP.ToString());
            ips.Add(endIP.ToString());

            return ips;
        }

        public static NetRange GetNetrange(string ip)
        {
            return API(ip);
        }

        public override string ToString()
        {
            return from + "-" + to + " [" + netname + "]";
        }
    }
}
