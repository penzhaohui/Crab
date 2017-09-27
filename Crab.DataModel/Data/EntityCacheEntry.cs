using System;
using System.Collections.Generic;
using System.Text;
using Crab.DataModel.Common;

namespace Crab.DataModel.Data
{
    public sealed class EntityCacheEntry
    {
        private EntityRowState _entryState;
        private IExtensibleEntity _entity;
        private ExtensibleObjectCache _cache;

        static EntityCacheEntry()
        {
            EntityCacheEntry.HandleEntityChanged = new EntityChangedHandler(EntityCacheEntry.EntityValueChangedHandler);
        }

        public EntityCacheEntry(IExtensibleEntity entity, ExtensibleObjectCache cache, bool newInstance)
        {
            _entryState = newInstance ? EntityRowState.Added : EntityRowState.Unchanged;
            _entity = entity;
            _cache = cache;
        }

        public ExtensibleObjectCache Cache
        {
            get
            {
                return _cache;
            }
        }

        public IExtensibleEntity Entity
        {
            get
            {
                return _entity;
            }
        }

        public EntityRowState State
        {
            get { return _entryState; }
            internal set
            {
                this._entryState = value;
            }

        }

        public void Delete()
        {
            Delete(true);
        }

        internal void Delete(bool doFixup)
        {
            switch (State)
            {
                case EntityRowState.Unchanged:
                    {
                        if (State != EntityRowState.Deleted)
                        {
                            _cache.ChangeState(this, EntityRowState.Unchanged, EntityRowState.Deleted);
                            _entryState = EntityRowState.Deleted;
                        }
                    } break;

                case (EntityRowState.Unchanged | EntityRowState.Detached):
                    return;
                case EntityRowState.Added:
                    {
                        if (State != EntityRowState.Detached)
                        {
                            _cache.ChangeState(this, EntityRowState.Added, EntityRowState.Detached);
                            _entryState = EntityRowState.Detached;
                            Reset();
                        }
                    }
                    return;
                case EntityRowState.Modified:
                    {
                        if (this.State != EntityRowState.Deleted)
                        {
                            _cache.ChangeState(this, EntityRowState.Added, EntityRowState.Detached);
                            _entryState = EntityRowState.Deleted;
                        }
                    }
                    return;
            }
        }

        public void SetModified()
        {
        }

        public void SetModified(string field)
        {
        }

        public void AcceptChanges()
        {
            ValidateState();
            switch (this.State)
            {
                case EntityRowState.Unchanged:
                case (EntityRowState.Unchanged | EntityRowState.Detached):
                    return;

                case EntityRowState.Added:
                    _cache.ChangeState(this, EntityRowState.Added, EntityRowState.Unchanged);
                    (_entity as ExtensibleEntity).AcceptChanges();
                    this.State = EntityRowState.Unchanged;
                    return;

                case EntityRowState.Deleted:
                    //TODO: accepts changes for relationships
                    _cache.ChangeState(this, EntityRowState.Deleted, EntityRowState.Detached);
                    this.Reset();
                    return;

                case EntityRowState.Modified:
                    _cache.ChangeState(this, EntityRowState.Modified, EntityRowState.Unchanged);
                    (_entity as ExtensibleEntity).AcceptChanges();
                    this.State = EntityRowState.Unchanged;
                    return;
            }

        }

        public void RejectChanges()
        {
            this.State = EntityRowState.Unchanged;
            (_entity as ExtensibleEntity).RejectChanges();
        }

        public IEnumerable<string> GetModifiedFields()
        {
            return (_entity as ExtensibleEntity).GetModifiedFields();
        }

        internal void AttachCacheToEntity()
        {
            ExtensibleEntity entity = _entity as ExtensibleEntity;
            if (entity != null)
            {
                entity.AttachCache(this, EntityCacheEntry.HandleEntityChanged);
            }
        }

        internal static void EntityValueChangedHandler(EntityCacheEntry cacheEntry, string fieldName, object value)
        {
            cacheEntry.ChangeEntityValue(fieldName, value);
        }

        internal void ChangeEntityValue(string memberName, object value)
        {
            EntityRowState oldState = _entryState;
            if (_entryState != EntityRowState.Added)
            {
                _entryState = EntityRowState.Modified;
            }
            if (oldState != _entryState)
            {
                _cache.ChangeState(this, oldState, _entryState);
            }
        }

        private static readonly EntityChangedHandler HandleEntityChanged;

        internal void Reset()
        {
            DetachCacheFromEntity();
            _cache = null;
            _entity = null;
            _entryState = EntityRowState.Detached;
        }

        internal void DetachCacheFromEntity()
        {
            ExtensibleEntity entity = _entity as ExtensibleEntity;
            if (entity != null)
            {
                entity.DetachCache();
            }
        }

        private void ValidateState()
        {
            if (_cache == null || _entryState == EntityRowState.Detached)
            {
                throw new InvalidOperationException("Invalid state of cache entry");
            }
        }
    }
}
