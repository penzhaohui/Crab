using System;
using System.Collections.Generic;
using System.Text;

namespace Crab.DataModel.Common
{
    public sealed class DataTypeConvert
    {
        public static Type ToSysType(DataTypes dataType, bool nullable)
        {
            switch (dataType)
            {
                case DataTypes.String:
                    return typeof(string);
                case DataTypes.Int:
                    return nullable?typeof(int?):typeof(int);
                case DataTypes.Decimal:
                    return nullable ? typeof(decimal?) : typeof(decimal);
                case DataTypes.DateTime:
                    return nullable ? typeof(DateTime?) : typeof(DateTime);
                case DataTypes.Boolean:
                    return nullable ? typeof(Boolean?) : typeof(Boolean);
                case DataTypes.Guid:
                    return nullable ? typeof(Guid?) : typeof(Guid);
                default:
                    throw new ArgumentException("Invalid data type", "dataType");
            }
        }
    }
}
