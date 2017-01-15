using System;

namespace Gbd.IO.Serial.Event {
    /// <summary> Event arguments for when data is available. </summary>
    public class DataRxEventArgs : EventArgs {
        /// <summary> The number of bytes available. </summary>
        /// <value> The count of bytes available. </value>
        public int Count { get; }

        /// <summary> Default Constructor. </summary>
        /// <param name="count"> The count of bytes available. </param>
        public DataRxEventArgs(int count) {
            Count = count;
        }
    }
}