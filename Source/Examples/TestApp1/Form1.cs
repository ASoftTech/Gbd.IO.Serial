using System;
using System.Diagnostics;
using System.Windows.Forms;
using Appst.IO.Serial;
using Appst.IO.Serial.Streams;

namespace TestApp1 {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            //TestPins();
            TestRxTx();
        }

        /// <summary> Test sending and recieving data. </summary>
        public void TestRxTx() {

            var x1 = Platform.GetController();


            var controller = Appst.IO.Serial.Win32.SerialController.GetDefault();
            var ports = controller.GetPorts();

            var FirstSerial = ports[0];
            var SecondSerial = ports[1];

            FirstSerial.Open();
            SecondSerial.Open();

            var sw = FirstSerial.ToSerialWriter();
            var sr = SecondSerial.ToSerialReader();

            sw.Write("Hello World");
            var temp3 = sr.ReadString();
            Debug.WriteLine(temp3);

            sw.Write("Hello World2");
            var temp4 = sr.ReadString();
            Debug.WriteLine(temp4);

            FirstSerial.Dispose();
            SecondSerial.Dispose();
        }

        /// <summary> Tests pin inputs and outputs </summary>
        public void TestPins() {
            var controller = Appst.IO.Serial.Win32.SerialController.GetDefault();
            var ports = controller.GetPorts();

            var FirstSerial = ports[0];
            var SecondSerial = ports[1];

            FirstSerial.Open();
            SecondSerial.Open();

            bool cts1, dsr1, cd1;

            // Test of Setting the CTS Pin / Input of the RTS pin via a Null Modem cable
            Debug.WriteLine("Setting Rts_Enable to false");
            FirstSerial.PinStates.Rts_Enable = false;
            FirstSerial.PinStates.Write();
            SecondSerial.PinStates.Read();
            cts1 = SecondSerial.PinStates.CTS_Detect;
            Debug.WriteLine("CTS_Detect: " + cts1);
            Debug.WriteLine("");

            Debug.WriteLine("Setting Rts_Enable to true");
            FirstSerial.PinStates.Rts_Enable = true;
            FirstSerial.PinStates.Write();
            SecondSerial.PinStates.Read();
            cts1 = SecondSerial.PinStates.CTS_Detect;
            Debug.WriteLine("CTS_Detect: " + cts1);
            Debug.WriteLine("");

            Debug.WriteLine("Setting Rts_Enable to false");
            FirstSerial.PinStates.Rts_Enable = false;
            FirstSerial.PinStates.Write();
            SecondSerial.PinStates.Read();
            cts1 = SecondSerial.PinStates.CTS_Detect;
            Debug.WriteLine("CTS_Detect: " + cts1);
            Debug.WriteLine("");

            // Test of Setting the Dtr Pin / Input of the DSR pin via a Null Modem cable
            // With some null modem cables Carrier Detect is wired to the output of DSR
            Debug.WriteLine("Setting Dtr_Enable to false");
            FirstSerial.PinStates.Dtr_Enable = false;
            FirstSerial.PinStates.Write();
            SecondSerial.PinStates.Read();
            dsr1 = SecondSerial.PinStates.DSR_Detect;
            cd1 = SecondSerial.PinStates.CD_Detect;
            Debug.WriteLine("DSR_Detect: " + dsr1 + " CD_Detect: " + cd1);
            Debug.WriteLine("");

            Debug.WriteLine("Setting Dtr_Enable to true");
            FirstSerial.PinStates.Dtr_Enable = true;
            FirstSerial.PinStates.Write();
            SecondSerial.PinStates.Read();
            dsr1 = SecondSerial.PinStates.DSR_Detect;
            cd1 = SecondSerial.PinStates.CD_Detect;
            Debug.WriteLine("DSR_Detect: " + dsr1 + " CD_Detect: " + cd1);
            Debug.WriteLine("");

            Debug.WriteLine("Setting Dtr_Enable to false");
            FirstSerial.PinStates.Dtr_Enable = false;
            FirstSerial.PinStates.Write();
            SecondSerial.PinStates.Read();
            dsr1 = SecondSerial.PinStates.DSR_Detect;
            cd1 = SecondSerial.PinStates.CD_Detect;
            Debug.WriteLine("DSR_Detect: " + dsr1 + " CD_Detect: " + cd1);
            Debug.WriteLine("");

            FirstSerial.Dispose();
            SecondSerial.Dispose();
        }

    }
}
