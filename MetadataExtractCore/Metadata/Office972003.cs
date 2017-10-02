using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using MetadataExtractCore.Utilities;
using MetadataExtractCore.Analysis;
using MetadataExtractCore.Diagrams;

namespace MetadataExtractCore.Metadata
{
    [Serializable]
    public class Office972003 : MetaExtractor
    {
        public SerializableDictionary<string, EXIFDocument> dicPictureEXIF { get; set; }

        public Office972003()
        {
            dicPictureEXIF = new SerializableDictionary<string, EXIFDocument>();
        }

        public Office972003(Stream stm): this()
        {
            this.stm = new MemoryStream();
            Functions.CopyStream(stm, this.stm);
        }

        public override void analyzeFile()
        {
            OleDocument doc = null;
            try
            {
                doc = new OleDocument(stm);
                if (!doc.isValid())
                {
                    return;
                }
                ReadDocument(doc);

                GetSummaryInformation(doc);
                AnalizarTitulo();
                AnalizarPlantilla();

                GetPrinters(doc);
                GetRelatedDocuments(doc);

                GetPrintersInXls(doc);
                GetHistory(doc);

                GetOperatingSystem(doc);

                GetPathPpt(doc);

                GetImagesDoc(doc);
                GetImagesPpt(doc);

                GetLinksBinary(doc);

                GetUserFromPaths();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
                return;
            }
            finally
            {
                if (doc != null)
                    doc.Close();
            }
        }

        private void ReadDocument(OleDocument doc)
        {
            doc.readMSAT();
            doc.readSAT();
            doc.readSSAT();
            doc.readDir();
        }

        private void GetSummaryInformation(OleDocument doc)
        {
            using (Stream SummaryInformation = doc.OpenStream("SummaryInformation"))
            {
                if (SummaryInformation != null)
                {
                    SummaryInformation.Seek(0, SeekOrigin.Begin);
                    OleStream os = new OleStream(SummaryInformation);
                    os.GetMetadata(FoundMetaData, FoundUsers, FoundDates, FoundEmails);
                }
            }
            using (Stream DocumentSummaryInformation = doc.OpenStream("DocumentSummaryInformation"))
            {
                if (DocumentSummaryInformation != null)
                {
                    DocumentSummaryInformation.Seek(0, SeekOrigin.Begin);
                    OleStream os = new OleStream(DocumentSummaryInformation);
                    os.GetMetadata(FoundMetaData, FoundUsers, FoundDates, FoundEmails);
                }
            }
        }

        private void AnalizarTitulo()
        {
            if (FoundMetaData.Title != null)
                FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(FoundMetaData.Title), true);
        }

