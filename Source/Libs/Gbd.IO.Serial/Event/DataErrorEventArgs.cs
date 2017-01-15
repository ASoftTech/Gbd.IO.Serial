using Gbd.IO.Serial.Enums;
using System;

namespace Gbd.IO.Serial.Event {
    /// <summary> Events that occur when there are errors recieving serial data. </summary>
    public class DataErrorEventArgs : EventArgs {
        /// <summary> Gets the type of the event. </summary>
        /// <value> The type of the event. </value>
        public SerialError EventType { get; }

        /// <summary> Default Constructor. </summary>
        /// <param name="eventType"> The type of the event. </param>
        public DataErrorEventArgs(SerialError eventType) {
            EventType = eventType;
        }
    }
}