using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using Potestas.Extensions;
using Potestas.MongoDB.Plugin.Entities;
using Potestas.MongoDB.Plugin.Mappers;
using Potestas.Validators;

namespace Potestas.MongoDB.Plugin.Storages
{
    public class MongoDBStorage<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        private readonly BsonDocument _getAllFilter;
        private string _connectionString;
        private MongoClient _dbClient;
        private IMongoDatabase _database;
        private IMongoCollection<BsonEnergyObservation> _collection;

        public string Description => "MongoDB storage.";

        public int Count => unchecked((int)_collection.CountDocuments(_getAllFilter));

        public bool IsReadOnly => false;

        public MongoDBStorage(string connectionString, string dbName, string collectionName)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException($"{nameof(_connectionString)} can not be null.");
            dbName = dbName ?? throw new ArgumentNullException($"{nameof(dbName)} can not be null.");
            collectionName = collectionName ?? throw new ArgumentNullException($"{nameof(collectionName)} can not be null.");

            _dbClient = new MongoClient(connectionString);
            _database = _dbClient.GetDatabase(dbName);
            _collection = _database.GetCollection<BsonEnergyObservation>(collectionName);
            _getAllFilter = new BsonDocument();
        }

        public void Add(T item)
        {
            GenericValidator.CheckInitialization(item, nameof(item));

            _collection.InsertOne(item.ToBsonEntity());            
        }

        public void Clear()
        {
            _collection.DeleteMany(_getAllFilter);
        }

        public bool Remove(T item)
        {
            GenericValidator.CheckInitialization(item, nameof(item));

            var deleteResult = _collection.DeleteOne(observation => observation.Id == item.Id);

            if (!deleteResult.IsAcknowledged)
            {
                return false;
            }

            return deleteResult.DeletedCount > 0 ? true : false;
        }

        public bool Contains(T item)
        {
            GenericValidator.CheckInitialization(item, nameof(item));

            return _collection.AsQueryable().Any(observation => observation.Id == item.Id);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            array = array ?? throw new ArgumentNullException($"The {nameof(array)} can not be null.");

            if (arrayIndex < 0)
            {
                throw new ArgumentOutOfRangeException($"The {nameof(arrayIndex)} can not be less than 0.");
            }

            if (array.Length - arrayIndex < Count)
            {
                throw new ArgumentException($"The available space in {nameof(array)} is not enough.");
            }

            var observations = _collection.Find(_getAllFilter)
                                          .ToEnumerable()
                                          .ConvertObservationCollectionToGeneric<T, BsonEnergyObservation>();


            observations.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.Find(_getAllFilter)
                              .ToEnumerable()
                              .ConvertObservationCollectionToGeneric<T, BsonEnergyObservation>()
                              .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
