using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;

namespace Crab.DataModel.Utility
{
    /// <summary>
    /// Generic helper class to manipulate entities
    /// TODO: This class could be replaced with ADO.NET Entity Framework
    /// </summary>
    /// <typeparam name="T">The target entity type</typeparam>
    public static class SqlEntityHelper<T>
    {
        private static IDictionary<string, FieldAttribute> propToFieldMap;
        private static IDictionary<string, PropertyInfo> fieldToPropMap;
        private static Type entityType;
        private static string tableName;
        private static IList<FieldAttribute> primaryKey;

        /// <summary>
        /// Initialize a helper class to cache the field-property maps
        /// </summary>
        static SqlEntityHelper()
        {
            entityType = typeof(T);
            TableAttribute[] classAttributes = (TableAttribute[])entityType.GetCustomAttributes(typeof(TableAttribute), false);
            tableName = classAttributes != null && classAttributes.Length!=0 ? classAttributes[0].TableName : "";
            PropertyInfo[] props = entityType.GetProperties();
            propToFieldMap = new Dictionary<string, FieldAttribute>();
            fieldToPropMap = new Dictionary<string, PropertyInfo>();
            primaryKey = new List<FieldAttribute>(1);
            if (props != null)
            {
                foreach (PropertyInfo prop in props)
                {
                    FieldAttribute[] attrs = (FieldAttribute[])prop.GetCustomAttributes(typeof(FieldAttribute), true);
                    if (attrs != null && attrs.Length > 0)
                    {
                        propToFieldMap.Add(prop.Name, attrs[0]);
                        if (attrs[0].PrimaryKey)
                        {
                            primaryKey.Add(attrs[0]);
                        }
                        fieldToPropMap.Add(attrs[0].FieldName.ToUpper(), prop); //put the upper case fields
                    }
                }
            }
        }

        /// <summary>
        /// Gets the FieldAttribute object with the properyt name
        /// </summary>
        /// <param name="propName">The name of the property</param>
        /// <returns>A FieldAttribute object representing the specified field.</returns>
        public static FieldAttribute GetFieldByProperty(string propName)
        {
            FieldAttribute fieldAtt = null;
            propToFieldMap.TryGetValue(propName, out fieldAtt);
            return fieldAtt;
        }

        /// <summary>
        /// Gets the property name with the field name of the entity
        /// </summary>
        /// <param name="fieldName">Field name of the entity</param>
        /// <returns>The name of the property</returns>
        public static string GetPropertyByField(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                return null;
            PropertyInfo prop = null;
            fieldToPropMap.TryGetValue(fieldName.ToUpper(), out prop);
            return prop == null ? null : prop.Name;
        }

        /// <summary>
        /// Gets the PropertyInfo object with the field name
        /// </summary>
        /// <param name="fieldName">The field name of an entity</param>
        /// <returns>A PropertyInfo object representing the specified field</returns>
        public static PropertyInfo GetPropertyInfoByField(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
                return null;
            PropertyInfo prop = null;
            fieldToPropMap.TryGetValue(fieldName.ToUpper(), out prop);
            return prop;
        }

        /// <summary>
        /// Gets an ICollection<string> of all the property names of an entity
        /// </summary>
        public static ICollection<string> EntityProperties
        {
            get
            {
                return propToFieldMap.Keys;
            }
        }

        public static string[] EntityFields
        {
            get
            {
                IList<string> fields = new List<string>();
                foreach (KeyValuePair<string, FieldAttribute> pair in propToFieldMap)
                {
                    fields.Add(pair.Value.FieldName);
                }
                string[] result = new string[fields.Count];
                fields.CopyTo(result, 0);
                return result;
            }
        }

        public static IList<FieldAttribute> EntityFieldAttributes
        {
            get
            {
                IList<FieldAttribute> fields = new List<FieldAttribute>();
                foreach (KeyValuePair<string, FieldAttribute> pair in propToFieldMap)
                {
                    fields.Add(pair.Value);
                }
                return fields;
            }
        }

        public static void SetPropertyValue(T entity, string propName, object value)
        {
            PropertyInfo propInfo = entityType.GetProperty(propName);
            MethodInfo method = propInfo.GetGetMethod();
            Type returnType = method.ReturnType;
            value = TypeConvert.ChangeType(value, returnType);
            PropertyHelper.SetPropertyValue(entity, propName, value);
        }

        /// <summary>
        /// Writes all the fields to an entity from the SqlDataReader object
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="rdr">The System.Data.SqlClient.SqlDataReader which contains the values of the entity</param>
        public static void WriteEntity(T entity, SqlDataReader rdr)
        {
            for (int i = 0; i < rdr.FieldCount; i++)
            {
                string fieldName = rdr.GetName(i);
                string propName = GetPropertyByField(fieldName);
                if (string.IsNullOrEmpty(propName))
                    continue;  //can't find the correct property by the specific field name
                object dbValue = rdr[i];
                SetPropertyValue(entity, propName, dbValue);
            }

        }

