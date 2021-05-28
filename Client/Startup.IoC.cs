using System;
using System.Net.Http;
using Client.PurchaseService;
using Client.SaleService;
using Client.StockService;
using Client.UserService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Client
{
    public partial class Startup
    {
        private void ConfigureIoC(IServiceCollection services)
        {
            // UserClient
            services.AddHttpClient<UserClient>(Client => Client.BaseAddress = Configuration.GetValue<Uri>("UserServiceHttp1Uri")).ConfigurePrimaryHttpMessageHandler(() => {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            return clientHandler;                
            });

            // PurchaseClient
            services.AddHttpClient<PurchaseClient>(Client => Client.BaseAddress = Configuration.GetValue<Uri>("PurchaseServiceHttp1Uri")).ConfigurePrimaryHttpMessageHandler(() => {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            return clientHandler;                
            });

            // SalesClient
            services.AddHttpClient<SalesClient>(Client => Client.BaseAddress = Configuration.GetValue<Uri>("SalesServiceHttp1Uri")).ConfigurePrimaryHttpMessageHandler(() => {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            return clientHandler;                
            });

            // StockClient
            services.AddHttpClient<StockClient>(Client => Client.BaseAddress = Configuration.GetValue<Uri>("StockServiceUri")).ConfigurePrimaryHttpMessageHandler(() => {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            return clientHandler;                
            });
        }
    }
}