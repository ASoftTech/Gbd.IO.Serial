using System;

namespace Gbd.IO.Serial.Interfaces
{
    /// <summary> Interface for serial stream. </summary>
    public interface ISerialUart {

        /// <summary> Reads from the serial stream. </summary>
        /// <param name="buffer"> The buffer. </param>
        /// <param name="offset"> The offset. </param>
        /// <param name="count">  Number of bytes to read. </param>
        /// <returns> Number of bytes read. </returns>
        int Read(byte[] buffer, int offset, int count);

        /// <summary> Writes to the serial stream. </summary>
        /// <param name="buffer"> The buffer. </param>
        /// <param name="offset"> The offset. </param>
        /// <param name="count">  Number of bytes to write. </param>
        void Write(byte[] buffer, int offset, int count);

        /// <summary> Flushes the buffers. </summary>
        void Flush();

        /// <summary> Discard the input buffer. </summary>
        void DiscardInBuffer();

        /// <summary> Discard the output buffer. </summary>
        void DiscardOutBuffer();

        /// <summary> Gets the number of bytes available to read. </summary>
        /// <value> The number of bytes available to read. </value>
        int BytesToRead { get; }

        /// <summary> Gets the number of bytes available to write. </summary>
        /// <value> The number of bytes available to write. </value>
        int BytesToWrite { get; }

        /// <summary> Event queue for all listeners interested in DataReceived events. </summary>
        event EventHandler DataRx;

        /// <summary> Event queue for all listeners interested in ErrorReceived events. </summary>
        event EventHandler ErrorRx;
    }
}
