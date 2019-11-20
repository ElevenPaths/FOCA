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
        private static Regex pathRegex = new Regex(@"([a-z]:|\\\\)\\\\(([a-z0-9\s\-_\$&()ñÇ/n/r]+)\\\\)*[a-z0-9\s,;.\-_\$%&()=ñ{}Ç/n/r+@]+", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex emailsAndLinksRegex = new Regex(@"\((?<value>(mailto|https?):.*?)\)", RegexOptions.Singleline | RegexOptions.Compiled);

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

                SearchPathsLinksAndEmails(this.fileStream);

                //Find users in paths
                foreach (Diagrams.Path path in this.foundMetadata.Paths)
                {
                    string strUser = PathAnalysis.ExtractUserFromPath(path.Value);
                    this.foundMetadata.Add(new User(strUser, path.IsComputerFolder));
                }

                //Also search software in the title (only pdf). It is added only if the software is known.
                if (!String.IsNullOrEmpty(foundMetadata.Title))
                {
                    string strSoftware = ApplicationAnalysis.GetApplicationsFromString(foundMetadata.Title);
                    if (!String.IsNullOrWhiteSpace(strSoftware))
                        this.foundMetadata.Add(new Application(strSoftware));
                }
            }
            catch (PdfReaderException ex)
            { }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            finally
            {
                if (foundMetadata == null)
                    this.foundMetadata = new FileMetadata();

                if (fileStream != null)
                {
                    this.fileStream.Dispose();
                }
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
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.XmlResolver = null;
                        xDoc.LoadXml(xmp);
                        this.ExtractFromXMP(xDoc);
                    }
                }
            }
        }

        private void SearchPathsLinksAndEmails(Stream stm)
        {
            try
            {
                stm.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(stm);

                String fileInBytes = sr.ReadToEnd();

                this.foundMetadata.AddRange(SearchForPaths(fileInBytes).ToArray());
                SearchForLinksAndEmails(fileInBytes);

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Problem in {nameof(SearchPathsLinksAndEmails)}" + ex.ToString());
            }
        }

        private IEnumerable<Diagrams.Path> SearchForPaths(String fileInString)
        {
            foreach (Match m in pathRegex.Matches(fileInString))
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

        private void SearchForLinksAndEmails(string fileInString)
        {
            foreach (Match match in emailsAndLinksRegex.Matches(fileInString))
            {
                String valueFound = match.Groups["value"].Value;

                if (valueFound.Contains("mailto"))
                {
                    string email = valueFound.Remove(0, "mailto:".Length);

                    if (!this.foundMetadata.Emails.Any(x => x.Value == email))
                    {
                        this.foundMetadata.Emails.Add(new Email(email));
                    }
                }
                else
                {
                    string link = PathAnalysis.CleanPath(valueFound);

                    if (!this.foundMetadata.Paths.Any(x => x.Value == link))
                    {
                        this.foundMetadata.Paths.Add(new Diagrams.Path(link, false));
                    }
                }
            }
        }
    }
}
