using Potestas.MongoDB.Plugin.Analizers;
using Potestas.MongoDB.Plugin.Processors;
using Potestas.MongoDB.Plugin.Storages;
using System.Configuration;

namespace Potestas.MongoDB.Plugin.Factories
{
    public class SaveToMongoDBProcessingFactory : IProcessingFactory<IEnergyObservation>
    {
        private readonly string _connectionString;
        private readonly string _databaseName;
        private readonly string _collectionName;
        private IEnergyObservationStorage<IEnergyObservation> _storage;

        public SaveToMongoDBProcessingFactory()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MongoDBObservationConnection"].ConnectionString;
            _databaseName = ConfigurationManager.AppSettings["MongoDBName"];
            _collectionName = ConfigurationManager.AppSettings["MongoDBCollectionName"];

        }
        public IEnergyObservationAnalizer CreateAnalizer()
        {
            _storage = GetStorage();

            return new MongoDBAnalizer(((MongoDBStorage<IEnergyObservation>)_storage).ObservationDBCollection);
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            return new SaveToMongoDBProcessor<IEnergyObservation>(GetStorage());
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage()
        {
            return GetStorage();
        }

        private IEnergyObservationStorage<IEnergyObservation> GetStorage()
        {
            if (_storage == null)
            {
                _storage = new MongoDBStorage<IEnergyObservation>(_connectionString, _databaseName, _collectionName);
            }

            return _storage;
        }    
    }
}
