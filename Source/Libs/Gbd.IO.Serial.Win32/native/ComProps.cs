using Gbd.IO.Serial.Error;
using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using Gbd.IO.Serial.Win32.Settings.SerialInfoEnums;

// https://msdn.microsoft.com/en-gb/library/windows/desktop/aa363189%28v=vs.85%29.aspx

namespace Gbd.IO.Serial.Win32.native {
    public class ComProps {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetCommProperties(SafeFileHandle hFile, ref Comprops_Unmanaged lpCommProp);

        /// <summary> Unmanaged structure for comm port properties. </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct Comprops_Unmanaged {
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

        internal Comprops_Unmanaged _commprops;

        /// <summary> Default Constructor. </summary>
        internal ComProps() {
            _commprops = new Comprops_Unmanaged();
        }

        /// <summary> Reads com port properties from win32. </summary>
        public void Read() {
            if (GetCommProperties(Handle, ref _commprops)) return;
            var errorCode = Marshal.GetLastWin32Error();
            if ((errorCode == WinError.ERROR_INVALID_PARAMETER) ||
                (errorCode == WinError.ERROR_INVALID_HANDLE))
                throw new ArgumentException(SR.Arg_InvalidSerialPortExtended.ToResValue());
            WinError.WinIOError(errorCode, string.Empty);
        }


        //TODO remove this in favour of serial properties

        /// <summary> Gets the maximum baud rate as a string. </summary>
        /// <value> The maximum baud rate as a string. </value>
        public double MaxBaudRate {
            get {
                BaudInfoEnum tempval;
                Enum.TryParse(_commprops.dwMaxBaud.ToString(), out tempval);
                return tempval.ToBaudRate();
            }
        }
    }
}