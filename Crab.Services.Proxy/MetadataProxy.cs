using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using Crab.Runtime.Contract;
using Crab.DataModel.Common;

namespace Crab.Services.Proxy
{
    static public class MetadataProxy
    {
        /// <summary>
        /// Channel to proxy IMetadataService
        /// </summary>
        private class BasicChannel : ClientBase<IMetadataService>, IMetadataService
        {
            public EntityDef[] GetEntityDefs()
            {
                return base.Channel.GetEntityDefs();
            }

            public EntityDef GetEntityDefById(Guid id)
            {
                return base.Channel.GetEntityDefById(id);
            }

            public EntityDef GetEnityDefByName(string name)
            {
                return base.Channel.GetEnityDefByName(name);
            }

            public void UpdateEntityDef(Guid entityDefId, string caption)
            {
                base.Channel.UpdateEntityDef(entityDefId, caption);
            }

            public EntityFieldDef[] GetFieldDefs(Guid entityDefId)
            {
                return base.Channel.GetFieldDefs(entityDefId);
            }

            public EntityFieldDef GetFieldDefById(Guid fieldId)
            {
                return base.Channel.GetFieldDefById(fieldId);
            }

            public EntityFieldDef AddEntityFieldDef(Guid entityDefId, string fieldName, string caption, DataTypes dataType, int length)
            {
                return base.Channel.AddEntityFieldDef(entityDefId, fieldName, caption, dataType, length);
            }

            public void DeleteEntityFieldDef(Guid fieldDefId)
            {
                base.Channel.DeleteEntityFieldDef(fieldDefId);
            }

            public void UpdateEntityFieldDef(Guid fieldDefId, string caption)
            {
                base.Channel.UpdateEntityFieldDef(fieldDefId, caption);
            }
        }

        static public EntityDef[] GetEntityDefs()
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetEntityDefs();
            }
        }

        static public EntityDef GetEntityDefById(Guid id)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetEntityDefById(id);
            }
        }

        static public EntityDef GetEnityDefByName(string name)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetEnityDefByName(name);
            }
        }

        static public void UpdateEntityDef(Guid entityDefId, string caption)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.UpdateEntityDef(entityDefId, caption);
            }
        }

        static public EntityFieldDef[] GetFieldDefs(Guid entityDefId)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetFieldDefs(entityDefId);
            }
        }

        static public EntityFieldDef GetFieldDefById(Guid fieldId)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetFieldDefById(fieldId);
            }
        }

        static public EntityFieldDef AddEntityFieldDef(Guid entityDefId, string fieldName, string caption, DataTypes dataType, int length)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.AddEntityFieldDef(entityDefId, fieldName, caption, dataType, length);
            }
        }

        static public void DeleteEntityFieldDef(Guid fieldDefId)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.DeleteEntityFieldDef(fieldDefId);
            }
        }

        static public void UpdateEntityFieldDef(Guid fieldDefId, string caption)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.UpdateEntityFieldDef(fieldDefId, caption);
            }
        }
    }
}
