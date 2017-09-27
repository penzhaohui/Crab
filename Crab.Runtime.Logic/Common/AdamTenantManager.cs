using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.DirectoryServices.Protocols;
using System.Configuration;
using System.Globalization;
using System.DirectoryServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Crab.Runtime.Logic.Common
{
    /// <summary>
    /// The class to manage tenant users on ADAM
    /// </summary>
    public class AdamTenantManager
    {
        /// <summary>
        /// Constants for AdamTenantManager
        /// </summary>
        private class Constants
        {
            internal const string Container = "Container";
            internal const string Group = "Group";
            internal const string User = "User";
            internal const string TenantFilter = "(&(objectClass=container)(cn={0}))";
            internal const string UserFilter = "(&(objectClass=user)(userPrincipalName={0}))";
            internal const string TenantFormat = "cn={0}";
            internal const string UpnFormat = "{0}@{1}";
            internal const string Member = "member";
            internal const string MemberOf = "memberof";
            internal const string UserPrincipalName = "userPrincipalName";
            internal const string Mail = "mail";
            //{{Constants from ADSI
            internal const long ADS_OPTION_PASSWORD_METHOD = 7;
            internal const long ADS_OPTION_PASSWORD_PORTNUMBER = 6;
            internal const int ADS_PASSWORD_ENCODE_CLEAR = 1;
            internal const int ADS_PASSWORD_ENCODE_REQUIRE_SSL = 0;
            //}}Constants from ADSI
            internal static string[] UserProperties
            {
                get
                {
                    return new string[] { "userPrincipalName", "mail" };
                }
            }
        }

        #region private members
        private string _connectionString;
        private string _username;
        private string _password;
        private bool _secureConnection;

        #endregion

        /// <summary>
        /// Default constructor
        /// </summary>
        public AdamTenantManager()
        {
        }

        /// <summary>
        /// Create a Saas.Security.AdamTenantAuthentication object with the specified property values
        /// </summary>
        /// <param name="connectionString">The connection string of the ADAM directory path which points to the tenant parent node</param>
        /// <param name="username">The name of the user having the role of "Administrators" of the ADAM directory.</param>
        /// <param name="password">The password of the user.</param>
        /// <param name="secureConnection">
        /// A Boolean value indicates whether accessing to the ADAM directory needs SSL or not. 
        /// True means use SSL;otherwise, the value is False.
        /// </param>
        public AdamTenantManager(string connectionString, string username, string password, bool secureConnection)
        {
            _connectionString = connectionString;
            _username = username;
            _password = password;
            _secureConnection = secureConnection;
        }

        #region properties
        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        public bool SecureConnection
        {
            get { return _secureConnection; }
            set { _secureConnection = value; }
        }
        #endregion

        /// <summary>
        /// Create DirectoryEntry objects for a new tenant
        /// </summary>
        /// <param name="tenantName">The name of the new tenant</param>
        public void CreateTenant(string tenantName)
        {
            using (DirectoryEntry baseEntry = NewDirectoryEntry())
            {
                using (DirectoryEntry tenantEntry = baseEntry.Children.Add(string.Format(CultureInfo.CurrentCulture, Constants.TenantFormat, tenantName), Constants.Container))
                {
                    tenantEntry.CommitChanges();
                    using (DirectoryEntry usersEntry = tenantEntry.Children.Add("cn=Users", Constants.Container))
                    {
                        usersEntry.CommitChanges();
                    }

                    using (DirectoryEntry rolesEntry = tenantEntry.Children.Add("cn=Roles", Constants.Container))
                    {
                        rolesEntry.CommitChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Check whether the tanant exists.
        /// </summary>
        /// <param name="tenantName"></param>
        /// <returns></returns>
        public bool TenantExists(string tenantName)
        {
            string filter = String.Format(CultureInfo.CurrentCulture, Constants.TenantFilter, tenantName);
            using (DirectoryEntry baseEntry = NewDirectoryEntry())
            {
                using (DirectorySearcher searcher = new DirectorySearcher(baseEntry, filter, null, System.DirectoryServices.SearchScope.OneLevel))
                {
                    SearchResult result = searcher.FindOne();
                    return result != null;
                }
            }
        }

        /// <summary>
        /// Delete a tenant and all the nodes under the tenant node from the directory.
        /// </summary>
        /// <param name="tenantName"></param>
        public void DeleteTenant(string tenantName)
        {
            if (TenantExists(tenantName))
            {
                using (DirectoryEntry tenantEntry = NewDirectoryEntry(GetTenantConnectionString(tenantName)))
                {
                    tenantEntry.DeleteTree();
                }
            }
        }

        #region Membership APIs
        /// <summary>
        /// Method to create a new user node
        /// </summary>
        /// <param name="tenantName"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public void CreateUser(string tenantName, string username, string password, string email)
        {
            string usersConnectionString = GetUsersConnectionString(tenantName);
            using (DirectoryEntry baseEntry = NewDirectoryEntry(usersConnectionString))
            {
                string upn = ToUpn(tenantName, username);
                using (DirectoryEntry userEntry = baseEntry.Children.Add("cn=" + username, Constants.User))
                {
                    try
                    {                        
                        userEntry.Properties[Constants.Mail].Value = email;
                        userEntry.Properties["title"].Value = "project manager";
                        userEntry.CommitChanges();
                    }
                    catch
                    {
                        throw;
                    }

                    try
                    {
                        Uri uri = new Uri(usersConnectionString);
                        int port = uri.Port;
                        userEntry.Invoke("SetOption", new object[] { Constants.ADS_OPTION_PASSWORD_PORTNUMBER, port });
                        if (!_secureConnection)
                            userEntry.Invoke("SetOption", new object[] { Constants.ADS_OPTION_PASSWORD_METHOD, Constants.ADS_PASSWORD_ENCODE_CLEAR });
                        else
                            userEntry.Invoke("SetOption", new object[] { Constants.ADS_OPTION_PASSWORD_METHOD, Constants.ADS_PASSWORD_ENCODE_REQUIRE_SSL });

                        userEntry.Invoke("SetOption", new object[] { Constants.ADS_OPTION_PASSWORD_METHOD, Constants.ADS_PASSWORD_ENCODE_CLEAR });
                        // https://msdn.microsoft.com/en-us/library/ms180915(VS.90).aspx
                        // https://stackoverflow.com/questions/28860601/cannot-set-password-with-directoryentry-invoke-when-user-is-created-in-ad-using  
                        // http://www.morgantechspace.com/2015/01/reset-ad-user-password-with-C-sharp.html
                        //userEntry.Invoke("SetPassword", password);
                        //newUserEntry.Invoke("SetPassword", new object[] { password });
                        userEntry.Invoke("SetPassword", new object[] { password });
                        // userEntry.Invoke("Put", new string[2] { "SetPassword", password });
                        userEntry.Properties[Constants.UserPrincipalName].Value = upn;
                        userEntry.Properties["msDS-UserAccountDisabled"].Value = "FALSE";
                        //newUserEntry.Properties["unicodePwd"].Value = "IgBwAGUAdABlAHIAQAAyADAAMQA3ACIA";
                        userEntry.CommitChanges();
                    }
                    catch (Exception ex)
                    {
                        baseEntry.Children.Remove(userEntry);
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 字符串转Unicode
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>Unicode编码后的字符串</returns>
        internal static string String2Unicode(string source)
        {
            var bytes = Encoding.Unicode.GetBytes(source);
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("\\u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Unicode转字符串
        /// </summary>
        /// <param name="source">经过Unicode编码的字符串</param>
        /// <returns>正常字符串</returns>
        internal static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.Compiled).Replace(source, x => Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)).ToString());
        }

        /// <summary>
        /// Delete a user from ADAM
        /// </summary>
        /// <param name="tenantName">The name of the tenant.</param>
        /// <param name="username">The name of the tenant user.</param>
        public void DeleteUser(string tenantName, string username)
        { 
            if(UserExists(tenantName, username))
            {
                string userConnectionString = GetUserConnectionString(tenantName, username);
                using (DirectoryEntry userEntry = NewDirectoryEntry(userConnectionString))
                {
                    userEntry.DeleteTree();
                }
            }
        }

        /// <summary>
        /// Check whether the user exists
        /// </summary>
        /// <param name="tenantName"></param>
        /// <param name="username"></param>
        /// <returns>true if the user exists;otherwise, return false.</returns>
        public bool UserExists(string tenantName, string username)
        {
            using (DirectoryEntry tenantEntry = NewDirectoryEntry(GetUsersConnectionString(tenantName)))
            {
                string filter = String.Format(CultureInfo.CurrentCulture, Constants.UserFilter, ToUpn(tenantName, username));
                using (DirectorySearcher searcher = new DirectorySearcher(tenantEntry, filter, Constants.UserProperties, System.DirectoryServices.SearchScope.OneLevel))
                {
                    SearchResult result = searcher.FindOne();
                    return result != null;
                }
            }
        }

        /// <summary>
        /// Authenticate the tenant user with tenant name, username and password
        /// </summary>
        /// <param name="tenantName">The name of the tenant owning the user</param>
        /// <param name="username">The name of the tenant user</param>
        /// <param name="password">The password of the tenant user</param>
        /// <returns>true if the user is authenticated succefully;otherwise, false.</returns>
        public bool ValidateUser(string tenantName, string username, string password)
        {
            string connectionString = GetTenantConnectionString(tenantName);
            string upn = ToUpn(tenantName, username);

            using (LdapConnection ldap = CreateNewLdapConnection(connectionString))
            {
                NetworkCredential credentials = new NetworkCredential(upn, password);
                try
                {
                    ldap.Bind(credentials);
                    return true;
                }
                catch (LdapException ex)
                {
                    if (ex.ErrorCode != 0x31)
                    {
                        throw;
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// Set password for the user 
        /// </summary>
        /// <param name="tenantName">The name of the tenant owning the user</param>
        /// <param name="username">The name of the tenant user</param>
        /// <param name="password">The password of the tenant user</param>
        /// <returns>true if the password is setted successfully;otherwise, false.</returns>
        public void SetPassword(string tenantName, string username, string password)
        {
            string userConnectionString = GetUserConnectionString(tenantName, username);
            using(DirectoryEntry userEntry = NewDirectoryEntry(userConnectionString))
            {
                Uri uri = new Uri(userConnectionString);
                int port = uri.Port;
                userEntry.Invoke("SetOption", new object[] { Constants.ADS_OPTION_PASSWORD_PORTNUMBER, port });

                if (!_secureConnection)
                    userEntry.Invoke("SetOption", new object[] { Constants.ADS_OPTION_PASSWORD_METHOD, Constants.ADS_PASSWORD_ENCODE_CLEAR });
                else
                    userEntry.Invoke("SetOption", new object[] { Constants.ADS_OPTION_PASSWORD_METHOD, Constants.ADS_PASSWORD_ENCODE_REQUIRE_SSL });

                userEntry.Invoke("SetOption", new object[] { Constants.ADS_OPTION_PASSWORD_METHOD, Constants.ADS_PASSWORD_ENCODE_CLEAR });
                userEntry.Invoke("SetPassword", new object[] { password });
                userEntry.CommitChanges();
            }
        }

        /// <summary>
        /// Change password for the user.
        /// </summary>
        /// <param name="tenantName">Name of the tenant.</param>
        /// <param name="username">Name of the user.</param>
        /// <param name="oldPassword">Old password of the user.</param>
        /// <param name="newPassword">New password of the user.</param>
        public void ChangePassword(string tenantName, string username, string oldPassword, string newPassword)
        {
            string userConnectionString = GetUserConnectionString(tenantName, username);
            using (DirectoryEntry userEntry = NewDirectoryEntry(userConnectionString))
            {
                Uri uri = new Uri(userConnectionString);
                int port = uri.Port;
                userEntry.Invoke("SetOption", new object[] { Constants.ADS_OPTION_PASSWORD_PORTNUMBER, port });
                if (!_secureConnection)
                    userEntry.Invoke("SetOption", new object[] { Constants.ADS_OPTION_PASSWORD_METHOD, Constants.ADS_PASSWORD_ENCODE_CLEAR });
                else
                    userEntry.Invoke("SetOption", new object[] { Constants.ADS_OPTION_PASSWORD_METHOD, Constants.ADS_PASSWORD_ENCODE_REQUIRE_SSL });

                userEntry.Invoke("SetOption", new object[] { Constants.ADS_OPTION_PASSWORD_METHOD, Constants.ADS_PASSWORD_ENCODE_CLEAR });
                userEntry.Invoke("ChangePassword", new object[] { oldPassword, newPassword});
                userEntry.CommitChanges();
            }
        }

        /// <summary>
        /// Get all the names of the users under a tenant
        /// </summary>
        /// <param name="tenantName">Name of the tenant</param>
        /// <returns></returns>
        public string[] GetUsers(string tenantName)
        {
            List<string> list = new List<string>();
            using (DirectoryEntry entry = NewDirectoryEntry(GetUsersConnectionString(tenantName)))
            {
                foreach (DirectoryEntry child in entry.Children)
                {
                    list.Add(child.Name.Substring(3));
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// Find users by username filter
        /// </summary>
        /// <param name="tenantName">Name of the tenant</param>
        /// <param name="usernameToMatch">Username to match</param>
        /// <returns></returns>
        public string[] FindUsers(string tenantName, string usernameToMatch)
        {
            List<string> list = new List<string>();
            string filter = String.Format(CultureInfo.CurrentCulture, Constants.UserFilter, usernameToMatch);
            using (DirectoryEntry entry = NewDirectoryEntry(GetUsersConnectionString(tenantName)))
            {
                using (DirectorySearcher searcher = new DirectorySearcher(entry, filter, Constants.UserProperties, System.DirectoryServices.SearchScope.Subtree))
                {
                    searcher.Sort = new SortOption(Constants.UserPrincipalName, SortDirection.Ascending);
                    using (SearchResultCollection resultCollection = searcher.FindAll())
                    {
                        foreach (SearchResult result in resultCollection)
                        {
                            DirectoryEntry userEntry = result.GetDirectoryEntry();
                            list.Add(userEntry.Name.Substring(3));
                        }
                    }
                }
            }
            return list.ToArray();
        }

        public bool GetUserInfo(string tenantName, string username, out string email, out byte[] sid)
        {
            email= null;
            sid = null;
            using (DirectoryEntry tenantEntry = NewDirectoryEntry(GetUsersConnectionString(tenantName)))
            {
                string filter = String.Format(CultureInfo.CurrentCulture, Constants.UserFilter, ToUpn(tenantName, username));
                using (DirectorySearcher searcher = new DirectorySearcher(tenantEntry, filter, Constants.UserProperties, System.DirectoryServices.SearchScope.OneLevel))
                {
                    SearchResult result = searcher.FindOne();
                    if (result == null)
                        return false;
                    DirectoryEntry userEntry = result.GetDirectoryEntry();
                    email = (string)userEntry.Properties[Constants.Mail].Value;
                    sid = (byte[])userEntry.Properties["objectSid"].Value;
                    return true;
                }
            }
        }

        #endregion

        #region Roles APIs
        /// <summary>
        /// Create a new role for a tenant
        /// </summary>
        /// <param name="tenantName">The name of the tenant</param>
        /// <param name="roleName">The name of the new role</param>
        public void CreateRole(string tenantName, string roleName)
        {
            using (DirectoryEntry entry = NewDirectoryEntry(GetRolesConnectionString(tenantName)))
            {
                using (DirectoryEntry newEntry = entry.Children.Add("CN=" + roleName, Constants.Group))
                {
                    newEntry.CommitChanges();
                }
            }
        }

        /// <summary>
        /// Delete a role from a tenant permanantly.
        /// </summary>
        /// <param name="tenantName"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public bool DeleteRole(string tenantName, string roleName)
        {
            bool result = false;
            string roleConnectionString = GetRoleConnectionString(tenantName, roleName);
            try
            {
                using (DirectoryEntry roleEntry = NewDirectoryEntry(roleConnectionString))
                {
                    roleEntry.DeleteTree();
                    result = true;
                }
            }
            catch
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// Gets a collection of all the roles of a tenant in ADAM
        /// </summary>
        /// <param name="tenantName">The name of the tenant</param>
        /// <returns>An string[] of roles representing all the tenants in the database.</returns>
        public string[] GetAllRoles(string tenantName)
        {
            List<string> list = new List<string>();
            using (DirectoryEntry entry = NewDirectoryEntry(GetRolesConnectionString(tenantName)))
            {
                foreach (DirectoryEntry child in entry.Children)
                {
                    list.Add(child.Name.Substring(3));
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// Assign a group of roles to a group of users
        /// </summary>
        /// <param name="tenantName">The name of the tenant</param>
        /// <param name="usernames">The array of user names</param>
        /// <param name="roleNames">The array of role names</param>
        public void AddUsersToRoles(string tenantName, string[] usernames, string[] roleNames)
        {
            foreach (string role in roleNames)
            {
                using (DirectoryEntry roleEntry = NewDirectoryEntry(GetRoleConnectionString(tenantName, role)))
                {
                    foreach(string username in usernames)
                    {
                        string userDn = GetDnFromConnectionString(GetUserConnectionString(tenantName, username));
                        roleEntry.Properties[Constants.Member].Add(userDn);
                    }
                    roleEntry.CommitChanges();
                }
            }
        }

        public void RemoveUsersFromRoles(string tenantName, string[] usernames, string[] roleNames)
        {
            foreach (string roleName in roleNames)
            {
                using(DirectoryEntry roleEntry = NewDirectoryEntry(GetRoleConnectionString(tenantName, roleName)))
                {
                    foreach (string username in usernames)
                    {
                        string userDn = GetDnFromConnectionString(GetUserConnectionString(tenantName, username));
                        roleEntry.Properties[Constants.Member].Remove(userDn);
                    }
                    roleEntry.CommitChanges();
                }
            }
        }

        public string[] GetRolesForUser(string tenantName, string username)
        {
            try
            {
                using (DirectoryEntry userEntry = NewDirectoryEntry(GetUserConnectionString(tenantName, username)))
                {
                    List<string> roles = new List<string>();
                    PropertyValueCollection property = userEntry.Properties[Constants.MemberOf];
                    if (property.Value is Array)
                    {
                        Array values = (Array)property.Value;
                        for (int i = 0; i < values.Length; i++)
                        {
                            roles.Add(GetNodeNameFromDn((string)values.GetValue(i)));
                        }
                    }
                    else if (property.Value is string)
                    {
                        roles.Add(GetNodeNameFromDn((string)property.Value));
                    }
                    return roles.ToArray();
                }
            }
            catch
            {
                return new string[0];
            }
        }

        public string[] GetUsersInRole(string tenantName, string roleName)
        {
            string roleConnectionString = GetRoleConnectionString(tenantName, roleName);
            List<string> usernames = new List<string>();
            using (DirectoryEntry roleEntry = NewDirectoryEntry(roleConnectionString))
            {
                PropertyValueCollection property = roleEntry.Properties[Constants.Member];
                if (property.Value is Array)
                {
                    Array values = (Array)property.Value;
                    for (int i = 0; i < values.Length; i++)
                    {
                        usernames.Add(GetNodeNameFromDn((string)values.GetValue(i)));
                    }
                }
                else if (property.Value is string)
                {
                    usernames.Add(GetNodeNameFromDn((string)property.Value));
                }
            }
            return usernames.ToArray();
        }

        public bool RoleExists(string tenantName, string roleName)
        {
            foreach (string role in new List<string>(GetAllRoles(tenantName)))
            {
                if (String.Compare(role, roleName, true, CultureInfo.InvariantCulture) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsUserInRole(string tenantName, string username, string roleName)
        {
            foreach (string role in new List<string>(GetRolesForUser(tenantName, username)))
            {
                if (String.Compare(role, roleName, true, CultureInfo.InvariantCulture) == 0)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Compose the user principle name from tenant name and username
        /// </summary>
        /// <param name="tenantName">The name of the tenant owning the user</param>
        /// <param name="username">The name of the tenant user</param>
        /// <returns></returns>
        public static string ToUpn(string tenantName, string username)
        {
            return string.Format(CultureInfo.CurrentCulture, Constants.UpnFormat, username, tenantName);
        }

        /// <summary>
        /// Try to parse the tenant name and the username from the user principle name.
        /// </summary>
        /// <param name="upn">User principle name.</param>
        /// <param name="tenantName">The name of the tenant owning the user</param>
        /// <param name="username">The name of the tenant user</param>
        /// <returns>true is the format of the user principle name is correct;otherwise, false.</returns>
        public static bool TryParseUpn(string upn, out string tenantName, out string username)
        {
            tenantName = null;
            username = null;
            if(string.IsNullOrEmpty(upn))
                return false;
            int pos = upn.LastIndexOf('@');
            if(pos == -1 || pos == 0|| pos == upn.Length-1)
                return false;
            username = upn.Substring(0, pos);
            tenantName = upn.Substring(pos + 1);
            return true;
        }

        /// <summary>
        /// Create a new LdapConnection object from the connection string
        /// </summary>
        /// <param name="connectionString">Connection string of the LDAP</param>
        /// <returns>An LdapConnection object.</returns>
        public static LdapConnection CreateNewLdapConnection(string connectionString)
        {
            Uri uri = new Uri(connectionString);
            LdapConnection ldap = null;
            ldap = new LdapConnection(new LdapDirectoryIdentifier(uri.Host + ":" + uri.Port));
            ldap.AuthType = AuthType.Basic;
            ldap.SessionOptions.ProtocolVersion = 3;
            return ldap;
        }

        /// <summary>
        /// Gets the LDAP connect string of a sub node based on the parent node connetion string.
        /// </summary>
        /// <param name="parentConnectionString">The connection string of the parent node</param>
        /// <param name="subname">The name of the sub node</param>
        /// <returns>A string representing the connection string of the sub node.</returns>
        private string GetSubConnectionString(string parentConnectionString, string subname)
        {
            int pos = parentConnectionString.LastIndexOf('/');
            if (pos == parentConnectionString.Length - 1)
                return parentConnectionString + subname;
            else
                return parentConnectionString.Substring(0, pos + 1) + subname + "," + parentConnectionString.Substring(pos + 1);
        }

        private string GetTenantConnectionString(string tenantName)
        {
            return GetSubConnectionString(_connectionString, string.Format(CultureInfo.CurrentCulture, Constants.TenantFormat, tenantName));
        }

        private string GetUsersConnectionString(string tenantName)
        {
            return GetSubConnectionString(GetTenantConnectionString(tenantName), "cn=Users");
        }

        private string GetUserConnectionString(string tenantName, string username)
        {
            return GetSubConnectionString(GetTenantConnectionString(tenantName), string.Format(CultureInfo.CurrentCulture, "cn={0},cn=Users", username));
        }

        private string GetRolesConnectionString(string tenantName)
        {
            return GetSubConnectionString(GetTenantConnectionString(tenantName), "cn=Roles");
        }

        private string GetRoleConnectionString(string tenantName, string roleName)
        {
            return GetSubConnectionString(GetTenantConnectionString(tenantName), string.Format(CultureInfo.CurrentCulture, "cn={0},cn=Roles", roleName));
        }

        /// <summary>
        /// Extract Distinguished name from LDAP://hostname:port/cn=cn1,cn=cn2 ...
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private string GetDnFromConnectionString(string connectionString)
        {
            return connectionString.Split(new string[] { "://" }, StringSplitOptions.RemoveEmptyEntries)[1].Split('/')[1];
        }

        private DirectoryEntry NewDirectoryEntry(string connectionString)
        {
            return new DirectoryEntry(connectionString, 
                _username, _password,
                _secureConnection ? AuthenticationTypes.Signing | AuthenticationTypes.Sealing | AuthenticationTypes.Secure 
                : AuthenticationTypes.None);
        }

        private DirectoryEntry NewDirectoryEntry()
        {
            return NewDirectoryEntry(_connectionString);
        }

        private static string GetNodeNameFromDn(string dn)
        {
            return dn.Split(',')[0].Split('=')[1];
        }
        #endregion
    }
}
