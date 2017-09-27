using System;
using System.Collections.Generic;
using System.Text;
using Crab.DataModel.Utility;

namespace Crab.DataModel
{
    /// <summary>
    /// The class for node property
    /// </summary>
    [Serializable]
    [Table("DataProperties")]
    public class DataProperty
    {
        #region fields
        private Guid? _tenantId;
        private Guid _id;
        private Guid _parentId;
        private int _propType;
        private string _value;
        #endregion

        [Field("TenantId")]
        public Guid? TenantId
        {
            get{ return _tenantId;}
            set { _tenantId = value; }
        }

        [Field(FieldName="Id", PrimaryKey=true)]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [Field("PropertyType")]
        public int PropertyType
        {
            get { return _propType; }
            set { _propType = value; }
        }

        [Field("ParentId")]
        public Guid ParentId
        {
            get { return _parentId; }
            set { _parentId = value; }
        }

        [Field("Value")]
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public DataProperty()
        {
        }

        public DataProperty(Guid? tenantId, Guid id, Guid parentId, int propType, string value)
        {
            this._tenantId = tenantId;
            this._id = id;
            this._parentId = parentId;
            this._propType = propType;
            this._value = value;
        }
    }
}
