using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Util;
using System.Web.Security;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Configuration.Provider;
using System.Reflection;
using System.Security.Principal;
using Crab.Services.Proxy;
using Crab.Runtime.Contract;

namespace Crab.Web.Security
{
    /// <summary>
    /// Manages storage of tenant membership information for an ASP.NET application in
    ///     Active Directory Application Mode service.
    /// </summary>
    public class WCFTenantMembershipProvider : MembershipProvider
    {
        private string _applicationName = "/";

        public WCFTenantMembershipProvider()
        {
        }

        public override string ApplicationName
        {
            get
            {
                return this._applicationName;
            }
            set
            {
                this._applicationName = value;
            }
        }

        public override bool EnablePasswordReset
        {
            get { return WCFMembershipConfigurationManager.EnablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw  new NotSupportedException();}
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return WCFMembershipConfigurationManager.MinRequiredNonAlphanumericCharacters; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return WCFMembershipConfigurationManager.MinRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return WCFMembershipConfigurationManager.PasswordStrengthRegularExpression; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotSupportedException(); }
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotSupportedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotSupportedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            status = MembershipCreateStatus.Success;
            string tenantName;
            string tenantUsername;

            if(string.IsNullOrEmpty(username))
            {
                status = MembershipCreateStatus.InvalidUserName;
                return null;
            }

            if(!Upn.TryParse(username, out tenantName, out tenantUsername))
            {
                status = MembershipCreateStatus.InvalidUserName;
                return null;
            }

            if (string.IsNullOrEmpty(email))
            {
                status = MembershipCreateStatus.InvalidEmail;
                return null;
            }

            if (string.IsNullOrEmpty(password) || password.Length < MinRequiredPasswordLength)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            int i = 0;
            for (int j = 0; j < password.Length; j++)
            {
                if (!char.IsLetterOrDigit(password, j))
                {
                    i++;
                }
            }
            if (i < MinRequiredNonAlphanumericCharacters)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            if ((PasswordStrengthRegularExpression.Length > 0) && !Regex.IsMatch(password, PasswordStrengthRegularExpression))
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            ValidatePasswordEventArgs args = new ValidatePasswordEventArgs(username, password, true);
            base.OnValidatingPassword(args);
            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            try
            {
                if (AuthenticationProxy.UserExists(tenantName, tenantUsername))
                {
                    status = MembershipCreateStatus.DuplicateUserName;
                    return null;
                }
                AuthenticationProxy.CreateUser(tenantName, tenantUsername, password, email);
            }
            catch (Exception ex)
            {
                throw new ProviderException(ex.Message, ex);
            }
            return GetUser(username, false);
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            string tenantName;
            string tenantUsername;

            if (!Upn.TryParse(username, out tenantName, out tenantUsername))
            {
                throw new ArgumentException(string.Format("The username {0} is invalid!", username));
            }
            AdamUser adamUser = AuthenticationProxy.GetAdamUser(tenantName, tenantUsername);
            if (adamUser == null)
                return null;
            object providerUserKey = new SecurityIdentifier(adamUser.Sid, 0);

            return new ActiveDirectoryMembershipUser(this.Name, adamUser.PrincipleName, providerUserKey, adamUser.Email, string.Empty, string.Empty
                , true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now);
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            string tenantName;
            string tenantUsername;
            if (!Upn.TryParse(username, out tenantName, out tenantUsername))
                return false;
            return AuthenticationProxy.ValidateUser(tenantName, tenantUsername, password);
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username");
            }
            string tenantName;
            string tenantUsername;
            if (!Upn.TryParse(username, out tenantName, out tenantUsername))
                return false;  //invalid username format
            try
            {
                if (!AuthenticationProxy.UserExists(tenantName, tenantUsername))
                    return false; //not exists
                AuthenticationProxy.DeleteUser(tenantName, tenantUsername);
            }
            catch(Exception ex)
            {
                throw new ProviderException(ex.Message, ex);
            }
            return true;
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username");
            }
            string tenantName;
            string tenantUsername;
            if (!Upn.TryParse(username, out tenantName, out tenantUsername))
                return false;
            CheckPassword(oldPassword, MinRequiredPasswordLength, "oldPassword");
            CheckPassword(oldPassword, MinRequiredPasswordLength, "newPassword");
            if (newPassword.Length < this.MinRequiredPasswordLength)
            {
                throw new ArgumentException(string.Format("The length of the passowrd {0} must not be shorter than {1} characters!", "newPassword", MinRequiredPasswordLength));
            }
            int num1 = 0;
            for (int num2 = 0; num2 < newPassword.Length; num2++)
            {
                if (!char.IsLetterOrDigit(newPassword, num2))
                {
                    num1++;
                }
            }
            if (num1 < this.MinRequiredNonAlphanumericCharacters)
            {
                throw new ArgumentException(string.Format("Non alpha numeric characters of {0} must be more than {1}", "newPassword", MinRequiredNonAlphanumericCharacters));
            }
            if ((this.PasswordStrengthRegularExpression.Length > 0) && !Regex.IsMatch(newPassword, this.PasswordStrengthRegularExpression))
            {
                throw new ArgumentException(string.Format("The format of the password {0} is not correct!", "newPassword"));
            }

            AuthenticationProxy.ChangePassword(tenantName, tenantUsername, oldPassword, newPassword);
            return true;
        }

        protected virtual void CheckPassword(string password, int minSize, string paramName)
        {
            if (password == null)
            {
                throw new ArgumentNullException(paramName);
            }

            if (password.Trim().Length < 1)
            {
                throw new ArgumentException(string.Format("The {0} can not be empty", paramName), paramName);
            }

            if ((minSize > 0) && (password.Length < minSize))
            {
                throw new ArgumentException(string.Format("The length of the passowrd {0} must not be shorter than {1} characters!", paramName, minSize.ToString()), paramName);
            }
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            //extract tenant name from the current user upn
            string tenantName;
            string username;
            Upn.TryParse(HttpContext.Current.User.Identity.Name, out tenantName, out username);

            MembershipUserCollection collection = new MembershipUserCollection();
            string[] usernames = AuthenticationProxy.FindUsers(tenantName, usernameToMatch);
            totalRecords = usernames.Length;
            int start = pageIndex * pageSize;
            if (start >= 0 && start < totalRecords)
            {
                for(int i=start; i<totalRecords&&i<start+pageSize; i++)
                {
                    collection.Add(System.Web.Security.Membership.GetUser((new Upn(tenantName, usernames[i])).ToString()));
                }
            }
            return collection;
        }


        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            //extract tenant name from the current user upn
            string tenantName;
            string username;
            Upn.TryParse(HttpContext.Current.User.Identity.Name, out tenantName, out username);

            MembershipUserCollection collection = new MembershipUserCollection();
            string[] usernames = AuthenticationProxy.GetUsers(tenantName);
            totalRecords = usernames.Length;
            int start = pageIndex * pageSize;
            if (start >= 0 && start < totalRecords)
            {
                for (int i = start; i < totalRecords && i < start + pageSize; i++)
                {
                    collection.Add(System.Web.Security.Membership.GetUser((new Upn(tenantName, usernames[i])).ToString()));
                }
            }
            return collection;
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }
    }
}
