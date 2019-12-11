using AutoMapper;
using Potestas.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Potestas.API.Services.Implementations
{
    public class EnergyObservationService : IEnergyObservationService
    {
        private readonly IEnergyObservationStorage<IEnergyObservation> _storage;
        private readonly IMapper _mapper;

        public EnergyObservationService(IEnergyObservationStorage<IEnergyObservation> storage, IMapper mapper)
        {
            _storage = storage;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EnergyObservationModel>> GetAllObservationsAsync()
        {
            var enumerator =  await Task.Run(() => _storage.GetEnumerator());
            var observations = new List<EnergyObservationModel>();

            while(enumerator.MoveNext())
            {
                observations.Add(_mapper.Map<EnergyObservationModel>(enumerator.Current));
            }

            return observations;
        }

        public async Task AddObservationAsync(EnergyObservationModel flashObservation)
        {
            await Task.Run(() => _storage.Add(_mapper.Map<IEnergyObservation>(flashObservation)));
        }

        public async Task DeleteObservationAsync(EnergyObservationModel flashObservation)
        {
            await Task.Run(() => _storage.Remove(_mapper.Map<IEnergyObservation>(flashObservation)));
        }

        public async Task ClearObservationsAsync()
        {
            await Task.Run(() => _storage.Clear());
        }
    }
}
