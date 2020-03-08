using System;
using System.IO.Ports;

namespace BarCode
{
    class TestWrite 
    {
        static void Main(string[] args)
        {
            SerialPort sp = new SerialPort("COM5", 9600, 0, 8, StopBits.One);
         
                sp.Open();
                sp.Write("E");
                System.Console.WriteLine("ping");
            
           

        }
    }
}
