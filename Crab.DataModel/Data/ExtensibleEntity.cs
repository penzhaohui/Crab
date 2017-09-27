using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Crab.DataModel.Common;

namespace Crab.DataModel.Data
{
    /// <summary>
    /// The base class for entity which also contains relationships
    /// This class could not been instantiated. 
    /// The child entity class must use a EntityClassAttribute or override the EntityClassName gets method to 
    /// specify the entity class name of the meta data
    /// </summary>
    [Serializable]
    public abstract class ExtensibleEntity : ExtensibleDataRow, IExtensibleEntity
    {
        private EntityCacheEntry _cacheEntry;
        private EntityKey _entityKey;
        private EntityChangedHandler _entityChangedHandler;

        public ExtensibleEntity()
        {
            Metadata = GetEntityMetadata(this.GetType());
        }
        /// <summary>
        /// Gets or sets the EntityKey
        /// </summary>
        public EntityKey Key
        {
            get
            {
                return this._entityKey;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                if (this._entityKey != null)
                    throw new InvalidOperationException("Can not reset entity key");
                this._entityKey = value;
            }

        }

        public ReadOnlyCollection<KeyValuePair<string, object>> KeyValues
        {
            get
            {
                if (_entityKey != null)
                    return _entityKey.KeyValues;
                else
                {
                    string[] keyNames = Metadata.KeyFieldNames;
                    KeyValuePair<string, object>[] keyValues = new KeyValuePair<string, object>[keyNames.Length];
                    for(int i=0; i<keyNames.Length; i++)
                    {
                        keyValues[i] = new KeyValuePair<string, object>(keyNames[i], GetValue(keyNames[i]));
                    }
                    return new ReadOnlyCollection<KeyValuePair<string, object>>(keyValues);
                }
            }
        }

        /// <summary>
        /// Gets the EntityRowState of the entity
        /// </summary>
        public EntityRowState EntityState 
        {
            get
            {
                if (this._cacheEntry == null)
                {
                    return EntityRowState.Detached;
                }
                return this._cacheEntry.State;
            }

        }

        /// <summary>
        /// Gets the cache object which cachs this entity
        /// </summary>
        protected ExtensibleObjectCache Cache
        {
            get
            {
                if (this._cacheEntry == null)
                {
                    return null;
                }
                return (this._cacheEntry.Cache as ExtensibleObjectCache);
            }
        }

        /// <summary>
        /// Gets the cache entry of the entity
        /// </summary>
        protected EntityCacheEntry CacheEntry
        {
            get
            {
                return this._cacheEntry;
            }
        }

        /// <summary>
        /// Delete the entity from cache and make a delete flag 
        /// which tells the object context to 
        /// delete the entity from database when the method ExtensibleObjectContext.SaveAllChanges is invoked
        /// </summary>
        public void Delete()
        {
            if (this._cacheEntry != null)
            {
                this._cacheEntry.Delete();
            }
        }

        /// <summary>
        /// Attach the entity to cache
        /// </summary>
        /// <param name="cacheEntry"></param>
        /// <param name="changed"></param>
        internal void AttachCache(EntityCacheEntry cacheEntry, EntityChangedHandler changed)
        {
            if (this._cacheEntry != null)
            {
                throw new InvalidOperationException("Entity cannot exist in multiple object caches");
            }
            this._cacheEntry = cacheEntry;
            this._entityChangedHandler = changed;
        }

        /// <summary>
        /// Detach the entity from cache
        /// </summary>
        internal void DetachCache()
        {
            this._entityChangedHandler = null;
            this._cacheEntry = null;
            this._entityKey = null;
            //TODO:this.Relationships.DetachContext();
        }

        protected override void ReportFieldChanged(string fieldName, object value)
        {
            if (this._entityChangedHandler != null)
            {
                this._entityChangedHandler(this._cacheEntry, fieldName, value);
            }
            base.ReportFieldChanged(fieldName, value);
        }

        /// <summary>
        /// Gets the name of the entity class. 
        /// The child class could override this method to specify a entity class
        /// </summary>
        /// <returns></returns>
        protected virtual string EntityClassName
        {
            get 
            {
                return ExtensibleEntity.GetEntityClassName(this.GetType());
            }
        }

        public static string GetEntityClassName(Type entityType)
        {
            EntityClassAttribute[] classAttributes =
                    (EntityClassAttribute[])entityType.GetCustomAttributes(typeof(EntityClassAttribute), false);
            if (classAttributes == null || classAttributes.Length == 0)
                return string.Empty;
            else
                return classAttributes[0].ClassName;
        }

        public static EntityMetadata GetEntityMetadata(Type entityType)
        {
            return DataModelWorkspace.Current.GetEntityMetadata(ExtensibleEntity.GetEntityClassName(entityType));
        }
    }
}
