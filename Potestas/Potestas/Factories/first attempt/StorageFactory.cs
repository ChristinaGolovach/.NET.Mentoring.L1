//using Potestas.Serializers;
//using Potestas.Storages;

//namespace Potestas.Factories
//{
//    public class StorageFactory : IStorageFactory<IEnergyObservation>
//    {
//        public IEnergyObservationStorage<IEnergyObservation> CreateFileStorage(string filePath, ISerializer<IEnergyObservation> serializer)
//        {
//            return new FileStorage<IEnergyObservation>(filePath, serializer);
//        }

//        public IEnergyObservationStorage<IEnergyObservation> CreateListStorage()
//        {
//            return new ListStorage();
//        }
//    }
//}
