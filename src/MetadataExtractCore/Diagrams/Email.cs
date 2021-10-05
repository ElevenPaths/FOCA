using System;
using System.Collections.Generic;

namespace MetadataExtractCore.Diagrams
{
    [Serializable]
    public class Email : MetadataValue
    {
        public Email(string email) : base(email)
        { }
    }
}