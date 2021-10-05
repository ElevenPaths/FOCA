using System;

namespace FOCA
{
    [Serializable]
    public class OperatingSystem
    {
        /// <summary>
        /// Enum with the OS that FOCA can discover
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
