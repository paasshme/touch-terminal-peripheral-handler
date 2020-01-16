using System;

namespace IDeviceLib
{
    public interface IDevice
    {
        public IPeripheralEventHandler eventHandler {get; set;}
         void Start();

         void Stop();

    }
}