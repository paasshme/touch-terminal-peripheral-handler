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
        private ConcurrentQueue<Event> PeripheralEventsQueue;

        public SocketHandler socketHandler {get; private set;}

        public PeripheralEventHandler(SocketHandler socketHandler)
        {
            System.Diagnostics.Debug.WriteLine("Peh created");
            Console.WriteLine("Peh created");

            this.PeripheralEventsQueue = new ConcurrentQueue<Event>();
            this.socketHandler = socketHandler;
            new Thread(new ThreadStart(QueueListening)).Start();
        }

        public void QueueListening()
        {
            System.Diagnostics.Debug.WriteLine("thread launched");
            System.Console.WriteLine("thread launched");

            while (true)
            {
                if (this.PeripheralEventsQueue.Count != 0)
                {
                    Event FirstTreated;
                    //Récupérer les données du premier event (objectName, eventName, et value) et appeler send
                    if (this.PeripheralEventsQueue.TryPeek(out FirstTreated))
                    {
                        this.send(FirstTreated.ObjectName, FirstTreated.EventName, FirstTreated.Value); 
                        System.Diagnostics.Debug.WriteLine("Remove the sent event from the queue");
                        System.Console.WriteLine("Remove the sent event from the queue");
                        Event dequeued;
                        this.PeripheralEventsQueue.TryDequeue(out dequeued);
                    }
                }
            }
        }

        public async void send(string objectName, string eventName, string value)
        {
            System.Diagnostics.Debug.WriteLine("Trying to send a message :" + objectName + " "  + eventName + " " + value);
            System.Console.WriteLine(objectName + eventName + value);
            byte[] bytes = Encoding.ASCII.GetBytes(""+objectName+" "+eventName+" "+value); //TODO do it in a cleaner way (maybe ?)
            await this.socketHandler.Send(bytes);
        }

        //Called by any device
        public void putPeripheralEventInQueue(string objectName, string eventName, string value)
        {
            System.Diagnostics.Debug.WriteLine("An event has been add in the queue by a device !" + objectName + " " + eventName + " " + value);
            Event newEvent = new Event(objectName, eventName, value);
            Console.WriteLine("True event : " + newEvent.ObjectName +  " " + newEvent.EventName + " " + newEvent.Value);
            this.PeripheralEventsQueue.Enqueue(newEvent);
        }
    }
}
