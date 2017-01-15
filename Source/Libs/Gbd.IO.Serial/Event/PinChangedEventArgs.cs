using Gbd.IO.Serial.Enums;
using System;

namespace Gbd.IO.Serial.Event {
    /// <summary> Event Arguments for a Pin Change event. </summary>
    public class PinChangedEventArgs : EventArgs {
        /// <summary> Gets the type of the event. </summary>
        /// <value> The type of the event. </value>
        public PinChange EventType { get; }

        /// <summary> Default Constructor. </summary>
        /// <param name="eventType"> The type of the event. </param>
        public PinChangedEventArgs(PinChange eventType) {
            EventType = eventType;
        }
    }
}