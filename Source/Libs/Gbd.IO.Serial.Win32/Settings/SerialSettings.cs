using Gbd.IO.Serial.Base;
using Gbd.IO.Serial.Error;
using Gbd.IO.Serial.Interfaces;
using System;

namespace Gbd.IO.Serial.Win32.Settings {
    /// <summary> Serial Port Settings. </summary>
    public class SerialSettings : SerialSettingsBase {
        /// <summary> The associated serial port. </summary>
        public SerialPort Port => _Port;

        protected SerialPort _Port;

        /// <summary> Default constructor. </summary>
        public SerialSettings() {}

        /// <summary> Constructor. </summary>
        /// <param name="sport"> The serial port to associate with. </param>
        internal SerialSettings(SerialPort sport) {
            _Port = sport;
        }

        /// <summary> Speed or baud_rate of the Serial Port, Expressed as an Integer Value. </summary>
        /// <value> The baud rate as an integer. </value>
        public override uint BaudRate_Int {
            get { return _BaudRate_Int; }
            set {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException(SR.ArgumentOutOfRange_NeedPosNum.ToResValue());
                if (value > _Port.comprops.MaxBaudRate && _Port.comprops.MaxBaudRate > 0)
                    // if no upper bound on baud rate imposed by serial driver, note that argument must be positive
                    if (_Port.comprops.MaxBaudRate <= 0)
                        throw new ArgumentOutOfRangeException(SR.ArgumentOutOfRange_NeedPosNum.ToResValue());
                    else
                        throw new ArgumentOutOfRangeException(
                            SR.ArgumentOutOfRange_Bounds_Lower_Upper.ToResValue(0, _Port.comprops.MaxBaudRate));
                _BaudRate_Int = value;
            }
        }

        protected uint _BaudRate_Int;

        private bool portopen() {
            if (_Port == null) return false;
            return _Port.IsOpen;
        }

        /// <summary> Reads the actual values from the port. </summary>
        public override void Read() {
            if (!portopen())
                throw new InvalidOperationException(SR.Port_not_open.ToResValue());
            _Port.dcb.Read();
            DataBits = _Port.dcb.ByteSize;
            Parity = _Port.dcb.Parity;
            StopBits = _Port.dcb.StopBits;
            Handshake = _Port.dcb.Handshake;
            _BaudRate_Int = _Port.dcb.BaudRate;
        }

        /// <summary> Writes cached values to the port. </summary>
        public override void Write() {
            if (!portopen())
                throw new InvalidOperationException(SR.Port_not_open.ToResValue());
            _Port.dcb.ByteSize = DataBits;
            _Port.dcb.Parity = Parity;
            _Port.dcb.StopBits = StopBits;
            _Port.dcb.Handshake = Handshake;
            _Port.dcb.BaudRate = _BaudRate_Int;
            _Port.dcb.Write();
        }

        /// <summary> Copy. </summary>
        /// <returns> A copy of this object. </returns>
        public override ISerialSettings Copy() {
            var ret = new SerialSettings();
            ret.Import(this);
            return ret;
        }
    }
}