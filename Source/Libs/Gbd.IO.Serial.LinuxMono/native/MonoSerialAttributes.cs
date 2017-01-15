using System;
using System.Runtime.InteropServices;
using Gbd.IO.Serial.Enums;

// PInvokes the mono serial posix functions
// https://github.com/mono/mono/blob/master/support/serial.c
// The enum values we're using already match those within the Mono Native code

namespace Gbd.IO.Serial.LinuxMono.native {
    public class MonoSerialAttributes {
        [DllImport("MonoPosixHelper", SetLastError = true)]
        private static extern bool set_attributes(int fd, int baudRate, Parity parity, int dataBits, StopBits stopBits,
            Handshake handshake);

        [DllImport("MonoPosixHelper")]
        private static extern bool is_baud_rate_legal(int baud_rate);

        public int Handle;

        public uint BaudRate;

        public Parity Parity;

        public DataBits DataBits;

        public StopBits StopBits;

        public Handshake Handshake;

        /// <summary> Default constructor. </summary>
        internal MonoSerialAttributes() {}

        /// <summary> Writes Serial Attribute Settings. </summary>
        public void Write() {
            if (!is_baud_rate_legal((int) BaudRate))
                throw new ArgumentOutOfRangeException(nameof(BaudRate),
                    "Given baud rate is not supported on this platform.");
            if (!set_attributes(Handle, (int) BaudRate, Parity, (int) DataBits, StopBits, Handshake))
                LinError.LinIOError();
        }
    }
}