using Ionic.Zip;
using MetadataExtractCore.Analysis;
using MetadataExtractCore.Diagrams;
using MetadataExtractCore.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace MetadataExtractCore.Extractors
{
    public class OpenOfficeDocument : DocumentExtractor
    {
        private string strExtlo;

        /// <summary>
        /// Constructor de la clase OpenOfficeDocument
        /// </summary>
        /// <param name="stm">Stream que contiene el fichero de OpenOffice</param>
        public OpenOfficeDocument(Stream stm, string strExt) : base(stm)
        {
            strExtlo = strExt.ToLower();
        }

        /// <summary>
        /// Extrae los metadatos del documento
        /// </summary>
        public override FileMetadata AnalyzeFile()
        {
            try
            {
                this.foundMetadata = new FileMetadata();
                using (ZipFile zip = ZipFile.Read(this.fileStream))
                {
                    string strFile = "meta.xml";
                    if (zip.EntryFileNames.Contains(strFile))
                    {
                        using (Stream stmXML = new MemoryStream())
                        {
                            zip.Extract(strFile, stmXML);
                            stmXML.Seek(0, SeekOrigin.Begin);
                            AnalizeFileMeta(stmXML);
                        }
                    }
                    strFile = "settings.xml";
                    if (zip.EntryFileNames.Contains(strFile))
                    {
                        using (Stream stmXML = new MemoryStream())
                        {
                            zip.Extract(strFile, stmXML);
                            stmXML.Seek(0, SeekOrigin.Begin);
                            analizeFileSettings(stmXML);
                        }
                    }
                    strFile = "content.xml";
                    if (zip.EntryFileNames.Contains(strFile))
                    {
                        using (Stream stmXML = new MemoryStream())
                        {
                            zip.Extract(strFile, stmXML);
                            stmXML.Seek(0, SeekOrigin.Begin);
                            AnalizeFileContent(stmXML);
                        }
                    }
                    strFile = "VersionList.xml";
                    if (zip.EntryFileNames.Contains(strFile))
                    {
                        using (Stream stmXML = new MemoryStream())
                        {
                            zip.Extract(strFile, stmXML);
                            stmXML.Seek(0, SeekOrigin.Begin);
                            AnalizeFileVersionList(stmXML, zip);
                        }
                    }
                    //Extrae inforamción EXIF de las imágenes embebidas en el documento
                    foreach (string strFileName in zip.EntryFileNames)
                    {
                        string strFileNameLo = strFileName.ToLower();
                        //Filtro que obtiene las imagenes *.jpg, *.jpeg dentro de la carpeta "Pictures/"
                        if (strFileNameLo.StartsWith("pictures/") &&
                            (strFileNameLo.EndsWith(".jpg") || strFileNameLo.EndsWith(".jpeg") || strFileNameLo.EndsWith(".png")))
                        {
                            using (Stream stmXML = new MemoryStream())
                            {
                                zip.Extract(strFileName, stmXML);
                                stmXML.Seek(0, SeekOrigin.Begin);
                                using (EXIFDocument eDoc = new EXIFDocument(stmXML))
                                {
                                    FileMetadata exifMetadata = eDoc.AnalyzeFile();
                                    //Añadimos al diccionario la imagen encontrada junto con la información EXIF de la misma
                                    this.foundMetadata.EmbeddedImages.Add(System.IO.Path.GetFileName(strFileName), exifMetadata);
                                    //Los usuarios de la información EXIF se añaden a los usuarios del documento
                                    this.foundMetadata.AddRange(exifMetadata.Users.ToArray());
                                    this.foundMetadata.AddRange(exifMetadata.Applications.ToArray());
                                }
                            }
                        }
                    }
                }
                //Buscamos usuarios en las rutas del documento
                foreach (Diagrams.Path ri in this.foundMetadata.Paths)
                {
                    string strUser = PathAnalysis.ExtractUserFromPath(ri.Value);
                    if (!string.IsNullOrEmpty(strUser))
                        this.foundMetadata.Add(new User(strUser, ri.IsComputerFolder, "Path: " + ri.Value));
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Error analyzing OpenOffice document ({0})", e.ToString()));
            }
            return this.foundMetadata;
        }

        private void AnalizeFileMeta(Stream stm)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null;
                doc.Load(stm);
                XmlNodeList xnl = doc.GetElementsByTagName("meta:generator");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                {
                    if (strExtlo == ".odt" ||
                        strExtlo == ".ods" ||
                        strExtlo == ".odg" ||
                        strExtlo == ".odp")
                    {
                        if (xnl[0].FirstChild.Value.IndexOf('$') != -1 && xnl[0].FirstChild.Value.IndexOf(' ') != -1 &&
                            xnl[0].FirstChild.Value.IndexOf(' ') > xnl[0].FirstChild.Value.IndexOf('$'))
                        {
                            string strSoftware = xnl[0].FirstChild.Value.Remove(xnl[0].FirstChild.Value.IndexOf('$')) + " - " + xnl[0].FirstChild.Value.Substring(xnl[0].FirstChild.Value.IndexOf(' ') + 1, xnl[0].FirstChild.Value.Length - xnl[0].FirstChild.Value.IndexOf(' ') - 1);
                            this.foundMetadata.Add(new Application(Analysis.ApplicationAnalysis.GetApplicationsFromString(strSoftware)));
                            this.foundMetadata.OperatingSystem = xnl[0].FirstChild.Value.Substring(xnl[0].FirstChild.Value.IndexOf('$') + 1, xnl[0].FirstChild.Value.IndexOf(' ') - xnl[0].FirstChild.Value.IndexOf('$')).Trim();
                        }
                        else
                            this.foundMetadata.Add(new Application(Analysis.ApplicationAnalysis.GetApplicationsFromString(xnl[0].FirstChild.Value)));
                    }
                    else if (strExtlo == ".sxw")
                    {
                        if (xnl[0].FirstChild.Value.IndexOf(')') != -1 && xnl[0].FirstChild.Value.IndexOf('(') != -1 &&
                            xnl[0].FirstChild.Value.IndexOf(')') > xnl[0].FirstChild.Value.IndexOf('('))
                        {
                            string strSoftware = xnl[0].FirstChild.Value.Remove(xnl[0].FirstChild.Value.IndexOf('('));
                            this.foundMetadata.Add(new Application(Analysis.ApplicationAnalysis.GetApplicationsFromString(strSoftware)));
                            this.foundMetadata.OperatingSystem = xnl[0].FirstChild.Value.Substring(xnl[0].FirstChild.Value.IndexOf('(') + 1, xnl[0].FirstChild.Value.IndexOf(')') - xnl[0].FirstChild.Value.IndexOf('(') - 1);
                        }
                        else
                            this.foundMetadata.Add(new Application(Analysis.ApplicationAnalysis.GetApplicationsFromString(xnl[0].FirstChild.Value)));
                    }
                }
                xnl = doc.GetElementsByTagName("dc:creator");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                    this.foundMetadata.Add(new User(xnl[0].FirstChild.Value, true, "dc:creator"));
                xnl = doc.GetElementsByTagName("meta:printed-by");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                    this.foundMetadata.Add(new User(xnl[0].FirstChild.Value, this.foundMetadata.Users.Count == 0, "meta:printed-by"));
                xnl = doc.GetElementsByTagName("dc:initial-creator");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                    this.foundMetadata.Add(new User(xnl[0].FirstChild.Value, this.foundMetadata.Users.Count == 0, "dc:initial-creator"));
                xnl = doc.GetElementsByTagName("meta:initial-creator");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                    this.foundMetadata.Add(new User(xnl[0].FirstChild.Value, this.foundMetadata.Users.Count == 0, "meta:initial-creator"));
                xnl = doc.GetElementsByTagName("meta:creation-date");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                {
                    DateTime d;
                    if (DateTime.TryParse(xnl[0].FirstChild.Value.Replace('T', ' '), out d))
                    {
                        this.foundMetadata.Dates.CreationDate = d;
                    }
                }
                xnl = doc.GetElementsByTagName("meta:date");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                {
                    DateTime d;
                    if (DateTime.TryParse(xnl[0].FirstChild.Value.Replace('T', ' '), out d))
                    {
                        this.foundMetadata.Dates.ModificationDate = d;
                    }
                }
                xnl = doc.GetElementsByTagName("dc:date");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                {
                    DateTime d;
                    if (DateTime.TryParse(xnl[0].FirstChild.Value.Replace('T', ' '), out d))
                    {
                        this.foundMetadata.Dates.ModificationDate = d;
                    }
                }
                xnl = doc.GetElementsByTagName("meta:print-date");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                {
                    DateTime d;
                    if (DateTime.TryParse(xnl[0].FirstChild.Value.Replace('T', ' '), out d))
                    {
                        this.foundMetadata.Dates.PrintingDate = d;
                    }
                }
                xnl = doc.GetElementsByTagName("dc:language");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                    this.foundMetadata.Language = xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("dc:title");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                {
                    this.foundMetadata.Title = xnl[0].FirstChild.Value;
                    //Si el título es una ruta válida, agregar como una ruta del equipo
                    this.foundMetadata.Add(new Diagrams.Path(PathAnalysis.CleanPath(this.foundMetadata.Title), true));
                }
                xnl = doc.GetElementsByTagName("dc:subject");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                    this.foundMetadata.Subject = xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("dc:description");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                    this.foundMetadata.Description = xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("meta:keyword");
                if (xnl != null && xnl.Count != 0)
                {
                    String keyWords = string.Empty;
                    foreach (XmlNode xn in xnl)
                        if (xn.HasChildNodes)
                            keyWords += xn.FirstChild.Value + " ";
                    this.foundMetadata.Keywords = keyWords;
                }
                xnl = doc.GetElementsByTagName("meta:editing-cycles");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                {
                    Decimal ediciones;
                    if (Decimal.TryParse(xnl[0].FirstChild.Value, out ediciones))
                        this.foundMetadata.VersionNumber = ediciones;
                }
                xnl = doc.GetElementsByTagName("meta:editing-duration");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                {
                    DateTime d;
                    if (DateTime.TryParse(xnl[0].FirstChild.Value.Replace('T', ' ').Replace('P', ' '), out d))
                        this.foundMetadata.EditTime = d.Ticks;
                }
                xnl = doc.GetElementsByTagName("meta:user-defined");
                if (xnl != null && xnl.Count != 0)
                {
                    String Info = string.Empty;
                    foreach (XmlNode xn in xnl)
                        if (xn.HasChildNodes)
                            Info += xn.Attributes.GetNamedItem("meta:name").Value + ": " + xn.FirstChild.Value + "|";
                    if (Info != string.Empty)
                        this.foundMetadata.UserInfo = Info;
                }
                xnl = doc.GetElementsByTagName("meta:template");
                if (xnl != null && xnl.Count != 0)
                    foreach (XmlNode xn in xnl)
                        this.foundMetadata.Add(new Diagrams.Path(PathAnalysis.CleanPath(xn.Attributes.GetNamedItem("xlink:href").Value), true));
                xnl = doc.GetElementsByTagName("meta:document-statistic");
                if (xnl != null && xnl.Count > 0)
                {
                    StringBuilder statisticBuilder = new StringBuilder();
                    if (xnl[0].Attributes.GetNamedItem("meta:table-count") != null)
                        statisticBuilder.Append("  Tables: " + xnl[0].Attributes.GetNamedItem("meta:table-count").Value);
                    if (xnl[0].Attributes.GetNamedItem("meta:cell-count") != null)
                        statisticBuilder.Append("  Cell: " + xnl[0].Attributes.GetNamedItem("meta:cell-count").Value);
                    if (xnl[0].Attributes.GetNamedItem("meta:image-count") != null)
                        statisticBuilder.Append("  Images: " + xnl[0].Attributes.GetNamedItem("meta:image-count").Value);
                    if (xnl[0].Attributes.GetNamedItem("meta:object-count") != null)
                        statisticBuilder.Append("  Objects: " + xnl[0].Attributes.GetNamedItem("meta:object-count").Value);
                    if (xnl[0].Attributes.GetNamedItem("meta:page-count") != null)
                        statisticBuilder.Append("  Pages: " + xnl[0].Attributes.GetNamedItem("meta:page-count").Value);
                    if (xnl[0].Attributes.GetNamedItem("meta:paragraph-count") != null)
                        statisticBuilder.Append("  Paragraph: " + xnl[0].Attributes.GetNamedItem("meta:paragraph-count").Value);
                    if (xnl[0].Attributes.GetNamedItem("meta:word-count") != null)
                        statisticBuilder.Append("  Words: " + xnl[0].Attributes.GetNamedItem("meta:word-count").Value);
                    if (xnl[0].Attributes.GetNamedItem("meta:character-count") != null)
                        statisticBuilder.Append("  Characters: " + xnl[0].Attributes.GetNamedItem("meta:character-count").Value);
                    if (statisticBuilder.Length > 0)
                        this.foundMetadata.Statistic = statisticBuilder.ToString().Trim();
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Error reading file meta.xml ({0}).", e.ToString()));
            }
        }

        private void analizeFileSettings(Stream stm)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null;
                doc.Load(stm);
                XmlNodeList xnl = doc.GetElementsByTagName("config:config-item");
                if (xnl != null)
                {
                    foreach (XmlNode xn in xnl)
                    {
                        if (xn.Attributes.GetNamedItem("config:name").Value == "PrinterName" && xn.HasChildNodes)
                        {
                            this.foundMetadata.Add(new Printer(Functions.FilterPrinter(xn.FirstChild.Value)));
                        }
                        else if (xn.Attributes.GetNamedItem("config:name").Value == "CurrentDatabaseDataSource" && xn.HasChildNodes)
                        {
                            this.foundMetadata.DataBase = xn.FirstChild.Value;
                        }
                        /* else if (xn.Attributes.GetNamedItem("config:name").Value == "PrintFaxName")
                         {
                             if (xn.HasChildNodes)
                                 escritor("Fax: " + xn.FirstChild.Value, objeto);
                         }
                         else if (xn.Attributes.GetNamedItem("config:name").Value == "CurrentDatabaseCommandType")
                         {
                             if (!resumen)
                                 if (xn.HasChildNodes && xn.FirstChild.Value != "0")
                                     escritor("Current Database Command Type: " + xn.FirstChild.Value, objeto);
                         }
                         else if (xn.Attributes.GetNamedItem("config:name").Value == "CurrentDatabaseCommand")
                         {
                             if (!resumen)
                                 if (xn.HasChildNodes)
                                     escritor("Current Database Command: " + xn.FirstChild.Value, objeto);
                         }*/
                        //Solo aparecen en ficheros ODP y ODG file:///
                        else if (xn.Attributes.GetNamedItem("config:name").Value == "ColorTableURL" && xn.HasChildNodes)
                        {
                            if (PathAnalysis.CleanPath(xn.FirstChild.Value) != "$(user)/config/")
                                this.foundMetadata.Add(new Diagrams.Path(PathAnalysis.CleanPath(xn.FirstChild.Value), true));
                        }
                        else if (xn.Attributes.GetNamedItem("config:name").Value == "BitmapTableURL" && xn.HasChildNodes)
                        {
                            if (PathAnalysis.CleanPath(xn.FirstChild.Value) != "$(user)/config/")
                                this.foundMetadata.Add(new Diagrams.Path(PathAnalysis.CleanPath(xn.FirstChild.Value), true));
                        }
                        else if (xn.Attributes.GetNamedItem("config:name").Value == "DashTableURL" && xn.HasChildNodes)
                        {
                            if (PathAnalysis.CleanPath(xn.FirstChild.Value) != "$(user)/config/")
                                this.foundMetadata.Add(new Diagrams.Path(PathAnalysis.CleanPath(xn.FirstChild.Value), true));
                        }
                        else if (xn.Attributes.GetNamedItem("config:name").Value == "GradientTableURL" && xn.HasChildNodes)
                        {
                            if (PathAnalysis.CleanPath(xn.FirstChild.Value) != "$(user)/config/")
                                this.foundMetadata.Add(new Diagrams.Path(PathAnalysis.CleanPath(xn.FirstChild.Value), true));
                        }
                        else if (xn.Attributes.GetNamedItem("config:name").Value == "HatchTableURL" && xn.HasChildNodes)
                        {
                            if (PathAnalysis.CleanPath(xn.FirstChild.Value) != "$(user)/config/")
                                this.foundMetadata.Add(new Diagrams.Path(PathAnalysis.CleanPath(xn.FirstChild.Value), true));
                        }
                        else if (xn.Attributes.GetNamedItem("config:name").Value == "LineEndTableURL" && xn.HasChildNodes)
                        {
                            if (PathAnalysis.CleanPath(xn.FirstChild.Value) != "$(user)/config/")
                                this.foundMetadata.Add(new Diagrams.Path(PathAnalysis.CleanPath(xn.FirstChild.Value), true));
                        }
                    }

                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Error reading file settings.xml ({0}).", e.ToString()));
            }
        }

        private void AnalizeFileContent(Stream stm)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null;
                doc.Load(stm);
                XmlNodeList xnl = doc.GetElementsByTagName("text:a");
                if (xnl != null)
                {
                    List<String> links = new List<String>();
                    foreach (XmlNode xn in xnl)
                    {
                        String href = xn.Attributes.GetNamedItem("xlink:href").Value;
                        if (href != string.Empty && this.IsInterestingLink(href))
                        {
                            links.Add(href);
                        }
                    }
                    if (links.Count != 0)
                    {
                        foreach (String link in links)
                        {
                            this.foundMetadata.Add(new Diagrams.Path(PathAnalysis.CleanPath(link), true));//false);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Error reading file content.xml ({0}).", e.ToString()));
            }
        }

        /// <summary>
        /// Analiza las versiones antiguas de un documento OpenOffice
        /// </summary>
        /// <param name="stm">Stream que contiene el fichero VersionList.xml</param>
        /// <param name="zip">Fichero zip que contiene las versiones antiguas embebidas</param>
        private void AnalizeFileVersionList(Stream stm, ZipFile zip)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null;
                doc.Load(stm);
                XmlNodeList xml = doc.GetElementsByTagName("VL:version-entry");
                if (xml != null && xml.Count != 0)
                {
                    foreach (XmlNode xn in xml)
                    {
                        OldVersion vai = new OldVersion(xn.Attributes["VL:title"].Value, xn.Attributes["VL:creator"].Value, xn.Attributes["VL:comment"].Value);
                        //Añadimos el usuario de la versión antigua a los usuarios del documento
                        this.foundMetadata.Add(new User(vai.Author, false, "VL:creator"));
                        DateTime d;
                        if (DateTime.TryParse(xn.Attributes["dc:date-time"].Value.Replace('T', ' '), out d))
                        {
                            vai.Date = d;
                        }
                        String strFile = "Versions/" + xn.Attributes["VL:title"].Value;
                        if (zip.EntryFileNames.Contains(strFile))
                        {
                            using (Stream stmXML = new MemoryStream())
                            {
                                zip.Extract(strFile, stmXML);
                                stmXML.Seek(0, SeekOrigin.Begin);
                                using (OpenOfficeDocument ooDoc = new OpenOfficeDocument(stmXML, strExtlo))
                                {
                                    vai.Metadata = ooDoc.AnalyzeFile();
                                }
                            }
                        }
                        this.foundMetadata.Add(vai);
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Error reading file content.xml ({0}).", e.ToString()));
            }
        }
    }
}
