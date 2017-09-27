using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;

namespace Crab.DataModel.Data
{
    /// <summary>
    /// The adapter class which is used to added/modified/deleted from database
    /// according to the object cache
    /// </summary>
    public class EntityUpdateAdapter: IEntityUpdateAdapter
    {
        private DbConnection _connection;

        private static bool IsCacheDirty(IExtensibleEntityCache entityCache)
        {
            bool flag = false;
            using (IEnumerator<EntityCacheEntry> enumerator = entityCache.GetCacheEntries(EntityRowState.Modified | EntityRowState.Deleted | EntityRowState.Added).GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    EntityCacheEntry entry = enumerator.Current;
                    return true;
                }
            }
            return flag;
        }

        /// <summary>
        /// Gets or sets the database connection object
        /// </summary>
        public DbConnection Connection 
        {
            get { return _connection; }
            set { _connection = value; }
        }

        /// <summary>
        /// Update the entity cache to database
        /// </summary>
        /// <param name="entityCache">The cache object</param>
        /// <returns>Rows affected</returns>
        public int Update(IExtensibleEntityCache entityCache)
        {
            if(entityCache == null)
                throw new ArgumentNullException("entityCache");
            //ADP.CheckArgumentNull<IEntityCache>(entityCache, "entityCache");
            if (!EntityUpdateAdapter.IsCacheDirty(entityCache))
            {
                return 0;
            }
            if (this._connection == null)
            {
                throw new InvalidOperationException("No connection for adapter");
            }
            bool flag = ConnectionState.Open != this._connection.State;
            if (flag)
            {
                this._connection.Open();
            }
            using (DbTransaction transaction = this._connection.BeginTransaction())
            {
                try
                {
                    int affectedRows = EntityCacheUpdateTranslator.Update(entityCache, this, transaction);
                    transaction.Commit();
                    return affectedRows;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    if (flag && (this._connection.State != ConnectionState.Closed))
                    {
                        this._connection.Close();
                    }
                }
            }
        }
    }
}
