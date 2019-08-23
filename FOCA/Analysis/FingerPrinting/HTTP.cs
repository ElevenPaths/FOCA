using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using FOCA.Database.Entities;

namespace FOCA.Analysis.FingerPrinting
{
    [Serializable]
    public class HTTP : FingerPrinting
    {
        private const string CertificatesFolderName = "certificates";
        private readonly string path = "/";
        private readonly bool ssl;
        public string Title = string.Empty;

        public HTTP()
        {
        }

        public HTTP(string domain, string path, int port, bool ssl) : base(domain, port)
        {
            this.path = path;
            this.ssl = ssl;

        }

        public override event EventHandler FingerPrintingError;
        public override event EventHandler FingerPrintingFinished;

        public override void GetVersion()
        {
            var httpProtocol = ssl ? "https://" : "http://";

            WebResponse response;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(httpProtocol + Host + ":" + Port + "/");
                request.KeepAlive = false;
                request.AllowAutoRedirect = false;
                request.Timeout = 3 * 1000; // 3 seconds
                response = request.GetResponse();

                Program.LogThis(new Log(Log.ModuleType.FingingerPrinting,
                    "HTTP Fingerprinting on " + httpProtocol + Host + ":" + Port, Log.LogType.debug));

                var rs = response.GetResponseStream();
                if (rs == null) return;
                var sr = new StreamReader(rs);
                var respuesta = sr.ReadToEnd();

                var m = Regex.Match(respuesta, @"<Title>\s*(.+?)\s*</Title>", RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    if (m.Value.Length < 200)
                        Title = m.Value;
                }
            }
            catch (WebException ex)
            {
                // it may occur if you use SSL to connect to a port that doesn't support it
                if (ex.Response == null)
                {
                    return;
                }
                response = ex.Response;
            }
            catch (Exception)
            {
                FingerPrintingError?.Invoke(this, null);
                return;
            }

            Version = response.Headers["Server"] ?? "(Unavailable)";
            response.Close();

            // try to guess the OS (sometimes the banner leaks that information)
            AnalyzeBanner(Version);

            // tests using HTTP Errors
            if (os == OperatingSystem.OS.Unknown)
            {
                AnalyzeErrors404();
                AnalyzeErrors403();
                AnalyzeAspx();
            }
            AnalyzeRobots();
            AnalyzeCertificate();

            FingerPrintingFinished?.Invoke(this, null);
        }

        private void AnalyzeBanner(string banner)
        {
            var operatingSystem = GetOsFromBanner(banner);
            try
            {
                var domain = Program.data.GetDomain(Host);
                if (domain == null)
                    return;
                domain.os = operatingSystem;
            }
            catch
            {
            }
        }

        public static OperatingSystem.OS GetOsFromBanner(string str)
        {
            // Windows Family
            if (str.ToLower().Contains("win32"))
                return OperatingSystem.OS.Windows;
            if (str.ToLower().Contains("iis/7"))
                return OperatingSystem.OS.Windows2008;
            if (str.ToLower().Contains("iis/6"))
                return OperatingSystem.OS.Windows2003;
            if (str.ToLower().Contains("iis/5"))
                return OperatingSystem.OS.Windows2000;
            if (str.ToLower().Contains("iis/3"))
                return OperatingSystem.OS.WindowsNT40;
            // MacOS Family
            if (str.ToLower().Contains("webserverx"))
                return OperatingSystem.OS.MacOS;
            if (str.ToLower().Contains("darwin"))
                return OperatingSystem.OS.MacOS;
            if (str.ToLower().Contains("webstar"))
                return OperatingSystem.OS.MacOS;
            if (str.ToLower().Contains("macos"))
                return OperatingSystem.OS.MacOS;
            if (str.ToLower().Contains("mac os"))
                return OperatingSystem.OS.MacOS;
            // BSD Family
            if (str.ToLower().Contains("freebsd"))
                return OperatingSystem.OS.FreeBSD;
            if (str.ToLower().Contains("openbsd"))
                return OperatingSystem.OS.OpenBSD;
            // *nix Family
            if (str.ToLower().Contains("centos"))
                return OperatingSystem.OS.CentOS;
            if (str.ToLower().Contains("solaris"))
                return OperatingSystem.OS.Solaris;
            // Linux Family
            if (str.ToLower().Contains("red hat"))
                return OperatingSystem.OS.LinuxRedHat;
            if (str.ToLower().Contains("redhat"))
                return OperatingSystem.OS.LinuxRedHat;
            if (str.ToLower().Contains("ubuntu"))
                return OperatingSystem.OS.LinuxUbuntu;
            if (str.ToLower().Contains("debian"))
                return OperatingSystem.OS.LinuxDebian;
            if (str.ToLower().Contains("fedora"))
                return OperatingSystem.OS.LinuxFedora;
            if (str.ToLower().Contains("mandrake"))
                return OperatingSystem.OS.LinuxFedora;
            if (str.ToLower().Contains("mandriva"))
                return OperatingSystem.OS.LinuxFedora;
            if (str.ToLower().Contains("suse"))
                return OperatingSystem.OS.LinuxSuse;
            if (str.ToLower().Contains("linux"))
                return OperatingSystem.OS.Linux;
            if (str.ToLower().Contains("unix"))
                return OperatingSystem.OS.Linux;
            if (str.ToLower().Contains("lighttpd"))
                return OperatingSystem.OS.Linux;
            return str.ToLower().Contains("zeus") ? OperatingSystem.OS.Linux : OperatingSystem.OS.Unknown;
        }

