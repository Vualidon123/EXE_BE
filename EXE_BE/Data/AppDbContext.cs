using EXE_BE.Models;
using EXE_BE.Models.ItemList;
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
        public DbSet<UserActivities> UserActivities { get; set; }
        public DbSet<EnergyUsage> EnergyUsages { get; set; }
        public DbSet<FoodUsage> FoodUsages { get; set; }
        public DbSet<PlasticUsage> PlasticUsages { get; set; }
        public DbSet<TrafficUsage> TrafficUsages { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<PlasticItem> PlasticItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure UserName is unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Id)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserActivities)
                .WithOne(ua => ua.User)
                .HasForeignKey(ua => ua.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserActivities>()
                .HasOne(ua => ua.EnergyUsage)
                .WithOne(eu => eu.UserActivities)
                .HasForeignKey<UserActivities>(ua => ua.EnergyUsageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserActivities>()
                .HasOne(ua => ua.FoodUsage)
                .WithOne(fu => fu.UserActivities)
                .HasForeignKey<UserActivities>(ua => ua.FoodUsageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserActivities>()
                .HasOne(ua => ua.PlasticUsage)
                .WithOne(pu => pu.UserActivities)
                .HasForeignKey<UserActivities>(ua => ua.PlasticUsageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserActivities>()
                .HasOne(ua => ua.TrafficUsage)
                .WithOne(tu => tu.UserActivities)
                .HasForeignKey<UserActivities>(ua => ua.TrafficUsageId)
                .OnDelete(DeleteBehavior.Cascade);
            

            // ------------------ One-to-Many Relations ------------------

            // FoodUsage → FoodItems
            modelBuilder.Entity<FoodItem>()
                .HasOne<FoodUsage>()
                .WithMany(fu => fu.FoodItems)
                .HasForeignKey(fi => fi.FoodUsageId)
                .OnDelete(DeleteBehavior.SetNull);                
            // PlasticUsage → PlasticItems
            modelBuilder.Entity<PlasticItem>()
                .HasOne<PlasticUsage>()
                .WithMany(pu => pu.PlasticItems)
                .HasForeignKey(pi => pi.PlasticUsageId)
                .OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(modelBuilder);
        }
    }
}