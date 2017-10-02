using System;
using System.Linq;
using System.IO;
using MetadataExtractCore.Utilities;
using System.Text.RegularExpressions;
using MetadataExtractCore.Analysis;
using System.Xml;
using MetadataExtractCore.Diagrams;

namespace MetadataExtractCore.Metadata
{
    [Serializable]
    public class InDDDocument : MetaExtractor
    {
        public InDDDocument()
        {
        }

        public InDDDocument(Stream stm)
        {
            this.stm = new MemoryStream();
            Functions.CopyStream(stm, this.stm);
        }

        /// <summary>
        /// Extract metadata.
        /// </summary>
        public override void analyzeFile()
        {
            try
            {
                using (StreamReader sr = new StreamReader(stm))
                {
                    String sRead = sr.ReadToEnd();
                    foreach (Match m in Regex.Matches(sRead, @"@([a-z]:|\\)\\(([a-z0-9\s\-_\$&()ñÇ/n/r]+)\\)*[a-z0-9\s,;.\-_\$%&()=ñ{}Ç/n/r+@]+", RegexOptions.IgnoreCase))
                    {
                        String path = m.Value.Trim();
                        path = path.Substring(1);
                        FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(path), true);
                    }
                    foreach (Match m in Regex.Matches(sRead, @"winspool\0([a-z]:|\\)\\(([a-z0-9\s\-_\$&()ñÇ/n/r]+)\\)*[a-z0-9\s,;.\-_\$%&()=ñ{}Ç/n/r+@]+", RegexOptions.IgnoreCase))
                    {
                        String printer = m.Value.Trim();
                        printer = printer.Substring(9);
                        FoundPrinters.AddUniqueItem(Functions.FilterPrinter(printer));
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
            finally
            {
                this.stm.Close();
                this.stm = null;
            }
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
                #region Metadatos como atributos
                XmlNodeList xnl = xDoc.GetElementsByTagName("rdf:Description");
                /*foreach (XmlNode xn in xnl)
                {
                    XmlAttribute xa;
                    /*xa= xn.Attributes["pdf:Creator"];
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
                    }*/
                    /*xa = xn.["xap:MetadataDate"];
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
                    xa = xn.Attributes["xap:ModifyDate"];
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
                    /*xa = xn.Attributes["pdf:Title"];
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
                }*/
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
                            //Si existe una fecha de modificación posterior, sobreescribir
                            if (!FoundDates.ModificationDateSpecified || FoundDates.ModificationDate < d)
                            {
                                FoundDates.CreationDateSpecified = true;
                                FoundDates.CreationDate = d;
                            }
                        }
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
                xnl = xDoc.GetElementsByTagName("xap:MetadataDate");
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
                    FoundUsers.AddUniqueItem(xnl[0].FirstChild.Value, true, "xap:Author");
                }
                xnl = xDoc.GetElementsByTagName("pdf:ModDate");
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
                xnl = xDoc.GetElementsByTagName("dc:creator");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                {
                    XmlNode xn = xnl[0].FirstChild;
                    //Busca el primer subnodo con valor
                    while (xn.Value == null && xn.HasChildNodes)
                        xn = xn.FirstChild;
                    if (!string.IsNullOrEmpty(xn.Value))
                    {
                        string strValue = xn.Value;
                        FoundUsers.AddUniqueItem(strValue, true);
                    }
                }
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
                xnl = xDoc.GetElementsByTagName("stRef:lastURL");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes && !string.IsNullOrEmpty(xnl[0].FirstChild.Value))
                {
                    FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(xnl[0].FirstChild.Value), true);
                }
                #endregion
            }
        }
    }
}
