using System;
using System.Collections.Generic;
using System.Text;
using IDeviceLib;

namespace TestDevices
{
    class DeviceWithMethodParameter : IDeviceWithParameters
    {
        public IPeripheralEventHandler eventHandler { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void MethodWithParameters(string aString)
        {
            Console.WriteLine("WE HAVE RECEIVED A PARAMETER " + aString);
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
