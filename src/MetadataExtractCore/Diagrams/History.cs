using System;

namespace MetadataExtractCore.Diagrams
{
    [Serializable]
    public class History : MetadataValue
    {
        public History(string author, string comments = "", string path = "") : base(author)
        {
            this.Comments = comments?.Trim();
            this.Path = path?.Trim();
        }

        public string Comments { get; set; }

        public string Path { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this.GetType() != obj.GetType())
                return false;

            History p = (History)obj;
            return String.Concat(this.Value, "--", this.Path).Equals(String.Concat(p.Value, "--", p.Path), StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return String.Concat(this.Value.ToLowerInvariant(), "--", this.Path.ToLowerInvariant()).GetHashCode();
        }
    }
}