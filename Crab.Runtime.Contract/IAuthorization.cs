using System;
using System.ServiceModel;

namespace Crab.Runtime.Contract
{
    [ServiceContract()]
    public interface IAuthorization
    {
        [OperationContract]
        void CreateRole(string tenantName, string roleName);

        [OperationContract]
        void DeleteRole(string tenantName, string roleName);

        [OperationContract]
        string[] GetAllRoles(string tenantName);

        [OperationContract]
        void AddUsersToRoles(string tenantName, string[] usernames, string[] roleNames);

        [OperationContract]
        void RemoveUsersFromRoles(string tenantName, string[] usernames, string[] roleNames);

        [OperationContract]
        string[] GetRolesForUser(string tenantName, string username);

        [OperationContract]
        string[] GetUsersInRole(string tenantName, string roleName);

        [OperationContract]
        bool RoleExists(string tenantName, string roleName);

        [OperationContract]
        bool IsUserInRole(string tenantName, string username, string roleName);
    }
}
