using System.Collections.Generic;
using System;

namespace Cartrack.OMDb.Application.Caching
{
    public class InMemoryCacheProvider<TItem> : ICacheProvider<TItem>
    {
        private Dictionary<string, TItem> _cache = new Dictionary<string, TItem>();

        private readonly object _lockObject = new object();

        public IEnumerable<TItem> GetAllItems()
        {
            return _cache.Values;
        }

        public bool TryGetValue(string key, out TItem item)
        {
            return _cache.TryGetValue(key, out item);
        }

        public void AddOrUpdate(string key, TItem item)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException("Cannot add/update an entry to the cache with null, empty or whitespace key.", new ArgumentNullException(nameof(key)));
            }

            lock (_lockObject)
            {
                if (_cache.ContainsKey(key))
                {
                    _cache[key] = item;
                }
                else
                {
                    _cache.Add(key, item);
                }
            }
        }

        public void Delete(string key)
        {
            lock (_lockObject)
            {
                if (_cache.ContainsKey(key))
                {
                    _cache.Remove(key);
                }
            }
        }        
    }
}