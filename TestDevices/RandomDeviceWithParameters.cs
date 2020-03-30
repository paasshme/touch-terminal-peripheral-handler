using System;
using PeripheralTools;

namespace TestDevices
{
    public class RandomDeviceWithParameters : IDevice
    {
        public String stringTest = "";
        public int intTest = 1;
        public Boolean boolTest = false;

        public RandomDeviceWithParameters(String sT,int iT,bool bT)
        {
            this.stringTest = sT;
            this.intTest = iT;
            this.boolTest = bT;
        }
        
        public IPeripheralEventHandler eventHandler { get; set; }
        void IDevice.Start()
        {
            Console.WriteLine(this.stringTest + this.intTest + this.boolTest);
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