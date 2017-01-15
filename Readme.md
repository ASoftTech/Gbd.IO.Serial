# Gbd.IO.Serial

## Overvew

This is a series of Input / Output libraries that can be used under .Net for serial port access

Microsoft recently released part of the code for the .Net library under a MIT licence.
However currently there's no support for the serial port within .Net Core.
Also I was never that fond of the API and saw it as less than optimal from an object orientated point of view.

  * The serial port code here uses interfaces so that other implementations can be used / written
    One example is a proxy connection via a device known as the Buspirate where you talk to the device via a serial port and then use that connection
    to relay serial port data of the device via another Uart.
 
  * There will be an addon library that will allow the use of Reactive extensions with the serial port library.
    This way we can stream recieved data back via the Rx model.
 
  * The serial port code will have support for IObservable for binding to controls such as settings.

The libraries include

  * Gbd.IO.Serial - serial port library base class's
  * Gbd.IO.Serial.Win32 - Windows implementation of the serial port
  * Gbd.IO.Serial.LinuxMono - Linux implementation of the serial port
  * Gbd.IO.Serial.Reactive - Reactive extensions for the sending / recieving of data via the serial port

## Windows

For Windows .Net 4.6.2 is used

  * https://www.microsoft.com/en-us/download/details.aspx?id=53321