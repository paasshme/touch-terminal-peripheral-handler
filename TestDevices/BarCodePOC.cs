using PeripheralTools;
using System;
using System.IO;
using System.IO.Ports;

namespace TestDevices
{
    public class BarCodePOC : IDevice, IDisposable
    {

        public IPeripheralEventHandler eventHandler { get; set; }

        //SerialPort sp = new SerialPort("/dev/ttyACM1", 9600, 0, 8, StopBits.One);
        private SerialPort sp;


        public BarCodePOC(string port)
        {
            string[] ports = SerialPort.GetPortNames();
            Console.WriteLine("The following serial ports were found:");

            foreach (string foundPort in ports)
            {
                Console.WriteLine(foundPort);
            }
            try
            {
                sp = new SerialPort(port, 9600, 0, 8, StopBits.One);
            }
            catch (IOException)
            {
                Console.Error.WriteLine("Port " + sp.PortName + "couldn't be found");
            }

            try
            {
                sp.Open();
            }
            catch (UnauthorizedAccessException)
            {
                System.Console.WriteLine("Unauthorized access to " + sp.PortName);
            }
            catch (ArgumentOutOfRangeException)
            {
                System.Console.WriteLine("Parity, databit or handshake are not valid for the " + sp.PortName);

                if (sp.BaudRate <= 0)
                {
                    System.Console.WriteLine("Baudrate is lower or equal to 0");
                }

                if (sp.WriteTimeout < 0)
                {
                    System.Console.WriteLine("WriteTimeout is less than 0");
                }

                if (sp.ReadTimeout < 0)
                {
                    System.Console.WriteLine("ReadTimeout is less than 0");
                }
            }
            catch (ArgumentException)
            {
                System.Console.WriteLine("Error on name " + sp.PortName + " or the file type of the port is not supported");
            }
            catch (IOException)
            {
                System.Console.WriteLine("the port " + sp.PortName + " is in a invalid state or open parameter are invalid");
            }
            catch (InvalidOperationException)
            {
                System.Console.WriteLine(sp.PortName + " is already open by another process ");
            }
        }

        void IDevice.Start()
        {
            try
            {
                if (sp.IsOpen)
                {
                    sp.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    sp.Write("E");
                    System.Console.WriteLine("Written");
                }

            }
            catch (InvalidOperationException)
            {
                System.Console.WriteLine("The port" + sp.PortName + " is not open");
            }
            catch (ArgumentNullException)
            {
                System.Console.WriteLine("The sent text is null");
            }
            catch (TimeoutException)
            {
                System.Console.WriteLine("The socket has timed out");
            }
        }
        private void DataReceivedHandler(
                    object sender,
                    SerialDataReceivedEventArgs e)
        {

            try {
                SerialPort sp = (SerialPort)sender;
                string indata = sp.ReadExisting();
                Console.WriteLine("Data Received:");
                Console.WriteLine(indata);
                eventHandler.putPeripheralEventInQueue(indata, "Barcode", "read data");
            }
            catch (InvalidOperationException)
            {
                System.Console.WriteLine("The port" + sp.PortName + "is not currently open");
            }
        }

        void IDevice.Stop()
        {
            try
            {
                if (sp.IsOpen)
                {
                    // sp.Open();
                    sp.ReadExisting();

                    sp.Write("D");

                    System.Console.WriteLine("closed");
                }

            }
            catch (InvalidOperationException)
            {
                System.Console.WriteLine("The port" + sp.PortName + " is not open");
            }
            catch (ArgumentNullException)
            {
                System.Console.WriteLine("The sent text is null");
            }
            catch (TimeoutException)
            {
                System.Console.WriteLine("The socket has timed out");
            }
        }

        public void Dispose()
        {
            try {
                sp.Close();
            }
            catch (IOException)
            {
                System.Console.WriteLine("The port" + sp.PortName + " is in an invalid state");
            }
        }
    }
}

