using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Crab.Runtime.Contract;

public partial class Logon : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
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
    }
}
