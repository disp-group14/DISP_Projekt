using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.DAL;

namespace UserService
{
    public partial class Startup
    {
        private void InitDatabase(IServiceCollection services)
        {
            services.AddSingleton(_ => Configuration);
            string connectionString = Configuration.GetValue<string>("DBConnection");
            services.AddDbContext<UserServiceDbContext>(options => options.UseSqlServer(connectionString, builder => builder.CommandTimeout(300)));
            try
            {
                UserServiceDbContext dataContext = services.BuildServiceProvider().GetRequiredService<UserServiceDbContext>();

                dataContext.Database.Migrate();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in database migration: {e.Message}");
            }
        }
    }
}