namespace Gbd.IO.Serial.Enums {
    /// <summary> Handshake mode for a serial port. </summary>
    public enum Handshake {
        None,
        XOnXOff,
        RequestToSend,
        RequestToSendXOnXOff
    }
}