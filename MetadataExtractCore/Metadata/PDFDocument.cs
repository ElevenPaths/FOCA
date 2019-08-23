using MetadataExtractCore.Analysis;
using MetadataExtractCore.Diagrams;
using MetadataExtractCore.Utilities;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace MetadataExtractCore.Extractors
{
    public class PDFDocument : XMPExtractor
    {
        public PDFDocument(Stream stm) : base(stm)
        {
        }

        /// <summary>
        /// Extrae los metadatos del documento
        /// </summary>
        public override FileMetadata AnalyzeFile()
        {
            try
            {
                this.foundMetadata = new FileMetadata();
                using (PdfDocument doc = PdfReader.Open(this.fileStream, PdfDocumentOpenMode.InformationOnly))
                {
                    ReadXMPMetadata(doc);
                    if (doc.Info.Title != string.Empty)
                    {
                        this.foundMetadata.Title = Functions.ToPlainText(doc.Info.Title);
                        if (Uri.IsWellFormedUriString(doc.Info.Title, UriKind.Absolute))
                        {
                            this.foundMetadata.Add(new Diagrams.Path(PathAnalysis.CleanPath(doc.Info.Title), true));
                        }
                    }

                    if (doc.Info.Subject != string.Empty)
                        this.foundMetadata.Subject = Functions.ToPlainText(doc.Info.Subject);
                    if (doc.Info.Author != string.Empty)
                        this.foundMetadata.Add(new User(Functions.ToPlainText(doc.Info.Author), true));
                    if (doc.Info.Keywords != string.Empty)
                        this.foundMetadata.Keywords = Functions.ToPlainText(doc.Info.Keywords);

                    if (doc.Info.Creator != string.Empty)
                    {
                        string strSoftware = ApplicationAnalysis.GetApplicationsFromString(Functions.ToPlainText(doc.Info.Creator));
                        if (strSoftware.Trim() != string.Empty)
                        {
                            this.foundMetadata.Add(new Application(strSoftware));
                        }
                        //No se ha localizado ninguna aplicación conocida, aun así mostrar la aplicación encontrada
                        else if (!String.IsNullOrWhiteSpace(Functions.ToPlainText(doc.Info.Creator)))
                        {
                            this.foundMetadata.Add(new Application(Functions.ToPlainText(doc.Info.Creator).Trim()));
                        }
                    }

                    if (!String.IsNullOrWhiteSpace(doc.Info.Producer))
                    {
                        string strSoftware = ApplicationAnalysis.GetApplicationsFromString(Functions.ToPlainText(doc.Info.Producer));
                        if (!String.IsNullOrWhiteSpace(strSoftware))
                        {
                            this.foundMetadata.Add(new Application(strSoftware));
                        }
                        //No se ha localizado ninguna aplicación conocida, aun así mostrar la aplicación encontrada
                        else if (!String.IsNullOrWhiteSpace(Functions.ToPlainText(doc.Info.Producer)))
                        {
                            this.foundMetadata.Add(new Application(Functions.ToPlainText(doc.Info.Producer).Trim()));
                        }
                    }

                    try
                    {
                        if (doc.Info.CreationDate != DateTime.MinValue)
                        {
                            this.foundMetadata.Dates.CreationDate = doc.Info.CreationDate;
                        }
                    }
                    catch (InvalidCastException)
                    {
                    }

                    try
                    {
                        if (doc.Info.ModificationDate != DateTime.MinValue)
                        {
                            this.foundMetadata.Dates.ModificationDate = doc.Info.ModificationDate;
                        }
                    }
                    catch (InvalidCastException)
                    {
                    }
                }

                //Busca path y links binariamente
                this.foundMetadata.AddRange(BinarySearchPaths(this.fileStream).ToArray());
                this.foundMetadata.AddRange(BinarySearchLinks(this.fileStream).ToArray());

                foreach (Diagrams.Path ri in this.foundMetadata.Paths)
                {
                    //Busca usuarios dentro de la ruta
                    string strUser = PathAnalysis.ExtractUserFromPath(ri.Value);
                    this.foundMetadata.Add(new User(strUser, ri.IsComputerFolder));
                }

                //También busca el software en el título solo en los pdf, solo lo añade si es software conocido
                if (!String.IsNullOrEmpty(foundMetadata.Title))
                {
                    string strSoftware = ApplicationAnalysis.GetApplicationsFromString(foundMetadata.Title);
                    if (!String.IsNullOrWhiteSpace(strSoftware))
                        this.foundMetadata.Add(new Application(strSoftware));
                }
            }
            catch (PdfReaderException)
            { }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            finally
            {
                if (foundMetadata == null)
                    this.foundMetadata = new FileMetadata();
            }
            return this.foundMetadata;
        }

        /// <summary>
        /// Search the XMP metadata
        /// </summary>
        /// <param name="doc">A open PdfDocument</param>
        private void ReadXMPMetadata(PdfDocument doc)
        {
            if (doc.Internals.Catalog.Elements.ContainsKey("/Metadata"))
            {
                PdfItem pi = doc.Internals.Catalog.Elements["/Metadata"];
                //doc.Internals.Catalog.Elements.Remove("/Metadata");
                if (pi is PdfSharp.Pdf.Advanced.PdfReference)
                {
                    int intXMPObjectNumber = (pi as PdfSharp.Pdf.Advanced.PdfReference).ObjectNumber;
                    PdfDictionary pDic = (PdfDictionary)doc.Internals.GetObject(new PdfObjectID(intXMPObjectNumber));
                    string xmp = pDic.Stream.ToString();
                    if (xmp != string.Empty)
                    {
                        System.Xml.XmlDocument xDoc = new XmlDocument();
                        xDoc.XmlResolver = null;
                        xDoc.LoadXml(xmp);
                        this.ExtractFromXMP(xDoc);
                    }
                }
            }
        }

        private IEnumerable<Diagrams.Path> BinarySearchPaths(Stream stm)
        {
            stm.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(stm);
            String sRead = sr.ReadToEnd();
            foreach (Match m in Regex.Matches(sRead, @"([a-z]:|\\\\)\\\\(([a-z0-9\s\-_\$&()ñÇ/n/r]+)\\\\)*[a-z0-9\s,;.\-_\$%&()=ñ{}Ç/n/r+@]+", RegexOptions.IgnoreCase))
            {
                String path = m.Value.Trim();
                if (path.Contains(")"))
                    path = path.Remove(path.IndexOf(')') - 1);
                path = path.Replace(@"\\", @"\");
                path = path.Replace("\\\r", "");
                path = path.Replace("\\\n", "");
                yield return new Diagrams.Path(PathAnalysis.CleanPath(path), true);
            }
        }

        private IEnumerable<Diagrams.Path> BinarySearchLinks(Stream stm)
        {
            stm.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(stm);
            String sRead = sr.ReadToEnd();
            List<string> links = new List<string>();
            foreach (Match m in Regex.Matches(sRead, @"http://[^)]*", RegexOptions.IgnoreCase))
            {
                String href = m.Value.Trim();
                if (IsInterestingLink(href))
                {
                    if (!links.Contains(href))
                        links.Add(href);
                }
            }
            foreach (Match m in Regex.Matches(sRead, @"file:///[^)]*", RegexOptions.IgnoreCase))
            {
                if (!links.Contains(m.Value))
                    links.Add(m.Value);
            }
            if (links.Count != 0)
            {
                foreach (String link in links)
                {
                    yield return new Diagrams.Path(PathAnalysis.CleanPath(link), true);
                }
            }
        }
    }
}
