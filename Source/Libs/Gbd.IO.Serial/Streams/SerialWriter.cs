using System;
using System.IO;
using System.Text;
using Gbd.IO.Serial.Interfaces;

namespace Gbd.IO.Serial.Streams
{
    /// <summary> A Writer for outputting different forms to a serial port. </summary>
    public class SerialWriter : BinaryWriter {
        protected ISerialPort _SPort;

        protected Encoding _encoding => _SPort.BufferSettings.Encoding;

        protected string _newline => _SPort.BufferSettings.NewLine;

        private byte[] _largeByteBuffer;  // temp space for writing chars.
        private int _maxChars;   // max # of chars we can put in _largeByteBuffer
                                 // Size should be around the max number of chars/string * Encoding's max bytes/char
        private const int LargeByteBufferSize = 256;

        /// <summary> Default Constructor </summary>
        /// <param name="sport"> The serial port. </param>
        public SerialWriter(ISerialPort sport) : base(new SerialStream(sport), sport.BufferSettings.Encoding, true) {
            _SPort = sport;
        }

        /// <summary>
        ///     Writes a length-prefixed string to this stream in the BinaryWriter's current Encoding.
        /// </summary>
        /// <exception cref="ArgumentNullException"> Thrown when one or more required arguments are null. </exception>
        /// <param name="value"> The value to write. </param>
        public override void Write(string value) {
            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }

            int len = _encoding.GetByteCount(value);
            if (_largeByteBuffer == null) {
                _largeByteBuffer = new byte[LargeByteBufferSize];
                _maxChars = LargeByteBufferSize / _encoding.GetMaxByteCount(1);
            }

            if (len <= LargeByteBufferSize) {
                _encoding.GetBytes(value, 0, value.Length, _largeByteBuffer, 0);
                OutStream.Write(_largeByteBuffer, 0, len);
            }
            else {
                // Aggressively try to not allocate memory in this loop for
                // runtime performance reasons.  Use an Encoder to write out 
                // the string correctly (handling surrogates crossing buffer
                // boundaries properly).  
                var charStart = 0;
                var numLeft = value.Length;
                while (numLeft > 0) {
                    // Figure out how many chars to process this round.
                    var charCount = (numLeft > _maxChars) ? _maxChars : numLeft;
                    var byteLen = _encoding.GetEncoder().GetBytes(value.ToCharArray(), charStart, charCount, _largeByteBuffer, 0, charCount == numLeft);
                    OutStream.Write(_largeByteBuffer, 0, byteLen);
                    charStart += charCount;
                    numLeft -= charCount;
                }
            }
        }

        /// <summary> Writes a string of text to the serial port followed by a newline. </summary>
        /// <param name="str"> The string to write. </param>
        public void WriteLine(string str) {
            Write(str + _newline);
        }

    }
}
