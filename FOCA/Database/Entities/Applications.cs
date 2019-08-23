using FOCA.ModifiedComponents;
using System;
using System.Linq;

namespace FOCA.Database.Entities
{
    [Serializable]
    public class Applications {
        public int Id { get; set; }
        public ThreadSafeList<ApplicationsItem> Items { get; set; }

        public Applications()
        {
            Items = new ThreadSafeList<ApplicationsItem>();
        }

        /// <summary>
        /// Add new item
        /// </summary>
        /// <param name="app"></param>
        public void AddUniqueItem(ApplicationsItem app)
        {
            if (Items.Count(s => s.Name == app.Name) == 0)
                Items.Add(app);
        }
    }

    [Serializable]
    public class ApplicationsItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Source { get; set; }

        public ApplicationsItem() { }

        public ApplicationsItem(string name)
        {
            Name = name;
        }

        public ApplicationsItem(string name, string source)
        {
            Name = name;
            Source = source;
        }
    }
}