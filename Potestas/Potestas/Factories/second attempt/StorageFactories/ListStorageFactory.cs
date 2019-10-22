using Potestas.Serializers;
using Potestas.Storages;

namespace Potestas.Factories.second_attempt.StorageFactories
{
    public class ListStorageFactory : IStorageFactory<IEnergyObservation>
    {
        public IEnergyObservationStorage<IEnergyObservation> CreateStorage(string filePath, ISerializer<IEnergyObservation> serializer)
        {
            return new ListStorage();
        }
    }
}
