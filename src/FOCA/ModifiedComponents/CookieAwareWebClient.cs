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
    }
}
