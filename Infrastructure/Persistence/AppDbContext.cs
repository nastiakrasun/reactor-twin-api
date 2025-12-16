using Microsoft.EntityFrameworkCore;
using ReactorTwinAPI.Domain.Entities;

namespace ReactorTwinAPI.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<ReactorTwin> ReactorTwins => Set<ReactorTwin>();
        public DbSet<User> Users => Set<User>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
