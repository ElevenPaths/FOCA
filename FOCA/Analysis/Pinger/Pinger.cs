using FOCA.Utilites;
using System.Net;
using System.Net.NetworkInformation;

namespace FOCA.Analysis.Pinger
{
    static class Pinger
    {
        public static bool IsIPAlive(string ip)
        {
            if (Functions.IsIP(ip))
            {
                IPAddress ipa;
                if (IPAddress.TryParse(ip, out ipa))
                    return IsIPAlive(ipa);
            }
            return false;
        }

        public static bool IsIPAlive(IPAddress ip)
        {
            //Ignora las Ips privadas
            if (DNSUtil.IsPrivateIP(ip))
                return false;
            return PingIt(ip);
        }

        private static bool PingIt(IPAddress ip)
        {
            try
            {
                Ping pingSender = new Ping();
                //Usamos el timeout, y las opciones por defecto
                PingReply reply = pingSender.Send(ip);
                return reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
        }
    }
}
