using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using IDeviceLib;

namespace ProjetS3.PeripheralRequestHandler
{
    public class PeripheralEventHandlerProxy : IPeripheralEventHandler
    {

        ConcurrentQueue<String[]> eventQueue;

        PeripheralEventHandler eventHandler;

        public PeripheralEventHandlerProxy()
        {
            this.eventQueue = new ConcurrentQueue<string[]>();
            this.eventHandler = null;
        }

        /*
         *  Put event in event queue if the websocket of the event handler is up,
         *  else store the event in a queue until a valid event handler is set
         */
        public void putPeripheralEventInQueue(string objectName, string eventName, string value)
        {
            if (!this.eventHandler.GetSocketStatus())
            {
                this.eventHandler = null;
            }

            if (this.eventHandler == null)
            {
                this.eventQueue.Enqueue(new string[3] { objectName, eventName, value });
            }
            else
            {
                this.eventHandler.putPeripheralEventInQueue(objectName, eventName, value);
            }
        }

        public void SetEventHandler(PeripheralEventHandler eventHandler)
        {
            this.eventHandler = eventHandler;
            CallEventInQueue();
        }

        /*
         * Call all events in enventQueue in putPeripheralEventInQueue method and empty the queue.
         */
        private void CallEventInQueue()
        {
            while(!this.eventQueue.IsEmpty)
            {
                string[] outEvent;
                this.eventQueue.TryDequeue(out outEvent);
                this.eventHandler.putPeripheralEventInQueue(outEvent[0], outEvent[1], outEvent[2]);
            }
        }
        // Test purpose only
        public ConcurrentQueue<String[]> GetEventQueue()
        {
            return this.eventQueue;
        }
    }
}
