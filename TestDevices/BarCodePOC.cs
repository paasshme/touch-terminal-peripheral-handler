using IDeviceLib;
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
                    Console.Error.WriteLine("Port " + port + "couldn't be opened");
                }
                
                sp.Open();      
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

                catch (Exception e1)
                {
                    System.Console.WriteLine(e1.Message);
                }
            }
            private void DataReceivedHandler(
                        object sender,
                        SerialDataReceivedEventArgs e)
            {

                SerialPort sp = (SerialPort)sender;
                string indata = sp.ReadExisting();
                Console.WriteLine("Data Received:");
                Console.WriteLine(indata);
                eventHandler.putPeripheralEventInQueue(indata,"Barcode", "read data");
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
                catch (Exception e0)
                {
                    System.Console.WriteLine(e0.Message);
                }

            }

        public void Dispose()
        {
            sp.Close();
        }
    }
}

