using System;
using System.Configuration;

namespace Potestas.MongoDB.Plugin.Factories
{
    public class SaveToMongoDBProcessingFactory : IProcessingFactory<IEnergyObservation>
    {
        private readonly string _connectionString;

        public SaveToMongoDBProcessingFactory()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MongoDBObservationConnection"].ConnectionString;
        }
        public IEnergyObservationAnalizer CreateAnalizer()
        {
            throw new NotImplementedException();
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            throw new NotImplementedException();
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage()
        {
            throw new NotImplementedException();
        }
    }
}
