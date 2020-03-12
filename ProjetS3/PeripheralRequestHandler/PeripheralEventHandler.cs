using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using IDeviceLib;
using System.Collections.Concurrent;

namespace ProjetS3.PeripheralRequestHandler
{
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

        public void QueueListening()
        {
            while (true)
            {
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

        public async void send(string objectName, string eventName, string value)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(objectName + SEPARATOR + eventName + SEPARATOR + value); 
            await this.socketHandler.Send(bytes);
        }

        //Called by any device
        public void putPeripheralEventInQueue(string objectName, string eventName, string value)
        {
            Event newEvent = new Event(objectName, eventName, value);
            this.PeripheralEventsQueue.Enqueue(newEvent);
        }
    }
}
