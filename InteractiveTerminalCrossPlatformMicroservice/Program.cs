using InteractiveTerminalCrossPlatformMicroservice.PeripheralCreation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace InteractiveTerminalCrossPlatformMicroservice
{
    //Entrypoint of ASP.CORE application

    public class Program
    {
        public static void Main(string[] args)
        {
            PeripheralFactory.Init();
            CreateHostBuilder(args).Build().Run();
        }

        // Create the host and use the current Startup
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
