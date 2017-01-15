using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

// https://msdn.microsoft.com/en-us/library/windows/desktop/aa363435%28v=vs.85%29.aspx

namespace Gbd.IO.Serial.Win32.native {
    internal class ComMask {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetCommMask(SafeFileHandle hFile, int dwEvtMask);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetCommMask(SafeFileHandle hFile, ref int dwEvtMask);

        [FlagsAttribute]
        public enum EvtMask {
            /// <summary> A break was detected on input. </summary>
            EV_BREAK = 0x0040,

            /// <summary> The CTS (clear-to-send) signal changed state. </summary>
            EV_CTS = 0x0008,

            /// <summary> The DSR (data-set-ready) signal changed state. </summary>
            EV_DSR = 0x0010,

            /// <summary> A line-status error occurred. Line-status errors are CE_FRAME, CE_OVERRUN, and CE_RXPARITY. </summary>
            EV_ERR = 0x0080,

            /// <summary> A ring indicator was detected. </summary>
            EV_RING = 0x0100,

            /// <summary>The RLSD (receive-line-signal-detect / Carrier Detect) signal changed state. </summary>
            EV_RLSD = 0x0020,

            /// <summary> A character was received and placed in the input buffer. </summary>
            EV_RXCHAR = 0x0001,

            /// <summary>The event character was received and placed in the input buffer. The event character is specified in the device's DCB structure, which is applied to a serial port by using the SetCommState function. </summary>
            EV_RXFLAG = 0x0002,

            /// <summary> The last character in the output buffer was sent. </summary>
            EV_TXEMPTY = 0x0004,

            /// <summary> Disable com events. </summary>
            NONE = 0,

            /// <summary> All events except for EV_TXEMPTY, which is typically what we're interested in. </summary>
            ALL_EVENTS = 507,
        }

        public SafeFileHandle Handle;

        public EvtMask MaskValue;

        /// <summary> Reads the event mask. </summary>
        public void Read() {
            int tmpint = 0;
            GetCommMask(Handle, ref tmpint);
            MaskValue = (EvtMask) tmpint;
        }

        /// <summary> Writes the event mask. </summary>
        public void Write() {
            SetCommMask(Handle, (int) MaskValue);
        }
    }
}