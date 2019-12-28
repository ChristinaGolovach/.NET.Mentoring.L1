using Potestas.Utils;
using System;
using System.Collections.Generic;

namespace Potestas.Analizers
{
    public class AnalizerLoggerDecorator : IEnergyObservationAnalizer
    {
        private readonly IEnergyObservationAnalizer _analizer;
        private readonly ILoggerManager _loggerManager;

        public AnalizerLoggerDecorator(IEnergyObservationAnalizer analizer, ILoggerManager loggerManager)
        {
            _analizer = analizer ?? throw new ArgumentNullException();
            _loggerManager = loggerManager ?? throw new ArgumentNullException();
        }

        public double GetAverageEnergy() 
            => Helper.Run(() => _analizer.GetAverageEnergy(), _loggerManager, "GetAverageEnergy");


        public double GetAverageEnergy(DateTime startFrom, DateTime endBy) 
            => Helper.Run(() => _analizer.GetAverageEnergy(startFrom, endBy), _loggerManager, $"GetAverageEnergy with value: {startFrom} {endBy}");


        public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight)
            => Helper.Run(() => _analizer.GetAverageEnergy(rectTopLeft, rectBottomRight), _loggerManager, $"GetAverageEnergy with value: {rectTopLeft} {rectBottomRight}");


        public IDictionary<Coordinates, int> GetDistributionByCoordinates()
            => Helper.Run(() => _analizer.GetDistributionByCoordinates(), _loggerManager, "GetDistributionByCoordinates");


        public IDictionary<double, int> GetDistributionByEnergyValue()
            => Helper.Run(() => _analizer.GetDistributionByEnergyValue(), _loggerManager, "GetDistributionByEnergyValue");


        public IDictionary<DateTime, int> GetDistributionByObservationTime()
            =>Helper. Run(() => _analizer.GetDistributionByObservationTime(), _loggerManager, "GetDistributionByObservationTime");


        public double GetMaxEnergy()
            => Helper.Run(() => _analizer.GetMaxEnergy(), _loggerManager, "GetMaxEnergy");


        public double GetMaxEnergy(Coordinates coordinates)
            => Helper.Run(() => _analizer.GetMaxEnergy(coordinates), _loggerManager, $"GetMaxEnergy with value {coordinates}");


        public double GetMaxEnergy(DateTime dateTime)
            => Helper.Run(() => _analizer.GetMaxEnergy(dateTime), _loggerManager, $"GetMaxEnergy with value {dateTime}");


        public Coordinates GetMaxEnergyPosition()
            => Helper.Run(() => _analizer.GetMaxEnergyPosition(), _loggerManager, "GetMaxEnergyPosition");


        public DateTime GetMaxEnergyTime()
            => Helper.Run(() => _analizer.GetMaxEnergyTime(), _loggerManager, "GetMaxEnergyTime");


        public double GetMinEnergy()
            => Helper.Run(() => _analizer.GetMinEnergy(), _loggerManager, "GetMinEnergy");


        public double GetMinEnergy(Coordinates coordinates)
            => Helper.Run(() => _analizer.GetMinEnergy(coordinates), _loggerManager, $"GetMinEnergy with value {coordinates}");


        public double GetMinEnergy(DateTime dateTime)
            => Helper.Run(() => _analizer.GetMinEnergy(dateTime), _loggerManager, $"GetMinEnergy with value {dateTime}");


        public Coordinates GetMinEnergyPosition()
            => Helper.Run(() => _analizer.GetMinEnergyPosition(), _loggerManager, "GetMinEnergyPosition");


        public DateTime GetMinEnergyTime()
            => Helper.Run(() => _analizer.GetMinEnergyTime(), _loggerManager, "GetMinEnergyTime");
    }
}
