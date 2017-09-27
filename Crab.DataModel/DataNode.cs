using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.ComponentModel;
using Crab.DataModel;
using Crab.DataModel.Common;
using Crab.DataModel.Utility;

namespace Crab.DataModel
{
    [Serializable]
    [Table("DataNodes")]
    public class DataNode
    {
        private enum PropertyTypes
        {
            Caption = 0,
        }
        //private string _providerName;
        private Guid? _tenantId;
        private Guid _id;
        private string _name;
        private int _nodeType;
        private Guid? _parentId;
        private DataNode _parentNode;
        private DataNodeCollection _childNodes;
        private DataPropertyCollection _properties;

        //properties
        /*public string ProviderName
        {
            get { return _providerName; }
        }*/

        [Field("TenantId")]
        public Guid? TenantId
        {
            get { return _tenantId; }
            internal set { _tenantId = value; }
        }

        [Field(FieldName = "Id", PrimaryKey = true)]
        public Guid Id
        {
            get { return _id; }
            internal set { _id = value; }
        }

        [Field("Name")]
        public string Name
        {
            get { return _name; }
            set { _name = value;}
        }

        [Browsable(false), Field("NodeType")]
        public int NodeType
        {
            get { return _nodeType; }
            internal set { _nodeType = value; }
        }

        [Browsable(false), Field("ParentId")]
        public Guid? ParentId
        {
            get { return _parentId; }
            internal set { _parentId = value; }
        }

        [Browsable(false)]
        public DataNode ParentNode
        {
            get 
            { 
                return _parentNode; 
            }
            set 
            { 
                _parentNode = value;
                if (_parentNode != null)
                    _parentId = _parentNode.Id;
                else
                    _parentId = null;
            }
        }

        [Browsable(false)]
        public DataNodeCollection ChildNodes
        {
            get
            {
                LoadChildNodes();
                return _childNodes; 
            }
        }

        [Browsable(false)]
        public DataPropertyCollection Properties
        {
            get 
            {
                LoadProperties();
                return _properties; 
            }
        }

        public bool IsExtension
        {
            get { return _tenantId != null; }
        }

        /*[Browsable(false)]
        public DirtyFlags DirtyFlag
        {
            get { return _dirtyFlag; }
        }*/

        public DataNode()
        {
            //_dirtyFlag = DirtyFlags.Clean;
        }

        public void Initialize(Guid id, string name, int nodeType, Guid? parentId)
        {
            _tenantId = DataModelContext.TenantId;
            _id = id;
            _name = name;
            _nodeType = nodeType;
            _parentId = parentId;
        }

        internal void LoadChildNodes()
        {
            if (_childNodes != null)
                return;
            lock (this)
            {
                if (_childNodes != null)
                    return;
                try
                {
                    _childNodes = new DataNodeCollection(this, MetadataManager.GetChildNodes(_id));
                }
                catch
                {
                    _childNodes = null;
                    throw;
                }
            }
        }

        protected void LoadProperties()
        {
            if (_properties != null)
                return;
            lock (this)
            {
                if (_properties != null)
                    return;
                try
                {
                    _properties = new DataPropertyCollection(this);
                }
                catch
                {
                    _properties = null;
                    throw;
                }
            }
        }
        
        public string GetPropertyValue(int propType)
        {
            return Properties[propType];
        }

        public void SetPropertyValue(int propType, string value)
        {
            Properties[propType] = value;
        }

        public T GetPropertyValue<T>(int propType)
        {
            string textValue = GetPropertyValue(propType);
            return TypeConvert.ChangeType<T>(textValue);
        }

        public void SetPropertyValue<T>(int propType, T value)
        {
            string textValue = (string)TypeConvert.ChangeType<string>(value);
            SetPropertyValue(propType, textValue);
        }

        public void Delete()
        {
            //_dirtyFlag = DirtyFlags.Delete;
        }

        public virtual string Caption
        {
            get { return GetPropertyValue<string>((int)PropertyTypes.Caption); }
            set { SetPropertyValue<string>((int)PropertyTypes.Caption, value); }
        }
    }
}