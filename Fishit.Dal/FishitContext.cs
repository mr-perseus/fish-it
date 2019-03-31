using Fishit.Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fishit.Dal
{
    public class FishitContext : DbContext
    {
        public DbSet<FishingTrip> FishingTrips { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder
                    .UseSqlite("Data Source=fishit.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FishingTrip>()
                .Property(e => e.Name)
                .HasMaxLength(50);

            modelBuilder.Entity<FishingTrip>()
                .Property(e => e.Location)
                .HasMaxLength(50);

            /*modelBuilder.Entity<FishingTrip>()
                .Property(e => e.RowVersion)
                .IsRowVersion();*/
        }
    }
}