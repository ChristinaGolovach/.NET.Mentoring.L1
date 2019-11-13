using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Potestas.Models
{
    public partial class ObservationContext : DbContext
    {
        public ObservationContext()
        {
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
                optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["ObservationConnection"].ConnectionString);
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
