using System;

namespace Gbd.IO.Serial.Interfaces {
    /// <summary> Interface for a serial port. </summary>
    public interface ISerialPort : IDisposable {
        /// <summary> Text string name of the serial port. </summary>
        string Name { get; }

        /// <summary> If the Serial Port is open. </summary>
        bool IsOpen { get; }

        /// <summary> Serial Port Settings. </summary>
        ISerialSettings SerialSettings { get; }

        /// <summary> Serial Port Pin States. </summary>
        ISerialPinStates PinStates { get; }

        /// <summary> Serial Port Buffer Settings. </summary>
        ISerialBufferSettings BufferSettings { get; }

        /// <summary> Serial Port properties. </summary>
        ISerialInfo SerialInfo { get; }

        /// <summary> Serial Port Uart. </summary>
        ISerialUart Uart { get; }

        /// <summary> Opens the serial port. </summary>
        void Open();

        /// <summary> Closes the serial port. </summary>
        void Close();

        /// <summary> Event queue for all listeners interested in PinChanged events. </summary>
        event EventHandler PinChanged;
    }
}