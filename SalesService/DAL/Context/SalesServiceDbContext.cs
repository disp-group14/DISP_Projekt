using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesService.Models;

namespace SalesService.DAL.Context
{
    public partial class SalesServiceControlDbContext : DbContext
    {
        public SalesServiceControlDbContext()
        {

        }
        public SalesServiceControlDbContext(DbContextOptions<SalesServiceControlDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SaleRequest>(entity => {
                // Index and primary key
                entity.HasKey(e => e.Id);

                // Query filter
                entity.HasQueryFilter(db => !db.IsDeleted);
            });
            OnModelCreatingPartial(modelBuilder);
        }

        // Overridde SaveChanges to update CreatedOn and Modified on automatically
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is ModelBase && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));
            foreach (var entityEntry in entries)
            {
                var entity = entityEntry.Entity as ModelBase;
                entity.ModifiedOn = DateTimeOffset.Now;
                if (entityEntry.State == EntityState.Added)
                {
                    entity.CreatedOn = DateTimeOffset.Now;
                }
                else
                {
                    entityEntry.Property(nameof(ModelBase.CreatedOn)).IsModified = false;
                }
            }
            return (await base.SaveChangesAsync(true, cancellationToken));
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}