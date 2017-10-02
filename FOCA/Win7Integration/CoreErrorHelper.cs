//Copyright (c) Microsoft Corporation.  All rights reserved.

namespace MS.WindowsAPICodePack.Internal
{
    /// <summary>
    /// HRESULT Wrapper
    /// This is intended for Library Internal use only.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "HRESULT"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1028:EnumStorageShouldBeInt32",
        Justification="The base type for all of these value is uint")]
    public enum HRESULT : uint
    {
        /// <summary>
        /// S_FALSE
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "FALSE")]
        S_FALSE = 0x0001,
        
        /// <summary>
        /// S_OK
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OK")]
        S_OK = 0x0000,
        
        /// <summary>
        /// E_INVALIDARG
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "INVALIDARG")]
        E_INVALIDARG = 0x80070057,
        
        /// <summary>
        /// E_OUTOFMEMORY
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "OUTOFMEMORY")]
        E_OUTOFMEMORY = 0x8007000E,

        /// <summary>
        /// E_NOINTERFACE
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "NOINTERFACE")]
        E_NOINTERFACE = 0x80004002,

        /// <summary>
        /// E_FAIL
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "FAIL")]
        E_FAIL = 0x80004005,

        /// <summary>
        /// E_ELEMENTNOTFOUND
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ELEMENTNOTFOUND")]
        E_ELEMENTNOTFOUND = 0x80070490,

        /// <summary>
        /// TYPE_E_ELEMENTNOTFOUND
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "TYPE")]
        TYPE_E_ELEMENTNOTFOUND = 0x8002802B,


        /// <summary>
        /// NO_OBJECT
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "NO_OBJECT")]
        NO_OBJECT = 0x800401E5,

        /// <summary>
        /// Win32 Error code: ERROR_CANCELLED
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ERROR")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "CANCELLED")]
        ERROR_CANCELLED = 1223,

        /// <summary>
        /// ERROR_CANCELLED
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "ERROR" )]
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "CANCELLED" )]
        E_ERROR_CANCELLED = 0x800704C7,

        /// <summary>
        /// The requested resource is in use
        /// </summary>
        RESOURCE_IN_USE = 0x800700AA,
    }

    /// <summary>
    /// Provide Error Message Helper Methods.
    /// This is intended for Library Internal use only.
    /// </summary>
    public static class CoreErrorHelper
    {
        /// <summary>
        /// This is intended for Library Internal use only.
        /// </summary>
        private const int FACILITY_WIN32 = 7;

        /// <summary>
        /// This is intended for Library Internal use only.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "IGNORED")]
        public const int IGNORED = (int)HRESULT.S_OK;

        /// <summary>
        /// This is intended for Library Internal use only.
        /// </summary>
        /// <param name="win32ErrorCode">The Windows API error code.</param>
        /// <returns>The equivalent HRESULT.</returns>
        public static int HResultFromWin32(int win32ErrorCode)
        {
            if (win32ErrorCode > 0)
            {
                win32ErrorCode =
                    (int)(((uint)win32ErrorCode & 0x0000FFFF) | (FACILITY_WIN32 << 16) | 0x80000000);
            }
            return win32ErrorCode;

        }

        /// <summary>
        /// This is intended for Library Internal use only.
        /// </summary>
        /// <param name="hresult">The error code.</param>
        /// <returns>True if the error code indicates success.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "hresult")]
        public static bool Succeeded(int hresult)
        {
            return (hresult >= 0);
        }

        /// <summary>
        /// This is intended for Library Internal use only.
        /// </summary>
        /// <param name="hResult">The error code.</param>
        /// <returns>True if the error code indicates failure.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "h")]
        public static bool Failed(HRESULT hResult)
        {
            return ((int)hResult < 0);
        }

        /// <summary>
        /// This is intended for Library Internal use only.
        /// </summary>
        /// <param name="hresult">The COM error code.</param>
        /// <param name="win32ErrorCode">The Win32 error code.</param>
        /// <returns>Inticates that the Win32 error code corresponds to the COM error code.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "hresult")]
        public static bool Matches(int hresult, int win32ErrorCode)
        {
            return (hresult == HResultFromWin32(win32ErrorCode));
        }


    }
}
