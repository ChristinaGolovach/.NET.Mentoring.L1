using System.Collections.Generic;
using System.Threading.Tasks;
using Potestas.API.ViewModels;

namespace Potestas.API.Services
{
    public interface IEnergyObservationService
    {
        Task<IEnumerable<EnergyObservationModel>> GetAllObservationsAsync();

        Task<int> GetCountAsync();

        Task<bool> CheckExistenceAsync(EnergyObservationModel flashObservation);

        Task AddObservationAsync(EnergyObservationModel flashObservation);

        Task DeleteObservationAsync(EnergyObservationModel flashObservation);

        Task ClearObservationsAsync();
    }
}
