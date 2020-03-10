using IDeviceLib;
using System;
using System.Collections.Generic;
using System.Text;


namespace TestDevices
{
    public class DeviceWithMethodParameter : IDeviceWithParameters
    {
        public IPeripheralEventHandler eventHandler { get; set;}

       
        void IDeviceWithParameters.MethodWithParameters(string aString)
        {
            Console.WriteLine("WE HAVE RECEIVED A PARAMETER " + aString);
        }

        void IDevice.Start()
        {
            Console.WriteLine("Device with method parameter started");
        }

        void IDevice.Stop()
        {
            throw new NotImplementedException();
        }
    }
}
