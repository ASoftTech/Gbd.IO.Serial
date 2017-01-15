using System;
using System.Runtime.InteropServices;

namespace Gbd.IO.Serial.LinuxMono.native
{
    public class MonoSerialSignal
    {
        [DllImport("MonoPosixHelper", SetLastError = true)]
        internal static extern int set_signal(int fd, SerialSignal signal, bool value);

        [DllImport("MonoPosixHelper", SetLastError = true)]
        static extern SerialSignal get_signals(int fd, out int error);

        [DllImport("MonoPosixHelper", SetLastError = true)]
        static extern int breakprop(int fd);

        [Flags]
        internal enum SerialSignal {
            None = 0,
            Cd = 1, // Carrier detect 
            Cts = 2, // Clear to send
            Dsr = 4, // Data set ready
            Dtr = 8, // Data terminal ready
            Rts = 16 // Request to send
        }

        public int Handle;

        // Output Signals
        public bool Rts_Enable;
        public bool Dtr_Enable;
        public bool BreakState;

        // Input Signals
        public bool Ring_Detect;
        public bool CD_Detect;
        public bool CTS_Detect;
        public bool DSR_Detect;

        /// <summary> Default constructor. </summary>
        internal MonoSerialSignal() { }

        /// <summary> Writes Serial Signals. </summary>
        public void Write() {
            if (set_signal(Handle, SerialSignal.Dtr, Dtr_Enable) == -1)
                LinError.LinIOError();
            if (set_signal(Handle, SerialSignal.Rts, Rts_Enable) == -1)
                LinError.LinIOError();
            // TODO I think this only sets the break state?, with no way to clear
            // TODO maybe the port only resets on open / close
            if (BreakState)
                if (breakprop(Handle) == -1)
                    LinError.LinIOError();
        }

        /// <summary> Reads Serial Signals. </summary>
        public void Read() {
            int error;
            SerialSignal signals = get_signals(Handle, out error);
            if (error == -1)
                LinError.LinIOError();
            Ring_Detect = false; // TODO Not Supported
            CD_Detect = ((signals & SerialSignal.Cd) != 0);
            CTS_Detect = ((signals & SerialSignal.Cts) != 0);
            DSR_Detect = ((signals & SerialSignal.Dsr) != 0);
        }
    }
}
