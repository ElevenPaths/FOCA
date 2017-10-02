using System;
using FOCA.ModifiedComponents;

[Serializable]
public class ComputerIPs : IDisposable
{
    private ThreadSafeList<ComputerIPsItem> itemsField;

    public ComputerIPs()
    {
        itemsField = new ThreadSafeList<ComputerIPsItem>();
    }

    public ThreadSafeList<ComputerIPsItem> Items
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
public class ComputerIPsItem
{
    public int Id { get; set; }
    public int IdProject { get; set; }
    public virtual ComputersItem Computer { get; set; }

    public virtual IPsItem Ip { get; set; }
    //Fuente de la relación
    public string Source { get; set; }

    /// <summary>
    /// Necesario para la serialización
    /// </summary>
    public ComputerIPsItem() { }

    public ComputerIPsItem(ComputersItem computer, IPsItem ip, string source)
    {
        this.Computer = computer;
        this.Ip = ip;
        this.Source = source;
    }
}