using PeripheralTools;

namespace TestDevices
{
    public class RandomDevice : IDevice
    {
        public IPeripheralEventHandler eventHandler {get; set;}

        public void Start()
        {
            this.eventHandler.PutPeripheralEventInQueue("start", "startEvent", "3");
        }

        public void Stop()
        {
            this.eventHandler.PutPeripheralEventInQueue("stop", "stopEvent", "4");
        }
    }
}


