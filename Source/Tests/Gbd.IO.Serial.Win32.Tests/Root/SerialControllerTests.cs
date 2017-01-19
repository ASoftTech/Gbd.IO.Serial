using System;
using System.Collections.Generic;
using Xunit;

namespace Gbd.IO.Serial.Win32.Tests.Root {
    /// <summary> Tests for the Serial Port Controller. </summary>
    public class SerialControllerTests {
        /// <summary> Gets the default instance of the Serial Controller </summary>
        [Fact]
        public void GetDefault() {
            var controller = SerialController.GetDefault();
            Assert.NotNull(controller);
            Assert.IsType<SerialController>(controller);
        }

        /// <summary> Check we can get a list of port names. </summary>
        [Fact]
        public void GetPortNames() {
            var controller = Platform.GetController();
            var portnames = controller.GetPortNames();
            Assert.NotNull(portnames);
            Assert.IsType<List<string>>(portnames);
            foreach (string item in portnames) {
                Console.WriteLine("Port Found: " + item);
            }
            if (portnames.Count == 0) {
                Console.WriteLine("No ports found");
            }
        }
    }
}