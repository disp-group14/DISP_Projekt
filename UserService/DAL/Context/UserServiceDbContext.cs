using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.DAL
{
    public partial class UserServiceDbContext : DbContext
    {
        public UserServiceDbContext()
        {

        }
        public UserServiceDbContext(DbContextOptions<UserServiceDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => {
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
            modelBuilder.Entity<User>().HasData(
                new User{Id = 1, Username = "Egon Olsen", Password = "bankkup"},
                new User{Id = 2, Username = "Kjeld", Password = "smælderfed"},
                new User{Id = 3, Username = "Elon Musk", Password = "tesla123"},
                new User{Id = 4, Username = "Mette Frederiksen", Password = "minkkiller99"},
                new User{Id = 5, Username = "Peter Falktoft", Password = "hergaarddetgodt"},
                new User{Id = 6, Username = "Esben Bjerre", Password = "hergaardetgodt"}
            );
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}