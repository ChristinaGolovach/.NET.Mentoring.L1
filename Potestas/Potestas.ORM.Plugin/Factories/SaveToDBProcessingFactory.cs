using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Potestas.ORM.Plugin.Analizers;
using Potestas.ORM.Plugin.Models;
using Potestas.ORM.Plugin.Processors;
using Potestas.ORM.Plugin.Storages;

namespace Potestas.ORM.Plugin.Factories
{
    public class SaveToDBProcessingFactory : IProcessingFactory<IEnergyObservation>
    {
        private readonly string _connectionString;
        private DbContext _dbContext;
        private IEnergyObservationStorage<IEnergyObservation> _storage;


        public SaveToDBProcessingFactory()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ObservationConnection"].ConnectionString;
        }

        public IEnergyObservationAnalizer CreateAnalizer()
        {
            return new ORMAnalizer(GetContext());
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            return new SaveToDBProcessor<IEnergyObservation>(GetStorage());
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage()
        {
            return GetStorage();
        }

        private IEnergyObservationStorage<IEnergyObservation> GetStorage()
        {
            if (_storage == null)
            {
                _storage = new DBStorage<IEnergyObservation>(GetContext());
            }

            return _storage;
        }

        private DbContext GetContext()
        {
            if (_dbContext == null)
            {
                _dbContext = new ObservationContext(_connectionString);
            }

            return _dbContext;
        }
    }
}
