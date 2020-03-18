using System;
using Microsoft.OpenApi.Models;
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

using ProjetS3.SwaggerCustom;
namespace ProjetS3
{
    public class Startup
    {
        public static TaskCompletionSource<object> tcs;
        public static WebSocket ws;
        private const string WEBSOCKET_URL = "/ws";
        public IConfiguration Configuration { get; }

        private IServiceCollection myservices;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

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

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.DocumentFilter<CustomMethodIntrospection>();
            });
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

            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(10),
                ReceiveBufferSize = 4 * 1024
            };

            app.UseWebSockets(webSocketOptions);

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=CommunicateToPeripheral}/{id?}");
                    
            });


            app.Use(async (context, next) =>
            {
                if (context.Request.Path == WEBSOCKET_URL)
                {
                    if (context.WebSockets.IsWebSocketRequest)
                    {

                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        ws = webSocket;
                        TaskCompletionSource<object> socketFinishedTcs = new TaskCompletionSource<object>();
                        tcs = socketFinishedTcs;
                        SocketHandler socketHandler = new SocketHandler(webSocket, socketFinishedTcs);
                        PeripheralEventHandler peripheralEventHandler = new PeripheralEventHandler(socketHandler);
                        PeripheralFactory.SetHandler(peripheralEventHandler);

                        await socketFinishedTcs.Task;
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

            app.UseSwagger();


            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Test projet IUT IPM France");
            });
            PeripheralFactory.Init();
        }
    }
}
