using System;

namespace Potestas.ORM.CodeFirstAproach.Models
{
    public class EnergyObservation
    {
        public int Id { get; set; }
        public double EstimatedValue { get; set; }
        public DateTime ObservationTime { get; set; }

        public int CoordinateId { get; set; }
        public Coordinate Coordinate { get; set; }
    }

    //https://www.entityframeworktutorial.net/efcore/one-to-many-conventions-entity-framework-core.aspx
}
