using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.IO.Packaging;
using MetadataExtractCore.Utilities;
using MetadataExtractCore.Analysis;
using MetadataExtractCore.Diagrams;

namespace MetadataExtractCore.Metadata
{
    [Serializable]
    public class OfficeOpenXMLDocument : MetaExtractor
    {
        private string strExtlo;
        public SerializableDictionary<string, EXIFDocument> dicPictureEXIF { get; set; }

        public OfficeOpenXMLDocument()
        {
            dicPictureEXIF = new SerializableDictionary<string, EXIFDocument>();
        }

        public OfficeOpenXMLDocument(Stream stm, string strExt): this()
        {
            this.stm = new MemoryStream();
            //Copia el stream de modo que pueda ser liberado o usado en la clase cliente
            Functions.CopyStream(stm, this.stm);
            this.strExtlo = strExt.ToLower();
        }

        public override void analyzeFile()
        {
            try
            {
                using (Package pZip = Package.Open(stm))
                {
                    Uri uriFile = new Uri("/docProps/core.xml", UriKind.Relative);
                    if (pZip.PartExists(uriFile))
                    {
                        PackagePart pDocument = pZip.GetPart(uriFile);
                        using (Stream stmDoc = pDocument.GetStream(FileMode.Open, FileAccess.Read))
                        {
                            analizeFileCore(stmDoc);
                        }
                    }
                    uriFile = new Uri("/docProps/app.xml", UriKind.Relative);
                    if (pZip.PartExists(uriFile))
                    {
                        PackagePart pDocument = pZip.GetPart(uriFile);
                        using (Stream stmDoc = pDocument.GetStream(FileMode.Open, FileAccess.Read))
                        {
                            analizeFileApp(stmDoc);
                        }
                    }
                    //Control de versiones
                    if (strExtlo == ".docx")
                    {
                        uriFile = new Uri("/word/document.xml", UriKind.Relative);
                        if (pZip.PartExists(uriFile))
                        {
                            PackagePart pDocument = pZip.GetPart(uriFile);
                            using (Stream stmDoc = pDocument.GetStream(FileMode.Open, FileAccess.Read))
                            {
                                analizeFileDocument(stmDoc);
                            }
                        }
                        //Consulta el fichero settings para recuperar el idioma del documento
                        if (FoundMetaData.Language == string.Empty)
                        {
                            uriFile = new Uri("/word/settings.xml", UriKind.Relative);
                            if (pZip.PartExists(uriFile))
                            {
                                PackagePart pDocument = pZip.GetPart(uriFile);
                                using (Stream stmDoc = pDocument.GetStream(FileMode.Open, FileAccess.Read))
                                {
                                    analizeFileSettings(stmDoc);
                                }
                            }
                        }
                        //Consulta el fichero document.xml.rels para obtener los links del documento
                        uriFile = new Uri("/word/_rels/document.xml.rels", UriKind.Relative);
                        if (pZip.PartExists(uriFile))
                        {
                            PackagePart pDocument = pZip.GetPart(uriFile);
                            using (Stream stmDoc = pDocument.GetStream(FileMode.Open, FileAccess.Read))
                            {
                                analizeLinks(stmDoc);
                            }
                        }
                    }
                    //Obtiene el nombre de las impresoras y los links de los documentos xlsx
                    else if (strExtlo == ".xlsx")
                    {
                        List<Uri> lstFiles = new List<Uri>();
                        foreach (PackagePart pp in pZip.GetParts())
                        {
                            if (pp.Uri.ToString().StartsWith("/xl/printerSettings/printerSettings"))
                            {
                                PackagePart pDocument = pZip.GetPart(pp.Uri);
                                if (pDocument != null)
                                {
                                    char[] name = new char[32];
                                    using (StreamReader sr = new StreamReader(pDocument.GetStream(FileMode.Open, FileAccess.Read), Encoding.Unicode))
                                    {
                                        sr.Read(name, 0, 32);
                                    }
                                    FoundPrinters.AddUniqueItem(Functions.FilterPrinter((new string(name).Replace("\0", ""))));
                                }
                            }
                            if (pp.Uri.ToString().StartsWith("/xl/worksheets/_rels/"))
                            {
                                PackagePart pDocument = pZip.GetPart(pp.Uri);
                                using (Stream stmDoc = pDocument.GetStream(FileMode.Open, FileAccess.Read))
                                {
                                    analizeLinks(stmDoc);
                                }
                            }
                        }
                    }
                    else if (strExtlo == ".pptx")
                    {
                        List<Uri> lstFiles = new List<Uri>();
                        foreach (PackagePart pp in pZip.GetParts())
                        {
                            if (pp.Uri.ToString().StartsWith("/ppt/slides/_rels/"))
                            {
                                PackagePart pDocument = pZip.GetPart(pp.Uri);
                                using (Stream stmDoc = pDocument.GetStream(FileMode.Open, FileAccess.Read))
                                {
                                    analizeLinks(stmDoc);
                                }
                            }
                        }
                    }
                    //Extraer información EXIF de cada imagen
                    foreach (PackagePart pp in pZip.GetParts())
                    {
                        string strFileName = pp.Uri.ToString();
                        string strFileNameLo = strFileName.ToLower();
                        //Filtro que se queda con todas las imagenes *.jpg y *.jpeg de las 3 posibles carpetas
                        if ((strFileNameLo.StartsWith("/word/media/") ||
                             strFileNameLo.StartsWith("/ppt/media/") ||
                             strFileNameLo.StartsWith("/xl/media/")) &&
                            (strFileNameLo.EndsWith(".jpg") ||
                             strFileNameLo.EndsWith(".jpeg")))
                        {
                            EXIFDocument eDoc = new EXIFDocument(pp.GetStream(FileMode.Open, FileAccess.Read), Path.GetExtension(strFileNameLo));
                            eDoc.analyzeFile();
                            dicPictureEXIF.Add(Path.GetFileName(strFileName), eDoc);
                            //Copiamos los metadatos sobre usuarios y Applications de la imagen al documento
                            foreach (UserItem uiEXIF in eDoc.FoundUsers.Items)
                                FoundUsers.AddUniqueItem(uiEXIF.Name, false,uiEXIF.Notes);
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
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }

        private void analizeFileCore(Stream stm)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null;
                doc.Load(stm);
                XmlNodeList xnl;
                xnl = doc.GetElementsByTagName("dc:title");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                    {
                        FoundMetaData.Title = xnl[0].FirstChild.Value;
                        //Si el título es una ruta válida, agregar como una ruta del equipo
                        FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(FoundMetaData.Title), true);
                    }
                xnl = doc.GetElementsByTagName("dc:subject");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        FoundMetaData.Subject = xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("dc:description");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        FoundMetaData.Description = xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("cp:lastModifiedBy");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        FoundUsers.AddUniqueItem(xnl[0].FirstChild.Value, true, "cp:lastModifiedBy");
                xnl = doc.GetElementsByTagName("dc:creator");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        FoundUsers.AddUniqueItem(xnl[0].FirstChild.Value, FoundUsers.Items.Count == 0, "dc:creator");
                xnl = doc.GetElementsByTagName("cp:revision");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                    {
                        Decimal d;
                        if (Decimal.TryParse(xnl[0].FirstChild.Value, out d))
                            FoundMetaData.VersionNumber = d;
                    }
                xnl = doc.GetElementsByTagName("dcterms:created");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        if (xnl[0].FirstChild.Value != "1601-01-01T00:00:00Z")
                        {
                            DateTime d;
                            if (DateTime.TryParse(xnl[0].FirstChild.Value.Replace("T", " ").Replace("Z", ""), out d))
                            {
                                FoundDates.CreationDateSpecified = true;
                                FoundDates.CreationDate = d.ToLocalTime();
                            }
                        }
                xnl = doc.GetElementsByTagName("dcterms:modified");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        if (xnl[0].FirstChild.Value != "1601-01-01T00:00:00Z")
                        {
                            DateTime d;
                            if (DateTime.TryParse(xnl[0].FirstChild.Value.Replace("T", " ").Replace("Z", ""), out d))
                            {
                                FoundDates.ModificationDateSpecified = true;
                                FoundDates.ModificationDate = d.ToLocalTime();
                            }
                        }
                xnl = doc.GetElementsByTagName("cp:keywords");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        FoundMetaData.Keywords = xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("cp:category");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        FoundMetaData.Category = xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("dc:language");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        FoundMetaData.Language = xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("cp:lastPrinted");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        if (xnl[0].FirstChild.Value != "1601-01-01T00:00:00Z")
                            if (xnl[0].FirstChild.Value != "1601-01-01T00:00:00Z")
                            {
                                DateTime d;
                                if (DateTime.TryParse(xnl[0].FirstChild.Value.Replace("T", " ").Replace("Z", ""), out d))
                                {
                                    FoundDates.DatePrintingSpecified = true;
                                    FoundDates.DatePrinting = d.ToLocalTime();
                                }
                            }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Error reading file core.xml ({0}).", e.ToString()));
            }
        }

        private void analizeFileApp(Stream stm)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null;
                doc.Load(stm);
                XmlNodeList xnl = doc.GetElementsByTagName("Application");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                    {
                        string app = xnl[0].FirstChild.Value;
                        xnl = doc.GetElementsByTagName("AppVersion");
                        if (xnl.Count != 0)
                            if (xnl[0].HasChildNodes)
                            {
                                string strSoftware = Analysis.ApplicationAnalysis.GetApplicationsFromString(app + " - " + xnl[0].FirstChild.Value);
                                FoundMetaData.Applications.Items.Add(new ApplicationsItem(strSoftware));
                            }
                    }
                xnl = doc.GetElementsByTagName("Company");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        FoundMetaData.Company = xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("Manager");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        FoundUsers.AddUniqueItem(xnl[0].FirstChild.Value, false, "Manager");
                xnl = doc.GetElementsByTagName("TotalTime");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                    {
                        Double d;
                        if (Double.TryParse(xnl[0].FirstChild.Value, out d))
                            FoundMetaData.EditTime = (decimal)d;
                    }
                String estadisticas = string.Empty;
                xnl = doc.GetElementsByTagName("Pages");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        estadisticas += "Pages: " + xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("Words");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        estadisticas += "\tWords: " + xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("Characters");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        estadisticas += "\tCharacters: " + xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("Lines");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        estadisticas += "\tLines: " + xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("Paragraphs");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        estadisticas += "\tParagraphs: " + xnl[0].FirstChild.Value;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Error reading file app.xml ({0}).", e.ToString()));
            }
        }

