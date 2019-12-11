using Potestas.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Potestas.API.Services
{
    public interface IResearcherService
    {
        Task<IDictionary<double, int>> GetDistributionByEnergyValueAsync();

        Task<IDictionary<CoordinatesModel, int>> GetDistributionByCoordinatesAsync();

        Task<IDictionary<DateTime, int>> GetDistributionByObservationTimeAsync();

        Task<double> GetMaxEnergyAsync();

        Task<double> GetMaxEnergyAsync(CoordinatesModel coordinates);

        Task<double> GetMaxEnergyAsync(DateTime dateTime);

        Task<double> GetMinEnergyAsync();

        Task<double> GetMinEnergyAsync(CoordinatesModel coordinates);

        Task<double> GetMinEnergyAsync(DateTime dateTime);

        Task<double> GetAverageEnergyAsync();

        Task<double> GetAverageEnergyAsync(DateTime startFrom, DateTime endBy);

        Task<double> GetAverageEnergyAsync(CoordinatesModel rectTopLeft, CoordinatesModel rectBottomRight);

        Task<DateTime> GetMaxEnergyTimeAsync();

        Task<CoordinatesModel> GetMaxEnergyPositionAsync();

        Task<DateTime> GetMinEnergyTimeAsync();

        Task<CoordinatesModel> GetMinEnergyPositionAsync();
    }
}
