using Gbd.IO.Serial.Error;
using Gbd.IO.Serial.Interfaces;
using System;

//TODO Add more based on ComProps structure

namespace Gbd.IO.Serial.Win32.Settings {
    public class SerialProperties : ISerialProperties {
        /// <summary> The associated serial port. </summary>
        public SerialPort Port => _Port;

        protected SerialPort _Port;

        /// <summary> Gets the maximum baud rate. </summary>
        /// <value> The maximum bad rate. </value>
        public double BaudRate_Max => _BaudRate_Max;

        protected double _BaudRate_Max;

        /// <summary> Default constructor. </summary>
        public SerialProperties() {}

        /// <summary> Constructor. </summary>
        /// <param name="sport"> The serial port to associate with. </param>
        internal SerialProperties(SerialPort sport) {
            _Port = sport;
        }

        private bool portopen() {
            if (_Port == null) return false;
            return _Port.IsOpen;
        }

        /// <summary> Reads the actual values from the port. </summary>
        public void Read() {
            if (!portopen())
                throw new InvalidOperationException(SR.Port_not_open.ToResValue());
            _Port.comprops.Read();
            _BaudRate_Max = _Port.comprops.MaxBaudRate;
        }
    }
}