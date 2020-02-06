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

        private ConcurrentQueue<String[]> eventQueue;

        private PeripheralEventHandler eventHandler;

        private static PeripheralEventHandlerProxy pehp = null;

        private PeripheralEventHandlerProxy()
        {
            this.eventQueue = new ConcurrentQueue<string[]>();
            this.eventHandler = null;
        }

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
            System.Console.WriteLine("HERE THE PROXY YAY");
            System.Console.WriteLine(this.eventHandler);
           /* if (!this.eventHandler.socketHandler.GetWebsocketStatus())
            {
                System.Console.WriteLine("1");
                this.eventHandler = null;
            }*/

            if (this.eventHandler == null)
            {
                System.Console.WriteLine("[PROXY] The event handler is placed in a waiting queue...");
                this.eventQueue.Enqueue(new string[3] { objectName, eventName, value });
            }
            else
            {
                System.Console.WriteLine("[PROXY] everything went fine");
                this.eventHandler.putPeripheralEventInQueue(objectName, eventName, value);
            }
            System.Console.WriteLine("out of the fake");
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
