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

       public SocketHandler (WebSocket socket, TaskCompletionSource<object> tcs)
       {
            this.ws = socket;
            this.tcs = tcs;
       }

        public async Task Send (ArraySegment<byte> outg)
        {
            var buffer = new byte[BufferSize];
            var seg = new ArraySegment<byte>(buffer);

           // while (this.ws.State == WebSocketState.Open)
            //{
                var outgoing = new ArraySegment<byte>(buffer, 0, outg.Count);
                System.Console.WriteLine("Sending the thing" + outg.ToString());

                try {
                    await this.ws.SendAsync(outg, WebSocketMessageType.Text, true, CancellationToken.None);
                }
                catch (Exception e)
                {
                    System.Console.WriteLine("Websocket doesn't exist anymore");
                }
                //await this.ws.SendAsync(outgoing, WebSocketMessageType.Text, true, CancellationToken.None);
        //    }
        }

/*        static async Task Acceptor (HttpContext context, Func<Task> n)
        {
            if (!context.WebSockets.IsWebSocketRequest)
                return;

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var handler = new SocketHandler(socket);
            var buffer = new byte[2];
            buffer[0] = 1;

            await handler.Send(buffer);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "" , CancellationToken.None);

        }

        public static void Map(IApplicationBuilder app)
        {
            app.UseWebSockets();
            app.Use(SocketHandler.Acceptor);
        }
      */
    }
}