using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace FOCA.Search
{
    public class CookieAwareWebClient : WebClient
    {
        public CookieContainer m_container = new CookieContainer();

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).CookieContainer = m_container;
            }
            return request;
        }

        public static void AcceptAllCertificates()
        {
            //Acepta todos los certificados aunque sean invalidos
            ServicePointManager.ServerCertificateValidationCallback +=
                delegate(
                    object
                    senderr,
                    System.Security.Cryptography.X509Certificates.X509Certificate
                    pCertificate,
                    System.Security.Cryptography.X509Certificates.X509Chain pChain,
                    System.Net.Security.SslPolicyErrors pSSLPolicyErrors)
                {
                    return true;
                };
        }
    }
}
