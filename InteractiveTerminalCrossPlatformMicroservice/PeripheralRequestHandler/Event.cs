namespace InteractiveTerminalCrossPlatformMicroservice.PeripheralRequestHandler
{
    /**
     * An Event is the representation of a message in the application
     * It contains the object (peripheral) concerned, an event (method) and a value (parameter).
     */
    public class Event
    {
        public string ObjectName { get; private set; }
        public string EventName { get; private set; }
        public string Value { get; private set; }

        public Event(string peripheral,string method, string value)
        {
            this.ObjectName = peripheral;
            this.EventName = method;
            this.Value = value;
        }

        public Event()
        {
            this.ObjectName = "";
            this.EventName = "";
            this.Value = "";
        }

    }
}
