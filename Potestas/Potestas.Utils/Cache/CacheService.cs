using System;
using System.Collections.Generic;
using System.Linq;
using Potestas.Utils.Cache.Model;
using Potestas.Utils.Cache.Storages;

namespace Potestas.Utils.Cache
{
    public class CacheService<TKey, TItem> : ICacheService<TKey, TItem>
    {       
        private int _cacheCapacity;
        private IStorage<TKey, TItem> _cacheStorage;

        public CacheService(IStorage<TKey, TItem> cacheStorage)
        {
            _cacheStorage = cacheStorage ?? throw new ArgumentNullException();

            _cacheCapacity = cacheStorage.Capacity;
        }

        public void AddItem(TKey key, TItem item, int lifeTimeInSeconds)
        {
            CheckKey(key);

            if (lifeTimeInSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (_cacheStorage.IsCachedItem(key))
            {
                _cacheStorage.Remove(key);
            }

            if (_cacheCapacity == _cacheStorage.CachedItemCount)
            {
                RemoveLastAccessedItem();
            }

            _cacheStorage.Add(key, item, lifeTimeInSeconds);
        }

        public TItem GetItem(TKey key)
        {
            CheckKey(key);

            CachedItemModel<TItem> cachedItem = null;

            if (!_cacheStorage.IsCachedItem(key))
            {
                throw new ArgumentException($"The data with key {key} is not found.");
            }

            cachedItem = _cacheStorage.Get(key);

            if (cachedItem.IsExpired())
            {
                _cacheStorage.Remove(key);
            }

            return cachedItem.Value;
        }

        public void DeleteItem(TKey key)
        {
            CheckKey(key);

            if (_cacheStorage.IsCachedItem(key))
            {
                _cacheStorage.Remove(key);
            }
        }

        public void Clear() => _cacheStorage.Clear();

        private void RemoveLastAccessedItem()
        {
            DateTime minLastAccessItemTime = _cacheStorage.GetAll().Min(item => item.Value.LastAccessTime);

            var lastAccessedItem = _cacheStorage.GetAll().First(item => item.Value.LastAccessTime == minLastAccessItemTime);

            _cacheStorage.Remove(lastAccessedItem.Key);
        }

        private void CheckKey(TKey key)
        {
            if (EqualityComparer<TKey>.Default.Equals(key, default(TKey)))
            {
                throw new ArgumentNullException($"The {nameof(key)} can not be null.");
            }
        }
    }
}
