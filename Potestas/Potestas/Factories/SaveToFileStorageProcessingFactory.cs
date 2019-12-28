using System;
using Potestas.Analizers;
using Potestas.Processors;
using Potestas.Serializers;
using Potestas.Storages;
using Potestas.Utils;

namespace Potestas.Factories
{
    public class SaveToFileStorageProcessingFactory: IProcessingFactory<IEnergyObservation>
    {
        private IEnergyObservationStorage<IEnergyObservation> _storage;
        private ISerializer<IEnergyObservation> _serializer;
        private ILoggerManager _logger;
        private string _storagePath;

        public SaveToFileStorageProcessingFactory(string storagePath, ISerializer<IEnergyObservation> serializer)
        {
            _storagePath = storagePath ?? throw new ArgumentNullException($"The {nameof(storagePath)} can not be null.");
            _serializer = serializer ?? throw new ArgumentNullException($"The {nameof(serializer)} can not be null.");
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
            => new ProcessorLoggerDecorator<IEnergyObservation>(new SaveToStorageProcessor<IEnergyObservation>(GetStorage()), GetLogger());
        

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage() => GetStorage();


        public IEnergyObservationAnalizer CreateAnalizer()
            => new AnalizerLoggerDecorator(new LINQAnalizer(GetStorage()), GetLogger());       
        

        private IEnergyObservationStorage<IEnergyObservation> GetStorage()
            => _storage ?? new StorageLoggerDecorator<IEnergyObservation>(new FileStorage<IEnergyObservation>(_storagePath, _serializer), GetLogger());


        private ILoggerManager GetLogger() => _logger == null ? _logger = new LoggerManager() : _logger;
    }
}
