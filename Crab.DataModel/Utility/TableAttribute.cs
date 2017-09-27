using System;
using System.Collections.Generic;
using System.Text;

namespace Crab.DataModel.Utility
{
    /// <summary>
    /// Attribute class 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TableAttribute : System.Attribute
    {
        private string tableName;

        public TableAttribute()
        {
            tableName = "";
        }

        public TableAttribute(string tableName)
        {
            this.tableName = tableName;
        }

        public string TableName
        {
            get { return this.tableName; }
            set { tableName = value; }
        }
    }
}