        private void analizeFileDocument(Stream stm)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null;
                doc.Load(stm);
                XmlNodeList xnlIns = doc.GetElementsByTagName("w:ins");
                XmlNodeList xnlDel = doc.GetElementsByTagName("w:del");
                if (xnlIns.Count > 0 || xnlDel.Count > 0)
                {
                    SerializableDictionary<string, PairValue<int, int>> dicHistoryControl = new SerializableDictionary<string, PairValue<int, int>>();
                    //Recorre la inserciones
                    foreach (XmlNode xn in xnlIns)
                    {
                        if (xn.Attributes["w:author"] != null &&
                            xn.Attributes["w:author"].Value != string.Empty)
                        {
                            if (dicHistoryControl.ContainsKey(xn.Attributes["w:author"].Value))
                                dicHistoryControl[xn.Attributes["w:author"].Value].x++;
                            else
                                dicHistoryControl.Add(xn.Attributes["w:author"].Value, new PairValue<int, int>(1, 0));
                        }
                    }
                    foreach (XmlNode xn in xnlDel)
                    {
                        if (xn.Attributes["w:author"] != null &&
                            xn.Attributes["w:author"].Value != string.Empty)
                        {
                            if (dicHistoryControl.ContainsKey(xn.Attributes["w:author"].Value))
                                dicHistoryControl[xn.Attributes["w:author"].Value].y++;
                            else
                                dicHistoryControl.Add(xn.Attributes["w:author"].Value, new PairValue<int, int>(0, 1));
                        }
                    }
                    foreach (string strKey in dicHistoryControl.Keys)
                    {
                        HistoryItem hi = new HistoryItem();
                        hi.Author = strKey;
                        if (dicHistoryControl[strKey].x > 0)
                            hi.Comments = dicHistoryControl[strKey].x + " insertions";
                        if (dicHistoryControl[strKey].y > 0)
                            hi.Comments += hi.Comments.Length == 0 ? dicHistoryControl[strKey].y + " deletes" : ", " + dicHistoryControl[strKey].y + " deletes";
                        FoundHistory.Items.Add(hi);
                        //Añadimos a la lista de usuarios del documento
                        FoundUsers.AddUniqueItem(strKey, false, "history");
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Error reading file document.xml ({0}).", e.ToString()));
            }
        }

