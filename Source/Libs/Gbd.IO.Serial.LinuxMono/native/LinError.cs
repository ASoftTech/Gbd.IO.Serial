using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Gbd.IO.Serial.LinuxMono.native {
    /// <summary> Used for Capturing IO errors from Linux. </summary>
    public class LinError {
        [DllImport("libc")]
        private static extern IntPtr strerror(int errnum);

        /// <summary> Capture and throw the last linux io error. </summary>
        /// <exception cref="IOException"> Thrown when an IO failure occurred. </exception>
        internal static void LinIOError() {
            var errnum = Marshal.GetLastWin32Error();
            var error_message = Marshal.PtrToStringAnsi(strerror(errnum));
            throw new IOException(error_message);
        }
    }
}