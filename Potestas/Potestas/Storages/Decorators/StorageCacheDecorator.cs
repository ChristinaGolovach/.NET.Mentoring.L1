using System;
using System.Collections;
using System.Collections.Generic;
using Potestas.Utils.Cache;
using Potestas.Validators;

namespace Potestas.Storages
{
    public class StorageCacheDecorator<T> : IEnergyObservationStorage<T> where T : IEnergyObservation
    {
        public readonly int cacheLifeTimeInSeconds = 600; 
        private readonly IEnergyObservationStorage<T> _storage;
        private readonly ICacheService<int, T> _cacheService;

        public StorageCacheDecorator(IEnergyObservationStorage<T> storage, ICacheService<int,T> cacheService)
        {
            _storage = storage ?? throw new ArgumentNullException();
            _cacheService = cacheService ?? throw new ArgumentNullException();
        }

        public string Description => _storage.Description;

        public int Count => _storage.Count;

        public bool IsReadOnly => _storage.IsReadOnly;

        public void Add(T item)
        {
            GenericValidator.CheckInitialization(item, nameof(item));

            _cacheService.AddItem(item.Id, item, cacheLifeTimeInSeconds);
            _storage.Add(item);
        }

        public void Clear()
        {
            _cacheService.Clear();
            _storage.Clear();
        } 

        public bool Contains(T item)
        {
            GenericValidator.CheckInitialization(item, nameof(item));

            if (!EqualityComparer<T>.Default.Equals(_cacheService.GetItem(item.Id), default))
            {
                return true;
            }

            return _storage.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) => _storage.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => _storage.GetEnumerator();

        public bool Remove(T item)
        {
            GenericValidator.CheckInitialization(item, nameof(item));

            if (_storage.Remove(item))
            {
                _cacheService.DeleteItem(item.Id);
                return true;
            }

            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
