using Potestas.Serializers;
using Potestas.Storages;

namespace Potestas.Factories.second_attempt.StorageFactories
{
    public class FileStorageFactory : IStorageFactory<IEnergyObservation>
    {
        public IEnergyObservationStorage<IEnergyObservation> CreateStorage(string filePath, ISerializer<IEnergyObservation> serializer)
        {
            return new FileStorage<IEnergyObservation>(filePath, serializer);
        }
    }
}
