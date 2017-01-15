using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace Gbd.IO.Serial.Win32.native
{
    internal class ComBufferStatus {

        [DllImport("kernel32", SetLastError = true)]
        static extern bool SetupComm(SafeFileHandle handle, int read_buffer_size, int write_buffer_size);

        [DllImport("kernel32", SetLastError = true)]
        static extern bool ClearCommError(SafeFileHandle handle, ref uint errors, CommStat_Unmanaged stat);

        [StructLayout(LayoutKind.Sequential)]
        public class CommStat_Unmanaged {
            public uint flags;
            public uint BytesIn;
            public uint BytesOut;
        }

        public SafeFileHandle Handle;

        private readonly CommStat_Unmanaged _comstat;

        /// <summary> Default Constructor. </summary>
        internal ComBufferStatus() {
            _comstat = new CommStat_Unmanaged();
        }

        /// <summary> Sets the serial port Buffer sizes. </summary>
        /// <param name="read_buffer_size">  Size of the read buffer. </param>
        /// <param name="write_buffer_size"> Size of the write buffer. </param>
        public void SetupBuffer(int read_buffer_size, int write_buffer_size) {
            SetupComm(Handle, read_buffer_size, write_buffer_size);
        }

        /// <summary> Reads current buffer state. </summary>
        public void Read() {
            uint errors = 0;
            if (!ClearCommError(Handle, ref errors, _comstat))
                WinError.WinIOError();
        }

        /// <summary> Number of bytes available to read. </summary>
        public int BytesToRead => (int)_comstat.BytesIn;

        /// <summary> Number of bytes available to write. </summary>
        public int BytesToWrite => (int)_comstat.BytesOut;

    }
}
