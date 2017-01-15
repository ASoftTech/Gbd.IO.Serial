namespace Gbd.IO.Serial.Enums {
    /// <summary> Values that represent serial errors. </summary>
    public enum SerialError {
        RXOver,
        Overrun,
        RXParity,
        Frame,
        TXFull,
    }
}