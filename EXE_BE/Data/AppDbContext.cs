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
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<ChallengeProgress> ChallengeProgresses { get; set; }
        public DbSet<EnergyUsage> EnergyUsages { get; set; }
        public DbSet<FoodUsage> FoodUsages { get; set; }
        public DbSet<PlasticUsage> PlasticUsages { get; set; }
        public DbSet<TrafficUsage> TrafficUsages { get; set; }
        public DbSet<FoodItem> FoodItems { get; set; }
        public DbSet<PlasticItem> PlasticItems { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Notify> Notifies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserActivities>(entity =>
            {
                entity.ToTable("UserActivities");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Date).IsRequired();
                entity.Property(e => e.TotalCO2Emission).HasColumnType("float");

                // One-to-One relationships
                entity.HasOne(e => e.PlasticUsage)
                      .WithOne(e => e.UserActivities)
                      .HasForeignKey<PlasticUsage>(e => e.ActivityId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.TrafficUsage)
                      .WithOne(e => e.UserActivities)
                      .HasForeignKey<TrafficUsage>(e => e.ActivityId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.FoodUsage)
                      .WithOne(e => e.UserActivities)
                      .HasForeignKey<FoodUsage>(e => e.ActivityId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.EnergyUsage)
                      .WithOne(e => e.UserActivities)
                      .HasForeignKey<EnergyUsage>(e => e.ActivityId)
                      .OnDelete(DeleteBehavior.Cascade);

                // One-to-Many with User
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuring PlasticUsage
            modelBuilder.Entity<PlasticUsage>(entity =>
            {
                entity.ToTable("PlasticUsages");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.date).IsRequired();
                entity.Property(e => e.CO2emission).HasColumnType("float");

                // One-to-Many with PlasticItem
                entity.HasMany(e => e.PlasticItems)
                      .WithOne(e => e.PlasticUsage)
                      .HasForeignKey(e => e.PlasticUsageId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuring TrafficUsage
            modelBuilder.Entity<TrafficUsage>(entity =>
            {
                entity.ToTable("TrafficUsages");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.date).IsRequired();
                entity.Property(e => e.distance).HasColumnType("float");
                entity.Property(e => e.CO2emission).HasColumnType("float");
                entity.Property(e => e.trafficCategory).HasConversion<int>().IsRequired();
            });

            // Configuring FoodUsage
            modelBuilder.Entity<FoodUsage>(entity =>
            {
                entity.ToTable("FoodUsages");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.date).IsRequired();
                entity.Property(e => e.CO2emission).HasColumnType("float");
                entity.Property(e => e.score).IsRequired();

                // One-to-Many with FoodItem
                entity.HasMany(e => e.FoodItems)
                      .WithOne(e => e.FoodUsage)
                      .HasForeignKey(e => e.FoodUsageId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuring EnergyUsage
            modelBuilder.Entity<EnergyUsage>(entity =>
            {
                entity.ToTable("EnergyUsages");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.date).IsRequired();
                entity.Property(e => e.electricityconsumption).HasColumnType("float");
                entity.Property(e => e.CO2emission).HasColumnType("float");
            });

            // Configuring FoodItem
            modelBuilder.Entity<FoodItem>(entity =>
            {
                entity.ToTable("FoodItems");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Weight).HasColumnType("float").IsRequired();
                entity.Property(e => e.FoodCategory).HasConversion<int>().IsRequired();
            });

            // Configuring PlasticItem
            modelBuilder.Entity<PlasticItem>(entity =>
            {
                entity.ToTable("PlasticItems");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Weight).HasColumnType("float").IsRequired();
                entity.Property(e => e.PlasticCategory).HasConversion<int>().IsRequired();
            });

            modelBuilder.Entity<ChallengeProgress>()
                .HasOne(cp => cp.Challenge)
                .WithMany()
                .HasForeignKey(cp => cp.ChallengeId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ChallengeProgress>()
                .HasOne(cp => cp.User)
                .WithMany(u => u.ChallengeProgresses)
                .HasForeignKey(cp => cp.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}