using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Potestas.ORM.CodeFirstAproach.Models;

namespace Potestas.ORM.CodeFirstAproach.Configurations
{
    public class CoordinateConfiguration : IEntityTypeConfiguration<Coordinate>
    {
        public void Configure(EntityTypeBuilder<Coordinate> builder)
        {
            builder.ToTable("Coordinates").HasKey(c => c.Id);

            builder.Property(c => c.X)
                   .HasColumnType("float(53)");

            builder.Property(c => c.Y)
                   .HasColumnType("float(53)");

            builder.HasMany<EnergyObservation>(c => c.EnergyObservations)
                   .WithOne(obs => obs.Coordinate)
                   .HasForeignKey(obs => obs.CoordinateId)
                   .OnDelete(DeleteBehavior.ClientSetNull); // https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.deletebehavior?view=efcore-3.0

        }
    }
}
