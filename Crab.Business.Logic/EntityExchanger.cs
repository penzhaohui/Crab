using System;
using System.Collections.Generic;
using System.Text;
using Crab.DataModel;
using Crab.DataModel.Data;
using Crab.DataModel.Common;
using Crab.DataModel.Utility;
using Crab.Runtime.Contract;

namespace Crab.Business.Logic
{
    public class EntityExchanger
    {
        static public void WriteEntityToDC(ExtensibleEntity entity, ExtensibleDC dc)
        {
            FieldMetadata[] fields = entity.Metadata.Fields;
            for (int i = 0; i < fields.Length; i++)
            {
                object value = entity.GetValue(fields[i].Name);
                dc.SetValue(fields[i].Name, value == null ? null : value.ToString());
            }
        }


        static public void WriteDCToEntity(ExtensibleDC dc, ExtensibleEntity entity)
        {
            EntityMetadata entityMetadata = ExtensibleEntity.GetEntityMetadata(entity.GetType());
            FieldMetadata[] fields = entityMetadata.Fields;
            for (int i = 0; i < fields.Length; i++)
            {
                //if (string.Compare(fields[i].Name, entityMetadata.Key, true) == 0)//skip the key fields
                 //   continue;
                if (!dc.Contains(fields[i].Name))
                    continue;
                FieldMetadata field = fields[i];
                object value = TypeConvert.ChangeType(dc.GetValue(field.Name), DataTypeConvert.ToSysType(field.DataType, field.Nullable));
                entity.SetValue(field.Name, value);
            }
        }
    }
}
