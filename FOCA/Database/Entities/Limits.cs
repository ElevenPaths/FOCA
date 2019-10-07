using System;
using System.Linq;

namespace FOCA.Database.Entities
{
    [Serializable]
    public class Limits : BaseItem
    {
        public int Id { get; set; }

        public string Range { get; set; }

        public int Lower { get; set; }

        public int Higher { get; set; }

        public Limits() : base()
        {
        }

        public Limits(string ip) : base()
        {
            string r = ip.Split(new char[] { '.' })[0] + "." +
                       ip.Split(new char[] { '.' })[1] + "." +
                       ip.Split(new char[] { '.' })[2];

            int lastOct = int.Parse(ip.Split(new char[] { '.' })[3]);

            this.Range = r;
            this.Higher = lastOct;
            this.Lower = lastOct;
        }

        public bool IsInRangeLimit(string ip)
        {
            if (DNSUtil.IsIPv4(ip))
            {
                string r = ip.Split(new char[] { '.' })[0] + "." +
                           ip.Split(new char[] { '.' })[1] + "." +
                           ip.Split(new char[] { '.' })[2];

                int lastOct = int.Parse(ip.Split(new char[] { '.' })[3]);

                /* 
                     Se comprueba si hay alguna IP en el limite Higher +1 y en el limite Lower -1.
                     En caso de que exista, y esta no tenga asociado ningun dominio, se modifica el limite
                     Lower a Lower-1 y/o el Higher a Higher+1.
                     */
                {
                    if ((lastOct == Lower - 1) && (lastOct > 0))
                    {
                        string ipLimitInferior = ip.Split(new char[] { '.' })[0] + "." +
                                                 ip.Split(new char[] { '.' })[1] + "." +
                                                 ip.Split(new char[] { '.' })[2] + "." +
                                                 (lastOct - 1).ToString();

                        int count = Program.data.GetRelationsOfIP(ipLimitInferior).Count;
                        if (count == 0)
                            this.Lower--;
                    }

                    if ((lastOct == Higher + 1) && (lastOct < 254))
                    {
                        string ipLimitSuperior = ip.Split(new char[] { '.' })[0] + "." +
                                                 ip.Split(new char[] { '.' })[1] + "." +
                                                 ip.Split(new char[] { '.' })[2] + "." +
                                                 (lastOct + 1).ToString();

                        int count = Program.data.GetRelationsOfIP(ipLimitSuperior).Count;
                        if (count == 0)
                            this.Higher++;
                    }
                }

                if (r == this.Range)
                {
                    if ((lastOct >= Lower) && (lastOct <= Higher))
                        return true;
                }
            }

            return false;
        }
    }
}