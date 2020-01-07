using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ProjetS3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Thread t = new Thread(new ThreadStart(ThreadProc));
            t.Start();
            //t.Join();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void ThreadProc()
        {
            while(true)
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("ThreadProc: {0}", i);
                    // Yield the rest of the time slice.
                    Thread.Sleep(0);
                }
            }
        }
    }
}
