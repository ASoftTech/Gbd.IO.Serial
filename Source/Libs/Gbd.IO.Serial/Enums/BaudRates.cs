using System.ComponentModel;

namespace Gbd.IO.Serial.Enums {
    /// <summary> Common Baud Rate Values. </summary>
    public enum BaudRates {
        /// <summary> Invalid Baud Rate</summary>
        [EditorBrowsable(EditorBrowsableState.Never)] Unknown = -1,
        B75 = 75,
        B110 = 110,
        B134 = 134,
        B150 = 150,
        B300 = 300,
        B600 = 600,
        B1200 = 1200,
        B1800 = 1800,
        B2400 = 2400,
        B4800 = 4800,
        B7200 = 7200,
        B9600 = 9600,
        B14400 = 14400,
        B19200 = 19200,
        B38400 = 38400,
        B57600 = 57600,
        B115200 = 115200,
        B128000 = 128000,
        B230400 = 230400,
        B256000 = 256000,
        B460800 = 460800,
        B512000 = 512000
    }
}