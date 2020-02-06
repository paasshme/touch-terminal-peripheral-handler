using System;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ProjetS3.PeripheralCreation;
using ProjetS3.PeripheralRequestHandler;
using System.Threading.Tasks;

namespace ProjetS3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        private IServiceCollection myservices;
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            this.myservices = services;
            services.AddCors(options => options.AddPolicy("APolicy?", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
            services.AddControllers();
            //services.AddSingleton<PeripheralFactory>();
       /*     services.AddSingleton<DispatcherMiddleware>();
            services.AddSingleton<Sender>();
            services.AddSingleton<Receiver>();*/

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // app.UseMiddleware<DispatcherMiddleware>();

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(10),
                ReceiveBufferSize = 4 * 1024
            };
//            webSocketOptions.AllowedOrigins.Add("*"); Default values allow every origin

            app.UseWebSockets(webSocketOptions);

            app.UseDefaultFiles();


            app.UseStaticFiles();

            //app.Map("/ws", SocketHandler.Map);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //Mieux (mieux):
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=CommunicateToPeripheral}/{id?}");
                    
                /*Moins bien :
                endpoints.MapControllers();
                */
            });


            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/ws")
                {

                    if (context.WebSockets.IsWebSocketRequest)
                    {

                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        var socketFinishedTcs = new TaskCompletionSource<object>();
                        SocketHandler sh = new SocketHandler(webSocket, socketFinishedTcs);
                        PeripheralEventHandler peh = new PeripheralEventHandler(sh);
                        PeripheralFactory.SetHandler(peh);

                        await socketFinishedTcs.Task;
//TODO wip: (fix for websocket error)
                        //var socketFinishedTcs = new TaskCompletionSource<object>();

                        //BackgroundSocketProcessor.AddSocket(socket, socketFinishedTcs);
                        //await socketFinishedTcs.Task;
                        //Test purpose only
                     peh.send("aa", "bb", "cc");
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                    }
                }
                else
                {
                    await next();
                }
            });

        }
        /*
        private async void Start(HttpContext context, WebSocket webSocket)
        {

        }
        
        private async Task Echo(HttpContext context, WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
        */

    }
}
