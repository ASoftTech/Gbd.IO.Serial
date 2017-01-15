namespace Gbd.IO.Serial.Interfaces {
    public interface ISerialProperties {
        /// <summary> Gets the maximum baud rate </summary>
        /// <value> The maximum bad rate. </value>
        double BaudRate_Max { get; }

        /// <summary> Reads in the properties from the serial port. </summary>
        void Read();
    }
}