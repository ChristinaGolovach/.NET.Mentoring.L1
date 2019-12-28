using Potestas.Utils.Cache.Model;
using System;
using System.Collections.Generic;

namespace Potestas.Utils.Cache.Storages
{
    public class InMemoryStorage<TKey, TValue> : IStorage<TKey, TValue>
    {
        private IDictionary<TKey, CachedItemModel<TValue>> _cachedItems;

        public int CachedItemCount { get; private set; }
        public int Capacity { get; private set; }

        public InMemoryStorage(int capacity = 25, IEqualityComparer<TKey> keyEqualityComparer = null)
        {
            if (capacity <= 0)
            {
                throw new ArgumentOutOfRangeException($"The {nameof(capacity)} can not be lezz one.");
            }

            keyEqualityComparer = keyEqualityComparer ?? EqualityComparer<TKey>.Default;
            Capacity = capacity;
            _cachedItems = new Dictionary<TKey, CachedItemModel<TValue>>(capacity, keyEqualityComparer);
        }

        public bool IsCachedItem(TKey key)
        {
            CheckKey(key);

            return _cachedItems.ContainsKey(key);
        }

        public CachedItemModel<TValue> Get(TKey key)
        {
            CheckKey(key);

            CachedItemModel<TValue> cachedItem;

            if (!_cachedItems.TryGetValue(key, out cachedItem))
            {
                throw new ArgumentException($"The data with key {key} is not found.");
            }

            cachedItem.LastAccessTime = DateTime.Now;

            return cachedItem;
        }

        public IEnumerable<KeyValuePair<TKey, CachedItemModel<TValue>>> GetAll() => _cachedItems;

        public void Add(TKey key, TValue value, int lifeTimeInSeconds)
        {
            CheckKey(key);

            if (lifeTimeInSeconds <= 0)
            {
                throw new ArgumentOutOfRangeException($"The {nameof(lifeTimeInSeconds)} can not be less one.");
            }

            if (_cachedItems.ContainsKey(key))
            {
                throw new ArgumentException($"The item with key {key} already exists.");
            }

            var item = new CachedItemModel<TValue>()
            {
                Value = value,
                ExpirationTime = DateTime.Now.AddSeconds(lifeTimeInSeconds),
                LastAccessTime = DateTime.Now
            };

            _cachedItems.Add(key, item);
            CachedItemCount++;
        }

        public void Remove(TKey key)
        {
            CheckKey(key);

            if (!_cachedItems.ContainsKey(key))
            {
                throw new ArgumentException($"The item with {key} not found.");
            }

            _cachedItems.Remove(key);
            CachedItemCount--;
        }

        public void Clear()
        {
            _cachedItems.Clear();
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
