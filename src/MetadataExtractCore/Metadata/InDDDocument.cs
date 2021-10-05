using MetadataExtractCore.Analysis;
using MetadataExtractCore.Diagrams;
using MetadataExtractCore.Utilities;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace MetadataExtractCore.Extractors
{
    public class InDDDocument : XMPExtractor
    {
        public InDDDocument(Stream stm) : base(stm)
        {
        }

        /// <summary>
        /// Extract metadata.
        /// </summary>
        public override FileMetadata AnalyzeFile()
        {
            try
            {
                this.foundMetadata = new FileMetadata();
                using (StreamReader sr = new StreamReader(this.fileStream))
                {
                    String sRead = sr.ReadToEnd();
                    foreach (Match m in Regex.Matches(sRead, @"@([a-z]:|\\)\\(([a-z0-9\s\-_\$&()ñÇ/n/r]+)\\)*[a-z0-9\s,;.\-_\$%&()=ñ{}Ç/n/r+@]+", RegexOptions.IgnoreCase))
                    {
                        String path = m.Value.Trim();
                        path = path.Substring(1);
                        this.foundMetadata.Add(new Diagrams.Path(PathAnalysis.CleanPath(path), true));
                    }
                    foreach (Match m in Regex.Matches(sRead, @"winspool\0([a-z]:|\\)\\(([a-z0-9\s\-_\$&()ñÇ/n/r]+)\\)*[a-z0-9\s,;.\-_\$%&()=ñ{}Ç/n/r+@]+", RegexOptions.IgnoreCase))
                    {
                        String printer = m.Value.Trim();
                        printer = printer.Substring(9);
                        this.foundMetadata.Add(new Printer(Functions.FilterPrinter(printer)));
                    }
                    foreach (Match m in Regex.Matches(sRead, @"<x:xmpmeta[^\0]*</x:xmpmeta>", RegexOptions.IgnoreCase))
                    {
                        String xmp = m.Value.Trim();
                        ReadXMPMetadata(xmp);
                    }
                    foreach (Match m in Regex.Matches(sRead, @"<rdf:RDF[^\0]*</rdf:RDF>", RegexOptions.IgnoreCase))
                    {
                        String xmp = m.Value.Trim();
                        ReadXMPMetadata(xmp);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            return this.foundMetadata;
        }

        /// <summary>
        /// Search the XMP metadata
        /// </summary>
        /// <param name="doc">A open PdfDocument</param>
        public void ReadXMPMetadata(string xmp)
        {
            if (xmp != string.Empty)
            {
                System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
                xDoc.XmlResolver = null;
                xDoc.LoadXml(xmp);
                this.ExtractFromXMP(xDoc);
            }
        }
    }
}
