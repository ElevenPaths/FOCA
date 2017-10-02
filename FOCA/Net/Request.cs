using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace FOCA.Net
{
    [Serializable]
    public class Request
    {
        private string aux_certName = string.Empty;
        public bool followRedirects = false;

        public Request()
        {
        }

        public string DoGet(string url, out int responseCode)
        {
            responseCode = 200;
            string responseString = string.Empty;
            HttpStatusCode httpStatus = new HttpStatusCode();

            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
                                    new RemoteCertificateValidationCallback(VerifyServerCertificate);

                ASCIIEncoding encoding = new ASCIIEncoding();
                HttpWebResponse response;

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.AllowAutoRedirect = followRedirects;
                request.Proxy = null;
                response = (HttpWebResponse)request.GetResponse();
                using (var reader = new StreamReader(response.GetResponseStream())) {
                    responseString = reader.ReadToEnd();
                }
                httpStatus = response.StatusCode;
                responseCode = (int)httpStatus;

                response.Close();
                response.GetResponseStream().Close();
                response.GetResponseStream().Dispose();

                response = null;
                request = null;
                encoding = null;
            }
            catch (WebException ex)
            {
                responseCode = (int)ex.Status;

                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    responseCode = (int)(((HttpWebResponse)ex.Response).StatusCode);
                }
                else
                {
                    responseCode = 0;
                }
            }
            catch (Exception)
            {
                responseCode = 0;
            }

            return responseString;
        }

            private bool VerifyServerCertificate(
         object sender, X509Certificate certificate,
         X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                if (certificate == null)
                    return true;

            int inicioCN = certificate.Subject.IndexOf("CN=") + "CN=".Length;
            int offset = certificate.Subject.Length - inicioCN;

            string recorte = certificate.Subject.Substring(inicioCN, offset);
            if (recorte.Contains(","))
                recorte = recorte.Split(new char[] { ',' })[0];
            aux_certName = recorte;

            string folder = "/certificates/";
            if (!Directory.Exists(Program.data.Project.FolderToDownload + "\\" + folder))
            {
                try
                {
                    Directory.CreateDirectory(Program.data.Project.FolderToDownload + "\\" + folder);
                }
                catch
                {
                    return true;
                }
            }
            System.IO.FileStream fs = null;
            try
            {
                HttpWebRequest hWr = (HttpWebRequest)sender;
                fs = new FileStream(Program.data.Project.FolderToDownload + "\\" + folder + "\\" + hWr.Address.Host + "_" + aux_certName + ".crt", FileMode.Create);
                fs.Write(certificate.GetRawCertData(), 0, certificate.GetRawCertData().Length);
            }
            catch
            {
            }
            finally
            {
                try
                {
                    fs.Close();
                }
                catch
                { };
            }

            return true;
        }
    }
}
