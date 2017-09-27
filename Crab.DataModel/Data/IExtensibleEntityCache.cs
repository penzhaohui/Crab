using System;
using System.Collections.Generic;
using System.Text;

namespace Crab.DataModel.Data
{
    public interface IExtensibleEntityCache
    {
        DataModelWorkspace DataModelWorkspace { get;}

        EntityCacheEntry GetCacheEntry(EntityKey key);

        IEnumerable<EntityCacheEntry> GetCacheEntries(EntityRowState state);

        bool TryGetCacheEntry(EntityKey key, out EntityCacheEntry cacheEntry);
    }
}
