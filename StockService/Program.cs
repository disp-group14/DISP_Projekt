using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

namespace StockService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        Console.WriteLine("Stock Service now serves HTTP 1 at: " + Int32.Parse(Environment.GetEnvironmentVariable("HTTP2PORT")));
                        options.Listen(IPAddress.Any, Int32.Parse(Environment.GetEnvironmentVariable("HTTP2PORT")), listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http1;
                        });
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
