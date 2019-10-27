using Potestas.Analizers;
using Potestas.Attributes;
using Potestas.Processors;
using Potestas.Storages;

namespace Potestas.Factories
{
    public class SaveToListStorageProcessingFactory: IProcessingFactory<IEnergyObservation>
    {
        private IEnergyObservationStorage<IEnergyObservation> _storage;

        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
        {
            return new SaveToStorageProcessor<IEnergyObservation>(GetStorage());
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
                _storage = new ListStorage();
            }

            return _storage;
        }
    }
}
