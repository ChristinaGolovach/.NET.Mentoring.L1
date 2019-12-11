using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Potestas.API.ViewModels;

namespace Potestas.API.Services.Implementations
{
    public class ResearcherService : IResearcherService
    {
        private readonly IEnergyObservationAnalizer _analizer;
        private readonly IMapper _mapper;

        public ResearcherService(IEnergyObservationAnalizer analizer, IMapper mapper)
        {
            _analizer = analizer;
            _mapper = mapper;
        }

        public async Task<double> GetAverageEnergyAsync()
            => await Task.Run(() => _analizer.GetAverageEnergy());

        public async Task<double> GetAverageEnergyAsync(DateTime startFrom, DateTime endBy)
            => await Task.Run(() => _analizer.GetAverageEnergy(startFrom, endBy));   

        public async Task<double> GetAverageEnergyAsync(CoordinatesModel firstCoordinate, CoordinatesModel secondCoordinate)
        {
            var rectTopLeft = _mapper.Map<Coordinates>(firstCoordinate);
            var rectBottomRight = _mapper.Map<Coordinates>(secondCoordinate);

            return await Task.Run(() => _analizer.GetAverageEnergy(rectTopLeft, rectBottomRight));
        }

        public async Task<IDictionary<CoordinatesModel, int>> GetDistributionByCoordinatesAsync()
        {
            var distribution = await Task.Run(() => _analizer.GetDistributionByCoordinates());

            return distribution.ToDictionary(keyValue => _mapper.Map<CoordinatesModel>(keyValue.Key), 
                                             keyValue => keyValue.Value);
        }


        public async Task<IDictionary<double, int>> GetDistributionByEnergyValueAsync()
            => await Task.Run(() => _analizer.GetDistributionByEnergyValue());

        public async Task<IDictionary<DateTime, int>> GetDistributionByObservationTimeAsync()
            => await Task.Run(() => _analizer.GetDistributionByObservationTime());

        public async Task<double> GetMaxEnergyAsync()
            => await Task.Run(() => _analizer.GetMaxEnergy());

        public async Task<double> GetMaxEnergyAsync(CoordinatesModel coordinate)
            => await Task.Run(() => _analizer.GetMaxEnergy(_mapper.Map<Coordinates>(coordinate)));

        public async Task<double> GetMaxEnergyAsync(DateTime dateTime)
            => await Task.Run(() => _analizer.GetMaxEnergy(dateTime));

        public async Task<CoordinatesModel> GetMaxEnergyPositionAsync()
        {
            var energyPosition = await Task.Run(() => _analizer.GetMaxEnergyPosition());

            return _mapper.Map<CoordinatesModel>(energyPosition);
        }

        public async Task<DateTime> GetMaxEnergyTimeAsync()
            => await Task.Run(() => _analizer.GetMaxEnergyTime());

        public async Task<double> GetMinEnergyAsync()
            => await Task.Run(() => _analizer.GetMinEnergy());

        public async Task<double> GetMinEnergyAsync(CoordinatesModel coordinate)
            => await Task.Run(() => _analizer.GetMinEnergy(_mapper.Map<Coordinates>(coordinate)));

        public async Task<double> GetMinEnergyAsync(DateTime dateTime)
            => await Task.Run(() => _analizer.GetMinEnergy(dateTime));

        public async Task<CoordinatesModel> GetMinEnergyPositionAsync()
        {
            var energyPosition = await Task.Run(() => _analizer.GetMinEnergyPosition());

            return _mapper.Map<CoordinatesModel>(energyPosition);
        }

        public async Task<DateTime> GetMinEnergyTimeAsync()
            => await Task.Run(() => _analizer.GetMinEnergyTime());
    }
}
