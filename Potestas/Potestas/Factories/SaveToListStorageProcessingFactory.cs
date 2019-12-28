using Potestas.Analizers;
using Potestas.Processors;
using Potestas.Storages;
using Potestas.Utils;

namespace Potestas.Factories
{
    public class SaveToListStorageProcessingFactory: IProcessingFactory<IEnergyObservation>
    {
        private IEnergyObservationStorage<IEnergyObservation> _storage;
        private ILoggerManager _logger;

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
            => new SaveToStorageProcessor<IEnergyObservation>(GetStorage());

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage() => GetStorage();

        public IEnergyObservationAnalizer CreateAnalizer()
            => new AnalizerLoggerDecorator(new LINQAnalizer(GetStorage()), GetLogger());

        private IEnergyObservationStorage<IEnergyObservation> GetStorage()
            => _storage == null ? _storage = new StorageLoggerDecorator<IEnergyObservation>(new ListStorage(), GetLogger()) : _storage;

        private ILoggerManager GetLogger() => _logger == null ? _logger = new LoggerManager() : _logger;
    }
}
