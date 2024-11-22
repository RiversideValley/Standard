using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Riverside.Runtime
{
    public class SimpleCache<TKey, TValue>(TimeSpan defaultExpiration)
    {
        private readonly ConcurrentDictionary<TKey, CacheItem> _cache = new();
        private readonly TimeSpan _defaultExpiration = defaultExpiration;

        public void Set(TKey key, TValue value, TimeSpan? expiration = null)
        {
            var cacheItem = new CacheItem(value, DateTime.UtcNow + (expiration ?? _defaultExpiration));
            _cache[key] = cacheItem;
        }

        public bool TryGet(TKey key, out TValue value)
        {
            if (_cache.TryGetValue(key, out var cacheItem) && cacheItem.Expiration > DateTime.UtcNow)
            {
                value = cacheItem.Value;
                return true;
            }

            value = default;
            return false;
        }

        public async Task<TValue> GetOrAddAsync(TKey key, Func<Task<TValue>> valueFactory, TimeSpan? expiration = null)
        {
            if (TryGet(key, out var value))
            {
                return value;
            }

            value = await valueFactory();
            Set(key, value, expiration);
            return value;
        }

        private class CacheItem
        {
            public CacheItem(TValue value, DateTime expiration)
            {
                Value = value;
                Expiration = expiration;
            }

            public TValue Value { get; }
            public DateTime Expiration { get; }
        }
    }
}
