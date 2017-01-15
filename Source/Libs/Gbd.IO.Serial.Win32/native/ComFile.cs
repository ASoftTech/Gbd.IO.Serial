using Gbd.IO.Serial.Error;
using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;

namespace Gbd.IO.Serial.Win32.native {
    public class ComFile {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true, BestFitMapping = false)]
        internal static extern SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode,
            IntPtr securityAttrs, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern int GetFileType(SafeFileHandle hFile);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool FlushFileBuffers(SafeFileHandle hFile);

        [DllImport("kernel32", SetLastError = true)]
        internal static extern bool PurgeComm(SafeFileHandle handle, uint flags);

        [DllImport("kernel32", SetLastError = true)]
        public static extern unsafe bool WriteFile(SafeFileHandle handle, byte* buffer, int numBytesToWrite,
            out int numBytesWritten, IntPtr overlapped);

        [DllImport("kernel32", SetLastError = true)]
        public static extern unsafe bool ReadFile(SafeFileHandle handle, byte* buffer, int bytes_to_read,
            out int numBytesRead, IntPtr overlapped);

        [DllImport("kernel32", SetLastError = true)]
        public static extern bool GetOverlappedResult(SafeFileHandle handle, IntPtr overlapped, ref int bytes_transfered, bool wait);

        [FlagsAttribute]
        public enum PurgeSetting {
            PURGERX = 0x0008,
            PURGETX = 0x0004,
        }

        public const int GENERIC_READ = -2147483648;
        public const int GENERIC_WRITE = 1073741824;
        public const int OPEN_EXISTING = 3;
        public const int FILE_FLAG_OVERLAPPED = 1073741824;
        public const int FILE_TYPE_UNKNOWN = 0;
        public const int FILE_TYPE_CHAR = 2;
        public const uint FILEIOPENDING = 997;

        public SafeFileHandle Handle;

        public string Name;

        public int FileType;

        /// <summary> Default Constructor. </summary>
        internal ComFile(string nameparam) {
            Name = nameparam;
        }

        public void Open() {
            //Error checking done in SerialPort.
            var flags = FILE_FLAG_OVERLAPPED;
            var portname = (Name != null && !Name.StartsWith(@"\\.\"))
                ? @"\\.\" + Name : Name;
            Handle = CreateFile(portname, GENERIC_READ | GENERIC_WRITE, 0, IntPtr.Zero, OPEN_EXISTING, flags, IntPtr.Zero);

            if (Handle.IsInvalid)
                WinError.WinIOError(Name);
        }

        public void Close() {
            Handle.Close();
        }

        public void GetFileType() {
            FileType = GetFileType(Handle);

            // Allowing FILE_TYPE_UNKNOWN for legitimate serial device such as USB to serial adapter device 
            if ((FileType != FILE_TYPE_CHAR) &&
                (FileType != FILE_TYPE_UNKNOWN))
                throw new ArgumentException(SR.Arg_InvalidSerialPort.ToResValue());
        }

        public void FlushFileBuffers() {
            FlushFileBuffers(Handle);
        }

        /// <summary> Purge the Rx or Tx Buffers. </summary>
        /// <param name="flags"> Which buffers to purge. </param>
        public void PurgeBuffer(PurgeSetting flags) {
            if (!PurgeComm(Handle, (uint) flags))
                WinError.WinIOError();
        }
    }
}