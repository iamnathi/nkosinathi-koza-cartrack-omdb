using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cartrack.OMDb.Application.Caching
{
    public class InMemoryCacheProvider<TItem> : ICacheProvider<TItem>
    {
        private Dictionary<string, TItem> _cache = new Dictionary<string, TItem>();

        private readonly object _lockObject = new object();

        public void Initialize(Dictionary<string, TItem> items)
        {
            lock (_lockObject)
            {
                _cache = items;
            }            
        }

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

        public async Task AddOrUpdateAsync(string key, Func<Task<TItem>> func)
        {
            var item = await func();

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