using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using Crab.DataModel.Common;

namespace Crab.Runtime.Contract
{
    [DataContract, Serializable]
    public class EntityFieldDef
    {
        #region private members
        private Guid _id;
        private string _name;
        private string _caption;
        private DataTypes _dataType;
        private int _length;
        private bool _nullable;
        private bool _isShared;
        #endregion

        [DataMember]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [DataMember]
        public string Caption
        {
            get { return _caption; }
            set { _caption = value; }
        }

        [DataMember]
        public DataTypes DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        [DataMember]
        public int Length
        {
            get { return _length; }
            set { _length = value; }
        }

        [DataMember]
        public bool Nullable
        {
            get { return _nullable; }
            set { _nullable = value; }
        }

        [DataMember]
        public bool IsShared
        {
            get { return _isShared; }
            set { _isShared = value; }
        }
    }
}
