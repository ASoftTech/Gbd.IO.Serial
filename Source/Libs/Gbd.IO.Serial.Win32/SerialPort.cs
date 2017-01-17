using Gbd.IO.Serial.Error;
using Gbd.IO.Serial.Event;
using Gbd.IO.Serial.Interfaces;
using Gbd.IO.Serial.Win32.native;
using Gbd.IO.Serial.Win32.Settings;
using Microsoft.Win32.SafeHandles;
using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

// TODO
// Event Handling

namespace Gbd.IO.Serial.Win32 {
    /// <summary> Windows implementation of a serial port. </summary>
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

        /// <summary> The serial properties. </summary>
        public ISerialProperties SerialProperties => _SerialProperties;

        protected internal SerialProperties _SerialProperties;

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

        // The internal C# representations of Win32 structures necessary for communication
        // hold most of the internal "fields" maintaining information about the port.
        internal SafeFileHandle _handle;
        internal ComFile comfile;
        internal DCB dcb;
        internal ComProps comprops;
        internal ComStatus comstatus;
        internal ComTimeouts comtimeouts;
        internal ComMask commask;
        internal ComBufferStatus combufferstatus;
        internal EscapeCom escapecom;
        internal SerialEventHandler serialeventhandler;
        internal WaitComm waitcom;

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
            _SerialProperties = new SerialProperties(this);
            _Uart = new SerialUart(this);
            serialeventhandler = new SerialEventHandler(this);

            // Unmanaged Structures and Access
            dcb = new DCB();
            comprops = new ComProps();
            comstatus = new ComStatus();
            comtimeouts = new ComTimeouts();
            commask = new ComMask();
            combufferstatus = new ComBufferStatus();
            escapecom = new EscapeCom();
            waitcom = new WaitComm();

            // Set Defaults
            _SerialSettings.SetDefaults();
            _PinStates.SetDefaults();
            _BufferSettings.SetDefaults();

