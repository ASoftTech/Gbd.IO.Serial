# Serial Port

The SerialPort class is typically a representation of a single Serial Port, obtained via the SerialController.

There's a number of different properties

| Property | Description |
|----------|-------------|
| *Name*             | This describes the name of the serial port, under Windows this will be usually be something like COM1 or COM5 |
| *IsOpen*           | This is a boolean value that indicates if the serial port has been opened |
| *SerialSettings*   | This allows read / write access to the port configuration such as Baud rate, Handshaking etc |
| *PinStates*        | This allows read / write access to pins such as CD, CTS, DSR, DTR, RTS etc |
| *BufferSettings*   | This property represents the settings for the Buffer, this includes the Read / Write Buffer sizes, parity replace byte etc |
| *SerialProperties* | This reads associated properties of the Serial Port such as max baud rate etc |
| *Uart*             | This property contains methods for reading and writing to the port, as well as flushing the buffers |
