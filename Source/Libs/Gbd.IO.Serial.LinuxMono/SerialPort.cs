using System;
using System.Diagnostics;
using System.IO;
using Gbd.IO.Serial.Error;
using Gbd.IO.Serial.Event;
using Gbd.IO.Serial.Interfaces;
using Gbd.IO.Serial.LinuxMono.native;
using Gbd.IO.Serial.LinuxMono.Settings;

// TODO
// Event Handling

namespace Gbd.IO.Serial.LinuxMono {
    /// <summary> Linux Mono implementation of a serial port. </summary>
    // ReSharper disable once UseNameofExpression
    [DebuggerDisplay("SerialPort = {Name}")]
    public class SerialPort : ISerialPort {
        /// <summary> Text string name of the serial port. </summary>
        public string Name => _Name;

        protected string _Name { get; set; }

        /// <summary> If the Serial Port is open. </summary>
        public bool IsOpen => _IsOpen;

        protected bool _IsOpen { get; set; }
        private bool disposed;

        /// <summary> Serial Port Settings. </summary>
        public ISerialSettings SerialSettings => _SerialSettings;

        protected internal SerialSettings _SerialSettings;

        /// <summary> Serial Port pin states. </summary>
        public ISerialPinStates PinStates => _PinStates;

        protected internal SerialPinStates _PinStates;

        /// <summary> Serial Port buffer settings. </summary>
        public ISerialBufferSettings BufferSettings => _BufferSettings;

        protected internal SerialBufferSettings _BufferSettings;

        /// <summary> The Serial Port Properties. </summary>
        public ISerialInfo SerialInfo => _SerialInfo;

        protected internal SerialInfo _SerialInfo;

        /// <summary> Serial Port buffer settings. </summary>
        public ISerialUart Uart => _Uart;

        protected internal SerialUart _Uart;

        /// <summary> Event queue for all listeners interested in PinChanged events. </summary>
        public event EventHandler PinChanged;

        /// <summary> Raises the pin changed event. </summary>
        /// <param name="e"> Event information to send to registered event handlers. </param>
        protected internal virtual void OnPinChanged(PinChangedEventArgs e) {
            PinChanged?.Invoke(this, e);
        }

        // The internal C# representations of Linux structures necessary for communication
        // hold most of the internal "fields" maintaining information about the port.
        internal int _handle;
        internal MonoSerialAttributes monoserialattrib;
        internal MonoSerialSignal monoserialsignal;
        internal MonoSerialFile monoserialfile;

        /// <summary> Default Constructor. </summary>
        /// <param name="portname"> The portname. </param>
        public SerialPort(string portname) : this(portname, false) {}

        /// <summary> Internal Constructor. </summary>
        /// <param name="portname">  The portname. </param>
        /// <param name="skipcheck"> true to skipcheck. </param>
        internal SerialPort(string portname, bool skipcheck) {
            _Name = portname;
            disposed = false;

            // Public Access Properties
            _SerialSettings = new SerialSettings(this);
            _PinStates = new SerialPinStates(this);
            _BufferSettings = new SerialBufferSettings(this);
            _SerialInfo = new SerialInfo(this);
            _Uart = new SerialUart(this);

            // Unmanaged Structures and Access
            monoserialattrib = new MonoSerialAttributes();
            monoserialsignal = new MonoSerialSignal();
            monoserialfile = new MonoSerialFile();

            // Set Defaults
            _SerialSettings.SetDefaults();
            _PinStates.SetDefaults();
            _BufferSettings.SetDefaults();

            if (skipcheck) return;
            if (_Name == null)
                throw new ArgumentNullException(nameof(portname));
            if (_Name.Length == 0)
                throw new ArgumentException(SR.PortNameEmpty_String.ToResValue(), nameof(portname));
        }

        /// <summary> Reads in all settings from the serial port. </summary>
        public void Read_Settings() {
            if (!IsOpen)
                throw new InvalidOperationException(SR.Port_not_open.ToResValue());
            _PinStates.Read();
        }

        /// <summary> Initialise the settings after the port is opened. </summary>
        public void Write_Settings() {
            if (!IsOpen)
                throw new InvalidOperationException(SR.Port_not_open.ToResValue());
            // This will cause all the values to be replicated into unmanaged space
            _SerialSettings.Write();
            _PinStates.Write();
        }

        /// <summary> Opens the serial port. </summary>
        /// <exception cref="InvalidOperationException"> Thrown when the requested operation is invalid. </exception>
        public void Open() {
            if (disposed)
                throw new InvalidOperationException(SR.Port_disposed.ToResValue());
            if (IsOpen)
                throw new InvalidOperationException(SR.Port_already_open.ToResValue());

            try {
                monoserialfile.OpenPort(_Name);
                _handle = monoserialfile.Handle;
                monoserialattrib.Handle = _handle;
                monoserialsignal.Handle = _handle;
                _IsOpen = true;

                // Initialise the settings after the port is opened
                Write_Settings();

                // Flush the Uart Buffers
                _Uart.DiscardInBuffer();
                _Uart.DiscardOutBuffer();
            }
            catch {
                monoserialfile.ClosePort();
                monoserialfile.Handle = -1;
                monoserialsignal.Handle = -1;
                monoserialattrib.Handle = -1;
                _handle = -1;
                _IsOpen = false;
                throw;
            }
        }

        /// <summary> Closes the serial port. </summary>
        public void Close() {
            if (!IsOpen) return;
            try {
                MonoSerialSignal.set_signal(_handle, MonoSerialSignal.SerialSignal.Dtr, false);
                _Uart.Flush();
                _Uart.DiscardInBuffer();
                _Uart.DiscardOutBuffer();
            }
            finally {
                monoserialfile.ClosePort();
                monoserialfile.Handle = -1;
                monoserialsignal.Handle = -1;
                monoserialattrib.Handle = -1;
                _handle = -1;
                _IsOpen = false;
            }
        }

        /// <summary> Finaliser. </summary>
        ~SerialPort() {
            try {
                Dispose();
            }
            catch (IOException) {
            }
        }

        /// <summary> Disposal. </summary>
        public void Dispose() {
            if (disposed) return;
            if (IsOpen) Close();
            _Uart.Dispose();
            disposed = true;
        }

    }
}