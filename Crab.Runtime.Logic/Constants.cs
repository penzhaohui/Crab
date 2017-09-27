using System;
using System.Collections.Generic;
using System.Text;

namespace Crab.Runtime.Logic
{
    internal static class Constants
    {
        internal static class Database
        {
            internal const string TenantData = "TenantData";
            internal const string TenantIdentity = "TenantIdentity";
            internal const string TenantMetadata = "TenantMetadata";
            internal const string TenantWorkflowStore = "TenantWorkflowStore";
        }

        internal static class Provision
        {
            internal const int TrialDays = 90;
        }

        internal static class AdamSettings
        {
            internal const string ConnectionStringName = "ADAM_ConnectionString";
            internal const string ConnectionUsername = "ADAM_Username";
            internal const string ConnectionPassword = "ADAM_Password";
            internal const string SecureConnection = "ADAM_SecureConnection";
        }

        internal static class Authorization
        {
            internal const string Roles = "Tenant_Roles";
            internal const string AdminRole = "Tenant_Admin_Role";
        }
    }
}
