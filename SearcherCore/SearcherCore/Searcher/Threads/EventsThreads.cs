using System;
using System.Collections.Generic;
using System.Text;

namespace FOCA.Threads
{
    public class EventsThreads
    {
        public class ThreadListDataFoundEventArgs : EventArgs
        {
            public ThreadListDataFoundEventArgs(List<object> lstData)
            {
                this.lstData = lstData;
            }
            private List<object> lstData;

            public List<object> Data
            {
                get { return lstData; }
                set { lstData = value; }
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

        public class ThreadListDataEndEventArgs : EventArgs
        {
            public ThreadListDataEndEventArgs(EndReasonEnum EndReason, List<string> lstData)
            {
                this.EndReason = EndReason;
                this.lstData = lstData;
            }

            public enum EndReasonEnum { NoMoreData, LimitReached, ErrorFound, Stopped }
            public EndReasonEnum EndReason;
            private List<string> lstData;

            public List<string> Data
            {
                get { return lstData; }
                set { lstData = value; }
            }
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
