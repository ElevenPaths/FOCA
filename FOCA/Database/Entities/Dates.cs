using System;

namespace FOCA.Database.Entities
{
    [Serializable]
    public class Dates {

        public int Id { get; set; }

        public DateTime CreationDate = DateTime.Now;

        public bool CreationDateSpecified { get; set; }

        public DateTime ModificationDate = DateTime.Now;

        public bool ModificationDateSpecified { get; set; }

        public DateTime DatePrinting = DateTime.Now;

        public bool DatePrintingSpecified { get; set; }
    }
}
