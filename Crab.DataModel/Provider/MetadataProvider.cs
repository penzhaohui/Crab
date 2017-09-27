using System;
using System.Configuration;
using System.Configuration.Provider;
using Crab.DataModel;
using Crab.DataModel.Configuration;

namespace Crab.DataModel.Provider
{
    /// <summary>
    ///  Defines the contract that implements to provide datamodel services
    ///     using custom datamodel providers
    /// </summary>
    public abstract class MetadataProvider: ProviderBase
    {
        //methods
        protected MetadataProvider() {}

        public abstract string ConnectionString{get;}

        public abstract DataNode GetDataNode(Guid id);

        public abstract DataNode[] GetAllNodes(int nodeType);

        public abstract DataNode[] GetChildNodes(Guid parentId);

        public abstract DataProperty[] GetProperties(Guid nodeId);

        public abstract DataNode CreateDataNode(DataNode node);
             
        public abstract void UpdateDataNode(DataNode node);

        public abstract void DeleteDataNode(DataNode node);

        public virtual DataNode NewDataNode(int nodeType)
        {
            KeyTypeMappingSettings mapping = MetadataManager.TypeMappings[nodeType.ToString()];
            if (mapping == null)
            {
                return new DataNode();
            }
            else
            {
                string typeName = (mapping.Type == null) ? null : mapping.Type.Trim();
                if (string.IsNullOrEmpty(typeName))
                {
                    throw new ArgumentException("No type name found.");
                }
                Type newType = Type.GetType(typeName);
                if (!typeof(DataNode).IsAssignableFrom(newType))
                {
                    throw new ArgumentException(string.Format("The datanode object must implement {0}", typeof(DataNode).ToString()));
                }
                return (DataNode)Activator.CreateInstance(Type.GetType(mapping.Type));
            }   
        }

    }
}
