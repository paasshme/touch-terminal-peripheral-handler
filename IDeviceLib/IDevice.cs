using System;

namespace IDeviceLib
{
    public interface IDevice
    {
         IPeripheralEventHandler eventHandler {get; set;}
         void Start();

         void Stop();

    }
}