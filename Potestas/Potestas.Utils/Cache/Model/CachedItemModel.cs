using System;

namespace Potestas.Utils.Cache.Model
{
    public class CachedItemModel<TValue>
    {

        public TValue Value { get; set; }

        public DateTime LastAccessTime { get; set; }

        public DateTime ExpirationTime { get; set; }

        public bool IsExpired() => DateTime.Now >= ExpirationTime;
    }
}
