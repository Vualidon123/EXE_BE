using EXE_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace EXE_BE.Data
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

            // Ensure UserName is unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Id)
                .IsUnique();
        }
    }
}