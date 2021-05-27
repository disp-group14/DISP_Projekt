using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace OwnershipService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) {
            // Default cluster port
            var port = 5000;

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") {
                 // Load config file
                // https://dejanstojanovic.net/aspnet/2018/december/setting-up-kestrel-port-in-configuration-file-in-aspnet-core/
                var config = new ConfigurationBuilder()  
                .SetBasePath(Directory.GetCurrentDirectory())  
                .AddJsonFile("appsettings.Development.json", optional: false)  
                .Build();
                port = config.GetValue<int>("gRPCClientPort");
            }
        
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureKestrel(options =>
                    {
                        options.Listen(IPAddress.Any, port, listenOptions =>
                        {
                            listenOptions.Protocols = HttpProtocols.Http2;
                        });
                    });
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
