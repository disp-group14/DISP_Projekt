using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShareOwnerControl.Models;

namespace ShareOwnerControl.DAL.Context
{
    public partial class ShareOwnerControlDbContext : DbContext
    {
        public ShareOwnerControlDbContext()
        {

        }
        public ShareOwnerControlDbContext(DbContextOptions<ShareOwnerControlDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ShareHolder
            modelBuilder.Entity<ShareHolder>(
                entity =>
                {
                    // Index and primary key
                    entity.HasKey(e => e.Id);

                    // Query filter
                    entity.HasQueryFilter(db => !db.IsDeleted);
                }
            );

            // Share
            modelBuilder.Entity<Share>(
                entity =>
                {
                    // Index and primary key
                    entity.HasKey(e => e.Id);

                    // One to Many
                    entity
                        .HasOne(sh => sh.Stock)
                        .WithMany(st => st.Shares)
                        .HasForeignKey(sh => sh.Id);

                    // One to Many
                    entity
                        .HasOne(sh => sh.Holding)
                        .WithMany(h => h.Shares)
                        .HasForeignKey(sh => sh.Id);

                    // Query filter
                    entity.HasQueryFilter(db => !db.IsDeleted);
                }
            );

            // Holding
            modelBuilder.Entity<Holding>(
                entity =>
                {
                    // Index and primary key
                    entity.HasKey(e => e.Id);

                    // One to Many
                    entity
                        .HasOne(h => h.ShareHolder)
                        .WithMany(sh => sh.Holdings)
                        .HasForeignKey(h => h.Id);

                    // One to Many
                    // entity
                    //     .HasOne(h => h.Stock)
                    //     .WithMany(s => s.Holdings)
                    //     .HasForeignKey(h => h.Id);

                    // Query filter
                    entity.HasQueryFilter(db => !db.IsDeleted);
                }
            );


            // Stock
            modelBuilder.Entity<Stock>(
                entity =>
                {
                    // Index and primary key
                    entity.HasKey(e => e.Id);

                    // Query filter
                    entity.HasQueryFilter(db => !db.IsDeleted);
                }
            );
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