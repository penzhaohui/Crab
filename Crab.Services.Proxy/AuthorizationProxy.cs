using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using Crab.Runtime.Contract;

namespace Crab.Services.Proxy
{
    static public class AuthorizationProxy
    {
        /// <summary>
        /// Channel to proxy IAuthorization
        /// </summary>
        private class BasicChannel : ClientBase<IAuthorization>, IAuthorization
        {
            #region Implements IAuthorization
            public void CreateRole(string tenantName, string roleName)
            {
                base.Channel.CreateRole(tenantName, roleName);
            }

            public void DeleteRole(string tenantName, string roleName)
            {
                base.Channel.DeleteRole(tenantName, roleName);
            }

            public string[] GetAllRoles(string tenantName)
            {
                return base.Channel.GetAllRoles(tenantName);
            }

            public void AddUsersToRoles(string tenantName, string[] usernames, string[] roleNames)
            {
                base.Channel.AddUsersToRoles(tenantName, usernames, roleNames);
            }

            public void RemoveUsersFromRoles(string tenantName, string[] usernames, string[] roleNames)
            {
                base.Channel.RemoveUsersFromRoles(tenantName, usernames, roleNames);
            }

            public string[] GetRolesForUser(string tenantName, string username)
            {
                return base.Channel.GetRolesForUser(tenantName, username);
            }

            public string[] GetUsersInRole(string tenantName, string roleName)
            {
                return base.Channel.GetRolesForUser(tenantName, roleName);
            }

            public bool RoleExists(string tenantName, string roleName)
            {
                return base.Channel.RoleExists(tenantName, roleName);
            }

            public bool IsUserInRole(string tenantName, string username, string roleName)
            {
                return base.Channel.IsUserInRole(tenantName, username, roleName);
            }
            #endregion
        }

        static public void CreateRole(string tenantName, string roleName)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.CreateRole(tenantName, roleName);
            }
        }

        static public void DeleteRole(string tenantName, string roleName)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.DeleteRole(tenantName, roleName);
            }
        }

        static public string[] GetAllRoles(string tenantName)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetAllRoles(tenantName);
            }
        }

        static public void AddUsersToRoles(string tenantName, string[] usernames, string[] roleNames)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.AddUsersToRoles(tenantName, usernames, roleNames);
            }
        }

        static public void RemoveUsersFromRoles(string tenantName, string[] usernames, string[] roleNames)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.RemoveUsersFromRoles(tenantName, usernames, roleNames);
            }
        }

        static public string[] GetRolesForUser(string tenantName, string username)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetRolesForUser(tenantName, username);
            }
        }

        static public string[] GetUsersInRole(string tenantName, string roleName)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetUsersInRole(tenantName, roleName);
            }
        }

        static public bool RoleExists(string tenantName, string roleName)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.RoleExists(tenantName, roleName);
            }
        }

        static public bool IsUserInRole(string tenantName, string username, string roleName)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.IsUserInRole(tenantName, username, roleName);
            }
        }
    }
}
