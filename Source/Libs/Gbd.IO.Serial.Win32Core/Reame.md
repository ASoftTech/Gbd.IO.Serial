# Readme

This is a Windows implementation of a Serial port driver.

  * The API is more logically laid out (the original MS one is designed for backwards compatibility)
  * For use with Gbd.IO.Serial.Reactive via reactive extensions
  * Inherits from base class's within Gbd.IO.Serial

This implemtation borrows a lot of code from

  * https://github.com/dotnet/corefx/tree/master/src/System.IO.Ports
  * https://github.com/dotnet/corefx/tree/master/src/Common/src/Interop


## TODO

### API

Integrate with the existing API I've setup so far

### Interface

Setup a test app using .Net Core / VueJS front end

### UAP / UWP

Looking at Microsoft's implementation of System.IO.Ports
For the serial port they use

  * Interop\Windows\kernel32\Interop.CreateFile.cs - For non UAP Programs
  * Interop\Windows\mincore\Interop.OpenCommPort.cs - For UAP

At the moment I'm only using Interop.CreateFile.cs so I'll need to look at that later
if we want to add in UAP Support