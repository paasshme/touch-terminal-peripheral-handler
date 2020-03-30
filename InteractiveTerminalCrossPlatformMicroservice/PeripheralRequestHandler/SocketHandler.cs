using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace InteractiveTerminalCrossPlatformMicroservice.PeripheralRequestHandler
{

    /// <summary>
    /// An encapsulation for a WebSocket simplifying the use and management of this socket
    /// </summary>
    public class SocketHandler 
    {
        private WebSocket websocket;

        // A task used to keep alive the websocket after the end of the pipeline
        private TaskCompletionSource<object> taskCompletionSource;

        public SocketHandler(WebSocket socket, TaskCompletionSource<object> task)
        {
            this.websocket = socket;
            this.taskCompletionSource = task;
        }

        /// <summary>
        ///Send into the websocket 
        /// </summary>
        /// <param name="toSendData">An ArraySegment of byte representing the data through the websocket</param>
        /// <returns>A Task representing the state of the job</returns>
        public async Task Send(ArraySegment<byte> toSendData)
        {
            try
            {
                await this.websocket.SendAsync(toSendData, WebSocketMessageType.Text, true, CancellationToken.None);
            }
            
            catch (Exception ex)
            {
                if(ex is InvalidOperationException)
                {
                    Console.Error.WriteLine("Webscoket isn't connected yet");
                }
                if(ex is ObjectDisposedException)
                { 
                    Console.Error.WriteLine("Websocket doesn't exist anymore");
                }
            }
            
        }

        /// <summary>
        /// Used by the proxy to check the state of the websocket before sending 
        /// </summary>
        /// <returns> If the websocket is ready or not </returns>
        public bool GetWebsocketStatus()
        {
            return this.websocket != null;
        }
    }
}