using Microsoft.Extensions.DependencyInjection;
using OwnershipService.DAL;

namespace OwnershipService
{
    public partial class Startup
    {
        private void ConfigureIoC(IServiceCollection services)
        {
            // Data managers
            services.AddTransient<IShareHolderDataManager, ShareHolderDataManager>();
            services.AddTransient<IShareDataManager, ShareDataManager>();
            
        }
    }
}