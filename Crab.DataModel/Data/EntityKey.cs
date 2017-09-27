using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Crab.DataModel;
using System.Diagnostics;

namespace Crab.DataModel.Data
{
    /// <summary>
    /// Represents the key of the entity in the object context
    /// </summary>
    public class EntityKey
    {
        private readonly KeyValuePair<string, object>[] _keyValues;
        private readonly EntityMetadata _entityMetaData;
        private int _hashCode;

        public ReadOnlyCollection<KeyValuePair<string, object>> KeyValues
        {
            get
            {
                return new ReadOnlyCollection<KeyValuePair<string, object>>(this._keyValues);
            }
        }

        public EntityMetadata EntityMetadata
        {
            get
            {
                return _entityMetaData;
            }
        }

        public EntityKey(EntityMetadata entityMetaData, IEnumerable<KeyValuePair<string, object>> keyValues)
        {
            this._entityMetaData = entityMetaData;
            List<KeyValuePair<string, object>> keyValueList = new List<KeyValuePair<string, object>>(keyValues);
            if(keyValueList.Count == 0)
                throw new ArgumentException("The key value pairs can not be empty", "keyValues");
            this._keyValues = keyValueList.ToArray();
        }

        public override int GetHashCode()
        {
            if (this._hashCode == 0)
            {
                int code = _entityMetaData.GetHashCode() ^ _entityMetaData.Name.GetHashCode();
                foreach (KeyValuePair<string, object> pair in this._keyValues)
                {
                    code ^= pair.Key.GetHashCode() + pair.Value.GetHashCode();
                }
                this._hashCode = code;
            }
            return this._hashCode;
        }

        internal object FindValueByKey(string key)
        {
            foreach (KeyValuePair<string, object> pair in this._keyValues)
            {
                if (pair.Key == key)
                {
                    return pair.Value;
                }
            }
            return null;
        }

        public override bool Equals(object obj)
        {
            return EntityKey.InternalEquals(this, obj as EntityKey);
        }

        private static bool InternalEquals(EntityKey key1, EntityKey key2)
        {
            if (!object.ReferenceEquals(key1, key2))
            {
                if (object.ReferenceEquals(key1, null) || object.ReferenceEquals(key2, null))
                {
                    return false;
                }

                if (key1.GetHashCode() != key2.GetHashCode())
                {
                    return false;
                }

                if (!StringComparer.Ordinal.Equals(key1.EntityMetadata.Name, key2.EntityMetadata.Name))
                {
                    return false;
                }

                if (key1._keyValues.Length != key2._keyValues.Length)
                {
                    return false;
                }

                for (int i = 0; i < key1._keyValues.Length; i++)
                {
                    bool flag = false;
                    for (int j = 0; j < key2._keyValues.Length; j++)
                    {
                        if (StringComparer.Ordinal.Equals(key1._keyValues[i].Key, key2._keyValues[j].Key))
                        {
                            if (object.Equals(key1._keyValues[i].Value, key2._keyValues[j].Value))
                            {
                                flag = true;
                                break;
                            }
                            return false;
                        }
                    }
                    if (!flag)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
