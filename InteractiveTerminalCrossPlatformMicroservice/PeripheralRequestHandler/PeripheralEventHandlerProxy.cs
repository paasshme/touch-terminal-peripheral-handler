using System.Collections.Concurrent;
using PeripheralTools;

namespace InteractiveTerminalCrossPlatformMicroservice.PeripheralRequestHandler
{
    
    /// <summary>
    /// Implementation of a PeripheralEventHanlder
    /// Add control before enqueueing events
    /// Allow the user to use method before and during the WebSocket initialisation (without loosing those events by storing them)
    /// </summary>
    
    public class PeripheralEventHandlerProxy : IPeripheralEventHandler
    {

        /// <summary>
        /// Queue that stores the non treated event yet 
        /// </summary>
        private ConcurrentQueue<Event> eventQueue;

        private PeripheralEventHandler eventHandler;

        /// <summary>
        /// Singleton in order to have only one proxy for the whole Application
        /// </summary>
        private static PeripheralEventHandlerProxy peripheralEventHandlerProxy = new PeripheralEventHandlerProxy();


        private PeripheralEventHandlerProxy()
        {
            this.eventQueue = new ConcurrentQueue<Event>();
            this.eventHandler = null;
        }

        /// <summary>
        /// Get an instance of the peripheral event handler proxy
        /// </summary>
        /// <returns></returns>

        public static PeripheralEventHandlerProxy GetInstance()
        {
            return peripheralEventHandlerProxy;
        }

        /*
         *  
         */

        /// <summary>
        /// Put event in event queue if the websocket of the event handler is up,
        /// else store the event in a queue until a valid event handler is set
        /// Return true if the event is put in queue, return false in other cases
        /// </summary>
        /// <param name="objectName"> The name of the object that sent this event</param>
        /// <param name="eventName">The name of the event sent by the object</param>
        /// <param name="value"> Value of the event sent by the object </param>
        public void PutPeripheralEventInQueue(string objectName, string eventName, string value)
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
                    this.eventHandler.PutPeripheralEventInQueue(objectName, eventName, value);
                }
            }
        }

        /// <summary>
        /// Set an eventHandler to the proxy
        /// It will empty the queue of events in order to treat them in the given PeripheralEventHandler
        /// </summary>
        /// <param name="eventHandler"></param>
        public void SetEventHandler(PeripheralEventHandler eventHandler)
        {
            this.eventHandler = eventHandler;
            CallEventInQueue();
        }

        /// <summary>
        /// Treat every events in enventQueue in putPeripheralEventInQueue method and empty the queue.
        /// </summary>
        private void CallEventInQueue()
        {
            while (!this.eventQueue.IsEmpty)
            {
                Event outEvent;
                this.eventQueue.TryDequeue(out outEvent);
                this.eventHandler.PutPeripheralEventInQueue(outEvent.ObjectName, outEvent.EventName, outEvent.Value);
            }
        }

    }
}
