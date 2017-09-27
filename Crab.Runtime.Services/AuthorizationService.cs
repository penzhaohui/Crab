using System;
using System.Collections.Generic;
using System.Text;
using Crab.Runtime.Contract;
using Crab.Runtime.Logic;

namespace Crab.Runtime.Services
{
    public class AuthorizationService: IAuthorization
    {
        #region implements IAuthorization
        public void CreateRole(string tenantName, string roleName)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName");

            AuthorizationLogic.CreateRole(tenantName, roleName);
        }

        public void DeleteRole(string tenantName, string roleName)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName");

            AuthorizationLogic.DeleteRole(tenantName, roleName);
        }

        public string[] GetAllRoles(string tenantName)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");

            return AuthorizationLogic.GetAllRoles(tenantName);
        }

        public void AddUsersToRoles(string tenantName, string[] usernames, string[] roleNames)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            if (usernames == null || usernames.Length == 0)
                throw new ArgumentNullException("usernames");
            if (roleNames == null || roleNames.Length == 0)
                throw new ArgumentNullException("roleNames");

            AuthorizationLogic.AddUsersToRoles(tenantName, usernames, roleNames);
        }

        public void RemoveUsersFromRoles(string tenantName, string[] usernames, string[] roleNames)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            if (usernames == null || usernames.Length == 0)
                throw new ArgumentNullException("usernames");
            if (roleNames == null || roleNames.Length == 0)
                throw new ArgumentNullException("roleNames");

            AuthorizationLogic.RemoveUsersFromRoles(tenantName, usernames, roleNames);
        }

        public string[] GetRolesForUser(string tenantName, string username)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");

            return AuthorizationLogic.GetRolesForUser(tenantName, username);
        }

        public string[] GetUsersInRole(string tenantName, string roleName)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName");

            return AuthorizationLogic.GetUsersInRole(tenantName, roleName);
        }

        public bool RoleExists(string tenantName, string roleName)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName");

            return AuthorizationLogic.RoleExists(tenantName, roleName);
        }

        public bool IsUserInRole(string tenantName, string username, string roleName)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");
            if (string.IsNullOrEmpty(roleName))
                throw new ArgumentNullException("roleName");

            return AuthorizationLogic.IsUserInRole(tenantName, username, roleName);
        }
        #endregion
    }
}
