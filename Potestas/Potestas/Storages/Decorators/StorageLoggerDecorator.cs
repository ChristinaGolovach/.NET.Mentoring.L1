using Potestas.Utils;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Potestas.Storages
{
    public class StorageLoggerDecorator<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        private readonly IEnergyObservationStorage<T> _storage;
        private readonly ILoggerManager _loggerManager;

        public StorageLoggerDecorator(IEnergyObservationStorage<T> storage, ILoggerManager loggerManager)
        {
            _storage = storage ?? throw new ArgumentNullException();
            _loggerManager = loggerManager ?? throw new ArgumentNullException();
        }

        public string Description => _storage.Description;

        public int Count => _storage.Count;

        public bool IsReadOnly => _storage.IsReadOnly;

        public void Add(T item) => Helper.Run(() => _storage.Add(item), _loggerManager, $"Add with value {item}");

        public void Clear() => Helper.Run(() => _storage.Clear(), _loggerManager, "Clear");

        public bool Contains(T item) 
            => Helper.Run(() => _storage.Contains(item), _loggerManager, $"Contains with value {item}");


        public void CopyTo(T[] array, int arrayIndex) 
            => Helper.Run(() => _storage.CopyTo(array, arrayIndex), _loggerManager, $"CopyTo with arrayIndex {arrayIndex}");


        public IEnumerator<T> GetEnumerator()
            => Helper.Run(() => _storage.GetEnumerator(), _loggerManager, "GetEnumerator");

        public bool Remove(T item)
            => Helper.Run(() => _storage.Remove(item), _loggerManager, $"Remove with value {item}");

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
