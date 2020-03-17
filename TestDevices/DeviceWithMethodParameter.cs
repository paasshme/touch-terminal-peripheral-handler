using IDeviceLib;
using System;
using TestDevice.Interfaces;

namespace TestDevices
{
    class DeviceWithMethodParameter : IDeviceWithParameters
    {
        public IPeripheralEventHandler eventHandler { get; set;}

       
        public void MethodWithParameters(string aString)
        {
            Console.WriteLine("WE HAVE RECEIVED A PARAMETER " + aString);
        }

        public void MethodWithTwoParameters(string aString,string bString)
        {
            Console.WriteLine("recieved parameters : \n 1) " + aString + "\n 2) " + bString);
        }


        public void Start()
        {
            Console.WriteLine("Device with method parameter started");
        }

        public void Stop()
        {
            Console.WriteLine("Device with method parameter stoped");

        }
    }
}