        private void analizeFileSettings(Stream stm)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null;
                doc.Load(stm);
                XmlNodeList xnl = doc.GetElementsByTagName("w:themeFontLang");
                if (xnl.Count != 0)
                    FoundMetaData.Language = xnl[0].Attributes["w:val"].Value;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Error reading file settings.xml ({0}).", e.ToString()));
            }
        }

        private void analizeLinks(Stream stm)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null;
                doc.Load(stm);
                XmlNodeList xnl = doc.GetElementsByTagName("Relationship");
                List<string> links = new List<string>();
                foreach (XmlNode xn in xnl)
                {
                    if (xn.Attributes["Type"].Value == "http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink")
                    {
                        String href = xn.Attributes["Target"].Value;
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
                            else if (href.StartsWith("http:"))
                            {

                                if (!href.EndsWith("/"))    // Si la direccion no termina con un slash, se le añade
                                {
                                    int cuentaSlash = 0;
                                    for (int i = 0; i < href.Length; i++)
                                        if (href[i] == '/')
                                            cuentaSlash++;
                                    if (cuentaSlash == 2)
                                        href += "/";
                                }

                                if (!links.Contains(href))
                                    links.Add(href);
                            }
                            else
                            {
                                try
                                {
                                    Uri u = new Uri(href);
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
                }
                if (links.Count != 0)
                {
                    foreach (String link in links)
                    {
                        FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(link), true);//false);
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Error searching links ({0}).", e.ToString()));
            }
        }
    }
}
