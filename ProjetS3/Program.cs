using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace ProjetS3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //PeripheralEventHandler p = new PeripheralEventHandler();
            //try {

                CreateHostBuilder(args).Build().Run();
            /*}
            catch (Exception e)
            {
                System.Console.WriteLine("Stoping...");
            }*/
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
