using System;
using System.Linq;
using System.IO;
using System.Xml;
using Ionic.Zip;
using System.Collections.Generic;
using MetadataExtractCore.Utilities;
using MetadataExtractCore.Analysis;
using MetadataExtractCore.Diagrams;

namespace MetadataExtractCore.Metadata
{
    [Serializable]
    public class OpenOfficeDocument : MetaExtractor
    {
        private string strExtlo;
        //Un diccionario que contiene los metadatos de cada versión antigua, encontrada en este documento.
        public SerializableDictionary<string, OpenOfficeDocument> dicOldVersions { get; set; }

        //Un diccionario con cada imagen encontrada en el documento
        public SerializableDictionary<string, EXIFDocument> dicPictureEXIF { get; set; }

        /// <summary>
        /// Lo pide la serialización
        /// </summary>
        public OpenOfficeDocument()
        {
            dicOldVersions = new SerializableDictionary<string, OpenOfficeDocument>();
            dicPictureEXIF = new SerializableDictionary<string, EXIFDocument>();
        }

        /// <summary>
        /// Constructor de la clase OpenOfficeDocument
        /// </summary>
        /// <param name="stm">Stream que contiene el fichero de OpenOffice</param>
        public OpenOfficeDocument(Stream stm, string strExt): this()
        {
            this.stm = new MemoryStream();
            Functions.CopyStream(stm, this.stm);
            strExtlo = strExt.ToLower();
        }

