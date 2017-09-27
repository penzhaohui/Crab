using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using Crab.Runtime.Contract;

namespace Crab.Services.Proxy
{
    static public class AuthenticationProxy
    {
        /// <summary>
        /// Channel to proxy IAuthentication
        /// </summary>
        private class BasicChannel : ClientBase<IAuthentication>, IAuthentication
        {
            #region IAuthentication Members
            public void CreateUser(string tenantName, string username, string password, string email)
            {
                base.Channel.CreateUser(tenantName, username, password, email);
            }

            public void DeleteUser(string tenantName, string username)
            {
                base.Channel.DeleteUser(tenantName, username);
            }

            public bool UserExists(string tenantName, string username)
            {
                return base.Channel.UserExists(tenantName, username);
            }

            public bool ValidateUser(string tenantName, string username, string password)
            {
                return base.Channel.ValidateUser(tenantName, username, password);
            }

            public void SetPassword(string tenantName, string username, string password)
            {
                base.Channel.SetPassword(tenantName, username, password);
            }

            public void ChangePassword(string tenantName, string username, string oldPassword, string newPassword)
            {
                base.Channel.ChangePassword(tenantName, username, oldPassword, newPassword);
            }

            public string[] GetUsers(string tenantName)
            {
                return base.Channel.GetUsers(tenantName);
            }

            public string[] FindUsers(string tenantName, string usernameToMatch)
            {
                return base.Channel.FindUsers(tenantName, usernameToMatch);
            }

            public AdamUser GetAdamUser(string tenantName, string username)
            {
                return base.Channel.GetAdamUser(tenantName, username);
            }

            public Guid GetUserIdByName(string tenantName, string username)
            {
                return base.Channel.GetUserIdByName(tenantName, username);
            }

            public string GetUpnByUserId(Guid id)
            {
                return base.Channel.GetUpnByUserId(id);
            }

            #endregion
        }

        #region proxy methods
        static public void CreateUser(string tenantName, string username, string password, string email)
        {
            using(BasicChannel channel= new BasicChannel())
            {
                channel.CreateUser(tenantName, username, password, email);
            }
        }

        static public void DeleteUser(string tenantName, string username)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.DeleteUser(tenantName, username);
            }
        }

        static public bool ValidateUser(string tenantName, string username, string password)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.ValidateUser(tenantName, username, password);
            }
        }

        static public bool UserExists(string tenantName, string username)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.UserExists(tenantName, username);
            }
        }

        static public void SetPassword(string tenantName, string username, string password)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.SetPassword(tenantName, username, password);
            }
        }

        static public void ChangePassword(string tenantName, string username, string oldPassword, string newPassword)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.ChangePassword(tenantName, username, oldPassword, newPassword);
            }
        }

        static public string[] GetUsers(string tenantName)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetUsers(tenantName);
            }
        }

        static public string[] FindUsers(string tenantName, string usernameToMatch)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.FindUsers(tenantName, usernameToMatch);
            }
        }

        static public AdamUser GetAdamUser(string tenantName, string username)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetAdamUser(tenantName, username);
            }
        }

        static public Guid GetUserIdByName(string tenantName, string username)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetUserIdByName(tenantName, username);
            }
        }

        static public string GetUpnByUserId(Guid id)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetUpnByUserId(id);
            }
        }
        #endregion
    }
}
