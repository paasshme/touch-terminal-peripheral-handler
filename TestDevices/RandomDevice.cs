
using System;
using IDeviceLib;

namespace TestDevices
{
    public class RandomDevice : IDevice
    {
        public IPeripheralEventHandler eventHandler {get; set;}
        void IDevice.Start()
        {
            System.Console.WriteLine("Is starting !");

        }

        void IDevice.Stop()
        {
            System.Console.WriteLine("It's stoping dude");

        }
    }
}