        /// <summary>
        /// Extrae los metadatos del documento
        /// </summary>
        public override void analyzeFile()
        {
            try
            {
                using (ZipFile zip = ZipFile.Read(stm))
                {
                    string strFile = "meta.xml";
                    if (zip.EntryFileNames.Contains(strFile))
                    {
                        using (Stream stmXML = new MemoryStream())
                        {
                            zip.Extract(strFile, stmXML);
                            stmXML.Seek(0, SeekOrigin.Begin);
                            analizeFileMeta(stmXML);
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
                            analizeFileContent(stmXML);
                        }
                    }
                    strFile = "VersionList.xml";
                    if (zip.EntryFileNames.Contains(strFile))
                    {
                        using (Stream stmXML = new MemoryStream())
                        {
                            zip.Extract(strFile, stmXML);
                            stmXML.Seek(0, SeekOrigin.Begin);
                            analizeFileVersionList(stmXML, zip);
                        }
                    }
                    //Extrae inforamción EXIF de las imágenes embebidas en el documento
                    foreach (string strFileName in zip.EntryFileNames)
                    {
                        string strFileNameLo = strFileName.ToLower();
                        //Filtro que obtiene las imagenes *.jpg, *.jpeg dentro de la carpeta "Pictures/"
                        if (strFileNameLo.StartsWith("pictures/") &&
                            (strFileNameLo.EndsWith(".jpg") || strFileNameLo.EndsWith(".jpeg")))
                        {
                            using (Stream stmXML = new MemoryStream())
                            {
                                zip.Extract(strFileName, stmXML);
                                stmXML.Seek(0, SeekOrigin.Begin);
                                EXIFDocument eDoc = new EXIFDocument(stmXML, Path.GetExtension(strFileNameLo));
                                eDoc.analyzeFile();
                                //Añadimos al diccionario la imagen encontrada junto con la información EXIF de la misma
                                dicPictureEXIF.Add(Path.GetFileName(strFileName), eDoc);
                                //Los usuarios de la información EXIF se añaden a los usuarios del documento
                                foreach (UserItem uiEXIF in eDoc.FoundUsers.Items)
                                    FoundUsers.AddUniqueItem(uiEXIF.Name, false, "EXIF");
                                //Añadir el software encontrado en la información EXIF al software usado para generar el documento
                                foreach (ApplicationsItem Application in eDoc.FoundMetaData.Applications.Items)
                                {
                                    string strApplication = Application.Name;
                                    if (!FoundMetaData.Applications.Items.Any(A => A.Name == strApplication.Trim()))
                                        FoundMetaData.Applications.Items.Add(new ApplicationsItem(strApplication.Trim()));
                                }
                            }
                        }
                    }
                }
                //Buscamos usuarios en las rutas del documento
                foreach (PathsItem ri in FoundPaths.Items)
                {
                    string strUser = PathAnalysis.ExtractUserFromPath(ri.Path);
                    if (!string.IsNullOrEmpty(strUser))
                        FoundUsers.AddUniqueItem(strUser, ri.IsComputerFolder, "Path: " + ri.Path);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Error analizing OpenOffice document ({0})", e.ToString()));
            }
        }

        private void analizeFileMeta(Stream stm)
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
                            this.FoundMetaData.Applications.Items.Add(new ApplicationsItem(Analysis.ApplicationAnalysis.GetApplicationsFromString(strSoftware)));
                            this.FoundMetaData.OperativeSystem = xnl[0].FirstChild.Value.Substring(xnl[0].FirstChild.Value.IndexOf('$') + 1, xnl[0].FirstChild.Value.IndexOf(' ') - xnl[0].FirstChild.Value.IndexOf('$'));
                        }
                        else
                            this.FoundMetaData.Applications.Items.Add(new ApplicationsItem(Analysis.ApplicationAnalysis.GetApplicationsFromString(xnl[0].FirstChild.Value)));
                    }
                    else if (strExtlo == ".sxw")
                    {
                        if (xnl[0].FirstChild.Value.IndexOf(')') != -1 && xnl[0].FirstChild.Value.IndexOf('(') != -1 &&
                            xnl[0].FirstChild.Value.IndexOf(')') > xnl[0].FirstChild.Value.IndexOf('('))
                        {
                            string strSoftware = xnl[0].FirstChild.Value.Remove(xnl[0].FirstChild.Value.IndexOf('('));
                            this.FoundMetaData.Applications.Items.Add(new ApplicationsItem(Analysis.ApplicationAnalysis.GetApplicationsFromString(strSoftware)));
                            this.FoundMetaData.OperativeSystem = xnl[0].FirstChild.Value.Substring(xnl[0].FirstChild.Value.IndexOf('(') + 1, xnl[0].FirstChild.Value.IndexOf(')') - xnl[0].FirstChild.Value.IndexOf('(') - 1);
                        }
                        else
                            this.FoundMetaData.Applications.Items.Add(new ApplicationsItem(Analysis.ApplicationAnalysis.GetApplicationsFromString(xnl[0].FirstChild.Value)));
                    }
                }
                xnl = doc.GetElementsByTagName("dc:creator");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                    FoundUsers.AddUniqueItem(xnl[0].FirstChild.Value, true, "dc:creator");
                xnl = doc.GetElementsByTagName("meta:printed-by");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                    FoundUsers.AddUniqueItem(xnl[0].FirstChild.Value, FoundUsers.Items.Count == 0, "meta:printed-by");
                xnl = doc.GetElementsByTagName("dc:initial-creator");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                    FoundUsers.AddUniqueItem(xnl[0].FirstChild.Value, FoundUsers.Items.Count == 0, "dc:initial-creator");
                xnl = doc.GetElementsByTagName("meta:initial-creator");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                    FoundUsers.AddUniqueItem(xnl[0].FirstChild.Value, FoundUsers.Items.Count == 0, "meta:initial-creator");
                xnl = doc.GetElementsByTagName("meta:creation-date");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                {
                    DateTime d;
                    if (DateTime.TryParse(xnl[0].FirstChild.Value.Replace('T', ' '), out d))
                    {
                        FoundDates.CreationDateSpecified = true;
                        FoundDates.CreationDate = d;
                    }
                }
                xnl = doc.GetElementsByTagName("meta:date");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                {
                    DateTime d;
                    if (DateTime.TryParse(xnl[0].FirstChild.Value.Replace('T', ' '), out d))
                    {
                        FoundDates.ModificationDateSpecified = true;
                        FoundDates.ModificationDate = d;
                    }
                }
                xnl = doc.GetElementsByTagName("dc:date");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                {
                    DateTime d;
                    if (DateTime.TryParse(xnl[0].FirstChild.Value.Replace('T', ' '), out d))
                    {
                        FoundDates.ModificationDateSpecified = true;
                        FoundDates.ModificationDate = d;
                    }
                }
                xnl = doc.GetElementsByTagName("meta:print-date");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                {
                    DateTime d;
                    if (DateTime.TryParse(xnl[0].FirstChild.Value.Replace('T', ' '), out d))
                    {
                        FoundDates.DatePrintingSpecified = true;
                        FoundDates.DatePrinting = d;
                    }
                }
                xnl = doc.GetElementsByTagName("dc:language");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                    FoundMetaData.Language = xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("dc:title");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                {
                    FoundMetaData.Title = xnl[0].FirstChild.Value;
                    //Si el título es una ruta válida, agregar como una ruta del equipo
                    FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(FoundMetaData.Title), true);
                }
                xnl = doc.GetElementsByTagName("dc:subject");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                    FoundMetaData.Subject = xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("dc:description");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                    FoundMetaData.Description = xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("meta:keyword");
                if (xnl != null && xnl.Count != 0)
                {
                    String keyWords = string.Empty;
                    foreach (XmlNode xn in xnl)
                        if (xn.HasChildNodes)
                            keyWords += xn.FirstChild.Value + " ";
                    FoundMetaData.Keywords = keyWords;
                }
                xnl = doc.GetElementsByTagName("meta:editing-cycles");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                {
                    Decimal ediciones;
                    if (Decimal.TryParse(xnl[0].FirstChild.Value, out ediciones))
                        FoundMetaData.VersionNumber = ediciones;
                }
                xnl = doc.GetElementsByTagName("meta:editing-duration");
                if (xnl != null && xnl.Count != 0 && xnl[0].HasChildNodes)
                {
                    DateTime d;
                    if (DateTime.TryParse(xnl[0].FirstChild.Value.Replace('T', ' ').Replace('P', ' '), out d))
                        FoundMetaData.EditTime = d.Ticks;
                }
                xnl = doc.GetElementsByTagName("meta:user-defined");
                if (xnl != null && xnl.Count != 0)
                {
                    String Info = string.Empty;
                    foreach (XmlNode xn in xnl)
                        if (xn.HasChildNodes)
                            Info += xn.Attributes.GetNamedItem("meta:name").Value + ": " + xn.FirstChild.Value + "|";
                    if (Info != string.Empty)
                        FoundMetaData.UserInfo = Info;
                }
                xnl = doc.GetElementsByTagName("meta:template");
                if (xnl != null && xnl.Count != 0)
                    foreach (XmlNode xn in xnl)
                        FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(xn.Attributes.GetNamedItem("xlink:href").Value), true);
                xnl = doc.GetElementsByTagName("meta:document-statistic");
                if (xnl != null && xnl.Count > 0)
                {
                    String estadisticas = string.Empty;
                    if (xnl[0].Attributes.GetNamedItem("meta:table-count") != null)
                        estadisticas += "  Tables: " + xnl[0].Attributes.GetNamedItem("meta:table-count").Value;
                    if (xnl[0].Attributes.GetNamedItem("meta:cell-count") != null)
                        estadisticas += "  Cell: " + xnl[0].Attributes.GetNamedItem("meta:cell-count").Value;
                    if (xnl[0].Attributes.GetNamedItem("meta:image-count") != null)
                        estadisticas += "  Images: " + xnl[0].Attributes.GetNamedItem("meta:image-count").Value;
                    if (xnl[0].Attributes.GetNamedItem("meta:object-count") != null)
                        estadisticas += "  Objects: " + xnl[0].Attributes.GetNamedItem("meta:object-count").Value;
                    if (xnl[0].Attributes.GetNamedItem("meta:page-count") != null)
                        estadisticas += "  Pages: " + xnl[0].Attributes.GetNamedItem("meta:page-count").Value;
                    if (xnl[0].Attributes.GetNamedItem("meta:paragraph-count") != null)
                        estadisticas += "  Paragraph: " + xnl[0].Attributes.GetNamedItem("meta:paragraph-count").Value;
                    if (xnl[0].Attributes.GetNamedItem("meta:word-count") != null)
                        estadisticas += "  Words: " + xnl[0].Attributes.GetNamedItem("meta:word-count").Value;
                    if (xnl[0].Attributes.GetNamedItem("meta:character-count") != null)
                        estadisticas += "  Characters: " + xnl[0].Attributes.GetNamedItem("meta:character-count").Value;
                    if (estadisticas != string.Empty)
                        FoundMetaData.Statistic = estadisticas;
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
                        if (xn.Attributes.GetNamedItem("config:name").Value == "PrinterName")
                            if (xn.HasChildNodes)
                                FoundPrinters.AddUniqueItem(Functions.FilterPrinter(xn.FirstChild.Value));
                        else if (xn.Attributes.GetNamedItem("config:name").Value == "CurrentDatabaseDataSource")
                        {
                            if (xn.HasChildNodes)
                                FoundMetaData.DataBase = xn.FirstChild.Value;
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
                        else if (xn.Attributes.GetNamedItem("config:name").Value == "ColorTableURL")
                        {
                            if (xn.HasChildNodes)
                                if (PathAnalysis.CleanPath(xn.FirstChild.Value) != "$(user)/config/")
                                    FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(xn.FirstChild.Value), true);
                        }
                        else if (xn.Attributes.GetNamedItem("config:name").Value == "BitmapTableURL")
                        {
                            if (xn.HasChildNodes)
                                if (PathAnalysis.CleanPath(xn.FirstChild.Value) != "$(user)/config/")
                                    FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(xn.FirstChild.Value), true);
                        }
                        else if (xn.Attributes.GetNamedItem("config:name").Value == "DashTableURL")
                        {
                            if (xn.HasChildNodes)
                                if (PathAnalysis.CleanPath(xn.FirstChild.Value) != "$(user)/config/")
                                    FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(xn.FirstChild.Value), true);
                        }
                        else if (xn.Attributes.GetNamedItem("config:name").Value == "GradientTableURL")
                        {
                            if (xn.HasChildNodes)
                                if (PathAnalysis.CleanPath(xn.FirstChild.Value) != "$(user)/config/")
                                    FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(xn.FirstChild.Value), true);
                        }
                        else if (xn.Attributes.GetNamedItem("config:name").Value == "HatchTableURL")
                        {
                            if (xn.HasChildNodes)
                                if (PathAnalysis.CleanPath(xn.FirstChild.Value) != "$(user)/config/")
                                    FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(xn.FirstChild.Value), true);
                        }
                        else if (xn.Attributes.GetNamedItem("config:name").Value == "LineEndTableURL")
                        {
                            if (xn.HasChildNodes)
                                if (PathAnalysis.CleanPath(xn.FirstChild.Value) != "$(user)/config/")
                                    FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(xn.FirstChild.Value), true);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Error reading file settings.xml ({0}).", e.ToString()));
            }
        }

        private void analizeFileContent(Stream stm)
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
                        if (href != string.Empty)
                        {
                            if (href.StartsWith("mailto:"))
                            {
                                String email = href.Substring(7, (href.Contains("?") ? href.IndexOf('?') : href.Length) - 7);
                                FoundEmails.AddUniqueItem(email);
                            }
                            else if (href.StartsWith("ftp:"))
                            {
                                if (!links.Contains(href))
                                    links.Add(href);
                            }
                            else if (href.StartsWith("telnet:"))
                            {
                                if (!links.Contains(href))
                                    links.Add(href);
                            }
                            else if (href.StartsWith("ldap:"))
                            {
                                if (!links.Contains(href))
                                    links.Add(href);
                            }
                            else
                            {
                                try
                                {
                                    Uri u = new Uri(href);
                                    /* ¿Porque este if, oca?, pueden sacarse nombres sin reoslucion como http://privado/ */
                                    /*
                                    if (u.HostNameType != UriHostNameType.Dns)
                                    {
                                        if (!links.Contains(href))
                                            links.Add(href);
                                    }
                                    */
                                    if (!links.Contains(href))
                                        links.Add(href);
                                }
                                catch (UriFormatException)  //No es una URI, será un path interno...
                                {
                                    if (!href.StartsWith("#"))  //Se omiten referencias del tipo #Pais
                                    {
                                        if (!links.Contains(href))
                                            links.Add(href);
                                    }
                                }
                            }
                        }
                    }
                    if (links.Count != 0)
                    {
                        foreach (String link in links)
                        {
                            FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(link), true);//false);
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
        private void analizeFileVersionList(Stream stm, ZipFile zip)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null;
                doc.Load(stm);
                XmlNodeList xnl = doc.GetElementsByTagName("VL:version-entry");
                if (xnl != null && xnl.Count != 0)
                {
                    foreach (XmlNode xn in xnl)
                    {
                        OldVersionsItem vai = new OldVersionsItem();
                        vai.Comments = xn.Attributes["VL:comment"].Value;
                        vai.Author = xn.Attributes["VL:creator"].Value;
                        //Añadimos el usuario de la versión antigua a los usuarios del documento
                        FoundUsers.AddUniqueItem(vai.Author, false, "VL:creator");
                        DateTime d;
                        if (DateTime.TryParse(xn.Attributes["dc:date-time"].Value.Replace('T', ' '), out d))
                        {
                            vai.SpecificDate = true;
                            vai.Date = d;
                        }
                        String strFile = "Versions/" + xn.Attributes["VL:title"].Value;
                        if (zip.EntryFileNames.Contains(strFile))
                        {
                            //Se analiza la versión antigua embebida al completo
                            using (Stream stmXML = new MemoryStream())
                            {
                                zip.Extract(strFile, stmXML);
                                stmXML.Seek(0, SeekOrigin.Begin);
                                OpenOfficeDocument ooDoc = new OpenOfficeDocument(stmXML, strExtlo);
                                ooDoc.analyzeFile();
                                dicOldVersions.Add(xn.Attributes["VL:title"].Value, ooDoc);
                                ooDoc.Close();
                            }
                        }
                        FoundOldVersions.Items.Add(vai);
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
