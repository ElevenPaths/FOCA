using FOCA.Database.Entities;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace FOCA.Searcher
{
    //Es una clase que se mantendrá siempre en ejecución en un hilo diferente al principal
    //Su cometido será el de recibir urls de las que tendrá que obtener, cuando pueda, su tamaño mediante el método HEAD
    public class HTTPSizeDaemon
    {
        private ConcurrentQueue<FilesItem> filesToReadSizeQueue;
        private readonly Thread thrSizeSearcher;
        private CancellationTokenSource threadToken;

        public ThreadState ThreadState
        {
            get
            {
                return thrSizeSearcher.ThreadState;
            }
        }

        public HTTPSizeDaemon()
        {
            this.filesToReadSizeQueue = new ConcurrentQueue<FilesItem>();
            this.threadToken = new CancellationTokenSource();
            thrSizeSearcher = new Thread(Work) { IsBackground = true };
            thrSizeSearcher.Start();
        }

        public void AddURL(FilesItem metadataFile)
        {
            if (Program.cfgCurrent.UseHead)
            {
                this.filesToReadSizeQueue.Enqueue(metadataFile);
            }
        }

        public void Work()
        {
            do
            {
                try
                {
                    if (this.filesToReadSizeQueue.Count > 0 && this.filesToReadSizeQueue.TryDequeue(out FilesItem currentItem))
                    {
                        long lSize = GetURLContentLength(currentItem.URL);
                        if (lSize >= 0)
                        {
                            Program.FormMainInstance.panelMetadataSearch.listViewDocuments.Invoke(
                                new MethodInvoker(delegate
                                {
                                    currentItem.Size = (int)lSize;
                                    Program.FormMainInstance.panelMetadataSearch.listViewDocuments_Update(currentItem);
                                }));
                        }
                    }
                    else
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(5));
                    }
                }
                catch { }
            }
            while (!this.threadToken.IsCancellationRequested);
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
                wr.Timeout = 3000;
                using (WebResponse wrp = wr.GetResponse())
                {
                    return wrp.ContentLength;
                }
            }
            catch
            {
                return -1;
            }
        }

        public void Abort()
        {
            this.threadToken.Cancel();
            this.thrSizeSearcher.Join();
        }
    }
}
