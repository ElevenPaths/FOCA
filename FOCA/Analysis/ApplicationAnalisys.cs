using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FOCA.Analysis
{
    class ApplicationAnalysis
    {
        public static string GetApplicationsFromString(string strApplication)
        {
            return GetApplicationsFromString(strApplication, false);
        }

        /// <summary>
        /// A partir de una cadena, proveniente del campo application del documento,
        /// obtener el software, correctamente formateado, usado para crear el documento.    
        /// "powerpoint 12.000" -> "Microsoft Office 2007"
        /// </summary>
        /// <param name="strApplication">String with the "raw" application</param>
        /// <returns>All software used list</returns>
        public static string GetApplicationsFromString(string strApplication, bool IsMac)
        {
            if (strApplication != null)
            {
                if (strApplication.ToLower().Contains("distiller"))
                {
                    string strVersion = ExtractVersion(strApplication);
                    return "Acrobat Distillier" + (strVersion != String.Empty ? " " + strVersion : String.Empty);
                }
                else if (strApplication.ToLower().Contains("acrobat") && strApplication.ToLower().Contains("paper") && strApplication.ToLower().Contains("capture"))
                {
                    string strVersion = ExtractVersion(strApplication);
                    return "Adobe Acrobat" + (strVersion != String.Empty ? " " + strVersion : String.Empty) + " Paper Capture Plug-in";
                }
                else if (strApplication.ToLower().Contains("adobe") && strApplication.ToLower().Contains("acrobat"))
                {
                    string strVersion = ExtractVersion(strApplication);
                    return "Adobe Acrobat" + (strVersion != String.Empty ? " " + strVersion : String.Empty);
                }
                else if (strApplication.ToLower().Contains("framemaker"))
                {
                    string strVersion = ExtractVersion(strApplication);
                    return "Adobe FrameMaker" + (strVersion != String.Empty ? " " + strVersion : String.Empty);
                }
                else if (strApplication.ToLower().Contains("quarkxpress"))
                {
                    string strVersion = ExtractVersion(strApplication);
                    if (strVersion == "1" || strVersion == "2" || strVersion == "3" || strVersion == "8.15")
                        strVersion += " Mac";
                    return "QuarkXPress" + (strVersion != String.Empty ? " " + strVersion : String.Empty);
                }
                else if (strApplication.ToLower().Contains("openoffice") || strApplication.ToLower().Contains("broffice") || strApplication.ToLower().Contains("neooffice") || strApplication.ToLower().Contains("staroffice"))
                {
                    string strRealApplication = String.Empty;
                    if (strApplication.ToLower().Contains("broffice"))
                        strRealApplication = "BrOffice";
                    else if (strApplication.ToLower().Contains("neooffice"))
                        strRealApplication = "NeoOffice";
                    else if (strApplication.ToLower().Contains("staroffice"))
                        strRealApplication = "StarOffice";
                    else if (strApplication.ToLower().Contains("openoffice"))
                        strRealApplication = "OpenOffice";
                    string strVersion = ExtractVersion(strApplication);
                    string strSoftware = strRealApplication + (strVersion != String.Empty && strVersion != "680" && strVersion != "300"? " " + strVersion : String.Empty);

                    //Si encuentra alguna mX lo añade
                    if (strApplication.ToLower().Contains("680m"))
                    {
                        strSoftware += " " + strApplication.Substring(strApplication.IndexOf("680m") + 3, 2);
                    }
                    if (strApplication.ToLower().Contains("300m"))
                    {
                        strSoftware += " " + strApplication.Substring(strApplication.IndexOf("300m") + 3, 2);
                    }
                    //Si encuentra un Build lo añade
                    if (strApplication.ToLower().Contains("build-"))
                    {
                        int intBuildStart = strApplication.ToLower().IndexOf("build-") + 6;
                        if (intBuildStart + 4 <= strApplication.Length)
                            strSoftware += " Build " + strApplication.Substring(strApplication.ToLower().IndexOf("build-") + 6, 4);
                    }
                    return strSoftware;
                }
                else if (strApplication.ToLower().Contains("mac os x") &&
                         strApplication.ToLower().Contains("quartz") &&
                         strApplication.ToLower().Contains("pdfcontext"))
                {
                    string strVersion = ExtractVersion(strApplication);
                    return "Mac OS X" + (strVersion != String.Empty ? " " + strVersion : String.Empty) + " Quartz PDFContext";
                }
                else if (strApplication.ToLower().Contains("quartz"))
                {
                    return "Quartz";
                }
                else if (strApplication.ToLower().Contains("coreldraw"))
                {
                    if (strApplication.ToLower().Contains("12"))
                    {
                        return "CorelDRAW 12.0";
                    }
                    else
                    {
                        return "CorelDRAW";
                    }
                }
                else if (strApplication.ToLower().Contains("ghostscript"))
                {
                    if (strApplication.ToLower().Contains("gpl"))
                    {
                        string strVersion = ExtractVersion(strApplication);
                        return "GPL Ghostscript" + (strVersion != String.Empty ? " " + strVersion : String.Empty);
                    }
                    else if (strApplication.ToLower().Contains("gnu"))
                    {
                        string strVersion = ExtractVersion(strApplication);
                        return "GNU Ghostscript" + (strVersion != String.Empty ? " " + strVersion : String.Empty);
                    }
                    else
                    {
                        string strVersion = ExtractVersion(strApplication);
                        return "Ghostscript" + (strVersion != String.Empty ? " " + strVersion : String.Empty);
                    }
                }
                else if (strApplication.ToLower().Contains("corel") && strApplication.ToLower().Contains("pdf") && strApplication.ToLower().Contains("engine"))
                {
                    string strVersion = ExtractVersion(strApplication);
                    return "Corel PDF Engine" + (strVersion != String.Empty ? " " + strVersion : String.Empty);
                }
                else if (strApplication.ToLower().Contains("acrobat") && strApplication.ToLower().Contains("pdfwriter"))
                {
                    string strVersion = ExtractVersion(strApplication);
                    return "Acrobat PDFWriter" + (strVersion != String.Empty ? " " + strVersion : String.Empty);
                }
                else if (strApplication.ToLower().Contains("adobe") && strApplication.ToLower().Contains("pagemaker"))
                {
                    string strVersion = ExtractVersion(strApplication);
                    return "Adobe PageMaker" + (strVersion != String.Empty ? " " + strVersion : String.Empty);
                }
                else if (strApplication.ToLower().Contains("pdflib"))
                {
                    string strVersion = ExtractVersion(strApplication);
                    return "PDFlib" + (strVersion != String.Empty ? " " + strVersion : String.Empty);
                }
                else if (strApplication.ToLower().Contains("pdfcreator"))
                {
                    string strVersion = ExtractVersion(strApplication);
                    if (strVersion.Contains("5") || strVersion.Contains("6") || strVersion.Contains("7"))
                        return "PDFCreator" + (strVersion != String.Empty ? " " + strVersion : String.Empty) + " Windows XP";
                    return "PDFCreator" + (strVersion != String.Empty ? " " + strVersion : String.Empty) + "Windows";
                }
                else if (strApplication.ToLower().Contains("adobe") && strApplication.ToLower().Contains("pdf") && strApplication.ToLower().Contains("library"))
                {
                    string strVersion = ExtractVersion(strApplication);
                    return "Adobe PDF Library" + (strVersion != String.Empty ? " " + strVersion : String.Empty);
                }
                else if (strApplication.ToLower().Contains("photoshop"))
                {
                    string strVersion = ExtractVersion(strApplication);
                    if (strApplication.ToLower().Contains("cs"))
                        strVersion = "CS" + strVersion;
                    return "Adobe Photoshop" + (strVersion != String.Empty ? " " + strVersion : String.Empty);
                }
                else if (strApplication.ToLower().Contains("amyuni pdf converter"))
                {
                    string strVersion = ExtractVersion(strApplication);
                    return "Amyuni PDF Converter" + (strVersion != String.Empty ? " " + strVersion : String.Empty);
                }
                else if (strApplication.ToLower().Contains("apple keynote"))
                {
                    string strVersion = ExtractVersion(strApplication);
                    return "Apple Keynote" + (strVersion != String.Empty ? " " + strVersion : String.Empty);
                }
                else if (strApplication.ToLower().Contains("powerpoint") ||
                    strApplication.ToLower().Contains("excel") ||
                    strApplication.ToLower().Contains("word") ||
                    (strApplication.ToLower().Contains("microsoft") && strApplication.ToLower().Contains("office")))
                {
                    //La información de la versión va siempre antes del titulo, delimitado con un "-"
                    string strVersion = strApplication;
                    int intVersionStart = strApplication.IndexOf('-');
                    string strExactVersion = intVersionStart > 0 ? ExtractVersion(strApplication.Remove(intVersionStart)) : ExtractVersion(strApplication);
                    //Detectamos si es Microsoft Macintosh
                    //La versión 12.0000 y 12.0 es la de windows
                    if (strVersion.Contains("12.") && !strVersion.Contains("12.0000") && strExactVersion != "12.0")
                    {
                        return "Microsoft Office 2008 for Mac";
                    }
                    //La version 11.0 es de Windows
                    else if (strVersion.Contains("11.") && strExactVersion != "11.0")
                    {
                        return "Microsoft Office 2004 for Mac";
                    }
                    else if (IsMac || strVersion.ToLower().Contains("macintosh"))
                    {
                        if (strVersion.Contains("12."))
                            return "Microsoft Office 2008 for Mac";
                        else if (strVersion.Contains("11."))
                            return "Microsoft Office 2004 for Mac";
                        else if (strVersion.Contains("10."))
                            return "Microsoft Office X for Mac";
                        else if (strVersion.Contains("9."))
                            return "Microsoft Office 2001 for Mac";
                        else if (strVersion.Contains("8."))
                            return "Microsoft Office 98 for Mac";
                        else
                            return "Microsoft Office for Mac";
                    }

                    else if (strVersion.ToLower().Contains("12") || strVersion.ToLower().Contains("2007"))
                    {
                        return "Microsoft Office 2007";
                    }
                    else if (strVersion.ToLower().Contains("11") || strVersion.ToLower().Contains("2003"))
                    {
                        return "Microsoft Office 2003";
                    }
                    else if (strVersion.ToLower().Contains("10") || strVersion.ToLower().Contains("xp"))
                    {
                        return "Microsoft Office XP";
                    }
                    else if (strVersion.ToLower().Contains("9") || strVersion.ToLower().Contains("2000"))
                    {
                        return "Microsoft Office 2000";
                    }
                    else if (strVersion.ToLower().Contains("8") || strVersion.ToLower().Contains("97"))
                    {
                        return "Microsoft Office 97";
                    }
                    else if (strVersion.ToLower().Contains("7") || strVersion.ToLower().Contains("95"))
                    {
                        return "Microsoft Office 95";
                    }
                    else
                    {
                        return "Microsoft Office";
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Dada una cadena busca en ella la posible versión
        /// "distiller 4.60 (windows)" => "4.60"
        /// </summary>
        /// <param name="strApplication">La cadena de la aplicación</param>
        /// <returns>La versión obtenida</returns>
        private static string ExtractVersion(string strApplication)
        {
            char[] achrVersionChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.' };
            //Primer número, fijarse que no tiene el '.'
            int intVersionStart = strApplication.IndexOfAny(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'});
            if (intVersionStart > 0)
            {
                string strVersion = string.Empty;
                do
                {
                    strVersion += strApplication[intVersionStart++];
                }
                while (intVersionStart < strApplication.Length && achrVersionChars.Contains<char>(strApplication[intVersionStart]));
                return strVersion;
            }
            else
                return string.Empty;
        }
    }
}
