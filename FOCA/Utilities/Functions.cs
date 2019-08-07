using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace FOCA.Utilites
{
    public static class Functions
    {
        public static char[] MyInvalidPathChars = {'\\', '/', ':', '*', '?', '"', '<', '>', '|'};

        public static string GetFileSizeAsString(long size)
        {
            double s = size;
            string[] sizes = {"B", "KB", "MB", "GB", "TB"};
            int order;

            for (order = 0; order < sizes.Length - 1 && s >= 1024; order++)
                s /= 1024;

            return string.Format("{0:0.##} {1}", s, sizes[order]);
        }

        /// <summary>
        ///     Check if filename is valid. Only filename, not the whole path
        /// </summary>
        /// <param name="strFileName">Path</param>
        /// <returns></returns>
        public static bool IsValidFilename(string strFileName)
        {
            if (strFileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                return false;
            try
            {
                Path.GetDirectoryName(strFileName);
            }
            catch (PathTooLongException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        ///     Get a valid unused filename to avoid files overwriting
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public static string GetNotExistsPath(string strPath)
        {
            if (Path.GetFileName(strPath) == string.Empty)
                strPath = Path.GetDirectoryName(strPath) + "\\tempfile";
            if (!File.Exists(strPath) && IsValidFilename(strPath))
                return strPath;
            string strDirectory, strFilename;
            var strExt = Path.GetExtension(strPath);
            try
            {
                strDirectory = Path.GetDirectoryName(strPath);
                strFilename = Path.GetFileName(strPath);
            }
            catch (PathTooLongException)
            {
                strDirectory = strPath.Remove(strPath.LastIndexOf('\\'));
                var strRandomPath = Path.GetRandomFileName();
                strFilename = strRandomPath + strExt;
                strPath = strDirectory + "\\" + strFilename;
            }
            for (var i = 1; File.Exists(strPath); i++)
            {
                strPath = strDirectory + "\\" + Path.GetFileNameWithoutExtension(strFilename) + " (" + i + ")" + strExt;
            }
            return strPath;
        }

        /// <summary>
        ///     Check if the route corresponds to a server
        /// </summary>
        /// <param name="s"></param>
        /// <param name="server">name of the server</param>
        /// <returns>true if it's a server</returns>
        public static bool IsServerPath(string s, out string server)
        {
            server = string.Empty;
            if (s.Length > 3 && s[0] == '\\' && s[1] == '\\')
            {
                s = s.IndexOf('\\', 3) >= 3 ? s.Substring(2, s.IndexOf('\\', 3) - 2) : s.Substring(2);
                server = s.IndexOf('/') >= 0 ? s.Remove(s.IndexOf('/')) : s;
            }
            if (s.ToLower().StartsWith("https://"))
            {
                server = s.Substring(8);
                if (server.IndexOf('/') > 0)
                    server = server.Remove(server.IndexOf('/'));
                if (server.IndexOf('\\') > 0)
                    server = server.Remove(server.IndexOf('\\'));
                return server.Length > 0;
            }
            if (s.ToLower().StartsWith("http://"))
            {
                server = s.Substring(7);
                if (server.IndexOf('/') > 0)
                    server = server.Remove(server.IndexOf('/'));
                if (server.IndexOf('\\') > 0)
                    server = server.Remove(server.IndexOf('\\'));
                return server.Length > 0;
            }
            return server.Length > 0;
        }

        /// <summary>
        ///     Check if a given string is a valid IPv4 address and comes in the form www.xxx.yyy.zzz
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsIP(string s)
        {
            // avoid cases like '3', which is a valid IP address for TryParse()
            if (s.Split('.').Length != 4)
                return false;
            IPAddress address;
            if (IPAddress.TryParse(s, out address))
            {
                switch (address.AddressFamily)
                {
                    case AddressFamily.InterNetwork:
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Convert an IP address string into its unsigned decimal representation
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static ulong IPToInt(string ip)
        {
            return (ulong) BitConverter.ToInt32(IPAddress.Parse(ip).GetAddressBytes(), 0);
        }

        public static string SearchBetweenDelimiters(string strSource, string from, string until, ref int startPosition,
            StringComparison comparison)
        {
            var fromLength = (from ?? string.Empty).Length;
            var startIndex = !string.IsNullOrEmpty(from)
                ? strSource.IndexOf(from, comparison) + fromLength
                : 0;

            if (startIndex < fromLength)
            {
                return string.Empty;
            }

            var endIndex = !string.IsNullOrEmpty(until)
                ? strSource.IndexOf(until, startIndex, comparison)
                : strSource.Length;

            if (endIndex < 0)
            {
                return string.Empty;
            }

            startPosition = strSource.IndexOf(until, startIndex, comparison) + until.Length;
            return strSource.Substring(startIndex, endIndex - startIndex);
        }
    }
}