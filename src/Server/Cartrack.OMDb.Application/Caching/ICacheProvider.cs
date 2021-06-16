using System.Collections.Generic;

namespace Cartrack.OMDb.Application.Caching
{
    public interface ICacheProvider<TItem>
    {
        IEnumerable<TItem> GetAllItems();
        bool TryGetValue(string key, out TItem item);
        void AddOrUpdate(string key, TItem item);
        void Delete(string key);
    }
}