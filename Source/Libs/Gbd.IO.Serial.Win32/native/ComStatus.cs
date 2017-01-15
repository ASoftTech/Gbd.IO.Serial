using Gbd.IO.Serial.Error;
using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace Gbd.IO.Serial.Win32.native {
    /// <summary> Determines a serial port's pin and break status. </summary>
    internal class ComStatus {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetCommModemStatus(SafeFileHandle hFile, ref int lpModemStat);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetCommBreak(SafeFileHandle hFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool ClearCommBreak(SafeFileHandle hFile);

        internal const int MS_CTS_ON = 16;
        internal const int MS_DSR_ON = 32;
        internal const int MS_RING_ON = 64;
        internal const int MS_RLSD_ON = 128;

        public SafeFileHandle Handle;

        public int Status;

        /// <summary> Reads settings from serial port register. </summary>
        public void Read() {
            if (!GetCommModemStatus(Handle, ref Status)) {
                var errorCode = Marshal.GetLastWin32Error();
                if ((errorCode == WinError.ERROR_INVALID_PARAMETER) ||
                    (errorCode == WinError.ERROR_INVALID_HANDLE))
                    throw new ArgumentException(SR.Arg_InvalidSerialPortExtended.ToResValue());
                WinError.WinIOError(errorCode, string.Empty);
            }
        }

        /// <summary> Carrier Detect Pin. </summary>
        public bool CD_Detect => (MS_RLSD_ON & Status) != 0;

        /// <summary> Ring Detect Pin. </summary>
        public bool Ring_Detect => (MS_RING_ON & Status) != 0;

        /// <summary> CTS Detect Pin. </summary>
        public bool CTS_Detect => (MS_CTS_ON & Status) != 0;

        /// <summary> DSR Detect Pin. </summary>
        public bool DSR_Detect => (MS_DSR_ON & Status) != 0;

        /// <summary> Writes a break state. </summary>
        /// <param name="value"> true to set break state, false to clear break state. </param>
        public void WriteBreakState(bool value) {
            if (value) {
                if (!SetCommBreak(Handle))
                    WinError.WinIOError();
            }
            else if (!ClearCommBreak(Handle))
                WinError.WinIOError();
        }
    }
}