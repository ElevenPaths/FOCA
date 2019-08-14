using Ionic.Zip;
using MetadataExtractCore.Extractors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace MetadataExtractCore.Utilities
{
    public class PairValue<T, Y>
    {
        public T x;
        public Y y;

        public PairValue(T x, Y y)
        {
            this.x = x;
            this.y = y;
        }
    }

    [Serializable]

    public class WithoutAcentsStringEqualityComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            return Functions.RemoveAccentsWithNormalization(x.Trim().ToLower()) == Functions.RemoveAccentsWithNormalization(y.Trim().ToLower());
        }

        public int GetHashCode(string obj)
        {
            return Functions.RemoveAccentsWithNormalization(obj.Trim().ToLower()).GetHashCode();
        }
    }

    public static class Functions
    {
        public static string GetFormatFile(Stream stmFile)
        {
            try
            {
                if (stmFile.Length < 8)
                {
                    return string.Empty;
                }

                BinaryReader br = new BinaryReader(stmFile);
                byte[] start = br.ReadBytes(8);

                if (Encoding.ASCII.GetString(start).StartsWith("%PDF-"))
                    return ".pdf";
                byte[] magic_number = new byte[8] { 0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1 };
                bool isdoc = true;
                for (var i = 0; i < magic_number.Length; i++)
                {
                    if (start[i] != magic_number[i])
                    {
                        isdoc = false;
                        break;
                    }
                }
                if (isdoc)
                {
                    OleDocument doc = new OleDocument(stmFile);
                    if (doc.isValid())
                    {
                        doc.readMSAT();
                        doc.readSAT();
                        doc.readSSAT();
                        doc.readDir();
                        if (doc.OpenStream("WordDocument") != null)
                            return ".doc";
                        if (doc.OpenStream("Workbook") != null)
                            return ".xls";
                        if (doc.OpenStream("PowerPoint Document") != null)
                            return ".ppt";
                    }
                }
                ZipFile zipFile = null;
                try
                {
                    zipFile = ZipFile.Read(stmFile);
                    foreach (var s in zipFile.EntryFileNames)
                    {
                        if (s.StartsWith("word"))
                            return ".docx";
                        if (s.StartsWith("xl"))
                            return ".xlsx";
                        if (s.StartsWith("ppt"))
                            return ".pptx";
                    }
                    return ".odt";
                }
                catch
                {
                    return String.Empty;
                }
                finally
                {
                    zipFile?.Dispose();
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Validate if contains letter param.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool StringContainAnyLetter(string str)
        {
            return str.Any(c => char.IsLetter(c));
        }

        /// <summary>
        /// Extract printer from byte.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ExtractPrinterFromBytes(byte[] bytes)
        {
            var printer = string.Empty;
            for (var i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] != '\\' || i + 2 >= bytes.Length || bytes[i + 2] != '\\') continue;
                int j;

                for (j = i; j < bytes.Length; j++)
                {
                    if (bytes[j] == 0 && j + 1 < bytes.Length && bytes[j + 1] == 0)
                    {
                        break;
                    }
                }
                printer = Encoding.Unicode.GetString(bytes, i, j - i).Replace('\0', ' ');

                if (printer.IndexOf(',') > 0)
                    printer = printer.Remove(printer.IndexOf(','));
            }

            return printer;
        }

        /// <summary>
        /// Return path folder.
        /// </summary>
        /// <param name="pathValue"></param>
        /// <returns></returns>
        public static string GetPathFolder(string pathValue)
        {
            return System.IO.Path.GetFileName(pathValue).Length == 0 ? pathValue : pathValue.Replace(System.IO.Path.GetFileName(pathValue), "");
        }

        /// <summary>
        /// Return string to plain text.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToPlainText(string source)
        {
            if (source.Length >= 2 && source[0] == Convert.ToChar(0xFE) && source[1] == Convert.ToChar(0xFF))
            {
                var s = Encoding.ASCII.GetBytes(source);
                return Encoding.BigEndianUnicode.GetString(s, 2, s.Length - 2);
            }
            else if (source.StartsWith(@"\376\377\"))
            {
                return String.Concat(source.Split('\\').Where(p => p.Length == 4).Select(p => p[3]));
            }
            return source;
        }

        /// <summary>
        /// Delete accents from a string
        /// </summary>
        /// <param name="inputString"></param>
        /// <returns></returns>
        public static string RemoveAccentsWithNormalization(string inputString)
        {
            var normalizedString = inputString.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (var item in from item in normalizedString let uc = CharUnicodeInfo.GetUnicodeCategory(item) where uc != UnicodeCategory.NonSpacingMark select item)
            {
                sb.Append(item);
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        /// <summary>
        /// Return printable characters
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetPrintableCharacters(string str)
        {
            var sb = new StringBuilder(str);

            for (var i = sb.Length - 1; i >= 0; i--)
            {
                if (sb[i] < Convert.ToChar(32) || sb[i] > Convert.ToChar(126))
                    sb.Remove(i, 1);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Return filter printer.
        /// </summary>
        /// <param name="printerValue"></param>
        /// <returns></returns>
        public static string FilterPrinter(string printerValue)
        {
            printerValue = GetPrintableCharacters(printerValue).Trim();
            if (printerValue.ToLower().StartsWith(@"\\ipp://"))
                printerValue = @"\\" + printerValue.Remove(0, @"\\ipp://".Length);
            if (printerValue.ToLower().StartsWith(@"ipp://"))
                printerValue = @"\\" + printerValue.Remove(0, @"ipp://".Length);
            return printerValue;
        }

        /// <summary>
        /// Copy Stream
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public static void CopyStream(Stream input, Stream output)
        {
            var lngInputPosition = input.Position;
            var lngOutputPosition = output.Position;
            var buffer = new byte[32768];
            while (true)
            {
                var read = input.Read(buffer, 0, buffer.Length);
                if (read <= 0)
                    break;
                output.Write(buffer, 0, read);
            }
            input.Position = lngInputPosition;
            output.Position = lngOutputPosition;
        }

        /// <summary>
        /// Search bytes in Bytes.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="bytesSearched"></param>
        /// <returns></returns>
        public static List<int> SearchBytesInBytes(byte[] buffer, byte[] bytesSearched)
        {
            var lstResults = new List<int>();
            var intCoincidencias = 0;
            for (var i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] != bytesSearched[intCoincidencias])
                    intCoincidencias = 0;
                if (buffer[i] == bytesSearched[intCoincidencias])
                    intCoincidencias++;
                if (intCoincidencias != bytesSearched.Length) continue;
                lstResults.Add(i - bytesSearched.Length + 1);
                intCoincidencias = 0;
            }
            return lstResults;
        }
    }
}