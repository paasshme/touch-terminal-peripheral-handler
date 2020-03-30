using System.Collections.Concurrent;
using PeripheralTools;

namespace InteractiveTerminalCrossPlatformMicroservice.PeripheralRequestHandler
{
    /**
     * Implementation of a PeripheralEventHanlder
     * Add control before enqueueing events
     * Allow the user to use method before and during the WebSocket initialisation (without loosing those events by storing them)
     */
    public class PeripheralEventHandlerProxy : IPeripheralEventHandler
    {

        // Store the non yet treated event (because of the lack of WebSocket)
        private ConcurrentQueue<Event> eventQueue;

        private PeripheralEventHandler eventHandler;

        // Singleton in order to have only one proxy for the whole Application

        private static PeripheralEventHandlerProxy peripheralEventHandlerProxy = new PeripheralEventHandlerProxy();


        private PeripheralEventHandlerProxy()
        {
            this.eventQueue = new ConcurrentQueue<Event>();
            this.eventHandler = null;
        }

        /**
        *   Get an instance of the peripheral event handler proxy
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
                // This will relaunch the waiting and storing queue
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

        // Set an eventHandler to the proxy
        // It will empty the queue of events in order to treat them in the given PeripheralEventHandler
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
