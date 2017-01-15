namespace Gbd.IO.Serial.Interfaces {
    /// <summary> Interface for serial pin states. </summary>
    public interface ISerialPinStates {
        /// <summary> INPUT: Gets the state of the Ring line for the port. </summary>
        /// <value> true if Ring is active, false if not. </value>
        bool Ring_Detect { get; }

        /// <summary> INPUT: Gets the state of the Carrier Detect line for the port. </summary>
        /// <value> true if CD holding, false if not. </value>
        bool CD_Detect { get; }

        /// <summary>
        ///     INPUT: Gets the state of the Clear-to-Send line Typically set via RTS on the other end of
        ///     the serial port.
        /// </summary>
        /// <value> true if cts holding, false if not. </value>
        bool CTS_Detect { get; }

        /// <summary>
        ///     INPUT: Gets the state of the Data Set Ready (DSR) signal Typically set via Dtr on the
        ///     other end of the serial port.
        /// </summary>
        /// <value> true if dsr holding, false if not. </value>
        bool DSR_Detect { get; }

        /// <summary> Gets or sets a value indicating the break state. </summary>
        /// <value> true if break state, false if not. </value>
        bool BreakState { get; set; }

        /// <summary>
        ///     OUTPUT: Gets or sets a value that enables the Data Terminal Ready (DTR) signal during
        ///     serial communication.
        /// </summary>
        /// <value> true if dtr enable, false if not. </value>
        bool Dtr_Enable { get; set; }

        /// <summary>
        ///     OUTPUT: Gets or sets a value indicating whether the Request to Send (RTS) signal is
        ///     enabled during serial communication.
        /// </summary>
        /// <value> true if RTS enable, false if not. </value>
        bool Rts_Enable { get; set; }

        /// <summary> Setup Default Port Pin State Settings. </summary>
        void SetDefaults();

        /// <summary> Reads in the settings from the serial port. </summary>
        void Read();

        /// <summary> Writes the settings to the serial port. </summary>
        void Write();

        /// <summary> Import pin states into this class. </summary>
        /// <param name="importobj"> The states to import. </param>
        void Import(ISerialPinStates importobj);

        /// <summary> Copy. </summary>
        /// <returns> A copy of this object. </returns>
        ISerialPinStates Copy();
    }
}