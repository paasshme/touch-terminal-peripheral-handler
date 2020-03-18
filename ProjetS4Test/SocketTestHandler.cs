using ProjetS3.PeripheralRequestHandler;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjetS4Test
{
    class SocketTestHandler : SocketHandler
    {
        private WebSocket websocket;
        public SocketTestHandler(WebSocket socket, TaskCompletionSource<object> task) : base(socket, task)
        {

        }
        public async Task Send(ArraySegment<byte> toSendData)
        {
            try
            {
                await this.websocket.SendAsync(toSendData, WebSocketMessageType.Text, true, CancellationToken.None);
                CommunicationTest.status.setSocketHandlerStatus(true);
            }

            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    Console.Error.WriteLine("Webscoket isn't connected yet");
                }
                if (ex is ObjectDisposedException)
                {
                    Console.Error.WriteLine("Websocket doesn't exist anymore");
                }
            }

        }
    }
}