            if (skipcheck) return;
            if (_Name == null)
                throw new ArgumentNullException(nameof(portname));
            if (_Name.Length == 0)
                throw new ArgumentException(SR.PortNameEmpty_String.ToResValue(), nameof(portname));
            // disallow access to device resources beginning with @"\\", instead requiring "COM2:", etc.
            // Note that this still allows freedom in mapping names to ports, etc., but blocks security leaks.
            if (_Name.StartsWith("\\\\", StringComparison.Ordinal))
                throw new ArgumentException(SR.Arg_SecurityException.ToResValue(), nameof(portname));
        }

        /// <summary> Reads in all settings from the serial port. </summary>
        public void Read_Settings() {
            if (!IsOpen)
                throw new InvalidOperationException(SR.Port_not_open.ToResValue());
            _SerialSettings.Read();
            _PinStates.Read();
            _BufferSettings.Read();
        }

        /// <summary> Initialise the settings after the port is opened. </summary>
        public void Write_Settings() {
            if (!IsOpen)
                throw new InvalidOperationException(SR.Port_not_open.ToResValue());
            // This will cause all the values to be replicated into unmanaged space
            _SerialSettings.Write();
            _PinStates.Write();
            _BufferSettings.Write();
        }

        /// <summary> Opens the serial port. </summary>
        public void Open() {
            if (disposed)
                throw new InvalidOperationException(SR.Port_disposed.ToResValue());
            if (IsOpen)
                throw new InvalidOperationException(SR.Port_already_open.ToResValue());

            // Demand unmanaged code permission
            new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();

            // Open the connection to the serial port com file
            comfile = new ComFile(_Name);
            comfile.Open();

            try {
                // Assign Handles
                comfile.GetFileType();
                _handle = comfile.Handle;
                dcb.Handle = _handle;
                comprops.Handle = _handle;
                comstatus.Handle = _handle;
                comtimeouts.Handle = _handle;
                commask.Handle = _handle;
                combufferstatus.Handle = _handle;
                escapecom.Handle = _handle;
                waitcom.Handle = _handle;

                // Read Com Port Status
                dcb.Read();
                comprops.Read();
                comstatus.Read();
                comtimeouts.Read();

                if (!ThreadPool.BindHandle(_handle)) throw new IOException(SR.IO_BindHandleFailed.ToResValue());

                // monitor all events except TXEMPTY
                commask.MaskValue = ComMask.EvtMask.ALL_EVENTS;
                commask.Write();

                //TODO Event Loop
                // prep. for starting event cycle.
                //eventRunner = new EventLoopRunner(this);
                //Thread eventLoopThread = new Thread(new ThreadStart(eventRunner.WaitForCommEvent));
                //eventLoopThread.IsBackground = true;
                //eventLoopThread.Start();

                _IsOpen = true;

                // Initialise the settings after the port is opened
                Write_Settings();

                // Flush the Uart Buffers
                _Uart.DiscardInBuffer();
                _Uart.DiscardOutBuffer();
            }
            catch {
                // if there are any exceptions after the call to CreateFile, we need to be sure to close the
                // handle before we let them continue up.
                comfile.Close();
                _handle = null;
                dcb.Handle = null;
                comprops.Handle = null;
                comstatus.Handle = null;
                comtimeouts.Handle = null;
                commask.Handle = null;
                combufferstatus.Handle = null;
                escapecom.Handle = null;
                _IsOpen = false;
                throw;
            }
        }

        /// <summary> Closes the serial port. </summary>
        public void Close() {
            if (!IsOpen) return;
            try {
                //TODO Event Handling
                //eventRunner.endEventLoop = true;

                Thread.MemoryBarrier();
                var skipSPAccess = false;

                // turn off all events and signal WaitCommEvent
                commask.MaskValue = ComMask.EvtMask.NONE;
                commask.Write();

                if (!escapecom.WriteBool(EscapeCom.ExtendedFunctions.CLRDTR)) {
                    var hr = Marshal.GetLastWin32Error();

                    // access denied can happen if USB is yanked out. If that happens, we
                    // want to at least allow finalize to succeed and clean up everything 
                    // we can. To achieve this, we need to avoid further attempts to access
                    // the SerialPort.  A customer also reported seeing ERROR_BAD_COMMAND here.
                    // Do not throw an exception on the finalizer thread - that's just rude,
                    // since apps can't catch it and we may tear down the app.
                    if ((hr == WinError.ERROR_ACCESS_DENIED || hr == WinError.ERROR_BAD_COMMAND)) skipSPAccess = true;
                    else // should not happen
                        Contract.Assert(false,
                            $"Unexpected error code from EscapeCommFunction in SerialPort.Dispose(bool)  Error code: 0x{(uint) hr:x}");
                }

                if (!skipSPAccess && !_handle.IsClosed) _Uart.Flush();

                //TODO Event Handling
                //eventRunner.waitCommEventWaitHandle.Set();

                if (!skipSPAccess) {
                    _Uart.DiscardInBuffer();
                    _Uart.DiscardOutBuffer();
                }

                //TODO Event Handling
                //if (disposing && eventRunner != null) {
                //    // now we need to wait for the event loop to tell us it's done.  Without this we could get into a ---- where the
                //    // event loop kept the port open even after Dispose ended.
                //    eventRunner.eventLoopEndedSignal.WaitOne();
                //    eventRunner.eventLoopEndedSignal.Close();
                //    eventRunner.waitCommEventWaitHandle.Close();
                //}
            }
            finally {
                comfile.Close();
                _handle = null;
                dcb.Handle = null;
                comprops.Handle = null;
                comstatus.Handle = null;
                comtimeouts.Handle = null;
                commask.Handle = null;
                combufferstatus.Handle = null;
                escapecom.Handle = null;
                _IsOpen = false;
            }
        }

        /// <summary> Finaliser. </summary>
        ~SerialPort() {
            Dispose();
        }

        /// <summary> Disposal. </summary>
        public void Dispose() {
            if (disposed) return;
            if (_handle == null || _handle.IsInvalid) return;
            if (IsOpen) Close();
            _Uart.Dispose();
            disposed = true;
        }
    }
}