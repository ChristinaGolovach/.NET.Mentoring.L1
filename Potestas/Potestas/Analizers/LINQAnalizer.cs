using System;
using System.Collections.Generic;
using System.Linq;

namespace Potestas.Analizers
{
    /* TASK. Implement an Analizer for Observations using LINQ
     */
    public class LINQAnalizer : IEnergyObservationAnalizer
    {
        private IEnergyObservationStorage<IEnergyObservation> _observationStorage;
        public LINQAnalizer(IEnergyObservationStorage<IEnergyObservation> observationStorage)
        {
            _observationStorage = observationStorage ?? throw new ArgumentNullException($"The {nameof(observationStorage)} can not be null.");
        }
        public double GetAverageEnergy()
        {
           return  _observationStorage.Average(obs => obs.EstimatedValue);
        }

        public double GetAverageEnergy(DateTime startFrom, DateTime endBy)
        {
            return _observationStorage.Where(obs => obs.ObservationTime >= startFrom && endBy >= obs.ObservationTime)
                                      .Average(obs => obs.EstimatedValue);
        }

        public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight)
        {
            return _observationStorage.Where(obs => obs.ObservationPoint.X > rectTopLeft.X
                                                 && obs.ObservationPoint.X < rectBottomRight.X
                                                 && obs.ObservationPoint.Y > rectBottomRight.Y
                                                 && obs.ObservationPoint.Y < rectTopLeft.Y)
                                      .Average(obs => obs.EstimatedValue);
                
        }

        public IDictionary<Coordinates, int> GetDistributionByCoordinates()
        {
            return _observationStorage.GroupBy(obs => obs.ObservationPoint)
                                      .ToDictionary(k => k.Key, v => v.Count());
        }

        public IDictionary<double, int> GetDistributionByEnergyValue()
        {
            return _observationStorage.GroupBy(obs => obs.EstimatedValue)
                                      .ToDictionary(k => k.Key, v => v.Count());
        }

        public IDictionary<DateTime, int> GetDistributionByObservationTime()
        {
            return _observationStorage.GroupBy(obs => obs.ObservationTime)
                                      .ToDictionary(k => k.Key, v => v.Count());
        }

        public double GetMaxEnergy()
        {
            return _observationStorage.Max(obs => obs.EstimatedValue);
        }

        public double GetMaxEnergy(Coordinates coordinates)
        {
            return _observationStorage.Where(obs => obs.ObservationPoint == coordinates)
                                      .Max(obs => obs.EstimatedValue);
        }

        public double GetMaxEnergy(DateTime dateTime)
        {
            return _observationStorage.Where(obs => obs.ObservationTime == dateTime)
                                      .Max(obs => obs.EstimatedValue);
        }

        public Coordinates GetMaxEnergyPosition()
        {
            return _observationStorage.Max(obs => obs.ObservationPoint);
        }

        public DateTime GetMaxEnergyTime()
        {
            return _observationStorage.Max(obs => obs.ObservationTime);
        }

        public double GetMinEnergy()
        {
            return _observationStorage.Min(obs => obs.EstimatedValue);
        }

        public double GetMinEnergy(Coordinates coordinates)
        {
            return _observationStorage.Where(obs => obs.ObservationPoint == coordinates)
                                      .Min(obs => obs.EstimatedValue);
        }

        public double GetMinEnergy(DateTime dateTime)
        {
            return _observationStorage.Where(obs => obs.ObservationTime == dateTime)
                                      .Max(obs => obs.EstimatedValue);
        }

        public Coordinates GetMinEnergyPosition()
        {
            return _observationStorage.Min(obs => obs.ObservationPoint);
        }

        public DateTime GetMinEnergyTime()
        {
            return _observationStorage.Min(obs => obs.ObservationTime);
        }
    }
}
