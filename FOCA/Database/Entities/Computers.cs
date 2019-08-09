using FOCA.ModifiedComponents;
using MetadataExtractCore.Diagrams;
using System;

namespace FOCA.Database.Entities
{
    [Serializable]
    public class Computers : IDisposable
    {
        private ThreadSafeList<ComputersItem> itemsField;

        public Computers()
        {
            itemsField = new ThreadSafeList<ComputersItem>();
        }

        public ThreadSafeList<ComputersItem> Items
        {
            get { return itemsField; }
            set { itemsField = value; }
        }

        #region IDisposable Support

        private bool disposedValue; // To detect redundant calls

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
    public class ComputersItem : BaseItem, IDisposable 
    {
        public enum Tipo
        {
            ClientPC,
            Server
        };

        public int Id { get; set; }

        public virtual Descriptions Description { get; set; } = new Descriptions();
        public Paths Folders = new Paths();
        public string localName { get; set; }
        private string name_ { get; set; }

        public bool NotOS { get; set; }

        public OperatingSystem.OS os { get; set; }
        public virtual Printers Printers { get; set; } = new Printers();
        public virtual Paths RemoteFolders { get; set; } = new Paths();
        public virtual Passwords RemotePasswords { get; set; } = new Passwords();
        public virtual Printers RemotePrinters { get; set; } = new Printers();
        public virtual Users RemoteUsers { get; set; } = new Users();
        public virtual Applications Software { get; set; } = new Applications();
        public virtual ThreadSafeList<string> SourceDocuments { get; set; } = new ThreadSafeList<string>();
        public Tipo type { get; set; }
        public virtual Users Users { get; set; } = new Users();

        public string name
        {
            get { return name_; }
            set
            {
                if (!string.IsNullOrEmpty(name_))
                {
                    OnNameChangedEvent(new DoubleStringEventArgs(name_, value));
                    name_ = value;
                }
                else
                    name_ = value;
            }
        }

        [field: NonSerialized]

        public event EventHandler<DoubleStringEventArgs> NameChangedEvent;

        protected void OnNameChangedEvent(DoubleStringEventArgs e)
        {
            var handler = NameChangedEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #region IDisposable Support

        private bool disposedValue; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    SourceDocuments.Dispose();
                }

                SourceDocuments = null;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }


    public class DoubleStringEventArgs : EventArgs
    {
        public DoubleStringEventArgs(string oldString, string newString)
        {
            this.oldString = oldString;
            this.newString = newString;
        }

        public string oldString { get; }
        public string newString { get; }
    }
}