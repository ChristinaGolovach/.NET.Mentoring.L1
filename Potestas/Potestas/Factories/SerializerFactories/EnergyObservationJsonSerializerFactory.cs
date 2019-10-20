using Potestas.Serializers;

namespace Potestas.Factories.SerializerFactories
{
    public class EnergyObservationJsonSerializerFactory : ISerializerFactory<IEnergyObservation>
    {
        public ISerializer<IEnergyObservation> CreateSerializer()
        {
            return new JsonSerializer<IEnergyObservation>();
        }
    }
}
