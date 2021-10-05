using System;
using System.Net.Sockets;
using System.Xml.Serialization;

namespace FOCA.Analysis.FingerPrinting
{
    [XmlInclude(typeof(HTTP))]
    [XmlInclude(typeof(SMTP))]
    [XmlInclude(typeof(FTP))]
    [XmlInclude(typeof(DNS))]
    [Serializable]
    public abstract class FingerPrinting : IDisposable
    {
        public abstract event EventHandler FingerPrintingFinished;
        public abstract event EventHandler FingerPrintingError;

        protected Log.ModuleType source = Log.ModuleType.FingingerPrinting;
        public int Id { get; set; }
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Version { get; set; } = string.Empty;
        public FOCA.OperatingSystem.OS os = OperatingSystem.OS.Unknown;

        [NonSerialized]
        protected TcpClient tcp = new TcpClient();
        [NonSerialized]
        private NetworkStream ns;

        public int TimeOutConnectionMS = 2000;

        public FingerPrinting() { }

        public FingerPrinting(string host, int port)
        {
            this.Host = host;
            this.Port = port;
        }

        public abstract void GetVersion();

        protected bool TcpConnect()
        {
            tcp.Connect(this.Host, this.Port);
            if (tcp.Connected)
                ns = tcp.GetStream();
            return tcp.Connected;
        }

        protected void TcpDisconnect()
        {
            tcp.Close();
        }

        protected NetworkStream TcpStream()
        {
            return ns;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    tcp.Close();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}