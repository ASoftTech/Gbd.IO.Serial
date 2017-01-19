using System.Collections.Generic;

namespace Gbd.IO.Serial.Interfaces {
    public interface ISerialInfo {
        /// <summary>
        /// Serial Port properties, such as max baud rate etc.
        /// </summary>
        Dictionary<string, string> Props { get; }

        /// <summary> Reads in the properties from the serial port. </summary>
        void Read();

    }
}