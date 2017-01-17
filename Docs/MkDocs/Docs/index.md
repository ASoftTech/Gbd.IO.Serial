# Gbd.IO.Serial

## Overvew

This is a series of Input / Output libraries that can be used under .Net for serial port access

Microsoft recently released part of the code for the .Net library under a MIT licence.
I had a couple of different reasons for writing this

  * Currently there's no support for the serial port using the .Net Core framework
  * The original serial port implementation within .Net was a bit buggy when it cames to events
  * The original serial port API lacked a object orientated layout

There were also a few features I wanted to include in a new re-write

  * The serial port code here uses interfaces so that other implementations can be used / written
    One example is a proxy connection via a device known as the Buspirate where you talk to the device via a serial port and then use that connection
    to relay serial port data of the device via another Uart.
 
  * There will be an addon library with extension functions that will allow the use of [Reactive extensions](http://reactivex.io/).
    This way we can stream recieved data back via the Rx model.
 
  * The serial port code will have support for IObservable for binding to controls such as settings.

## Libraries

  * **Gbd.IO.Serial** - Base class's for the serial port
  * **Gbd.IO.Serial.Win32** - Windows implementation of the serial port using Pinvoke
  * **Gbd.IO.Serial.LinuxMono** - Linux implementation of the serial port using the C Mono library wrapper
  * **Gbd.IO.Serial.Reactive** - Reactive extensions for the sending / receiving of data.
