
using System;
using IDeviceLib;

namespace TestDevices
{
    class RandomDevice : IDevice
    {
        public IPeripheralEventHandler eventHandler {get; set;}
        public void IDevice.Start()
        {

        }

        public void IDevice.Stop()
        {

        }
    }
}