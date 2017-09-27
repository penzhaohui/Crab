using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Util;
using System.Web.Security;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Configuration.Provider;
using System.Reflection;
using Crab.Runtime.Contract;
using Crab.Services.Proxy;

namespace Crab.Web.Security
{
    /// <summary>
    /// Manages storage of tenant role information for an ASP.NET application in
    //     Active Directory Application Mode by WCF
    /// </summary>
    public class WCFTenantRoleProvider: RoleProvider
    {
        public override string ApplicationName
        {
            get { return "/";}
            set { ;}
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            Upn upn = new Upn(HttpContext.Current.User.Identity.Name);
            string[] adamUsernames = FromUpns(usernames);
            AuthorizationProxy.AddUsersToRoles(upn.TenantName, adamUsernames, roleNames);
        }

        public override void CreateRole(string roleName)
        {
            string tenantName = null;
            string tenantUsername = null;
            Upn.TryParse(HttpContext.Current.User.Identity.Name, out tenantName, out tenantUsername);
            if (!string.IsNullOrEmpty(tenantName))
                AuthorizationProxy.CreateRole(tenantName, roleName);
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            string tenantName = null;
            string tenantUsername = null;
            Upn.TryParse(HttpContext.Current.User.Identity.Name, out tenantName, out tenantUsername);
            if (!string.IsNullOrEmpty(tenantName))
                AuthorizationProxy.DeleteRole(tenantName, roleName);
            return true;
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            string tenantName = null;
            string tenantUsername = null;
            Upn.TryParse(username, out tenantName, out tenantUsername);
            return AuthorizationProxy.IsUserInRole(tenantName, tenantUsername, roleName);
        }

        public override string[] GetRolesForUser(string username)
        {
            string tenantName = null;
            string tenantUsername = null;
            Upn.TryParse(username, out tenantName, out tenantUsername);
            return AuthorizationProxy.GetRolesForUser(tenantName, tenantUsername);
        }

        public override bool RoleExists(string roleName)
        {
            string tenantName = null;
            string tenantUsername = null;
            Upn.TryParse(HttpContext.Current.User.Identity.Name, out tenantName, out tenantUsername);
            if (!string.IsNullOrEmpty(tenantName))
                return AuthorizationProxy.RoleExists(tenantName, roleName);
            else
                return false;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            string tenantName = null;
            string tenantUsername = null;
            Upn.TryParse(HttpContext.Current.User.Identity.Name, out tenantName, out tenantUsername);
            string[] adamUsernames = FromUpns(usernames);
            if (!string.IsNullOrEmpty(tenantName))
                AuthorizationProxy.RemoveUsersFromRoles(tenantName, adamUsernames, roleNames);
        }

        public override string[] GetUsersInRole(string roleName)
        {
            string tenantName = null;
            string tenantUsername = null;
            Upn.TryParse(HttpContext.Current.User.Identity.Name, out tenantName, out tenantUsername);
            if (!string.IsNullOrEmpty(tenantName))
                return AuthorizationProxy.GetUsersInRole(tenantName, roleName);
            else
                return null;
        }

        public override string[] GetAllRoles()
        {
            string tenantName = null;
            string tenantUsername = null;
            Upn.TryParse(HttpContext.Current.User.Identity.Name, out tenantName, out tenantUsername);
            if (!string.IsNullOrEmpty(tenantName))
                return AuthorizationProxy.GetAllRoles(tenantName);
            else
                return null;
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotSupportedException();
        }

        private string[] FromUpns(string[] upns)
        {
            string tenantName = null;
            string[] usernames = new string[upns.Length];
            for (int i = 0; i < upns.Length; i++)
            {
                Upn.TryParse(upns[i], out tenantName, out usernames[i]);
            }
            return usernames;
        }

    }
}
