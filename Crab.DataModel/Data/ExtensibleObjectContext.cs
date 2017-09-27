using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Diagnostics;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Crab.DataModel.Exceptions;
using Crab.DataModel;

namespace Crab.DataModel.Data
{
    public class ExtensibleObjectContext: ExtensibleObjectContextBase
    {
        private IEntityUpdateAdapter _adapter;

        public ExtensibleObjectContext(string connectionName)
            :this(CreateConnection(connectionName), DataModelWorkspace.Current)
        {
        }

        public ExtensibleObjectContext(DbConnection connection, DataModelWorkspace workspace)
            : base(connection, workspace)
        {
        }

        public EntityKey CreateKey(Type entityType, IEnumerable<KeyValuePair<string, object>> keyValues)
        {
            if (entityType == null)
                throw new ArgumentNullException("entityType");
            if (keyValues == null)
                throw new ArgumentNullException("keyValues");
            base.AssertNotDisposed();
            return new EntityKey(base.GetEntityMetadata(entityType), keyValues);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public T GetObjectByKey<T>(EntityKey key)
        {
            T entity;
            if (!this.TryGetObjectByKey(key, out entity))
            {
                throw new ObjectNotFoundException("Object not found by the key");
            }
            return entity;
        }

        public bool TryGetObjectByKey<T>(EntityKey key, out T value)
        {
            Type baseType = typeof(ExtensibleEntity);
            if (!baseType.IsAssignableFrom(typeof(T)))
                throw new ArgumentException("Must use a type inherited from ExtensibleEntity", "T");
            if (key == null)
                throw new ArgumentNullException("key");
            base.AssertNotDisposed();
            value = default(T);
            //find from cache
            EntityCacheEntry entry = base.Cache.FindCacheEntry(key);
            if (entry != null)
            {
                value = (T)((entry.State != EntityRowState.Deleted) ? entry.Entity : null);
                return (value != null);
            }
            this.CheckConnection();
            KeyValuePair<string, object>[] keyValues = new KeyValuePair<string, object>[key.KeyValues.Count];
            key.KeyValues.CopyTo(keyValues, 0);
            EntityQuery<T> query = new EntityQuery<T>(this, keyValues);
            base.OpenConnection();
            using (IEnumerator<T> enumerator = query.GetEnumerator())
            {
                if (enumerator.MoveNext())
                    value = enumerator.Current;
            }
            return value!=null;
        }

        /// <summary>
        /// Save all changes to database
        /// </summary>
        public void SaveAllChanges()
        {
            this.CheckConnection();
            if (this._adapter == null)
            {
                //TODO: the adapter should be configurable
                this._adapter = new EntityUpdateAdapter(); 
            }
            this.SaveChanges(this._adapter);
        }

        /// <summary>
        /// Save all changes to database by a adapter
        /// </summary>
        /// <param name="adapter"></param>
        protected virtual void SaveChanges(IEntityUpdateAdapter adapter)
        {
            if (adapter == null)
                throw new ArgumentNullException("adapter");
            this.CheckConnection();
            base.OpenConnection();
            adapter.Connection = base.Connection;
            adapter.Update(base.Cache);
        }

        public EntityQuery<T> GetQuery<T>(string queryClause)
        {
            base.OpenConnection();
            return new EntityQuery<T>(this, queryClause);
        }

        public EntityQuery<T> GetQuery<T>(params KeyValuePair<string, object>[] conditions)
        {
            base.OpenConnection();
            return new EntityQuery<T>(this, conditions);
        }

        public void Refresh()
        {
        }

        private void CheckConnection()
        {
            base.AssertNotDisposed();
            if (base.Connection == null)
            {
                throw new InvalidOperationException("Connection can not be null");
            }
        }

        private static DbConnection CreateConnection(string connectionName)
        {
            return DatabaseFactory.CreateDatabase(connectionName).CreateConnection();
        }
    }
}
