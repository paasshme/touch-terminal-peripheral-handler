using System;
using IDeviceLib;

namespace TestDevices
{
    public class RandomDevice : IDevice
    {
        public IPeripheralEventHandler eventHandler {get; set;}

        void IDevice.Start()
        {
            System.Console.WriteLine("[Start] Event preparing");
            this.eventHandler.putPeripheralEventInQueue("start", "start", "start");
            System.Console.WriteLine("Event added!");

        }

        void IDevice.Stop()
        {
            System.Console.WriteLine("[Stop] Event preparing...");
            this.eventHandler.putPeripheralEventInQueue("stop", "stop", "stop");
            System.Console.WriteLine("Event added!");
        }

        
    }
}