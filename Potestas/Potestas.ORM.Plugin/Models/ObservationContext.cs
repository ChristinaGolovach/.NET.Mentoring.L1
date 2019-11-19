using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;

namespace Potestas.ORM.Plugin.Models
{
    public partial class ObservationContext : DbContext
    {
        private string _connectionString;

        public ObservationContext()
        {
        }

        public ObservationContext(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException($"{nameof(_connectionString)} can not be null.");
        }

        public ObservationContext(DbContextOptions<ObservationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Coordinates> Coordinates { get; set; }
        public virtual DbSet<EnergyObservations> EnergyObservations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (_connectionString == null)
                {
                    _connectionString = ConfigurationManager.ConnectionStrings["ObservationConnection"].ConnectionString;
                }

                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<EnergyObservations>(entity =>
            {
                entity.Property(e => e.ObservationTime).HasColumnType("datetime");

                entity.HasOne(d => d.Coordinate)
                    .WithMany(p => p.EnergyObservations)
                    .HasForeignKey(d => d.CoordinateId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
        }
    }
}
