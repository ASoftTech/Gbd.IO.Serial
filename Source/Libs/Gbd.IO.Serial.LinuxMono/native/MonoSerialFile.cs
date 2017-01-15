using System;
using System.Runtime.InteropServices;

namespace Gbd.IO.Serial.LinuxMono.native {
    public class MonoSerialFile {
        [DllImport("MonoPosixHelper", SetLastError = true)]
        private static extern int get_bytes_in_buffer(int fd, int input);

        [DllImport("MonoPosixHelper", SetLastError = true)]
        private static extern int discard_buffer(int fd, bool inputBuffer);

        [DllImport("MonoPosixHelper", SetLastError = true)]
        private static extern int open_serial(string portName);

        [DllImport("MonoPosixHelper", SetLastError = true)]
        private static extern int close_serial(int fd);

        [DllImport("MonoPosixHelper", SetLastError = true)]
        private static extern int read_serial(int fd, byte[] buffer, int offset, int count);

        [DllImport("MonoPosixHelper", SetLastError = true)]
        private static extern int write_serial(int fd, byte[] buffer, int offset, int count, int timeout);

        [DllImport("MonoPosixHelper", SetLastError = true)]
        private static extern bool poll_serial(int fd, out int error, int timeout);

        public int Handle;

        /// <summary> Default constructor. </summary>
        internal MonoSerialFile() {}

        /// <summary> Discard input buffer. </summary>
        public void DiscardInBuffer() {
            if (discard_buffer(Handle, true) != 0)
                LinError.LinIOError();
        }

        /// <summary> Discard output buffer. </summary>
        public void DiscardOutBuffer() {
            if (discard_buffer(Handle, false) != 0)
                LinError.LinIOError();
        }

        /// <summary> Gets the number of bytes available to read. </summary>
        /// <returns> The number of bytes available to read. </returns>
        public int GetBytesToRead() {
            int result = get_bytes_in_buffer(Handle, 1);
            if (result == -1)
                LinError.LinIOError();
            return result;
        }

        /// <summary> Gets the number of bytes available to write. </summary>
        /// <returns> The number of bytes available to write. </returns>
        public int GetBytesToWrite() {
            var result = get_bytes_in_buffer(Handle, 0);
            if (result == -1)
                LinError.LinIOError();
            return result;
        }

        /// <summary> Reads from the serial port. </summary>
        /// <exception cref="TimeoutException"> Thrown when a Timeout error condition occurs. </exception>
        /// <param name="buffer">  The buffer. </param>
        /// <param name="offset">  The offset. </param>
        /// <param name="count">   Number of. </param>
        /// <param name="timeout"> The timeout. </param>
        /// <returns> The serial. </returns>
        public int ReadSerial([In, Out] byte[] buffer, int offset, int count, int timeout) {
            int error;
            var poll_result = poll_serial(Handle, out error, timeout);
            if (error == -1)
                LinError.LinIOError();
            if (!poll_result) // see bug 79735   http://bugzilla.ximian.com/show_bug.cgi?id=79735
                // should the next line read: return -1; 
                throw new TimeoutException();
            var result = read_serial(Handle, buffer, offset, count);
            if (result == -1)
                throw new TimeoutException("The operation has timed-out");
            return result;
        }

        /// <summary> Writes to the serial output. </summary>
        /// <exception cref="TimeoutException"> Thrown when a Timeout error condition occurs. </exception>
        /// <param name="buffer">  The buffer. </param>
        /// <param name="offset">  The offset. </param>
        /// <param name="count">   Number of. </param>
        /// <param name="timeout"> The timeout. </param>
        public void WriteSerial(byte[] buffer, int offset, int count, int timeout) {
            if (write_serial(Handle, buffer, offset, count, timeout) < 0)
                throw new TimeoutException("The operation has timed-out");
        }

        /// <summary> Opens a port. </summary>
        /// <param name="name"> The name. </param>
        public void OpenPort(string name) {
            Handle = open_serial(name);
            if (Handle == -1)
                LinError.LinIOError();
        }

        /// <summary> Closes the serial port. </summary>
        public void ClosePort() {
            close_serial(Handle);
        }
    }
}