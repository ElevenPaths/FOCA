using System;

namespace MetadataExtractCore.Diagrams
{
    [Serializable]
    public class Server : MetadataValue
    {
        public string Source { get; set; }

        public Server(string name) : base(name)
        {
        }

        public Server(string name, string source) : this(name)
        {
            Source = source?.Trim();
        }
    }
}