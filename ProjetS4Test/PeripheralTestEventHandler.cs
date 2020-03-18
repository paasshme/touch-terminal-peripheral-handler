using ProjetS3.PeripheralRequestHandler;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetS4Test
{
     class PeripheralTestEventHandler : PeripheralEventHandler
    {
        private const string SEPARATOR = " ";
        public PeripheralTestEventHandler(SocketHandler socketHandler) : base(socketHandler)
        {
        }
        public async void send(string objectName, string eventName, string value)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(objectName + SEPARATOR + eventName + SEPARATOR + value);
            await this.socketHandler.Send(bytes);
            CommunicationTest.status.setEventHandlerStatus(true);
        }
    }
}
