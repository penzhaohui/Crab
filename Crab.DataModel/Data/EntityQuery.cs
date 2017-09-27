using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Crab.DataModel.Utility;

namespace Crab.DataModel.Data
{
    /// <summary>
    /// Reprensents the class to be used to query entities from database
    /// </summary>
    public class EntityQuery<T>: IEnumerable<T>, IEnumerable
    {
        private class Consts
        {
            internal const string DefExtensionTable = "ExtensionValues";
        }
        private ExtensibleObjectContextBase _context;
        private string _queryClause;

        internal EntityQuery(ExtensibleObjectContextBase context, string queryClause)
        {
            Type entityType = typeof(ExtensibleEntity);
            //if (!entityType.IsAssignableFrom(typeof(T)))
              //  throw new ArgumentException("Must use a type inherited from ExtensibleEntity", "type");
            this._queryClause = queryClause;
            this._context = context;
        }

        internal EntityQuery(ExtensibleObjectContextBase context, params KeyValuePair<string, object>[] conditions)
            :this(context, ToSqlClause(conditions))
        {
        }

        public int GetCount()
        {
            string sqlCommand = string.Format("SELECT COUNT(0) FROM ({0}) AS T", BuildSqlString(typeof(T), _queryClause));
            Trace.WriteLine(sqlCommand);
            using (DbCommand command = _context.Connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = sqlCommand;
                return (int)command.ExecuteScalar();
            }
        }

        private static string BuildSqlString(Type entityType, string queryClause)
        {
            EntityMetadata entityMetaData = ExtensibleEntity.GetEntityMetadata(entityType);
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            FieldMetadata[]  fields = entityMetaData.PreDefinedFields;
            for (int i = 0; i < fields.Length; i++)
            {
                sb.Append(string.Format("P.{0} AS {1}", fields[i].ColumnName, fields[i].Name));
                if (i != fields.Length - 1)
                    sb.Append(",");
            }
            fields = entityMetaData.ExtensionFields;
            if(fields.Length>0)
                sb.Append(",");
            for (int i = 0; i < fields.Length; i++)
            {
                sb.Append(string.Format("E{0}.Value AS {1}", i, fields[i].Name));
                if (i != fields.Length - 1)
                    sb.Append(",");
            }
            sb.Append(string.Format(" FROM {0} AS P", entityMetaData.SourceName));
            string extensionTableName = string.IsNullOrEmpty(entityMetaData.ExtensionTable) ? Consts.DefExtensionTable : entityMetaData.ExtensionTable;
            if (fields.Length > 0)
            {
                for(int i=0; i<fields.Length; i++)
                {
                    sb.Append(string.Format(" LEFT OUTER JOIN {0} AS E{1}", extensionTableName, i));
                    sb.Append(string.Format(" ON P.{0} = E{1}.RecordId AND E{1}.FieldId = {2}", 
                        entityMetaData.Key, i, SqlFormat.ToSqlValueString(fields[i].Id)));
                }
            }
            if(!string.IsNullOrEmpty(queryClause))
                sb.Append(" WHERE ").Append(queryClause);
            return sb.ToString();
        }

        internal DbDataReader ExecuteReader()
        {
            string commandText = BuildSqlString(typeof(T), _queryClause);
            Trace.WriteLine(commandText);
            DbCommand command = _context.Connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = commandText;
            return command.ExecuteReader();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return this.GetResults().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetResults().GetEnumerator();
        }
 
        private IEnumerable<T> GetResults()
        {
            IEnumerable<T> enumerable = null;
            DbDataReader reader = ExecuteReader();
            try
            {
                enumerable = EntityMaterializer.CreateObjects<T>(reader, _context);
            }
            catch (InvalidCastException)
            {
                reader.Dispose();
                throw;
            }
            catch (NotSupportedException)
            {
                reader.Dispose();
                throw;
            }
            return enumerable;
        }

        private static string ToSqlClause(KeyValuePair<string, object>[] conditions)
        {
            if(conditions == null)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            for(int i=0; i<conditions.Length; i++)
            {
                string op = (conditions[i].Value ==null||conditions[i].Value is DBNull)?" IS ":"=";
                sb.Append(string.Format("{0}{1}{2}", 
                    SqlFormat.EscapeSql(conditions[i].Key), op, 
                    SqlFormat.ToSqlValueString(conditions[i].Value)));
                if(i != conditions.Length-1)
                    sb.Append(" AND ");
            }
            return sb.ToString();
        }

        public void Dispose()
        {
        }
    }
}
