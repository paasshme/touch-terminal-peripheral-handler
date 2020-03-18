using IDeviceLib;
using TestDevice.Interfaces;

namespace TestDevices
{
    //Test purpose only
    class Program
    {
        public static void Main()
        {

            IDevice barCode = new BarCodePOC("/dev/ttyACM1");
            barCode.eventHandler = new FakeEventHandler();

            barCode.Start();
            while (true);
        }
        
    }
}