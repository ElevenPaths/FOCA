using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FOCA.Analysis.FingerPrinting
{
    public static class BannerAnalysis
    {
        /// <summary>
        /// Expresiones regulares de todo el software reconocido en los banners, ordenadas alfabeticamente para poder localizarlas mejor
        /// </summary>
        private static string[] SoftwareRecognized = 
        { 
        //La mayoría sacados de: http://www.computec.ch/projekte/httprecon/?s=database&t=get_existing&f=banner
        @"(4d_webstar_s)(/[^\s]+)?",
        @"(abyss)(/[^\s]+)?",
        @"(aolserver)(/[^\s]+)?",
        @"(authmysql)(/[^\s]+)?",
        @"(apache)(/[^\s]+\s\(.*\))?",
        @"(badblue)(/[^\s]+)?",
        @"(ben-ssl)(/[^\s]+)?",
        @"(boa)(/[^\s]+)?",
        @"(caudium)(/[^\s]+)?",
        @"(cherokee)(/[^\s]+)?",
        @"(compaqhttpserver)(/[^\s]+)?",
        @"(covalent_auth)(/[^\s]+)?",
        @"(covalentssl)(/[^\s]+)?",
        @"(dav)(/[^\s]+)?",
        @"(exim)(\ [^\s]+)?",
        @"(filemakerpro)(/[^\s]+)?",
        @"(frontpage)(/[^\s]+)?",
        @"(iii\s100)",
        @"(jrun)(/[^\s]+)?",
        @"(lasso)(/[^\s]+)?",
        @"(lighttpd)(/[^\s]+)?",
        @"(microsoft-httpapi)(/[^\s]+)?",
        @"(microsoft-iis)(/[^\s]+)?",
        @"(Jetty://)",
        @"(mark)(/[^\s]+)?",
        @"(mod_auth_passthrough)(/[^\s]+)?",
        @"(mod_auth_pgsql)(/[^\s]+)?",
        @"(mod_auth_sspi)(/[^\s]+)?",
        @"(mod_auth_tkt)(/[^\s]+)?",
        @"(mod_autoindex_color)(/[^\s]+)?",
        @"(mod_bwlimited)(/[^\s]+)?",
        @"(mod_chroot)(/[^\s]+)?",
        @"(mod_fastcgi)(/[^\s]+)?",
        @"(mod_gzip)(/[^\s]+)?",
        @"(mod_jk)(/[^\s]+)?", 
        @"(mod_layout)(/[^\s]+)?",
        @"(mod_log_bytes)(/[^\s]+)?",
        @"(mod_perl)(/[^\s]+)?",
        @"(mod_psoft_traffic)(/[^\s]+)?",
        @"(mod_python)(/[^\s]+)?",
        @"(mod_ruby)(/[^\s]+)?",
        @"(mod_ssl)(/[^\s]+)?",
        @"(mod_throttle)(/[^\s]+)?",
        @"(mod_watch)(/[^\s]+)?",
        @"(mod_wsgi)(/[^\s]+)?",
        @"(nginx)(/[^\s]+)?",
        @"(openpkg)(/[^\s]+)?",
        @"(openssl)(/[^\s]+)?",
        @"(oracle-application-server-)([^\s]+)?",
        //@"(oracle-application-server-10g)(/[^\s]+)?",
        @"(ow)(/[^\s]+)?",
        @"(perl)(/[^\s]+)?",
        @"(php)(/[^\s]+)?",
        @"(php-cgi)(/[^\s]+)?",
        @"(phusion_passenger)(/[^\s]+)?",
        @"(plone)(/[^\s]+)?",
        @"(proxy_html)(/[^\s]+)?",
        @"(python)(/[^\s]+)?",
        @"(rompager)(/[^\s]+)?",
        @"(rsa)(/[^\s]+)?",
        @"(ruby)(/[^\s]+)?",
        @"(sidex)(/[^\s]+)?",
        @"(squid)(/[^\s]+)?",
        @"(suhosin-patch)",
        @"(sun-java-system)(/[^\s]+)?",
        @"(svn)(/[^\s]+)?",
        @"(virata-emweb)(/[^\s]+)?",
        @"(webcompanion)(/[^\s]+)?",
        @"(webstar)(/[^\s]+)?",
        @"(zope)(/\(.*\))?",
        @"(zserver)(/[^\s]+)?"
        };

        //Dado un banner obtiene el software que identifica
        public static List<string> GetSoftwareFromBanner(string banner)
        {
            List<string> software = new List<string>();
            foreach (string s in SoftwareRecognized)
            {
                Match c = Regex.Match(banner, s, RegexOptions.IgnoreCase);
                if (c.Success)
                    software.Add(c.Value);
            }
            return software;
        }
    }
}
