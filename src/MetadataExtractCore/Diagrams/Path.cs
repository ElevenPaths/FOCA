using MetadataExtractCore.Analysis;
using MetadataExtractCore.Utilities;
using System;

namespace MetadataExtractCore.Diagrams
{
    [Serializable]
    public class Path : MetadataValue
    {
        public bool IsComputerFolder { get; }

        public Path(string path, bool isComputerFolder) : base(Functions.GetPathFolder(path))
        {
            this.IsComputerFolder = isComputerFolder;
        }

        public override bool IsValid()
        {
            return PathAnalysis.IsValidPath(this.Value.Trim());
        }
    }
}