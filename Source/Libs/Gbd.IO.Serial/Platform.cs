using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Gbd.IO.Serial.Interfaces;

namespace Gbd.IO.Serial {
    public class Platform {

        /// <summary> Gets the default controller for a given platform. </summary>
        /// <returns> The serial port controller. </returns>
        public static ISerialController GetController() {
            ISerialController ret = null;
            var IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            if (IsWindows) {
                // Get the Windows Controller
                var wincontroller = Type.GetType("Gbd.IO.Serial.Win32.SerialController, Gbd.IO.Serial.Win32");
                if (wincontroller != null)
                    ret = (ISerialController) wincontroller.GetMethod("GetDefault").Invoke(null, null);
            }
            else {
                // Get the Linux Controller
                var wincontroller = Type.GetType("Gbd.IO.Serial.LinuxMono.SerialController, Gbd.IO.Serial.LinuxMono");
                if (wincontroller != null)
                    ret = (ISerialController)wincontroller.GetMethod("GetDefault").Invoke(null, null);
            }
            return ret;
        }
    }
}