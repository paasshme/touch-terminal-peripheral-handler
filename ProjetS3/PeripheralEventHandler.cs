using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjetS3
{
    public class PeripheralEventHandler
    {
        public Queue<IEvent> PeripheralEventsQueue;

        public Thread EventQueueListener;

        public PeripheralEventHandler()
        {
            this.PeripheralEventsQueue = new Queue<IEvent>();
            
        }

       
    }

}
