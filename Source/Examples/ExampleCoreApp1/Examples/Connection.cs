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