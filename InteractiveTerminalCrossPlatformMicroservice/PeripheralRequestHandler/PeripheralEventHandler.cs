using System.Text;
using System.Threading;
using System.Collections.Concurrent;
using PeripheralTools;

namespace InteractiveTerminalCrossPlatformMicroservice.PeripheralRequestHandler
{
    /// <summary>
    /// A PeripheralEventHandler is used by peripheral to interact with the application
    /// </summary>

    public class PeripheralEventHandler : IPeripheralEventHandler
    {
        /// <summary>
        /// Separator used in the websocket in order to separate each element (objectName, eventName, info) 
        /// </summary>
        private const string SEPARATOR = " ";

        /// <summary>
        ///  Events thread safe queue 
        /// </summary>
        private ConcurrentQueue<Event> PeripheralEventsQueue;

        /// <summary>
        /// Encapsulation of a socket
        /// </summary>
        public SocketHandler socketHandler {get; private set;}

        public PeripheralEventHandler(SocketHandler socketHandler)
        {
            this.PeripheralEventsQueue = new ConcurrentQueue<Event>();
            this.socketHandler = socketHandler;
            new Thread(new ThreadStart(QueueListening)).Start();
        }

        /// <summary>
        /// Watch the event queue and handle events in queue
        /// </summary>
        private void QueueListening()
        {
            while (true)
            {
                //If there is an event in the queue
                if (this.PeripheralEventsQueue.Count != 0)
                {
                    Event FirstTreated;
                    // Get data of first event(objectName, eventName, et value) and then send it
                    if (this.PeripheralEventsQueue.TryPeek(out FirstTreated))
                    {
                        this.Send(FirstTreated.ObjectName, FirstTreated.EventName, FirstTreated.Value); 
                        Event dequeued;
                        this.PeripheralEventsQueue.TryDequeue(out dequeued);
                    }
                }
            }
        }

        /// <summary>
        /// Send event information to the socketHandler
        /// </summary>
        /// <param name="objectName"> The name of the object that sent this event</param>
        /// <param name="eventName">The name of the event sent by the object</param>
        /// <param name="value"> Value of the event sent by the object </param>
        public async void Send(string objectName, string eventName, string value)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(objectName + SEPARATOR + eventName + SEPARATOR + value); 
            await this.socketHandler.Send(bytes);
        }

        /// <summary>
        /// Enqueue an event. Called by any devices
        /// </summary>
        /// <param name="objectName"> The name of the object that sent this event</param>
        /// <param name="eventName">The name of the event sent by the object</param>
        /// <param name="value"> Value of the event sent by the object </param>
        public void PutPeripheralEventInQueue(string objectName, string eventName, string value)
        {
            Event newEvent = new Event(objectName, eventName, value);
            this.PeripheralEventsQueue.Enqueue(newEvent);
        }
    }
}
