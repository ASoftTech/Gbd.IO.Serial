using System;
using System.Linq;
using Gbd.IO.Serial;

namespace ExampleCoreApp1.Examples.Settings {
    public class SerialInfo {
        public static void Run() {
            // Get the Serial Port Controller based on the platform
            var controller = Platform.GetController();

            // Get the first serial port in the list
            var sport1 = controller.GetPorts().FirstOrDefault();

            // Read in the serial port properties if supported
            if (sport1 != null) {
                sport1.Open();
                sport1.SerialInfo.Read();

                // Write all the values to the Console
                if (sport1.SerialInfo != null) {
                    foreach (var item in sport1.SerialInfo.Props) {
                        Console.WriteLine(item.Key + " : " + item.Value);
                    }
                }
                sport1.Close();
            }
        }
    }
}