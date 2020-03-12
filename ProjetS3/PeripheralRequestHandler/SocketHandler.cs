using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System;


namespace ProjetS3.PeripheralRequestHandler
{
    public class SocketHandler
    {
        private const int BufferSize = 4096;

        private WebSocket websocket;

        private TaskCompletionSource<object> taskCompletionSource;

        public SocketHandler(WebSocket socket, TaskCompletionSource<object> task)
        {
            this.websocket = socket;
            this.taskCompletionSource = task;
        }

        public async Task Send(ArraySegment<byte> toSendData)
        {
            try
            {
                await this.websocket.SendAsync(toSendData, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Websocket doesn't exist anymore");
            }
        }

        public bool GetWebsocketStatus()
        {
            return this.websocket == null;
        }
    }
}