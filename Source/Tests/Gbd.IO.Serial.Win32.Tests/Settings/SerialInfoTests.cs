using System;
using System.Linq;
using Xunit;

namespace Gbd.IO.Serial.Win32.Tests.Settings {
    public class SerialInfoTests {
        [Fact]
        public void Read() {
            // Get the Serial Port Controller based on the platform
            var controller = Platform.GetController();
            // Get the first serial port in the list
            var sport1 = controller.GetPorts().FirstOrDefault();
            if (sport1 == null) throw new ArgumentException("No Serial Device Found");

            // Write all the values to the output
            sport1.Open();
            sport1.SerialInfo.Read();
            if (sport1.SerialInfo != null) {
                foreach (var item in sport1.SerialInfo.Props) {
                    Console.WriteLine(item.Key + " : " + item.Value);
                }
            }
            sport1.Close();
        }
    }
}