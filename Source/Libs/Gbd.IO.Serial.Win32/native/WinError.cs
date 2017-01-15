using Gbd.IO.Serial.Error;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Gbd.IO.Serial.Win32.native {
    internal class WinError {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int FormatMessage(int dwFlags, IntPtr lpSource_mustBeNull, uint dwMessageId,
            int dwLanguageId, StringBuilder lpBuffer, int nSize, IntPtr[] arguments);

        public const int ERROR_FILE_NOT_FOUND = 2;
        public const int ERROR_PATH_NOT_FOUND = 3;
        public const int ERROR_ACCESS_DENIED = 5;
        public const int ERROR_INVALID_HANDLE = 6;
        public const int ERROR_BAD_COMMAND = 22;
        public const int ERROR_FILENAME_EXCED_RANGE = 206;
        public const int ERROR_SHARING_VIOLATION = 32;
        public const int ERROR_INVALID_PARAMETER = 87;
        public const int FORMAT_MESSAGE_IGNORE_INSERTS = 512;
        public const int FORMAT_MESSAGE_FROM_SYSTEM = 4096;
        public const int FORMAT_MESSAGE_ARGUMENT_ARRAY = 8192;
        public const int ERROR_COUNTER_TIMEOUT = 1121;

#if FEATURE_NETCORE
        [SecuritySafeCritical]
#endif

        internal static string GetMessage(int errorCode) {
            StringBuilder sb = new StringBuilder(512);
            var result = FormatMessage(FORMAT_MESSAGE_IGNORE_INSERTS |
                                       FORMAT_MESSAGE_FROM_SYSTEM |
                                       FORMAT_MESSAGE_ARGUMENT_ARRAY,
                IntPtr.Zero, (uint) errorCode, 0, sb, sb.Capacity, null);
            if (result != 0) {
                // result is the # of characters copied to the StringBuilder on NT,
                // but on Win9x, it appears to be the number of MBCS bytes.
                // Just give up and return the String as-is...
                var s = sb.ToString();
                return s;
            }
            return SR.IO_UnknownError.ToResValue(errorCode);
        }

#if FEATURE_NETCORE
        [SecuritySafeCritical]
#endif

        internal static void WinIOError() {
            var errorCode = Marshal.GetLastWin32Error();
            WinIOError(errorCode, string.Empty);
        }

#if FEATURE_NETCORE
        [SecuritySafeCritical]
#endif

        internal static void WinIOError(string str) {
            var errorCode = Marshal.GetLastWin32Error();
            WinIOError(errorCode, str);
        }

        // After calling GetLastWin32Error(), it clears the last error field,
        // so you must save the HResult and pass it to this method.  This method
        // will determine the appropriate exception to throw dependent on your 
        // error, and depending on the error, insert a string into the message 
        // gotten from the ResourceManager.
        internal static void WinIOError(int errorCode, string str) {
            switch (errorCode) {
                case ERROR_FILE_NOT_FOUND:
                case ERROR_PATH_NOT_FOUND:
                    if (str.Length == 0)
                        throw new IOException(SR.IO_PortNotFound.ToResValue());
                    throw new IOException(SR.IO_PortNotFoundFileName.ToResValue(str));

                case ERROR_ACCESS_DENIED:
                    if (str.Length == 0)
                        throw new UnauthorizedAccessException(SR.UnauthorizedAccess_IODenied_NoPathName.ToResValue());
                    throw new UnauthorizedAccessException(SR.UnauthorizedAccess_IODenied_Path.ToResValue(str));

                case ERROR_FILENAME_EXCED_RANGE:
                    throw new PathTooLongException(SR.IO_PathTooLong.ToResValue());

                case ERROR_SHARING_VIOLATION:
                    // error message.
                    if (str.Length == 0)
                        throw new IOException(SR.IO_SharingViolation_NoFileName.ToResValue());
                    throw new IOException(SR.IO_SharingViolation_File.ToResValue(str));

                default:
                    throw new IOException(GetMessage(errorCode), MakeHRFromErrorCode(errorCode));
            }
        }

        // Use this to translate error codes like the above into HRESULTs like
        // 0x80070006 for ERROR_INVALID_HANDLE
        internal static int MakeHRFromErrorCode(int errorCode) {
            return unchecked(((int) 0x80070000) | errorCode);
        }
    }
}