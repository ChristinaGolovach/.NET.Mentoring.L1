using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Potestas.ORM.CodeFirstAproach.Models;

namespace Potestas.ORM.CodeFirstAproach.Configurations
{
    public class EnergyObservationConfiguration : IEntityTypeConfiguration<EnergyObservation>
    {
        public void Configure(EntityTypeBuilder<EnergyObservation> builder)
        {
            builder.ToTable("EnergyObservations").HasKey(obs => obs.Id);

            builder.Property(obs => obs.EstimatedValue)
                    .IsRequired()
                    .HasColumnType("float(53)");

            builder.Property(obs => obs.ObservationTime)
                    .IsRequired()
                    .HasColumnType("datetime");

            builder.HasOne<Coordinate>(obs => obs.Coordinate)
                   .WithMany(c => c.EnergyObservations)
                   .HasForeignKey(obs => obs.CoordinateId);
        }
    }

    //https://www.entityframeworktutorial.net/efcore/configure-one-to-many-relationship-using-fluent-api-in-ef-core.aspx
}
