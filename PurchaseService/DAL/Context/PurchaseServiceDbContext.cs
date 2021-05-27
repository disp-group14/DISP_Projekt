using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PurchaseService.Models;

namespace PurchaseService.DAL.Context
{
    public partial class PurchaseServiceControlDbContext : DbContext
    {
        public PurchaseServiceControlDbContext()
        {

        }
        public PurchaseServiceControlDbContext(DbContextOptions<PurchaseServiceControlDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PurchaseRequest>(entity => {
                // Index and primary key
                entity.HasKey(e => e.Id);

                // Query filter
                entity.HasQueryFilter(db => !db.IsDeleted);
            });
            OnModelCreatingPartial(modelBuilder);
            SeedData(modelBuilder);
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
            var stockLookUpTable = new Dictionary<int, float>(){
                {1, 126.85F},
                {2,  506.98F},
                {3, 3265.16F},
                {4, 596.69F},
                {5, 118.24F},
                {6, 52.91F},
                {7, 143.99F},
                {8, 327.66F},
                {9, 61.44F},
                {10, 251.49F},
                {11, 261.37F},
                {12, 628.0F},
                {13, 502.36F}, 
                {14, 619.13F},
                {15, 188.36F}
            };

            var purchaseRequests = new List<PurchaseRequest>();

            foreach(var stockPair in stockLookUpTable) {
                for(var userIndex = 1; userIndex <= 6; userIndex++) {
                    purchaseRequests.AddRange(Enumerable.Range(purchaseRequests.Count + 1, new Random().Next(6)).Select(requestIndex => {
                        return new PurchaseRequest(){
                            Id = requestIndex,
                            Amount = new Random().Next(1, 40),
                            UserId = userIndex,
                            Price = stockPair.Value - new Random().Next(1, 25),
                            StockId = stockPair.Key
                        };
                    }));
                }
            }

            modelBuilder.Entity<PurchaseRequest>().HasData(purchaseRequests);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}