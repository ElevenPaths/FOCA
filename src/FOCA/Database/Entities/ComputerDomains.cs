using FOCA.ModifiedComponents;
using System;

namespace FOCA.Database.Entities
{
    public class ComputerDomains
    {
        private ThreadSafeList<ComputerDomainsItem> itemsField;

        public ComputerDomains()
        {
            itemsField = new ThreadSafeList<ComputerDomainsItem>();
        }


        public ThreadSafeList<ComputerDomainsItem> Items
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
    public class ComputerDomainsItem : BaseItem
    {
        public int Id { get; set; }
        public virtual ComputersItem Computer { get; set; }
        public virtual DomainsItem Domain { get; set; }
        public string Source { get; set; }

        public ComputerDomainsItem() : base() { }

        public ComputerDomainsItem(ComputersItem computer, DomainsItem domain, string source) : base()
        {
            this.Computer = computer;
            this.Domain = domain;
            this.Source = source;
        }
    }
}