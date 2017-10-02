using System;
using System.Net;

namespace FOCA.Analysis.HttpMap
{
    [Serializable]
    public class FuzzOpenFolderObject
    {
        public int Id { get; set; }

        public string url;

        public FuzzOpenFolderObject()
        {
        }

        public FuzzOpenFolderObject(string url)
        {
            this.url = url;
        }
    }

    [Serializable]
    public class FuzzMutexObject
    {
        public HttpStatusCode cod;
        public string url;

        public FuzzMutexObject()
        {
        }

        public FuzzMutexObject(string url, HttpStatusCode cod)
        {
            this.url = url;
            this.cod = cod;
        }
    }

    [Serializable]
    public class FuzzMethodFolderObject
    {
        public string methods;
        public string url;

        public FuzzMethodFolderObject()
        {
        }

        public FuzzMethodFolderObject(string url, string methods)
        {
            this.url = url;
            this.methods = methods;
        }
    }
}