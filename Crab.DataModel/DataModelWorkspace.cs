using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Transactions;
using Crab.DataModel;
using Crab.DataModel.Common;

namespace Crab.DataModel
{
    [Serializable]
    public sealed class DataModelWorkspace
    {
        private Guid? _tenantId;
        private List<EntityMetadata> _entityMetadatas;
        private IDictionary<string, EntityMetadata> _namesEntityMetadatas;
        private IDictionary<Guid, EntityMetadata> _idsEntityMetadatas;

        public static DataModelWorkspace Current
        {
            get
            {
                if (DataModelContext.DataModelWorkspace != null)
                    return DataModelContext.DataModelWorkspace;
                return DataModelContext.DataModelWorkspace = new DataModelWorkspace(DataModelContext.TenantId);
            }
        }

        public DataModelWorkspace(Guid? tenantId)
        {
            this._tenantId = tenantId;
        }
        
        public EntityMetadata[] GetAllEntityMetadatas()
        {
            LoadEntityMetadatas();
            return _entityMetadatas.ToArray();
        }

        public EntityMetadata GetEntityMetadata(string name)
        {
            LoadEntityMetadatas();
            EntityMetadata dataClass = null;
            _namesEntityMetadatas.TryGetValue(name.ToLower(), out dataClass);
            return dataClass;
        }

        public EntityMetadata GetEntityMetadata(Guid id)
        {
            LoadEntityMetadatas();
            EntityMetadata dataClass = null;
            _idsEntityMetadatas.TryGetValue(id, out dataClass);
            return dataClass;
        }

        private void LoadEntityMetadatas()
        {
            if (_entityMetadatas != null)
                return;
            DataNode[] nodes = MetadataManager.GetAllNodes((int)DataNodeTypes.Entity);
            _entityMetadatas = new List<EntityMetadata>();
            _namesEntityMetadatas = new Dictionary<string, EntityMetadata>();
            _idsEntityMetadatas = new Dictionary<Guid, EntityMetadata>();
            foreach (DataNode node in nodes)
            {
                _entityMetadatas.Add((EntityMetadata)node);
                _namesEntityMetadatas.Add(node.Name.ToLower(), (EntityMetadata)node);
                _idsEntityMetadatas.Add(node.Id, (EntityMetadata)node);
            }
        }

        public void Refresh()
        {
            _entityMetadatas = null;
            _namesEntityMetadatas = null;
            _idsEntityMetadatas = null;
        }
    }
}
