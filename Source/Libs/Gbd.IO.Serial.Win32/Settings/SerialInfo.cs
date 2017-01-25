using Gbd.IO.Serial.Error;
using Gbd.IO.Serial.Interfaces;
using Gbd.IO.Serial.Win32.Settings.SerialInfoEnums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using Gbd.IO.Serial.Win32.Logging;

// https://msdn.microsoft.com/en-us/library/windows/desktop/aa363189(v=vs.85).aspx

namespace Gbd.IO.Serial.Win32.Settings {
    /// <summary> Information about the serial port. </summary>
    public class SerialInfo : ISerialInfo {

        /// <summary> The library logger. </summary>
        private static readonly ILog LibLogger = LogProvider.GetCurrentClassLogger();

        /// <summary> Serial Port properties. </summary>
        public Dictionary<string, string> Props => _Props;

        protected Dictionary<string, string> _Props;

        /// <summary> The associated serial port. </summary>
        public SerialPort Port => _Port;

        protected SerialPort _Port;

        /// <summary> Default constructor. </summary>
        public SerialInfo() {}

        /// <summary> Constructor. </summary>
        /// <param name="sport"> The serial port to associate with. </param>
        internal SerialInfo(SerialPort sport) {
            _Port = sport;
            _Props = new Dictionary<string, string>();
        }

        /// <summary> Reads the actual values from the port. </summary>
        public void Read() {
            ReadMO();
            ReadComProps();
            RefreshDictionary();
        }

        /// <summary> Read details about the serial port using a ManagementObject </summary>
        public void ReadMO() {
            LibLogger.Debug(@"Reading properties from Win32_PnPEntity root\CIMV2");

            var searcher = new ManagementObjectSearcher(@"root\CIMV2",
                "SELECT * FROM Win32_PnPEntity WHERE ConfigManagerErrorCode = 0");

            foreach (var queryObj in searcher.Get()) {
                if (queryObj["Caption"] == null) continue;
                var caption = queryObj["Caption"].ToString();
                if (!caption.Contains(_Port.Name)) continue;

                _Driver_Caption = caption;
                _Driver_Description = queryObj["Description"].ToString();
                _Driver_Manufacturer = queryObj["Manufacturer"].ToString();
                var hardwareid = queryObj["HardwareID"];
                var hardwareienum = hardwareid as IEnumerable<string>;
                if (hardwareienum != null) {
                    _Driver_HardwareID = hardwareienum.FirstOrDefault();
                }
            }
        }

        /// <summary> Read details about the serial port using the ComProps API. </summary>
        public void ReadComProps() {
            LibLogger.Debug(@"Reading from the Comprops API");
            CheckPort();
            _Port.comprops.Read();
            _TxQueue_Max = _Port.comprops._commprops.dwMaxTxQueue;
            _RxQueue_Max = _Port.comprops._commprops.dwMaxRxQueue;
            _TxQueue_Current = _Port.comprops._commprops.dwCurrentTxQueue;
            _RxQueue_Current = _Port.comprops._commprops.dwCurrentRxQueue;
            Enum.TryParse(_Port.comprops._commprops.dwMaxBaud.ToString(), out _MaxBaud);
            Enum.TryParse(_Port.comprops._commprops.dwProvSubType.ToString(), out _ProviderSubType);
            Enum.TryParse(_Port.comprops._commprops.dwProvCapabilities.ToString(), out _ProviderCapabilities);
            Enum.TryParse(_Port.comprops._commprops.dwSettableParams.ToString(), out _SettableParams);
            Enum.TryParse(_Port.comprops._commprops.dwSettableBaud.ToString(), out _SettableBaud);
            Enum.TryParse(_Port.comprops._commprops.wSettableData.ToString(), out _SettableData);
            Enum.TryParse(_Port.comprops._commprops.wSettableStopParity.ToString(), out _SettableStopParity);
            _ProviderSpec1 = _Port.comprops._commprops.dwProvSpec1;
            _ProviderSpec2 = _Port.comprops._commprops.dwProvSpec2;
            _ProviderChar = _Port.comprops._commprops.wcProvChar;
        }

        private void CheckPort() {
            if (_Port == null)
                throw new ArgumentNullException();
            if (!_Port.IsOpen)
                throw new InvalidOperationException(SR.Port_not_open.ToResValue());
        }

