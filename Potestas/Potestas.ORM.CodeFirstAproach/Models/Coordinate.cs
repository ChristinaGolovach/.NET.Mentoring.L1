using System.Collections.Generic;

namespace Potestas.ORM.CodeFirstAproach.Models
{
    public class Coordinate
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public ICollection<EnergyObservation> EnergyObservations { get; set; }
    }
}
