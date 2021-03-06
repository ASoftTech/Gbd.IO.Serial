# Connection

The first step in getting a connection to the serial port is to get the SerialController instance. <br>
The SerialController is a single instance class and allows us to

  * Obtain a list of all available serial ports names
  * Obtain a serial port class for the given name
  * Obtain a enumerable list of all serial ports as a list of class's

## Platform

Since we have different SerialController's for different platforms such as Windows and Linux <br>
The best way to get the correct controller for a given platform is to use the Platform class


```cs
var controller = Platform.GetController();
```


  * In the case of windows the SerialController class instance from the //Gbd.IO.Serial.Win32// library will be returned.
  * In the case of linux the SerialController class instance from the //Gbd.IO.Serial.LinuxMono// library will be returned.

## Serial Controller

Once we have the controller we can then use this to obtain a list of all available Serial Ports <br>
This is an example of a few different methods of getting the serialport instance

```cs
using System;
using System.Linq;
using Gbd.IO.Serial;

namespace ExampleCoreApp1.Examples {
    /// <summary> An example of getting a connection and serial port. </summary>
    public class Connection {
        public static void Run() {
            // Get the Serial Port Controller based on the platform
            var controller = Platform.GetController();

            // Show a list of all serial port names
            var portnames = controller.GetPortNames();
            foreach (var item in portnames) {
                Console.WriteLine(item);
            }

            // Get a Serial Port based on the name
            // Note this does not check if the serial port exists
            var sport1 = controller.GetPort("COM10");

            // Get the first serial port in the list of names
            if (portnames.Count > 0) {
                var sport2 = controller.GetPort(portnames.First());
            }

            // Get a List of Serial Port Class's
            var portlist = controller.GetPorts();

            // Use Linq to select a port
            var sport3 = (from port in controller.GetPorts()
                where port.Name == "COM10"
                select port).FirstOrDefault();
        }
    }
}
```

