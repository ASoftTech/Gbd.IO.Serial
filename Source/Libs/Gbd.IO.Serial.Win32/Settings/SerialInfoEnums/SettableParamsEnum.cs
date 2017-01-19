using System;

namespace Gbd.IO.Serial.Win32.Settings.SerialInfoEnums {
    /// <summary> A bitmask indicating the communications parameters that can be changed. </summary>
    [Flags]
    public enum SettableParamsEnum {
        /// <summary> Baud rate. </summary>
        Baud = 0x0002,

        /// <summary> Data bits. </summary>
        DataBits = 0x0004,

        /// <summary> Handshaking (flow control). </summary>
        HandShaking = 0x0010,

        /// <summary> Parity. </summary>
        Parity = 0x0001,

        /// <summary> Parity checking. </summary>
        Parity_Check = 0x0020,

        /// <summary> RLSD (receive-line-signal-detect). </summary>
        RLSD = 0x0040,

        /// <summary> Stop bits. </summary>
        StopBits = 0x0008,
    }
}