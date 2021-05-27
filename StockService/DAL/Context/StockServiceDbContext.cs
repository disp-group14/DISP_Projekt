using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockService.Models;

namespace UserService.DAL
{
    public partial class StockServiceDbContext : DbContext
    {
        public StockServiceDbContext()
        {

        }
        public StockServiceDbContext(DbContextOptions<StockServiceDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stock>(entity => {
                entity.HasIndex(e => e.Id);
                
                entity.HasQueryFilter(db => !db.IsDeleted);
            });
            SeedData(modelBuilder);
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

        private void SeedData(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Stock>().HasData(
                // Chosen stocks from nasdaq 100 (Accessed 27. May)
                new Stock{Id = 1, Name = "Apple Inc", Price = 126.85F},
                new Stock{Id = 2, Name = "Adobe", Price = 506.98F},
                new Stock{Id = 3, Name = "Amazom", Price = 3265.16F},
                new Stock{Id = 4, Name = "Align Technology", Price = 596.69F},
                new Stock{Id = 5, Name = "Check Point Software", Price = 118.24F},
                new Stock{Id = 6, Name = "Cisco", Price = 52.91F},
                new Stock{Id = 7, Name = "Electronic Arts", Price = 143.99F},
                new Stock{Id = 8, Name = "Facebook", Price = 327.66F},
                new Stock{Id = 9, Name = "eBay", Price = 61.44F},
                new Stock{Id = 10, Name = "Microsoft", Price = 251.49F},
                new Stock{Id = 11, Name = "PayPal", Price = 261.37F},
                new Stock{Id = 12, Name = "NVIDIA", Price = 628.0F},
                new Stock{Id = 13, Name = "Netflix", Price = 502.36F},
                new Stock{Id = 14, Name = "Tesla", Price = 619.13F},
                new Stock{Id = 15, Name = "Texas Intruments", Price = 188.36F}
            );
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}