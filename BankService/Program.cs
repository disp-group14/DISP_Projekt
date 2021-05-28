using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BankService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {   
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        Console.WriteLine("Ownership Service now serves HTTP 2 at: " + Int32.Parse(Environment.GetEnvironmentVariable("gRPCClientPort")));
                        options.Listen(IPAddress.Any, Int32.Parse(Environment.GetEnvironmentVariable("gRPCClientPort")), listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http2;
                        });

                    });
                    webBuilder.UseStartup<Startup>();
                });
        }
    }       
}
