using System;
using System.Collections.Generic;
using System.Text;

namespace Crab.DataModel.Utility
{
    /// <summary>
    /// The class used to convert a value to the given conversion type
    /// </summary>
    public static class TypeConvert
    {
        public static object ChangeType(object value, Type conversionType)
        {
            if(value is DBNull)
                value = null;
            if (value is string && string.IsNullOrEmpty((string)value)&&conversionType != typeof(string))
                value = null;
            if (value == null)
            {
                if (conversionType.IsValueType && !conversionType.IsGenericType)
                {
                    if (conversionType == typeof(int))
                        return Convert.ToInt32(value);
                    else if (conversionType == typeof(bool))
                        return Convert.ToBoolean(value);
                    else if (conversionType == typeof(DateTime))
                        return Convert.ToDateTime(value);
                    else if (conversionType == typeof(decimal))
                        return Convert.ToDecimal(value);
                    else if(conversionType == typeof(double))
                        return Convert.ToDouble(value);
                    else if(conversionType.IsEnum)
                        return Enum.GetValues(conversionType).GetValue(0);
                        //return Enum.ToObject(conversionType, 0);
                    else 
                        return null;
                }
                else
                    return null;
            }
            
            if (conversionType.IsGenericType &&
                    conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }

            if (value.GetType() == conversionType)
                return value;
            if (value is IConvertible)
            {
                if (conversionType.IsEnum)
                {
                    if (value is string&&!((value as string)[0]>=0&&(value as string)[0]<=9))
                        value = Enum.Parse(conversionType, value as string);
                    else if (value is string)
                        value = Convert.ToInt32(value);
                    value = Enum.ToObject(conversionType, value);
                }
                else if (conversionType == typeof(Boolean) && (value is string))
                {
                    bool boolValue = false;
                    if (!Boolean.TryParse((string)value, out boolValue))
                        value = Convert.ToInt32(value);
                }
                else if (conversionType == typeof(Guid) && (value is string))
                {
                    value = (object)new Guid((string)value);
                }
                value = Convert.ChangeType(value, conversionType);
            }
            else
            {
                value = Convert.ChangeType(value.ToString(), conversionType);
            }
            return value;
        }

        public static T ChangeType<T>(object value)
        {
            return (T)TypeConvert.ChangeType(value, typeof(T));
        }
    }
}
