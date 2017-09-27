using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data.Common;
using System.Data;

namespace Crab.DataModel.Data
{
    internal static class EntityMaterializer
    {
        private struct MaterializerEnumerable<T> : IEnumerable<T>, IEnumerable
        {
            private DbDataReader _reader;
            private ExtensibleObjectContextBase _context;
            private bool _skipDeletedItems;

            internal MaterializerEnumerable(DbDataReader reader, ExtensibleObjectContextBase context, bool skipDeletedItems)
            {
                this._reader = reader;
                this._context = context;
                this._skipDeletedItems = skipDeletedItems;
            }

            public IEnumerator<T> GetEnumerator()
            {
                return  new MaterializerEnumerator(_reader, _context, _skipDeletedItems);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.GetEnumerator();
            }

            private class MaterializerEnumerator: IEnumerator<T>, IDisposable, IEnumerator
            {
                private DbDataReader _reader;
                private ExtensibleObjectContextBase _context;
                private bool _skipDeletedItems;
                private object _current;

                internal MaterializerEnumerator(DbDataReader reader, ExtensibleObjectContextBase context, bool skipDeletedItems)
                {
                    this._reader = reader;
                    this._context = context;
                    this._skipDeletedItems = skipDeletedItems;
                    this._current = default(T);
                }

                public void Dispose()
                {
                    DbDataReader readerTmp = this._reader;
                    this._reader = null;
                    this._context = null;
                    this._current = null;
                    if (readerTmp != null)
                    {
                        readerTmp.Dispose();
                    }
                }

                public T Current
                {
                    get { return (T)_current; }
                }

                object IEnumerator.Current 
                { 
                    get{return this.Current;}
                }

                void IEnumerator.Reset()
                {
                }

                bool IEnumerator.MoveNext()
                {
                    IExtensibleEntity deletedEntity;
                    if (_context == null)
                    {
                        return false;
                    }
                    do
                    {
                        _current = null;
                        if (!this._reader.Read())
                        {
                            _context = null;
                            return false;
                        }
                        this._current = EntityMaterializer.CreateObject(typeof(T), _reader, _context, _skipDeletedItems, out deletedEntity);
                    }
                    while (deletedEntity != null && _skipDeletedItems);
                    return true;
                }
            }
        }

        internal static IEnumerable<T> CreateObjects<T>(DbDataReader reader, ExtensibleObjectContextBase context)
        {
            return CreateObjects<T>(reader, context, true);
        }

        internal static IEnumerable<T> CreateObjects<T>(DbDataReader reader, ExtensibleObjectContextBase context, bool skipDeletedItems)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
            if (context == null)
                throw new ArgumentNullException("context");
            if (reader.IsClosed)
            {
                throw new InvalidOperationException("The data reader is closed");
            }
            return new MaterializerEnumerable<T>(reader, context, skipDeletedItems);
        }

        internal static object CreateObject(Type entityType, DbDataReader record, ExtensibleObjectContextBase context, bool skipDeletedItems, out IExtensibleEntity deletedEntity)
        {
            return CreateObject(entityType, record, context, false, skipDeletedItems, out deletedEntity);
        }

        private static IExtensibleEntity CreateObject(Type entityType, DbDataReader record, ExtensibleObjectContextBase context, bool updatableFromCache, bool skipDeletedItems, out IExtensibleEntity deletedEntity)
        {
            IExtensibleEntity returnEntity = null;
            deletedEntity = null;
            EntityKey key = CreateEntityKey(entityType, record);
            EntityCacheEntry entry = context.Cache.FindCacheEntry(key);
            if (entry != null)
            {
                returnEntity = entry.Entity;
                switch (entry.State)
                {
                    case EntityRowState.Added:
                        throw new InvalidOperationException("Added entity already exists in the cache");
                    case EntityRowState.Deleted:
                        deletedEntity = entry.Entity;
                        break;
                }
                if (deletedEntity != null && skipDeletedItems)
                    return null;
                else
                    return returnEntity; //return the entity from the cache
            }

            returnEntity = CreateEntity(entityType, record, context, true); //create entity from db record
            context.Cache.AddEntry(returnEntity, false);
            return returnEntity;
        }

        private static EntityKey CreateEntityKey(Type entityType, IDataRecord dbReader)
        {
            EntityMetadata entityMetadata = ExtensibleEntity.GetEntityMetadata(entityType);
            FieldMetadata[] keyFields = entityMetadata.KeyFields;
            KeyValuePair<string, object>[] keyValuePairs = new KeyValuePair<string, object>[keyFields.Length];
            for (int i = 0; i < keyFields.Length; i++)
            {
                object value = dbReader[keyFields[i].Name];
                if (value is DBNull)
                    value = null;
                keyValuePairs[i] = new KeyValuePair<string, object>(keyFields[i].Name, value);
            }
            return new EntityKey(entityMetadata, keyValuePairs);
        }

        private static IExtensibleEntity CreateEntity(Type entityType, IDataRecord dbReader, ExtensibleObjectContextBase context, bool withExtensionValues)
        {
            ExtensibleEntity entity = Activator.CreateInstance(entityType) as ExtensibleEntity;
            EntityMetadata entityMetaData = ExtensibleEntity.GetEntityMetadata(entityType);
            FieldMetadata[] fields = withExtensionValues?entityMetaData.Fields:entityMetaData.PreDefinedFields;
            for (int i = 0; i < fields.Length; i++)
            {
                object value = dbReader[fields[i].Name]; //note: use .Name rather than .ColumnName
                if (value is DBNull)
                    value = null;
                entity.SetValueWithFlag(fields[i].Name, value, true); //don't set the dirty flag
            }
            return entity;
        }
    }
}
