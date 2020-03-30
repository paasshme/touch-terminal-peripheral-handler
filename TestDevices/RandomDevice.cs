using PeripheralTools;

namespace TestDevices
{
    public class RandomDevice : IDevice
    {
        public IPeripheralEventHandler eventHandler {get; set;}

        public void Start()
        {
            this.eventHandler.putPeripheralEventInQueue("start", "startEvent", "3");
        }

        public void Stop()
        {
            this.eventHandler.putPeripheralEventInQueue("stop", "stopEvent", "4");
        }
    }
}


