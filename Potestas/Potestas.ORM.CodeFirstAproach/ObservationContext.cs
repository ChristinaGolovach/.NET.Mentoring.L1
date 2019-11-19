using Microsoft.EntityFrameworkCore;
using Potestas.ORM.CodeFirstAproach.Configurations;
using Potestas.ORM.CodeFirstAproach.Models;

namespace Potestas.ORM.CodeFirstAproach
{
    // This project is only for demo how to create DB from Code first aproach (task 4)
    //https://stackoverflow.com/a/44434134

    public class ObservationContext : DbContext
    {
        public DbSet<Coordinate> Coordinates { get; set; }
        public DbSet<EnergyObservation> EnergyObservations { get; set; }

        public ObservationContext()
        {
           // Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Observation_CodeFirstDemo;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CoordinateConfiguration());
            modelBuilder.ApplyConfiguration(new EnergyObservationConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
