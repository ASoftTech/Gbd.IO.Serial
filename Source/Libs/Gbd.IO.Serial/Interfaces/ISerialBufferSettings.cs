using System.Text;

namespace Gbd.IO.Serial.Interfaces
{
    public interface ISerialBufferSettings
    {
        /// <summary>
        ///     Gets or sets a value indicating whether null bytes are ignored when transmitted between
        ///     the port and the receive buffer.
        /// </summary>
        /// <value> true if discard null, false if not. </value>
        bool DiscardNull { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether we want to replace invalid bytes with a fixed value.
        /// </summary>
        /// <value> true if parity replace enable, false if not. </value>
        bool ParityReplace_Enable { get; set; }

        /// <summary>
        ///     Gets or sets the byte that replaces invalid bytes in a data stream when a parity error
        ///     occurs.
        /// </summary>
        /// <value> The parity replace byte. </value>
        byte ParityReplace { get; set; }

        /// <summary> Gets or sets the size of the SerialPort input buffer. </summary>
        /// <value> The size of the read buffer. </value>
        int ReadBufferSize { get; set; }

        /// <summary> Gets or sets the size of the serial port output buffer. </summary>
        /// <value> The size of the write buffer. </value>
        int WriteBufferSize { get; set; }

        /// <summary>
        ///     Gets or sets the number of milliseconds before a time-out occurs when a read operation
        ///     does not finish.
        /// </summary>
        /// <value> The read timeout. </value>
        int ReadTimeout { get; set; }

        /// <summary>
        ///     Gets or sets the number of milliseconds before a time-out occurs when a write operation
        ///     does not finish.
        /// </summary>
        /// <value> The write timeout. </value>
        int WriteTimeout { get; set; }

        /// <summary>
        ///     Gets or sets the value used to interpret the end of a call to the ReadLine and
        ///     WriteLine(System.String) methods.
        /// </summary>
        /// <value> The new line. </value>
        string NewLine { get; set; }

        /// <summary>
        ///     Gets or sets the byte encoding for pre- and post-transmission conversion of text.
        /// </summary>
        /// <value> The string encoding to use. </value>
        Encoding Encoding { get; set; }

        /// <summary>
        ///     Gets or sets the number of bytes in the internal input buffer before a
        ///     SerialPort.DataReceived event occurs.
        /// </summary>
        /// <value> The received bytes threshold. </value>
        int ReceivedBytesThreshold { get; set; }

        /// <summary> Import pin states into this class. </summary>
        /// <param name="importobj"> The states to import. </param>
        void Import(ISerialBufferSettings importobj);

        /// <summary> Setup Default Port Pin State Settings. </summary>
        void SetDefaults();

        /// <summary> Reads in the settings from the serial port. </summary>
        void Read();

        /// <summary> Writes the settings to the serial port. </summary>
        void Write();

        /// <summary> Copy. </summary>
        /// <returns> A copy of this object. </returns>
        ISerialBufferSettings Copy();
    }
}
