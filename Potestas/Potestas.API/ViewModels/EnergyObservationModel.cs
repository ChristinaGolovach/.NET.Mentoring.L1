using System;

namespace Potestas.API.ViewModels
{
    public class EnergyObservationModel
    {
        public int Id { get; set; }

        public CoordinatesModel ObservationPoint { get; set; }

        public double EstimatedValue { get; set; }

        public DateTime ObservationTime { get; set; }
    }
}
