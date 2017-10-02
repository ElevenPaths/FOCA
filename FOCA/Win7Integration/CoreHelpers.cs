//Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Text;

namespace MS.WindowsAPICodePack.Internal
{
    /// <summary>
    /// Common Helper methods
    /// </summary>
    static public class CoreHelpers
    {
        /// <summary>
        /// Determines if the application is running on Windows 7
        /// </summary>
        public static bool RunningOnWin7
        {
            get
            {
                return (Environment.OSVersion.Version.Major > 6) ||
                    (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 1);
            }
        }

        /// <summary>
        /// Throws PlatformNotSupportedException if the application is not running on Windows 7
        /// </summary>
        public static void ThrowIfNotWin7()
        {
            if (!CoreHelpers.RunningOnWin7)
            {
                throw new PlatformNotSupportedException("Only supported on Windows 7 or newer.");
            }
        }
    }
}
