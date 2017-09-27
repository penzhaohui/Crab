using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Profile;
using Crab.Runtime.Contract;
using Crab.Services.Proxy;
/// <summary>
/// Summary description for PageBase
/// </summary>
public class PageBase : System.Web.UI.Page
{
    private static class Constants
    {
        internal const string TenantId = "http://www.microsoft.com/architecture/saas/security/tenantId";
        internal const string TenantName = "http://www.microsoft.com/architecture/saas/security/tenantName";
        internal const string UserId = "http://www.microsoft.com/architecture/saas/security/userId";
        internal const string Username = "http://www.microsoft.com/architecture/saas/security/username";
        internal const string Tenant = "http://www.microsoft.com/architecture/saas/security/Tenant";
    }


    protected override void OnPreInit(EventArgs e)
    {
        //Set theme
        InitializeTheme();
        base.OnPreInit(e);
    }

    protected override void OnUnload(EventArgs e)
    {
        base.OnUnload(e);
    }

    public string TenantName
    {
        get 
        {
            if (Session[Constants.TenantName] == null)
            {
                Upn upn = new Upn(HttpContext.Current.User.Identity.Name);
                Session[Constants.TenantName] = upn.TenantName;
            }
            return (string)Session[Constants.TenantName];
        }
    }

    public Guid TenantId
    {
        get
        {
            if (Session[Constants.TenantId]==null)
            {
                Session[Constants.TenantId] = CurrentTenant.Id;
            }
            return (Guid)Session[Constants.TenantId];
        }
    }

    public Guid UserId
    {
        get
        {
            if (Session[Constants.UserId] == null)
            {
                Session[Constants.UserId] = AuthenticationProxy.GetUserIdByName(TenantName, Username);
            }
            return Session[Constants.UserId]!=null?(Guid)Session[Constants.UserId]:Guid.Empty;
        }
    }

    public string Username
    {
        get 
        {
            Upn upn = new Upn(HttpContext.Current.User.Identity.Name);
            return upn.Username;
        }
    }

    public Tenant CurrentTenant
    {
        get 
        {
            if (HttpContext.Current.Items[Constants.Tenant] == null)
                HttpContext.Current.Items[Constants.Tenant] = ProvisionProxy.GetTenantByName(TenantName);
            return HttpContext.Current.Items[Constants.Tenant] as Tenant;
        }
    }


    public void Alert(string message)
    {
        Response.Write("<script>alert('"+message+"')</script>");
    }

    private void InitializeTheme()
    {
        ProfileCommon profile = HttpContext.Current.Profile as ProfileCommon;
        if (!string.IsNullOrEmpty(profile.Theme)&&string.Compare(profile.Theme, Page.Theme, true)!=0)
        {
            Page.Theme = profile.Theme;
        }
    }

    protected void ChangeTheme(string themeName)
    {
        ProfileCommon profile = HttpContext.Current.Profile as ProfileCommon;
        if (string.Compare(themeName, Page.Theme, true) != 0)
        {
            profile.Theme = themeName;
            profile.Save();
            Server.Transfer(HttpContext.Current.Request.RawUrl);
        }
    }
}
