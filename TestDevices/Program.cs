using System;
using IDeviceLib;

namespace TestDevices
{
    class Program
    {
        public static void Main()
        {
            IDevice i = new RandomDevice();
            i.Start();
            i.Stop();
            IScanner i2 = new RandomScannerDevice();
            i2.Foo();

            IDevice barCode = new BarCodePOC();
            barCode.Start();
            barCode.Stop();
        }
    }
}