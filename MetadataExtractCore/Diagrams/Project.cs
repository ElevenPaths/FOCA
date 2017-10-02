using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MetadataExtractCore.Diagrams
{
    [Serializable]
    public class Project : IDisposable
    {
        public int Id { get; set; }

        public enum ProjectStates { Uninitialized, InitializedUnsave, InitializedSave};

        public string ProjectName { get; set; }

        public string ProjectSaveFile { get; set; }

        public ProjectStates ProjectState { get; set; }

        public string Domain { get; set; }

        public List<string> AlternativeDomains { get; set; }

        public string FolderToDownload { get; set; }

        public DateTime ProjectDate { get; set; }

        public string ProjectNotes { get; set; }

        public ThreadSafeList<NetRange> LstNetRange = new ThreadSafeList<NetRange>();

        public ThreadSafeList<AntiSpoofingPolitics> LstAntispoofingPolitics = new ThreadSafeList<AntiSpoofingPolitics>();


        public int AutoSaveMinuts = 5;

        public Project()
        {
            ProjectState = ProjectStates.Uninitialized;
            AlternativeDomains = new List<string>();
            FolderToDownload = Path.GetTempPath();
        }

        public NetRange GetNetrange(string ip)
        {
            return NetRange.GetNetrange(ip);
        }

        public int GetIpsOfNetrange(NetRange netrange)
        {
            var p1 = int.Parse(netrange.to.Split('.')[0]) - (int.Parse(netrange.from.Split('.')[0]));
            var p2 = int.Parse(netrange.to.Split('.')[1]) - (int.Parse(netrange.from.Split('.')[1]));
            var p3 = int.Parse(netrange.to.Split('.')[2]) - (int.Parse(netrange.from.Split('.')[2]));
            var p4 = int.Parse(netrange.to.Split('.')[3]) - (int.Parse(netrange.from.Split('.')[3]));

            if (p1==0)p1=1;
            if (p2==0)p2=1;
            if (p3==0)p3=1;
            if (p4==0)p4=1;

            return p1*p2*p3*p4;

        }

        public List<string> GenerateIpsOfNetrange(NetRange netrange)
        {
            var lstIps = new List<string>();
            try
            {
                int[] rangeFrom =
                {
                    int.Parse(netrange.from.Split('.')[0]),
                    int.Parse(netrange.from.Split('.')[1]),
                    int.Parse(netrange.from.Split('.')[2]),
                    int.Parse(netrange.from.Split('.')[3])
                };

                int[] rangeTo =
                {
                    int.Parse(netrange.to.Split('.')[0]),
                    int.Parse(netrange.to.Split('.')[1]),
                    int.Parse(netrange.to.Split('.')[2]),
                    int.Parse(netrange.to.Split('.')[3])
                };

                while (
                    (
                        rangeFrom[0] + "." + rangeFrom[1] + "." + rangeFrom[2] + "." + rangeFrom[3])
                    !=
                    (rangeTo[0] + "." + rangeTo[1] + "." + rangeTo[2] + "." + rangeTo[3]))
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

        /// <summary>
        /// Validate if Ip is in NetRange.
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool IsIpInNetrange(string ip)
        {
            for (int i = 0; i < LstNetRange.Count; i++)
            {
                NetRange netrange = LstNetRange[i];
                if (netrange == null)
                    return true;

                if (netrange.IsIpInNetrange(ip))
                    return true;
            }
            return false;
        }

        #region IDisposable Support

        private bool DisposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (DisposedValue) return;

            if (disposing)
            {
                LstAntispoofingPolitics.Clear();
                LstNetRange.Clear();
            }

            LstAntispoofingPolitics = null;
            LstNetRange = null;

            DisposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion


    }
}