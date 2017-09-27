using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Crab.DataModel.Common;

namespace Crab.DataModel
{
    /// <summary>
    /// Representing the metadata for the fileds of the entity
    /// </summary>
    [Serializable]
    public class FieldMetadata: DataNode
    {
        /// <summary>
        /// The types of the properties of the EntityFieldMetaData node
        /// </summary>
        private enum PropertyTypes
        {
            Caption =0,
            ColumnName = 1,
            DataType = 2,
            Length = 3,
            Nullable = 4
            /*DefaultValue = 5,
            ColumnOrder = 6,
            Visibility = 7,
            Readonly = 8,
            ControlType =9,
            Required = 10*/
        }

        /// <summary>
        /// Gets or Sets the column name of the physical table
        /// </summary>
        [Description("The column name of the physical table")]
        public string ColumnName
        {
            get { return GetPropertyValue<string>((int)PropertyTypes.ColumnName); }
            set { SetPropertyValue<string>((int)PropertyTypes.ColumnName, value); }
        }

        /// <summary>
        /// Gets or Sets the Data type of the entity field
        /// </summary>
        [Description("Data type of the entity field")]
        public DataTypes DataType
        {
            get
            {
                return GetPropertyValue<DataTypes>((int)PropertyTypes.DataType); 
            }
            set
            {
                SetPropertyValue<DataTypes>((int)PropertyTypes.DataType, value);
            }
        }


        /// <summary>
        /// Gets or Sets the max length of the field. This field is meaningful only if the data type is string
        /// </summary> 
        [Description("The lengh of the inputted characters allowed.")]
        public int Length
        {
            get { return GetPropertyValue<int>((int)PropertyTypes.Length); }
            set { SetPropertyValue<int>((int)PropertyTypes.Length, value); }
        }

        /// <summary>
        /// Gets or Sets whether the field is allowed to be null
        /// </summary> 
        [Description("Whether the field is allowed to be null"), DefaultValue(true)]
        public bool Nullable
        {
            get { return GetPropertyValue<bool>((int)PropertyTypes.Nullable); }
            set { SetPropertyValue<bool>((int)PropertyTypes.Nullable, value); }
        }

        /*
        /// <summary>
        /// Gets or Sets the the value indicating if the field is primary key
        /// </summary>
        [DefaultValue(false)]
        public bool IsPrimaryKey
        {
            get { return GetPropertyValue<bool>((int)PropertyTypes.IsPrimaryKey); }
            set { SetPropertyValue<bool>((int)PropertyTypes.IsPrimaryKey, value); }
        }

        /// <summary>
        /// Gets or Sets the default value of the field
        /// </summary>
        [Description("The default value of the field")]
        public string DefaultValue
        {
            get { return GetPropertyValue<string>((int)PropertyTypes.DefaultValue); }
            set { SetPropertyValue<string>((int)PropertyTypes.DefaultValue, value); }
        }
        /// <summary>
        /// Gets or Sets the order 
        /// </summary>
        [Description("The order of the column")]
        public int ColumnOrder
        {
            get { return GetPropertyValue<int>((int)PropertyTypes.ColumnOrder); }
            set { SetPropertyValue<int>((int)PropertyTypes.ColumnOrder, value); }
        }

        /// <summary>
        /// Gets or Sets the visibility of the field on UI
        /// </summary>
        [Description("The visibility of the field")]
        public Visibilities Visibility
        {
            get { return GetPropertyValue<Visibilities>((int)PropertyTypes.Visibility); }
            set { SetPropertyValue<Visibilities>((int)PropertyTypes.Visibility, value); }
        }

        /// <summary>
        /// Gets or Sets the whether the field is readonly
        /// </summary>
        [Description("Whether the field is readonly for user on UI")]
        public bool Readonly
        {
            get { return GetPropertyValue<bool>((int)PropertyTypes.Readonly); }
            set { SetPropertyValue<bool>((int)PropertyTypes.Readonly, value); }
        }


        /// <summary>
        /// Gets or Sets the control type of the field
        /// </summary>
        [Description("The control type of the field")]
        public ControlTypes ControlType
        {
            get { return GetPropertyValue<ControlTypes>((int)PropertyTypes.ControlType); }
            set { SetPropertyValue<ControlTypes>((int)PropertyTypes.ControlType, value); }
        }

        /// <summary>
        /// Gets or Sets whether the field can not be empty
        /// </summary>
        [Description("Whether the field can not ben empty. true means can not be empty."), DefaultValue(false)]
        public bool Required
        {
            get { return GetPropertyValue<bool>((int)PropertyTypes.Required); }
            set { SetPropertyValue<bool>((int)PropertyTypes.Required, value); }
        }
         */
    }
}
