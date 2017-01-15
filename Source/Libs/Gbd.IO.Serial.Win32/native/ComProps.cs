using Gbd.IO.Serial.Error;
using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

// https://msdn.microsoft.com/en-gb/library/windows/desktop/aa363189%28v=vs.85%29.aspx

namespace Gbd.IO.Serial.Win32.native {
    public class ComProps {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetCommProperties(SafeFileHandle hFile, Comprops_Unmanaged lpCommProp);

        /// <summary> Unmanaged structure for comm port properties. </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal class Comprops_Unmanaged {
            public ushort wPacketLength;
            public ushort wPacketVersion;
            public int dwServiceMask;
            public int dwReserved1;
            public int dwMaxTxQueue;
            public int dwMaxRxQueue;
            public int dwMaxBaud;
            public int dwProvSubType;
            public int dwProvCapabilities;
            public int dwSettableParams;
            public int dwSettableBaud;
            public ushort wSettableData;
            public ushort wSettableStopParity;
            public int dwCurrentTxQueue;
            public int dwCurrentRxQueue;
            public int dwProvSpec1;
            public int dwProvSpec2;
            public char wcProvChar;
        }

        public SafeFileHandle Handle;

        private readonly Comprops_Unmanaged _commprops;

        /// <summary> Default Constructor. </summary>
        internal ComProps() {
            _commprops = new Comprops_Unmanaged();
        }

        /// <summary> Reads com port properties from win32. </summary>
        public void Read() {
            if (!GetCommProperties(Handle, _commprops)) {
                var errorCode = Marshal.GetLastWin32Error();
                if ((errorCode == WinError.ERROR_INVALID_PARAMETER) ||
                    (errorCode == WinError.ERROR_INVALID_HANDLE))
                    throw new ArgumentException(SR.Arg_InvalidSerialPortExtended.ToResValue());
                WinError.WinIOError(errorCode, string.Empty);
            }
        }

        /// <summary> Gets the maximum baud rate. </summary>
        /// <value> The maximum baud rate. </value>
        public double MaxBaudRate {
            get {
                switch (_commprops.dwMaxBaud) {
                    case 0x00000001:
                        return 75;
                    case 0x00000002:
                        return 110;
                    case 0x00000004:
                        return 134.5;
                    case 0x00000008:
                        return 150;
                    case 0x00000010:
                        return 300;
                    case 0x00000020:
                        return 600;
                    case 0x00000040:
                        return 1200;
                    case 0x00000080:
                        return 1800;
                    case 0x00000100:
                        return 2400;
                    case 0x00000200:
                        return 4800;
                    case 0x00000400:
                        return 7200;
                    case 0x00000800:
                        return 9600;
                    case 0x00001000:
                        return 14400;
                    case 0x00002000:
                        return 19200;
                    case 0x00004000:
                        return 38400;
                    case 0x00008000:
                        return 56000;
                    case 0x00040000:
                        return 57600;
                    case 0x00020000:
                        return 115200;
                    case 0x00010000:
                        return 128000;
                    //0x10000000 = Programmable baud rate
                }
                return _commprops.dwMaxBaud;
            }
        }
    }
}