using Gbd.IO.Serial.Interfaces;

namespace Gbd.IO.Serial.Streams {
    /// <summary> Extension methods for ISerialPort. </summary>
    public static class StreamExtension {
        /// <summary> Create a Serial Stream from a ISerialPort. </summary>
        /// <param name="value"> The serial port to act on. </param>
        /// <returns> A SerialStream object. </returns>
        public static SerialStream ToStream(this ISerialPort value) {
            var ret = new SerialStream(value);
            return ret;
        }

        /// <summary> Create a SerialWriter from a ISerialPort. </summary>
        /// <param name="value"> The serial port to act on. </param>
        /// <returns> A SerialWriter object. </returns>
        public static SerialWriter ToSerialWriter(this ISerialPort value) {
            var ret = new SerialWriter(value);
            return ret;
        }

        /// <summary> Create a SerialReader from a ISerialPort. </summary>
        /// <param name="value"> The serial port to act on. </param>
        /// <returns> A SerialReader object. </returns>
        public static SerialReader ToSerialReader(this ISerialPort value) {
            var ret = new SerialReader(value);
            return ret;
        }
    }
}