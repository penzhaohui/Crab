using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Crab.DataModel
{
    public sealed class DataModelContext
    {
        [ThreadStatic]
        private static IDictionary<string, object> _context;

        public  static Guid? TenantId
        {
            get
            {
                return (Guid?)GetValue(Constants.DataModelContext.TenantId);
            }
            set
            {
                SetValue(Constants.DataModelContext.TenantId, value);
            }
        }

        public static string TenantName
        {
            get
            {
                return (string)GetValue(Constants.DataModelContext.TenantName);
            }
            set
            {
                SetValue(Constants.DataModelContext.TenantName, value);
            }
        }

        public static Guid? UserId
        {
            get
            {
                return (Guid?)GetValue(Constants.DataModelContext.UserId);
            }
            set
            {
                SetValue(Constants.DataModelContext.UserId, value);
            }
        }

        public static string Username
        {
            get
            {
                return (string)GetValue(Constants.DataModelContext.Username);
            }
            set
            {
                SetValue(Constants.DataModelContext.Username, value);
            }
        }

        internal static DataModelWorkspace DataModelWorkspace
        {
            get
            {
                return (DataModelWorkspace)GetValue(Constants.DataModelContext.Workspace);
            }
            set
            {
                SetValue(Constants.DataModelContext.Workspace, value);
            }
        }

        private static object GetValue(string name)
        {
            object tempValue = null;
            if (_context != null)
                _context.TryGetValue(name, out tempValue);
            return tempValue;
        }

        private static void SetValue(string name, object value)
        {
            if (_context == null)
                _context = new Dictionary<string, object>();
            _context[name] = value;
        }

        public static void Initialize(Guid tenantId)
        {
            _context = null;
            DataModelContext.TenantId = tenantId==Guid.Empty?(Guid?)null:tenantId;
        }

        public static void Clear()
        {
            _context = null;
        }
    }
}
