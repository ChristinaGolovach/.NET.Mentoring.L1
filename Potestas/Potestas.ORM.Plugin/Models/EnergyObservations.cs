using System;
using System.Collections.Generic;

namespace Potestas.ORM.Plugin.Models
{
    public partial class EnergyObservations
    {
        public int Id { get; set; }
        public int CoordinateId { get; set; }
        public double EstimatedValue { get; set; }
        public DateTime ObservationTime { get; set; }

        public virtual Coordinates Coordinate { get; set; }
    }
}

// In PM Console for DB first approach
//Scaffold-DbContext "Server=(localdb)\MSSQLLocalDB;Database=Observation;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models