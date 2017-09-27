using System;
using System.Collections.Generic;
using System.Text;
using Crab.DataModel.Utility;

namespace Crab.DataModel.Data
{
    internal class SqlUpdateScriptGenerator: IUpdateScriptGenerator
    {
        private class Consts
        {
            internal const string DefExtensionTable = "ExtensionValues";
        }

        public IEnumerable<string> GenerateUpdateScripts(EntityCacheEntry cacheEntry)
        {
            switch (cacheEntry.State)
            {
                case EntityRowState.Added:
                    return GenerateInsertScripts(cacheEntry);
                case EntityRowState.Deleted:
                    return GenerateDeleteScripts(cacheEntry);
                case EntityRowState.Modified:
                    {
                        ExtensibleEntity entity = cacheEntry.Entity as ExtensibleEntity;
                        return GenerateUpdateScripts(entity, entity.Metadata, entity.GetModifiedFields());
                    }
                default:
                    return null;
            }
        }

        protected IEnumerable<string> GenerateInsertScripts(EntityCacheEntry cacheEntry)
        {
            ExtensibleEntity entity = cacheEntry.Entity as ExtensibleEntity;
            return GenerateInsertScripts(entity, entity.Metadata);
            //TODO: insert relationships
        }

        protected IEnumerable<string> GenerateDeleteScripts(EntityCacheEntry cacheEntry)
        {
            ExtensibleEntity entity = cacheEntry.Entity as ExtensibleEntity;
            return GenerateDeleteScripts(entity, entity.Metadata);
            //TODO: delete relationships
        }

