using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

// https://msdn.microsoft.com/en-us/library/windows/desktop/aa363254%28v=vs.85%29.aspx

namespace Gbd.IO.Serial.Win32.native {
    /// <summary> Access to Extended Com Funnctions. </summary>
    internal class EscapeCom {
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool EscapeCommFunction(SafeFileHandle hFile, int dwFunc);

        internal enum ExtendedFunctions {
            /// <summary> Causes transmission to act as if an XOFF character has been received.. </summary>
            SETXOFF = 1,

            /// <summary> Causes transmission to act as if an XON character has been received. </summary>
            SETXON = 2,

            /// <summary> Sends the RTS (request-to-send) signal. </summary>
            SETRTS = 3,

            /// <summary> Clears the RTS (request-to-send) signal. </summary>
            CLRRTS = 4,

            /// <summary> Sends the DTR (data-terminal-ready) signal. </summary>
            SETDTR = 5,

            /// <summary> Clears the DTR (data-terminal-ready) signal. </summary>
            CLRDTR = 6,

            /// <summary> Suspends character transmission and places the transmission line in a break state. </summary>
            SETBREAK = 8,

            /// <summary> Restores character transmission and places the transmission line in a nonbreak state. </summary>
            CLRBREAK = 9,
        }

        public SafeFileHandle Handle;

        /// <summary> Writes the extended comm function. </summary>
        /// <param name="value"> The value. </param>
        public void Write(ExtendedFunctions value) {
            if (!EscapeCommFunction(Handle, (int) value))
                WinError.WinIOError();
        }

        /// <summary> Writes the extended comm function. </summary>
        /// <param name="value"> The value. </param>
        public bool WriteBool(ExtendedFunctions value) {
            return EscapeCommFunction(Handle, (int) value);
        }
    }
}