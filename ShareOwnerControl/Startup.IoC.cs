using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShareOwnerControl.DAL.Context;

namespace ShareOwnerControl
{
    public partial class Startup
    {
        public void ConfigureIoC(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(_ => configuration);
            string connectionString = configuration.GetValue<string>("DBConnection");
            services.AddDbContext<ShareOwnerControlDbContext>(options => options.UseSqlServer(connectionString, builder => builder.CommandTimeout(300)));

        }
    }
}