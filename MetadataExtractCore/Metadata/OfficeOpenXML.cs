using MetadataExtractCore.Analysis;
using MetadataExtractCore.Diagrams;
using MetadataExtractCore.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Xml;

namespace MetadataExtractCore.Extractors
{
    public class OfficeOpenXMLDocument : DocumentExtractor
    {
        private string strExtlo;
        public OfficeOpenXMLDocument(Stream stm, string strExt) : base(stm)
        {
            this.strExtlo = strExt.ToLower();
        }

        public override FileMetadata AnalyzeFile()
        {
            try
            {
                this.foundMetadata = new FileMetadata();
                using (Package pZip = Package.Open(this.fileStream))
                {
                    if (pZip.PackageProperties != null)
                    {
                        this.foundMetadata.Add(new User(pZip.PackageProperties.Creator, false));
                        this.foundMetadata.Add(new User(pZip.PackageProperties.LastModifiedBy, false));
                        if (pZip.PackageProperties.Created.HasValue)
                        {
                            this.foundMetadata.Dates.CreationDate = pZip.PackageProperties.Created.Value;
                        }
                        if (pZip.PackageProperties.Modified.HasValue)
                        {
                            this.foundMetadata.Dates.ModificationDate = pZip.PackageProperties.Modified.Value;
                        }
                        if (pZip.PackageProperties.LastPrinted.HasValue)
                        {
                            this.foundMetadata.Dates.PrintingDate = pZip.PackageProperties.LastPrinted.Value;
                        }
                        if (!String.IsNullOrWhiteSpace(pZip.PackageProperties.Title))
                        {
                            this.foundMetadata.Title = pZip.PackageProperties.Title;
                        }
                        if (!String.IsNullOrWhiteSpace(pZip.PackageProperties.Keywords))
                        {
                            this.foundMetadata.Keywords = pZip.PackageProperties.Keywords;
                        }
                    }

                    Uri uriFile = new Uri("/docProps/core.xml", UriKind.Relative);
                    if (pZip.PartExists(uriFile))
                    {
                        PackagePart pDocument = pZip.GetPart(uriFile);
                        using (Stream stmDoc = pDocument.GetStream(FileMode.Open, FileAccess.Read))
                        {
                            AnalizeFileCore(stmDoc);
                        }
                    }
                    uriFile = new Uri("/docProps/app.xml", UriKind.Relative);
                    if (pZip.PartExists(uriFile))
                    {
                        PackagePart pDocument = pZip.GetPart(uriFile);
                        using (Stream stmDoc = pDocument.GetStream(FileMode.Open, FileAccess.Read))
                        {
                            AnalizeFileApp(stmDoc);
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
                                AnalizeFileDocument(stmDoc);
                            }
                        }
                        //Consulta el fichero settings para recuperar el idioma del documento
                        if (foundMetadata.Language == string.Empty)
                        {
                            uriFile = new Uri("/word/settings.xml", UriKind.Relative);
                            if (pZip.PartExists(uriFile))
                            {
                                PackagePart pDocument = pZip.GetPart(uriFile);
                                using (Stream stmDoc = pDocument.GetStream(FileMode.Open, FileAccess.Read))
                                {
                                    AnalizeFileSettings(stmDoc);
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
                                AnalizeLinks(stmDoc);
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
                                    this.foundMetadata.Add(new Printer(Functions.FilterPrinter((new string(name).Replace("\0", "")))));
                                }
                            }
                            if (pp.Uri.ToString().StartsWith("/xl/worksheets/_rels/"))
                            {
                                PackagePart pDocument = pZip.GetPart(pp.Uri);
                                using (Stream stmDoc = pDocument.GetStream(FileMode.Open, FileAccess.Read))
                                {
                                    AnalizeLinks(stmDoc);
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
                                    AnalizeLinks(stmDoc);
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
                             strFileNameLo.EndsWith(".jpeg") ||
                             strFileNameLo.EndsWith(".png")))
                        {
                            using (EXIFDocument eDoc = new EXIFDocument(pp.GetStream(FileMode.Open, FileAccess.Read)))
                            {
                                FileMetadata exifMetadata = eDoc.AnalyzeFile();
                                foundMetadata.EmbeddedImages.Add(System.IO.Path.GetFileName(strFileName), exifMetadata);
                                //Copiamos los metadatos sobre usuarios y Applications de la imagen al documento
                                this.foundMetadata.AddRange(exifMetadata.Users.ToArray());
                                this.foundMetadata.AddRange(exifMetadata.Applications.ToArray());
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }

            return this.foundMetadata;
        }

        private void AnalizeFileCore(Stream stm)
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
                        this.foundMetadata.Title = xnl[0].FirstChild.Value;
                        //Si el título es una ruta válida, agregar como una ruta del equipo
                        this.foundMetadata.Add(new Diagrams.Path(PathAnalysis.CleanPath(foundMetadata.Title), true));
                    }
                xnl = doc.GetElementsByTagName("dc:subject");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        this.foundMetadata.Subject = xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("dc:description");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        this.foundMetadata.Description = xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("cp:lastModifiedBy");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        this.foundMetadata.Add(new User(xnl[0].FirstChild.Value, true, "cp:lastModifiedBy"));
                xnl = doc.GetElementsByTagName("dc:creator");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        this.foundMetadata.Add(new User(xnl[0].FirstChild.Value, this.foundMetadata.Users.Count == 0, "dc:creator"));
                xnl = doc.GetElementsByTagName("cp:revision");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                    {
                        Decimal d;
                        if (Decimal.TryParse(xnl[0].FirstChild.Value, out d))
                            this.foundMetadata.VersionNumber = d;
                    }
                xnl = doc.GetElementsByTagName("dcterms:created");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        if (xnl[0].FirstChild.Value != "1601-01-01T00:00:00Z")
                        {
                            DateTime d;
                            if (DateTime.TryParse(xnl[0].FirstChild.Value.Replace("T", " ").Replace("Z", ""), out d))
                            {
                                this.foundMetadata.Dates.CreationDate = d.ToLocalTime();
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
                                this.foundMetadata.Dates.ModificationDate = d.ToLocalTime();
                            }
                        }
                xnl = doc.GetElementsByTagName("cp:keywords");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        this.foundMetadata.Keywords = xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("cp:category");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        this.foundMetadata.Category = xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("dc:language");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        this.foundMetadata.Language = xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("cp:lastPrinted");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        if (xnl[0].FirstChild.Value != "1601-01-01T00:00:00Z")
                        {
                            DateTime d;
                            if (DateTime.TryParse(xnl[0].FirstChild.Value.Replace("T", " ").Replace("Z", ""), out d))
                            {
                                this.foundMetadata.Dates.PrintingDate = d.ToLocalTime();
                            }
                        }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Error reading file core.xml ({0}).", e.ToString()));
            }
            finally
            {
                if (foundMetadata == null)
                    this.foundMetadata = new FileMetadata();
            }
        }

        private void AnalizeFileApp(Stream stm)
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
                                string strSoftware = ApplicationAnalysis.GetApplicationsFromString(app + " - " + xnl[0].FirstChild.Value);
                                this.foundMetadata.Add(new Application(strSoftware));
                            }
                    }
                xnl = doc.GetElementsByTagName("Company");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        this.foundMetadata.Company = xnl[0].FirstChild.Value;
                xnl = doc.GetElementsByTagName("Manager");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        this.foundMetadata.Add(new User(xnl[0].FirstChild.Value, false, "Manager"));
                xnl = doc.GetElementsByTagName("TotalTime");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                    {
                        Double d;
                        if (Double.TryParse(xnl[0].FirstChild.Value, out d))
                            this.foundMetadata.EditTime = (decimal)d;
                    }

