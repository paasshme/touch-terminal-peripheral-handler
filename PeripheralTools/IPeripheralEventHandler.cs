namespace PeripheralTools
{
    /**
        IPeripheralEventHandler represent an handler for any kind of devices.
        Every IDevice own one EventHandler and they are able to send their message
        by placing them in a queue with the following method.
        Hence, the event/message will be handled by the microservice.

    */
    public interface IPeripheralEventHandler
    {
        void putPeripheralEventInQueue(string objectName, string eventName, string value);
    }
}
