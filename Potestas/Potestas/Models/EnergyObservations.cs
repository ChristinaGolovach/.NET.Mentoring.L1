﻿using System;

namespace Potestas.Models
{
    public partial class EnergyObservations : IEnergyObservation
    {
        public int Id { get; set; }
        public int CoordinateId { get; set; }
        public double EstimatedValue { get; set; }
        public DateTime ObservationTime { get; set; }

        public virtual Coordinates Coordinate { get; set; }

        public Potestas.Coordinates ObservationPoint => new Potestas.Coordinates(Coordinate.Id, Coordinate.X, Coordinate.Y);
    }
}
