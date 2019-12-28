using System;
using Potestas.Analizers;
using Potestas.Processors;
using Potestas.Serializers;
using Potestas.Storages;
using Potestas.Utils;
using Potestas.Utils.Cache;
using Potestas.Utils.Cache.Storages;

namespace Potestas.Factories
{
    public class SaveToFileProcessingFactory : IProcessingFactory<IEnergyObservation>
    {
        private IEnergyObservationStorage<IEnergyObservation> _storage;
        private ISerializer<IEnergyObservation> _serializer;
        private ILoggerManager _logger;
        private IStorage<int, IEnergyObservation> _cacheStorage;
        private string _storagePath;

        public SaveToFileProcessingFactory(string storagePath, ISerializer<IEnergyObservation> serializer)
        {
            //_storagePath = ConfigurationManager.AppSettings["fileStoragePath"]; // just to test this way of working
            _storagePath = storagePath ?? throw new ArgumentNullException($"The {nameof(storagePath)} can not be null.");  //ConfigurationManager.AppSettings["fileStoragePath"];
            _serializer = serializer ?? throw new ArgumentNullException($"The {nameof(serializer)} can not be null.");
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            var processor = new SaveToFileProcessor<IEnergyObservation>(new SerializeProcessor<IEnergyObservation>(), _storagePath);

            return new ProcessorLoggerDecorator<IEnergyObservation>(processor, GetLogger());
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage() => GetStorage();

        public IEnergyObservationAnalizer CreateAnalizer()
            =>  new AnalizerLoggerDecorator(new LINQAnalizer(GetStorage()), GetLogger());
        

        private IEnergyObservationStorage<IEnergyObservation> GetStorage()
        {
            if(_storage == null)
            {
                var fileStorage = new FileStorage<IEnergyObservation>(_storagePath, _serializer);
                var storageWithLogger = new StorageLoggerDecorator<IEnergyObservation>(fileStorage, GetLogger());
                _storage = new StorageCacheDecorator<IEnergyObservation>(storageWithLogger, new CacheService<int, IEnergyObservation>(GetCacheStorage()));
            }

            return _storage;
        }

        private ILoggerManager GetLogger() => _logger == null ? _logger = new LoggerManager() : _logger;

        private IStorage<int, IEnergyObservation> GetCacheStorage()
            => _cacheStorage == null ? _cacheStorage = new InMemoryStorage<int, IEnergyObservation>() : _cacheStorage; 
    }
}
