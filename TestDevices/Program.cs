

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
        }
    }
}