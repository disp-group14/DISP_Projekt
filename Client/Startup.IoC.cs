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
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            services.AddHttpClient<UserClient>(Client => Client.BaseAddress = Configuration.GetValue<Uri>("UserServiceUri")).ConfigurePrimaryHttpMessageHandler(() => clientHandler);
            services.AddHttpClient<PurchaseClient>(Client => Client.BaseAddress = Configuration.GetValue<Uri>("PurchaseServiceUri")).ConfigurePrimaryHttpMessageHandler(() => clientHandler);
            services.AddHttpClient<SalesClient>(Client => Client.BaseAddress = Configuration.GetValue<Uri>("SalesServiceUri")).ConfigurePrimaryHttpMessageHandler(() => clientHandler);
            services.AddHttpClient<StockClient>(Client => Client.BaseAddress = Configuration.GetValue<Uri>("StockServiceUri")).ConfigurePrimaryHttpMessageHandler(() => clientHandler);
        }
    }
}