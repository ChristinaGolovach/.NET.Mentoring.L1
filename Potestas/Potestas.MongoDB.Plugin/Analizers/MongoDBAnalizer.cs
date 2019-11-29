using MongoDB.Driver;
using Potestas.MongoDB.Plugin.Entities;
using Potestas.MongoDB.Plugin.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Potestas.MongoDB.Plugin.Analizers
{
    //https://mongodb.github.io/mongo-csharp-driver/2.1/reference/driver/crud/linq/

    public class MongoDBAnalizer : IEnergyObservationAnalizer
    {
        private IMongoCollection<BsonEnergyObservation> _dbCollection;

        public MongoDBAnalizer(IMongoCollection<BsonEnergyObservation> dbCollection)
        {
            _dbCollection = dbCollection ?? throw new ArgumentNullException($"The {nameof(dbCollection)} can not be null.");
        }
        public double GetAverageEnergy()
        {
           return _dbCollection.AsQueryable().Average(obs => obs.EstimatedValue);
        }

        public double GetAverageEnergy(DateTime startFrom, DateTime endBy)
        {
            return _dbCollection.AsQueryable().Where(obs => obs.ObservationTime >= startFrom && endBy >= obs.ObservationTime)
                                              .Average(obs => obs.EstimatedValue);
        }

        public double GetAverageEnergy(Coordinates rectTopLeft, Coordinates rectBottomRight)
        {
            return _dbCollection.AsQueryable().Where(obs => obs.ObservationPoint.X > rectTopLeft.X
                                                            && obs.ObservationPoint.X < rectBottomRight.X
                                                            && obs.ObservationPoint.Y > rectBottomRight.Y
                                                            && obs.ObservationPoint.Y < rectTopLeft.Y)
                                              .Average(obs => obs.EstimatedValue);
        }

        public IDictionary<Coordinates, int> GetDistributionByCoordinates()
        {
            var bsonGruppedCollection =  _dbCollection.AsQueryable()
                                                      .GroupBy(obs => obs.ObservationPoint)
                                                      .ToDictionary(k => k.Key, v => v.Count());

            var resultCollection = new Dictionary<Coordinates, int>();

            foreach (var item in bsonGruppedCollection)
            {
                resultCollection.Add(item.Key.ToDomainEntity(), item.Value);
            }

            return resultCollection;
        }

        public IDictionary<double, int> GetDistributionByEnergyValue()
        {
            return _dbCollection.AsQueryable().GroupBy(obs => obs.EstimatedValue)
                                              .ToDictionary(k => k.Key, v => v.Count());
        }

        public IDictionary<DateTime, int> GetDistributionByObservationTime()
        {
            return _dbCollection.AsQueryable().GroupBy(obs => obs.ObservationTime)
                                              .ToDictionary(k => k.Key, v => v.Count());
        }

        public double GetMaxEnergy()
        {
            return _dbCollection.AsQueryable().Max(obs => obs.EstimatedValue);
        }

        public double GetMaxEnergy(Coordinates coordinates)
        {
            // TODO проверить
            // на рантайме может упасть, т.к внутри Where исп-ся дерево выражений и не все лямбды парсятся (у меня внутри вызов кастомного ToDomainEntity)
            return _dbCollection.AsQueryable().Where(obs => obs.ObservationPoint.ToDomainEntity() == coordinates)
                                              .Max(obs => obs.EstimatedValue);
        }

        public double GetMaxEnergy(DateTime dateTime)
        {
            return _dbCollection.AsQueryable().Where(obs => obs.ObservationTime == dateTime)
                                              .Max(obs => obs.EstimatedValue);
        }

        public Coordinates GetMaxEnergyPosition()
        {
            var bsonCoordinates =  _dbCollection.AsQueryable()
                                                .OrderByDescending(obs => obs.EstimatedValue)                
                                                .First().ObservationPoint;

            return bsonCoordinates.ToDomainEntity();
        }

        public DateTime GetMaxEnergyTime()
        {
            return _dbCollection.AsQueryable().OrderByDescending(obs => obs.EstimatedValue)
                                              .First().ObservationTime;
        }

        public double GetMinEnergy()
        {
            return _dbCollection.AsQueryable().Min(obs => obs.EstimatedValue);
        }

        public double GetMinEnergy(Coordinates coordinates)
        {
            // TODO проверить
            // на рантайме может упасть, т.к внутри Where иcп-ся дерево выражений и не все лямбды парсятся (у меня внутри вызов кастомного ToDomainEntity)
            return _dbCollection.AsQueryable().Where(obs => obs.ObservationPoint.ToDomainEntity() == coordinates)
                                              .Min(obs => obs.EstimatedValue);
        }

        public double GetMinEnergy(DateTime dateTime)
        {
            return _dbCollection.AsQueryable().Where(obs => obs.ObservationTime == dateTime)
                                              .Min(obs => obs.EstimatedValue);
        }

        public Coordinates GetMinEnergyPosition()
        {
            var bsonCoordinates = _dbCollection.AsQueryable()
                                               .OrderBy(obs => obs.EstimatedValue)
                                               .First().ObservationPoint;

            return bsonCoordinates.ToDomainEntity();
        }

        public DateTime GetMinEnergyTime()
        {
            return _dbCollection.AsQueryable()
                                .OrderBy(obs => obs.EstimatedValue)
                                .First().ObservationTime;
        }
    }
}
