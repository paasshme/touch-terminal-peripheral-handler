using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using IDeviceLib;

namespace ProjetS3.PeripheralRequestHandler
{
    public class PeripheralEventHandler : IPeripheralEventHandler
    {
        private Queue<string> PeripheralEventsQueue;


        private SocketHandler socketHandler;


        public PeripheralEventHandler(SocketHandler socketHandler)
        {
            System.Diagnostics.Debug.WriteLine("Peh created");
            Console.WriteLine("Peh created");

            this.PeripheralEventsQueue = new Queue<string>();
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
                    //Récupérer les données du premier       event (objectName, eventName, et value) et appeler send
                    string FirstTreated = this.PeripheralEventsQueue.Peek();
                    this.send(FirstTreated, FirstTreated, FirstTreated); //TODO change that way of sending
                    System.Diagnostics.Debug.WriteLine("Remove the sent event from the queue");
                    System.Console.WriteLine("Remove the sent event from the queue");
                    this.PeripheralEventsQueue.Dequeue();
                }
            }
        }

        public async void send(string objectName, string eventName, string value)
        {
            System.Diagnostics.Debug.WriteLine("Trying to send a message :" + objectName + " "  + eventName + " " + value);
            System.Console.WriteLine(objectName + eventName + value);
            byte[] bytes = Encoding.ASCII.GetBytes(objectName + eventName + value); //TODO do it in a cleaner way
            
            await this.socketHandler.Send(bytes);//Not working because of CORS ? 
            /*
              ObjectDisposedException : Cannot write to the response body, the response has completed.  
              Object name: 'HttpResponseStream'.
            */
 
        }

        //Called by any device
        public void putPeripheralEventInQueue(string objectName, string eventName, string value)
        {
            System.Diagnostics.Debug.WriteLine("An event has been add in the queue by a device !" + objectName + " " + eventName + " " + value);
            this.PeripheralEventsQueue.Enqueue(objectName);
            //throw new NotImplementedException();
        }
        /*
        public void putPeripheralEventInQueue(IEvent peripheralEvent)
        {
            this.PeripheralEventsQueue.Enqueue(peripheralEvent);
        }*/

        /*
         * Comment je connais le client (fichier de config ?) 
         * Est ce que je peux avoir plusieurs clients ? ( A priori non )
         */
    }

}
