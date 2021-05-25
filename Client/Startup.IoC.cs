using System;
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
            services.AddHttpClient<UserClient>(Client => Client.BaseAddress = Configuration.GetValue<Uri>("UserServiceUri"));
            services.AddHttpClient<PurchaseClient>(Client => Client.BaseAddress = Configuration.GetValue<Uri>("PurchaseServiceUri"));
            services.AddHttpClient<SalesClient>(Client => Client.BaseAddress = Configuration.GetValue<Uri>("SalesServiceUri"));
            services.AddHttpClient<StockClient>(Client => Client.BaseAddress = Configuration.GetValue<Uri>("StockServiceUri"));
        }
    }
}