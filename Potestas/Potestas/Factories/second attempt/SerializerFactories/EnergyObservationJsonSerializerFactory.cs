using Potestas.Serializers;

namespace Potestas.Factories.second_attempt.SerializerFactories
{
    public class EnergyObservationJsonSerializerFactory : ISerializerFactory<IEnergyObservation>
    {
        public ISerializer<IEnergyObservation> CreateSerializer()
        {
            return new JsonSerializer<IEnergyObservation>();
        }
    }
}
