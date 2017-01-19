namespace Gbd.IO.Serial.Win32.Settings.SerialInfoEnums {
    /// <summary> Provider Subtype for a serial port. </summary>
    public enum ProviderSubTypeEnum {
        /// <summary> FAX device. </summary>
        Fax = 0x00000021,

        /// <summary> LAT protocol. </summary>
        Lat = 0x00000101,

        /// <summary> Modem device. </summary>
        Modem = 0x00000006,

        /// <summary> Unspecified network bridge. </summary>
        Network_Bridge = 0x00000100,

        /// <summary> Parallel port. </summary>
        Parallel_Port = 0x00000002,

        /// <summary> RS-232 serial port. </summary>
        RS232 = 0x00000001,

        /// <summary> RS-422 port. </summary>
        RS422 = 0x00000003,

        /// <summary> RS-423 port. </summary>
        RS423 = 0x00000004,

        /// <summary> RS-449 port. </summary>
        RS449 = 0x00000005,

        /// <summary> Scanner device. </summary>
        Scanner = 0x00000022,

        /// <summary> TCP/IP Telnet protocol. </summary>
        TCPIP_Telnet = 0x00000102,

        /// <summary> Unspecified. </summary>
        Unspecified = 0x00000000,

        /// <summary> X.25 standards. </summary>
        X25 = 0x00000103,
    }
}