using System;
using System.Text;
using Gbd.IO.Serial.Base;
using Gbd.IO.Serial.Error;
using Gbd.IO.Serial.Interfaces;

namespace Gbd.IO.Serial.LinuxMono.Settings {
    /// <summary> Settings for the Serial Buffer. </summary>
    public class SerialBufferSettings : SerialBufferSettingsBase {
        /// <summary> The associated serial port. </summary>
        public SerialPort Port => _Port;

        protected SerialPort _Port;

        /// <summary>
        ///     Gets or sets a value indicating whether null bytes are ignored when transmitted between
        ///     the port and the receive buffer.
        /// </summary>
        /// <value> true if discard null, false if not. </value>
        public override bool DiscardNull {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        ///     Gets or sets the byte that replaces invalid bytes in a data stream when a parity error
        ///     occurs.
        /// </summary>
        /// <value> The parity replace byte. </value>
        public override byte ParityReplace {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }


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
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        /// <summary> Gets or sets the size of the serial port output buffer. </summary>
        /// <value> The size of the write buffer. </value>
        public override int WriteBufferSize {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

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
            throw new NotImplementedException();
        }

        /// <summary> Writes settings to the port. </summary>
        public override void Write() {
            if (!portopen())
                throw new InvalidOperationException(SR.Port_not_open.ToResValue());
            throw new NotImplementedException();
        }

        /// <summary> Setup Default Port Pin State Settings. </summary>
        public override void SetDefaults() {
            ReadTimeout = InfiniteTimeout;
            WriteTimeout = InfiniteTimeout;
            NewLine = "\n";
            Encoding = Encoding.GetEncoding("us-ascii");
            ReceivedBytesThreshold = 1;
        }

        /// <summary> Import pin states into this class. </summary>
        /// <param name="importobj"> The states to import. </param>
        public override void Import(ISerialBufferSettings importobj) {
            ReadTimeout = importobj.ReadTimeout;
            WriteTimeout = importobj.WriteTimeout;
            NewLine = importobj.NewLine;
            Encoding = importobj.Encoding;
            ReceivedBytesThreshold = importobj.ReceivedBytesThreshold;
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