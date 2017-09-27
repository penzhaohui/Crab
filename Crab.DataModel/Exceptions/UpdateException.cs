using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using Crab.DataModel.Data;

namespace Crab.DataModel.Exceptions
{
    [Serializable]
    public class UpdateException: DataException
    {
        [NonSerialized]
        private ReadOnlyCollection<EntityCacheEntry> _cacheEntries;
 

        public UpdateException()
        {
        }

        public UpdateException(string message)
            : base(message)
        {
        }

        protected UpdateException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public UpdateException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public UpdateException(string message, Exception innerException, IEnumerable<EntityCacheEntry> cacheEntries)
            : base(message, innerException)
        {
            this._cacheEntries = new List<EntityCacheEntry>(cacheEntries).AsReadOnly();
        }


        public ReadOnlyCollection<EntityCacheEntry> CacheEntries
        {
            get
            {
                return this._cacheEntries;
            }
        }

        public EntityCacheEntry FirstCacheEntry
        {
            get
            {
                if ((this.CacheEntries != null) && (this.CacheEntries.Count > 0))
                {
                    return this.CacheEntries[0];
                }
                return null;
            }
        }
    }
}
