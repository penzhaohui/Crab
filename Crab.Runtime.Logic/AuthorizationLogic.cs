using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Crab.Runtime.Contract;
using Crab.Runtime.Contract.Exceptions;
using Crab.Runtime.Logic.Common;

namespace Crab.Runtime.Logic
{
    public class AuthorizationLogic
    {
        public static string[] PreDefinedRoles
        {
            get 
            { 
                string allRoles = ConfigurationManager.AppSettings[Constants.Authorization.Roles];
                if (string.IsNullOrEmpty(allRoles))
                    return new string[0];
                else
                    return allRoles.Split(',');
            }
        }

        public static string AdminRole
        {
            get
            {
                return ConfigurationManager.AppSettings[Constants.Authorization.AdminRole];
            }
        }

        static public void CreateRole(string tenantName, string roleName)
        {
            AdamConfigurationHelper.AdamManager.CreateRole(tenantName, roleName);
        }

        static public void DeleteRole(string tenantName, string roleName)
        {
            AdamConfigurationHelper.AdamManager.DeleteRole(tenantName, roleName);
        }

        static public string[] GetAllRoles(string tenantName)
        {
            return AdamConfigurationHelper.AdamManager.GetAllRoles(tenantName);
        }

        static public void AddUsersToRoles(string tenantName, string[] usernames, string[] roleNames)
        {
            AdamConfigurationHelper.AdamManager.AddUsersToRoles(tenantName, usernames, roleNames);
        }

        static public void RemoveUsersFromRoles(string tenantName, string[] usernames, string[] roleNames)
        {
            AdamConfigurationHelper.AdamManager.RemoveUsersFromRoles(tenantName, usernames, roleNames);
        }

        static public string[] GetRolesForUser(string tenantName, string username)
        {
            return AdamConfigurationHelper.AdamManager.GetRolesForUser(tenantName, username);
        }

        static public string[] GetUsersInRole(string tenantName, string roleName)
        {
            return AdamConfigurationHelper.AdamManager.GetUsersInRole(tenantName, roleName);
        }

        static public bool RoleExists(string tenantName, string roleName)
        {
            return AdamConfigurationHelper.AdamManager.RoleExists(tenantName, roleName);
        }

        static public bool IsUserInRole(string tenantName, string username, string roleName)
        {
            return AdamConfigurationHelper.AdamManager.IsUserInRole(tenantName, username, roleName);
        }
    }
}
