using Gbd.IO.Serial.Base;
using Gbd.IO.Serial.Error;
using Gbd.IO.Serial.Event;
using Gbd.IO.Serial.Interfaces;
using Gbd.IO.Serial.Win32.native;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

// TODO OnDataRx
// TODO OnErrorRx

namespace Gbd.IO.Serial.Win32 {
    public class SerialUart : ISerialUart {
        /// <summary> The associated serial port. </summary>
        public SerialPort Port => _Port;

        protected SerialPort _Port;
        private bool disposed;

        internal IntPtr read_overlap;
        internal IntPtr write_overlap;

        /// <summary> Event queue for all listeners interested in DataReceived events. </summary>
        public event EventHandler DataRx;

        /// <summary> Raises the data receive event. </summary>
        /// <param name="e"> Event information to send to registered event handlers. </param>
        protected internal virtual void OnDataRx(DataRxEventArgs e) {
            DataRx?.Invoke(this, e);
        }

        /// <summary> Event queue for all listeners interested in ErrorRx events. </summary>
        public event EventHandler ErrorRx;

        /// <summary> Raises the data error event. </summary>
        /// <param name="e"> Event information to send to registered event handlers. </param>
        protected internal virtual void OnErrorRx(DataErrorEventArgs e) {
            ErrorRx?.Invoke(this, e);
        }

        /// <summary> Constructor. </summary>
        /// <param name="sport"> The serial port to associate with. </param>
        internal SerialUart(SerialPort sport) {
            _Port = sport;
            read_overlap = IntPtr.Zero;
            write_overlap = IntPtr.Zero;
            disposed = false;

            var writeov = new NativeOverlapped();
            write_overlap = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (NativeOverlapped)));
            Marshal.StructureToPtr(writeov, write_overlap, true);