        protected void RefreshDictionary() {
            _Props.Clear();
            _Props["TxQueue_Max"] = _TxQueue_Max == 0 ? "Unlimited" : _TxQueue_Max.ToString();
            _Props["RxQueue_Max"] = _RxQueue_Max == 0 ? "Unlimited" : _RxQueue_Max.ToString();
            _Props["TxQueue_Current"] = _TxQueue_Current == 0 ? "Unavailable" : _TxQueue_Current.ToString();
            _Props["RxQueue_Current"] = _RxQueue_Current == 0 ? "Unavailable" : _RxQueue_Current.ToString();
            _Props["MaxBaud"] = _MaxBaud.ToString();
            _Props["ProviderSubType"] = _ProviderSubType.ToString();
            _Props["ProviderCapabilities"] = _ProviderCapabilities.ToString();
            _Props["SettableParameters"] = _SettableParams.ToString();
            _Props["SettableBaud"] = _SettableBaud.ToString();
            _Props["SettableData"] = _SettableData.ToString();
            _Props["SettableStopParity"] = _SettableStopParity.ToString();
            _Props["ProviderSpec1"] = _ProviderSpec1.ToString();
            _Props["ProviderSpec2"] = _ProviderSpec2.ToString();
            _Props["ProviderChar"] = _ProviderChar.ToString();

            _Props["Driver.Caption"] = _Driver_Caption;
            _Props["Driver.Description"] = _Driver_Description;
            _Props["Driver.Manufacturer"] = _Driver_Manufacturer;
            _Props["Driver.HardwareID"] = _Driver_HardwareID;
        }


        /// <summary>
        ///     The maximum size of the driver's internal output buffer, in bytes. A value of zero
        ///     indicates that no maximum value is imposed by the serial provider.
        /// </summary>
        public int TxQueue_Max => _TxQueue_Max;

        protected int _TxQueue_Max;

        /// <summary>
        ///     The maximum size of the driver's internal input buffer, in bytes. A value of zero
        ///     indicates that no maximum value is imposed by the serial provider.
        /// </summary>
        public int RxQueue_Max => _RxQueue_Max;

        protected int _RxQueue_Max;

        /// <summary>
        ///     The size of the driver's internal output buffer, in bytes. A value of zero indicates that
        ///     the value is unavailable.
        /// </summary>
        public int TxQueue_Current => _TxQueue_Current;

        protected int _TxQueue_Current;

        /// <summary>
        ///     The size of the driver's internal input buffer, in bytes. A value of zero indicates that
        ///     the value is unavailable.
        /// </summary>
        public int RxQueue_Current => _RxQueue_Current;

        protected int _RxQueue_Current;

        /// <summary> The maximum allowable baud rate, in bits per second (bps). </summary>
        public BaudInfoEnum MaxBaud => _MaxBaud;

        protected BaudInfoEnum _MaxBaud;

        /// <summary> Provider Sub Type. </summary>
        public ProviderSubTypeEnum ProviderSubType => _ProviderSubType;

        protected ProviderSubTypeEnum _ProviderSubType;

        /// <summary> The provider capabilities. </summary>
        public ProviderCapabilitiesEnum ProviderCapabilities => _ProviderCapabilities;

        protected ProviderCapabilitiesEnum _ProviderCapabilities;

        /// <summary> Settable Parameters. </summary>
        public SettableParamsEnum SettableParams => _SettableParams;

        protected SettableParamsEnum _SettableParams;

        /// <summary> Settable Baud Rates. </summary>
        public BaudInfoEnum SettableBaud => _SettableBaud;

        protected BaudInfoEnum _SettableBaud;


        /// <summary> A bitmask indicating the number of data bits that can be set. </summary>
        public SettableDataEnum SettableData => _SettableData;

        protected SettableDataEnum _SettableData;


        /// <summary> A bitmask indicating the stop bit and parity settings that can be selected. </summary>
        public SettableStopParityEnum SettableStopParity => _SettableStopParity;

        protected SettableStopParityEnum _SettableStopParity;

        /// <summary> Provider Specification 1. </summary>
        public int ProviderSpec1 => _ProviderSpec1;

        protected int _ProviderSpec1;

        /// <summary> Provider Specification 2. </summary>
        public int ProviderSpec2 => _ProviderSpec2;

        protected int _ProviderSpec2;

        /// <summary> Provider Specification Char. </summary>
        public char ProviderChar => _ProviderChar;

        protected char _ProviderChar;

        /// <summary> Driver Caption. </summary>
        public string Driver_Caption => _Driver_Caption;

        protected string _Driver_Caption;

        /// <summary> Driver Description. </summary>
        public string Driver_Description => _Driver_Description;

        protected string _Driver_Description;

        /// <summary> Driver Manufacturer. </summary>
        public string Driver_Manufacturer => _Driver_Manufacturer;

        protected string _Driver_Manufacturer;

        /// <summary> Driver Hardware ID. </summary>
        public string Driver_HardwareID => _Driver_HardwareID;

        protected string _Driver_HardwareID;
    }
}