using System;

namespace Gbd.IO.Serial.Win32.Settings.SerialInfoEnums {
    /// <summary> A bitmask indicating the number of data bits that can be set. </summary>
    [Flags]
    public enum SettableDataEnum {
        /// <summary> 5 data bits. </summary>
        DataBits_5 = 0x0001,

        /// <summary> 6 data bits. </summary>
        DataBits_6 = 0x0002,

        /// <summary> 7 data bits. </summary>
        DataBits_7 = 0x0004,

        /// <summary> 8 data bits. </summary>
        DataBits_8 = 0x0008,

        /// <summary> 16 data bits. </summary>
        DataBits_16 = 0x0010,

        /// <summary> Special wide path through serial hardware lines. </summary>
        DataBits_16X = 0x0020,
    }
}