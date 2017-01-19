using System;

namespace Gbd.IO.Serial.Win32.Settings.SerialInfoEnums {
    /// <summary> A bitmask indicating the stop bit and parity settings that can be selected. </summary>
    [Flags]
    public enum SettableStopParityEnum {
        /// <summary> 1 stop bit. </summary>
        StopBits_1 = 0x0001,

        /// <summary> 1.5 stop bits. </summary>
        StopBits_15 = 0x0002,

        /// <summary> 2 stop bits. </summary>
        StopBits_2 = 0x0004,

        /// <summary> No parity. </summary>
        Parity_None = 0x0100,

        /// <summary> Odd Parity. </summary>
        Parity_Odd = 0x0200,

        /// <summary> Even Parity. </summary>
        Parity_Even = 0x0400,

        /// <summary> Mark Parity. </summary>
        Parity_Mark = 0x0800,

        /// <summary> Space Parity. </summary>
        Parity_Space = 0x1000,
    }
}