using System.Collections.Generic;

namespace Gbd.IO.Serial.Interfaces {
    /// <summary> Top level interface for sourcing serial ports from the system or devices. </summary>
    public interface ISerialController {
        /// <summary> Gets a list of all available serial ports this controller provides. </summary>
        /// <returns> list of all available serial ports this controller provides. </returns>
        List<string> GetPortNames();

        /// <summary> Gets a specific serial port based on the name. </summary>
        /// <param name="portname"> The serial port name. </param>
        /// <returns> The serial port. </returns>
        ISerialPort GetPort(string portname);

        /// <summary> Gets all serial ports this controller can provide as class instances. </summary>
        /// <returns> A List of all serial ports for this controller. </returns>
        List<ISerialPort> GetPorts();
    }
}