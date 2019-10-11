using MetadataExtractCore.Analysis;
using MetadataExtractCore.Diagrams;
using MetadataExtractCore.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MetadataExtractCore.Extractors
{
    public class Office972003 : DocumentExtractor
    {
        public Office972003(Stream stm) : base(stm)
        {
        }

        public override FileMetadata AnalyzeFile()
        {
            OleDocument doc = null;
            this.foundMetadata = new FileMetadata();
            try
            {
                doc = new OleDocument(this.fileStream);
                if (!doc.isValid())
                {
                    return this.foundMetadata;
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
                return this.foundMetadata;
            }
            finally
            {
                if (doc != null)
                    doc.Close();
            }
            return this.foundMetadata;
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
                    os.GetMetadata(this.foundMetadata);
                }
            }
            using (Stream DocumentSummaryInformation = doc.OpenStream("DocumentSummaryInformation"))
            {
                if (DocumentSummaryInformation != null)
                {
                    DocumentSummaryInformation.Seek(0, SeekOrigin.Begin);
                    OleStream os = new OleStream(DocumentSummaryInformation);
                    os.GetMetadata(this.foundMetadata);
                }
            }
        }

        private void AnalizarTitulo()
        {
            if (this.foundMetadata.Title != null)
                this.foundMetadata.Add(new Diagrams.Path(PathAnalysis.CleanPath(this.foundMetadata.Title), true));
        }

        private void AnalizarPlantilla()
        {
            if (this.foundMetadata.Template != null && this.foundMetadata.Template.Trim().Length > 1)
                this.foundMetadata.Add(new Diagrams.Path(PathAnalysis.CleanPath(this.foundMetadata.Template), false));
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
                    if (table != null)
                    {
                        Byte[] DriverImpresora = new Byte[tam];
                        table.Seek(dir, SeekOrigin.Begin);
                        table.Read(DriverImpresora, 0, (int)tam);
                        this.foundMetadata.Add(new Printer(Functions.FilterPrinter(Encoding.Default.GetString(DriverImpresora).Replace("\0", ""))));
                        table.Close();
                    }
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
                        this.foundMetadata.Add(new Diagrams.Path(PathAnalysis.CleanPath(ruta), true));
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
                while (Workbook.Position <= Workbook.Length - 4)
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
                                        this.foundMetadata.Add(new Printer(Functions.FilterPrinter(ImpresoraDriver.Replace("\0", ""))));
                                    else
                                        this.foundMetadata.Add(new Printer(Functions.FilterPrinter(PrinterName.Replace("\0", ""))));
                                }
                                else
                                    this.foundMetadata.Add(new Printer(Functions.FilterPrinter(PrinterName.Replace("\0", ""))));
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

                            UInt16 strSize = br.ReadUInt16();
                            string author;
                            if (unicode)
                            {
                                Byte[] cadena = br.ReadBytes(strSize * 2);
                                author = Encoding.Unicode.GetString(cadena).Replace('\0', ' ');
                            }
                            else
                            {
                                Byte[] cadena = br.ReadBytes(strSize);
                                author = Encoding.Default.GetString(cadena).Replace('\0', ' ');
                            }
                            History hi = new History(author);

                            this.foundMetadata.Add(new User(author, false, "History"));

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
                            this.foundMetadata.Add(hi);
                            bool isComputerPath = false;

                            foreach (User ui in this.foundMetadata.Users)
                                if (hi.Value.Trim() == ui.Value.Trim())
                                    isComputerPath = ui.IsComputerUser;
                            this.foundMetadata.Add(new Diagrams.Path(PathAnalysis.CleanPath(hi.Path), isComputerPath));
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
                            if (this.foundMetadata.Applications.Count == 0)
                                this.foundMetadata.Add(new Application("OpenOffice"));
                            break;
                    }
                    break;
                case 3:
                    switch (intLowVersion)
                    {
                        case 10:
                            this.foundMetadata.OperatingSystem = "Mac OS";
                            break;
                        case 51:
                            this.foundMetadata.OperatingSystem = "Windows NT 3.51";
                            break;
                    }
                    break;
                case 4:
                    switch (intLowVersion)
                    {
                        case 0:
                            this.foundMetadata.OperatingSystem = "Windows NT 4.0";
                            break;
                        case 10:
                            this.foundMetadata.OperatingSystem = "Windows 98";
                            break;
                    }
                    break;
                case 5:
                    switch (intLowVersion)
                    {
                        case 0:
                            this.foundMetadata.OperatingSystem = "Windows Server 2000";
                            break;
                        case 1:
                            this.foundMetadata.OperatingSystem = "Windows XP";
                            break;
                        case 2:
                            this.foundMetadata.OperatingSystem = "Windows Server 2003";
                            break;
                    }
                    break;
                case 6:
                    switch (intLowVersion)
                    {
                        case 0:
                            this.foundMetadata.OperatingSystem = "Windows Vista";
                            break;
                        case 1:
                            this.foundMetadata.OperatingSystem = "Windows 7";
                            break;
                    }
                    break;
            }
        }

        private void GetPathPpt(OleDocument doc)
        {
            using (var WordDocument = doc.OpenStream("PowerPoint Document"))
            {
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
                            this.foundMetadata.Add(new Diagrams.Path(PathAnalysis.CleanPath(path), true));
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
                    int imagesFound = 0;

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
                                    if (bufferLen <= 0) continue;
                                    byte[] bufferPIC = brData.ReadBytes(bufferLen);

                                    string strImageName = String.Empty;

                                    using (StreamReader sr = new StreamReader(new MemoryStream(bufferPIC), Encoding.Unicode))
                                    {
                                        String sRead = sr.ReadToEnd();
                                        foreach (Match m in Regex.Matches(sRead, @"([a-z]:|\\)\\[a-zá-ú0-9\\\s,;.\-_#\$%&()=ñ´'¨{}Ç`/n/r\[\]+^@]+\\[a-zá-ú0-9\\\s,;.\-_#\$%&()=ñ´'¨{}Ç`/n/r\[\]+^@]+", RegexOptions.IgnoreCase))
                                        {
                                            String path = m.Value.Trim();
                                            this.foundMetadata.Add(new Diagrams.Path(PathAnalysis.CleanPath(path), true));
                                            strImageName = System.IO.Path.GetFileName(path);
                                        }
                                    }

                                    if (String.IsNullOrEmpty(strImageName) || foundMetadata.EmbeddedImages.ContainsKey(strImageName))
                                    {
                                        strImageName = "Image" + imagesFound++;
                                    }


                                    List<int> lstJPEG = Functions.SearchBytesInBytes(bufferPIC, new byte[] { 0xFF, 0xD8, 0xFF });
                                    if (lstJPEG.Count > 0)
                                    {
                                        using (MemoryStream msJPG = new MemoryStream(bufferPIC, lstJPEG[0], bufferPIC.Length - lstJPEG[0]))
                                        {
                                            using (EXIFDocument eDoc = new EXIFDocument(msJPG))
                                            {
                                                FileMetadata exifMetadata = eDoc.AnalyzeFile();
                                                foundMetadata.EmbeddedImages.Add(strImageName, exifMetadata);
                                                this.foundMetadata.AddRange(exifMetadata.Users.ToArray());
                                                this.foundMetadata.AddRange(exifMetadata.Applications.ToArray());
                                            }
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
                        FileMetadata exifMetadata = null;
                        using (EXIFDocument eDoc = new EXIFDocument(msJPG))
                        {
                            exifMetadata = eDoc.AnalyzeFile();
                        }
                        if (exifMetadata != null)
                        {
                            foundMetadata.EmbeddedImages.Add(strImageName, exifMetadata);

                            this.foundMetadata.AddRange(exifMetadata.Users.ToArray());
                            this.foundMetadata.AddRange(exifMetadata.Applications.ToArray());
                        }

                    }
                }
            }
        }

        private void GetLinksBinary(OleDocument doc)
        {
            GetLinksBinaryWorkbook(doc.OpenStream("Workbook"));
            GetLinksWordDocument(doc.OpenStream("WordDocument"));
            GetLinksBinaryPowerPointDocument(doc.OpenStream("PowerPoint Document"));
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

                        this.foundMetadata.Add(new Diagrams.Path(aux, true));
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
                        this.foundMetadata.Add(new Diagrams.Path(aux, true));
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
                        this.foundMetadata.Add(new Diagrams.Path(PathAnalysis.CleanPath(link), true));
                }
            }
        }

        private void GetUserFromPaths()
        {
            foreach (Diagrams.Path ri in this.foundMetadata.Paths)
            {
                string strUser = PathAnalysis.ExtractUserFromPath(ri.Value);
                this.foundMetadata.Add(new User(strUser, ri.IsComputerFolder, ri.Value));
            }
        }
    }
}