using System;

namespace MetadataExtractCore.Diagrams
{
    [Serializable]
    public class Application : MetadataValue
    {
        public string Source { get; set; }

        public Application(string name) : base(name)
        {
        }

        public Application(string name, string source) : this(name)
        {
            Source = source?.Trim();
        }
    }
}