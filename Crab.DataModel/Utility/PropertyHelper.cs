using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Crab.DataModel.Utility
{
    /// <summary>
    /// Helper class to get/set the public property of an object
    /// </summary>
    public sealed class PropertyHelper
    {
        public static object GetPropertyValue(object target, string propName)
        {
            PropertyInfo propInfo = target.GetType().GetProperty(propName);
            if (propInfo == null)
                return null;
            return propInfo.GetValue(target, null);
        }

        public static void SetPropertyValue(object target, string propName, object value)
        {
            PropertyInfo propInfo = target.GetType().GetProperty(propName);
            if (propInfo == null)
                return;
            propInfo.SetValue(target, value, null);
        }
    }
}
