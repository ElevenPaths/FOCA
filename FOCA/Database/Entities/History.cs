using System;
using System.Collections.Generic;

namespace FOCA.Database.Entities
{
    [Serializable]
    public class History
    {
        public int Id { get; set; }
        public List<HistoryItem> Items { get; set; }

        public History()
        {
            Items = new List<HistoryItem>();
        }
    }

    [Serializable]
    public class HistoryItem
    {
        public int Id { get; set; }
        public string Author { get; set; }

        public string Comments { get; set; }

        public string Path { get; set; }
    }
}