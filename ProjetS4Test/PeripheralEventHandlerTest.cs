using ProjetS3.PeripheralRequestHandler;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProjetS4Test
{
    public class PeripheralEventHandlerTest
    {
    [Fact]
     public void PutEventInQueueTestAsync()
        {
            PeripheralEventHandlerProxy pehp = PeripheralEventHandlerProxy.GetInstance();
            pehp.putPeripheralEventInQueue("testObject", "testEvent", "testValue");
            Assert.Single(pehp.GetEventQueue());
        }
    
    
    }
}
