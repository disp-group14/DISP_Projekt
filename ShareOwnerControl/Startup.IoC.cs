using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShareOwnerControl.BLL;
using ShareOwnerControl.DAL;
using ShareOwnerControl.DAL.Context;
using ShareOwnerControl.Protos;

namespace ShareOwnerControl
{
    public partial class Startup
    {
        public void ConfigureIoC(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(_ => configuration);
            string connectionString = configuration.GetValue<string>("DBConnection");
            services.AddDbContext<ShareOwnerControlDbContext>(options => options.UseSqlServer(connectionString, builder => builder.CommandTimeout(300)));

            // Logic
            services.AddTransient<IStockLogic, StockLogic>();
            services.AddTransient<IHoldingLogic, HoldingLogic>();
            services.AddTransient<IShareLogic, ShareLogic>();
            services.AddTransient<IShareHolderLogic, ShareHolderLogic>();

            // DataManagers
            services.AddTransient<IStockDataManager, StockDataManager>();
            services.AddTransient<IHoldingDataManager, HoldingDataManager>();
            services.AddTransient<IShareDataManager, ShareDataManager>();
            services.AddTransient<IShareHolderDataManager, ShareHolderDataManager>();
        }
    }
}