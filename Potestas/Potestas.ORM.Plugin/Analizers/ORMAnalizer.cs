using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Potestas.ORM.Plugin.Mappers;
using Potestas.ORM.Plugin.Models;

namespace Potestas.ORM.Plugin.Analizers
{
    public class ORMAnalizer : IEnergyObservationAnalizer
    {
        private readonly DbContext _dbContext;

        public ORMAnalizer(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException($"the {nameof(dbContext)} can not be null.");
        }
        public double GetAverageEnergy()
        {
            return _dbContext.Set<EnergyObservations>().Average(obs => obs.EstimatedValue);
        }

        public double GetAverageEnergy(DateTime startFrom, DateTime endBy)
        {
            return _dbContext.Set<EnergyObservations>().Where(obs => obs.ObservationTime >= startFrom && endBy >= obs.ObservationTime)
                                                       .Average(obs => obs.EstimatedValue);
        }

        public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight)
        {
            var result = _dbContext.Set<EnergyObservations>().Include(obs => obs.Coordinate);

            return _dbContext.Set<EnergyObservations>().Include(obs => obs.Coordinate)
                                                       .Where(obs => obs.Coordinate.X > rectTopLeft.X
                                                                     && obs.Coordinate.X < rectBottomRight.X
                                                                     && obs.Coordinate.Y > rectBottomRight.Y
                                                                     && obs.Coordinate.Y < rectTopLeft.Y)
                                                       .DefaultIfEmpty()
                                                       .Average(obs => obs.EstimatedValue);
        }

        public IDictionary<Coordinates, int> GetDistributionByCoordinates()
        {
            var ORMDistribution = _dbContext.Set<EnergyObservations>().Include(obs => obs.Coordinate)
                                                                      .GroupBy(obs => obs.Coordinate)
                                                                      .ToDictionary(k => k.Key, v => v.Count());

            var domainDistribution = new Dictionary<Coordinates, int>();

            foreach (var ormEntity in ORMDistribution)
            {
                domainDistribution.Add(ormEntity.Key.ToDomainEntity(), ormEntity.Value);
            }

            return domainDistribution;
        }

        public IDictionary<double, int> GetDistributionByEnergyValue()
        {
            return _dbContext.Set<EnergyObservations>().GroupBy(obs => obs.EstimatedValue)
                                                       .ToDictionary(k => k.Key, v => v.Count());
        }

        public IDictionary<DateTime, int> GetDistributionByObservationTime()
        {
            return _dbContext.Set<EnergyObservations>().GroupBy(obs => obs.ObservationTime)
                                                       .ToDictionary(k => k.Key, v => v.Count());
        }

        public double GetMaxEnergy()
        {
            return _dbContext.Set<EnergyObservations>().Max(obs => obs.EstimatedValue);
        }

        public double GetMaxEnergy(Coordinates coordinates)
        {
            return _dbContext.Set<EnergyObservations>().Where(obs => obs.CoordinateId == coordinates.Id)
                                                       .Max(obs => obs.EstimatedValue);
        }

        public double GetMaxEnergy(DateTime dateTime)
        {
            return _dbContext.Set<EnergyObservations>().Where(obs => obs.ObservationTime == dateTime)
                                                       .Max(obs => obs.EstimatedValue);
        }

        public Coordinates GetMaxEnergyPosition()
        {
            var ORMCoordinates = _dbContext.Set<EnergyObservations>().Include(obs => obs.Coordinate)
                                                                     .OrderByDescending(obs => obs.EstimatedValue)
                                                                     .AsQueryable()
                                                                     .First().Coordinate;

            return ORMCoordinates.ToDomainEntity();
        }

        public DateTime GetMaxEnergyTime()
        {
            return _dbContext.Set<EnergyObservations>().OrderByDescending(obs => obs.EstimatedValue)
                                                       .AsQueryable()
                                                       .First().ObservationTime;
        }

        public double GetMinEnergy()
        {
            return _dbContext.Set<EnergyObservations>().Min(obs => obs.EstimatedValue);
        }

        public double GetMinEnergy(Coordinates coordinates)
        {
            return _dbContext.Set<EnergyObservations>().Where(obs => obs.CoordinateId == coordinates.Id)
                                                       .Min(obs => obs.EstimatedValue);
        }

        public double GetMinEnergy(DateTime dateTime)
        {
            return _dbContext.Set<EnergyObservations>().Where(obs => obs.ObservationTime == dateTime)
                                                       .Min(obs => obs.EstimatedValue);
        }

        public Coordinates GetMinEnergyPosition()
        {
            var ORMCoordinates = _dbContext.Set<EnergyObservations>().Include(obs => obs.Coordinate)
                                                                      .OrderBy(obs => obs.EstimatedValue)
                                                                      .AsQueryable()
                                                                      .First().Coordinate;

            return ORMCoordinates.ToDomainEntity();
        }

        public DateTime GetMinEnergyTime()
        {
            return _dbContext.Set<EnergyObservations>().OrderBy(obs => obs.EstimatedValue)
                                                       .AsQueryable()
                                                       .First().ObservationTime;
        }
    }
}
