using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Diagnostics;
using Crab.DataModel.Exceptions;

namespace Crab.DataModel.Data
{
    internal class EntityCacheUpdateTranslator
    {
        internal static int Update(IExtensibleEntityCache cache, IEntityUpdateAdapter adapter, DbTransaction storeTransaction)
        {
            int objectsAffected = 0;
            try
            {
                //TODO: To support other type of database
                IUpdateScriptGenerator scriptGenenrator = new SqlUpdateScriptGenerator();
                IEnumerable<EntityCacheEntry> dirtyEntries = cache.GetCacheEntries(EntityRowState.Added | EntityRowState.Modified | EntityRowState.Deleted);
                List<string > scripts= new List<string>();
                foreach (EntityCacheEntry entry in dirtyEntries)
                    scripts.AddRange(scriptGenenrator.GenerateUpdateScripts(entry));
                foreach (string script in scripts)
                {
                    using (DbCommand cmd = storeTransaction.Connection.CreateCommand())
                    {
                        Trace.WriteLine(script);
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.CommandText = script;
                        cmd.Transaction = storeTransaction;
                        cmd.ExecuteNonQuery();
                    }
                }
                objectsAffected += AcceptChanges(dirtyEntries);
            }
            catch (DbException exception)
            {
                throw new UpdateException("Exception when update the entity cache", exception);
            }
            return objectsAffected;
        }

        private static int AcceptChanges(IEnumerable<EntityCacheEntry> entries)
        {
            int num = 0;
            foreach(EntityCacheEntry entry in entries)
            {
                if (EntityRowState.Unchanged != entry.State)
                {
                    entry.AcceptChanges();
                    num++;
                }
            }
            return num;
        }
    }
}
