using PeripheralTools;
using System;
using TestDevice.Interfaces;

namespace TestDevices
{
    class AwesomeObject : ITest
    {
        public IPeripheralEventHandler eventHandler { get; set; }

        public void Start()
        {
            Console.WriteLine("[AwesomeObject] Start");

        }

       public void Stop()
       {
           Console.WriteLine("[AwesomeObject] Stop");
       }

       public void Test()
       {
           Console.WriteLine("[AwesomeObject] test");
       }
    }
}
