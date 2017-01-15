using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

// https://msdn.microsoft.com/en-us/library/windows/desktop/aa363190%28v=vs.85%29.aspx

// Timeouts are considered to be TOTAL time for the Read/Write operation and to be in milliseconds.
// Timeouts are translated into DCB structure as follows:
// Desired timeout      =>  ReadTotalTimeoutConstant    ReadTotalTimeoutMultiplier  ReadIntervalTimeout
//  0                                   0                           0               MAXDWORD
//  0 < n < infinity                    n                       MAXDWORD            MAXDWORD
// infinity                             infiniteTimeoutConst    MAXDWORD            MAXDWORD
//
// rationale for "infinity": There does not exist in the COMMTIMEOUTS structure a way to
// *wait indefinitely for any byte, return when found*.  Instead, if we set ReadTimeout
// to infinity, SerialPort's EndRead loops if infiniteTimeoutConst mills have elapsed
// without a byte received.  Note that this is approximately 24 days, so essentially
// most practical purposes effectively equate 24 days with an infinite amount of time

namespace Gbd.IO.Serial.Win32.native {
    /// <summary> Wrapper class for handling communication timeouts. </summary>
    internal class ComTimeouts {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetCommTimeouts(SafeFileHandle hFile, Timeouts_Unamanged lpCommTimeouts);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetCommTimeouts(SafeFileHandle hFile, Timeouts_Unamanged lpCommTimeouts);

        internal const int MAXDWORD = -1;

        /// <summary> Unmanaged structure for comm port timeouts. </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal class Timeouts_Unamanged {
            public int ReadIntervalTimeout;
            public int ReadTotalTimeoutMultiplier;
            public int ReadTotalTimeoutConstant;
            public int WriteTotalTimeoutMultiplier;
            public int WriteTotalTimeoutConstant;
        }

        private const int InfiniteTimeout = -1;
        protected const int win32_infiniteTimeoutConst = -2;

        public SafeFileHandle Handle;

        private readonly Timeouts_Unamanged _timeouts;

        /// <summary> Default Constructor. </summary>
        internal ComTimeouts() {
            _timeouts = new Timeouts_Unamanged();
        }

        /// <summary> Reads timeout settings from win32. </summary>
        public void Read() {
            if (!GetCommTimeouts(Handle, _timeouts))
                WinError.WinIOError();
        }

        /// <summary> Writes timeout settings to win32. </summary>
        public void Write() {
            if (!SetCommTimeouts(Handle, _timeouts))
                WinError.WinIOError();
        }

        /// <summary> Gets or sets the read timeout. </summary>
        /// <value> The read timeout. </value>
        public int ReadTimeout {
            get {
                if (_timeouts.ReadTotalTimeoutConstant == win32_infiniteTimeoutConst)
                    return InfiniteTimeout;
                return _timeouts.ReadTotalTimeoutConstant;
            }
            set {
                switch (value) {
                    case 0:
                        _timeouts.ReadTotalTimeoutConstant = 0;
                        _timeouts.ReadTotalTimeoutMultiplier = 0;
                        break;

                    case InfiniteTimeout:
                        _timeouts.ReadTotalTimeoutConstant = win32_infiniteTimeoutConst;
                        _timeouts.ReadTotalTimeoutMultiplier = MAXDWORD;
                        break;

                    default:
                        _timeouts.ReadTotalTimeoutConstant = value;
                        _timeouts.ReadTotalTimeoutMultiplier = MAXDWORD;
                        break;
                }
                _timeouts.ReadIntervalTimeout = MAXDWORD;
            }
        }

        /// <summary> Gets or sets the write timeout. </summary>
        /// <value> The write timeout. </value>
        public int WriteTimeout {
            get {
                return _timeouts.WriteTotalTimeoutConstant == 0 ? InfiniteTimeout : _timeouts.WriteTotalTimeoutConstant;
            }
            set { _timeouts.WriteTotalTimeoutConstant = ((value == InfiniteTimeout) ? 0 : value); }
        }
    }
}