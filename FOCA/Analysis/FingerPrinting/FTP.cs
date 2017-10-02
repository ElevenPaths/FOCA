using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net.Sockets;
using System.IO;

namespace FOCA.Analysis.FingerPrinting
{
    [Serializable]
    public class FTP : FingerPrinting
    {
        public override event EventHandler FingerPrintingFinished;  // salta cuando se finaliza la conexion y el analisis
        public override event EventHandler FingerPrintingError;     // salta cuando se produce un error en la conexion

        [NonSerialized]
        private NetworkStream ns;
        byte[] buffer;

        public FTP()
        {
        }

        public FTP(string host, int port) : base(host, port) { }

        public override void GetVersion()
        {
            try
            {
                base.tcp.SendTimeout = 2000;
                base.tcp.ReceiveTimeout = 2000;
                if (!base.TcpConnect())
                    throw new Exception();

                System.Threading.Thread.Sleep(5000); // Algunos servidores ftp tardan unos segundos en mostrar el banner
                ns = base.TcpStream();

                if (ns == null)
                    return;

            }
            catch (Exception)
            {
                FingerPrintingError?.Invoke(this, null);
                return;
            }

            StringBuilder sb = new StringBuilder();

            int size = 1024;
            buffer = new byte[size];
            int bytesLeidos = 0;
            bool endOfStream = false;

            try
            {
                while (bytesLeidos >= 0 && (endOfStream == false))
                {
                    bytesLeidos = ns.Read(buffer, 0, size);
                    sb.Append(ASCIIEncoding.ASCII.GetString(buffer, 0, bytesLeidos));
                    endOfStream = !ns.DataAvailable;
                }
                Version = sb.ToString();
                Program.LogThis(new Log(Log.ModuleType.FingingerPrinting, "FTP server found on " + base.Host + ":" + base.Port, Log.LogType.medium));
            }
            catch
            {
                Version = "(Error)";
            }
            finally
            {
                base.TcpDisconnect();

                FingerPrintingFinished?.Invoke(this, null);
            }
        }
    }
}
