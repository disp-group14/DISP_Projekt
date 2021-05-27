using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OwnershipService.Models;

namespace UserService.DAL
{
    public partial class OwnershipServiceDbContext : DbContext
    {
        public OwnershipServiceDbContext()
        {

        }
        public OwnershipServiceDbContext(DbContextOptions<OwnershipServiceDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Shareholder
            modelBuilder.Entity<ShareHolder>(entity =>
            {
                entity.HasIndex(e => e.Id);

                entity.HasQueryFilter(db => !db.IsDeleted);
            });

            // Share
            modelBuilder.Entity<Share>(entity =>
            {
                entity.HasIndex(e => e.Id);
                // One to many relationship with foreignkey  => One shareholder with many shares
                entity
                    .HasOne(s => s.ShareHolder)
                    .WithMany(sh => sh.Shares)
                    .HasForeignKey(s => s.ShareHolderId);

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
            // Create shareholders
            List<ShareHolder> shareHolders = Enumerable.Range(1, 6).Select(index => new ShareHolder(){
                UserId = index,
                Id = index
            }).ToList();

            // Seed shareholders in db
            modelBuilder.Entity<ShareHolder>().HasData(shareHolders);

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

            // Generate shares
            var shares = new List<Share>();

            foreach(var shareHolder in shareHolders) {
                foreach(var stockPair in stockLookUpTable) {
                Random random = new Random();
                shares.AddRange(Enumerable.Range(shares.Count + 1, random.Next(10)).Select(index => {
                    return new Share(){
                        Id = index,
                        StockId = stockPair.Key,
                        PurchasePrice = randomPurchasePrice(stockPair.Value),
                        ShareHolderId = shareHolder.Id 
                    };
                }));
                }
            }

            modelBuilder.Entity<Share>().HasData(shares);
        }

        private float randomPurchasePrice(float marketPrice){
            Random random = new Random();
            int offset = random.Next(50);
            return (random.NextDouble() > 0.5 ? marketPrice + offset : marketPrice - offset );
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}