using System;

namespace MetadataExtractCore.Diagrams
{
    public class OldVersion : MetadataValue
    {
        public string Author { get; set; }

        public string Comments { get; set; }

        public DateTime? Date { get; set; }

        public string Path { get; set; }

        public FileMetadata Metadata { get; set; }

        public OldVersion(string title, string author, string comment) : base(title)
        {
            this.Comments = comment?.Trim();
            this.Author = author?.Trim();
        }
    }
}
