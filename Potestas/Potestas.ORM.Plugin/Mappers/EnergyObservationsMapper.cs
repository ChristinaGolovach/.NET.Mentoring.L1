using Potestas.ORM.Plugin.Models;

namespace Potestas.ORM.Plugin.Mappers
{
    internal static class EnergyObservationsMapper
    {
        public static EnergyObservations ToORMEntity(this IEnergyObservation energyObservation)
        {
            return new EnergyObservations()
            {
                Id = energyObservation.Id,
                CoordinateId = energyObservation.ObservationPoint.Id,
                EstimatedValue = energyObservation.EstimatedValue,
                ObservationTime = energyObservation.ObservationTime
            };
        }
    }
}
