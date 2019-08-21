using MetadataExtractCore.Diagrams;
using System;
using System.IO;
using System.Linq;
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

        public static readonly string[] SupportedExtensions = new string[] { ".sxw", ".odt", ".ods", ".odg", ".odp", ".docx", ".xlsx", ".pptx", ".ppsx", ".doc", ".xls", ".ppt", ".pps", ".pdf", ".wpd", ".raw", ".cr2", ".crw", ".jpg", ".jpeg", ".svg", ".svgz", ".indd", ".rdp", ".ica" };

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

        private static string NormalizeExtension(string extension)
        {
            if (String.IsNullOrWhiteSpace(extension))
                throw new ArgumentNullException(nameof(extension));

            if (!extension.StartsWith("."))
                extension = "." + extension;

            return extension.ToLowerInvariant().Trim();
        }

        public static bool IsSupportedExtension(string extension)
        {
            string normalizedExtension = NormalizeExtension(extension);
            return SupportedExtensions.Any(p => p.Equals(normalizedExtension));
        }

        public static MetaExtractor Create(string extension, Stream file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));

            string normalizedExtension = NormalizeExtension(extension);
            if (IsSupportedExtension(normalizedExtension))
            {
                MetaExtractor document = null;
                switch (normalizedExtension)
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
                        throw new ArgumentException("Extension not supported", nameof(extension));
                }

                return document;
            }
            else
            {
                throw new ArgumentException("Extension not supported", nameof(extension));
            }
        }
    }
}
