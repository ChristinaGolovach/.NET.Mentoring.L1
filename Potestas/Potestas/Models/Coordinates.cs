using System;
using System.Collections.Generic;

namespace Potestas.Models
{
    public partial class Coordinates
    {
        public Coordinates()
        {
            EnergyObservations = new HashSet<EnergyObservations>();
        }

        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public virtual ICollection<EnergyObservations> EnergyObservations { get; set; }
    }
}
