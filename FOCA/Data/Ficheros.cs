using System;
using FOCA.ModifiedComponents;
using MetadataExtractCore.Metadata;
using Newtonsoft.Json;

[Serializable]
public class Ficheros : IDisposable
{

    private ThreadSafeList<FilesITem> itemsField;

    public Ficheros()
    {
        itemsField = new ThreadSafeList<FilesITem>();
    }

    public ThreadSafeList<FilesITem> Items
    {
        get {
            return this.itemsField;
        }
        set {
            this.itemsField = value;
        }
    }

    public FilesITem GetByURL(string URL)
    {
        //No se puede usar linq sino da fallo el eazfuscator
        foreach (FilesITem fi in Items)
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
public class FilesITem {
    [JsonIgnore]
    public int Id { get; set; }
    [JsonIgnore]
    public int IdProject { get; set; }

    public string Ext { get; set; }
    public DateTime ModifiedDate { get; set; } = DateTime.Now;

    [JsonIgnore]
    public virtual MetaExtractor Metadata { get; set; }

    public string URL { get; set; }
    public string Path { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public int Size { get; set; }
    public bool Downloaded { get; set; }
    public bool Processed { get; set; }
}
