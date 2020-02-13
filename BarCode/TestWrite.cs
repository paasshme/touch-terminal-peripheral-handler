using System;
using System.IO.Ports;

namespace BarCode
{
    class TestWrite 
    {
        static void Main(string[] args)
        {
            SerialPort sp = new SerialPort("/dev/ttyACM0",9600,0,8,StopBits.One);
            SerialPort sp1 = new SerialPort("/dev/ttyACM1",9600,0,8,StopBits.One);
            SerialPort sp2 = new SerialPort("/dev/tty8",9600,0,8,StopBits.One);
            sp.Open();
            sp1.Open();
            //sp2.Open();
            sp.Write("2");
            //sp2.Write("2");
            sp1.Write("2");
            sp.Write("1");
            sp.Write("1");
            sp.Write("1");
            sp.Write("1");
            sp.Write("1");
            sp.Write("2");
            sp.WriteLine("1\n");
            sp.Write("\n");
            System.Console.WriteLine("ping");
        }
    }
}
