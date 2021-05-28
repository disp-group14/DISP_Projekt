using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

namespace TaxService
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
                        Console.WriteLine("Tax Service now serves HTTP 2 at: " + Int32.Parse(Environment.GetEnvironmentVariable("HTTP2PORT")));
                        options.Listen(IPAddress.Any, Int32.Parse(Environment.GetEnvironmentVariable("HTTP2PORT")), listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http2;
                        });

                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
