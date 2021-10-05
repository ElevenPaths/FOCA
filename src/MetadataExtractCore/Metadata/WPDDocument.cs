using MetadataExtractCore.Analysis;
using MetadataExtractCore.Diagrams;
using MetadataExtractCore.Utilities;
using System;
using System.IO;

namespace MetadataExtractCore.Extractors
{
    public class WPDDocument : DocumentExtractor
    {
        private enum MetadataType { SharedResource, Printer, File, Unknown };

        public WPDDocument(Stream stm) : base(stm)
        {
        }

        public override FileMetadata AnalyzeFile()
        {
            try
            {
                this.foundMetadata = new FileMetadata();
                if (IsWPD(this.fileStream))
                {
                    long entryPoint = 0;
                    MetadataType tipo;
                    while ((entryPoint = EntryPointString(this.fileStream, out tipo)) > -1)
                    {
                        this.fileStream.Seek(entryPoint + 2, SeekOrigin.Begin);

                        var aux = ReadBinaryString16(this.fileStream);
                        if (!IsPossibleString(aux)) continue;
                        if (tipo == MetadataType.Unknown && !PathAnalysis.IsValidPath(aux))
                        {
                            if (aux.ToLower().Contains("jet") || aux.ToLower().Contains("printer") || aux.ToLower().Contains("hp") ||
                                aux.ToLower().Contains("series") || aux.ToLower().Contains("canon") || aux.ToLower().Contains("laser") ||
                                aux.ToLower().Contains("epson") || aux.ToLower().Contains("lj") || aux.ToLower().Contains("lexmark") ||
                                aux.ToLower().Contains("xerox") || aux.ToLower().Contains("sharp"))
                            {
                                this.foundMetadata.Add(new Printer(Functions.FilterPrinter(aux)));
                            }
                            else if (aux.ToLower().Contains("acrobat") || aux.ToLower().Contains("adobe") || aux.ToLower().Contains("creator") ||
                                     aux.ToLower().Contains("writer") || aux.ToLower().Contains("pdf") || aux.ToLower().Contains("converter"))
                            {
                                this.foundMetadata.Add(new Application(Functions.FilterPrinter(Analysis.ApplicationAnalysis.GetApplicationsFromString(aux))));
                            }
                        }
                        else
                        {
                            var strPath = Functions.GetPathFolder(aux);
                            if (!PathAnalysis.IsValidPath(strPath)) continue;
                            this.foundMetadata.Add(new Diagrams.Path(PathAnalysis.CleanPath(strPath), true));
                            var strUser = PathAnalysis.ExtractUserFromPath(strPath);
                            if (!string.IsNullOrEmpty(strUser))
                                this.foundMetadata.Add(new User(strUser, true));
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

        private static long EntryPointString(Stream fs, out MetadataType tipo)
        {
            var pos = fs.Position;
            var intBufferLen = (int)(fs.Length - pos);
            var buffer = new byte[intBufferLen];
            fs.Read(buffer, 0, intBufferLen);
            tipo = MetadataType.Unknown;

            try
            {
                for (var i = 0; i < buffer.Length - 2; i++)
                {
                    if ((buffer[i] == 0x00) &
                        (buffer[i + 1] == 0x98)
                       )
                    {
                        return i + pos;
                    }

                    if (buffer.Length < i + 7) continue;
                    if ((buffer[i] == 0x00) &
                        (buffer[i + 1] == 0x01) &
                        (buffer[i + 2] == 0x00) &
                        (buffer[i + 3] > 0x00) &
                        (buffer[i + 4] == 0x00) &
                        (buffer[i + 5] == 0x01) &
                        (buffer[i + 6] == 0x00)
                        )
                    {
                        tipo = MetadataType.File;
                        return i + 5 + pos;
                    }
                }
            }
            catch
            {
            }
            return -1;
        }

        private static string ReadBinaryString16(Stream fs)
        {
            var aux = fs.Position;
            var br = new BinaryReader(fs);
            const long finCadena = 0x00;
            var strCadena = string.Empty;

            short byteReaded = 0;
            while ((byteReaded = br.ReadInt16()) > finCadena)
                strCadena += (char)byteReaded;

            fs.Seek(aux, SeekOrigin.Begin);
            return strCadena;
        }

        private static bool IsPossibleString(string str)
        {
            if (str == null)
                return false;
            if (str.Contains("?"))
                return false;
            if (str.Length == 1)
                return false;

            var chars = "abcdefghijklmnñopqrstuvwxyz0123456789:\\ ";

            return str.IndexOfAny(chars.ToCharArray()) != -1;
        }

        /// <summary>
        /// Check for the WPD header
        /// </summary>
        /// <param name="fs">Stream que contiene un archivo pwd</param>
        /// <returns>true si el archivo es wpd</returns>
        private static bool IsWPD(Stream fs)
        {
            var aux = fs.Position;
            var cabecera = new byte[4];
            fs.Seek(0, SeekOrigin.Begin);
            if (fs.Length < 4)
                return false;
            fs.Read(cabecera, 0, 4);
            //0xFF,'W','P','C'
            if ((cabecera[0] == 0xff) &
                (cabecera[1] == 0x57) &
                (cabecera[2] == 0x50) &
                (cabecera[3] == 0x43))
            {
                fs.Seek(aux, SeekOrigin.Begin);
                return true;
            }
            fs.Seek(aux, SeekOrigin.Begin);
            return false;
        }
    }
}
