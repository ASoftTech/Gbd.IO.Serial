using System;
using System.IO;
using Gbd.IO.Serial.Error;
using Gbd.IO.Serial.Interfaces;

namespace Gbd.IO.Serial.Streams {
    /// <summary> A serial stream. </summary>
    public class SerialStream : Stream {
        protected ISerialPort _SPort;

        public override bool CanTimeout => _SPort.IsOpen;

        public override bool CanRead => _SPort.IsOpen;

        public override bool CanWrite => _SPort.IsOpen;

        public override bool CanSeek => false;

        /// <summary> Default Constructor. </summary>
        /// <param name="sport"> The serial port. </param>
        public SerialStream(ISerialPort sport) {
            _SPort = sport;
        }

        /// <summary> Flushes all buffers for the serial port. </summary>
        public override void Flush() {
            _SPort.Uart.Flush();
        }

        /// <summary> Reads a sequence of bytes to the serial port. </summary>
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
        public override int Read(byte[] buffer, int offset, int count) {
            return _SPort.Uart.Read(buffer, offset, count);
        }

        /// <summary> Writes a sequence of bytes to the serial port. </summary>
        /// <param name="buffer"> An array of bytes. This method copies <paramref name="count" /> bytes
        ///                       from <paramref name="buffer" /> to the current stream. </param>
        /// <param name="offset"> The zero-based byte offset in <paramref name="buffer" /> at which to
        ///                       begin copying bytes to the current stream. </param>
        /// <param name="count">  The number of bytes to be written to the current stream. </param>
        public override void Write(byte[] buffer, int offset, int count) {
            _SPort.Uart.Write(buffer, offset, count);
        }


        // Not Supported properties for a serial port

        public override long Seek(long offset, SeekOrigin origin) {
            throw new NotSupportedException(SR.NotSupported_UnseekableStream.ToResValue());
        }

        public override void SetLength(long value) {
            throw new NotSupportedException(SR.NotSupported_UnseekableStream.ToResValue());
        }

        public override long Length {
            get { throw new NotSupportedException(SR.NotSupported_UnseekableStream.ToResValue()); }
        }

        public override long Position {
            get { throw new NotSupportedException(SR.NotSupported_UnseekableStream.ToResValue()); }
            set { throw new NotSupportedException(SR.NotSupported_UnseekableStream.ToResValue()); }
        }
    }
}