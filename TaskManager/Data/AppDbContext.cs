using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManager.Data.Configurations;

namespace TaskManager.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Apply configurations
            modelBuilder.ApplyConfiguration(new TaskItemConfiguration());

            // Create schema if not exists
            modelBuilder.HasDefaultSchema("taskmanager");
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Auto-update UpdatedAt timestamp
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is TaskItem && 
                           (e.State == EntityState.Modified || e.State == EntityState.Added));

            foreach (var entityEntry in entries)
            {
                ((TaskItem)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;

                if (entityEntry.State == EntityState.Added)
                {
                    ((TaskItem)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}