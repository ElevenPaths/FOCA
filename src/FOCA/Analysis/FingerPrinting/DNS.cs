using System;
using System.Linq;

namespace FOCA.Analysis.FingerPrinting
{
    [Serializable]
    public class DNS : FingerPrinting
    {

        public override event EventHandler FingerPrintingFinished;  // salta cuando se finaliza la conexion y el analisis
        public override event EventHandler FingerPrintingError;     // salta cuando se produce un error en la conexion

        public DNS()
        {
        }

        public DNS(string host) : base(host, 53) { }

        public override void GetVersion()
        {
            try
            {
                System.Net.IPAddress[] ips = System.Net.Dns.GetHostAddresses(Host);

                if (ips.Length == 0)
                    return;

                Heijden.DNS.Resolver r = new Heijden.DNS.Resolver(ips[0], base.Port);
                r.TimeOut = 10;
                Heijden.DNS.Response response = r.Query("version.bind", Heijden.DNS.QType.TXT, Heijden.DNS.QClass.CH);
                if (response.RecordsTXT.Length > 0)
                {
                    OperatingSystem.OS os = OperatingSystem.OS.Unknown;
                    Version = response.RecordsTXT.SelectMany(p => p.TXT.Where(q => !String.IsNullOrEmpty(q)))
                        .FirstOrDefault(p =>
                            {
                                os = AnalyzeBanner(p);
                                return os != OperatingSystem.OS.Unknown;
                            }
                        );

                    this.os = os;
                }
                if (this.FingerPrintingFinished != null)
                    FingerPrintingFinished(this, null);
            }
            catch
            {
                if (FingerPrintingError != null)
                    FingerPrintingError(this, null);
            }
        }

        private OperatingSystem.OS AnalyzeBanner(string banner)
        {
            if (this.os != OperatingSystem.OS.Unknown)
                return this.os; // Si ya tiene OS no se re-analiza...

            // Familia Windows
            if (banner.ToLower().Contains("win32"))
                return OperatingSystem.OS.Windows;
            else if (banner.ToLower().Contains("macos"))
                return OperatingSystem.OS.MacOS;
            else if (banner.ToLower().Contains("mac os"))
                return OperatingSystem.OS.MacOS;
            // Familia BSD
            else if (banner.ToLower().Contains("freebsd"))
                return OperatingSystem.OS.FreeBSD;
            else if (banner.ToLower().Contains("openbsd"))
                return OperatingSystem.OS.OpenBSD;
            // Familia *nix
            else if (banner.ToLower().Contains("centos"))
                return OperatingSystem.OS.CentOS;
            else if (banner.ToLower().Contains("solaris"))
                return OperatingSystem.OS.Solaris;
            // Familia Linux
            else if (banner.ToLower().Contains("red hat"))
                return OperatingSystem.OS.LinuxRedHat;
            else if (banner.ToLower().Contains("redhat"))
                return OperatingSystem.OS.LinuxRedHat;
            else if (banner.ToLower().Contains("ubuntu"))
                return OperatingSystem.OS.LinuxUbuntu;
            else if (banner.ToLower().Contains("debian"))
                return OperatingSystem.OS.LinuxDebian;
            else if (banner.ToLower().Contains("fedora"))
                return OperatingSystem.OS.LinuxFedora;
            else if (banner.ToLower().Contains("mandrake"))
                return OperatingSystem.OS.LinuxFedora;
            else if (banner.ToLower().Contains("mandriva"))
                return OperatingSystem.OS.LinuxFedora;
            else if (banner.ToLower().Contains("suse"))
                return OperatingSystem.OS.LinuxSuse;
            else if (banner.ToLower().Contains("linux"))
                return OperatingSystem.OS.Linux;
            else if (banner.ToLower().Contains("unix"))
                return OperatingSystem.OS.Linux;
            else
                return OperatingSystem.OS.Unknown;
        }

    }
}
