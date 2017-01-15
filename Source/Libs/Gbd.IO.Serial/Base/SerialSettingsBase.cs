using Gbd.IO.Serial.Enums;
using Gbd.IO.Serial.Interfaces;
using System;

namespace Gbd.IO.Serial.Base {
    /// <summary> A Base class for serial port settings. </summary>
    public class SerialSettingsBase : ISerialSettings {
        /// <summary> Gets or sets the number of data bits associated with the port. </summary>
        /// <value> The number of data bits. </value>
        public virtual DataBits DataBits { get; set; }

        /// <summary> Gets or sets the serial port parity. </summary>
        /// <value> The serial port parity. </value>
        public virtual Parity Parity { get; set; }

        /// <summary> Gets or sets the serial port stop bits. </summary>
        /// <value> The number of stop bits used on the serial port. </value>
        public virtual StopBits StopBits { get; set; }

        /// <summary> Gets or sets the handshake mode for the serial port. </summary>
        /// <value> The handshake mode. </value>
        public virtual Handshake Handshake { get; set; }

        /// <summary> Speed or BaudRate of the Serial Port, Expressed as an Integer Value. </summary>
        /// <value> The baud rate as an integer. </value>
        public virtual uint BaudRate_Int { get; set; }

        /// <summary> Speed or BaudRate of the Serial Port, Expressed as an Enum Value. </summary>
        /// <value> The baud rate as an enum. </value>
        public virtual BaudRates BaudRate {
            get {
                BaudRates tmpenum;
                Enum.TryParse(BaudRate_Int.ToString(), out tmpenum);
                return tmpenum;
            }
            set { BaudRate_Int = (uint) value; }
        }

        /// <summary> Reads in the settings from the serial port. </summary>
        public virtual void Read() {
            throw new NotImplementedException();
        }

        /// <summary> Writes the settings to the serial port. </summary>
        public virtual void Write() {
            throw new NotImplementedException();
        }

        /// <summary> Setup Default Port Settings. </summary>
        public virtual void SetDefaults() {
            DataBits = DataBits.D8;
            Parity = Parity.None;
            StopBits = StopBits.One;
            Handshake = Handshake.None;
            BaudRate = BaudRates.B9600;
        }

        /// <summary> Copy / Import Port Settings into underlying Serial Port Object. </summary>
        /// <param name="impsetts"> The settings to import / copy from. </param>
        public virtual void Import(ISerialSettings impsetts) {
            DataBits = impsetts.DataBits;
            Parity = impsetts.Parity;
            StopBits = impsetts.StopBits;
            Handshake = impsetts.Handshake;
            BaudRate_Int = impsetts.BaudRate_Int;
        }

        /// <summary> Copy. </summary>
        /// <returns> A copy of this object. </returns>
        public virtual ISerialSettings Copy() {
            SerialSettingsBase ret = new SerialSettingsBase();
            ret.Import(this);
            return ret;
        }
    }
}