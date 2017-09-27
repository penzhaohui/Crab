using System;
using System.Collections.Generic;
using System.Text;
using Crab.DataModel.Common;
using System.ComponentModel;

namespace Crab.DataModel
{
    /// <summary>
    /// Reprensents the meta data of a entity
    /// </summary>
    public class EntityMetadata: DataNode
    {
        /// <summary>
        /// The types of the properties of the FieldContainer node
        /// </summary>
        private enum PropertyTypes
        {
            Caption = 0,
            SourceName = 1,
            Key = 2,
            EnableVersionCheck=3,
            VersionField=4,
            ExtensionTable = 5
        }

        public string SourceName
        {
            get { return GetPropertyValue<string>((int)PropertyTypes.SourceName); }
            set { SetPropertyValue<string>((int)PropertyTypes.SourceName, value); }
        }

        [Description("The primary keys, splitted by ','")]
        public string Key
        {
            get { return GetPropertyValue<string>((int)PropertyTypes.Key); }
            set { SetPropertyValue<string>((int)PropertyTypes.Key, value); }
        }

        [Description("Whether enable version check when update data to database.")]
        public bool EnableVersionCheck
        {
            get { return GetPropertyValue<bool>((int)PropertyTypes.EnableVersionCheck); }
            set { SetPropertyValue<bool>((int)PropertyTypes.EnableVersionCheck, value); }
        }

        [Description("The name of the version field"), DefaultValue("Version")]
        public string VersionField
        {
            get { return GetPropertyValue<string>((int)PropertyTypes.VersionField);}
            set { SetPropertyValue<string>((int)PropertyTypes.VersionField, value); }
        }

        [Description("Specify the table which stores the values for extension fields"), DefaultValue("Version")]
        public string ExtensionTable
        {
            get { return GetPropertyValue<string>((int)PropertyTypes.ExtensionTable); }
            set { SetPropertyValue<string>((int)PropertyTypes.ExtensionTable, value); }
        }

        [Browsable(false)]
        public string[] KeyFieldNames
        {
            get 
            {
                if (string.IsNullOrEmpty(Key))
                    return null;
                else
                    return Key.Split(',');
            } 
        }

        [Browsable(false)]
        public FieldMetadata[] KeyFields
        {
            get
            {
                string[] keyFieldNames = KeyFieldNames;
                int size = keyFieldNames == null?0:keyFieldNames.Length;
                FieldMetadata[] fileds = new FieldMetadata[size];
                for (int i = 0; i < fileds.Length; i++)
                {
                    fileds[i] = (FieldMetadata)ChildNodes[keyFieldNames[i]];
                }
                return fileds;
            }
        }

        [Browsable(false)]
        public FieldMetadata[] Fields
        {
            get
            {
                FieldMetadata[] fileds = new FieldMetadata[ChildNodes.Count];
                ChildNodes.CopyTo((DataNode[])fileds, 0);
                return fileds;
            }
        }

        [Browsable(false)]
        public FieldMetadata[] PreDefinedFields
        {
            get
            {
                List<FieldMetadata> fields = new List<FieldMetadata>();
                foreach (FieldMetadata field in ChildNodes)
                {
                    if (!field.IsExtension)
                        fields.Add(field);
                }
                return fields.ToArray();
            }
        }

        [Browsable(false)]
        public FieldMetadata[] ExtensionFields
        {
            get
            {
                List<FieldMetadata> fields = new List<FieldMetadata>();
                foreach (FieldMetadata field in ChildNodes)
                {
                    if (field.IsExtension)
                        fields.Add(field);
                }
                return fields.ToArray();
            }
        }

        public bool ContainsField(string fieldName)
        {
            return ChildNodes.Contains(fieldName);
        }

        public FieldMetadata this[string fieldName]
        {
            get
            {
                return ChildNodes[fieldName] as FieldMetadata;
            }
        }

        public FieldMetadata this[Guid fieldId]
        {
            get
            {
                return ChildNodes[fieldId] as FieldMetadata;
            }
        }
    }
}
