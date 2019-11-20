using System;

namespace Potestas.ORM.Plugin.Models
{

    // DBStorage is generic class and has some methods that return T,
    // so we need to convert EnergyObservations to T.
    // For this purpose I used the ConvertObservationCollectionToGeneric method from EnergyObservationExtension.cs in the Potestas project
    // and if this model (EnergyObservations) does not implement IEnergyObservation
    // the in runtime we will have an exception in method ConvertObservationCollectionToGeneric
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