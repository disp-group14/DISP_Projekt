using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ShareOwnerControl.DAL.Context;

namespace ShareOwnerControl
{
    public partial class Startup
    {
        private void InitDatabase(IServiceCollection services)
        {
            try {
                ShareOwnerControlDbContext dataContext = services.BuildServiceProvider().GetRequiredService<ShareOwnerControlDbContext>();

                dataContext.Database.Migrate();
            }
            catch(Exception e)
            {
                Console.WriteLine($"Error in database migration: {e.Message}");
            }
        }
    }
}