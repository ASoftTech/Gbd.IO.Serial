using System;
using System.Linq;
using Gbd.IO.Serial.Win32.Tests.Base;
using Gbd.IO.Serial.Win32.Tests.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Gbd.IO.Serial.Win32.Tests.Tests.Settings {
    public class SerialInfoTests : BaseTest {
        /// <summary> Constructor. </summary>
        /// <param name="outputHelper"> The output helper used by XUnit. </param>
        public SerialInfoTests(ITestOutputHelper outputHelper) : base(outputHelper) {}

        /// <summary> Reads meta information about the serial port. </summary>
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
                var props = "Serial Properties: " + sport1.Name + "\n";
                foreach (var item in sport1.SerialInfo.Props) {
                    props += item.Key + " : " + item.Value + "\n";
                }
                Logger.Info(props);
            }
            sport1.Close();
        }
    }
}