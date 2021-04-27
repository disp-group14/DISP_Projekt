using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesService.DAL.Context;

namespace SalesService
{
    public partial class Startup
    {
        private void InitDatabase(IServiceCollection services)
        {
            try {
                SalesServiceControlDbContext dataContext = services.BuildServiceProvider().GetRequiredService<SalesServiceControlDbContext>();

                dataContext.Database.Migrate();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error in database migration: {e.Message}");
            }
        }
    }
}