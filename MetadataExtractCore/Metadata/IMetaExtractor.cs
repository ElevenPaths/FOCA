using System;
using System.IO;
using System.Xml.Serialization;
using MetadataExtractCore.Diagrams;

namespace MetadataExtractCore.Metadata
{

    [XmlInclude(typeof(EXIFDocument))]
    [XmlInclude(typeof(Office972003))]
    [XmlInclude(typeof(OfficeOpenXMLDocument))]
    [XmlInclude(typeof(OpenOfficeDocument))]
    [XmlInclude(typeof(PDFDocument))]
    [XmlInclude(typeof(WPDDocument))]
    [XmlInclude(typeof(InDDDocument))]
    [XmlInclude(typeof(SVGDocument))]
    [XmlInclude(typeof(WPDDocument))]
    [XmlInclude(typeof(RDPDocument))]
    [XmlInclude(typeof(ICADocument))]
    [Serializable]

    public abstract class MetaExtractor
    {
        [NonSerialized]
        protected MemoryStream stm;

        public int Id { get; set; }
        public Emails FoundEmails { get; set; }
        public Dates FoundDates { get; set; }
        public Printers FoundPrinters { get; set; }
        public Paths FoundPaths { get; set; }
        public OldVersions FoundOldVersions { get; set; }
        public History FoundHistory { get; set; }
        public MetaData FoundMetaData { get; set; }
        public Users FoundUsers {get; set; }
        public Servers FoundServers { get; set; }
        public Passwords FoundPasswords { get; set; }

        public abstract void analyzeFile();

        public MetaExtractor()
        {
            FoundEmails = new Emails();
            FoundDates = new Dates();
            FoundPrinters = new Printers();
            FoundPaths = new Paths();
            FoundOldVersions = new OldVersions();
            FoundHistory = new History();
            FoundMetaData = new MetaData();
            FoundUsers = new Users();
            FoundServers = new Servers();
            FoundPasswords = new Passwords();
        }

        /// <summary>
        /// Release used resources
        /// </summary>
        public void Close()
        {

            if (stm != null)
            {
                stm.Close();
                stm = null;
            }
        }
    }
}
