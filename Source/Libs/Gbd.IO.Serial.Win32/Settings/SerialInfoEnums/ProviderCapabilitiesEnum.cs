using System;

namespace Gbd.IO.Serial.Win32.Settings.SerialInfoEnums {
    /// <summary> A bitmask indicating the capabilities offered by the provider. </summary>
    [Flags]
    public enum ProviderCapabilitiesEnum {
        /// <summary> Special 16-bit mode supported. </summary>
        Mode_16Bit = 0x0200,

        /// <summary> DTR (data-terminal-ready)/DSR (data-set-ready) supported. </summary>
        DTRDSR = 0x0001,

        /// <summary> Interval time-outs supported. </summary>
        Int_Timeouts = 0x0080,

        /// <summary> Parity checking supported. </summary>
        Parity_Check = 0x0008,

        /// <summary> RLSD (receive-line-signal-detect) supported. </summary>
        RLSD = 0x0004,

        /// <summary> RTS (request-to-send)/CTS (clear-to-send) supported. </summary>
        RTSCTS = 0x0002,

        /// <summary> Settable XON/XOFF supported. </summary>
        Set_XChar = 0x0020,

        /// <summary> Special character support provided. </summary>
        Special_Chars = 0x0100,

        /// <summary> The total (elapsed) time-outs supported. </summary>
        Total_Timeouts = 0x0040,

        /// <summary> XON/XOFF flow control supported. </summary>
        XOn_XOff = 0x0010,
    }
}