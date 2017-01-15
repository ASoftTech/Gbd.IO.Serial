using Gbd.IO.Serial.Interfaces;

namespace Gbd.IO.Serial.Base {
    /// <summary> Base class for pin states of a serial port. </summary>
    public class SerialPinStatesBase : ISerialPinStates {
        /// <summary> INPUT: Gets the state of the Ring line for the port. </summary>
        /// <value> true if Ring is active, false if not. </value>
        public virtual bool Ring_Detect => _Ring_Detect;

        protected bool _Ring_Detect;

        /// <summary> INPUT: Gets the state of the Carrier Detect line for the port. </summary>
        /// <value> true if CD holding, false if not. </value>
        public virtual bool CD_Detect => _CD_Detect;

        protected bool _CD_Detect;

        /// <summary>
        ///     INPUT: Gets the state of the Clear-to-Send line Typically set via RTS on the other end of
        ///     the serial port.
        /// </summary>
        /// <value> true if cts holding, false if not. </value>
        public virtual bool CTS_Detect => _CTS_Detect;

        protected bool _CTS_Detect;

        /// <summary>
        ///     INPUT: Gets the state of the Data Set Ready (DSR) signal Typically set via Dtr on the
        ///     other end of the serial port.
        /// </summary>
        /// <value> true if dsr holding, false if not. </value>
        public virtual bool DSR_Detect => _DSR_Detect;

        protected bool _DSR_Detect;

        /// <summary> Gets or sets a value indicating the break state. </summary>
        /// <value> true if break state, false if not. </value>
        public virtual bool BreakState { get; set; }

        /// <summary>
        ///     OUTPUT: Gets or sets a value that enables the Data Terminal Ready (DTR) signal during
        ///     serial communication.
        /// </summary>
        /// <value> true if dtr enable, false if not. </value>
        public virtual bool Dtr_Enable { get; set; }

        /// <summary>
        ///     OUTPUT: Gets or sets a value indicating whether the Request to Send (RTS) signal is
        ///     enabled during serial communication.
        /// </summary>
        /// <value> true if RTS enable, false if not. </value>
        public virtual bool Rts_Enable { get; set; }

        /// <summary> Setup Default Port Pin State Settings. </summary>
        public virtual void SetDefaults() {
            BreakState = false;
            Dtr_Enable = false;
            Rts_Enable = false;
        }

        /// <summary> Reads in the settings from the serial port. </summary>
        public virtual void Read() {
            throw new System.NotImplementedException();
        }

        /// <summary> Writes the settings to the serial port. </summary>
        public virtual void Write() {
            throw new System.NotImplementedException();
        }

        /// <summary> Import pin states into this class. </summary>
        /// <param name="importobj"> The states to import. </param>
        public void Import(ISerialPinStates importobj) {
            BreakState = importobj.BreakState;
            Dtr_Enable = importobj.Dtr_Enable;
            Rts_Enable = importobj.Rts_Enable;
        }

        /// <summary> Copy. </summary>
        /// <returns> A copy of this object. </returns>
        public ISerialPinStates Copy() {
            SerialPinStatesBase ret = new SerialPinStatesBase();
            ret.Import(this);
            return ret;
        }
    }
}