using System;
using Gbd.IO.Serial.Win32.Tests.Logging;
using Gbd.IO.Serial.Win32.Tests.Root;
using Xunit;
using Xunit.Abstractions;

namespace Gbd.IO.Serial.Win32.Tests.Log {
    public class LoggingTest : IDisposable {

        private readonly ITestOutputHelper output;
        private readonly IDisposable _logCapture;

        public LoggingTest(ITestOutputHelper outputHelper) {
            output = outputHelper;
            _logCapture = LoggingHelper.Capture(outputHelper);
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


        //private static readonly ILog Logger = LogProvider.For<MyClass>();

        public void Dispose() {
            _logCapture.Dispose();
        }
    }
}