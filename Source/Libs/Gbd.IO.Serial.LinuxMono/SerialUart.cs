using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Gbd.IO.Serial.Base;
using Gbd.IO.Serial.Error;
using Gbd.IO.Serial.Event;
using Gbd.IO.Serial.Interfaces;

// TODO OnDataRx
// TODO OnErrorRx
// See WaitForCommEvent / UnsafeNativeMethods.WaitCommEvent
// https://msdn.microsoft.com/en-us/library/windows/desktop/aa363479%28v=vs.85%29.aspx

namespace Gbd.IO.Serial.LinuxMono {
    /// <summary> A serial uart. </summary>
    public class SerialUart : ISerialUart {
        /// <summary> The associated serial port. </summary>
        public SerialPort Port => _Port;

        protected SerialPort _Port;
        private bool disposed;

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
            disposed = false;
        }

        /// <summary> Finaliser. </summary>
        ~SerialUart() {
            Dispose();
        }

        /// <summary> Free unmanaged memory blocks. </summary>
        internal void Dispose() {
            if (disposed) return;
            disposed = true;
        }

        private bool portopen() {
            if (_Port == null) return false;
            return _Port.IsOpen;
        }

        /// <summary> Reads from the serial stream. </summary>
        /// <exception cref="InvalidOperationException">   Thrown when the requested operation is
        ///                                                invalid. </exception>
        /// <exception cref="ArgumentNullException">       Thrown when one or more required arguments
        ///                                                are null. </exception>
        /// <exception cref="ArgumentOutOfRangeException"> Thrown when one or more arguments are outside
        ///                                                the required range. </exception>
        /// <exception cref="ArgumentException">           Thrown when one or more arguments have
        ///                                                unsupported or illegal values. </exception>
        /// <param name="buffer"> The buffer. </param>
        /// <param name="offset"> The offset. </param>
        /// <param name="count">  Number of bytes to read. </param>
        /// <returns> Number of bytes read. </returns>
        public int Read([In, Out] byte[] buffer, int offset, int count) {
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

            var result = _Port.monoserialfile.ReadSerial(buffer, offset, count, _Port._BufferSettings.ReadTimeout);
            return result;
        }

        /// <summary> Writes to the serial stream. </summary>
        /// <param name="buffer"> The buffer. </param>
        /// <param name="offset"> The offset. </param>
        /// <param name="count">  Number of bytes to write. </param>
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

            // TODO: this reports every write error as timeout
            _Port.monoserialfile.WriteSerial(buffer, offset, count, _Port._BufferSettings.WriteTimeout);
        }

        /// <summary> Flushes the buffers. </summary>
        public void Flush() {
            //TODO Not implemented by mono
        }

        /// <summary> Discard the input buffer. </summary>
        /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
        public void DiscardInBuffer() {
            if (!_Port.IsOpen) throw new InvalidOperationException(SR.Port_not_open.ToResValue());
            _Port.monoserialfile.DiscardInBuffer();
        }

        /// <summary> Discard the output buffer. </summary>
        /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
        public void DiscardOutBuffer() {
            if (!_Port.IsOpen) throw new InvalidOperationException(SR.Port_not_open.ToResValue());
            _Port.monoserialfile.DiscardOutBuffer();
        }

        /// <summary> Gets the number of bytes available to read. </summary>
        /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
        /// <value> The number of bytes available to read. </value>
        public int BytesToRead {
            get {
                if (!_Port.IsOpen) throw new InvalidOperationException(SR.Port_not_open.ToResValue());
                return _Port.monoserialfile.GetBytesToRead();
            }
        }

        /// <summary> Gets the number of bytes available to write. </summary>
        /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
        /// <value> The number of bytes available to write. </value>
        public int BytesToWrite {
            get {
                if (!_Port.IsOpen) throw new InvalidOperationException(SR.Port_not_open.ToResValue());
                return _Port.monoserialfile.GetBytesToWrite();
            }
        }
    }
}