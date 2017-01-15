using Gbd.IO.Serial.Base;
using Gbd.IO.Serial.Error;
using Gbd.IO.Serial.Interfaces;
using System;

namespace Gbd.IO.Serial.Win32.Settings {

    /// <summary> Settings for the Serial Buffer. </summary>
    public class SerialBufferSettings : SerialBufferSettingsBase {
        /// <summary> The associated serial port. </summary>
        public SerialPort Port => _Port;

        protected SerialPort _Port;

        /// <summary>
        ///     Gets or sets the number of milliseconds before a time-out occurs when a read operation
        ///     does not finish.
        /// </summary>
        /// <value> The read timeout value. </value>
        public override int ReadTimeout {
            get { return _ReadTimeout; }
            set {
                if (value <= 0 && value != InfiniteTimeout)
                    throw new ArgumentOutOfRangeException(SR.ArgumentOutOfRange_WriteTimeout.ToResValue());
                _ReadTimeout = value;
            }
        }

        protected int _ReadTimeout;

        /// <summary>
        ///     Gets or sets the number of milliseconds before a time-out occurs when a write operation
        ///     does not finish.
        /// </summary>
        /// <remarks>
        ///     WriteTimeout must be either SerialPort.InfiniteTimeout or POSITIVE.
        ///      a timeout of zero implies that every Write call throws an exception.
        /// </remarks>
        /// <value> The write timeout. </value>
        public override int WriteTimeout {
            get { return _WriteTimeout; }
            set {
                if (value <= 0 && value != InfiniteTimeout)
                    throw new ArgumentOutOfRangeException(SR.ArgumentOutOfRange_WriteTimeout.ToResValue());
                _WriteTimeout = value;
            }
        }

        protected int _WriteTimeout;

        /// <summary>
        ///     Gets or sets the number of bytes in the internal input buffer before a
        ///     SerialPort.DataReceived event occurs.
        /// </summary>
        /// <value> The received bytes threshold. </value>
        public override int ReceivedBytesThreshold {
            get { return _ReceivedBytesThreshold; }
            set {
                _ReceivedBytesThreshold = value;
                //TODO
            }
        }

        protected int _ReceivedBytesThreshold;

        /// <summary>
        ///     Gets or sets the value used to interpret the end of a call to the ReadLine and
        ///     WriteLine(System.String) methods.
        /// </summary>
        /// <value> The new line. </value>
        public override string NewLine {
            get { return _NewLine; }
            set {
                if (value == null)
                    throw new ArgumentNullException();
                if (value.Length == 0)
                    throw new ArgumentException(SR.InvalidNullEmptyArgument.ToResValue("NewLine"));
                _NewLine = value;
            }
        }

        protected string _NewLine;

        /// <summary> Gets or sets the size of the SerialPort input buffer. </summary>
        /// <value> The size of the read buffer. </value>
        public override int ReadBufferSize {
            get { return _ReadBufferSize; }
            set {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();
                _ReadBufferSize = value;
            }
        }

        protected int _ReadBufferSize;

        /// <summary> Gets or sets the size of the serial port output buffer. </summary>
        /// <value> The size of the write buffer. </value>
        public override int WriteBufferSize {
            get { return _WriteBufferSize; }
            set {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();
                _WriteBufferSize = value;
            }
        }

        protected int _WriteBufferSize;

        /// <summary> Default constructor. </summary>
        public SerialBufferSettings() {}

        /// <summary> Constructor. </summary>
        /// <param name="sport"> The serial port to associate with. </param>
        internal SerialBufferSettings(SerialPort sport) {
            _Port = sport;
        }

        private bool portopen() {
            if (_Port == null) return false;
            return _Port.IsOpen;
        }

        /// <summary> Reads settings from the port. </summary>
        public override void Read() {
            if (!portopen())
                throw new InvalidOperationException(SR.Port_not_open.ToResValue());
            _Port.dcb.Read();
            _Port.comtimeouts.Read();
            DiscardNull = _Port.dcb.DiscardNull;
            ParityReplace_Enable = _Port.dcb.ParityReplace_Enable;
            ParityReplace = _Port.dcb.ParityReplace_Byte;
            _ReadTimeout = _Port.comtimeouts.ReadTimeout;
            _WriteTimeout = _Port.comtimeouts.WriteTimeout;
            //TODO check ReceivedBytesThreshold
        }

        /// <summary> Writes settings to the port. </summary>
        public override void Write() {
            if (!portopen())
                throw new InvalidOperationException(SR.Port_not_open.ToResValue());
            _Port.dcb.DiscardNull = DiscardNull;
            _Port.dcb.ParityReplace_Enable = ParityReplace_Enable;
            _Port.dcb.ParityReplace_Byte = ParityReplace;
            _Port.comtimeouts.ReadTimeout = _ReadTimeout;
            _Port.comtimeouts.WriteTimeout = _WriteTimeout;
            //TODO check ReceivedBytesThreshold

            // DCB Register
            _Port.dcb.Write();
            // Com Timeouts Register
            _Port.comtimeouts.Write();
            // Buffer Sizes
            if (!portopen()) _Port.combufferstatus.SetupBuffer(ReadTimeout, WriteTimeout);
        }

        /// <summary> Clone. </summary>
        /// <returns> A copy of this object. </returns>
        public override ISerialBufferSettings Copy() {
            var ret = new SerialBufferSettings();
            ret.Import(this);
            return ret;
        }
    }
}