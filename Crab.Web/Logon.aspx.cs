using Crab.Runtime.Contract;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;

public partial class Logon : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //PerformRememberInfo();

            // http://codeverge.com/asp.net.web-forms/asp-login-remember-me-functionality/390002
            if (Request.Cookies["myCookie"] != null)
            {
                // TextBox ctrUserName = ((TextBox)Login1.FindControl("UserName"));
                TextBox ctrTenantName = ((TextBox)Login1.FindControl("TenantName"));

                HttpCookie cookie = Request.Cookies.Get("myCookie");
                Login1.UserName = cookie.Values["username"];
                ctrTenantName.Text = cookie.Values["tenantname"];

                Login1.RememberMeSet = (!String.IsNullOrEmpty(Login1.UserName));
            }
            
            TextBox ctrPassword = Login1.FindControl("Password") as TextBox;
            if (ctrPassword != null)
                this.SetFocus(ctrPassword);           
        }

        // Note this
        Response.Cache.SetNoStore();
    }

    protected void Login1_Authenticate(object sender, AuthenticateEventArgs e)
    {
        string tenantName = ((TextBox)Login1.FindControl("TenantName")).Text.Trim();
        string tenantUsername = Login1.UserName.Trim();
        Upn upn = new Upn(tenantName, tenantUsername);

        bool authenticated = false;
        if (!(authenticated = Membership.ValidateUser(upn.ToString(), Login1.Password)))
        {
            Login1.FailureText = Resources.GlobalResources.FailLogin;
            return;
        }

        Login1.UserName = upn.ToString();
        e.Authenticated = authenticated;

        // http://codeverge.com/asp.net.web-forms/asp-login-remember-me-functionality/390002
        HttpCookie myCookie = new HttpCookie("myCookie");
        Boolean remember = Login1.RememberMeSet;
        if (remember)
        {
            Int32 persistDays = 15;
            myCookie.Values.Add("username", tenantUsername);
            myCookie.Values.Add("tenantname", tenantName);
            myCookie.Expires = DateTime.Now.AddDays(persistDays); //you can add years and months too here
        }
        else
        {
            myCookie.Values.Add("username", string.Empty); // overwrite empty string is safest
            myCookie.Values.Add("tenantname", string.Empty);
            myCookie.Expires = DateTime.Now.AddMinutes(5); //you can add years and months too here
        }
        Response.Cookies.Add(myCookie);

        /*
        if (User.Identity.IsAuthenticated)
        {
            FormsIdentity identity = User.Identity as FormsIdentity;
            FormsAuthenticationTicket ticket = identity.Ticket;
            if (ticket.IsPersistent)
            {
                Login1.RememberMeSet = true;
            }
        }
        */

        //RememberMe();
    }

    private void RememberMe()
    {
        bool rememberMe = ((CheckBox)Login1.FindControl("RememberMe")).Checked;

        // https://blogs.msdn.microsoft.com/friis/2010/08/18/remember-me-checkbox-does-not-work-with-forms-authentication/
        // https://www.codeproject.com/Articles/31914/Beginner-s-Guide-To-ASP-NET-Cookies
        // 浅析ASP.NET 2.0的用户密码加密机制  - http://www.cnblogs.com/AndersLiu/archive/2007/12/28/encode-password-with-salt.html
        // 使用Forms Authentication实现用户注册、登录 （一）基础知识 - http://www.cnblogs.com/AndersLiu/archive/2008/01/01/forms-authentication-part-1.html
        // 使用Forms Authentication实现用户注册、登录 （二）用户注册与登录 - http://www.cnblogs.com/AndersLiu/archive/2008/01/01/forms-authentication-part-2.html
        // 使用Forms Authentication实现用户注册、登录 （三）用户实体替换 - http://www.cnblogs.com/AndersLiu/archive/2008/01/01/forms-authentication-part-3.html
        // Roles-Based Authentication - https://www.codeproject.com/KB/web-security/rolesbasedauthentication.aspx?print=true
        // https://www.codeproject.com/Articles/779844/Remember-Me
        // 细说ASP.NET Forms身份认证 - http://www.cnblogs.com/fish-li/archive/2012/04/15/2450571.html

        //treat the case where we set the remember me check box
        if (rememberMe)
        {
            //clear any other tickets that are already in the response
            Response.Cookies.Clear();

            //set the new expiry date - to thirty days from now
            DateTime expiryDate = DateTime.Now.AddDays(30);

            //create a new forms auth ticket
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(2, Login1.UserName, DateTime.Now, expiryDate, true, String.Empty);

            //encrypt the ticket
            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            //create a new authentication cookie - and set its expiration date
            HttpCookie authenticationCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            authenticationCookie.Expires = ticket.Expiration;

            //add the cookie to the response.
            Response.Cookies.Add(authenticationCookie);

            HttpCookie cookie = new HttpCookie("Crab_Remember_User_Name", Login1.UserName);
            bool isSSL = "HTTPS".Equals(HttpContext.Current.Request.Url.Scheme, StringComparison.OrdinalIgnoreCase);
            //if Secure is true,only https can access it.
            cookie.Secure = isSSL;
            //Prevent java script access it.
            cookie.HttpOnly = true;
            TimeSpan cookiesExistTime = new TimeSpan(2, 0, 0, 0);
            cookie.Expires = DateTime.Now + cookiesExistTime;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }

    /// <summary>
    /// when user check remember me and click login button the user id will be recorded in cookie.
    /// </summary>
    private void PerformRememberInfo()
    {
        TextBox ctrUserName = ((TextBox)Login1.FindControl("UserName"));
        TextBox ctrPassword = ((TextBox)Login1.FindControl("Password"));
        TextBox ctrTenantName = ((TextBox)Login1.FindControl("TenantName"));
        CheckBox ctrRememberMe = ((CheckBox)Login1.FindControl("RememberMe"));

        HttpCookie cookie = Context.Request.Cookies.Get("Crab_Remember_User_Name");

        if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
        {
            try
            {
                ctrUserName.Text = cookie.Value;
                this.SetFocus(ctrPassword.ClientID);
                ctrRememberMe.Checked = true;
            }
            catch
            {
                ctrUserName.Text = string.Empty;

                //let the current cookie expire if its user name is failed to decoded
                bool isSSL = "HTTPS".Equals(HttpContext.Current.Request.Url.Scheme, StringComparison.OrdinalIgnoreCase);
                //if Secure is true,only https can access it.
                cookie.Secure = isSSL;
                //Prevent java script access it.
                cookie.HttpOnly = true;
                cookie.Expires = DateTime.Now.AddDays(-1);
                Context.Response.Cookies.Add(cookie);

                this.SetFocus(ctrUserName.ClientID);
            }
        }
        else
        {
            this.SetFocus(ctrUserName.ClientID);
        }
    }
}