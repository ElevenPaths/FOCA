using MetadataExtractCore.Utilities;
using System;

namespace MetadataExtractCore.Analysis
{
    public static class PathAnalysis
    {
        /// <summary>
        /// Get user by path.
        /// </summary>
        /// <param name="pathValue">path</param>
        /// <returns>string</returns>
        public static string ExtractUserFromPath(string pathValue)
        {
            string resultUser;

            if (GetUserFromPathValue(pathValue, @":\winnt\profiles\", out resultUser)) return resultUser;

            if (GetUserFromPathValue(pathValue, @":\documents and settings\", out resultUser)) return resultUser;

            if (GetUserFromPathValue(pathValue, @":\dokumente und einstellungen\", out resultUser)) return resultUser;

            if (GetUserFromPathValue(pathValue, @":\docume~1\", out resultUser)) return resultUser;

            if (GetUserFromPathValue(pathValue, @":\dokume~1\", out resultUser)) return resultUser;

            if (GetUserFromPathValue(pathValue, @":\users\", out resultUser)) return resultUser;

            if (GetUserFromPathValue(pathValue, "/home/", out resultUser)) return resultUser;

            return GetUserFromPathValue(pathValue, "/users/", out resultUser) ? resultUser : string.Empty;
        }

        /// <summary>
        /// Return true or false for user by path.
        /// </summary>
        /// <param name="pathValue">Path</param>
        /// <param name="pathValidate">Path to validate</param>
        /// <param name="resultValue">return result.</param>
        /// <returns></returns>
        private static bool GetUserFromPathValue(string pathValue, string pathValidate, out string resultValue)
        {
            resultValue = string.Empty;
            var intUserStart = pathValue.ToLower().IndexOf(pathValidate);

            if (intUserStart != 1) return false;

            var strUser = pathValue.Substring(intUserStart + pathValidate.Length);
            var intUserEnd = strUser.IndexOf('\\');
            if (intUserEnd > 0)
                strUser = strUser.Remove(intUserEnd);
            {
                resultValue = strUser;
                return true;
            }
        }

        /// <summary>
        /// Validate Path
        /// </summary>
        /// <param name="pathValue">Path to validate.</param>
        /// <returns>True or False.</returns>
        public static bool IsValidPath(string pathValue)
        {
            pathValue = pathValue.ToLower();
            if (!Functions.StringContainAnyLetter(pathValue))
                return false;

            if (pathValue.Length > 2 && char.IsLetter(pathValue[0]) && pathValue[1] == ':' && pathValue[2] == '\\')
                return true;

            if (pathValue.StartsWith("\\\\") &&
                pathValue.Length > 2 && pathValue[2] != '\\')
                return true;

            if (pathValue.StartsWith("\\") &&
                pathValue.Length > 1 && pathValue[1] != '\\')
                return true;

            if (pathValue.StartsWith("/"))
                return true;

            if (pathValue.StartsWith("http://") && pathValue.Length > "http://".Length)
                return true;

            if (pathValue.StartsWith("https://") && pathValue.Length > "https://".Length)
                return true;

            if (pathValue.StartsWith("ftp://") && pathValue.Length > "ftp://".Length)
                return true;

            if (pathValue.StartsWith("ldap://") && pathValue.Length > "ldap://".Length)
                return true;

            return pathValue.StartsWith("telnet://") && pathValue.Length > "telnet://".Length;
        }

        /// <summary>
        /// Remove name of path for unix y windows
        /// </summary>
        /// <param name="pathValue"></param>
        /// <returns></returns>
        public static string RemoveNamePath(string pathValue)
        {
            if (pathValue.LastIndexOf('\\') > pathValue.LastIndexOf('/') && pathValue.LastIndexOf('\\') < pathValue.Length - 1)
                return pathValue.Remove(pathValue.LastIndexOf('\\') + 1);
            if (pathValue.LastIndexOf('/') > pathValue.LastIndexOf('\\') && pathValue.LastIndexOf('/') < pathValue.Length - 1)
                return pathValue.Remove(pathValue.LastIndexOf('/') + 1);

            return pathValue;
        }

        /// <summary>
        /// Clean Path
        /// </summary>
        /// <param name="pathValue"></param>
        /// <returns></returns>
        public static string CleanPath(string pathValue)
        {
            if (Uri.TryCreate(pathValue, UriKind.Absolute, out Uri uriValue))
            {
                pathValue = uriValue.AbsoluteUri;
                pathValue = RemoveNamePath(pathValue);
                if (pathValue.IndexOf("file://") != -1)
                    pathValue = pathValue.Substring(pathValue.IndexOf("file://") + 7, pathValue.Length - pathValue.IndexOf("file://") - 7);
                if (pathValue.IndexOf(':') != 2)
                    return IsValidPath(pathValue) ? pathValue.Trim() : String.Empty;

                pathValue = pathValue.Replace('/', '\\');
                pathValue = pathValue.Substring(1, pathValue.Length - 1);

                return IsValidPath(pathValue) ? pathValue.Trim() : String.Empty;
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
