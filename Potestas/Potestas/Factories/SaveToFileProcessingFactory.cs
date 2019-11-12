using System;
using System.Configuration;
using Potestas.Analizers;
using Potestas.Processors;
using Potestas.Serializers;
using Potestas.Storages;

namespace Potestas.Factories
{
    public class SaveToFileProcessingFactory : IProcessingFactory<IEnergyObservation>
    {
        private IEnergyObservationStorage<IEnergyObservation> _storage;
        private ISerializer<IEnergyObservation> _serializer;
        private string _storagePath;

        public SaveToFileProcessingFactory(string storagePath, ISerializer<IEnergyObservation> serializer)
        {
            //_storagePath = ConfigurationManager.AppSettings["fileStoragePath"]; // just to test this way of working
            _storagePath = storagePath ?? throw new ArgumentNullException($"The {nameof(storagePath)} can not be null.");  //ConfigurationManager.AppSettings["fileStoragePath"];
            _serializer = serializer ?? throw new ArgumentNullException($"The {nameof(serializer)} can not be null.");
        }

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            return new SaveToFileProcessor<IEnergyObservation>(new SerializeProcessor<IEnergyObservation>(), _storagePath);
        }

        public IEnergyObservationStorage<IEnergyObservation> CreateStorage()
        {
            return GetStorage();
        }

        public IEnergyObservationAnalizer CreateAnalizer()
        {
            return new LINQAnalizer(GetStorage());
        }

        private IEnergyObservationStorage<IEnergyObservation> GetStorage()
        {
            if (_storage == null)
            {
                _storage = new FileStorage<IEnergyObservation>(_storagePath, _serializer);
            }

            return _storage;
        }
    }
}
