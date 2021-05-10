using System;
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
                    .HasForeignKey(s => s.Id);

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