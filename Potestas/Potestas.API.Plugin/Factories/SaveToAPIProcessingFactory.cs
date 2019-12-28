using Potestas.API.Plugin.Analizers;
using Potestas.API.Plugin.Services;
using Potestas.API.Plugin.Services.Implementations;
using Potestas.API.Plugin.Storages;
using Potestas.Processors;
using Potestas.Utils;

namespace Potestas.API.Plugin.Factories
{
    public class SaveToAPIProcessingFactory : IProcessingFactory<IEnergyObservation>
    {
        private IEnergyObservationStorage<IEnergyObservation> _storage;
        private IHttpClientService _httpClientService;

        public IEnergyObservationAnalizer CreateAnalizer() 
            => new APIAnalizer(GetHttpService());


        public IEnergyObservationProcessor<IEnergyObservation> CreateProcessor()
            => new SaveToStorageProcessor<IEnergyObservation>(GetStorage());


        public IEnergyObservationStorage<IEnergyObservation> CreateStorage() 
            => GetStorage();


        private IEnergyObservationStorage<IEnergyObservation> GetStorage()
        {
            if (_storage == null)
            {
                _storage = new APIStorage<IEnergyObservation>(GetHttpService());
            }

            return _storage;
        }

        private IHttpClientService GetHttpService()
        {
            if (_httpClientService == null)
            {
                _httpClientService = new HttpClientService(new LoggerManager());
            }

            return _httpClientService;
        }
    }
}
