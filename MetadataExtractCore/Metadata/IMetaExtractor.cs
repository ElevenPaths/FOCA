using MetadataExtractCore.Diagrams;
using System;
using System.IO;
using System.Xml.Serialization;

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
        public Users FoundUsers { get; set; }
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

        public static MetaExtractor Create(string extension, Stream file)
        {
            if (String.IsNullOrWhiteSpace(extension))
                throw new ArgumentNullException(nameof(extension));

            if (file == null)
                throw new ArgumentNullException(nameof(file));

            MetaExtractor document = null;
            switch (extension.ToLowerInvariant().Trim())
            {
                case ".sxw":
                case ".odt":
                case ".ods":
                case ".odg":
                case ".odp":
                    document = new OpenOfficeDocument(file, extension);
                    break;
                case ".docx":
                case ".xlsx":
                case ".pptx":
                case ".ppsx":
                    document = new OfficeOpenXMLDocument(file, extension);
                    break;
                case ".doc":
                case ".xls":
                case ".ppt":
                case ".pps":
                    document = new Office972003(file);
                    break;
                case ".pdf":
                    document = new PDFDocument(file);
                    break;
                case ".wpd":
                    document = new WPDDocument(file);
                    break;
                case ".raw":
                case ".cr2":
                case ".crw":
                case ".jpg":
                case ".jpeg":
                    document = new EXIFDocument(file, extension);
                    break;
                case ".svg":
                case ".svgz":
                    document = new SVGDocument(file);
                    break;
                case ".indd":
                    document = new InDDDocument(file);
                    break;
                case ".rdp":
                    document = new RDPDocument(file);
                    break;
                case ".ica":
                    document = new ICADocument(file);
                    break;
                default:
                    throw new ArgumentException("Extension not allowed", nameof(extension));
            }
            return document;
        }
    }
}
