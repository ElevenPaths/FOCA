using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace MetadataExtractCore.Analysis
{
    public static class ApplicationAnalysis
    {
        private static readonly Regex versionRegex = new Regex("(\\d+\\.){0,6}(\\d+)", RegexOptions.Compiled);

        /// <summary>
        /// Get Application from string.
        /// </summary>
        /// <param name="strApplication"></param>
        /// <returns></returns>
        public static string GetApplicationsFromString(string strApplication)
        {
            return GetApplicationsFromString(strApplication, false);
        }

        /// <summary>
        /// Get application from string.
        /// </summary>
        /// <param name="applicationValue">String with the "raw" application</param>
        /// <returns>All software used list</returns>
        public static string GetApplicationsFromString(string applicationValue, bool IsMac)
        {
            if (applicationValue == null) return string.Empty;

            var strApplication = applicationValue.ToLower();

            var strVersion = ExtractVersion(strApplication);

            if (strApplication.Contains("iexplore"))
                return "Internet Explorer";

            if (strApplication.Contains("distiller"))
                return "Acrobat Distiller" + (strVersion != string.Empty ? " " + strVersion : string.Empty);

            if (strApplication.Contains("acrobat") && strApplication.Contains("paper") && strApplication.Contains("capture"))
                return "Adobe Acrobat" + (strVersion != string.Empty ? " " + strVersion : string.Empty) + " Paper Capture Plug-in";
            
            if (strApplication.Contains("adobe") && strApplication.Contains("acrobat"))
                return "Adobe Acrobat" + (strVersion != string.Empty ? " " + strVersion : string.Empty);

            if (strApplication.Contains("framemaker"))
                return "Adobe FrameMaker" + (strVersion != string.Empty ? " " + strVersion : string.Empty);

            if (strApplication.Contains("quarkxpress"))
            {
                if (strVersion == "1" || strVersion == "2" || strVersion == "3" || strVersion == "8.15")
                    strVersion += " Mac";
                return "QuarkXPress" + (strVersion != string.Empty ? " " + strVersion : string.Empty);
            }
            if (strApplication.Contains("openoffice") || strApplication.Contains("broffice") || strApplication.Contains("neooffice") || strApplication.Contains("staroffice"))
            {
                var strRealApplication = string.Empty;
                if (strApplication.Contains("broffice"))
                    strRealApplication = "BrOffice";
                else if (strApplication.Contains("neooffice"))
                    strRealApplication = "NeoOffice";
                else if (strApplication.Contains("staroffice"))
                    strRealApplication = "StarOffice";
                else if (strApplication.Contains("openoffice"))
                    strRealApplication = "OpenOffice";
                var strSoftware = strRealApplication + (strVersion != string.Empty && strVersion != "680" && strVersion != "300"? " " + strVersion : string.Empty);
                
                if (strApplication.Contains("680m"))
                    strSoftware += " " + strApplication.Substring(strApplication.IndexOf("680m") + 3, 2);

                if (strApplication.Contains("300m"))
                    strSoftware += " " + strApplication.Substring(strApplication.IndexOf("300m") + 3, 2);

                if (!strApplication.Contains("build-")) return strSoftware;

                var intBuildStart = strApplication.IndexOf("build-") + 6;
                if (intBuildStart + 4 <= strApplication.Length)
                    strSoftware += " Build " + strApplication.Substring(strApplication.IndexOf("build-") + 6, 4);

                return strSoftware;
            }
            if (strApplication.Contains("mac os x") &&
                     strApplication.Contains("quartz") &&
                     strApplication.Contains("pdfcontext"))
                return "Mac OS X" + (strVersion != string.Empty ? " " + strVersion : string.Empty) + " Quartz PDFContext";

            if (strApplication.Contains("quartz"))
                return "Quartz";

            if (strApplication.Contains("coreldraw"))
                return strApplication.Contains("12") ? "CorelDRAW 12.0" : "CorelDRAW";

            if (strApplication.Contains("ghostscript"))
            {
                if (strApplication.Contains("gpl"))
                    return "GPL Ghostscript" + (strVersion != string.Empty ? " " + strVersion : string.Empty);

                if (strApplication.Contains("gnu"))
                    return "GNU Ghostscript" + (strVersion != string.Empty ? " " + strVersion : string.Empty);

                return "Ghostscript" + (strVersion != string.Empty ? " " + strVersion : string.Empty);
            }
            if (strApplication.Contains("corel") && strApplication.Contains("pdf") && strApplication.Contains("engine"))
                return "Corel PDF Engine" + (strVersion != string.Empty ? " " + strVersion : string.Empty);

            if (strApplication.Contains("acrobat") && strApplication.Contains("pdfwriter"))
                return "Acrobat PDFWriter" + (strVersion != string.Empty ? " " + strVersion : string.Empty);

            if (strApplication.Contains("adobe") && strApplication.Contains("pagemaker"))
                return "Adobe PageMaker" + (strVersion != string.Empty ? " " + strVersion : string.Empty);

            if (strApplication.Contains("pdflib"))
                return "PDFlib" + (strVersion != string.Empty ? " " + strVersion : string.Empty);

            if (strApplication.Contains("pdfcreator"))
            {
                if (strVersion.Contains("5") || strVersion.Contains("6") || strVersion.Contains("7"))
                    return "PDFCreator" + (strVersion != string.Empty ? " " + strVersion : string.Empty) + " Windows XP";
                return "PDFCreator" + (strVersion != string.Empty ? " " + strVersion : string.Empty) + "Windows";
            }
            if (strApplication.Contains("adobe") && strApplication.Contains("pdf") && strApplication.Contains("library"))
                return "Adobe PDF Library" + (strVersion != string.Empty ? " " + strVersion : string.Empty);
            if (strApplication.Contains("photoshop"))
            {
                if (strApplication.Contains("cs"))
                    strVersion = "CS" + strVersion;
                return "Adobe Photoshop" + (strVersion != string.Empty ? " " + strVersion : string.Empty);
            }
            if (strApplication.Contains("amyuni pdf converter"))
                return "Amyuni PDF Converter" + (strVersion != string.Empty ? " " + strVersion : string.Empty);

            if (strApplication.Contains("apple keynote"))
                return "Apple Keynote" + (strVersion != string.Empty ? " " + strVersion : string.Empty);

            if (!strApplication.Contains("powerpoint") && !strApplication.Contains("excel") &&
                !strApplication.Contains("word") &&
                (!strApplication.Contains("microsoft") || !strApplication.Contains("office")))
                return string.Empty;

            var intVersionStart = strApplication.IndexOf('-');
            var strExactVersion = intVersionStart > 0 ? ExtractVersion(strApplication.Remove(intVersionStart)) : ExtractVersion(strApplication);
                
            if (strVersion.Contains("12.") && !strVersion.Contains("12.0000") && strExactVersion != "12.0")
                return "Microsoft Office 2008 for Mac";
                
            if (strVersion.Contains("11.") && strExactVersion != "11.0")
                return "Microsoft Office 2004 for Mac";

            if (IsMac || strVersion.ToLower().Contains("macintosh"))
            {
                if (strVersion.Contains("12."))
                    return "Microsoft Office 2008 for Mac";
                if (strVersion.Contains("11."))
                    return "Microsoft Office 2004 for Mac";
                if (strVersion.Contains("10."))
                    return "Microsoft Office X for Mac";
                if (strVersion.Contains("9."))
                    return "Microsoft Office 2001 for Mac";
                return strVersion.Contains("8.") ? "Microsoft Office 98 for Mac" : "Microsoft Office for Mac";
            }

            if (strVersion.ToLower().Contains("12") || strVersion.ToLower().Contains("2007"))
                return "Microsoft Office 2007";
                
            if (strVersion.ToLower().Contains("11") || strVersion.ToLower().Contains("2003"))
                return "Microsoft Office 2003";
                
            if (strVersion.ToLower().Contains("10") || strVersion.ToLower().Contains("xp"))
                return "Microsoft Office XP";
                
            if (strVersion.ToLower().Contains("9") || strVersion.ToLower().Contains("2000"))
                return "Microsoft Office 2000";
                
            if (strVersion.ToLower().Contains("8") || strVersion.ToLower().Contains("97"))
                return "Microsoft Office 97";
                
            if (strVersion.ToLower().Contains("7") || strVersion.ToLower().Contains("95"))
                return "Microsoft Office 95";

            return "Microsoft Office";
        }

        /// <summary>
        /// Extract version from string
        /// </summary>
        /// <param name="strApplication">app value</param>
        /// <returns>version</returns>
        private static string ExtractVersion(string strApplication)
        {
            Match match = versionRegex.Match(strApplication);

            return match.Success ? match.Value : String.Empty;
        }
    }
}