            var readov = new NativeOverlapped();
            read_overlap = Marshal.AllocHGlobal(Marshal.SizeOf(typeof (NativeOverlapped)));
            Marshal.StructureToPtr(readov, read_overlap, true);
        }

        /// <summary> Finaliser. </summary>
        ~SerialUart() {
            Dispose();
        }

        /// <summary> Free unmanaged memory blocks. </summary>
        internal void Dispose() {
            if (disposed) return;
            Marshal.FreeHGlobal(read_overlap);
            Marshal.FreeHGlobal(write_overlap);
            disposed = true;
        }

        private bool portopen() {
            return _Port != null && _Port.IsOpen;
        }

        /// <summary>
        ///     When overridden in a derived class, reads a sequence of bytes from the current stream and
        ///     advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer"> An array of bytes. When this method returns, the buffer contains the
        ///                       specified byte array with the values between
        ///                       <paramref name="offset" /> and (<paramref name="offset" /> +
        ///                       <paramref name="count" /> - 1) replaced by the bytes read from the
        ///                       current source. </param>
        /// <param name="offset"> The zero-based byte offset in <paramref name="buffer" /> at which to
        ///                       begin storing the data read from the current stream. </param>
        /// <param name="count">  The maximum number of bytes to be read from the current stream. </param>
        /// <returns>
        ///     The total number of bytes read into the buffer. This can be less than the number of bytes
        ///     requested if that many bytes are not currently available, or zero (0) if the end of the
        ///     stream has been reached.
        /// </returns>
        public unsafe int Read([In, Out] byte[] buffer, int offset, int count) {
            if (!portopen())
                throw new InvalidOperationException(SR.Port_not_open.ToResValue());
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer), SR.ArgumentNull_Buffer.ToResValue());
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset),
                    SR.ArgumentOutOfRange_NeedNonNegNumRequired.ToResValue());
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count),
                    SR.ArgumentOutOfRange_NeedNonNegNumRequired.ToResValue());
            if (buffer.Length - offset < count)
                throw new ArgumentException(SR.Argument_InvalidOffLen.ToResValue());
            if (count == 0) return 0; // return immediately if no bytes requested; no need for overhead.

            int bytes_read;
            fixed (byte* ptr = buffer) {
                if (ComFile.ReadFile(_Port._handle, ptr + offset, count, out bytes_read, read_overlap))
                    return bytes_read;
                if (Marshal.GetLastWin32Error() != ComFile.FILEIOPENDING)
                    WinError.WinIOError();
                if (!ComFile.GetOverlappedResult(_Port._handle, read_overlap, ref bytes_read, true))
                    WinError.WinIOError();
            }
            if (bytes_read == 0)
                throw new TimeoutException(); // We didn't get any byte
            return bytes_read;
        }

        /// <summary>
        ///     When overridden in a derived class, writes a sequence of bytes to the current stream and
        ///     advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer"> An array of bytes. This method copies <paramref name="count" /> bytes
        ///                       from <paramref name="buffer" /> to the current stream. </param>
        /// <param name="offset"> The zero-based byte offset in <paramref name="buffer" /> at which to
        ///                       begin copying bytes to the current stream. </param>
        /// <param name="count">  The number of bytes to be written to the current stream. </param>
        public void Write(byte[] buffer, int offset, int count) {
            if (!portopen())
                throw new InvalidOperationException(SR.Port_not_open.ToResValue());
            if (_Port.PinStates.BreakState)
                throw new InvalidOperationException(SR.In_Break_State.ToResValue());
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer), SR.ArgumentNull_Array.ToResValue());
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset), SR.ArgumentOutOfRange_NeedPosNum.ToResValue());
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), SR.ArgumentOutOfRange_NeedPosNum.ToResValue());
            if (count == 0) return; // no need to expend overhead in creating asyncResult, etc.
            if (buffer.Length == 0) return;
            if (buffer.Length - offset < count)
                throw new ArgumentException("count", SR.ArgumentOutOfRange_OffsetOut.ToResValue());
            Debug.Assert(
                _Port.BufferSettings.WriteTimeout == SerialBufferSettingsBase.InfiniteTimeout ||
                _Port.BufferSettings.WriteTimeout >= 0,
                "Serial Stream Write - write timeout is " + _Port.BufferSettings.WriteTimeout);

            // check for open handle, though the port is always supposed to be open
            if (_Port._handle == null)
                throw new ObjectDisposedException(null, SR.Port_not_open.ToResValue());

            int written;
            unsafe {
                fixed (byte* ptr = buffer) {
                    if (ComFile.WriteFile(_Port._handle, ptr + offset, count, out written, write_overlap))
                        return;
                    if (Marshal.GetLastWin32Error() != ComFile.FILEIOPENDING)
                        WinError.WinIOError();
                    if (!ComFile.GetOverlappedResult(_Port._handle, write_overlap, ref written, true))
                        WinError.WinIOError();
                }
            }

            if (written < count)
                throw new TimeoutException();
        }

        /// <summary>
        ///     Flush dumps the contents of the serial driver's internal read and write buffers. We
        ///     actually expose the functionality for each, but fulfilling Stream's contract requires a
        ///     Flush() method.  Fails if handle closed. Note: Serial driver's write buffer is *already*
        ///     attempting to write it, so we can only wait until it finishes.
        /// </summary>
        /// <exception cref="ObjectDisposedException"> Thrown when a supplied object has been disposed. </exception>
        public void Flush() {
            if (!_Port.IsOpen) throw new InvalidOperationException(SR.Port_not_open.ToResValue());
            _Port.comfile.FlushFileBuffers();
        }

        /// <summary> Discard the input buffer. </summary>
        public void DiscardInBuffer() {
            if (!_Port.IsOpen) throw new InvalidOperationException(SR.Port_not_open.ToResValue());
            _Port.comfile.PurgeBuffer(ComFile.PurgeSetting.PURGERX);
        }

        /// <summary> Discard the output buffer. </summary>
        public void DiscardOutBuffer() {
            if (!_Port.IsOpen) throw new InvalidOperationException(SR.Port_not_open.ToResValue());
            _Port.comfile.PurgeBuffer(ComFile.PurgeSetting.PURGETX);
        }

        /// <summary> Gets the number of bytes available to read. </summary>
        /// <value> The number of bytes available to read. </value>
        public int BytesToRead {
            get {
                if (!_Port.IsOpen) throw new InvalidOperationException(SR.Port_not_open.ToResValue());
                _Port.combufferstatus.Read();
                return _Port.combufferstatus.BytesToRead;
            }
        }

        /// <summary> Gets the number of bytes left to write. </summary>
        /// <value> The number of bytes left to write. </value>
        public int BytesToWrite {
            get {
                if (!_Port.IsOpen) throw new InvalidOperationException(SR.Port_not_open.ToResValue());
                _Port.combufferstatus.Read();
                return _Port.combufferstatus.BytesToWrite;
            }
        }
    }
}