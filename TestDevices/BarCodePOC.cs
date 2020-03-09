using IDeviceLib;
using System;
using System.IO.Ports;

namespace TestDevices
{
    public class BarCodePOC : IDevice
    {
       
            public IPeripheralEventHandler eventHandler { get; set; }

        //SerialPort sp = new SerialPort("/dev/ttyACM1", 9600, 0, 8, StopBits.One);
        private SerialPort sp;


            public BarCodePOC()
            {
                string[] ports = SerialPort.GetPortNames();
                Console.WriteLine("The following serial ports were found:");

                foreach (string port in ports)
                {
                    Console.WriteLine(port);
                try
                {
                    sp = new SerialPort(port, 9600, 0, 8, StopBits.One);
                }catch(Exception e) { };
                }
            }

            void IDevice.Start()
            {
                try
                {
                    if (!sp.IsOpen)
                    {
                        sp.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                        sp.Open();
                        sp.Write("E");
                        System.Console.WriteLine("Written");
                        sp.Close();
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
                eventHandler.putPeripheralEventInQueue("Barcode", "read data", indata);
            }

            void IDevice.Stop()
            {
                try
                {
                    if (!sp.IsOpen)
                    {
                        sp.Open();
                        sp.Write("D");
                        System.Console.WriteLine("closed");
                        sp.Close();
                    }

                }
                catch (Exception e0)
                {
                    System.Console.WriteLine(e0.Message);
                }

            }
        }
    }

