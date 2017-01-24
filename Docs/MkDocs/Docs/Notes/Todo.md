# Todo Notes

## General

  * Consider Gbd.IO.Serial.Compat library for supporting the old API
  * Once the lower level API is working consider looking into a Reactive Extensions layer on top for Rx / Tx of data

  * Check removal of serial port when usb lead is disconnected
    We may be able to use info from the ManagementObjectSearcher / SerialInfo class

  * Check for Ring pin support, and additional speeds for the baud rate
  * Check / test the buffer code

## Vagrant

  * Look into using Vagrant to generate a VirtualBox VM Image for testing the linux implementation.
  * Look into using Virtualbox's seamless mode and remote debugging with Visual Studio

## Testing

  * Check the output from XUnit Tests to console / Test Output window
  * Code up all tests

## Examples

  * Code up Examples, maybe using GtkSharp and ASP .Net Core Console / Web apps

## Logging

  * It looks like there's a currently an issue when using ITestOutputHelper when outputing logging from XUnit with core.
    this seems to be more of a tooling issue with core instead of a XUnit issue
    https://github.com/xunit/xunit/issues/608

  * Issue with contentfiles and nuget / core projects
    https://github.com/NuGet/Home/issues/2262
    http://stackoverflow.com/questions/40469058/nuget-content-files-in-net-core-solution-not-getting-copied-when-installing-thr
    Eventually switch across to liblog once we can include it via Nuget, for now just use Serilog directly

## Documentation

  * Finish up Docs
