using MongoDB.Bson;
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
            //return _dbCollection.AsQueryable().Where(obs => obs.ObservationPoint.ToDomainEntity() == coordinates)
            //                                  .Max(obs => obs.EstimatedValue);

            var builder = Builders<BsonEnergyObservation>.Filter;

            var filter = builder.Eq("observationPoint.x", coordinates.X) &
                         builder.Eq("observationPoint.y", coordinates.Y);

            var result = _dbCollection.Aggregate()
                                      .Match(filter)
                                      .Group(new BsonDocument { { "_id", "_id" }, { "estimatedValue", new BsonDocument { { "$max", "$estimatedValue" } } } })
                                      .First();

            return result["estimatedValue"].AsDouble;
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
            // TODO 
            //https://www.tutorialsteacher.com/linq/expression-tree
            //https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/expression-trees/
            //https://stackoverflow.com/questions/11258307/binaryexpression-contains-method
            //https://www.red-gate.com/simple-talk/dotnet/net-framework/giving-clarity-to-linq-queries-by-extending-expressions/
            //https://stackoverflow.com/questions/8337774/lambda-and-expression-call-for-an-extension-method/20340865

            //ParameterExpression peBsonEnergyObservation = Expression.Parameter(typeof(BsonEnergyObservation), "obs");
            //ParameterExpression peCoordinates = Expression.Parameter(typeof(Coordinates), "coordinates");
            //MemberExpression me = Expression.Property(peBsonEnergyObservation, "ObservationPoint");

            //var toDomainEntityMethod = typeof(BsonEnergyObservation).GetExtensionMethod("ToDomainEntity");

            //var toDomainEntityMethod = typeof(BsonEnergyObservation).GetMethod("ToDomainEntity", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null,  new[] {typeof(BsonEnergyObservation)}, null);
            //var methodCall = Expression.Call(toDomainEntityMethod, peBsonEnergyObservation);

            //BinaryExpression body = Expression.Equal(methodCall, peCoordinates);

            //var resultExpression = Expression.Lambda<Func<BsonEnergyObservation, bool>>(body, peBsonEnergyObservation);

            //return _dbCollection.AsQueryable().Where(resultExpression)
            //                                  .Min(obs => obs.EstimatedValue);

            //return _dbCollection.AsQueryable().Where(obs => obs.ObservationPoint.ToDomainEntity() == coordinates)
            //                                  .Min(obs => obs.EstimatedValue);

            var builder = Builders<BsonEnergyObservation>.Filter;

            var filter = builder.Eq("observationPoint.x", coordinates.X) & 
                         builder.Eq("observationPoint.y", coordinates.Y);

            var result = _dbCollection.Aggregate()
                                      .Match(filter)
                                      .Group(new BsonDocument {{"_id", "_id"}, {"estimatedValue", new BsonDocument {{"$min", "$estimatedValue"}}}})
                                      .First();

            return result["estimatedValue"].AsDouble;
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
