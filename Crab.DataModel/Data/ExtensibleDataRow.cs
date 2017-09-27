using System;
using System.Collections.Generic;
using System.Text;
using Crab.DataModel;
using Crab.DataModel.Common;
using Crab.DataModel.Utility;

namespace Crab.DataModel.Data
{
    /// <summary>
    /// Represents a row of data in the database table 
    /// which supports extension fields
    /// </summary>
    [Serializable]
    public class ExtensibleDataRow : IExtDataRecord
    {
        private Dictionary<string, object> _fieldValues = new Dictionary<string, object>();//shadow values
        private Dictionary<string, object> _dirtyValues = new Dictionary<string, object>();
        private EntityMetadata _metadata;

        #region Constructors
        public ExtensibleDataRow()
        {
        }

        public ExtensibleDataRow(EntityMetadata metadata)
        {
            //if (fieldsMetaData == null)
            //  throw new ArgumentNullException("fieldsMetaData");
            this._metadata = metadata;
        }

        public ExtensibleDataRow(EntityMetadata metadata, IEnumerable<KeyValuePair<string, object>> fieldsValues)
        {
            //if (fieldsMetaData == null)
            //    throw new ArgumentNullException("fieldsMetaData");
            this._metadata = metadata;
            foreach (KeyValuePair<string, object> keyValue in fieldsValues)
                SetValueWithFlag(keyValue.Key, keyValue.Value, true);
        }
        #endregion

        /// <summary>
        /// Gets or sets the metadata object of the datarow
        /// </summary>
        public EntityMetadata Metadata
        {
            get { return _metadata; }
            set { _metadata = value; }
        }

        /// <summary>
        /// Gets the value by the field name. 
        /// 
        /// </summary>
        /// <param name="fieldName">
        /// The name of the field of entity.
        /// The field name must be the same with the metadata field name.
        /// The field name is case-insensitive
        /// </param>
        /// <returns></returns>
        public virtual object GetValue(string fieldName)
        {
            object value = null;
            TryGetValue(fieldName, out value);
            return value;
        }

        public virtual void SetValue(string fieldName, object value)
        {
            SetValueWithFlag(fieldName, value, false);
        }

        /// <summary>
        /// This method is used by SetValue or by outside
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        /// <param name="clean"></param>
        internal void SetValueWithFlag(string fieldName, object value, bool clean)
        {
            if (!_metadata.ContainsField(fieldName))
                throw new ArgumentException("The field name does not exist", "fieldName");
            string lowerName = fieldName.ToLower();
            Type destType = DataTypeConvert.ToSysType(_metadata[fieldName].DataType, _metadata[fieldName].Nullable);
            value = TypeConvert.ChangeType(value, destType);
            object oldValue = null;
            _fieldValues.TryGetValue(lowerName, out oldValue);
            bool changed = oldValue != value;
            if (changed)
            {
                if(clean)
                    _fieldValues[lowerName] = value;
                else
                {
                    _dirtyValues[lowerName] = value;
                    ReportFieldChanged(fieldName, value);
                }
            }
        }

        public bool TryGetValue(string fieldName, out object value)
        {
            if (fieldName == null)
                throw new ArgumentNullException("fieldName");
            string lowerName = fieldName.ToLower();
            if (_dirtyValues.TryGetValue(lowerName, out value))
                return true;
            else
                return _fieldValues.TryGetValue(lowerName, out value);
        }

        public T GetValue<T>(string fieldName)
        {
            if (!_metadata.ContainsField(fieldName))
                throw new ArgumentException("The field name does not exist", "fieldName");
            //CheckStrongType(fieldName, typeof(T));
            return (T)GetValue(fieldName);
        }

        public void SetValue<T>(string fieldName, T value)
        {
            if (!_metadata.ContainsField(fieldName))
                throw new ArgumentException("The field name does not exist", "fieldName");
            //CheckStrongType(fieldName, typeof(T));
            SetValue(fieldName, (object)value);
        }


        public ICollection<string> GetModifiedFields()
        {
            return _dirtyValues.Keys;
        }

        public bool IsDirty()
        {
            return _dirtyValues.Count > 0;
        }

        public virtual void AcceptChanges()
        {
            foreach (KeyValuePair<string, object> keyValue in _dirtyValues)
                _fieldValues[keyValue.Key] = keyValue.Value;
            _dirtyValues.Clear();
        }

        public virtual void RejectChanges()
        {
            _dirtyValues.Clear();
        }

        private void CheckStrongType(string fieldName, Type type)
        {
            FieldMetadata fieldMetaData = _metadata[fieldName];
            Type compareType = DataTypeConvert.ToSysType(fieldMetaData.DataType, fieldMetaData.Nullable);
            if (type != compareType)
                throw new ArgumentException("Type not match", "type");
        }

        protected virtual void ReportFieldChanged(string fieldName, object value)
        {
        }
    }
}
