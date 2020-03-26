using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using IDeviceLib;
using System.Collections.Concurrent;

namespace ProjetS3.PeripheralRequestHandler
{
    /**
     * A PeripheralEventHandler is used by peripheral to interract with the application
     */
    public class PeripheralEventHandler : IPeripheralEventHandler
    {
        private const string SEPARATOR = " ";
       
        private ConcurrentQueue<Event> PeripheralEventsQueue;

        public SocketHandler socketHandler {get; private set;}

        public PeripheralEventHandler(SocketHandler socketHandler)
        {
            this.PeripheralEventsQueue = new ConcurrentQueue<Event>();
            this.socketHandler = socketHandler;
            new Thread(new ThreadStart(QueueListening)).Start();
        }

        //Watch the event queue and handle events ins queue
        public void QueueListening()
        {
            while (true)
            {
                //If there is an event in the queue
                if (this.PeripheralEventsQueue.Count != 0)
                {
                    Event FirstTreated;
                    //Récupérer les données du premier event (objectName, eventName, et value) et appeler send
                    if (this.PeripheralEventsQueue.TryPeek(out FirstTreated))
                    {
                        this.send(FirstTreated.ObjectName, FirstTreated.EventName, FirstTreated.Value); 
                        Event dequeued;
                        this.PeripheralEventsQueue.TryDequeue(out dequeued);
                    }
                }
            }
        }

        //Send event information to the socketHandler
        public async void send(string objectName, string eventName, string value)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(objectName + SEPARATOR + eventName + SEPARATOR + value); 
            await this.socketHandler.Send(bytes);
        }

        //Enqueu an event. Called by any devices
        public void putPeripheralEventInQueue(string objectName, string eventName, string value)
        {
            Event newEvent = new Event(objectName, eventName, value);
            this.PeripheralEventsQueue.Enqueue(newEvent);
        }
    }
}
