using IDeviceLib;
using ProjetS3;
using ProjetS3.PeripheralRequestHandler;
using System.Collections.Concurrent;

namespace ProjetS4Test
{
    //Proxy used for test only in communicationTest class
    class EventHandlerTestProxy : PeripheralEventHandler
    {
        
        
        private ConcurrentQueue<Event> eventQueue;

        private PeripheralEventHandler eventHandler;

        private static EventHandlerTestProxy eventHandlerTestProxy = new EventHandlerTestProxy(new SocketTestHandler(Startup.ws, Startup.tcs));

        public EventHandlerTestProxy(SocketHandler socketHandler) : base(socketHandler)
        {
        }



        /**
        *   Singleton for the peripheral event handler proxy
        */
        public static EventHandlerTestProxy GetInstance()
        {
            return eventHandlerTestProxy;
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
                    CommunicationTest.status.setProxyStatus(true);
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

