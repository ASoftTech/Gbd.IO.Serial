using Gbd.IO.Serial.Enums;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

// https://msdn.microsoft.com/en-gb/library/windows/desktop/aa363214%28v=vs.85%29.aspx

namespace Gbd.IO.Serial.Win32.native {
    /// <summary> Wrapper class for accessing the DCB registers for serial port settings. </summary>
    internal class DCB {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool SetCommState(SafeFileHandle hFile, DCB_Unmanaged lpDCB);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool GetCommState(SafeFileHandle hFile, [Out] DCB_Unmanaged lpDCB);

        /// <summary> Unmanaged DCB Class structure passed to SetCommState / GetCommState. </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal class DCB_Unmanaged {
            public uint dcb_length;
            public uint baud_rate;
            public int flags;
            public ushort w_reserved;
            public ushort xon_lim;
            public ushort xoff_lim;
            public byte byte_size;
            public byte parity;
            public byte stop_bits;
            public byte xon_char;
            public byte xoff_char;
            public byte error_char;
            public byte eof_char;
            public byte evt_char;
            public ushort w_reserved1;
        }


        // flags:
        //private const int fBinary = 0x0001;
        //private const int fParity = 0x0002;
        private const int fOutxCtsFlow = 0x0004;
        //private const int fOutxDsrFlow = 0x0008;
        private const int fDtrControl1 = 0x0010;
        //private const int fDtrControl2 = 0x00020;
        //private const int fDsrSensitivity = 0x0040;
        //private const int fTXContinueOnXoff = 0x0080;
        private const int fOutX = 0x0100;
        private const int fInX = 0x0200;
        private const int fErrorChar = 0x0400;
        private const int fNull = 0x0800;
        private const int fRtsControl1 = 0x1000;
        private const int fRtsControl2 = 0x2000;
        //private const int fAbortOnError = 0x4000;


        public SafeFileHandle Handle;

        private readonly DCB_Unmanaged dcb;

        /// <summary> Default Constructor. </summary>
        internal DCB() {
            dcb = new DCB_Unmanaged();
        }

        /// <summary> Reads settings from serial port register. </summary>
        public void Read() {
            if (!GetCommState(Handle, dcb))
                WinError.WinIOError();
        }

        /// <summary> Writes settings to serial port register. </summary>
        public void Write() {
            if (!SetCommState(Handle, dcb))
                WinError.WinIOError();
        }

        //DCB Serial Port Settings

        public uint BaudRate {
            get { return dcb.baud_rate; }
            set { dcb.baud_rate = value; }
        }

        public StopBits StopBits {
            get {
                StopBits ret = StopBits.None;
                switch (dcb.stop_bits) {
                    case 0:
                        ret = StopBits.One;
                        break;
                    case 1:
                        ret = StopBits.OnePointFive;
                        break;
                    case 2:
                        ret = StopBits.Two;
                        break;
                }
                return ret;
            }
            set {
                switch (value) {
                    case StopBits.One:
                        dcb.stop_bits = 0;
                        break;
                    case StopBits.OnePointFive:
                        dcb.stop_bits = 1;
                        break;
                    case StopBits.Two:
                        dcb.stop_bits = 2;
                        break;
                }
            }
        }

        public Parity Parity {
            get { return (Parity) dcb.parity; }
            set { dcb.parity = (byte) value; }
        }

        public byte ParityReplace_Byte {
            get { return dcb.error_char; }
            set { dcb.error_char = value; }
        }

        public bool ParityReplace_Enable {
            get { return getflag(fErrorChar); }
            set { setflag(fErrorChar, value); }
        }

        public DataBits ByteSize {
            get { return (DataBits) dcb.byte_size; }
            set { dcb.byte_size = (byte) value; }
        }

        public Handshake Handshake {
            get {
                Handshake ret = Handshake.None;
                int tmpflags = 0;
                tmpflags &= ~(fOutxCtsFlow | fOutX | fInX | fRtsControl2);
                if (dcb.flags == (tmpflags | fOutX | fInX)) ret = Handshake.XOnXOff;
                else if (dcb.flags == (tmpflags | fOutxCtsFlow | fRtsControl2)) ret = Handshake.RequestToSend;
                else if (dcb.flags == (tmpflags | fOutxCtsFlow | fOutX | fInX | fRtsControl2))
                    ret = Handshake.RequestToSendXOnXOff;
                return ret;
            }
            set {
                // Clear Handshake flags
                dcb.flags &= ~(fOutxCtsFlow | fOutX | fInX | fRtsControl2);

                // Set Handshake flags
                switch (value) {
                    case Handshake.None:
                        break;
                    case Handshake.XOnXOff:
                        dcb.flags |= fOutX | fInX;
                        break;
                    case Handshake.RequestToSend:
                        dcb.flags |= fOutxCtsFlow | fRtsControl2;
                        break;
                    case Handshake.RequestToSendXOnXOff:
                        dcb.flags |= fOutxCtsFlow | fOutX | fInX | fRtsControl2;
                        break;
                }
            }
        }

        public bool DiscardNull {
            get { return getflag(fNull); }
            set { setflag(fNull, value); }
        }

        public bool RtsPinEnable {
            get { return getflag(fRtsControl1); }
            set { setflag(fRtsControl1, value); }
        }

        public bool DtrPinEnable {
            get { return getflag(fDtrControl1); }
            set { setflag(fDtrControl1, value); }
        }

        private void setflag(int flag, bool value) {
            if (value) dcb.flags |= flag;
            else dcb.flags &= ~(flag);
        }

        private bool getflag(int flag) {
            if ((dcb.flags & flag) == flag) return true;
            return false;
        }
    }
}