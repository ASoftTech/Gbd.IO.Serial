using Gbd.IO.Serial.Enums;

namespace Gbd.IO.Serial.Interfaces {
    /// <summary> Interface for serial settings. </summary>
    public interface ISerialSettings {
        /// <summary> Gets or sets the number of data bits associated with the port. </summary>
        /// <value> The number of data bits. </value>
        DataBits DataBits { get; set; }

        /// <summary> Gets or sets the serial port parity. </summary>
        /// <value> The serial port parity. </value>
        Parity Parity { get; set; }

        /// <summary> Gets or sets the serial port stop bits. </summary>
        /// <value> The number of stop bits used on the serial port. </value>
        StopBits StopBits { get; set; }

        /// <summary> Gets or sets the handshake mode for the serial port. </summary>
        /// <value> The handshake mode. </value>
        Handshake Handshake { get; set; }

        /// <summary> Speed or BaudRate of the Serial Port, Expressed as an Integer Value. </summary>
        /// <value> The baud rate as an integer. </value>
        uint BaudRate_Int { get; set; }

        /// <summary> Speed or BaudRate of the Serial Port, Expressed as an Enum Value. </summary>
        /// <value> The baud rate as an enum. </value>
        BaudRates BaudRate { get; set; }

        /// <summary> Copy / Import Port Settings into underlying Serial Port Object. </summary>
        /// <param name="impsetts"> The settings to import / copy from. </param>
        void Import(ISerialSettings impsetts);

        /// <summary> Setup Default Port Settings. </summary>
        void SetDefaults();

        /// <summary> Reads in the settings from the serial port. </summary>
        void Read();

        /// <summary> Writes the settings to the serial port. </summary>
        void Write();

        /// <summary> Copy. </summary>
        /// <returns> A copy of this object. </returns>
        ISerialSettings Copy();
    }
}