using System;
using System.Collections.Generic;
using System.Text;
using Crab.DataModel;

namespace Crab.DataModel.Data
{
    public class ExtensibleObjectCache: IExtensibleEntityCache
    {
        private DataModelWorkspace _workspace;
        private Dictionary<EntityKey, EntityCacheEntry> _addedEntityStore;
        private Dictionary<EntityKey, EntityCacheEntry> _deletedEntityStore;
        private Dictionary<EntityKey, EntityCacheEntry> _modifiedEntityStore;
        private Dictionary<EntityKey, EntityCacheEntry> _unchangedEntityStore;

        public ExtensibleObjectCache(DataModelWorkspace workspace)
        {
            if (workspace == null)
                throw new ArgumentNullException("workspace");
            this._workspace = workspace;
            this._addedEntityStore = new Dictionary<EntityKey, EntityCacheEntry>();
            this._deletedEntityStore = new Dictionary<EntityKey, EntityCacheEntry>(); this._addedEntityStore = new Dictionary<EntityKey, EntityCacheEntry>();
            this._modifiedEntityStore = new Dictionary<EntityKey, EntityCacheEntry>();
            this._unchangedEntityStore = new Dictionary<EntityKey, EntityCacheEntry>();
        }

        #region implements IExtensibleEntityCache
        public DataModelWorkspace DataModelWorkspace
        {
            get { return _workspace; }
        }

        public EntityCacheEntry GetCacheEntry(EntityKey key)
        {
            EntityCacheEntry entry = this.FindCacheEntry(key);
            if (entry == null)
                throw new ArgumentException("No entry exists for EntityKey in the object cache");
            return entry;
        }

        /// <summary>
        /// Get the EntityCacheEntry list by the EntityRowState flags
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public IEnumerable<EntityCacheEntry> GetCacheEntries(EntityRowState state)
        {
            List<EntityCacheEntry> entryList = new List<EntityCacheEntry>();
            if ((EntityRowState.Detached & state) != (EntityRowState)0)
                throw new ArgumentException("Detached cache entries does not exist in cache");
            if ((EntityRowState.Added & state) != ((EntityRowState)0))
                entryList.AddRange(_addedEntityStore.Values);

            if ((EntityRowState.Modified & state) != ((EntityRowState)0))
                entryList.AddRange(_modifiedEntityStore.Values);

            if ((EntityRowState.Deleted & state) != ((EntityRowState)0))
                entryList.AddRange(_deletedEntityStore.Values);

            if ((EntityRowState.Unchanged & state) != ((EntityRowState)0))
                entryList.AddRange(_unchangedEntityStore.Values);

            return entryList;
        }

        public bool TryGetCacheEntry(EntityKey key, out EntityCacheEntry cacheEntry)
        {
            cacheEntry = this.FindCacheEntry(key);
            return (cacheEntry != null);
        }

        #endregion

        internal EntityCacheEntry FindCacheEntry(EntityKey key)
        {
            if (key == null)
                throw new ArgumentNullException("key");
            EntityCacheEntry entry = null;
            if ((!this._addedEntityStore.TryGetValue(key, out entry) &&
                !this._modifiedEntityStore.TryGetValue(key, out entry)) &&
                !this._deletedEntityStore.TryGetValue(key, out entry))
            {
                this._unchangedEntityStore.TryGetValue(key, out entry);
            }
            return entry;
        }

        internal EntityCacheEntry Add(IExtensibleEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            return this.AddEntry(entity, true);
        }

        internal EntityCacheEntry AddEntry(IExtensibleEntity entity, bool newInstance)
        {
            //if (entity.Key == null)
             //   throw new ArgumentNullException("entity.Key");
            //TODO: If auto generate key is setted, generate the key values first 
            //TODO: create entity key with key values
            if (entity.Key == null)
            {
                //create entity key
                entity.Key = new EntityKey((entity as ExtensibleEntity).Metadata, (entity as ExtensibleEntity).KeyValues);
            }

            //see whether these's a key confliction
            EntityCacheEntry entry = this.FindCacheEntry(entity.Key);
            if (entry != null)
            {
                if (entry.Entity != entity) //key conflict
                {
                    throw new ArgumentException("key already exists", "entity");
                }
                if (entry.State == EntityRowState.Deleted)
                {
                    //entry.RevertDelete();
                }
                return entry;
            }
            entry = new EntityCacheEntry(entity, this, newInstance);
            entry.AttachCacheToEntity();
            this.AddCacheEntryToDictionary(entry, entry.State);
            return entry;
        }

        private void AddCacheEntryToDictionary(EntityCacheEntry entry, EntityRowState state)
        {
            switch (state)
            {
                case EntityRowState.Unchanged:
                     _unchangedEntityStore[entry.Entity.Key] = entry;
                     return;
                case (EntityRowState.Unchanged|EntityRowState.Detached):
                    return;
                case EntityRowState.Added:
                    _addedEntityStore[entry.Entity.Key] = entry;
                    return;
                case EntityRowState.Deleted:
                    _deletedEntityStore[entry.Entity.Key] = entry;
                    return;
                case EntityRowState.Modified:
                    _modifiedEntityStore[entry.Entity.Key] = entry;
                    return;
                default:
                    return;
            }
        }

        private void RemoveCacheEntryFromDictionary(EntityCacheEntry entry, EntityRowState state)
        {
            switch (state)
            {
                case EntityRowState.Unchanged:
                    _unchangedEntityStore.Remove(entry.Entity.Key);
                    return;

                case (EntityRowState.Unchanged | EntityRowState.Detached):
                    return;

                case EntityRowState.Added:
                    _addedEntityStore.Remove(entry.Entity.Key);
                    return;

                case EntityRowState.Deleted:
                    _deletedEntityStore.Remove(entry.Entity.Key);
                    return;

                case EntityRowState.Modified:
                    _modifiedEntityStore.Remove(entry.Entity.Key);
                    return;
            }
        }

        internal void ChangeState(EntityCacheEntry entry, EntityRowState oldState, EntityRowState newState)
        {
            if (newState == EntityRowState.Detached)
            {
                RemoveCacheEntryFromDictionary(entry, oldState);
                entry.Reset();
            }
            else
            {
                RemoveCacheEntryFromDictionary(entry, oldState);
                AddCacheEntryToDictionary(entry, newState);
            }
        }
    }
}
