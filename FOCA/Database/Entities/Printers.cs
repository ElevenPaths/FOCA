using System;
using System.Collections.Generic;
using System.Linq;

namespace FOCA.Database.Entities
{
    [Serializable]
    public class Printers
    {
        public int Id { get; set; }

        public List<PrintersItem> Items { get; set; }

        public Printers()
        {
            Items = new List<PrintersItem>();
        }

        public PrintersItem AddUniqueItem(string strPrinter)
        {
            if (!string.IsNullOrEmpty(strPrinter) && strPrinter.Trim() != string.Empty)
            {
                var printersItem = new PrintersItem();
                printersItem.Printer = strPrinter.Trim();
                if (Items.Contains(printersItem, new CaseInsensitivePrinterItemComparer<PrintersItem>()))
                    return Items.FirstOrDefault(i => i.Printer.ToLower() == printersItem.Printer.ToLower());

                Items.Add(printersItem);
                return printersItem;
            }
            return null;
        }
    }

    [Serializable]
    public class PrintersItem
    {
        public int Id { get; set; }

        public string Printer { get; set; }

        public Users RemoteUsers { get; set; }

        public PrintersItem()
        {
            RemoteUsers = new Users();
        }
    }

    public class CaseInsensitivePrinterItemComparer<T> : EqualityComparer<PrintersItem>
    {
        public override bool Equals(PrintersItem x, PrintersItem y)
        {
            return StringComparer.OrdinalIgnoreCase.Equals(x.Printer.Trim(), y.Printer.Trim());
        }

        public override int GetHashCode(PrintersItem obj)
        {
            return obj.GetHashCode();
        }
    }
}