using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FOCA.Database.Entities
{
    [Serializable]
    public class Servers
    {
        public int Id { get; set; }
        public List<ServersItem> Items { get; set; }

        public Servers()
        {
            Items = new List<ServersItem>();
        }

        public void AddUniqueItem(ServersItem server)
        {
            if (Items.Count(s => s.Name == server.Name)==0)
                Items.Add(server);
        }
    }

    [Serializable]
    public class ServersItem
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Source { get; set; }

        public ServersItem() { }

        public ServersItem(string name)
        {
            Name = name;
        }

        public ServersItem(string name, string source)
        {
            Name = name;
            Source = source;
        }
    }
}