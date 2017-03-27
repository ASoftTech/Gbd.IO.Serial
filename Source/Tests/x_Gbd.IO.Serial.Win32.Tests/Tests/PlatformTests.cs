using System;
using Gbd.IO.Serial.Win32.Tests.Base;
using Xunit;
using Xunit.Abstractions;

namespace Gbd.IO.Serial.Win32.Tests.Tests {
    /// <summary> Tests for the platform independent class. </summary>
    public class PlatformTests : BaseTest {
        /// <summary> Constructor. </summary>
        /// <param name="outputHelper"> The output helper used by XUnit. </param>
        public PlatformTests(ITestOutputHelper outputHelper) : base(outputHelper) {}

        /// <summary> These tests should only be run under Windows. </summary>
        [Fact]
        public void CheckPlatform() {
            Assert.True(Environment.OSVersion.ToString().Contains("Windows"), "These tests should only be run under windows");
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