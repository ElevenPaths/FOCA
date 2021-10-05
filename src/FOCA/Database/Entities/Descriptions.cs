using System;
using System.Collections.Generic;
using System.Linq;

namespace FOCA.Database.Entities
{
    [Serializable]
    public class Descriptions
    {
        public int Id { get; set; }

        public List<DescriptionsItem> Items { get; set; }

        public Descriptions()
        {
            Items = new List<DescriptionsItem>();
        }

        public void AddUniqueItem(DescriptionsItem desc)
        {
            if (Items.Count(S => S.Description == desc.Description) == 0)
                Items.Add(desc);
        }
    }

    [Serializable]
    public class DescriptionsItem
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string Source { get; set; }

        public DescriptionsItem() { }

        public DescriptionsItem(string description)
        {
            Description = description;
        }

        public DescriptionsItem(string description, string source)
        {
            Description = description;
            Source = source;
        }


    }
}