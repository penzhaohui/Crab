using Crab.DataModel.Data;

namespace Crab.DataModel.Common
{
    internal delegate void EntityChangedHandler(EntityCacheEntry cacheEntry, string fieldName, object value);
}
