using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Crab.DataModel;

namespace Crab.DataModel
{
    [Serializable]
    public class DataPropertyCollection : IEnumerable, IStatibleCollection<DataProperty>
    {
        private IDictionary<int, DataProperty> _values;
        private List<DataProperty> _inserted = new List<DataProperty>();
        private List<DataProperty> _updated = new List<DataProperty>();
        private DataNode _owner;

        public IEnumerator GetEnumerator()
        {
            LoadProperties();
            return _values.Values.GetEnumerator();
        }

        #region constructors
        public DataPropertyCollection(DataNode owner, DataProperty[] properties)
        {
            this._owner = owner;
            _values = new Dictionary<int, DataProperty>();
            if (properties == null)
                return;
            foreach (DataProperty property in properties)
            {
                _values.Add(property.PropertyType, property);
            }
        }

        public DataPropertyCollection(DataNode owner, DataPropertyCollection properties)
        {
            this._owner = owner;
            _values = new Dictionary<int, DataProperty>();
            if (properties == null)
                return;
            foreach (DataProperty property in properties)
            {
                _values.Add(property.PropertyType, property);
            }
        }

        public DataPropertyCollection(DataNode owner)
        {
            this._owner = owner;
        }
        #endregion

        #region properties
        public DataNode Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }
        #endregion 

        private void LoadProperties()
        {
            if (_values != null)
                return;
            lock (this)
            {
                if (_values != null)  //double check
                    return;
                try
                {
                    _values = new Dictionary<int, DataProperty>();
                    DataProperty[] properties = MetadataManager.Provider.GetProperties(_owner.Id);
                    if (properties != null)
                    {
                        foreach (DataProperty property in properties)
                        {
                            _values.Add(property.PropertyType, property);
                        }
                    }
                    
                }
                catch
                {
                    _values = null;
                    throw;
                }
            }
        }

        public string this[int propType]
        {
            get
            {
                LoadProperties();
                DataProperty property = null;
                _values.TryGetValue(propType, out property);
                return property!=null?property.Value:null;
            }
            set
            {
                if (this[propType] == value) //nothing changed
                    return;
                lock (this)
                {
                    bool contain = _values.Keys.Contains(propType);
                    if (contain && _values[propType].TenantId == DataModelContext.TenantId) //already have a property
                    {
                        _values[propType].Value = value;
                        if (!_inserted.Contains(_values[propType]) && !_updated.Contains(_values[propType]))
                            _updated.Add(_values[propType]);
                    }
                    else  //not contain the value or the tenant id is not null. new a property
                    {
                        if (contain)
                            _values.Remove(propType);
                        DataProperty property = NewProperty(propType, value);
                        _values.Add(propType, property);
                        _inserted.Add(property);
                    }
                }
            }
        }

        protected DataProperty NewProperty(int propType, string value)
        {
            DataProperty property = new DataProperty();
            property.Id = Guid.NewGuid();
            property.TenantId = DataModelContext.TenantId;
            property.ParentId = Owner.Id;
            property.PropertyType = propType;
            property.Value = value;
            return property;
        }

        public void GetAllChanges(out DataProperty[] inserted, out DataProperty[] updated, out DataProperty[] deleted)
        {
            inserted = updated = deleted = new DataProperty[0];
            inserted = _inserted.ToArray();
            updated = _updated.ToArray();
        }

        public void RefreshCollection()
        {
            _inserted.Clear();
            _updated.Clear();
            _values = null;
        }
    }
}
