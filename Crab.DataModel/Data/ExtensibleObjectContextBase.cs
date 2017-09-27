using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Crab.DataModel.Common;

namespace Crab.DataModel.Data
{
    public class ExtensibleObjectContextBase: IDisposable
    {
        private DbConnection _connection;
        private DataModelWorkspace _workspace;
        private ExtensibleObjectCache _cache;
        private bool _ownConnection;
        private bool _connectionOpenedByContext;

        #region constructors
        internal ExtensibleObjectContextBase()
        {
        }

        public ExtensibleObjectContextBase(DbConnection connection, DataModelWorkspace workspace)
        {
            this._connection = connection;
            this._workspace = workspace;
            //determine connection ownership
            if (this._connection == null)
            {
                this._ownConnection = false;
            }
            else
            {
                this._ownConnection = this._connection.State == ConnectionState.Closed;
                if (this._ownConnection)
                {
                    this._connectionOpenedByContext = false;
                }
            }

        }
        #endregion

        public ExtensibleObjectCache Cache
        {
            get
            {
                if (this._cache == null)
                {
                    this._cache = new ExtensibleObjectCache(this._workspace);
                }
                return this._cache;

            }
        }

        public DbConnection Connection 
        {
            get{ return _connection;}
            set{ _connection = value;}
        }

        public DataModelWorkspace DataModelWorkspace
        {
            get
            {
                return null;
            }
        }

        public void AddObject(IExtensibleEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            AssertNotDisposed();
            this.Cache.Add(entity);
            ExtensibleEntity entityObject = entity as ExtensibleEntity;
            if (entityObject != null)
            {
                //TODO: Add context to relationships
            }
        }

        public void DeleteObject(IExtensibleEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            this.AssertNotDisposed();
            if (entity.Key == null)
            {
                throw new ArgumentNullException("Can not delete entity without key","entity");
            }
            EntityCacheEntry entry = this.Cache.FindCacheEntry(entity.Key);
            if ((entry == null) || !object.ReferenceEquals(entry.Entity, entity))
            {
                throw new InvalidOperationException("Can not delete entity not in cache");
            }
            entry.Delete();
        }


        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.CloseOwnedConnection();
            }
            this._connection = null;
            this._workspace = null;
            this._cache = null;
        }

        internal void AssertNotDisposed()
        {
            if (this._workspace == null)
            {
                throw new ObjectDisposedException(base.GetType().Name);
            }
        }

        internal void OpenConnection()
        {
            this.AssertNotDisposed();
            if ((this._connection != null) && (((this._connection.State == ConnectionState.Closed) && this._ownConnection) && !this._connectionOpenedByContext))
            {
                this._connection.Open();
                this._connectionOpenedByContext = true;
            }
        }

        private void CloseOwnedConnection()
        {
            if (this._ownConnection && (this._connection != null))
            {
                this._connection.Close();
            }
        }

        internal EntityMetadata GetEntityMetadata(Type entityType)
        {
            EntityClassAttribute[] classAttributes =
               (EntityClassAttribute[])entityType.GetCustomAttributes(typeof(EntityClassAttribute), false);
            if (classAttributes == null || classAttributes.Length == 0)
                return null;
            return DataModelWorkspace.Current.GetEntityMetadata(classAttributes[0].ClassName);
        }
    }
}
