using Gbd.IO.Serial.Interfaces;
using System.Text;

namespace Gbd.IO.Serial.Base {
    /// <summary> Base class for buffer related settings. </summary>
    public class SerialBufferSettingsBase : ISerialBufferSettings {
        /// <summary> The infinite timeout. </summary>
        public const int InfiniteTimeout = -1;

        /// <summary>
        ///     Gets or sets a value indicating whether null bytes are ignored when transmitted between
        ///     the port and the receive buffer.
        /// </summary>
        /// <value> true if discard null, false if not. </value>
        public virtual bool DiscardNull { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether we want to replace invalid bytes with a fixed
        ///     value.
        /// </summary>
        /// <value> true if parity replace enable, false if not. </value>
        public virtual bool ParityReplace_Enable { get; set; }

        /// <summary>
        ///     Gets or sets the byte that replaces invalid bytes in a data stream when a parity error
        ///     occurs.
        /// </summary>
        /// <value> The parity replace byte. </value>
        public virtual byte ParityReplace { get; set; }

        /// <summary> Gets or sets the size of the SerialPort input buffer. </summary>
        /// <value> The size of the read buffer. </value>
        public virtual int ReadBufferSize { get; set; }

        /// <summary> Gets or sets the size of the serial port output buffer. </summary>
        /// <value> The size of the write buffer. </value>
        public virtual int WriteBufferSize { get; set; }

        /// <summary>
        ///     Gets or sets the number of milliseconds before a time-out occurs when a read operation
        ///     does not finish.
        /// </summary>
        /// <value> The read timeout. </value>
        public virtual int ReadTimeout { get; set; }

        /// <summary>
        ///     Gets or sets the number of milliseconds before a time-out occurs when a write operation
        ///     does not finish.
        /// </summary>
        /// <value> The write timeout. </value>
        public virtual int WriteTimeout { get; set; }

        /// <summary>
        ///     Gets or sets the value used to interpret the end of a call to the ReadLine and
        ///     WriteLine(System.String) methods.
        /// </summary>
        /// <value> The new line. </value>
        public virtual string NewLine { get; set; }

        /// <summary>
        ///     Gets or sets the byte encoding for pre- and post-transmission conversion of text.
        /// </summary>
        /// <value> The string encoding to use. </value>
        public virtual Encoding Encoding { get; set; }

        /// <summary>
        ///     Gets or sets the number of bytes in the internal input buffer before a
        ///     SerialPort.DataReceived event occurs.
        /// </summary>
        /// <value> The received bytes threshold. </value>
        public virtual int ReceivedBytesThreshold { get; set; }

        /// <summary> Reads in the settings from the serial port. </summary>
        public virtual void Read() {
            throw new System.NotImplementedException();
        }

        /// <summary> Writes the settings to the serial port. </summary>
        public virtual void Write() {
            throw new System.NotImplementedException();
        }

        /// <summary> Setup Default Port Pin State Settings. </summary>
        public virtual void SetDefaults() {
            DiscardNull = false;
            ParityReplace_Enable = false;
            ParityReplace = (byte)'\0';
            ReadBufferSize = 4096;
            WriteBufferSize = 2048;
            ReadTimeout = InfiniteTimeout;
            WriteTimeout = InfiniteTimeout;
            NewLine = "\n";
            Encoding = Encoding.GetEncoding("us-ascii");
            ReceivedBytesThreshold = 1;
        }

        /// <summary> Import pin states into this class. </summary>
        /// <param name="importobj"> The states to import. </param>
        public virtual void Import(ISerialBufferSettings importobj) {
            DiscardNull = importobj.DiscardNull;
            ParityReplace_Enable = importobj.ParityReplace_Enable;
            ParityReplace = importobj.ParityReplace;
            ReadBufferSize = importobj.ReadBufferSize;
            WriteBufferSize = importobj.WriteBufferSize;
            ReadTimeout = importobj.ReadTimeout;
            WriteTimeout = importobj.WriteTimeout;
            NewLine = importobj.NewLine;
            Encoding = importobj.Encoding;
            ReceivedBytesThreshold = importobj.ReceivedBytesThreshold;
        }

        /// <summary> Copy. </summary>
        /// <returns> A copy of this object. </returns>
        public virtual ISerialBufferSettings Copy() {
            SerialBufferSettingsBase ret = new SerialBufferSettingsBase();
            ret.Import(this);
            return ret;
        }
    }
}