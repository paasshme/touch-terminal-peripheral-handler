using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ProjetS3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //PeripheralEventHandler p = new PeripheralEventHandler();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
