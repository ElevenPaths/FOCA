using MetadataExtractCore.Diagrams;
using MetadataExtractCore.Utilities;
using System;
using System.IO;
using System.Linq;

namespace MetadataExtractCore.Extractors
{
    public abstract class DocumentExtractor : IDisposable
    {
        public static readonly string[] SupportedExtensions = new string[] { ".sxw", ".odt", ".ods", ".odg", ".odp", ".docx", ".xlsx", ".pptx", ".ppsx", ".doc", ".xls", ".ppt", ".pps", ".pdf", ".wpd", ".raw", ".cr2", ".crw", ".jpg", ".jpeg", ".png", ".svg", ".svgz", ".indd", ".rdp", ".ica" };

        protected MemoryStream fileStream;
        protected FileMetadata foundMetadata;

        private bool disposed = false;

        public abstract FileMetadata AnalyzeFile();

        public DocumentExtractor(Stream fileStream)
        {
            this.fileStream = new MemoryStream();
            Functions.CopyStream(fileStream, this.fileStream);
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

        public static DocumentExtractor Create(string extension, Stream file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            string normalizedExtension = NormalizeExtension(extension);
            if (IsSupportedExtension(normalizedExtension))
            {
                DocumentExtractor document = null;
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
                    case ".png":
                        document = new EXIFDocument(file);
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

        protected bool IsInterestingLink(string href)
        {
            if (href != string.Empty)
            {
                if (href.StartsWith("mailto:"))
                {
                    string email = href.Substring(7, (href.Contains("?") ? href.IndexOf('?') : href.Length) - 7);
                    this.foundMetadata?.Add(new Email(email));
                }
                else if (href.StartsWith("ftp:"))
                {
                    return true;
                }
                else if (href.StartsWith("http:"))
                {
                    return true;
                }
                else if (href.StartsWith("https:"))
                {
                    return true;
                }
                else if (href.StartsWith("telnet:"))
                {
                    return true;
                }
                else if (href.StartsWith("ldap:"))
                {
                    return true;
                }
                else if (href.StartsWith("file:"))
                {
                    return true;
                }
                else
                {
                    Uri url;
                    if (Uri.TryCreate(href, UriKind.Absolute, out url))
                    {
                        if (url.HostNameType != UriHostNameType.Dns)
                        {
                            return true;
                        }
                    }
                    else if (!href.StartsWith("#"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    this.fileStream.Dispose();
                }

                disposed = true;
            }
        }

        ~DocumentExtractor()
        {
            Dispose(false);
        }

    }
}
