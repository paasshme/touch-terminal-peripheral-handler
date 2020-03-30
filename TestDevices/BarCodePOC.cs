using PeripheralTools;
using System;
using System.IO;
using System.IO.Ports;

namespace TestDevices
{
    /**
     * Proof of concept:
     * This object allow the usage of a BarCode reader (here a Datalogic by Gyphon)
     * It must be well configured in order to be triggered by
     * 
     */

    public class BarCodePOC : IDevice, IDisposable
    {

        public IPeripheralEventHandler eventHandler { get; set; }

        // Stop message (configured) that will be send in the serial port
        private const string STOP_MESSAGE = "D";

        // Start message (configured) that will be send in the serial port
        private const string START_MESSAGE = "E";


        // serial port used to communicate with the barcode reader
        private SerialPort serialPort;


        public BarCodePOC(string port)
        {
            string[] ports = SerialPort.GetPortNames();

            if (ports.Length == 0)
            {
                Console.WriteLine("[BarCode]: No ports found");
            }
            else
            {
                Console.WriteLine("[BarCode]: The following serial ports were found:");
                foreach (string foundPort in ports)
                {
                    Console.WriteLine(foundPort);
                }
            }
            
            // Try to instanciate a serial port with the correct parameters
            try
            {
                serialPort = new SerialPort(port, 9600, 0, 8, StopBits.One);
            }
            catch (IOException)
            {
                Console.Error.WriteLine("Port " + serialPort.PortName + "couldn't be found");
            }

            try
            {
                serialPort.Open();
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Unauthorized access to " + serialPort.PortName);
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Parity, databit or handshake are not valid for the " + serialPort.PortName);

                if (serialPort.BaudRate <= 0)
                {
                    Console.WriteLine("Baudrate is lower or equal to 0");
                }

                if (serialPort.WriteTimeout < 0)
                {
                    Console.WriteLine("WriteTimeout is less than 0");
                }

                if (serialPort.ReadTimeout < 0)
                {
                    Console.WriteLine("ReadTimeout is less than 0");
                }
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Error on name " + serialPort.PortName + " or the file type of the port is not supported");
            }
            catch (IOException)
            {
                Console.WriteLine("the port " + serialPort.PortName + " is in a invalid state or open parameter are invalid");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine(serialPort.PortName + " is already open by another process ");
            }
        }

        /*
         * Allow the barcode reader to read barcodes
         */
        public void Start()
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    //Instanciate the delagate
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    serialPort.Write(START_MESSAGE);
                    Console.WriteLine("[BarCode]:  BarCode start signal sent");
                }

            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("The port" + serialPort.PortName + " is not open");
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("The sent text is null");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("The socket has timed out");
            }
        }

        /*
         * Stôp the barcode reading
         */
        public void Stop()
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.ReadExisting();

                    serialPort.Write(STOP_MESSAGE);

                    Console.WriteLine("[BarCode]:  BarCode stop signal sent");
                }

            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("The port" + serialPort.PortName + " is not open");
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("The sent text is null");
            }
            catch (TimeoutException)
            {
                Console.WriteLine("The socket has timed out");
            }
        }

        // Delegate that is used to communicate when something is read on the serial port
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {

            try
            {
                SerialPort sp = (SerialPort)sender;
                string indata = sp.ReadExisting();
                Console.WriteLine("[BarCode] Data Received:");
                Console.WriteLine(indata);

                // Send the event to the microservice
                eventHandler.putPeripheralEventInQueue(indata, "Barcode", "read data");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("The port" + serialPort.PortName + "is not currently open");
            }
        }

        //Handle the close of the port correctly
        public void Dispose()
        {
            try {
                serialPort.Close();
            }
            catch (IOException)
            {
                Console.WriteLine("The port" + serialPort.PortName + " is in an invalid state");
            }
        }
    }
}

