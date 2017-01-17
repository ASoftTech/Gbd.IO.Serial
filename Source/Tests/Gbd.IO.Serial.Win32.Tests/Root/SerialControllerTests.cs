using System;
using System.Collections.Generic;
using Gbd.IO.Serial.Win32.Tests.Log;
using Gbd.IO.Serial.Win32.Tests.Logging;
using Serilog;
using Serilog.Sinks.SystemConsole;
using Serilog.Sinks.Literate;
using Xunit;
using Xunit.Abstractions;

namespace Gbd.IO.Serial.Win32.Tests.Root {
    /// <summary> Tests for the Serial Port Controller. </summary>
    public class SerialControllerTests {

        private readonly ITestOutputHelper output;
        private readonly IDisposable _logCapture;

        public SerialControllerTests(ITestOutputHelper outputHelper) {
            _logCapture = LoggingHelper.Capture(outputHelper);
            //this.output = output;
        }

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

        [Fact]
        public void TestLog1() {
            var s_logger = LogProvider.For<SerialControllerTests>();

            s_logger.Info("Test Info");

            //var Logger = new LoggerConfiguration().WriteTo.LiterateConsole().CreateLogger();
            //Console.WriteLine("Test123");
            //Logger.Information("Test Warning");
            //Console.WriteLine("Test456");
            //output.WriteLine("Test789");
            //var log = LogManager.GetCurrentClassLogger();
            //LogManager.GetLogger()
        }

        public void Dispose() {
            _logCapture.Dispose();
        }

        //private static readonly ILog Logger = LogProvider.For<MyClass>();
    }
}