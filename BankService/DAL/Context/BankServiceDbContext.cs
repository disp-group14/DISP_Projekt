using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BankService.Models;
using Microsoft.EntityFrameworkCore;

namespace UserService.DAL
{
    public partial class BankServiceDbContext : DbContext
    {
        public BankServiceDbContext()
        {

        }
        public BankServiceDbContext(DbContextOptions<BankServiceDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity => {
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
            modelBuilder.Entity<Account>().HasData(
                new Account{Id = 1, UserId = 1, Balance = 50750.65F},
                new Account{Id = 2, UserId = 2, Balance = 250.02F},
                new Account{Id = 3, UserId = 3, Balance = 999999999.99F},
                new Account{Id = 4, UserId = 4, Balance = 8986568.45F},
                new Account{Id = 5, UserId = 5, Balance = 4567.56F},
                new Account{Id = 6, UserId = 6, Balance = 5634.67F}
            );
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}