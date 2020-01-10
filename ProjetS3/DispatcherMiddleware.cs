using System;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
//Deprecated
namespace ProjetS3
{
    public class DispatcherMiddleware
    {
        private Sender sender;

        private Receiver receiver;

        private RequestDelegate next;

        public DispatcherMiddleware()
        {
            System.Diagnostics.Debug.WriteLine("IS IN CONSTRUCTOR !");

            receiver = new Receiver();
            sender = new Sender();
        }

        public async Task Invoke(HttpContext context)
        {
            System.Diagnostics.Debug.WriteLine("IS IN INVOKE !");
            if (!context.WebSockets.IsWebSocketRequest)
                return;

            await DispatchAsync(context.WebSockets);
        }

        private async Task DispatchAsync(WebSocketManager webSockets)
        {
            WebSocket socket = await webSockets.AcceptWebSocketAsync();
            Task send = Task.Run(async () => await this.sender.SendAsync(socket));
            Task recv = Task.Run(async () => await this.receiver.ReceiveAsync(socket));
            await Task.WhenAll(send, recv);
        }
    }

    public class Sender
    {
        public async Task SendAsync(WebSocket ws)
        {
            try
            {
                while (true)
                {
                    //Send here
                }
            } catch (Exception ex)
            {

            }
        }
    }

    public class Receiver
    {
        public async Task ReceiveAsync(WebSocket ws)
        {
            try
            {
                while (true)
                {
                    //Receive  here (Needed ?)
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}