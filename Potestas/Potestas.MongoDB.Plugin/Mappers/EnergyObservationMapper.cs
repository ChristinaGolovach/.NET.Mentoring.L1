using Potestas.MongoDB.Plugin.Entities;

namespace Potestas.MongoDB.Plugin.Mappers
{
    public static class EnergyObservationMapper
    {
        public static BsonEnergyObservation ToBsonEntity(this IEnergyObservation energyObservation)
        {
            return new BsonEnergyObservation
            {
                Id = energyObservation.Id,
                EstimatedValue = energyObservation.EstimatedValue,
                ObservationTime = energyObservation.ObservationTime,
                ObservationPoint = energyObservation.ObservationPoint.ToBsonEntity()
            };
        }
    }
}
