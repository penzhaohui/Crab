using System;
using System.ServiceModel;

namespace Crab.Runtime.Contract
{
    [ServiceContract()]
    public interface IAuthentication
    {
        [OperationContract]
        void CreateUser(string tenantName, string username, string password, string email);

        [OperationContract]
        void DeleteUser(string tenantName, string username);

        [OperationContract]
        bool UserExists(string tenantName, string username);

        [OperationContract]
        bool ValidateUser(string tenantName, string username, string password);

        [OperationContract]
        void SetPassword(string tenantName, string username, string password);

        [OperationContract]
        void ChangePassword(string tenantName, string username, string oldPassword, string newPassword);

        [OperationContract]
        string[] GetUsers(string tenantName);

        [OperationContract]
        string[] FindUsers(string tenantName, string usernameToMatch);

        [OperationContract]
        AdamUser GetAdamUser(string tenantName, string username);

        [OperationContract]
        Guid GetUserIdByName(string tenantName, string username);

        [OperationContract]
        string GetUpnByUserId(Guid id);
    }
}
