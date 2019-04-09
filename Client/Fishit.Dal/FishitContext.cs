using Fishit.Dal.Entities;
using Fishit.Logging;
using Microsoft.EntityFrameworkCore;

namespace Fishit.Dal
{
    public class FishitContext : DbContext
    {
        private readonly ILogger _logger;

        public FishitContext()
        {
            _logger = LogManager.GetLogger(nameof(FishitContext));
        }

        public DbSet<FishingTrip> FishingTrips { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            _logger.Debug(nameof(OnConfiguring) + "; Start");

            if (!optionsBuilder.IsConfigured)
                optionsBuilder
                    .UseSqlite("Data Source=fishit.db");

            _logger.Debug(nameof(OnConfiguring) + "; End");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _logger.Debug(nameof(OnModelCreating) + "; Start");

            modelBuilder.Entity<FishingTrip>()
                .Property(e => e.Name)
                .HasMaxLength(50);

            modelBuilder.Entity<FishingTrip>()
                .Property(e => e.Location)
                .HasMaxLength(50);

            _logger.Debug(nameof(OnModelCreating) + "; Start");
        }
    }
}