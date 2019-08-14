using FOCA.Database.Entities;
using MetadataExtractCore.Diagrams;
using System;

namespace FOCA
{
    /// <summary>
    /// Clase que contiene toda la funcionalidad relaccionada con el tratamiento de Sistemas Operativos
    /// </summary>
    [Serializable]
    public class OperatingSystemUtils
    {
        /// <summary>
        /// Convierte un sistema operativo a cadena
        /// </summary>
        /// <param name="os"></param>
        /// <returns></returns>
        public static string OSToString(FOCA.OperatingSystem.OS os)
        {
            switch (os)
            {
                // Linux
                case FOCA.OperatingSystem.OS.Linux: return "Linux";
                case FOCA.OperatingSystem.OS.LinuxUbuntu: return "Linux (Ubuntu)";
                case FOCA.OperatingSystem.OS.LinuxDebian: return "Linux (Debian)";
                case FOCA.OperatingSystem.OS.LinuxRedHat: return "Linux (Red Hat)";
                case FOCA.OperatingSystem.OS.LinuxFedora: return "Linux (Fedora)";
                case FOCA.OperatingSystem.OS.LinuxMandrake: return "Linux (Mandrake)";
                case FOCA.OperatingSystem.OS.LinuxMandriva: return "Linux (Mandriva)";
                case FOCA.OperatingSystem.OS.LinuxSuse: return "Linux (Suse)";
                // BSD
                case FOCA.OperatingSystem.OS.FreeBSD: return "FreeBSD";
                case FOCA.OperatingSystem.OS.OpenBSD: return "OpenBSD";
                // *nix
                case FOCA.OperatingSystem.OS.CentOS: return "CentOS";
                case FOCA.OperatingSystem.OS.Solaris: return "Solaris";
                // Macos
                case FOCA.OperatingSystem.OS.MacOS: return "Mac OS";
                // Windows
                case FOCA.OperatingSystem.OS.Windows: return "Windows";
                case FOCA.OperatingSystem.OS.Windows7: return "Windows 7";
                case FOCA.OperatingSystem.OS.Windows2008: return "Windows 2008";
                case FOCA.OperatingSystem.OS.WindowsVista: return "Windows Vista";
                case FOCA.OperatingSystem.OS.Windows2003: return "Windows 2003";
                case FOCA.OperatingSystem.OS.WindowsXP: return "Windows XP";
                case FOCA.OperatingSystem.OS.Windows98: return "Windows 98";
                case FOCA.OperatingSystem.OS.Windows2000: return "Windows 2000";
                case FOCA.OperatingSystem.OS.WindowsNT351: return "Windows NT 3.51";
                case FOCA.OperatingSystem.OS.WindowsNT40: return "Windows NT 4.0";
                // Others
                case FOCA.OperatingSystem.OS.Unknown: return "Unknown";
                default: return "Unknown";
            }
        }

        /// <summary>
        /// Dada una cadena que contiene el sistema operativo, intentar descubrir a cual de ellos se refiere
        /// </summary>
        /// <param name="strOS">Cadena que contiene el sistema operativo en alguna parte</param>
        /// <returns>Sistema operativo encontrado</returns>
        public static FOCA.OperatingSystem.OS StringToOS(string strOS)
        {
            string strOSlo = strOS.ToLower();
            if (strOSlo.Contains("centos"))
                return FOCA.OperatingSystem.OS.CentOS;
            else if (strOSlo.Contains("freebsd"))
                return FOCA.OperatingSystem.OS.FreeBSD;
            else if (strOSlo.Contains("linux"))
                return FOCA.OperatingSystem.OS.Linux;
            else if (strOSlo.Contains("unix"))
                return FOCA.OperatingSystem.OS.Linux;
            else if (strOSlo.Contains("windows") || strOSlo.Contains("win"))
            {
                if (strOSlo.Contains("nt 3.51"))
                    return FOCA.OperatingSystem.OS.WindowsNT351;
                else if (strOSlo.Contains("nt 4.0"))
                    return FOCA.OperatingSystem.OS.WindowsNT40;
                else if (strOSlo.Contains("98"))
                    return FOCA.OperatingSystem.OS.Windows98;
                else if (strOSlo.Contains("2000"))
                    return FOCA.OperatingSystem.OS.Windows2000;
                else if (strOSlo.Contains("2003"))
                    return FOCA.OperatingSystem.OS.Windows2003;
                else if (strOSlo.Contains("vista"))
                    return FOCA.OperatingSystem.OS.WindowsVista;
                else if (strOSlo.Contains("xp"))
                    return FOCA.OperatingSystem.OS.WindowsXP;
                else if (strOSlo.Contains("windows 7"))
                    return FOCA.OperatingSystem.OS.Windows7;
                else
                    return FOCA.OperatingSystem.OS.Windows;
            }
            else if (strOSlo.Contains("mac"))
            {
                return FOCA.OperatingSystem.OS.MacOS;
            }
            return FOCA.OperatingSystem.OS.Unknown;
        }