        protected IEnumerable<string> GenerateInsertScripts(IExtDataRecord dataRow, EntityMetadata rowMetaData)
        {
            List<string> scripts = new List<string>();
            FieldMetadata[] preDefFields = rowMetaData.PreDefinedFields;
            FieldMetadata[] extFields = rowMetaData.ExtensionFields;

            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO ").Append(rowMetaData.SourceName).Append(" ( ");
            bool addFlag = false;
            for (int i = 0; i < preDefFields.Length; i++)
            {
                object value = null;
                if (!dataRow.TryGetValue(preDefFields[i].Name, out value))
                    continue; //value not exists. Insert the least values
                if(addFlag)
                    sb.Append(", ");
                sb.Append(preDefFields[i].ColumnName);
                addFlag = true;
            }
            sb.Append(") VALUES ( ");
            addFlag = false;
            for (int i = 0; i < preDefFields.Length; i++)
            {
                object value = null;
                if (!dataRow.TryGetValue(preDefFields[i].Name, out value))
                    continue; //value not exists. Insert the least values
                if (addFlag)
                    sb.Append(", ");
                sb.Append(SqlFormat.ToSqlValueString(value));
                addFlag = true;
            }
            sb.Append(")");
            scripts.Add(sb.ToString());
            if (extFields == null || extFields.Length == 0)
                return scripts;
            string recordIdStringValue = SqlFormat.ToSqlValueString(dataRow.GetValue(rowMetaData.Key));
            string extensionTable = string.IsNullOrEmpty(rowMetaData.ExtensionTable) ? Consts.DefExtensionTable : rowMetaData.ExtensionTable;
            foreach (FieldMetadata extField in extFields)
            {
                object value = null;
                if (!dataRow.TryGetValue(extField.Name, out value))
                    continue;
                value = TypeConvert.ChangeType<string>(value);
                sb = new StringBuilder();
                sb.Append("INSERT INTO ").Append(extensionTable).
                    Append("(RecordId, FieldId, Value) VALUES (").Append(recordIdStringValue).
                    Append(",").Append(SqlFormat.ToSqlValueString(extField.Id)).
                    Append(",").Append(SqlFormat.ToSqlValueString(value)).Append(")");
                scripts.Add(sb.ToString());
            }
            return scripts;
        }

        protected IEnumerable<string> GenerateUpdateScripts(IExtDataRecord dataRow, EntityMetadata rowMetaData, IEnumerable<string> dirtyFields)
        {
            List<string> scripts = new List<string>();
            List<string> preDefDirtyFields = new List<string>();
            List<string> extDirtyFields = new List<string>();
            foreach (string dirtyField in dirtyFields)
            {
                if (!rowMetaData.ContainsField(dirtyField))
                    continue;
                if (!rowMetaData[dirtyField].IsExtension)
                    preDefDirtyFields.Add(dirtyField);
                else
                    extDirtyFields.Add(dirtyField);
            }
            //update predefined fields
            if (preDefDirtyFields.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("UPDATE ").Append(rowMetaData.SourceName).Append(" SET ");
                for (int i = 0; i < preDefDirtyFields.Count; i++)
                {
                    sb.Append(preDefDirtyFields[i]).Append(" = ").
                        Append(SqlFormat.ToSqlValueString(dataRow.GetValue(preDefDirtyFields[i])));
                    if (i != preDefDirtyFields.Count - 1)
                        sb.Append(", ");
                }
                sb.Append(" WHERE ").Append(GetConditionClause(dataRow, rowMetaData.KeyFieldNames));
                scripts.Add(sb.ToString());
            }

            //uppdate extension values
            if (extDirtyFields.Count > 0)
            {
                string recordIdStringValue = SqlFormat.ToSqlValueString(dataRow.GetValue(rowMetaData.Key));
                string extensionTable = string.IsNullOrEmpty(rowMetaData.ExtensionTable) ? Consts.DefExtensionTable : rowMetaData.ExtensionTable;
                foreach (string extFieldName in extDirtyFields)
                {
                    object value = dataRow.GetValue(extFieldName);
                    value = TypeConvert.ChangeType<string>(value);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("DELETE FROM ").Append(extensionTable).
                        Append(" WHERE RecordId=").Append(recordIdStringValue).
                        Append(" AND FieldId=").Append(SqlFormat.ToSqlValueString(rowMetaData[extFieldName].Id));
                    scripts.Add(sb.ToString());
                    sb = new StringBuilder();
                    sb.Append("INSERT INTO ").Append(extensionTable).
                        Append("(RecordId, FieldId, Value ) VALUES (").Append(recordIdStringValue).
                        Append(",").Append(SqlFormat.ToSqlValueString(rowMetaData[extFieldName].Id)).
                        Append(",").Append(SqlFormat.ToSqlValueString(value)).Append(")");
                    scripts.Add(sb.ToString());
                }
            }
            //updates extension fields
            return scripts;
        }

        protected IEnumerable<string> GenerateDeleteScripts(IExtDataRecord dataRow, EntityMetadata rowMetaData)
        {
            List<string> scripts = new List<string>();
            string[] keyFields = rowMetaData.KeyFieldNames;
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE FROM ").Append(rowMetaData.SourceName).
                Append(" WHERE ").Append(GetConditionClause(dataRow, rowMetaData.KeyFieldNames));
            scripts.Add(sb.ToString());
            sb = new StringBuilder();
            string extensionTable = string.IsNullOrEmpty(rowMetaData.ExtensionTable) ? Consts.DefExtensionTable : rowMetaData.ExtensionTable;
            sb.Append("DELETE FROM ").Append(extensionTable).
                Append(" WHERE RecordId=").
                Append(SqlFormat.ToSqlValueString(dataRow.GetValue(rowMetaData.Key)));
            scripts.Add(sb.ToString());
            return scripts;
        }

        private string GetConditionClause(IExtDataRecord dataRow, string[] conditionFields)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < conditionFields.Length; i++)
            {
                object value = dataRow.GetValue(conditionFields[i]);
                string op = "=";
                if (value == null || value is DBNull)
                    op = " IS ";
                sb.Append(conditionFields[i]).Append(op).Append(SqlFormat.ToSqlValueString(value));
                if (i != conditionFields.Length - 1)
                    sb.Append(" AND ");
            }
            return sb.ToString();
        }
    }
}
