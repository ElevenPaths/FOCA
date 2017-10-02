using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace FOCA.Searcher
{
    //Es una clase que se mantendrá siempre en ejecución en un hilo diferente al principal
    //Su cometido será el de recibir urls de las que tendrá que obtener, cuando pueda, su tamaño mediante el método HEAD
    public class HTTPSizeDaemon
    {
        public ThreadState ThreadState
        {
            get
            {
                if (thrSizeSearcher != null)
                    return thrSizeSearcher.ThreadState;
                return ThreadState.Stopped;
            }
        }

        private List<HTTPSizeElement> LstURLs
        {
            get
            {
                return lstUrLs;
            }

            set
            {
                lstUrLs = value;
            }
        }

        private struct HTTPSizeElement{
            public string strURL;
            public System.Windows.Forms.ListViewItem lvi;
        };

        private readonly Thread thrSizeSearcher;
        private List<HTTPSizeElement> lstUrLs = new List<HTTPSizeElement>();

        public HTTPSizeDaemon()
        {
            thrSizeSearcher = new Thread(Work) {IsBackground = true};
            thrSizeSearcher.Start();
        }

        public void AddURL(string strURL, System.Windows.Forms.ListViewItem lvi)
        {
            HTTPSizeElement hse = new HTTPSizeElement();
            hse.strURL = strURL;
            hse.lvi = lvi;
            LstURLs.Add(hse);
        }

        public void Work()
        {
            try
            {
                if (!Program.cfgCurrent.UseHead || LstURLs.Count == 0 || !Program.FormMainInstance.panelMetadataSearch.listViewDocuments.Items.Contains(LstURLs[0].lvi)) return;
                var lSize = GetURLContentLength(LstURLs[0].strURL);
                if (lSize >= 0)
                {
                    Program.FormMainInstance.panelMetadataSearch.listViewDocuments.Invoke(
                        new MethodInvoker(delegate
                        {
                            FilesITem fi = (FilesITem) LstURLs[0].lvi.Tag;
                            fi.Size = (int) lSize;
                            LstURLs[0].lvi.SubItems[5].Text = Functions.GetFileSizeAsString(lSize);
                        }));
                }
                LstURLs.RemoveAt(0);
            }
            catch { }
        }

        /// <summary>
        /// Get the content size of a URL using the HEAD method
        /// </summary>
        /// <param name="strURL"></param>
        /// <returns></returns>
        public static long GetURLContentLength(string strURL)
        {
            try
            {
                HttpWebRequest wr = (HttpWebRequest)HttpWebRequest.Create(strURL);
                wr.Method = "HEAD";
                wr.KeepAlive = false;
                System.Net.WebResponse wrp = wr.GetResponse();
                wrp.Close();
                return wrp.ContentLength;
            }
            catch
            {
                return -1;
            }
        }

        public void Abort()
        {
            if (thrSizeSearcher != null)
                thrSizeSearcher.Abort();
        }
    }
}
