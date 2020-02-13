using System;
using System.Collections.Generic;
using System.Text;
using IDeviceLib;

namespace TestDevices
{

    //testing purposes
    public class RandomCalculatorDevice : ICalculator
    {
        public IPeripheralEventHandler eventHandler { get; set; }

        void ICalculator.add(int a, int b)
        {
            System.Console.WriteLine("[Start] Event preparing");
            this.eventHandler.putPeripheralEventInQueue("add", "add", "add");
            System.Console.WriteLine("Event added!");       
        }

        void IDevice.Start()
        {
            System.Console.WriteLine("[Start] Event preparing");
            this.eventHandler.putPeripheralEventInQueue("start", "start", "start");
            System.Console.WriteLine("Event added!");
        }

        void IDevice.Stop()
        {
            System.Console.WriteLine("[Start] Event preparing");
            this.eventHandler.putPeripheralEventInQueue("stop", "stop", "stop");
            System.Console.WriteLine("Event added!");
        }
    }
}
