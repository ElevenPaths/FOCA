using System;
using FOCA.ModifiedComponents;

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
public class RelationsItem
{
    public int Id { get; set; }
    public int IdProject { get; set; }
    public virtual DomainsItem Domain { get; set; }
    public virtual IPsItem Ip { get; set; }
    public string Source { get; set; }

    /// <summary>
    /// Necesario para la serialización
    /// </summary>
    public RelationsItem() { }

    public RelationsItem(DomainsItem domain, IPsItem ip, string source)
    {
        this.Domain = domain;
        this.Ip = ip;
        this.Source = source;
    }
}