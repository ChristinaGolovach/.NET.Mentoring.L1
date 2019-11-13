using System;

namespace Potestas.Models
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
