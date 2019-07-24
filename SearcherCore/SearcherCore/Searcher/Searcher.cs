using FOCA.Threads;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FOCA.Searcher
{
    public abstract class Searcher<TInput, TOutput> : IDisposable
    {
        /// <summary>
        /// Event raised when a start the search
        /// </summary>
        public event EventHandler SearcherStartEvent;

        /// <summary>
        /// Event raised when a new link or group of links is found. Be carefull! The searcher may return repeated links.
        /// </summary>
        public event EventHandler<EventsThreads.CollectionFound<TOutput>> ItemsFoundEvent;

        /// <summary>
        /// Event raised when the state of the class change, new search, error, end of search, etc.
        /// </summary>
        public event EventHandler<EventsThreads.ThreadStringEventArgs> SearcherChangeStateEvent;

        /// <summary>
        /// Event raised when there is any important to log
        /// </summary>
        public event EventHandler<EventsThreads.ThreadStringEventArgs> SearcherLogEvent;

        private CancellationTokenSource cancelTokenSource;

        protected CancellationToken CancelToken
        {
            get
            {
                return cancelTokenSource.Token;
            }
        }

        public string Name
        {
            get;
        }

        public Searcher(string searcherName)
        {
            this.Name = searcherName;
        }

        public Task<int> CustomSearch(params TInput[] customSearchValues)
        {
            return CustomSearch(null, customSearchValues);
        }

        public Task<int> CustomSearch(CancellationTokenSource cancelToken, params TInput[] customSearchValues)
        {
            if (customSearchValues == null || customSearchValues.Length == 0)
                throw new ArgumentNullException(nameof(customSearchValues));

            if (cancelToken != null)
            {
                this.cancelTokenSource = cancelToken;
            }
            else
            {
                this.cancelTokenSource = new CancellationTokenSource();
            }

            OnSearcherStartEvent(null);
            OnSearcherChangeStateEvent(new EventsThreads.ThreadStringEventArgs($"Starting search with {this.Name} engine"));
            return Task.Factory.StartNew<int>(() =>
            {
                int totalCount = 0;
                foreach (TInput searchValue in customSearchValues)
                {
                    totalCount += this.Search(searchValue, this.CancelToken);
                    this.CancelToken.ThrowIfCancellationRequested();
                }
                return totalCount;
            }, this.CancelToken);
        }

        protected abstract int Search(TInput customSearchString, CancellationToken cancelToken);

        protected void OnSearcherLinkFoundEvent(EventsThreads.CollectionFound<TOutput> e)
        {
            EventHandler<EventsThreads.CollectionFound<TOutput>> handler = ItemsFoundEvent;
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

        public void Dispose()
        {
            GC.Collect();
        }

        public void Cancel()
        {
            cancelTokenSource.Cancel();
        }
    }
}
