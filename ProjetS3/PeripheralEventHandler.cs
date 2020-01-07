using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace ProjetS3
{
    public class PeripheralEventHandler : IEventHandler
    {
        public Queue<IEvent> PeripheralEventsQueue;

        public Thread EventQueueListener;


        public PeripheralEventHandler()
        {
            this.PeripheralEventsQueue = new Queue<IEvent>();
            this.EventQueueListener = new Thread(new ThreadStart(QueueListening));
        }

        public void QueueListening()
        {
            while (true)
            {
                if (this.PeripheralEventsQueue.Count != 0)
                {
                    //Récupérer les données du premier event (objectName, eventName, et value) et appeler send
                    IEvent FirstTreated = this.PeripheralEventsQueue.Peek();
                    this.send(FirstTreated.objectName, FirstTreated.eventName, (string)FirstTreated.value);
                    this.PeripheralEventsQueue.Dequeue();
                }
            }
        }

        public void send(string objectName, string eventName, string value)
        {
            //Ecrire dans le websocket
        }

        public void connectWebSocket()
        {
      
        }

        /*
         * Comment je connais le client (fichier de config ?) 
         * Est ce que je peux avoir plusieurs clients ? ( A priori non )
         */
    }

}
