using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Xml;
using MetadataExtractCore.Analysis;
using MetadataExtractCore.Utilities;
using MetadataExtractCore.Diagrams;

namespace MetadataExtractCore.Metadata
{
    [Serializable]
    public class PDFDocument : MetaExtractor
    {
        /// <summary>
        /// Necesario para la serialización
        /// </summary>
        public PDFDocument() { }

        /// <summary>
        /// Constructor de la clase PDFDocument
        /// </summary>
        /// <param name="stm">Stream que contiene el fichero wpd</param>
        public PDFDocument(Stream stm)
        {
            this.stm = new MemoryStream();
            Functions.CopyStream(stm, this.stm);
        }

        /// <summary>
        /// Extrae los metadatos del documento
        /// </summary>
        public override void analyzeFile()
        {
            PdfDocument doc = null;
            try
            {
                doc = PdfReader.Open(stm, PdfDocumentOpenMode.InformationOnly);
                ReadXMPMetadata(doc);
                if (doc.Info.Title != string.Empty)
                {
                    FoundMetaData.Title = Functions.ToPlainText(doc.Info.Title);
                    FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(doc.Info.Title), true);
                }
                if (doc.Info.Subject != string.Empty)
                    FoundMetaData.Subject = Functions.ToPlainText(doc.Info.Subject);
                if (doc.Info.Author != string.Empty)
                    FoundUsers.AddUniqueItem(Functions.ToPlainText(doc.Info.Author), true);
                if (doc.Info.Keywords != string.Empty)
                    FoundMetaData.Keywords = Functions.ToPlainText(doc.Info.Keywords);
                if (doc.Info.Creator != string.Empty)
                {
                    string strSoftware = Analysis.ApplicationAnalysis.GetApplicationsFromString(Functions.ToPlainText(doc.Info.Creator));
                    if (strSoftware.Trim() != string.Empty)
                    {
                        if (!FoundMetaData.Applications.Items.Any(A => A.Name == strSoftware))
                            FoundMetaData.Applications.Items.Add(new ApplicationsItem(strSoftware));
                    }
                    //No se ha localizado ninguna aplicación conocida, aun así mostrar la aplicación encontrada
                    else
                    {
                        if (Functions.ToPlainText(doc.Info.Creator).Trim() != string.Empty && !FoundMetaData.Applications.Items.Any(A => A.Name == Functions.ToPlainText(doc.Info.Creator).Trim()))
                        {
                            FoundMetaData.Applications.Items.Add(new ApplicationsItem(Functions.ToPlainText(doc.Info.Creator).Trim()));
                        }
                    }
                }
                if (doc.Info.Producer != string.Empty)
                {
                    string strSoftware = Analysis.ApplicationAnalysis.GetApplicationsFromString(Functions.ToPlainText(doc.Info.Producer));
                    if (strSoftware.Trim() != string.Empty)
                    {
                        if (!FoundMetaData.Applications.Items.Any(A => A.Name == strSoftware))
                            FoundMetaData.Applications.Items.Add(new ApplicationsItem(strSoftware));
                    }
                    //No se ha localizado ninguna aplicación conocida, aun así mostrar la aplicación encontrada
                    else
                    {
                        if (Functions.ToPlainText(doc.Info.Producer).Trim() != string.Empty && !FoundMetaData.Applications.Items.Any(A => A.Name == Functions.ToPlainText(doc.Info.Producer).Trim()))
                        {
                            FoundMetaData.Applications.Items.Add(new ApplicationsItem(Functions.ToPlainText(doc.Info.Producer).Trim()));
                        }
                    }
                }
                if (doc.Info.CreationDate != DateTime.MinValue)
                {
                    FoundDates.CreationDateSpecified = true;
                    FoundDates.CreationDate = doc.Info.CreationDate;
                }
                if (doc.Info.ModificationDate != DateTime.MinValue)
                {
                    FoundDates.ModificationDateSpecified = true;
                    FoundDates.ModificationDate = doc.Info.ModificationDate;
                }
                //Busca path y links binariamente
                BinarySearchPaths(stm);
                BinarySearchLinks(stm);

                foreach (PathsItem ri in FoundPaths.Items)
                {
                    //Busca usuarios dentro de la ruta
                    string strUser = PathAnalysis.ExtractUserFromPath(ri.Path);
                    FoundUsers.AddUniqueItem(strUser, ri.IsComputerFolder);
                }
                //También busca el software en el título solo en los pdf, solo lo añade si es software conocido
                if (!String.IsNullOrEmpty(this.FoundMetaData.Title))
                {
                    string strSoftware = Analysis.ApplicationAnalysis.GetApplicationsFromString(this.FoundMetaData.Title);
                    if (strSoftware != string.Empty && !FoundMetaData.Applications.Items.Any(A => A.Name == strSoftware))
                        FoundMetaData.Applications.Items.Add(new ApplicationsItem(strSoftware));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
            finally
            {
                if (doc != null)
                    doc.Dispose();
            }
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
                        System.Xml.XmlDocument xDoc = new System.Xml.XmlDocument();
                        xDoc.XmlResolver = null;
                        xDoc.LoadXml(xmp);
                        #region Metadatos como atributos
                        XmlNodeList xnl = xDoc.GetElementsByTagName("rdf:Description");
                        foreach (XmlNode xn in xnl)
                        {
                            XmlAttribute xa = xn.Attributes["pdf:Creator"];
                            if (xa != null && !string.IsNullOrEmpty(xa.Value))
                            {
                                string strValue = Analysis.ApplicationAnalysis.GetApplicationsFromString(xa.Value);
                                if (strValue.Trim() != string.Empty)
                                {
                                    if (!FoundMetaData.Applications.Items.Any(A => A.Name == strValue.Trim()))
                                        FoundMetaData.Applications.Items.Add(new ApplicationsItem(strValue.Trim()));
                                }
                                //No se ha localizado ninguna aplicación conocida, aun así mostrar la aplicación encontrada
                                else
                                {
                                    if (xa.Value.Trim() != string.Empty && !FoundMetaData.Applications.Items.Any(A => A.Name == xa.Value.Trim()))
                                    {
                                        FoundMetaData.Applications.Items.Add(new ApplicationsItem(xa.Value.Trim()));
                                    }
                                }
                            }
                            xa = xn.Attributes["pdf:CreationDate"];
                            if (xa != null && !string.IsNullOrEmpty(xa.Value))
                            {
                                string strValue = xa.Value;
                                DateTime d;
                                if (DateTime.TryParse(strValue.Replace('T', ' ').Replace('Z', ' '), out d))
                                {
                                    //Si existe una fecha de creación anterior, sobreescribir
                                    if (!FoundDates.CreationDateSpecified || FoundDates.CreationDate > d)
                                    {
                                        FoundDates.CreationDateSpecified = true;
                                        FoundDates.CreationDate = d;
                                    }
                                }
                            }
                            xa = xn.Attributes["pdf:Title"];
                            if (xa != null && !string.IsNullOrEmpty(xa.Value))
                            {
                                string strValue = xa.Value;
                                if (string.IsNullOrEmpty(FoundMetaData.Title) || FoundMetaData.Title.Length < strValue.Length)
                                    FoundMetaData.Title = strValue;
                            }
                            xa = xn.Attributes["pdf:Author"];
                            if (xa != null && !string.IsNullOrEmpty(xa.Value))
                                FoundUsers.AddUniqueItem(xa.Value, true);
                            xa = xn.Attributes["pdf:Producer"];
                            if (xa != null && !string.IsNullOrEmpty(xa.Value))
                            {
                                string strValue = Analysis.ApplicationAnalysis.GetApplicationsFromString(xa.Value);
                                if (strValue.Trim() != string.Empty)
                                {
                                    if (!FoundMetaData.Applications.Items.Any(A => A.Name == strValue.Trim()))
                                        FoundMetaData.Applications.Items.Add(new ApplicationsItem(strValue.Trim()));
                                }
                                //No se ha localizado ninguna aplicación conocida, aun así mostrar la aplicación encontrada
                                else
                                {
                                    if (xa.Value.Trim() != string.Empty && !FoundMetaData.Applications.Items.Any(A => A.Name == xa.Value.Trim()))
                                    {
                                        FoundMetaData.Applications.Items.Add(new ApplicationsItem(xa.Value.Trim()));
                                    }
                                }
                            }
                            xa = xn.Attributes["pdf:ModDate"];
                            if (xa != null && !string.IsNullOrEmpty(xa.Value))
                            {
                                string strValue = xa.Value;
                                DateTime d;
                                if (DateTime.TryParse(strValue.Replace('T', ' ').Replace('Z', ' '), out d))
                                {
                                    FoundDates.ModificationDateSpecified = true;
                                    FoundDates.ModificationDate = d;
                                }
                            }
                            xa = xn.Attributes["xap:CreateDate"];
                            if (xa != null && !string.IsNullOrEmpty(xa.Value))
                            {
                                string strValue = xa.Value;
                                DateTime d;
                                if (DateTime.TryParse(strValue.Replace('T', ' ').Replace('Z', ' '), out d))
                                {
                                    //Si existe una fecha de creación anterior, sobreescribir
                                    if (!FoundDates.CreationDateSpecified || FoundDates.CreationDate > d)
                                    {
                                        //Si existe una fecha de modificación posterior, sobreescribir
                                        if (!FoundDates.ModificationDateSpecified || FoundDates.ModificationDate < d)
                                        {
                                            FoundDates.CreationDateSpecified = true;
                                            FoundDates.CreationDate = d;
                                        }
                                    }
                                }
                            }
                            xa = xn.Attributes["xap:Title"];
                            if (xa != null && !string.IsNullOrEmpty(xa.Value))
                            {
                                string strValue = xa.Value;
                                //Si ya existe un título y es mas pequeño, sobreescribirle.
                                if ((string.IsNullOrEmpty(FoundMetaData.Title) || FoundMetaData.Title.Length < strValue.Length))
                                    FoundMetaData.Title = strValue;
                            }
                            xa = xn.Attributes["xap:Author"];
                            if (xa != null && !string.IsNullOrEmpty(xa.Value))
                                FoundUsers.AddUniqueItem(xa.Value, true);
                            xa = xn.Attributes["xap:ModifyDate"];
                            if (xa != null && !string.IsNullOrEmpty(xa.Value))
                            {
                                string strValue = xa.Value;
                                DateTime d;
                                if (DateTime.TryParse(strValue.Replace('T', ' ').Replace('Z', ' '), out d))
                                {
                                    //Si existe una fecha de modificación posterior, sobreescribir
                                    if (!FoundDates.ModificationDateSpecified || FoundDates.ModificationDate < d)
                                    {
                                        FoundDates.ModificationDateSpecified = true;
                                        FoundDates.ModificationDate = d;
                                    }
                                }
                            }
                            xa = xn.Attributes["xap:CreatorTool"];
                            if (xa != null && !string.IsNullOrEmpty(xa.Value))
                            {
                                string strValue = Analysis.ApplicationAnalysis.GetApplicationsFromString(xa.Value);
                                if (strValue.Trim() != string.Empty)
                                {
                                    if (!FoundMetaData.Applications.Items.Any(A => A.Name == strValue.Trim()))
                                        FoundMetaData.Applications.Items.Add(new ApplicationsItem(strValue.Trim()));
                                }
                                //No se ha localizado ninguna aplicación conocida, aun así mostrar la aplicación encontrada
                                else
                                {
                                    if (xa.Value.Trim() != string.Empty && !FoundMetaData.Applications.Items.Any(A => A.Name == xa.Value.Trim()))
                                    {
                                        FoundMetaData.Applications.Items.Add(new ApplicationsItem(xa.Value.Trim()));
                                    }
                                }
                            }
                            //xap:MetadataDate, fecha en la que se añadieron los metadatos
                            xa = xn.Attributes["dc:title"];
                            if (xa != null && !string.IsNullOrEmpty(xa.Value))
                            {
                                string strValue = xa.Value;
                                //Si ya existe un título y es mas pequeño, sobreescribirle.
                                if (string.IsNullOrEmpty(FoundMetaData.Title) || FoundMetaData.Title.Length < strValue.Length)
                                    FoundMetaData.Title = strValue;
                            }
                            xa = xn.Attributes["dc:creator"];
                            if (xa != null && !string.IsNullOrEmpty(xa.Value))
                            {
                                string strValue = xa.Value;
                                if (!string.IsNullOrEmpty(strValue))
                                    FoundUsers.AddUniqueItem(strValue, true);
                            }
                        }
                        #endregion

                        #region Metadatos como nodos independientes
                        xnl = xDoc.GetElementsByTagName("pdf:Creator");
                        if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
                        {
                            string strValue = Analysis.ApplicationAnalysis.GetApplicationsFromString(xnl[0].FirstChild.Value);
                            if (strValue.Trim() != string.Empty)
                            {
                                if (!FoundMetaData.Applications.Items.Any(A => A.Name == strValue.Trim()))
                                    FoundMetaData.Applications.Items.Add(new ApplicationsItem(strValue.Trim()));
                            }
                            //No se ha localizado ninguna aplicación conocida, aun así mostrar la aplicación encontrada
                            else
                            {
                                if (xnl[0].FirstChild.Value.Trim() != string.Empty && !FoundMetaData.Applications.Items.Any(A => A.Name == xnl[0].FirstChild.Value.Trim()))
                                {
                                    FoundMetaData.Applications.Items.Add(new ApplicationsItem(xnl[0].FirstChild.Value.Trim()));
                                }
                            }
                        }
                        xnl = xDoc.GetElementsByTagName("pdf:CreationDate");
                        if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
                        {
                            string strValue = xnl[0].FirstChild.Value;
                            DateTime d;
                            if (DateTime.TryParse(strValue.Replace('T', ' ').Replace('Z', ' '), out d))
                            {
                                //Si existe una fecha de creación anterior, sobreescribir
                                if (!FoundDates.CreationDateSpecified || FoundDates.CreationDate > d)
                                {
                                    FoundDates.CreationDateSpecified = true;
                                    FoundDates.CreationDate = d;
                                }
                            }
                        }
                        xnl = xDoc.GetElementsByTagName("pdf:Title");
                        if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
                        {
                            string strValue = xnl[0].FirstChild.Value;
                            if ((string.IsNullOrEmpty(FoundMetaData.Title) || FoundMetaData.Title.Length < strValue.Length))
                                FoundMetaData.Title = strValue;
                        }
                        xnl = xDoc.GetElementsByTagName("pdf:Author");
                        if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
                            FoundUsers.AddUniqueItem(xnl[0].FirstChild.Value, true);
                        xnl = xDoc.GetElementsByTagName("pdf:Producer");
                        if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
                        {
                            string strValue = Analysis.ApplicationAnalysis.GetApplicationsFromString(xnl[0].FirstChild.Value);
                            if (strValue.Trim() != string.Empty)
                            {
                                if (!FoundMetaData.Applications.Items.Any(A => A.Name == strValue.Trim()))
                                    FoundMetaData.Applications.Items.Add(new ApplicationsItem(strValue.Trim()));
                            }
                            //No se ha localizado ninguna aplicación conocida, aun así mostrar la aplicación encontrada
                            else
                            {
                                if (xnl[0].FirstChild.Value.Trim() != string.Empty && !FoundMetaData.Applications.Items.Any(A => A.Name == xnl[0].FirstChild.Value.Trim()))
                                {
                                    FoundMetaData.Applications.Items.Add(new ApplicationsItem(xnl[0].FirstChild.Value.Trim()));
                                }
                            }
                        }
                        xnl = xDoc.GetElementsByTagName("pdf:ModDate");
                        if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
                        {
                            string strValue = xnl[0].FirstChild.Value;
                            DateTime d;
                            if (DateTime.TryParse(strValue.Replace('T', ' ').Replace('Z', ' '), out d))
                            {
                                FoundDates.ModificationDateSpecified = true;
                                FoundDates.ModificationDate = d;
                            }
                        }
                        xnl = xDoc.GetElementsByTagName("xap:CreateDate");
                        if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
                        {
                            string strValue = xnl[0].FirstChild.Value;
                            DateTime d;
                            if (DateTime.TryParse(strValue.Replace('T', ' ').Replace('Z', ' '), out d))
                            {
                                //Si existe una fecha de creación anterior, sobreescribir
                                if (!FoundDates.CreationDateSpecified || FoundDates.CreationDate > d)
                                {
                                    //Si existe una fecha de modificación posterior, sobreescribir
                                    if (!FoundDates.ModificationDateSpecified || FoundDates.ModificationDate < d)
                                    {
                                        FoundDates.CreationDateSpecified = true;
                                        FoundDates.CreationDate = d;
                                    }
                                }
                            }
                        }
                        xnl = xDoc.GetElementsByTagName("xap:Title");
                        if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                        {
                            XmlNode xn = xnl[0].FirstChild;
                            //Busca el primer subnodo con valor
                            while (xn.Value == null && xn.HasChildNodes)
                                xn = xn.FirstChild;
                            if (!string.IsNullOrEmpty(xn.Value))
                            {
                                string strValue = xn.Value;
                                //Si ya existe un título y es mas pequeño, sobreescribirle.
                                if ((string.IsNullOrEmpty(FoundMetaData.Title) || FoundMetaData.Title.Length < strValue.Length))
                                    FoundMetaData.Title = strValue;
                            }
                        }
                        xnl = xDoc.GetElementsByTagName("xap:Author");
                        if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
                        {
                            FoundUsers.AddUniqueItem(xnl[0].FirstChild.Value, true);
                        }
                        xnl = xDoc.GetElementsByTagName("xap:ModifyDate");
                        if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
                        {
                            string strValue = xnl[0].FirstChild.Value;
                            DateTime d;
                            if (DateTime.TryParse(strValue.Replace('T', ' ').Replace('Z', ' '), out d))
                            {
                                //Si existe una fecha de modificación posterior, sobreescribir
                                if (!FoundDates.ModificationDateSpecified || FoundDates.ModificationDate < d)
                                {
                                    FoundDates.ModificationDateSpecified = true;
                                    FoundDates.ModificationDate = d;
                                }
                            }
                        }
                        xnl = xDoc.GetElementsByTagName("xap:CreatorTool");
                        if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
                        {
                            string strValue = Analysis.ApplicationAnalysis.GetApplicationsFromString(xnl[0].FirstChild.Value);
                            if (strValue.Trim() != string.Empty)
                            {
                                if (!FoundMetaData.Applications.Items.Any(A => A.Name == strValue.Trim()))
                                    FoundMetaData.Applications.Items.Add(new ApplicationsItem(strValue.Trim()));
                            }
                            //No se ha localizado ninguna aplicación conocida, aun así mostrar la aplicación encontrada
                            else
                            {
                                if (xnl[0].FirstChild.Value.Trim() != string.Empty && !FoundMetaData.Applications.Items.Any(A => A.Name == xnl[0].FirstChild.Value.Trim()))
                                {
                                    FoundMetaData.Applications.Items.Add(new ApplicationsItem(xnl[0].FirstChild.Value.Trim()));
                                }
                            }
                        }
                        //xap:MetadataDate, fecha en la que se añadieron los metadatos
                        xnl = xDoc.GetElementsByTagName("dc:title");
                        if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                        {
                            XmlNode xn = xnl[0].FirstChild;
                            //Busca el primer subnodo con valor
                            while (xn.Value == null && xn.HasChildNodes)
                                xn = xn.FirstChild;
                            if (!string.IsNullOrEmpty(xn.Value))
                            {
                                string strValue = xn.Value;
                                //Si ya existe un título y es mas pequeño, sobreescribirle.
                                if ((string.IsNullOrEmpty(FoundMetaData.Title) || FoundMetaData.Title.Length < strValue.Length))
                                    FoundMetaData.Title = strValue;
                            }
                        }

                        //if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
                        //FoundUsers.AddUniqueItem(xnl[0].FirstChild.Value, true);
                        #endregion
                    }
                }
            }
        }
        
        private void BinarySearchPaths(Stream stm)
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
                FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(path), true);
            }
        }

        private void BinarySearchLinks(Stream stm)
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
                    FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(link), true);//false);
                }
            }
        }

        /// <summary>
        /// Devuelve true si el link es interesante ya que contiene una IP o una ruta, el resto de links se desprecian
        /// </summary>
        /// <returns></returns>
        private bool IsInterestingLink(string href)
        {
            if (href != string.Empty)
            {
                if (href.StartsWith("mailto:"))
                {
                    String email = href.Substring(7, (href.Contains("?") ? href.IndexOf('?') : href.Length) - 7);
                    FoundEmails.AddUniqueItem(email);
                }
                else if (href.StartsWith("ftp:"))
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
                else
                {
                    try
                    {
                        Uri u = new Uri(href);
                        if (u.HostNameType != UriHostNameType.Dns)
                        {
                            return true;
                        }
                    }
                    catch (UriFormatException)  //No es una URI, será un path interno...
                    {
                        if (!href.StartsWith("#"))  //Se omiten referencias del tipo #Pais
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}
