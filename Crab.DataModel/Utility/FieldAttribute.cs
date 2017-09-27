using System;
using System.Collections.Generic;
using System.Text;

namespace Crab.DataModel.Utility
{
    /// <summary>
    /// The attribute class for mapping table field to object property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class FieldAttribute : System.Attribute
    {
        private string fieldName = "Unknown";
        private bool primaryKey;

        public FieldAttribute()
        {
            primaryKey = false;
        }

        public FieldAttribute(string fieldName)
        {
            this.fieldName = fieldName;
            primaryKey = false;
        }

        public FieldAttribute(string fieldName, bool isPrimaryKey)
        {
            this.fieldName = fieldName;
            primaryKey = isPrimaryKey;
        }

        /// <summary>
        /// Table field name
        /// </summary>
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        /// <summary>
        /// The boolean value to indicate whether the field is a primary key
        /// </summary>
        public bool PrimaryKey
        {
            get { return primaryKey; }
            set { primaryKey = value; }
        }
    }
}
