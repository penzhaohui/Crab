using System;
using System.Collections.Generic;
using System.Text;

namespace Crab.DataModel.Utility
{
    /// <summary>
    /// The helper class for sql string
    /// </summary>
    public static class SqlFormat
    {
        /// <summary>
        /// Convert an object to value of sql string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToSqlValueString(object value)
        {
            if (value == null)
                return "NULL";
            Type returnType = value.GetType();
            if (returnType.IsGenericType &&
                    returnType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(returnType);
                returnType = nullableConverter.UnderlyingType;
            }
            if (returnType == typeof(string))
            {
                return "N'" + EscapeSql(value as string) + "'";
            }
            else if (returnType == typeof(DateTime))
            {
                return "'" + value + "'";
            }
            else if (returnType == typeof(Guid))
            {
                return "'{" + value.ToString() + "}'";
            }
            else if (returnType.IsEnum)
            {
                return ((int)value).ToString();
            }
            else if (returnType == typeof(bool))
            {
                return Convert.ChangeType(value, typeof(int)).ToString();
            }
            else
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// Escape a string value to prevent sql injection
        /// </summary>
        /// <param name="value">the string value</param>
        /// <returns>The escaped sql string</returns>
        public static string EscapeSql(string value)
        {
            return value.Replace("'", "''");
        }
    }
}
