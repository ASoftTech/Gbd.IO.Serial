using System;
using System.Linq;
using Gbd.IO.Serial.Interfaces;
using Gbd.IO.Serial.Win32.Settings;
using Gbd.IO.Serial.Win32.Tests.Base;
using Gbd.IO.Serial.Win32.Tests.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Gbd.IO.Serial.Win32.Tests.Tests.Settings {
    public class SerialPinStatesTests : BaseTest {
        /// <summary> Constructor. </summary>
        /// <param name="outputHelper"> The output helper used by XUnit. </param>
        public SerialPinStatesTests(ITestOutputHelper outputHelper) : base(outputHelper) {}

        /// <summary> Read the Pin States from the port </summary>
        [Fact]
        public void Read() {
            // Get the Serial Port Controller based on the platform
            var controller = Platform.GetController();
            // Get the first serial port name in the list
            var port = controller.GetPorts().FirstOrDefault();
            if (port == null) throw new ArgumentException("No Serial Device Found");
            Assert.NotNull(port.PinStates);

            port.Open();
            port.PinStates.Read();
            var pinstates = "Pin Statest for port" + port.Name + " \n";
            pinstates += "BreakState: " + port.PinStates.BreakState + "\n";
            pinstates += "CD Detect: " + port.PinStates.CD_Detect + "\n";
            pinstates += "CTS Detect: " + port.PinStates.CTS_Detect + "\n";
            pinstates += "DSR Detect: " + port.PinStates.DSR_Detect + "\n";
            pinstates += "Ring Detect: " + port.PinStates.Ring_Detect + "\n";
            pinstates += "Dtr Enable: " + port.PinStates.Dtr_Enable + "\n";
            pinstates += "Rts Enable: " + port.PinStates.Rts_Enable + "\n";
            Logger.Info(pinstates);
            port.Close();
        }

        /// <summary> Writes pin states </summary>
        [Fact]
        public void Write() {
            // Assume we have a Null Model cable with two serial ports connected

            // Get the Serial Port Controller based on the platform
            var controller = Platform.GetController();
            var portlist = controller.GetPorts();
            if (portlist.Count < 2) {
                throw new ArgumentException("At least 2 serial port needed in null modem ");
            }
            var sport1 = portlist[0];
            var sport2 = portlist[1];
            if (sport1 == null || sport2 == null)
                throw new ArgumentException("No Serial Devices Found");

            sport1.Open();
            sport2.Open();

            // Try Setting RTS and DTR over the null modem
            test_portpair(sport1, sport2, false);
            test_portpair(sport1, sport2, true);
            test_portpair(sport2, sport1, false);
            test_portpair(sport2, sport1, true);
            sport1.Close();
            sport2.Close();
        }

        private void test_portpair(ISerialPort sport1, ISerialPort sport2, bool testval) {
            sport1.PinStates.Read();
            sport1.PinStates.Rts_Enable = testval;
            sport1.PinStates.Dtr_Enable = testval;
            sport1.PinStates.Write();
            sport2.PinStates.Read();
            if (testval) {
                Assert.True(sport2.PinStates.CTS_Detect);
                Assert.True(sport2.PinStates.DSR_Detect);
            }
            else {
                Assert.False(sport2.PinStates.CTS_Detect);
                Assert.False(sport2.PinStates.DSR_Detect);
            }
        }

        /// <summary> Sets pin states to default values. </summary>
        [Fact]
        public void SetDefaults() {
            // Get the Serial Port Controller based on the platform
            var controller = Platform.GetController();
            // Get the first serial port name in the list
            var port = controller.GetPorts().FirstOrDefault();
            if (port == null) throw new ArgumentException("No Serial Device Found");
            Assert.NotNull(port.PinStates);
            port.Open();
            port.PinStates.Read();
            port.PinStates.SetDefaults();
            port.PinStates.Write();
            port.Close();
        }

        /// <summary> Import states from a copy. </summary>
        public void Import() {
            // Get the Serial Port Controller based on the platform
            var controller = Platform.GetController();
            // Get the first serial port name in the list
            var port = controller.GetPorts().FirstOrDefault();
            if (port == null) throw new ArgumentException("No Serial Device Found");
            Assert.NotNull(port.PinStates);
            port.Open();
            port.PinStates.Read();
            var newstates = new SerialPinStates {Rts_Enable = true};
            port.PinStates.Import(newstates);
            port.PinStates.Write();
            port.Close();
        }

        /// <summary> Create copy of pinstates. </summary>
        public void Copy() {
            // Get the Serial Port Controller based on the platform
            var controller = Platform.GetController();
            // Get the first serial port name in the list
            var port = controller.GetPorts().FirstOrDefault();
            if (port == null) throw new ArgumentException("No Serial Device Found");
            Assert.NotNull(port.PinStates);
            port.Open();
            port.PinStates.Read();
            var copyitem = port.PinStates.Copy();
            Assert.IsType<SerialPinStates>(copyitem);
            port.Close();
        }
    }
}