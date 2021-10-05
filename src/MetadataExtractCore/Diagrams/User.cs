using MetadataExtractCore.Utilities;
using System;

namespace MetadataExtractCore.Diagrams
{
    [Serializable]
    public class User : MetadataValue
    {
        public bool IsComputerUser { get; set; }

        public string Notes { get; set; }

        public User(string name, bool isComputerUser, string notes = "") : base(name)
        {
            this.IsComputerUser = isComputerUser;
            this.Notes = notes?.Trim();
        }

        public override bool IsValid()
        {
            return !String.IsNullOrWhiteSpace(this.Value) && this.Value.Trim().Length > 1 && Functions.StringContainAnyLetter(this.Value);
        }
    }
}