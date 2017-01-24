using System.Collections.Generic;
using Gbd.IO.Serial.LinuxMono.Tests.Base;
using Gbd.IO.Serial.LinuxMono.Tests.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Gbd.IO.Serial.LinuxMono.Tests.Tests {
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
    }
}