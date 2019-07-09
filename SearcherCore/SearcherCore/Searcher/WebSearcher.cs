using FOCA.Threads;
using System;
using System.Collections.Generic;
using System.Threading;

namespace FOCA.Searcher
{
    public abstract class WebSearcher
    {

        protected Thread thrSearchLinks;

        protected bool bSearchAll;
        protected string strSite;
        protected List<string> Extensions;


        /// <summary>
        /// Event raised when a start the search
        /// </summary>
        public event EventHandler SearcherStartEvent;

        /// <summary>
        /// Event raised when a new link or group of links is found. Be carefull! The searcher may return repeat links.
        /// </summary>
        public event EventHandler<EventsThreads.CollectionFound<Uri>> SearcherLinkFoundEvent;

        /// <summary>
        /// Event raised when a new link or group of links is found
        /// </summary>
        public event EventHandler<EventsThreads.ThreadEndEventArgs> SearcherEndEvent;

        /// <summary>
        /// Event raised when the state of the class change, new search, error, end of search, etc.
        /// </summary>
        public event EventHandler<EventsThreads.ThreadStringEventArgs> SearcherChangeStateEvent;

        /// <summary>
        /// Event raised when there is any important to log
        /// </summary>
        public event EventHandler<EventsThreads.ThreadStringEventArgs> SearcherLogEvent;

        public string Name
        {
            get;
        }

        /// <summary>
        /// Site or domain used in the searcher to find links. e.g. in google (site:domain.com)
        /// </summary>
        public string Site
        {
            get { return strSite; }
            set { strSite = value; }
        }

        public bool SearchAll
        {
            get { return bSearchAll; }
            set { bSearchAll = value; }
        }

        public string UserAgent { get; set; }

        /// <summary>
        /// Add a extension to use in the web searcher. e.g. in google (filetype:pdf)
        /// </summary>
        /// <param name="extension">Extension to add, without point</param>
        public void AddExtension(string extension)
        {
            Extensions.Add(extension);
        }

        public WebSearcher(string searcherName)
        {
            this.Name = searcherName;
            Extensions = new List<string>();
        }

        /// <summary>
        /// Destrcutor, isn't necesary?
        /// </summary>
        ~WebSearcher()
        {
            Dispose();
        }

        public void Dispose()
        {
            GC.Collect();
        }

        /// <summary>
        /// Procedure to Abort the search, it'll abort thread used by the class
        /// </summary>
        public void Abort()
        {
            if (thrSearchLinks != null && thrSearchLinks.IsAlive)
                thrSearchLinks.Abort();
        }

        /// <summary>
        /// Join to the thread to wait until finished
        /// </summary>
        public void Join()
        {
            thrSearchLinks.Join();
        }

        /// <summary>
        /// Procedure to search links in a web searcher, each searcher (google, live search) use diferent code
        /// </summary>
        public abstract void GetLinks();

        /// <summary>
        /// Procedure to search links in a web searcher with a custom SearchString
        /// </summary>
        /// <param name="customSearchString">The SearchString, diferent foreach web searcher</param>
        public abstract void GetCustomLinks(String customSearchString);

        protected void OnSearcherLinkFoundEvent(EventsThreads.CollectionFound<Uri> e)
        {
            EventHandler<EventsThreads.CollectionFound<Uri>> handler = SearcherLinkFoundEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void OnSearcherStartEvent(EventArgs e)
        {
            EventHandler handler = SearcherStartEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void OnSearcherEndEvent(EventsThreads.ThreadEndEventArgs e)
        {
            switch (e.EndReason)
            {
                case EventsThreads.ThreadEndEventArgs.EndReasonEnum.NoMoreData:
                    OnSearcherChangeStateEvent(new EventsThreads.ThreadStringEventArgs("Search done"));
                    break;
                case EventsThreads.ThreadEndEventArgs.EndReasonEnum.LimitReached:
                    OnSearcherChangeStateEvent(new EventsThreads.ThreadStringEventArgs(Name + " limit reached"));
                    break;
                case EventsThreads.ThreadEndEventArgs.EndReasonEnum.ErrorFound:
                    OnSearcherChangeStateEvent(new EventsThreads.ThreadStringEventArgs("Error found searching in " + Name));
                    break;
                case EventsThreads.ThreadEndEventArgs.EndReasonEnum.Stopped:
                    OnSearcherChangeStateEvent(new EventsThreads.ThreadStringEventArgs("Search stopped"));
                    break;
            }

            EventHandler<EventsThreads.ThreadEndEventArgs> handler = SearcherEndEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void OnSearcherChangeStateEvent(EventsThreads.ThreadStringEventArgs e)
        {
            EventHandler<EventsThreads.ThreadStringEventArgs> handler = SearcherChangeStateEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void OnSearcherLogEvent(EventsThreads.ThreadStringEventArgs e)
        {
            EventHandler<EventsThreads.ThreadStringEventArgs> handler = SearcherLogEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
