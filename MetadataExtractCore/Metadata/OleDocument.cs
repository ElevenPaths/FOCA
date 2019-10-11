using MetadataExtractCore.Diagrams;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MetadataExtractCore.Extractors
{
    public class OleDocument
    {
        private static byte[] magic_number = new byte[8] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 };
        private OleFileHeader ofh;
        enum SecIDType { MSATSecID = -4, SATSecID, EOFSecID, FreeSecID };
        private Int32[] MSAT;
        private Int32[] SAT;
        private Int32[] SSAT;
        private List<DirEntry> DirEntries;
        private Stream stmDocument;


        public class OleFileHeader
        {
            public byte[] signature = new byte[8];
            public byte[] UID = new byte[16];
            public UInt16 RevisionNumber;
            public UInt16 VersionNumber;
            public UInt16 ByteOrder;
            public UInt16 SizeOfSector;
            public UInt16 SizeOfShortSector;
            public UInt32 NumberOfSectorsSAT;
            public Int32 FirstSecIDDirectory;
            public UInt32 MinSizeOfStandardStream;
            public Int32 FirstSecIDSSAT;
            public UInt32 NumberOfSectorsSSAT;
            public Int32 FirstSecIDMSAT;
            public UInt32 NumberOfSectorsMSAT;
            public Int32[] FirstPartOfMSAT = new Int32[109];
        }

        public class DirEntry
        {
            public byte[] EntryName = new byte[64];
            public UInt16 EntryNameLength;
            public byte EntryType;
            public byte NodeColourEntry;
            public Int32 DirIDLeftChild;
            public Int32 DirIDRigthChild;
            public Int32 DirIDRootNodeEntry;
            public byte[] UID = new Byte[16];
            public Int32 flags;
            public byte[] CreationTime = new byte[8];
            public byte[] ModificationTime = new byte[8];
            public Int32 FirstSecIDStream;
            public Int32 LengthStream;
            public Int32 Reserved;
        }

        public OleDocument(Stream stmDocument)
        {
            this.stmDocument = stmDocument;
            this.stmDocument.Seek(0, SeekOrigin.Begin);
            this.ofh = new OleFileHeader();
            BinaryReader br = new BinaryReader(this.stmDocument);
            br.Read(this.ofh.signature, 0, 8);
            br.Read(this.ofh.UID, 0, 16);
            this.ofh.RevisionNumber = br.ReadUInt16();
            this.ofh.VersionNumber = br.ReadUInt16();
            this.ofh.ByteOrder = br.ReadUInt16();
            this.ofh.SizeOfSector = (UInt16)Math.Pow(2, br.ReadUInt16());
            this.ofh.SizeOfShortSector = (UInt16)Math.Pow(2, br.ReadUInt16());
            br.ReadBytes(10);
            this.ofh.NumberOfSectorsSAT = br.ReadUInt32();
            this.ofh.FirstSecIDDirectory = br.ReadInt32();
            br.ReadBytes(4);
            this.ofh.MinSizeOfStandardStream = br.ReadUInt32();
            this.ofh.FirstSecIDSSAT = br.ReadInt32();
            this.ofh.NumberOfSectorsSSAT = br.ReadUInt32();
            this.ofh.FirstSecIDMSAT = br.ReadInt32();
            this.ofh.NumberOfSectorsMSAT = br.ReadUInt32();
            for (int i = 0; i < 109; i++)
            {
                this.ofh.FirstPartOfMSAT[i] = br.ReadInt32();
            }
        }

        public bool isValid()
        {
            for (int i = 0; i < OleDocument.magic_number.Length; i++)
            {
                if (this.ofh.signature[i] != OleDocument.magic_number[i])
                {
                    return false;
                }
            }
            return true;
        }

        public void Close()
        {
            this.stmDocument.Close();
        }

        public UInt32 SectorOffset(Int32 SecID)
        {
            return (UInt32)(512 + SecID * this.ofh.SizeOfSector);
        }

        public void readMSAT()
        {
            this.MSAT = new Int32[this.ofh.NumberOfSectorsSAT];
            for (int i = 0; i < 109 && i < this.ofh.NumberOfSectorsSAT; i++)
            {
                if (this.ofh.FirstPartOfMSAT[i] == (Int32)SecIDType.EOFSecID)
                {
                    throw new Exception("Se encontró el fin del MSAT antes de lo esperado");
                }
                this.MSAT[i] = this.ofh.FirstPartOfMSAT[i];
            }
            if (this.ofh.NumberOfSectorsSAT > 109 &&
                this.ofh.NumberOfSectorsMSAT != 0 &&
                this.ofh.FirstSecIDMSAT >= 0)
            {
                //Must not use using clausule because it closes the main stream
                BinaryReader br = new BinaryReader(this.stmDocument);
                int nextSecID = this.ofh.FirstSecIDMSAT;
                int j = 109;
                while (nextSecID != (int)SecIDType.EOFSecID && j < this.ofh.NumberOfSectorsSAT)
                {
                    this.stmDocument.Seek(SectorOffset(nextSecID), SeekOrigin.Begin);
                    for (int i = 0; i < ((this.ofh.SizeOfSector - 4) / 4) && j < this.ofh.NumberOfSectorsSAT; i++, j++)
                    {
                        this.MSAT[j] = br.ReadInt32();
                    }
                    nextSecID = br.ReadInt32();
                }
            }
        }

        public void readSAT()
        {
            BinaryReader br = new BinaryReader(this.stmDocument);
            this.SAT = new Int32[this.ofh.NumberOfSectorsSAT * this.ofh.SizeOfSector];
            int p = 0;
            for (int i = 0; i < this.ofh.NumberOfSectorsSAT; i++)
            {
                this.stmDocument.Seek(SectorOffset(MSAT[i]), SeekOrigin.Begin);
                for (int j = 0; j < this.ofh.SizeOfSector / 4; j++, p++)
                {
                    this.SAT[p] = br.ReadInt32();
                }
            }
        }

        public void readSSAT()
        {
            BinaryReader br = new BinaryReader(this.stmDocument);
            this.SSAT = new Int32[this.ofh.NumberOfSectorsSSAT * this.ofh.SizeOfSector];

            int p = 0;
            int nextSecID = this.ofh.FirstSecIDSSAT;
            for (int i = 0; i < this.ofh.NumberOfSectorsSSAT; i++)
            {
                if (nextSecID < 0)
                {
                    throw new Exception("Error leyendo secuencia SSAT");
                }
                this.stmDocument.Seek(SectorOffset(nextSecID), SeekOrigin.Begin);
                for (int j = 0; j < this.ofh.SizeOfSector / 4; j++, p++)
                {
                    this.SSAT[p] = br.ReadInt32();
                }
                nextSecID = SAT[nextSecID];
            }
        }

        public void readDir()
        {
            BinaryReader br = new BinaryReader(this.stmDocument);
            this.DirEntries = new List<DirEntry>();
            Int32 NextSecID = this.ofh.FirstSecIDDirectory;
            while (NextSecID >= 0)
            {
                this.stmDocument.Seek(SectorOffset(NextSecID), SeekOrigin.Begin);

                for (int i = 0; i < (this.ofh.SizeOfSector / 128); i++)
                {
                    DirEntry Directorio = new DirEntry();

                    Directorio.EntryName = br.ReadBytes(64);
                    Directorio.EntryNameLength = br.ReadUInt16();
                    Directorio.EntryType = br.ReadByte();
                    Directorio.NodeColourEntry = br.ReadByte();
                    Directorio.DirIDLeftChild = br.ReadInt32();
                    Directorio.DirIDRigthChild = br.ReadInt32();
                    Directorio.DirIDRootNodeEntry = br.ReadInt32();
                    Directorio.UID = br.ReadBytes(16);
                    Directorio.flags = br.ReadInt32();
                    Directorio.CreationTime = br.ReadBytes(8);
                    Directorio.ModificationTime = br.ReadBytes(8);
                    Directorio.FirstSecIDStream = br.ReadInt32();
                    Directorio.LengthStream = br.ReadInt32();
                    Directorio.Reserved = br.ReadInt32();
                    this.DirEntries.Add(Directorio);
                }

                if (NextSecID == this.SAT[NextSecID])
                    return;
                NextSecID = this.SAT[NextSecID];
            }
        }

        private DirEntry SearchStream(String strStreamName)
        {
            if (this.DirEntries.Count > 0)
            {
                List<string> lstStreamPath = new List<string>(strStreamName.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries));
                List<int> nodos = new List<int>();
                nodos.Add(this.DirEntries[0].DirIDRootNodeEntry);
                while (nodos.Count > 0)
                {
                    int i = nodos[0];
                    nodos.RemoveAt(0);
                    String nombre = Encoding.Unicode.GetString(this.DirEntries[i].EntryName);
                    if (nombre.IndexOf('\0') > 0)
                        nombre = nombre.Substring(0, nombre.IndexOf('\0'));
                    nombre = nombre.Replace("\x05", "").Replace("\x01", "");
                    if (lstStreamPath[0] == nombre)
                    {

                        if (lstStreamPath.Count > 0 && this.DirEntries[i].DirIDRootNodeEntry != -1)
                        {
                            lstStreamPath.RemoveAt(0);

                            nodos.Clear();
                            nodos.Add(this.DirEntries[i].DirIDRootNodeEntry);
                        }

                        else if (this.DirEntries[i].LengthStream > 0)
                        {
                            return this.DirEntries[i];
                        }
                    }
                    else
                    {
                        if (this.DirEntries[i].DirIDLeftChild != -1)
                            nodos.Add(this.DirEntries[i].DirIDLeftChild);
                        if (this.DirEntries[i].DirIDRigthChild != -1)
                            nodos.Add(this.DirEntries[i].DirIDRigthChild);
                    }
                }
            }
            return null;
        }

        private List<DirEntry> SearchStreams(String strStreamName)
        {
            List<DirEntry> lstDE = new List<DirEntry>();
            if (this.DirEntries.Count > 0)
            {
                List<int> nodos = new List<int>();
                nodos.Add(this.DirEntries[0].DirIDRootNodeEntry);
                while (nodos.Count > 0)
                {
                    int i = nodos[0];
                    nodos.RemoveAt(0);
                    String nombre = Encoding.Unicode.GetString(this.DirEntries[i].EntryName);
                    if (nombre.IndexOf('\0') > 0)
                        nombre = nombre.Substring(0, nombre.IndexOf('\0'));
                    nombre = nombre.Replace("\x05", "").Replace("\x01", "");
                    if (strStreamName == nombre)
                    {
                        lstDE.Add(this.DirEntries[i]);
                    }
                    if (this.DirEntries[i].DirIDLeftChild != -1)
                        nodos.Add(this.DirEntries[i].DirIDLeftChild);
                    if (this.DirEntries[i].DirIDRigthChild != -1)
                        nodos.Add(this.DirEntries[i].DirIDRigthChild);
                    if (this.DirEntries[i].DirIDRootNodeEntry != -1)
                        nodos.Add(this.DirEntries[i].DirIDRootNodeEntry);
                }
            }
            return lstDE;
        }
        private Stream OpenStream(DirEntry de)
        {
            Stream s = new MemoryStream();
            if (de.LengthStream < this.ofh.MinSizeOfStandardStream)
            {
                Stream SSTATBytes = new MemoryStream();
                Int32 NextSecID = this.DirEntries[0].FirstSecIDStream;
                byte[] buffer = new byte[this.ofh.SizeOfSector];

                while (NextSecID != (Int32)SecIDType.EOFSecID)
                {
                    this.stmDocument.Seek(SectorOffset(NextSecID), SeekOrigin.Begin);
                    this.stmDocument.Read(buffer, 0, this.ofh.SizeOfSector);
                    SSTATBytes.Write(buffer, 0, this.ofh.SizeOfSector);
                    NextSecID = this.SAT[NextSecID];
                }
                NextSecID = de.FirstSecIDStream;
                buffer = new byte[this.ofh.SizeOfShortSector];

                while (NextSecID != (Int32)SecIDType.EOFSecID)
                {
                    SSTATBytes.Seek(NextSecID * this.ofh.SizeOfShortSector, SeekOrigin.Begin);
                    SSTATBytes.Read(buffer, 0, this.ofh.SizeOfShortSector);
                    s.Write(buffer, 0, this.ofh.SizeOfShortSector);
                    NextSecID = this.SSAT[NextSecID];
                }
                SSTATBytes.Close();
            }
            else
            {
                Int32 NextSecID = de.FirstSecIDStream;
                byte[] buffer = new byte[this.ofh.SizeOfSector];
                while (NextSecID != (Int32)SecIDType.EOFSecID)
                {
                    this.stmDocument.Seek(SectorOffset(NextSecID), SeekOrigin.Begin);
                    this.stmDocument.Read(buffer, 0, this.ofh.SizeOfSector);
                    s.Write(buffer, 0, this.ofh.SizeOfSector);
                    for (; this.SAT[NextSecID] == (Int32)SecIDType.FreeSecID; NextSecID++) ;
                    NextSecID = this.SAT[NextSecID];
                }
            }
            return s;
        }

        public Stream OpenStream(String strStreamName)
        {
            DirEntry de;
            if ((de = SearchStream(strStreamName)) != null)
            {
                return OpenStream(de);
            }
            else
                return null;
        }

        public void GetOperatingSystemFromStreamHeaders(out byte high, out byte low)
        {
            high = low = 0;
            List<DirEntry> lstStreams = new List<DirEntry>();
            lstStreams.AddRange(SearchStreams("SummaryInformation"));
            lstStreams.AddRange(SearchStreams("DocumentSummaryInformation"));
            foreach (DirEntry de in lstStreams)
            {
                using (Stream SummaryInformation = OpenStream(de))
                {
                    if (SummaryInformation != null)
                    {
                        SummaryInformation.Seek(0, SeekOrigin.Begin);
                        OleStream os = new OleStream(SummaryInformation);
                        os.GetOperatingSystem(out high, out low);
                        if (high != 0 || low != 0) break;
                    }
                }
            }
        }
    }

    public class OleStream
    {
        public class Header
        {
            public class SectionEntry
            {
                public Byte[] UID = new Byte[16];
                public UInt32 Offset;
            }
            public Int16 Unicode;
            public Int16 Zeros;
            public Byte OSH;
            public Byte OSL;
            public Byte BuildVersion;
            public Byte dwPlatformId;
            public Byte[] ClassID = new Byte[16];
            public UInt32 SectionCount;
            public SectionEntry[] Sections;

            public Header(Stream s)
            {
                BinaryReader br = new BinaryReader(s);
                this.Unicode = br.ReadInt16();
                this.Zeros = br.ReadInt16();
                this.OSH = br.ReadByte();
                this.OSL = br.ReadByte();
                this.BuildVersion = br.ReadByte();
                this.dwPlatformId = br.ReadByte();
                this.ClassID = br.ReadBytes(16);
                this.SectionCount = br.ReadUInt32();
                this.Sections = new SectionEntry[this.SectionCount];
                for (int i = 0; i < this.SectionCount; i++)
                {
                    this.Sections[i] = new SectionEntry();
                    this.Sections[i].UID = br.ReadBytes(16);
                    this.Sections[i].Offset = br.ReadUInt32();
                }
            }
        }
        public class Body
        {
            public static byte[] UIDSummaryInformation = new byte[16] { 0xE0, 0x85, 0x9F, 0xF2, 0xF9, 0x4F, 0x68, 0x10, 0xAB, 0x91, 0x08, 0x00, 0x2B, 0x27, 0xB3, 0xD9 };
            public static byte[] UIDDocumentSummaryInformation = new byte[16] { 0x02, 0xd5, 0xcd, 0xd5, 0x9c, 0x2e, 0x1b, 0x10, 0x93, 0x97, 0x08, 0x00, 0x2b, 0x2c, 0xf9, 0xae };
            public static byte[] UIDDocumentSummaryInformationCustom = new byte[16] { 0x05, 0xd5, 0xcd, 0xd5, 0x9c, 0x2e, 0x1b, 0x10, 0x93, 0x97, 0x08, 0x00, 0x2b, 0x2c, 0xf9, 0xae };

            public class PropertyEntry
            {
                public const Int32 SUMMARYINFORMATION_CODEPAGE = 1;
                public const Int32 SUMMARYINFORMATION_TITULO = 2;
                public const Int32 SUMMARYINFORMATION_SUBJECT = 3;
                public const Int32 SUMMARYINFORMATION_AUTHOR = 4;
                public const Int32 SUMMARYINFORMATION_KEYWORDS = 5;
                public const Int32 SUMMARYINFORMATION_COMMENTS = 6;
                public const Int32 SUMMARYINFORMATION_TEMPLATE = 7;
                public const Int32 SUMMARYINFORMATION_LASTSAVEDBY = 8;
                public const Int32 SUMMARYINFORMATION_REVISIONNUMBER = 9;
                public const Int32 SUMMARYINFORMATION_TOTALEDITINGTIME = 10;
                public const Int32 SUMMARYINFORMATION_LASTPRINTED = 11;
                public const Int32 SUMMARYINFORMATION_CREATETIME = 12;
                public const Int32 SUMMARYINFORMATION_LASTSAVEDTIME = 13;
                public const Int32 SUMMARYINFORMATION_NUMBER_OF_PAGES = 14;
                public const Int32 SUMMARYINFORMATION_NUMBER_OF_WORDS = 15;
                public const Int32 SUMMARYINFORMATION_NUMBER_OF_CHARACTERS = 16;
                public const Int32 SUMMARYINFORMATION_THUMBNAIL = 17;
                public const Int32 SUMMARYINFORMATION_APPLICATION = 18;
                public const Int32 SUMMARYINFORMATION_SECURITY = 19;

                public const Int32 DOCUMENTSUMMARYINFORMATION_CATEGORY = 2;
                public const Int32 DOCUMENTSUMMARYINFORMATION_PRESENTATIONTARGET = 3;
                public const Int32 DOCUMENTSUMMARYINFORMATION_BYTES = 4;
                public const Int32 DOCUMENTSUMMARYINFORMATION_LINES = 5;
                public const Int32 DOCUMENTSUMMARYINFORMATION_PARAGRAPHS = 6;
                public const Int32 DOCUMENTSUMMARYINFORMATION_SLIDES = 7;
                public const Int32 DOCUMENTSUMMARYINFORMATION_NOTES = 8;
                public const Int32 DOCUMENTSUMMARYINFORMATION_HIDDENSLIDES = 9;
                public const Int32 DOCUMENTSUMMARYINFORMATION_MMCLIPS = 10;
                public const Int32 DOCUMENTSUMMARYINFORMATION_SCALECROP = 11;
                public const Int32 DOCUMENTSUMMARYINFORMATION_HEADINGPAIRS = 12;
                public const Int32 DOCUMENTSUMMARYINFORMATION_TITLESOFPARTS = 13;
                public const Int32 DOCUMENTSUMMARYINFORMATION_MANAGER = 14;
                public const Int32 DOCUMENTSUMMARYINFORMATION_COMPANY = 15;
                public const Int32 DOCUMENTSUMMARYINFORMATION_LINKSUPTODATE = 16;

                public UInt32 IdPropiedad;
                public UInt32 Offset;

                public static String SummaryInformationPropertyToString(UInt32 IdPropiedad)
                {
                    switch (IdPropiedad)
                    {
                        case SUMMARYINFORMATION_CODEPAGE: return "Encoding";
                        case SUMMARYINFORMATION_TITULO: return "Title";
                        case SUMMARYINFORMATION_SUBJECT: return "Subject";
                        case SUMMARYINFORMATION_AUTHOR: return "Author";
                        case SUMMARYINFORMATION_KEYWORDS: return "Keywords";
                        case SUMMARYINFORMATION_COMMENTS: return "Comments";
                        case SUMMARYINFORMATION_TEMPLATE: return "Template";
                        case SUMMARYINFORMATION_LASTSAVEDBY: return "Last saved by";
                        case SUMMARYINFORMATION_REVISIONNUMBER: return "Revision";
                        case SUMMARYINFORMATION_TOTALEDITINGTIME: return "Editing time";
                        case SUMMARYINFORMATION_LASTPRINTED: return "Last printed";
                        case SUMMARYINFORMATION_CREATETIME: return "Create date";
                        case SUMMARYINFORMATION_LASTSAVEDTIME: return "Last saved";
                        case SUMMARYINFORMATION_NUMBER_OF_PAGES: return "Pages";
                        case SUMMARYINFORMATION_NUMBER_OF_WORDS: return "Words";
                        case SUMMARYINFORMATION_NUMBER_OF_CHARACTERS: return "Characters";
                        case SUMMARYINFORMATION_THUMBNAIL: return "Thumbnail";
                        case SUMMARYINFORMATION_APPLICATION: return "Application";
                        case SUMMARYINFORMATION_SECURITY: return "Security";
                        default: return string.Empty;
                    }
                }

                public static String DocumentSummaryInformationPropertyToString(UInt32 IdPropiedad)
                {
                    switch (IdPropiedad)
                    {
                        case DOCUMENTSUMMARYINFORMATION_CATEGORY: return "Category";
                        case DOCUMENTSUMMARYINFORMATION_PRESENTATIONTARGET: return "Presentation target";
                        case DOCUMENTSUMMARYINFORMATION_BYTES: return "Bytes";
                        case DOCUMENTSUMMARYINFORMATION_LINES: return "Lines";
                        case DOCUMENTSUMMARYINFORMATION_PARAGRAPHS: return "Paragraphs";
                        case DOCUMENTSUMMARYINFORMATION_SLIDES: return "Slides";
                        case DOCUMENTSUMMARYINFORMATION_NOTES: return "Notes";
                        case DOCUMENTSUMMARYINFORMATION_HIDDENSLIDES: return "Hidden slides";
                        case DOCUMENTSUMMARYINFORMATION_MMCLIPS: return "Multimedia clips";
                        case DOCUMENTSUMMARYINFORMATION_SCALECROP: return "Scale";
                        case DOCUMENTSUMMARYINFORMATION_HEADINGPAIRS: return "Headers";
                        case DOCUMENTSUMMARYINFORMATION_TITLESOFPARTS: return "Titles";
                        case DOCUMENTSUMMARYINFORMATION_MANAGER: return "Manager";
                        case DOCUMENTSUMMARYINFORMATION_COMPANY: return "Company";
                        case DOCUMENTSUMMARYINFORMATION_LINKSUPTODATE: return "Links";
                        default: return string.Empty;
                    }
                }
            }

            public struct Property
            {
                public const Int32 PROPERTY_TYPE_VT_EMPTY = 0;
                public const Int32 PROPERTY_TYPE_VT_NULL = 1;
                public const Int32 PROPERTY_TYPE_VT_I1 = 16;
                public const Int32 PROPERTY_TYPE_VT_UI1 = 17;
                public const Int32 PROPERTY_TYPE_VT_I2 = 2;
                public const Int32 PROPERTY_TYPE_VT_UI2 = 18;
                public const Int32 PROPERTY_TYPE_VT_I4 = 3;
                public const Int32 PROPERTY_TYPE_VT_UI4 = 19;
                public const Int32 PROPERTY_TYPE_VT_INT = 22;
                public const Int32 PROPERTY_TYPE_VT_UINT = 23;
                public const Int32 PROPERTY_TYPE_VT_I8 = 20;
                public const Int32 PROPERTY_TYPE_VT_UI8 = 21;
                public const Int32 PROPERTY_TYPE_VT_BOOL = 11;
                public const Int32 PROPERTY_TYPE_VT_LPSTR = 30;
                public const Int32 PROPERTY_TYPE_VT_LPWSTR = 31;
                public const Int32 PROPERTY_TYPE_VT_BSTR = 8;
                public const Int32 PROPERTY_TYPE_VT_FILETIME = 64;
                public const Int32 PROPERTY_TYPE_VT_VARIANT = 12;
                public const Int32 PROPERTY_TYPE_VT_VECTOR = 0x1000;
                public const Int32 PROPERTY_TYPE_VT_BLOB = 65;

                public Int32 Type;
            }

            public Byte[] UID = new Byte[16];
            public UInt32 Offset;
            public UInt32 TotalSize;
            public UInt32 NumeroDePropiedades;
            public PropertyEntry[] Propiedades;

            public Body(Stream s, Byte[] UIDSection, UInt32 OffsetStart)
            {
                this.UID = UIDSection;
                this.Offset = OffsetStart;

                BinaryReader br = new BinaryReader(s);
                this.TotalSize = br.ReadUInt32();
                this.NumeroDePropiedades = br.ReadUInt32();
                this.Propiedades = new PropertyEntry[NumeroDePropiedades];
                for (int i = 0; i < Propiedades.Length; i++)
                {
                    Propiedades[i] = new PropertyEntry();
                    Propiedades[i].IdPropiedad = br.ReadUInt32();
                    Propiedades[i].Offset = br.ReadUInt32();
                }
            }
        }

        Header Cabecera;
        Body[] Cuerpos;
        Stream stream;

        public OleStream(Stream s)
        {
            this.stream = s;
            this.stream.Seek(0, SeekOrigin.Begin);
            Cabecera = new Header(this.stream);
            Cuerpos = new Body[Cabecera.SectionCount];
            for (int i = 0; i < Cabecera.SectionCount; i++)
            {
                this.stream.Seek(Cabecera.Sections[i].Offset, SeekOrigin.Begin);
                Cuerpos[i] = new Body(this.stream, Cabecera.Sections[i].UID, Cabecera.Sections[i].Offset);
            }
        }

        private string CodePageToCodification(int codepage)
        {
            switch (codepage)
            {
                case 1250:
                    return "Center Europe";
                case 1251:
                    return "Cyrillic";
                case 1252:
                    return "Latin I";
                case 1253:
                    return "Greek";
                case 1254:
                    return "Turkish";
                case 1255:
                    return "Hebrew";
                case 1256:
                    return "Arabic";
                case 1257:
                    return "Baltic";
                case 1258:
                    return "Vietnam";
                case 874:
                    return "Thai";
                case 832:
                    return "Cyrillic";
                case 936:
                    return "Cyrillic";
                case 949:
                    return "Cyrillic";
                case 950:
                    return "Cyrillic";
                default:
                    return "Unknown";
            }
        }

        private bool CompareUID(Byte[] UID1, Byte[] UID2)
        {
            if (UID1.Length != UID2.Length)
                return false;
            for (int i = 0; i < UID1.Length; i++)
            {
                if (UID1[i] != UID2[i])
                    return false;
            }
            return true;
        }


        private String GetValueType(Int64 Offset, Int32 Tipo)
        {
            try
            {
                this.stream.Seek(Offset, SeekOrigin.Begin);
                BinaryReader br = new BinaryReader(this.stream);
                switch (Tipo)
                {
                    case Body.Property.PROPERTY_TYPE_VT_VARIANT:
                        Tipo = br.ReadInt32();
                        return GetValueType(this.stream.Position, Tipo);
                    case Body.Property.PROPERTY_TYPE_VT_EMPTY:
                        return string.Empty;
                    case Body.Property.PROPERTY_TYPE_VT_NULL:
                        br.ReadByte();
                        return string.Empty;
                    case Body.Property.PROPERTY_TYPE_VT_I1:
                        return br.ReadSByte().ToString();
                    case Body.Property.PROPERTY_TYPE_VT_UI1:
                        return br.ReadByte().ToString();
                    case Body.Property.PROPERTY_TYPE_VT_I2:
                        return br.ReadInt16().ToString();
                    case Body.Property.PROPERTY_TYPE_VT_UI2:
                        return br.ReadUInt16().ToString();
                    case Body.Property.PROPERTY_TYPE_VT_I4:
                    case Body.Property.PROPERTY_TYPE_VT_INT:
                        return br.ReadInt32().ToString();
                    case Body.Property.PROPERTY_TYPE_VT_UI4:
                    case Body.Property.PROPERTY_TYPE_VT_UINT:
                        return br.ReadUInt32().ToString();
                    case Body.Property.PROPERTY_TYPE_VT_I8:
                        return br.ReadInt64().ToString();
                    case Body.Property.PROPERTY_TYPE_VT_UI8:
                        return br.ReadUInt64().ToString();
                    case Body.Property.PROPERTY_TYPE_VT_BOOL:
                        return br.ReadBoolean().ToString();
                    case Body.Property.PROPERTY_TYPE_VT_BSTR:
                        UInt32 SizeUnicode = br.ReadUInt32();
                        Byte[] BytesUnicode = br.ReadBytes((int)SizeUnicode);
                        return Encoding.Unicode.GetString(BytesUnicode, 0, (int)SizeUnicode);
                    case Body.Property.PROPERTY_TYPE_VT_LPSTR:
                        UInt32 Size = br.ReadUInt32();
                        if (Size > 0)
                            return Encoding.Default.GetString(br.ReadBytes((int)Size)).Replace('\0', ' ');
                        else
                            return string.Empty;
                    case Body.Property.PROPERTY_TYPE_VT_LPWSTR:
                        UInt32 SizeW = br.ReadUInt32();
                        if (SizeW > 0)
                            return Encoding.Unicode.GetString(br.ReadBytes((int)SizeW * 2)).Replace('\0', ' ');
                        else
                            return string.Empty;
                    case Body.Property.PROPERTY_TYPE_VT_FILETIME:
                        Int64 datetime = br.ReadInt64();
                        return datetime != 0 ? DateTime.FromFileTimeUtc(datetime).ToString() : string.Empty;
                    case Body.Property.PROPERTY_TYPE_VT_BLOB:
                        Size = br.ReadUInt32();
                        if (Size > 0 && Size < 128)
                        {
                            String s = Encoding.Default.GetString(br.ReadBytes((int)Size)).Replace('\0', ' ');

                            for (int i = 0; i < s.Length;)
                                if (char.IsControl(s[i])) s = s.Remove(i, 1);
                                else i++;
                            return s;
                        }
                        else
                            return string.Empty;
                    default:
                        return "Ole Property not supported: " + Tipo;
                }
            }
            catch
            {
                return string.Empty;
            }
        }


        private String GetValue(Int64 Offset)
        {
            this.stream.Seek(Offset, SeekOrigin.Begin);
            BinaryReader br = new BinaryReader(this.stream);
            Body.Property p;
            p.Type = br.ReadInt32();
            if ((p.Type & Body.Property.PROPERTY_TYPE_VT_VECTOR) == Body.Property.PROPERTY_TYPE_VT_VECTOR)
            {
                UInt32 Valores = br.ReadUInt32();
                String Valor = string.Empty;
                for (int i = 0; i < Valores; i++)
                {
                    if (i > 0 && Valor != string.Empty)
                    {
                        Valor += Environment.NewLine + "\t";
                    }
                    Valor += GetValueType(this.stream.Position, p.Type & 0x0FFF);
                }
                return Valor;
            }
            else
                return GetValueType(this.stream.Position, p.Type & 0x0FFF);
        }

        public void GetOperatingSystem(out byte high, out byte low)
        {
            high = Cabecera.OSH;
            low = Cabecera.OSL;
        }

        public void GetMetadata(FileMetadata foundMetadata)
        {
            String strCreator = string.Empty;
            String strLastModifiedBy = string.Empty;

            foreach (Body Cuerpo in this.Cuerpos)
            {
                if (CompareUID(Cuerpo.UID, Body.UIDSummaryInformation))
                {
                    foreach (Body.PropertyEntry Propiedad in Cuerpo.Propiedades)
                    {
                        String Valor = this.GetValue(Cuerpo.Offset + Propiedad.Offset);
                        switch (Propiedad.IdPropiedad)
                        {
                            case Body.PropertyEntry.SUMMARYINFORMATION_TITULO:
                                if (!string.IsNullOrEmpty(Valor.Trim()))
                                    foundMetadata.Title = Valor;
                                break;
                            case Body.PropertyEntry.SUMMARYINFORMATION_SUBJECT:
                                if (!string.IsNullOrEmpty(Valor.Trim()))
                                    foundMetadata.Subject = Valor;
                                break;
                            case Body.PropertyEntry.SUMMARYINFORMATION_NUMBER_OF_PAGES:
                            case Body.PropertyEntry.SUMMARYINFORMATION_NUMBER_OF_WORDS:
                            case Body.PropertyEntry.SUMMARYINFORMATION_NUMBER_OF_CHARACTERS:
                                if (!string.IsNullOrEmpty(Valor.Trim()))
                                    foundMetadata.Statistic += Body.PropertyEntry.SummaryInformationPropertyToString(Propiedad.IdPropiedad) + ": " + Valor + "  ";
                                break;
                            case Body.PropertyEntry.SUMMARYINFORMATION_APPLICATION:
                                if (!string.IsNullOrEmpty(Valor.Trim()))
                                    foundMetadata.Add(new Application(Analysis.ApplicationAnalysis.GetApplicationsFromString(Valor, this.Cabecera.OSH == 3 && this.Cabecera.OSL == 10)));
                                break;
                            case Body.PropertyEntry.SUMMARYINFORMATION_KEYWORDS:
                                if (!string.IsNullOrEmpty(Valor.Trim()))
                                    foundMetadata.Keywords = Valor;
                                break;
                            case Body.PropertyEntry.SUMMARYINFORMATION_COMMENTS:
                                if (!string.IsNullOrEmpty(Valor.Trim()))
                                    foundMetadata.Comments = Valor;
                                break;
                            case Body.PropertyEntry.SUMMARYINFORMATION_TEMPLATE:
                                if (!string.IsNullOrEmpty(Valor.Trim()))
                                    foundMetadata.Template = Valor;
                                break;
                            case Body.PropertyEntry.SUMMARYINFORMATION_REVISIONNUMBER:
                                if (!string.IsNullOrEmpty(Valor.Trim()))
                                {
                                    Decimal d;
                                    if (Decimal.TryParse(Valor, out d))
                                        foundMetadata.VersionNumber = d;
                                }
                                break;
                            case Body.PropertyEntry.SUMMARYINFORMATION_CREATETIME:
                                if (!string.IsNullOrEmpty(Valor.Trim()))
                                {
                                    DateTime d;
                                    if (DateTime.TryParse(Valor, out d))
                                    {
                                        foundMetadata.Dates.CreationDate = d.ToLocalTime();
                                    }
                                }
                                break;
                            case Body.PropertyEntry.SUMMARYINFORMATION_LASTSAVEDTIME:
                                if (!string.IsNullOrEmpty(Valor.Trim()))
                                {
                                    DateTime d;
                                    if (DateTime.TryParse(Valor, out d))
                                    {
                                        foundMetadata.Dates.ModificationDate = d.ToLocalTime();
                                    }
                                }
                                break;
                            case Body.PropertyEntry.SUMMARYINFORMATION_LASTPRINTED:
                                if (!string.IsNullOrEmpty(Valor.Trim()))
                                {
                                    DateTime d;
                                    if (DateTime.TryParse(Valor, out d))
                                    {
                                        foundMetadata.Dates.PrintingDate = d.ToLocalTime();
                                    }
                                }
                                break;
                            case Body.PropertyEntry.SUMMARYINFORMATION_TOTALEDITINGTIME:
                                if (!string.IsNullOrEmpty(Valor.Trim()))
                                {
                                    DateTime d;
                                    if (DateTime.TryParse(Valor, out d))
                                    {
                                        d = new DateTime(d.ToFileTimeUtc());
                                        foundMetadata.EditTime = d.Ticks;
                                    }
                                }
                                break;
                            case Body.PropertyEntry.SUMMARYINFORMATION_AUTHOR:
                                if (!string.IsNullOrEmpty(Valor.Trim()))
                                {
                                    strCreator = Valor;
                                }
                                break;
                            case Body.PropertyEntry.SUMMARYINFORMATION_LASTSAVEDBY:
                                if (!string.IsNullOrEmpty(Valor.Trim()))
                                {
                                    strLastModifiedBy = Valor;
                                }
                                break;
                            case Body.PropertyEntry.SUMMARYINFORMATION_CODEPAGE:
                                Valor = CodePageToCodification(Convert.ToInt32(Valor));
                                if (!string.IsNullOrEmpty(Valor.Trim()))
                                    foundMetadata.Codification = Valor;
                                break;
                        }
                    }
                }
                else if (CompareUID(Cuerpo.UID, Body.UIDDocumentSummaryInformation))
                {
                    foreach (Body.PropertyEntry Propiedad in Cuerpo.Propiedades)
                    {
                        String Valor = this.GetValue(Cuerpo.Offset + Propiedad.Offset);
                        switch (Propiedad.IdPropiedad)
                        {
                            case Body.PropertyEntry.DOCUMENTSUMMARYINFORMATION_CATEGORY:
                                if (!string.IsNullOrEmpty(Valor.Trim()))
                                    foundMetadata.Category = Valor;
                                break;
                            case Body.PropertyEntry.DOCUMENTSUMMARYINFORMATION_BYTES:
                            case Body.PropertyEntry.DOCUMENTSUMMARYINFORMATION_LINES:
                            case Body.PropertyEntry.DOCUMENTSUMMARYINFORMATION_PARAGRAPHS:
                            case Body.PropertyEntry.DOCUMENTSUMMARYINFORMATION_SLIDES:
                            case Body.PropertyEntry.DOCUMENTSUMMARYINFORMATION_NOTES:
                            case Body.PropertyEntry.DOCUMENTSUMMARYINFORMATION_HIDDENSLIDES:
                            case Body.PropertyEntry.DOCUMENTSUMMARYINFORMATION_MMCLIPS:
                                if (!string.IsNullOrEmpty(Valor.Trim()))
                                    foundMetadata.Statistic += Body.PropertyEntry.DocumentSummaryInformationPropertyToString(Propiedad.IdPropiedad) + ": " + Valor + "  ";
                                break;
                            case Body.PropertyEntry.DOCUMENTSUMMARYINFORMATION_MANAGER:
                                if (!string.IsNullOrEmpty(Valor.Trim()))
                                {
                                    foundMetadata.Add(new User(Valor, false));
                                }
                                break;
                            case Body.PropertyEntry.DOCUMENTSUMMARYINFORMATION_COMPANY:
                                if (!string.IsNullOrEmpty(Valor.Trim()))
                                    foundMetadata.Company = Valor;
                                break;
                            case Body.PropertyEntry.DOCUMENTSUMMARYINFORMATION_HEADINGPAIRS: break;
                            case Body.PropertyEntry.DOCUMENTSUMMARYINFORMATION_TITLESOFPARTS: break;
                        }
                    }
                }
                else if (CompareUID(Cuerpo.UID, Body.UIDDocumentSummaryInformationCustom))
                {
                    if (Cuerpo.Propiedades[0].IdPropiedad == 0)
                    {
                        this.stream.Seek(Cuerpo.Offset + Cuerpo.Propiedades[1].Offset, SeekOrigin.Begin);
                        BinaryReader br = new BinaryReader(this.stream);
                        br.ReadInt32();
                        Boolean UnicodeNames = br.ReadInt32() == 0x4B0;
                        this.stream.Seek(Cuerpo.Offset + Cuerpo.Propiedades[0].Offset, SeekOrigin.Begin);
                        foundMetadata.UserInfo = string.Empty;
                        for (int NumberOfCustomProperties = br.ReadInt32(); NumberOfCustomProperties > 0; NumberOfCustomProperties--)
                        {
                            int PropertyNumber = br.ReadInt32();
                            int CustomPropertyNameLength = br.ReadInt32();
                            String Name = UnicodeNames ? Encoding.Unicode.GetString(br.ReadBytes(CustomPropertyNameLength * 2)).Trim('\0') : Encoding.UTF8.GetString(br.ReadBytes(CustomPropertyNameLength)).Trim('\0');
                            if (PropertyNumber < Cuerpo.NumeroDePropiedades)
                            {
                                int i;
                                for (i = 0; i < Cuerpo.Propiedades.Length && Cuerpo.Propiedades[i].IdPropiedad != PropertyNumber; i++) ;
                                if (i < Cuerpo.Propiedades.Length)
                                {
                                    long pos = this.stream.Position;

                                    if (UnicodeNames && pos % 4 != 0)
                                        pos += 2;
                                    String Valor = this.GetValue(Cuerpo.Offset + Cuerpo.Propiedades[i].Offset).Trim();
                                    this.stream.Seek(pos, SeekOrigin.Begin);
                                    if (!string.IsNullOrEmpty(Valor.Trim()))
                                    {
                                        foundMetadata.UserInfo += Name + ": " + Valor.Trim() + "\t";
                                        if (Name == "_AuthorEmail")
                                            foundMetadata.Add(new Email(Valor.Trim()));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (!string.IsNullOrEmpty(strLastModifiedBy))
            {
                foundMetadata.Add(new User(strLastModifiedBy, true));
            }
            if (!string.IsNullOrEmpty(strCreator))
            {

                foundMetadata.Add(new User(strCreator, string.IsNullOrEmpty(strLastModifiedBy)));
            }
        }
    }
}
