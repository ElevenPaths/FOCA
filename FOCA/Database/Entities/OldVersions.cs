using System;
using System.Collections.Generic;

namespace FOCA.Database.Entities
{
    [Serializable]
    public class OldVersions
    {
        public int Id { get; set; }
        public List<OldVersionsItem> Items { get; set; }

        public OldVersions()
        {
            Items = new List<OldVersionsItem>();
        }
    }

    [Serializable]
    public class OldVersionsItem
    {
        public int Id { get; set; }
        public string Author { get; set; }

        public string Comments { get; set; }

        public DateTime Date = new DateTime();

        public bool SpecificDate { get; set; }

        public string Path { get; set; }
    }
}