using System;
using System.Collections.Generic;
using System.Linq;
using MetadataExtractCore.Analysis;
using MetadataExtractCore.Utilities;

namespace FOCA.Database.Entities
{
    [Serializable]
    public class Paths {
        public int Id { get; set; }

        public virtual List<PathsItem> Items { get; set; }

        public Paths()
        {
            Items = new List<PathsItem>();
        }

        public PathsItem AddUniqueItem(string pathValue, bool blnIsComputerFolder)
        {
            if (string.IsNullOrEmpty(pathValue) || pathValue.Trim() == string.Empty ||
                !PathAnalysis.IsValidPath(pathValue.Trim())) return null;

            var pathItem = new PathsItem();
            pathItem.Path = Functions.GetPathFolder(pathValue.Trim());
            pathItem.IsComputerFolder = blnIsComputerFolder;
            if (Items.Contains(pathItem, new CaseInsensitiveFolderItemComparer<PathsItem>()))
                return Items.FirstOrDefault(r => r.Path.ToLower() == pathItem.Path.ToLower());
            Items.Add(pathItem);
            return pathItem;
        }
    }

    [Serializable]
    public class PathsItem {

        public int Id { get; set; }

        public string Path { get; set; }

        public bool IsComputerFolder { get; set; }

        public virtual Users RemoteUsers { get; set; }

        public PathsItem()
        {
            RemoteUsers = new Users();
        }
    }

    public class CaseInsensitiveFolderItemComparer<T> : EqualityComparer<PathsItem>
    {
        public override bool Equals(PathsItem x, PathsItem y)
        {
            return StringComparer.OrdinalIgnoreCase.Equals(x.Path.Trim(), y.Path.Trim());
        }

        public override int GetHashCode(PathsItem obj)
        {
            return obj.GetHashCode();
        }
    }
}