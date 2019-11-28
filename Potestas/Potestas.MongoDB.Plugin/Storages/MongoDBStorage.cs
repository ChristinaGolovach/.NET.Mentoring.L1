using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace Potestas.MongoDB.Plugin.Storages
{
    public class MongoDBStorage<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        private string _connectionString;
        private MongoClient _dbClient;
        private IMongoDatabase _database;

        public string Description => "MongoDB storage.";

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public MongoDBStorage(string connectionString, string dbName)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException($"{nameof(_connectionString)} can not be null.");
            dbName = dbName ?? throw new ArgumentNullException($"{nameof(dbName)} can not be null.");

            _dbClient = new MongoClient(connectionString);
            _database = _dbClient.GetDatabase(dbName);
        }

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
