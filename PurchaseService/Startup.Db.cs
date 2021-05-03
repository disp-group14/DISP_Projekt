using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PurchaseService.DAL.Context;

namespace PurchaseService
{
    public partial class Startup
    {
        private void InitDatabase(IServiceCollection services)
        {
            try {
                PurchaseServiceControlDbContext dataContext = services.BuildServiceProvider().GetRequiredService<PurchaseServiceControlDbContext>();

                dataContext.Database.Migrate();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error in database migration: {e.Message}");
            }
        }
    }
}