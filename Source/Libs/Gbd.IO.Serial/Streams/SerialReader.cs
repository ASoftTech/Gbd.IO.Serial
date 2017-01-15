using System.Collections.Generic;
using System.IO;
using System.Text;
using Gbd.IO.Serial.Interfaces;

namespace Gbd.IO.Serial.Streams {
    public class SerialReader : BinaryReader {
        protected ISerialPort _SPort;

        protected Encoding _Encoding => _SPort.BufferSettings.Encoding;

        protected string _NewLine => _SPort.BufferSettings.NewLine;

        /// <summary> Default Constructor. </summary>
        /// <param name="sport"> The serial port. </param>
        public SerialReader(ISerialPort sport) : base(new SerialStream(sport), sport.BufferSettings.Encoding, true) {
            _SPort = sport;
        }

        /// <summary> Reads a string from the current stream. </summary>
        /// <returns> The string being read. </returns>
        public override string ReadString() {
            int count = _SPort.Uart.BytesToRead;
            byte[] bytes = new byte[count];
            int n = Read(bytes, 0, count);
            var ret = new string(_Encoding.GetChars(bytes, 0, n));
            return ret;
        }

        /// <summary> Reads to a certain string sequence. </summary>
        /// <param name="value"> The value to read to. </param>
        /// <returns> The string read. </returns>
        public string ReadTo(string value) {
            // Turn into byte array, so we can compare
            byte[] byte_value = _Encoding.GetBytes(value);
            int current = 0;
            List<byte> seen = new List<byte>();

            while (true) {
                int n = ReadByte();
                if (n == -1)
                    break;
                seen.Add((byte) n);
                if (n == byte_value[current]) {
                    current++;
                    if (current == byte_value.Length)
                        return _Encoding.GetString(seen.ToArray(), 0, seen.Count - byte_value.Length);
                }
                else current = (byte_value[0] == n) ? 1 : 0;
            }
            return _Encoding.GetString(seen.ToArray(), 0, seen.Count);
        }

        /// <summary> Reads a line of text marked as ending with NewLine. </summary>
        /// <returns> The line of text. </returns>
        public string ReadLine() {
            return ReadTo(_NewLine);
        }
    }
}