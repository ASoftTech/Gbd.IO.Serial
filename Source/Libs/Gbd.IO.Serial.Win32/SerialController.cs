using Gbd.IO.Serial.Interfaces;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Permissions;

namespace Gbd.IO.Serial.Win32 {
    /// <summary> Serial Port controller for win32. </summary>
    public class SerialController : ISerialController {

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
            RegistryKey baseKey = null;
            RegistryKey serialKey = null;
            var portNames = new List<string>();
            var regperm = new RegistryPermission(
                RegistryPermissionAccess.Read,
                @"HKEY_LOCAL_MACHINE\HARDWARE\DEVICEMAP\SERIALCOMM");
            regperm.Assert();
            try {
                baseKey = Registry.LocalMachine;
                serialKey = baseKey.OpenSubKey(@"HARDWARE\DEVICEMAP\SERIALCOMM", false);
                if (serialKey != null) {
                    var deviceNames = serialKey.GetValueNames();
                    portNames.AddRange(deviceNames.Select(devname => (string) serialKey.GetValue(devname)));
                }
            }
            finally {
                baseKey?.Dispose();
                serialKey?.Dispose();
                CodeAccessPermission.RevertAssert();
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