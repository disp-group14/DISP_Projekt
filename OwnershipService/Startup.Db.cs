using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.DAL;

namespace OwnershipService
{
    public partial class Startup
    {
        private void InitDatabase(IServiceCollection services)
        {
            services.AddSingleton(_ => Configuration);
            string connectionString = Configuration.GetValue<string>("DBConnection");
            services.AddDbContext<OwnershipServiceDbContext>(options => options.UseSqlServer(connectionString, builder => builder.CommandTimeout(300)));
            try
            {
                OwnershipServiceDbContext dataContext = services.BuildServiceProvider().GetRequiredService<OwnershipServiceDbContext>();

                dataContext.Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in database migration: {e.Message}");
            }
        }
    }
}