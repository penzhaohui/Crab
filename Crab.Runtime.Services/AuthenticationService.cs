using System;
using System.Collections.Generic;
using System.Text;
using Crab.Runtime.Contract;
using Crab.Runtime.Logic;

namespace Crab.Runtime.Services
{
    public class AuthenticationService: IAuthentication
    {
        #region implements IAuthentication
        public void CreateUser(string tenantName, string username, string password, string email)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException("email");

            AuthenticationLogic.CreateUser(tenantName, username, password, email);
        }

        public void DeleteUser(string tenantName, string username)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");

            AuthenticationLogic.DeleteUser(tenantName, username);
        }

        public bool UserExists(string tenantName, string username)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");

            return AuthenticationLogic.UserExists(tenantName, username);
        }

        public bool ValidateUser(string tenantName, string username, string password)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            return AuthenticationLogic.ValidateUser(tenantName, username, password);
        }

        public void SetPassword(string tenantName, string username, string password)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            AuthenticationLogic.SetPassword(tenantName, username, password);
        }

        public void ChangePassword(string tenantName, string username, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");
            if (string.IsNullOrEmpty(oldPassword))
                throw new ArgumentNullException("oldPassword");
            if (string.IsNullOrEmpty(newPassword))
                throw new ArgumentNullException("newPassword");

            AuthenticationLogic.ChangePassword(tenantName, username, oldPassword, newPassword);
        }

        public string[] GetUsers(string tenantName)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");

            return AuthenticationLogic.GetUsers(tenantName);
        }

        public string[] FindUsers(string tenantName, string usernameToMatch)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");

            return AuthenticationLogic.FindUsers(tenantName, usernameToMatch);
        }

        public AdamUser GetAdamUser(string tenantName, string username)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");

            return AuthenticationLogic.GetAdamUser(tenantName, username);
        }

        public Guid GetUserIdByName(string tenantName, string username)
        {
            if (string.IsNullOrEmpty(tenantName))
                throw new ArgumentNullException("tenantName");
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException("username");

            return AuthenticationLogic.GetUserIdByName(tenantName, username);
        }

        public string GetUpnByUserId(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentNullException("id");

            return AuthenticationLogic.GetUpnByUserId(id);
        }
        #endregion
    }
}
