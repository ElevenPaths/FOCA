using System;

namespace MetadataExtractCore.Diagrams
{
    [Serializable]
    public class Printer : MetadataValue
    {
        public Printer(string printerName) : base(printerName)
        {
        }
    }
}