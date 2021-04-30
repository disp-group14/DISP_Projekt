using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaxService.DAL;

namespace TaxService
{
    public partial class Startup
    {
        private void InitDatabase(IServiceCollection services)
        {
            services.AddSingleton(_ => Configuration);
            string connectionString = Configuration.GetValue<string>("DBConnection");
            services.AddDbContext<TaxServiceDbContext>(options => options.UseSqlServer(connectionString, builder => builder.CommandTimeout(300)));
            try
            {
                TaxServiceDbContext dataContext = services.BuildServiceProvider().GetRequiredService<TaxServiceDbContext>();

                dataContext.Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in database migration: {e.Message}");
            }
        }
    }
}