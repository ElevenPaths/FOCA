using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace FOCA.Analysis.FingerPrinting
{
    [Serializable]
    public class SMTP : FingerPrinting
    {
        public override event EventHandler FingerPrintingFinished;  // salta cuando se finaliza la conexion y el analisis
        public override event EventHandler FingerPrintingError;     // salta cuando se produce un error en la conexion

        [NonSerialized]
        private NetworkStream ns;
        byte[] buffer;

        public SMTP() { }

        public SMTP(string host, int port) : base(host, port) { }

        public override void GetVersion()
        {

            Program.LogThis(new Log(Log.ModuleType.FingingerPrinting, "SMTP Fingerprinting on " + base.Host + ":" + base.Port, Log.LogType.debug));

            try
            {
                base.tcp.SendTimeout = 2000;
                base.tcp.ReceiveTimeout = 2000;

                if (!base.TcpConnect())
                    throw new Exception();

                ns = base.TcpStream();
                if (ns == null)
                    return;
            }
            catch (Exception)
            {
                if (FingerPrintingError != null)
                    FingerPrintingError(this, null);
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
                Program.LogThis(new Log(Log.ModuleType.FingingerPrinting, "SMTP server found on " + base.Host + ":" + base.Port, Log.LogType.medium));
            }
            catch
            {
                Version = "(Error)";
            }
            finally
            {
                base.TcpDisconnect();
            }

            AnalyzePosibleOS();
            FingerPrintingFinished?.Invoke(this, null);
        }

        private void AnalyzePosibleOS()
        {
            if (base.Version.ToLower().Contains("microsoft"))
                os = OperatingSystem.OS.Windows;
            else if (base.Version.ToLower().Contains("ubuntu"))
                os = OperatingSystem.OS.LinuxUbuntu;
            else if (base.Version.ToLower().Contains("debian"))
                os = OperatingSystem.OS.LinuxDebian;
            else if (base.Version.ToLower().Contains("fedora"))
                os = OperatingSystem.OS.LinuxFedora;
            else if (base.Version.ToLower().Contains("mandrake"))
                os = OperatingSystem.OS.LinuxMandrake;
            else if (base.Version.ToLower().Contains("mandriva"))
                os = OperatingSystem.OS.LinuxMandrake;
            else if (base.Version.ToLower().Contains("redhat"))
                os = OperatingSystem.OS.LinuxRedHat;
            else if (base.Version.ToLower().Contains("suse"))
                os = OperatingSystem.OS.LinuxSuse;
            else if (base.Version.ToLower().Contains("freebsd"))
                os = OperatingSystem.OS.FreeBSD;
            else if (base.Version.ToLower().Contains("openbsd"))
                os = OperatingSystem.OS.OpenBSD;
            else if (base.Version.ToLower().Contains("solaris"))
                os = OperatingSystem.OS.Solaris;
            else if (base.Version.ToLower().Contains("centos"))
                os = OperatingSystem.OS.CentOS;
            else if (base.Version.ToLower().Contains("macos"))
                os = OperatingSystem.OS.MacOS;
            else if (base.Version.ToLower().Contains("postfix"))
                os = OperatingSystem.OS.Linux;
            else if (base.Version.ToLower().Contains("linux"))
                os = OperatingSystem.OS.Linux;
        }
    }
}