        private void AnalizarPlantilla()
        {

            if (FoundMetaData.Template != null && FoundMetaData.Template.Trim().Length > 1)
                FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(FoundMetaData.Template), false);
        }

        private void GetPrinters(OleDocument doc)
        {
            using (Stream WordDocument = doc.OpenStream("WordDocument"))
            {
                if (WordDocument == null)
                    return;
                BinaryReader br = new BinaryReader(WordDocument);
                WordDocument.Seek(0xB, SeekOrigin.Begin);
                Byte tipo = br.ReadByte();
                WordDocument.Seek(0x172, SeekOrigin.Begin);
                UInt32 dir = br.ReadUInt32();
                UInt32 tam = br.ReadUInt32();
                if (tam > 0)
                {
                    Stream table = doc.OpenStream((tipo & 2) == 2 ? "1Table" : "0Table");
                    Byte[] DriverImpresora = new Byte[tam];
                    table.Seek(dir, SeekOrigin.Begin);
                    table.Read(DriverImpresora, 0, (int)tam);
                    FoundPrinters.AddUniqueItem(Functions.FilterPrinter(Encoding.Default.GetString(DriverImpresora).Replace("\0", "")));
                    table.Close();
                }
            }
        }

        private void GetRelatedDocuments(OleDocument doc)
        {
            using (Stream WordDocument = doc.OpenStream("WordDocument"))
            {
                if (WordDocument == null)
                    return;
                BinaryReader br = new BinaryReader(WordDocument);
                WordDocument.Seek(0xB, SeekOrigin.Begin);
                Byte tipo = br.ReadByte();

                WordDocument.Seek(0x19A, SeekOrigin.Begin);
                UInt32 dir = br.ReadUInt32();
                UInt32 tam = br.ReadUInt32();
                if (tam > 8)
                {
                    Stream table = doc.OpenStream((tipo & 2) == 2 ? "1Table" : "0Table");
                    BinaryReader br1 = new BinaryReader(table);
                    table.Seek(dir, SeekOrigin.Begin);
                    bool unicode = br1.ReadUInt16() == 0xFFFF;
                    int nro_strings = br1.ReadInt16();
                    int len_extradata = br1.ReadInt32();

                    if (nro_strings > 0)
                    {
                        int strSize = br1.ReadInt16();
                        string ruta;
                        if (unicode)
                        {
                            Byte[] cadena = br1.ReadBytes(strSize * 2);
                            ruta = Encoding.Unicode.GetString(cadena).Replace('\0', ' ');
                        }
                        else
                        {
                            Byte[] cadena = br1.ReadBytes(strSize);
                            ruta = Encoding.Default.GetString(cadena).Replace('\0', ' ');
                        }
                        FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(ruta), true);
                    }
                    table.Close();
                }
            }
        }

        private void GetPrintersInXls(OleDocument doc)
        {
            using (Stream Workbook = doc.OpenStream("Workbook"))
            {
                if (Workbook == null)
                    return;
                BinaryReader br = new BinaryReader(Workbook);
                Workbook.Seek(0, SeekOrigin.Begin);
                while (Workbook.Position <= Workbook.Length - 2)
                {
                    try
                    {
                        Int16 Tipo = br.ReadInt16();
                        Int16 Len = br.ReadInt16();
                        if (Len < 0)
                            break;
                        if (Tipo == 0x4D)
                        {
                            long PosOri = Workbook.Position;
                            if (br.ReadInt16() == 0) 
                            {
                                String PrinterName = Encoding.Unicode.GetString(br.ReadBytes(32 * 2)).Replace('\0', ' ');
                                br.ReadInt16();
                                br.ReadInt16();
                                Int16 StructSize = br.ReadInt16();
                                Int16 DriverSize = br.ReadInt16();
                               
                                if (DriverSize != 0)
                                {
                                    Workbook.Seek(StructSize - (8 + 64), SeekOrigin.Current);
                                    Byte[] Driver = br.ReadBytes(DriverSize);
                                    String ImpresoraDriver = Functions.ExtractPrinterFromBytes(Driver);
                                    if (!string.IsNullOrEmpty(ImpresoraDriver.Trim()))
                                        FoundPrinters.AddUniqueItem(Functions.FilterPrinter(ImpresoraDriver.Replace("\0", "")));
                                    else
                                        FoundPrinters.AddUniqueItem(Functions.FilterPrinter(PrinterName.Replace("\0", "")));
                                }
                                else
                                    FoundPrinters.AddUniqueItem(Functions.FilterPrinter(PrinterName.Replace("\0", "")));
                            }
                            Workbook.Position = PosOri;
                        }
                        Workbook.Seek(Len, SeekOrigin.Current);
                    }
                    catch
                    {
                        break;
                    }
                }
            }
        }

         private void GetHistory(OleDocument doc)
        {
            using (Stream WordDocument = doc.OpenStream("WordDocument"))
            {
                if (WordDocument == null)
                    return;
                BinaryReader br = new BinaryReader(WordDocument);
                WordDocument.Seek(0xB, SeekOrigin.Begin);
                Byte tipo = br.ReadByte();

                WordDocument.Seek(0x2D2, SeekOrigin.Begin);
                UInt32 dir = br.ReadUInt32();
                UInt32 tam = br.ReadUInt32();
                if (tam > 0)
                {
                    using (var table = doc.OpenStream((tipo & 2) == 2 ? "1Table" : "0Table"))
                    {
                        table.Seek(dir, SeekOrigin.Begin);
                        br = new BinaryReader(table);
                        Boolean unicode = br.ReadUInt16() == 0xFFFF;
                        UInt32 nroCadenas = br.ReadUInt16();
                        UInt32 extraDataTable = br.ReadUInt16();
                        for (int i = 0; i < nroCadenas; i += 2)
                        {
                            HistoryItem hi = new HistoryItem();
                            UInt16 strSize = br.ReadUInt16();
                            if (unicode)
                            {
                                Byte[] cadena = br.ReadBytes(strSize * 2);
                                hi.Author = Encoding.Unicode.GetString(cadena).Replace('\0', ' ');
                            }
                            else
                            {
                                Byte[] cadena = br.ReadBytes(strSize);
                                hi.Author = Encoding.Default.GetString(cadena).Replace('\0', ' ');
                            }
                            FoundUsers.AddUniqueItem(hi.Author, false, "History");
                            strSize = br.ReadUInt16();
                            if (unicode)
                            {
                                Byte[] cadena = br.ReadBytes(strSize * 2);
                                hi.Path = Encoding.Unicode.GetString(cadena).Replace('\0', ' ');
                            }
                            else
                            {
                                Byte[] cadena = br.ReadBytes(strSize);
                                hi.Path = Encoding.Default.GetString(cadena).Replace('\0', ' ');
                            }
                            FoundHistory.Items.Add(hi);
                            bool IsComputerPath = false;

                            foreach (UserItem ui in FoundUsers.Items)
                                if (hi.Author.Trim() == ui.Name.Trim())
                                    IsComputerPath = ui.IsComputerUser;
                            FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(hi.Path), IsComputerPath);
                        }
                    }
                }
            }
        }

        private void GetOperatingSystem(OleDocument doc)
        {
            byte intHighVersion, intLowVersion;
            doc.GetOperatingSystemFromStreamHeaders(out intHighVersion, out intLowVersion);
            switch (intHighVersion)
            {
                case 1:
                    switch (intLowVersion)
                    {
                        case 0:
                            if (FoundMetaData.Applications.Items.Count == 0)
                                FoundMetaData.Applications.Items.Add(new ApplicationsItem("OpenOffice"));
                            break;
                    }
                    break;
                case 3:
                    switch (intLowVersion)
                    {
                        case 10:
                            FoundMetaData.OperativeSystem = "Mac OS";
                            break;
                        case 51:
                            FoundMetaData.OperativeSystem = "Windows NT 3.51";
                            break;
                    }
                    break;
                case 4:
                    switch (intLowVersion)
                    {
                        case 0: FoundMetaData.OperativeSystem = "Windows NT 4.0";
                            break;
                        case 10: FoundMetaData.OperativeSystem = "Windows 98";
                            break;
                    }
                    break;
                case 5:
                    switch (intLowVersion)
                    {
                        case 0: FoundMetaData.OperativeSystem = "Windows Server 2000";
                            break;
                        case 1: FoundMetaData.OperativeSystem = "Windows XP";
                            break;
                        case 2: FoundMetaData.OperativeSystem = "Windows Server 2003";
                            break;
                    }
                    break;
                case 6:
                    switch (intLowVersion)
                    {
                        case 0: FoundMetaData.OperativeSystem = "Windows Vista";
                            break;
                        case 1: FoundMetaData.OperativeSystem = "Windows 7";
                            break;
                    }
                    break;
            }
        }

        private void GetPathPpt(OleDocument doc)
        {
            using (var WordDocument = doc.OpenStream("PowerPoint Document")) {
                if (WordDocument == null)
                    return;
                try
                {
                    WordDocument.Seek(0, SeekOrigin.Begin);
                    using (var sr = new StreamReader(doc.OpenStream("PowerPoint Document"), Encoding.Unicode))
                    {
                        foreach (Match m in Regex.Matches(sr.ReadToEnd(), @"([a-z]:|\\)\\[a-zá-ú0-9\\\s,;.\-_#\$%&()=ñ´'¨{}Ç`/n/r\[\]+^@]+\\[a-zá-ú0-9\\\s,;.\-_#\$%&()=ñ´'¨{}Ç`/n/r\[\]+^@]+", RegexOptions.IgnoreCase))
                        {
                            string path = m.Value.Trim();
                            FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(path), true);
                        }
                    }

                }
                catch (Exception)
                {
                }
            }
        }

        private void GetImagesDoc(OleDocument doc)
        {
            using (Stream WordDocument = doc.OpenStream("WordDocument"))
            {
                using (Stream stmData = doc.OpenStream("Data"))
                {
                    if (WordDocument == null || stmData == null)
                        return;
                    WordDocument.Seek(0x18, SeekOrigin.Begin);
                    BinaryReader br = new BinaryReader(WordDocument);
                    Int32 fcMin = br.ReadInt32();
                    Int32 fcMac = br.ReadInt32();
                    Int32 FKPStart = fcMac % 0x200 == 0 ? fcMac : (fcMac - fcMac % 0x200) + 0x200;
                    WordDocument.Seek(FKPStart, SeekOrigin.Begin);
                    int ImagesFound = 0;

                    while (WordDocument.Position + 0x200 < WordDocument.Length)
                    {
                        byte[] FKP = br.ReadBytes(0x200);
                        if (FKP[0x1FF] == 00) break;
                        foreach (int offset in Functions.SearchBytesInBytes(FKP, new byte[] { 0x03, 0x6A }))
                        {
                            if (offset < 0x200 - 5)
                            {
                                int PICOffset = FKP[offset + 5] * 0x1000000 + FKP[offset + 4] * 0x10000 + FKP[offset + 3] * 0x100 + FKP[offset + 2];
                                if (PICOffset >= 0 && PICOffset < stmData.Length)
                                {
                                    stmData.Seek(PICOffset, SeekOrigin.Begin);
                                    BinaryReader brData = new BinaryReader(stmData);
                                    UInt32 PICLength = brData.ReadUInt32();
                                    long posOri = stmData.Position;
                                    int bufferLen = PICLength < stmData.Length - stmData.Position ? (int)PICLength - 4 : (int)(stmData.Length - stmData.Position);
                                    if (bufferLen == 0) continue;
                                    byte[] bufferPIC = brData.ReadBytes(bufferLen);

                                    string strImageName = "Image" + ImagesFound++;

                                    using (StreamReader sr = new StreamReader(new MemoryStream(bufferPIC), Encoding.Unicode))
                                    {
                                        String sRead = sr.ReadToEnd();
                                        foreach (Match m in Regex.Matches(sRead, @"([a-z]:|\\)\\[a-zá-ú0-9\\\s,;.\-_#\$%&()=ñ´'¨{}Ç`/n/r\[\]+^@]+\\[a-zá-ú0-9\\\s,;.\-_#\$%&()=ñ´'¨{}Ç`/n/r\[\]+^@]+", RegexOptions.IgnoreCase))
                                        {
                                            String path = m.Value.Trim();
                                            FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(path), true);
                                            strImageName = Path.GetFileName(path);
                                        }
                                    }

                                    List<int> lstJPEG = Functions.SearchBytesInBytes(bufferPIC, new byte[] { 0xFF, 0xD8 });
                                    if (lstJPEG.Count > 0)
                                    {
                                        using (MemoryStream msJPG = new MemoryStream(bufferPIC, lstJPEG[0], bufferPIC.Length - lstJPEG[0]))
                                        {
                                            EXIFDocument eDoc = new EXIFDocument(msJPG, ".jpg");
                                            eDoc.analyzeFile();
                                            dicPictureEXIF.Add(strImageName, eDoc);
                                            foreach (UserItem uiEXIF in eDoc.FoundUsers.Items)
                                                FoundUsers.AddUniqueItem(uiEXIF.Name, false, uiEXIF.Notes);
                                            foreach (ApplicationsItem Application in eDoc.FoundMetaData.Applications.Items)
                                            {
                                                string strApplication = Application.Name;
                                                if (!string.IsNullOrEmpty(strApplication.Trim()) && !FoundMetaData.Applications.Items.Any(A => A.Name == strApplication.Trim()))
                                                    FoundMetaData.Applications.Items.Add(new ApplicationsItem(strApplication.Trim()));
                                            }
                                            eDoc.Close();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void GetImagesPpt(OleDocument doc)
        {

            using (Stream stmPictures = doc.OpenStream("Pictures"))
            {
                if (stmPictures == null)
                    return;
                int ImagesFound = 0;
                stmPictures.Seek(0, SeekOrigin.Begin);
                while (stmPictures.Position < stmPictures.Length - 0x19)
                {
                    stmPictures.Seek(0x4, SeekOrigin.Current);
                    BinaryReader brData = new BinaryReader(stmPictures);
                    UInt32 PICLength = brData.ReadUInt32();
                    if (PICLength == 0 || stmPictures.Position + PICLength > stmPictures.Length) break;
                    byte[] bufferPIC = brData.ReadBytes((int)PICLength);
                    string strImageName = "Image" + ImagesFound++;
                    using (MemoryStream msJPG = new MemoryStream(bufferPIC, 0x11, bufferPIC.Length - 0x11))
                    {
                        EXIFDocument eDoc = new EXIFDocument(msJPG, ".jpg");
       
                        eDoc.analyzeFile();
                        eDoc.Close();
                        if (eDoc.Thumbnail != null)
                            lon += eDoc.Thumbnail.Length;
                        cont++;
                        System.Diagnostics.Debug.WriteLine(cont.ToString());
                        System.Diagnostics.Debug.WriteLine(lon /(1024*1024) + " Megacas");

                        dicPictureEXIF.Add(strImageName, eDoc);

                        foreach (UserItem uiEXIF in eDoc.FoundUsers.Items)
                            FoundUsers.AddUniqueItem(uiEXIF.Name, false, uiEXIF.Notes);
                        foreach (ApplicationsItem Application in eDoc.FoundMetaData.Applications.Items)
                        {
                            string strApplication = Application.Name;
                            if (!string.IsNullOrEmpty(strApplication.Trim()) && !FoundMetaData.Applications.Items.Any(A => A.Name == strApplication.Trim()))
                                FoundMetaData.Applications.Items.Add(new ApplicationsItem(strApplication.Trim()));
                        }

                    }
                }
            }
        }

        static int cont = 0;
        static long lon = 0;

        private void GetLinksBinary(OleDocument doc)
        {
            var pending = new List<Action>() {
                () => GetLinksBinaryWorkbook(doc.OpenStream("Workbook")),
                () => GetLinksWordDocument(doc.OpenStream("WordDocument")),
                () => GetLinksBinaryPowerPointDocument(doc.OpenStream("PowerPoint Document"))
            };
            foreach (var call in pending)
            {
                call();
            }
            pending.Clear();
        }

        private void GetLinksWordDocument(Stream document)
        {
            if (document == null)
                return;

            document.Seek(0, SeekOrigin.Begin);
            using (var sr = new StreamReader(document, Encoding.ASCII))
            {
                foreach (Match m in Regex.Matches(sr.ReadToEnd(), "\"((ftp|http|https|ldap|mailto|ftp|telnet)://[^\"]*)" /*dominio + @"/[a-z\/\.]*)"*/, RegexOptions.IgnoreCase))
                {
                    string link = m.Groups[1].Value.Trim();
                    if (IsInterestingLink(link))
                    {
                        if (string.IsNullOrEmpty(link))
                            continue;

                        string aux = link;
                        if (!link.EndsWith("/"))
                        {
                            int cuentaSlash = 0;
                            for (int i = 0; i < aux.Length; i++)
                                if (aux[i] == '/')
                                    cuentaSlash++;
                            if (cuentaSlash == 2)
                                aux += "/";
                        }
                        aux = PathAnalysis.CleanPath(aux);

                        FoundPaths.AddUniqueItem(aux, true);
                    }
                }
            }
        }

        private void GetLinksBinaryWorkbook(Stream document)
        {
            if (document == null)
                return;

            document.Seek(0, SeekOrigin.Begin);
            using (var sr = new StreamReader(document, Encoding.ASCII))
            {
                foreach (Match m in Regex.Matches(sr.ReadToEnd(), @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.IgnoreCase))
                {
                    string link = m.Value.Trim();

                    if (IsInterestingLink(link))
                    {

                        if (string.IsNullOrEmpty(link))
                            continue;

                        string aux = link;
                        aux = aux.Trim(new char[] { (char)18 }); 

                        if (!link.EndsWith("/"))    
                        {
                            int cuentaSlash = 0;
                            for (int i = 0; i < aux.Length; i++)
                                if (aux[i] == '/')
                                    cuentaSlash++;
                            if (cuentaSlash == 2)
                                aux += "/";
                        }

                        aux = PathAnalysis.CleanPath(aux);
                        FoundPaths.AddUniqueItem(aux, true);
                    }
                }
            }
        }

        private void GetLinksBinaryPowerPointDocument(Stream document)
        {
            if (document == null)
                return;

            document.Seek(0, SeekOrigin.Begin);
            using (var sr = new StreamReader(document, Encoding.Unicode))
            {
                foreach (Match m in Regex.Matches(sr.ReadToEnd(), @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.IgnoreCase))
                {
                    string link = m.Value.Trim();
                    if (IsInterestingLink(link))
                        FoundPaths.AddUniqueItem(PathAnalysis.CleanPath(link), true);
                }
            }
        }


      private bool IsInterestingLink(string href)
        {
            if (href != string.Empty)
            {
                if (href.StartsWith("mailto:"))
                {
                    string email = href.Substring(7, (href.Contains("?") ? href.IndexOf('?') : href.Length) - 7);
                    FoundEmails.AddUniqueItem(email);
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
                    catch (UriFormatException)  
                    {
                        if (!href.StartsWith("#")) 
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void GetUserFromPaths()
        {
            foreach (PathsItem ri in FoundPaths.Items)
            {
                string strUser = PathAnalysis.ExtractUserFromPath(ri.Path);
                FoundUsers.AddUniqueItem(strUser, ri.IsComputerFolder, ri.Path);
            }
        }
    }
}