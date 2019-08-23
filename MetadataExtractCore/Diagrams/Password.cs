using System;

namespace MetadataExtractCore.Diagrams
{
    [Serializable]
    public class Password : MetadataValue
    {
        public string Type { get; set; }

        public string Source { get; set; }

        public Password(string password, string type) : base(password)
        {
            Type = type?.Trim();
        }

        public Password(string password, string type, string source) : this(password, type)
        {
            Source = source?.Trim();
        }
    }
}