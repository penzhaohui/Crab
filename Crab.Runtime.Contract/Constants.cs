using System;
using System.Collections.Generic;
using System.Text;

namespace Crab.Runtime.Contract
{
    public static class Constants
    {
        #region Headers
        public static class Headers
        {
            public const string TenantNameHeaderName = "TenantName";
            public const string UsernnameHeaderName = "Username";
            public const string PasswordHeaderName = "Password";
            public const string TenantNameHeaderNamespace = "http://www.microsoft.com/architecture/2007/01/01/multitenancy/TenantName";
            public const string UsernameHeaderNamespace = "http://www.microsoft.com/architecture/2007/01/01/multitenancy/Username";
            public const string PasswordHeaderNamespace = "http://www.microsoft.com/architecture/2007/01/01/multitenancy/Password";
        }
        #endregion
    }
}
