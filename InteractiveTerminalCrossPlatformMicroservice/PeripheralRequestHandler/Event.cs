namespace InteractiveTerminalCrossPlatformMicroservice.PeripheralRequestHandler
{

    /// <summary>
    ///  An Event is the representation of a message in the application
    /// </summary>
    public class Event
    {
        /// <summary>
        /// The name of the object that sent this event
        /// </summary>
        public string ObjectName { get; private set; }

        /// <summary>
        /// The name of the event sent by the object
        /// </summary>
        public string EventName { get; private set; }

        /// <summary>
        ///  Value of the event sent by the object
        /// </summary>
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
