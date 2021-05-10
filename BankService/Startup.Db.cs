using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.DAL;

namespace BankService
{
    public partial class Startup
    {
        private void InitDatabase(IServiceCollection services)
        {
            services.AddSingleton(_ => Configuration);
            string connectionString = Configuration.GetValue<string>("DBConnection");
            services.AddDbContext<BankServiceDbContext>(options => options.UseSqlServer(connectionString, builder => builder.CommandTimeout(300)));
            try
            {
                BankServiceDbContext dataContext = services.BuildServiceProvider().GetRequiredService<BankServiceDbContext>();

                dataContext.Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in database migration: {e.Message}");
            }
        }
    }
}