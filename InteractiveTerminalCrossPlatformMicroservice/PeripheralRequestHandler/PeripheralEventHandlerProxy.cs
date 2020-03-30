using System.Collections.Concurrent;
using PeripheralTools;

namespace InteractiveTerminalCrossPlatformMicroservice.PeripheralRequestHandler
{
    /**
     * Implementation of a PeripheralEventHanlder
     * Add control before enqueueing events
     */
    public class PeripheralEventHandlerProxy : IPeripheralEventHandler
    {

        private ConcurrentQueue<Event> eventQueue;

        private PeripheralEventHandler eventHandler;

        private static PeripheralEventHandlerProxy peripheralEventHandlerProxy = new PeripheralEventHandlerProxy();


        private PeripheralEventHandlerProxy()
        {
            this.eventQueue = new ConcurrentQueue<Event>();
            this.eventHandler = null;
        }

        /**
        *   Singleton for the peripheral event handler proxy
        */
        public static PeripheralEventHandlerProxy GetInstance()
        {
            return peripheralEventHandlerProxy;
        }

        /*
         *  Put event in event queue if the websocket of the event handler is up,
         *  else store the event in a queue until a valid event handler is set
         *  Return true if the event is put in queue, return false in other cases
         */
        public void putPeripheralEventInQueue(string objectName, string eventName, string value)
        {

            if (this.eventHandler == null)
            {
                this.eventQueue.Enqueue(new Event(objectName, eventName, value));
            }
            else
            {
                // In case of crash of the websocket
                if (!this.eventHandler.socketHandler.GetWebsocketStatus())
                {
                    this.eventHandler = null;
                }
                else
                {
                    this.eventHandler.putPeripheralEventInQueue(objectName, eventName, value);
                }
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
            while (!this.eventQueue.IsEmpty)
            {
                Event outEvent;
                this.eventQueue.TryDequeue(out outEvent);
                this.eventHandler.putPeripheralEventInQueue(outEvent.ObjectName, outEvent.EventName, outEvent.Value);
            }
        }

    }
}
