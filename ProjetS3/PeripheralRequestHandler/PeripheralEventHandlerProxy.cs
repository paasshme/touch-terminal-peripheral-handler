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

        //Todo: changer en queue d'event
        private ConcurrentQueue<Event> eventQueue;

        private PeripheralEventHandler eventHandler;

        private static PeripheralEventHandlerProxy pehp = null;

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
            if (pehp == null)
            {
                pehp = new PeripheralEventHandlerProxy();
            }
            return pehp;
        }

        /*
         *  Put event in event queue if the websocket of the event handler is up,
         *  else store the event in a queue until a valid event handler is set
         */
        public void putPeripheralEventInQueue(string objectName, string eventName, string value)
        {
            //System.Console.WriteLine("HERE THE PROXY YAY");
            //System.Console.WriteLine(this.eventHandler);

            Console.WriteLine("HEyyyyyyyyyyy proxyyyyyyyyyy " + objectName + "" + eventName +"" + value );

            if (this.eventHandler == null)
            {
                System.Console.WriteLine("[PROXY] The event handler is placed in a waiting queue...");
                this.eventQueue.Enqueue(new Event(objectName, eventName, value ));
            }
            else
            {
                //Hypothetic case
                if (!this.eventHandler.socketHandler.GetWebsocketStatus())
                {
                    System.Console.WriteLine("[PROXY] The websocket crashed !");
                    this.eventHandler = null;
                }
                else {
                    System.Console.WriteLine("[PROXY] Calling the event handler");
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
            System.Console.WriteLine("[PROXY] Emptying the queue...");
            while(!this.eventQueue.IsEmpty)
            {
                Event outEvent;
                this.eventQueue.TryDequeue(out outEvent);
                this.eventHandler.putPeripheralEventInQueue(outEvent.ObjectName, outEvent.EventName, outEvent.Value);
                

            }
        }
        // Test purpose only
        public ConcurrentQueue<Event> GetEventQueue()
        {
            return this.eventQueue;
        }
    }
}
