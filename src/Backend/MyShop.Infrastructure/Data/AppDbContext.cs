using Microsoft.EntityFrameworkCore;
using MyShop.Domain.Entities;

namespace MyShop.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration is now handled via Data Annotations in the User entity class.

        }
    }
}