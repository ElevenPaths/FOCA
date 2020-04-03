using FOCA.ModifiedComponents;
using Newtonsoft.Json;
using System;

namespace FOCA.Database.Entities
{
    [Serializable]
    public class Ficheros : IDisposable
    {

        private ThreadSafeList<FilesItem> itemsField;

        public Ficheros()
        {
            itemsField = new ThreadSafeList<FilesItem>();
        }

        public ThreadSafeList<FilesItem> Items
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

        public FilesItem GetByURL(string URL)
        {
            //No se puede usar linq sino da fallo el eazfuscator
            foreach (FilesItem fi in Items)
            {
                if (fi.URL.ToLower() == URL.ToLower())
                    return fi;
            }
            return null;
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
    public class FilesItem : BaseItem
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public override int IdProject { get; set; }

        public string Ext { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;

        [JsonIgnore]
        public virtual MetaExtractor Metadata { get; set; }

        public string URL { get; set; }
        public string Path { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int Size { get; set; }
        public bool Downloaded { get; set; }
        public bool MetadataExtracted { get; set; }
        public bool DiarioAnalyzed { get; set; }
        public string DiarioPrediction { get; set; } = "Unanalyzed";
    }
}