                StringBuilder statisticBuilder = new StringBuilder();
                xnl = doc.GetElementsByTagName("Pages");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        statisticBuilder.Append("Pages: " + xnl[0].FirstChild.Value);
                xnl = doc.GetElementsByTagName("Words");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        statisticBuilder.Append("\tWords: " + xnl[0].FirstChild.Value);
                xnl = doc.GetElementsByTagName("Characters");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        statisticBuilder.Append("\tCharacters: " + xnl[0].FirstChild.Value);
                xnl = doc.GetElementsByTagName("Lines");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        statisticBuilder.Append("\tLines: " + xnl[0].FirstChild.Value);
                xnl = doc.GetElementsByTagName("Paragraphs");
                if (xnl.Count != 0)
                    if (xnl[0].HasChildNodes)
                        statisticBuilder.Append("\tParagraphs: " + xnl[0].FirstChild.Value);
                this.foundMetadata.Statistic = statisticBuilder.ToString().Trim();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Error reading file app.xml ({0}).", e.ToString()));
            }
        }

        private void AnalizeFileDocument(Stream stm)
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
                    Dictionary<string, PairValue<int, int>> dicHistoryControl = new Dictionary<string, PairValue<int, int>>();
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
                        string comment = String.Empty;
                        if (dicHistoryControl[strKey].x > 0)
                            comment = dicHistoryControl[strKey].x + " insertions";
                        if (dicHistoryControl[strKey].y > 0)
                            comment += comment.Length == 0 ? dicHistoryControl[strKey].y + " deletes" : ", " + dicHistoryControl[strKey].y + " deletes";
                        this.foundMetadata.Add(new History(strKey, comment));
                        //Añadimos a la lista de usuarios del documento
                        this.foundMetadata.Add(new User(strKey, false, "history"));
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Error reading file document.xml ({0}).", e.ToString()));
            }
        }

        private void AnalizeFileSettings(Stream stm)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.XmlResolver = null;
                doc.Load(stm);
                XmlNodeList xnl = doc.GetElementsByTagName("w:themeFontLang");
                if (xnl.Count != 0)
                    this.foundMetadata.Language = xnl[0].Attributes["w:val"].Value;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Error reading file settings.xml ({0}).", e.ToString()));
            }
        }

        private void AnalizeLinks(Stream stm)
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
                                this.foundMetadata.Add(new Email(email));
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
                    this.foundMetadata.AddRange(links.Select(p => new Diagrams.Path(PathAnalysis.CleanPath(p), true)).ToArray());
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Error searching links ({0}).", e.ToString()));
            }
        }
    }
}
