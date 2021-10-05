using System;
using System.Collections.Generic;

namespace FOCA.Threads
{
    public class EventsThreads
    {
        public class CollectionFound<T> : EventArgs
        {
            public CollectionFound(ICollection<T> lstData)
            {
                this.Data = lstData;
            }

            public ICollection<T> Data
            {
                get;
            }
        }

        public class ThreadEndEventArgs : EventArgs
        {
            public ThreadEndEventArgs(EndReasonEnum EndReason)
            {
                this.EndReason = EndReason;
            }

            public enum EndReasonEnum { NoMoreData, LimitReached, ErrorFound, Stopped }
            public EndReasonEnum EndReason;
        }

        public class ThreadStringEventArgs : EventArgs
        {
            public ThreadStringEventArgs(String strMessage)
            {
                this.strMessage = strMessage;
            }

            private string strMessage;
            public string Message
            {
                get { return strMessage; }
            }
        }
    }
}
