using FOCA.ModifiedComponents;
using System;

namespace FOCA.Database.Entities
{

    [Serializable]
    public class Relations : IDisposable
    {
        private ThreadSafeList<RelationsItem> itemsField;

        public Relations()
        {
            itemsField = new ThreadSafeList<RelationsItem>();
        }

        public ThreadSafeList<RelationsItem> Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    itemsField.Dispose();
                }

                itemsField = null;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

    [Serializable]
    public class RelationsItem : BaseItem
    {
        public int Id { get; set; }
        public virtual DomainsItem Domain { get; set; }
        public virtual IPsItem Ip { get; set; }
        public string Source { get; set; }

        /// <summary>
        /// Necessary for serialization
        /// </summary>
        public RelationsItem() : base() { }

        public RelationsItem(DomainsItem domain, IPsItem ip, string source) : base()
        {
            this.Domain = domain;
            this.Ip = ip;
            this.Ip.IdProject = Program.data.Project.Id;
            this.Source = source;
        }
    }
}