using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cartrack.OMDb.Application.Caching
{
    public interface ICacheProvider<TItem>
    {
        void Initialize(Dictionary<string, TItem> items);
        IEnumerable<TItem> GetAllItems();
        bool TryGetValue(string key, out TItem item);
        void AddOrUpdate(string key, TItem item);
        Task AddOrUpdateAsync(string key, Func<Task<TItem>> func);
        void Delete(string key);
    }
}
