using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetS3.PeripheralRequestHandler
{
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
