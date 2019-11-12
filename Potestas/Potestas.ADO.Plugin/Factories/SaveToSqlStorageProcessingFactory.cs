using System.Configuration;

namespace Potestas.ADO.Plugin.Factories
{
    public class SaveToSqlStorageProcessingFactory : IProcessingFactory<IEnergyObservation>
    {
        private readonly string _connectionString;

        public SaveToSqlStorageProcessingFactory()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["ObservationConnection"].ConnectionString;
        }
        public IEnergyObservationAnalizer CreateAnalizer()
        {
            return new SqlAnalizer(_connectionString);
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            return new SaveToSqlProcessor<IEnergyObservation>(_connectionString);
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage()
        {
            return new SqlStorage<IEnergyObservation>(_connectionString);
        }
    }
}
