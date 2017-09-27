using System;
using System.Collections.Generic;
using System.Text;
using Crab.DataModel.Common;
using Crab.DataModel;
using Crab.Runtime.Contract;
using Crab.Runtime.Contract.Exceptions;

namespace Crab.Runtime.Logic
{
    public class MetadataLogic
    {
        static public EntityDef[] GetEntityDefs()
        {
            EntityMetadata[] metadatas = DataModelWorkspace.Current.GetAllEntityMetadatas();
            List<EntityDef> entityDefs = new List<EntityDef>(metadatas.Length);
            foreach (EntityMetadata metadata in metadatas)
                entityDefs.Add(EntityMetadataToEntityDef(metadata));
            return entityDefs.ToArray();
        }

        static public EntityDef GetEntityDefById(Guid id)
        {
            EntityMetadata metadata = DataModelWorkspace.Current.GetEntityMetadata(id);
            return EntityMetadataToEntityDef(metadata);
        }

        static public EntityDef GetEnityDefByName(string name)
        {
            EntityMetadata metadata = DataModelWorkspace.Current.GetEntityMetadata(name);
            return EntityMetadataToEntityDef(metadata);
        }

        static public void UpdateEntityDef(Guid entityDefId, string caption)
        {
            EntityMetadata metadata = DataModelWorkspace.Current.GetEntityMetadata(entityDefId);
            metadata.Caption = caption;
            MetadataManager.UpdateDataNode(metadata);
        }

        static public EntityFieldDef[] GetFieldDefs(Guid entityDefId)
        {
            EntityMetadata entityMetadata = DataModelWorkspace.Current.GetEntityMetadata(entityDefId);
            if (entityMetadata == null)
                throw new MetadataException("Entity definition does not exist!");
            FieldMetadata[] metadatas = entityMetadata.Fields;
            List<EntityFieldDef> fieldDefs = new List<EntityFieldDef>(metadatas.Length);
            foreach(FieldMetadata metadata in metadatas)
                fieldDefs.Add(FieldMetadataToFieldDef(metadata));
            return fieldDefs.ToArray();
        }

        static public EntityFieldDef GetFieldDefById(Guid fieldId)
        {
            FieldMetadata metadata = MetadataManager.GetDataNode(fieldId) as FieldMetadata;
            return FieldMetadataToFieldDef(metadata);
        }

        static public EntityFieldDef AddEntityFieldDef(Guid entityDefId, string fieldName, string caption, DataTypes dataType, int length)
        {
            FieldMetadata metadata = MetadataManager.NewDataNode((int)DataNodeTypes.Field) as FieldMetadata;
            metadata.Initialize(
                Guid.NewGuid(),
                fieldName,
                (int)DataNodeTypes.Field,
                entityDefId);

            metadata.Caption = caption;
            metadata.Nullable = true;
            if (dataType == DataTypes.String)
            {
                if (length < 0)
                    metadata.Length = 0;
                if (length > 256)
                    metadata.Length = 256;
            }
            else
                metadata.Length = 0;
            try
            {
                MetadataManager.CreateDataNode(metadata);
            }
            catch(Exception ex)
            {
                throw new MetadataException(ex.Message, ex);
            }
            return FieldMetadataToFieldDef(metadata);
        }

        static public void DeleteEntityFieldDef(Guid fieldDefId)
        {
            FieldMetadata metadata = MetadataManager.GetDataNode(fieldDefId) as FieldMetadata;
            if (metadata == null)
                return;
            if (metadata.TenantId == DataModelContext.TenantId)
                MetadataManager.DeleteDataNode(metadata);
        }

        static public void UpdateEntityFieldDef(Guid fieldDefId, string caption)
        {
            FieldMetadata metadata = MetadataManager.GetDataNode(fieldDefId) as FieldMetadata;
            if (metadata == null)
                return;
            metadata.Caption = caption;
            MetadataManager.UpdateDataNode(metadata);
        }

        #region helper methods
        static private EntityDef EntityMetadataToEntityDef(EntityMetadata metadata)
        {
            if (metadata == null)
                return null;
            EntityDef entityDef = new EntityDef();
            entityDef.Id = metadata.Id;
            entityDef.Name = metadata.Name;
            entityDef.Caption = metadata.Caption;
            return entityDef;
        }

        static private EntityFieldDef FieldMetadataToFieldDef(FieldMetadata metadata)
        {
            if (metadata == null)
                return null;
            EntityFieldDef fieldDef = new EntityFieldDef();
            fieldDef.Id = metadata.Id;
            fieldDef.Name = metadata.Name;
            fieldDef.Caption = metadata.Caption;
            fieldDef.DataType = metadata.DataType;
            fieldDef.Length = metadata.Length;
            fieldDef.Nullable = metadata.Nullable;
            fieldDef.IsShared = metadata.TenantId == null;
            return fieldDef;
        }
        #endregion
    }
}
