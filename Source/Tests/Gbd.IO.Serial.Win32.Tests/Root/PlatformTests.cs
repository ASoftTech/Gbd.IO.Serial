using System.Runtime.InteropServices;
using Xunit;

namespace Gbd.IO.Serial.Win32.Tests.Root {
    /// <summary> Tests for the platform independent class. </summary>
    public class PlatformTests {
        /// <summary> These tests should only be run under Windows. </summary>
        [Fact]
        public void CheckPlatform() {
            var IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            Assert.True(IsWindows, "These tests should only be run under windows");
        }

        /// <summary>
        /// Check we get a controller via the platform method
        /// and that it's the right type for windows.
        /// </summary>
        [Fact]
        public void GetController() {
            var controller = Platform.GetController();
            Assert.NotNull(controller);
            Assert.IsType<SerialController>(controller);
        }
    }
}