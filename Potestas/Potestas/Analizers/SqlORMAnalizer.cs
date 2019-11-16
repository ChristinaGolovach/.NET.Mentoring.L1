using Microsoft.EntityFrameworkCore;
using Potestas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Potestas.Analizers
{
    public class SqlORMAnalizer : IEnergyObservationAnalizer
    {
        private readonly DbContext _dbContext;
        public SqlORMAnalizer(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException($"the {nameof(dbContext)} can not be null.");
        }
        public double GetAverageEnergy()
        {
           return  _dbContext.Set<EnergyObservations>().Average(obs => obs.EstimatedValue);
        }

        public double GetAverageEnergy(DateTime startFrom, DateTime endBy)
        {
            return _dbContext.Set<EnergyObservations>().Where(obs => obs.ObservationTime >= startFrom && endBy >= obs.ObservationTime)
                                                       .Average(obs => obs.EstimatedValue);
        }

        public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight)
        {
            return _dbContext.Set<EnergyObservations>().Include(obs => obs.Coordinate)
                                                       .Where(obs => obs.Coordinate.X > rectTopLeft.X
                                                                     && obs.Coordinate.X < rectBottomRight.X
                                                                     && obs.Coordinate.Y > rectBottomRight.Y
                                                                     && obs.Coordinate.Y < rectTopLeft.Y)
                                                       .Average(obs => obs.EstimatedValue);
        }

        public IDictionary<Coordinates, int> GetDistributionByCoordinates()
        {
            throw new NotImplementedException();
        }

        public IDictionary<double, int> GetDistributionByEnergyValue()
        {
            throw new NotImplementedException();
        }

        public IDictionary<DateTime, int> GetDistributionByObservationTime()
        {
            throw new NotImplementedException();
        }

        public double GetMaxEnergy()
        {
            return _dbContext.Set<EnergyObservations>().Max(obs => obs.EstimatedValue);
        }

        public double GetMaxEnergy(Coordinates coordinates)
        {
            return _dbContext.Set<EnergyObservations>().Where(obs => obs.CoordinateId == coordinates.Id)
                                                       .Average(obs => obs.EstimatedValue);
        }

        public double GetMaxEnergy(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public Coordinates GetMaxEnergyPosition()
        {
            throw new NotImplementedException();
        }

        public DateTime GetMaxEnergyTime()
        {
            throw new NotImplementedException();
        }

        public double GetMinEnergy()
        {
            throw new NotImplementedException();
        }

        public double GetMinEnergy(Coordinates coordinates)
        {
            throw new NotImplementedException();
        }

        public double GetMinEnergy(DateTime dateTime)
        {
            throw new NotImplementedException();
        }

        public Coordinates GetMinEnergyPosition()
        {
            throw new NotImplementedException();
        }

        public DateTime GetMinEnergyTime()
        {
            throw new NotImplementedException();
        }
    }
}
