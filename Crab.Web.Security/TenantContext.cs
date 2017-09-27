using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;
using System.Web;
using Crab.Runtime.Contract;

namespace Crab.Web.Security
{
    /// <summary>
    /// Summary description for TenantContext
    /// </summary>
    public static class TenantContext
    {
        static private string _tenantName;
        static private string _username;
        static private string _password;

        static public string TenantName
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    Upn upn = new Upn(HttpContext.Current.User.Identity.Name);
                    return upn.TenantName;
                }
                else
                {
                    return _tenantName;   //for none web application
                }
            }
            set
            {
                _tenantName = value;
            }
        }

        static public string Username
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    Upn upn = new Upn(HttpContext.Current.User.Identity.Name);
                    return upn.Username;
                }
                else
                {
                    return _username;   //for none web application
                }
            }
            set
            {
                _username = value;
            }
        }

        static public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
    }
}
