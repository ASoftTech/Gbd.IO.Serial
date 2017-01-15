using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gbd.IO.Serial.Event;

// TODO fill this out for capturing events for the serial port
// TODO Pin change, data recieved, error recieved
// TODO 

namespace Gbd.IO.Serial.Win32
{
    /// <summary> Serial Port event handler </summary>
    public class SerialEventHandler
    {
        /// <summary> The associated serial port. </summary>
        public SerialPort Port => _Port;

        protected SerialPort _Port;

        /// <summary> Constructor. </summary>
        /// <param name="sport"> The serial port to associate with. </param>
        internal SerialEventHandler(SerialPort sport) {
            _Port = sport;
        }

        public void test1() {
            //_Port.OnPinChanged(null);
            //_Port._Uart.OnDataRx(null);
            //_Port._Uart.OnErrorRx(null);
        }
    }
}
