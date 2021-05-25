using Client.PurchaseService;
using Client.SaleService;
using Client.UserService;
using Microsoft.Extensions.DependencyInjection;

namespace Client
{
    public partial class Startup
    {
        private void ConfigureIoC(IServiceCollection services)
        {
            services.AddHttpClient<UserClient>();
            services.AddHttpClient<PurchaseClient>();
            services.AddHttpClient<SalesClient>();
        }
    }
}