        /// <summary>
        /// Writes all the fields to an entity from the DataRow object
        /// </summary>
        /// <param name="entity">The entity</param>
        /// <param name="dataRow">The System.Data.DataRow which contains the values of the entity</param>
        public static void WriteEntity(T entity, DataRow dataRow)
        {
            int count = dataRow.Table.Columns.Count;
            for (int i = 0; i < count; i++)
            {
                string fieldName = dataRow.Table.Columns[i].ColumnName;
                string propName = GetPropertyByField(fieldName);
                if (string.IsNullOrEmpty(propName))
                    continue;  //can't find the correct property by the specific field name
                object dbValue = dataRow[i];
                SetPropertyValue(entity, propName, dbValue);
            }
        }

        /// <summary>
        /// Gets the sql string of an property for dynamic sql string generation
        /// </summary>
        /// <param name="target">The target entity</param>
        /// <param name="propName">The property name</param>
        /// <returns>The sql string representing the property value</returns>
        public static string GetPropertySqlValueString(T target, string propName)
        {
            object value = PropertyHelper.GetPropertyValue(target, propName);
            return SqlFormat.ToSqlValueString(value);
        }

        /// <summary>
        /// Gets the insert scripts of an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string GenerateInsertScripts(T entity)
        {
            StringBuilder sb = new StringBuilder(100);
            sb.Append("INSERT INTO ").Append(tableName).Append(" ( ");
            string[] fields = EntityFields;
            for (int i = 0; i < fields.Length; i++)
            {
                sb.Append(fields[i]);
                if (i != fields.Length - 1)
                    sb.Append(", ");
            }
            sb.Append(") VALUES ( ");
            for (int i = 0; i < fields.Length; i++)
            {
                sb.Append(GetPropertySqlValueString(entity, GetPropertyByField(fields[i])));
                if (i != fields.Length - 1)
                    sb.Append(", ");
            }
            sb.Append(")");
            return sb.ToString();
        }

        /// <summary>
        /// Gets the update scripts of an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string GenerateUpdateScripts(T entity)
        {
            return GenerateUpdateScripts(entity, null);
        }

        /// <summary>
        /// Gets the update scripts of an entity with specified property names
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="props"></param>
        /// <returns></returns>
        public static string GenerateUpdateScripts(T entity, string[] props)
        {
            StringBuilder sb = new StringBuilder(100);
            sb.Append("UPDATE ").Append(tableName).Append(" SET ");
            string[] fields = EntityFields;
            for (int i = 0; i < fields.Length; i++)
            {
                if (IsPrimaryField(fields[i]))
                    continue;
                if(props!=null&&props.Length>0)
                {
                    string propName = GetPropertyByField(fields[i]);
                    int j=0;
                    for (; j < props.Length && propName.ToLower() != props[j].ToLower(); j++) ;
                    if(j == props.Length)
                        continue;
                }
                sb.Append(fields[i]).Append(" = ");
                sb.Append(GetPropertySqlValueString(entity, GetPropertyByField(fields[i])));
                if (i != fields.Length - 1)
                    sb.Append(", ");
            }
            sb.Append(" WHERE ");
            for (int i = 0; i < primaryKey.Count; i++)
            {
                sb.Append(primaryKey[i].FieldName).Append(" = ");
                sb.Append(GetPropertySqlValueString(entity, GetPropertyByField(primaryKey[i].FieldName)));
                if (i != primaryKey.Count - 1)
                    sb.Append(" AND ");
            }
            //append where clause
            return sb.ToString();
        }

        /// <summary>
        /// Gets the delete scritps of an entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static string GenerateDeleteScripts(T entity)
        {
            StringBuilder sb = new StringBuilder(100);
            sb.Append("DELETE FROM ").Append(tableName).Append(" WHERE ");
            for (int i = 0; i < primaryKey.Count; i++)
            {
                sb.Append(primaryKey[i].FieldName).Append(" = ").
                    Append(GetPropertySqlValueString(entity, GetPropertyByField(primaryKey[i].FieldName)));
                if (i != primaryKey.Count - 1)
                    sb.Append(" AND ");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets the physical table name of an entity
        /// </summary>
        public static string TableName
        {
            get { return tableName; }
        }

        /// <summary>
        /// Gets the boolean value which indicates whether the field is a primary key field
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static bool IsPrimaryField(string fieldName)
        {
            for (int i = 0; i < primaryKey.Count; i++)
            {
                if (primaryKey[i].FieldName.ToLower() == fieldName.ToLower())
                    return true;
            }
            return false;
        }
    }
}
