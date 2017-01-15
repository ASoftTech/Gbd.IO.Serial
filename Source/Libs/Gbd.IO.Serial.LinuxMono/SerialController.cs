using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gbd.IO.Serial.Interfaces;

namespace Gbd.IO.Serial.LinuxMono {
    /// <summary> Serial Controller for Linux / Mono Serial Port access. </summary>
    public class SerialController {
        /// <summary>
        ///     Constructor that prevents a default instance of this class from being created.
        /// </summary>
        private SerialController() {}

        protected static SerialController _instance;

        /// <summary> Static Constructor. </summary>
        /// <returns> Single instance. </returns>
        public static SerialController GetDefault() {
            return _instance ?? (_instance = new SerialController());
        }

        /// <summary> Gets a list of all available serial port names this controller provides. </summary>
        /// <returns> list of all available serial port names this controller provides. </returns>
        public List<string> GetPortNames() {
            var portNames = new List<string>();
            var ttys = Directory.GetFiles("/dev/", "tty*");

            // Probe for Unix-styled devices: /dev/ttyS* or /dev/ttyUSB* etc
            foreach (var dev in ttys) {
                if (dev.StartsWith("/dev/ttyS") || dev.StartsWith("/dev/ttyUSB") || dev.StartsWith("/dev/ttyACM")) {
                    portNames.Add(dev);
                }
                else if (dev != "/dev/tty" && dev.StartsWith("/dev/tty") && !dev.StartsWith("/dev/ttyC"))
                    portNames.Add(dev);
            }
            return portNames;
        }

        /// <summary> Gets a specific serial port based on the name. </summary>
        /// <param name="portname"> The serial port name. </param>
        /// <returns> The serial port. </returns>
        public ISerialPort GetPort(string portname) {
            return new SerialPort(portname);
        }

        /// <summary> Gets all serial ports this controller can provide as class instances. </summary>
        /// <returns> A List of all serial ports for this controller. </returns>
        public List<ISerialPort> GetPorts() {
            var portnames = GetPortNames();
            return portnames.Select(portname => new SerialPort(portname, true)).Cast<ISerialPort>().ToList();
        }
    }
}