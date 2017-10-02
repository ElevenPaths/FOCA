using System;

namespace FOCA
{
    [Serializable]
    public class OperatingSystem
    {
        /// <summary>
        /// Enumeración de los SistemaOperativos descubiertos por FOCA
        /// </summary>
        public enum OS
        {
            Windows7, Windows2008, WindowsVista, Windows2003, WindowsXP, Windows2000, WindowsNT40, Windows98, WindowsNT351, Windows,
            Linux, LinuxDebian, LinuxRedHat, LinuxFedora, LinuxUbuntu, LinuxMandrake, LinuxMandriva, LinuxSuse,
            FreeBSD, OpenBSD,
            CentOS, Solaris,
            MacOS,
            Unknown
        };
    }
}
