using System;

namespace Potestas.ORM.Plugin.Models
{
    public partial class EnergyObservations : IEnergyObservation
    {
        public int Id { get; set; }
        public int CoordinateId { get; set; }
        public double EstimatedValue { get; set; }
        public DateTime ObservationTime { get; set; }

        public virtual Coordinates Coordinate { get; set; }

        public Potestas.Coordinates ObservationPoint => new Potestas.Coordinates(CoordinateId, Coordinate.X, Coordinate.Y);
    }
}

// In PM Console for DB first approach => automatically generates models based on the database
//Scaffold-DbContext "Server=(localdb)\MSSQLLocalDB;Database=Observation;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models