        private void AnalyzeRobots()
        {
            try
            {
                HttpMap.HttpMap.CheckRobots(Host, Port);
            }
            catch
            {
                Program.LogThis(new Log(Log.ModuleType.FingingerPrinting, "Couldn't check robots.txt of host " + Host,
                    Log.LogType.debug));
            }
        }

        private void AnalyzeAspx()
        {
            var httpProtocol = ssl ? "https://" : "http://";
            const string file = "/F0C4.aspx";

            WebResponse response = null;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(httpProtocol + Host + ":" + Port + file);
                request.KeepAlive = false;
                request.AllowAutoRedirect = false;
                request.Timeout = 5 * 1000; // 5 seconds
                Program.LogThis(new Log(Log.ModuleType.FingingerPrinting,
                    "HTTP Fingerprinting ASPX on " + httpProtocol + Host + ":" + Port + file, Log.LogType.debug));
                response = request.GetResponse();
                // IIS with .NET framework, so, Windows
                if (response.ResponseUri.ToString().ToLower().Contains("asperrorpath="))
                    os = OperatingSystem.OS.Windows;
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                    return;
                response = ex.Response;
            }
            catch (Exception ex)
            {
                Program.LogThis(new Log(Log.ModuleType.FingingerPrinting,
                    ex.Message + ", " + httpProtocol + Host + ":" + Port + file, Log.LogType.error));
            }
            finally
            {
                response?.Close();
            }
        }

        public void AnalyzeErrors403()
        {
            var httpProtocol = ssl ? "https://" : "http://";
            WebResponse response = null;
            // Try to obtain a 403 error
            var file = "/.htF0C4_" + new Random().Next(0, 999);
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(httpProtocol + Host + ":" + Port + file);
                request.KeepAlive = false;
                request.AllowAutoRedirect = false;
                request.Timeout = 5 * 1000; // 5 seconds
                Program.LogThis(new Log(Log.ModuleType.FingingerPrinting,
                    "HTTP Fingerprinting 404 on " + httpProtocol + Host + ":" + Port + file, Log.LogType.debug));
                response = request.GetResponse();
            }
            catch (WebException ex)
            {
                var rs = ex.Response?.GetResponseStream();
                if (rs == null) return;
                var responseString = (new StreamReader(rs).ReadToEnd());
                AnalyzeHttp404Strings(responseString);
            }
            catch (Exception ex) // error creating the Uri
            {
                Program.LogThis(new Log(source, ex.Message + ", " + httpProtocol + Host + ":" + Port, Log.LogType.error));
            }
            finally
            {
                response?.Close();
            }
        }

        public void AnalyzeErrors404()
        {
            var httpProtocol = ssl ? "https://" : "http://";
            WebResponse response = null;
            var file = "/F0C4_" + new Random().Next(0, 999);
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(httpProtocol + Host + ":" + Port + file);
                request.KeepAlive = false;
                request.AllowAutoRedirect = false;
                request.Timeout = 5 * 1000; // 5 seconds
                Program.LogThis(new Log(Log.ModuleType.FingingerPrinting,
                    "HTTP Fingerprinting 404 on " + httpProtocol + Host + ":" + Port + file, Log.LogType.debug));
                response = request.GetResponse();
            }
            catch (WebException ex)
            {
                var rs = ex.Response?.GetResponseStream();
                if (rs == null) return;
                var responseString = (new StreamReader(rs).ReadToEnd());
                AnalyzeHttp404Strings(responseString);
            }
            catch (Exception ex) // error creating the Uri
            {
                Program.LogThis(new Log(source, ex.Message + ", " + httpProtocol + Host + ":" + Port, Log.LogType.error));
            }
            finally
            {
                response?.Close();
            }
        }

        public void AnalyzeErrorsJsp()
        {
            var httpProtocol = ssl ? "https://" : "http://";
            WebResponse response = null;

            var file = path.Replace(path, path + ".F0C4.jsp");

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(httpProtocol + Host + ":" + Port + file);
                request.KeepAlive = false;
                request.AllowAutoRedirect = false;
                request.Timeout = 8 * 1000; // 8 seconds
                Program.LogThis(new Log(Log.ModuleType.FingingerPrinting,
                    "HTTP Fingerprinting JSP error on " + httpProtocol + Host + ":" + Port + file, Log.LogType.debug));
                response = request.GetResponse();
            }
            catch (WebException ex)
            {
                var rs = ex.Response?.GetResponseStream();
                if (rs == null) return;
                var responseString = (new StreamReader(rs).ReadToEnd());
                AnalyzeHttp404Strings(responseString);
                foreach (var cdi in Program.data.computerDomains.Items.Where(cdi => cdi.Domain.Domain == Host))
                {
                    string[] expresions =
                    {
                        @"(Apache Tomcat)(/[^\s<]+)",
                        @"(JBoss Web)(/[^\s<]+)",
                        @"(SpringSource)(/[^\s<]+)",
                        @"(Sun-Java-System)(/[^\s<]+)"
                    };

                    foreach (
                        var c in
                            expresions.Select(s => Regex.Match(responseString, s, RegexOptions.IgnoreCase))
                                .Where(c => c.Success))
                    {
                        cdi.Computer.Software.AddUniqueItem(new ApplicationsItem(c.Value,
                            "jsp fingerprinting (" + httpProtocol + Host + ":" + Port + file + ")"));
                    }

                    if (responseString.ToLower().Contains("<H1>404 Not Found</H1>OracleJSP: ".ToLower()))
                        cdi.Computer.Software.AddUniqueItem(new ApplicationsItem("Oracle",
                            "jsp fingerprinting (" + httpProtocol + Host + ":" + Port + file + ")"));
                    if (responseString.ToLower().Contains("at com.ibm.".ToLower()))
                        cdi.Computer.Software.AddUniqueItem(new ApplicationsItem("IBM Web Sphere",
                            "jsp fingerprinting (" + httpProtocol + Host + ":" + Port + file + ")"));
                    if (
                        responseString.ToLower()
                            .Contains(
                                "The server has not found anything matching the Request-URI. No indication is given of whether the condition is temporary or permanent.</p><p>If the server does not wish to make this information available to the client, the status code 403 (Forbidden) can be used instead. The 410 (Gone) status code SHOULD be used if the server knows, through some internally configurable mechanism, that an old resource is permanently unavailable and has no forwarding address."
                                    .ToLower()))
                        cdi.Computer.Software.AddUniqueItem(new ApplicationsItem("WebLogic",
                            "jsp fingerprinting (" + httpProtocol + Host + ":" + Port + file + ")"));
                }
            }
            catch (Exception ex) // error creating the Uri
            {
                Program.LogThis(new Log(source, ex.Message + ", " + httpProtocol + Host + ":" + Port, Log.LogType.error));
            }
            finally
            {
                response?.Close();
            }
        }

        public void AnalyzeErrorsTpl()
        {
            var httpProtocol = ssl ? "https://" : "http://";
            WebResponse response = null;

            var file = path.Replace(path, path + ".F0C4.tpl");

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(httpProtocol + Host + ":" + Port + file);
                request.KeepAlive = false;
                request.AllowAutoRedirect = false;
                request.Timeout = 8 * 1000; // 8 seconds
                Program.LogThis(new Log(Log.ModuleType.FingingerPrinting,
                    "HTTP Fingerprinting TPL error on " + httpProtocol + Host + ":" + Port + file, Log.LogType.debug));
                response = request.GetResponse();

                var rs = response.GetResponseStream();
                if (rs == null) return;
                var responseString = (new StreamReader(rs).ReadToEnd());

                foreach (
                    var cdi in
                        Program.data.computerDomains.Items.Where(cdi => cdi.Domain.Domain == Host)
                            .Where(cdi => responseString.StartsWith("Error: template '")))
                {
                    try
                    {
                        var folder = responseString.Split('\'')[1];
                        folder = folder.Replace('/', '\\');
                        cdi.Computer.RemoteFolders.AddUniqueItem(folder, true);
                    }
                    catch
                    {
                    }
                }
            }
            catch (WebException)
            {
            }
            catch (Exception ex) // error creating the Uri
            {
                Program.LogThis(new Log(source, ex.Message + ", " + httpProtocol + Host + ":" + Port, Log.LogType.error));
            }
            finally
            {
                response?.Close();
            }
        }

        private void AnalyzeHttp404Strings(string str)
        {
            //<address>Apache/2.0.63 (FreeBSD) PHP/5.2.6 mod_ssl/2.0.63 OpenSSL/0.9.8g Server at mail.server.com Port 443</address>
            const string regularExpresion = @"(Server at )([^\s]+)( Port \d{1,})";

            var c = Regex.Match(str, regularExpresion, RegexOptions.IgnoreCase);
            if (!c.Success) return;
            var newName = c.Value.Split(' ')[2];

            // check if the name is the same as the one from the request
            if (newName == Host) return;
            try
            {
                var ipHostEntry = Dns.GetHostEntry(newName);

                if (ipHostEntry == null) return;
                // check if the domain already exists
                var dom = Program.data.GetDomain(newName);
                if (dom == null)
                {
                    Program.data.AddDomain(newName, "FingerPrinting", 1, Program.cfgCurrent);
                }
            }
            catch
            {
                try
                {
                    var dom = Program.data.GetDomain(Host);

                    foreach (
                        var cdi in
                            Program.data.computerDomains.Items.Where(cdi => cdi.Domain.Domain == dom.Domain)
                                .Where(cdi => string.IsNullOrEmpty(cdi.Computer.localName)))
                    {
                        cdi.Computer.localName = newName;
                    }
                }
                catch
                {
                }
            }
        }

        private void AnalyzeCertificate()
        {
            try
            {
                HttpWebRequest request = HttpWebRequest.CreateHttp("https://" + Host + ":" + "443" + "/");
                request.KeepAlive = false;
                request.AllowAutoRedirect = false;
                request.Timeout = 5 * 1000; // 5 seconds
                request.ServerCertificateValidationCallback += ExtractServerCertificateInformation;
                Program.LogThis(new Log(Log.ModuleType.FingingerPrinting,
                    "HTTP Fingerprinting on " + "https://" + Host + ":" + "443", Log.LogType.debug));
                var response = request.GetResponse();
                response.Dispose();
            }
            catch (WebException)
            {
            }
            catch (Exception)
            {
                FingerPrintingError?.Invoke(this, null);
            }
        }

        private static bool ExtractServerCertificateInformation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            HttpWebRequest sourceRequest = sender as HttpWebRequest;

            if (certificate == null || sourceRequest == null)
                return true;

            int startCn = certificate.Subject.IndexOf("CN=", StringComparison.OrdinalIgnoreCase) + "CN=".Length;
            int offset = certificate.Subject.Length - startCn;

            string aux_certName = certificate.Subject.Substring(startCn, offset);
            if (aux_certName.Contains(","))
                aux_certName = aux_certName.Split(',')[0];

            if (!aux_certName.Contains("*"))
            {
                if (aux_certName.EndsWith("." + Program.data.Project.Domain))
                {
                    Program.data.AddDomain(aux_certName, "Certificate FingerPrinting", 1, Program.cfgCurrent);
                }
                else if (Program.data.Project.AlternativeDomains.FirstOrDefault(p => p == aux_certName) == null)
                {
                    Program.data.Project.AlternativeDomains.Add(aux_certName);
                }
            }

            string certificatesFullPath = System.IO.Path.Combine(Program.data.Project.FolderToDownload, CertificatesFolderName);
            if (!Directory.Exists(certificatesFullPath))
            {
                try
                {
                    Directory.CreateDirectory(certificatesFullPath);
                }
                catch
                {
                    return true;
                }
            }

            FileStream fs = null;
            try
            {
                fs = new FileStream(System.IO.Path.Combine(certificatesFullPath, sourceRequest.Address.Host + "_" + aux_certName.Replace('*', '%') + ".crt"),
                    FileMode.Create);
                fs.Write(certificate.GetRawCertData(), 0, certificate.GetRawCertData().Length);
            }
            catch
            {
                Program.LogThis(new Log(Log.ModuleType.FingingerPrinting, "Couldn't get certificate of host " + sourceRequest.Address.Host,
                    Log.LogType.debug));
            }
            finally
            {
                fs?.Close();
            }

            return true;
        }
    }
}