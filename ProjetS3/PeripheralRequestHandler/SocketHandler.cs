using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System;


namespace ProjetS3.PeripheralRequestHandler
{
    public class SocketHandler
    {
        private const int BufferSize = 4096;

        private WebSocket ws;

        private TaskCompletionSource<object> tcs;

        public SocketHandler(WebSocket socket, TaskCompletionSource<object> tcs)
        {
            this.ws = socket;
            this.tcs = tcs;
        }

        public async Task Send(ArraySegment<byte> outg)
        {
            System.Console.WriteLine("[SOCKET STATUS]" + this.ws.State);
            var buffer = new byte[BufferSize];
            var seg = new ArraySegment<byte>(buffer);

            // while (this.ws.State == WebSocketState.Open)
            //{
            var outgoing = new ArraySegment<byte>(buffer, 0, outg.Count);
            System.Console.WriteLine("Sending the thing" + outg.ToString());

            try
            {
                await this.ws.SendAsync(outg, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Websocket doesn't exist anymore");
            }
        }

        public bool GetWebsocketStatus()
        {
            if (this.ws == null)
                return false;
            return true;
        }
    }
}