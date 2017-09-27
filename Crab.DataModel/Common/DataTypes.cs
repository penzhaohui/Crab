using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Crab.DataModel.Common
{
    /// <summary>
    /// The data types of the field
    /// </summary>
    [Serializable]
    [ComVisible(true)]
    public enum DataTypes
    {
        String = 0,
        Int,
        Decimal,
        DateTime,
        Boolean,
        Guid
    }
}
