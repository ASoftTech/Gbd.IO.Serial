using System;
using System.Collections.Generic;
using System.Linq;
using Gbd.IO.Serial.Interfaces;
using Gbd.IO.Serial.Win32.Tests.Base;
using Gbd.IO.Serial.Win32.Tests.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Gbd.IO.Serial.Win32.Tests.Tests {
    /// <summary> Tests for the Serial Port Controller. </summary>
    public class SerialControllerTests : BaseTest {
        /// <summary> Constructor. </summary>
        /// <param name="outputHelper"> The output helper used by XUnit. </param>
        public SerialControllerTests(ITestOutputHelper outputHelper) : base(outputHelper) {}

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
            foreach (var item in portnames) {
                Logger.InfoFormat("Serial Port Found: {SerialPortName}", item);
            }
            if (portnames.Count == 0) {
                Logger.Info("No Serial Ports found");
            }
        }

        /// <summary> Check we can get a serial port based on name. </summary>
        [Fact]
        public void GetPort() {
            // Get the Serial Port Controller based on the platform
            var controller = Platform.GetController();
            // Get the first serial port name in the list
            var portname = controller.GetPortNames().FirstOrDefault();
            if (portname == null) throw new ArgumentException("No Serial Device Found");

            // Get the Serial port
            var sport1 = controller.GetPort(portname);
            Assert.NotNull(sport1);
            Assert.IsType<SerialPort>(sport1);
        }

        /// <summary> Check we can get a serial port list. </summary>
        [Fact]
        public void GetPorts() {
            // Get the Serial Port Controller based on the platform
            var controller = Platform.GetController();
            // Get list of serial ports
            var sports = controller.GetPorts();
            Assert.NotNull(sports);
            Assert.IsType<List<ISerialPort>>(sports);
        }
    }
}