using System;
using System.Collections.Generic;
using System.Text;
using Crab.DataModel;
using Crab.DataModel.Common;
using Crab.Runtime.Contract;
using Crab.Runtime.Logic;

namespace Crab.Runtime.Services
{
    public class MetadataService: IMetadataService
    {
        #region implements IMetadataService
        public EntityDef[] GetEntityDefs()
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");

            EntityDef[] result = null;
            try
            {
                DataModelContext.Initialize(RequestContext.Current.TenantId);
                result = MetadataLogic.GetEntityDefs();
            }
            finally
            {
                DataModelContext.Clear();
            }
            return result;
        }

        public EntityDef GetEntityDefById(Guid id)
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");
            if (id == Guid.Empty)
                throw new ArgumentNullException("id");
            EntityDef result = null;
            try
            {
                DataModelContext.Initialize(RequestContext.Current.TenantId);
                result = MetadataLogic.GetEntityDefById(id);
            }
            finally
            {
                DataModelContext.Clear();
            }
            return result;
        }

        public EntityDef GetEnityDefByName(string name)
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            EntityDef result = null;
            try
            {
                DataModelContext.Initialize(RequestContext.Current.TenantId);
                result = MetadataLogic.GetEnityDefByName(name);
            }
            finally
            {
                DataModelContext.Clear();
            }
            return result;
        }

        public void UpdateEntityDef(Guid entityDefId, string caption)
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");
            if (entityDefId == Guid.Empty)
                throw new ArgumentNullException("entityDefId");

            try
            {
                DataModelContext.Initialize(RequestContext.Current.TenantId);
                MetadataLogic.UpdateEntityDef(entityDefId, caption);
            }
            finally
            {
                DataModelContext.Clear();
            }
        }

        public EntityFieldDef[] GetFieldDefs(Guid entityDefId)
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");
            if (entityDefId == Guid.Empty)
                throw new ArgumentNullException("entityDefId");
            EntityFieldDef[] result = null;
            try
            {
                DataModelContext.Initialize(RequestContext.Current.TenantId);
                result = MetadataLogic.GetFieldDefs(entityDefId);
            }
            finally
            {
                DataModelContext.Clear();
            }
            return result;
        }

        public EntityFieldDef GetFieldDefById(Guid fieldId)
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");
            if (fieldId == Guid.Empty)
                throw new ArgumentNullException("fieldId");
            EntityFieldDef result = null;
            try
            {
                DataModelContext.Initialize(RequestContext.Current.TenantId);
                result = MetadataLogic.GetFieldDefById(fieldId);
            }
            finally
            {
                DataModelContext.Clear();
            }
            return result;
        }


        public EntityFieldDef AddEntityFieldDef(Guid entityDefId, string fieldName, string caption, DataTypes dataType, int length)
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");
            if (entityDefId == Guid.Empty)
                throw new ArgumentNullException("entityDefId");
            if(string.IsNullOrEmpty(fieldName))
                    throw new ArgumentNullException("fieldName");
            EntityFieldDef result = null;
            try
            {
                DataModelContext.Initialize(RequestContext.Current.TenantId);
                result = MetadataLogic.AddEntityFieldDef(entityDefId, fieldName, caption, dataType, length);
            }
            finally
            {
                DataModelContext.Clear();
            }
            return result;
        }

        public void DeleteEntityFieldDef(Guid fieldDefId)
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");

            if (fieldDefId == Guid.Empty)
                throw new ArgumentNullException("fieldDefId");
            try
            {
                DataModelContext.Initialize(RequestContext.Current.TenantId);
                MetadataLogic.DeleteEntityFieldDef(fieldDefId);
            }
            finally
            {
                DataModelContext.Clear();
            }
        }

        public void UpdateEntityFieldDef(Guid fieldDefId, string caption)
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");
            if (fieldDefId == Guid.Empty)
                throw new ArgumentNullException("fieldDefId");

            try
            {
                DataModelContext.Initialize(RequestContext.Current.TenantId);
                MetadataLogic.UpdateEntityFieldDef(fieldDefId, caption);
            }
            finally
            {
                DataModelContext.Clear();
            }
        }

        #endregion
    }
}