        /// <summary>
        /// Dada una lista de software de un equipo intenta determinar el sistema operativo de dicho equipo
        /// </summary>
        /// <param name="lstSoftware">Lista de cadenas que contienen el software</param>
        public static FOCA.OperatingSystem.OS SoftwareToOS(Applications aplicaciones)
        {
            //Quizas se debería puntuar cada SO por cada coincidencia y devolver el mas probable

            //Se almacena un SO poco probable, pero si no se encuentra nada mejor se devuelve este
            FOCA.OperatingSystem.OS posibleOS = FOCA.OperatingSystem.OS.Unknown;
            foreach (ApplicationsItem aplicacion in aplicaciones.Items)
            {
                string strSoftware = aplicacion.Name;
                string strSoftwarelo = strSoftware.ToLower();
                if (strSoftwarelo.Contains("mac") || strSoftwarelo.Contains("macintosh") || strSoftwarelo.Contains("neooffice") || strSoftwarelo.Contains("quartz"))
                {
                    return FOCA.OperatingSystem.OS.MacOS;
                }
                else if (strSoftwarelo.Contains("office") && (strSoftwarelo.Contains("2007") || strSoftwarelo.Contains("12")))  //Si se detecta la version 2007 suponemos que se usa en vista
                {
                    return FOCA.OperatingSystem.OS.WindowsVista;
                }
                else if (strSoftwarelo.Contains("unix") || strSoftwarelo.Contains("linux"))
                {
                    return FOCA.OperatingSystem.OS.Linux;
                }
                //Puede ser Tanto windows como Mac, pero es mas probable que sea windows.
                else if (strSoftwarelo.Contains("microsoft") && strSoftwarelo.Contains("office"))
                {
                    posibleOS = FOCA.OperatingSystem.OS.Windows;
                }
                else if (strSoftwarelo.Contains("windows") || strSoftwarelo.Contains("win"))
                {
                    return FOCA.OperatingSystem.OS.Windows;
                }
            }
            if (posibleOS != FOCA.OperatingSystem.OS.Unknown)
                return posibleOS;
            else
                return FOCA.OperatingSystem.OS.Unknown;
        }

        /// <summary>
        /// Dado un sistema operativo devuelve el número de icono asignado a él
        /// </summary>
        /// <param name="os">Sistema operativo del que se desea saber el número de icono</param>
        /// <returns>Número de icono, depende del imagelist</returns>
        public static byte OSToIconNumber(FOCA.OperatingSystem.OS os)
        {
            switch (os)
            {
                case FOCA.OperatingSystem.OS.LinuxUbuntu: return 57;
                case FOCA.OperatingSystem.OS.LinuxRedHat: return 52;
                case FOCA.OperatingSystem.OS.LinuxFedora: return 53;
                case FOCA.OperatingSystem.OS.LinuxDebian: return 58;
                case FOCA.OperatingSystem.OS.LinuxSuse: return 56;
                case FOCA.OperatingSystem.OS.LinuxMandrake: return 54;
                case FOCA.OperatingSystem.OS.LinuxMandriva: return 55;
                case FOCA.OperatingSystem.OS.Linux: return 16;
                case FOCA.OperatingSystem.OS.CentOS: return 48;
                case FOCA.OperatingSystem.OS.FreeBSD: return 47;
                case FOCA.OperatingSystem.OS.OpenBSD: return 50;
                case FOCA.OperatingSystem.OS.Solaris: return 49;
                case FOCA.OperatingSystem.OS.MacOS: return 28;
                case FOCA.OperatingSystem.OS.Windows7: return 36;
                case FOCA.OperatingSystem.OS.Windows2008: return 39;
                case FOCA.OperatingSystem.OS.WindowsVista: return 35;
                case FOCA.OperatingSystem.OS.Windows2003: return 38;
                case FOCA.OperatingSystem.OS.Windows: return 20;
                case FOCA.OperatingSystem.OS.WindowsXP: return 40;
                case FOCA.OperatingSystem.OS.Windows98:
                case FOCA.OperatingSystem.OS.Windows2000: return 37;
                case FOCA.OperatingSystem.OS.WindowsNT351:
                case FOCA.OperatingSystem.OS.WindowsNT40: return 41;
                case FOCA.OperatingSystem.OS.Unknown: return 12;
                default: return 12;
            }
        }
    }
}
