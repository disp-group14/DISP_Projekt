using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.DAL;

namespace StockService
{
    public partial class Startup
    {
        private void InitDatabase(IServiceCollection services)
        {
            services.AddSingleton(_ => Configuration);
            string connectionString = Configuration.GetValue<string>("DBConnection");
            services.AddDbContext<StockServiceDbContext>(options => options.UseSqlServer(connectionString, builder => builder.CommandTimeout(300)));
            try
            {
                StockServiceDbContext dataContext = services.BuildServiceProvider().GetRequiredService<StockServiceDbContext>();

                dataContext.Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in database migration: {e.Message}");
            }
        }
    }
}