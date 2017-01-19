using System.Collections.Generic;
using Gbd.IO.Serial.Interfaces;

namespace Gbd.IO.Serial.LinuxMono.Settings {
    /// <summary> Information about the serial port. </summary>
    public class SerialInfo : ISerialInfo {
        /// <summary> Serial Port properties. </summary>
        public Dictionary<string, string> Props => _Props;

        protected Dictionary<string, string> _Props;

        /// <summary> The associated serial port. </summary>
        public SerialPort Port => _Port;

        protected SerialPort _Port;

        /// <summary> Default constructor. </summary>
        public SerialInfo() {}

        /// <summary> Constructor. </summary>
        /// <param name="sport"> The serial port to associate with. </param>
        internal SerialInfo(SerialPort sport) {
            _Port = sport;
            _Props = new Dictionary<string, string>();
        }

        /// <summary> Reads the actual values from the port. </summary>
        public void Read() {
            // Currently the mono C library doesn't support gathering info from the serial port
            _Props.Clear();
        }
    }
}