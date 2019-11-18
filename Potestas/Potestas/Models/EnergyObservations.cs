using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Potestas.Models
{
    public partial class EnergyObservations : IEnergyObservation
    {
        public int Id { get; set; }
        public int CoordinateId { get; set; }
        public double EstimatedValue { get; set; }
        public DateTime ObservationTime { get; set; }

        public virtual Coordinates Coordinate { get; set; }

        [NotMapped]
        public Potestas.Coordinates ObservationPoint => new Potestas.Coordinates(Coordinate.Id, Coordinate.X, Coordinate.Y);
    }
}
