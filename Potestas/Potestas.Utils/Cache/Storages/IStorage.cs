using Potestas.Utils.Cache.Model;
using System.Collections.Generic;

namespace Potestas.Utils.Cache.Storages
{
    public interface IStorage<TKey, TValue>
    {
        int CachedItemCount { get; }
        int Capacity { get; }
        bool IsCachedItem(TKey key);
        CachedItemModel<TValue> Get(TKey key);
        IEnumerable<KeyValuePair<TKey, CachedItemModel<TValue>>> GetAll();
        void Add(TKey key, TValue value, int lifeTimeInSeconds);
        void Remove(TKey key);
        void Clear();
    }
}
