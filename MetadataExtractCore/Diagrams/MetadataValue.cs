using System;

namespace MetadataExtractCore.Diagrams
{
    public abstract class MetadataValue
    {
        public string Value { get; }

        public MetadataValue(string value)
        {
            this.Value = value?.Trim();
        }

        public virtual bool IsValid()
        {
            return !String.IsNullOrWhiteSpace(this.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this.GetType() != obj.GetType())
                return false;

            MetadataValue p = (MetadataValue)obj;
            return this.Value.Equals(p.Value, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode()
        {
            return this.Value.ToLowerInvariant().GetHashCode();
        }
    }
}
