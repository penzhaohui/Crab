using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Crab.Runtime.Contract;
using Crab.Runtime.Contract.Exceptions;
using Crab.Runtime.Logic.Common;

namespace Crab.Runtime.Logic
{
    public class AuthenticationLogic
    {
        private const string SQL_GET_USER_ID_FROM_DB = @"SELECT Id FROM TenantUser WHERE LOWER(Upn) = LOWER(@Upn)";
        private const string SQL_CHECK_EXISTENCE = @"SELECT COUNT(*) FROM TenantUser WHERE LOWER(Upn) = LOWER(@Upn)";
        private const string SQL_CREATE_USER_IN_DB = @"INSERT INTO TenantUser(Id, TenantId, Upn) VALUES(@Id, @TenantId, @Upn)";
        private const string SQL_GET_UPN = @"SELECT Upn FROM TenantUser WHERE Id = @Id";

        static public void CreateUser(string tenantName, string username, string password, string email)
        {
            try
            {
                AdamTenantManager adamManager = AdamConfigurationHelper.AdamManager;
                if(adamManager.UserExists(tenantName, username))
                    throw new AuthenticationException(string.Format("The user with name {0} already exists!", username));
                adamManager.CreateUser(tenantName, username, password, email);
            }
            catch(Exception ex)
            {
                throw new AuthenticationException(ex.Message, ex);
            }
        }

        static public void DeleteUser(string tenantName, string username)
        {
            AdamConfigurationHelper.AdamManager.DeleteUser(tenantName, username);
    
        }

        static public bool UserExists(string tenantName, string username)
        {
            return AdamConfigurationHelper.AdamManager.UserExists(tenantName, username);
        }

        static public bool ValidateUser(string tenantName, string username, string password)
        {
            bool valid = AdamConfigurationHelper.AdamManager.ValidateUser(tenantName, username, password);
            if (!valid)
                return false;
            if (!UserExistsInDb(tenantName, username))  //synchronize adam user to db
            {
                CreateUserInDb(tenantName, username);
            }
            return valid;
        }

        static public void SetPassword(string tenantName, string username, string password)
        {
            AdamConfigurationHelper.AdamManager.SetPassword(tenantName, username, password);
        }

        static public void ChangePassword(string tenantName, string username, string oldPassword, string newPassword)
        {
            AdamConfigurationHelper.AdamManager.ChangePassword(tenantName, username, oldPassword, newPassword);
        }

        static public string[] GetUsers(string tenantName)
        {
            return AdamConfigurationHelper.AdamManager.GetUsers(tenantName);
        }

        static public string[] FindUsers(string tenantName, string usernameToMatch)
        {
            return AdamConfigurationHelper.AdamManager.FindUsers(tenantName, usernameToMatch);
        }

        static public AdamUser GetAdamUser(string tenantName, string username)
        {
            string email;
            byte[] sid;
            if (AdamConfigurationHelper.AdamManager.GetUserInfo(tenantName, username, out email, out sid))
            {
                AdamUser user = new AdamUser();
                user.PrincipleName = (new Upn(tenantName, username)).ToString();
                user.Email = email;
                user.Sid = sid;
                return user;
            }
            return null;
        }

        static public Guid GetUserIdByName(string tenantName, string username)
        {
            return GetUserIdFromDb(tenantName, username);
        }

        static public string GetUpnByUserId(Guid id)
        {
            Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantIdentity);
            using (DbCommand command = db.GetSqlStringCommand(SQL_GET_UPN))
            {
                db.AddInParameter(command, "Id", DbType.Guid, id);
                object result = db.ExecuteScalar(command);
                if (result == null || result is DBNull)
                    return null;
                else
                    return (string)result;
            }
        }

        static private void CreateUserInDb(string tenantName, string username)
        {
            Tenant tenant = ProvisionLogic.GetTenantByName(tenantName);
            Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantIdentity);
            using (DbCommand command = db.GetSqlStringCommand(SQL_CREATE_USER_IN_DB))
            {
                Upn upn = new Upn(tenantName, username);
                db.AddInParameter(command, "Id", DbType.Guid, Guid.NewGuid());
                db.AddInParameter(command, "TenantId", DbType.Guid, tenant.Id);
                db.AddInParameter(command, "Upn", DbType.String, upn.ToString().ToLower());
                db.ExecuteNonQuery(command);
            }
        }

        static private Guid GetUserIdFromDb(string tenantName, string username)
        {
            Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantIdentity);
            using (DbCommand command = db.GetSqlStringCommand(SQL_GET_USER_ID_FROM_DB))
            {
                Upn upn = new Upn(tenantName, username);
                db.AddInParameter(command, "Upn", DbType.String, upn.ToString());
                object result = db.ExecuteScalar(command);
                if (result == null||result is DBNull)
                    return Guid.Empty;
                else
                    return (Guid)result;
            }
        }

        static private bool UserExistsInDb(string tenantName, string username)
        {
            Database db = DatabaseFactory.CreateDatabase(Constants.Database.TenantIdentity);
            using (DbCommand command = db.GetSqlStringCommand(SQL_CHECK_EXISTENCE))
            {
                Upn upn = new Upn(tenantName, username);
                db.AddInParameter(command, "Upn", DbType.String, upn.ToString());
                return (int)db.ExecuteScalar(command) > 0;
            }
        }

    }
}
