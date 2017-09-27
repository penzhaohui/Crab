using System;
using System.ServiceModel;
using Crab.DataModel.Common;

namespace Crab.Runtime.Contract
{
    [ServiceContract()]
    public interface IMetadataService
    {
        [OperationContract]
        EntityDef[] GetEntityDefs();

        [OperationContract]
        EntityDef GetEntityDefById(Guid id);

        [OperationContract]
        EntityDef GetEnityDefByName(string name);

        [OperationContract]
        void UpdateEntityDef(Guid entityDefId, string caption);

        [OperationContract]
        EntityFieldDef[] GetFieldDefs(Guid entityDefId);

        [OperationContract]
        EntityFieldDef GetFieldDefById(Guid fieldId);

        [OperationContract]
        EntityFieldDef AddEntityFieldDef(Guid entityDefId, string fieldName, string caption, DataTypes dataType, int length);

        [OperationContract]
        void DeleteEntityFieldDef(Guid fieldDefId);

        [OperationContract]
        void UpdateEntityFieldDef(Guid fieldDefId, string caption);
    }
}
