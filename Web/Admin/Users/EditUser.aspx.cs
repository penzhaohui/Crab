using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Crab.Runtime.Contract;

public partial class Admin_Users_EditUser : PageBase 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!HasPriv())
        {
            lblErrMsg.Text = Resources.GlobalResources.NoPrivilege;
            return;
        }

        if (!IsPostBack)
        {
            string username = Request["username"];
            string returnUrl = Request["ReturnUrl"];
            ViewState["UserName"] = username;
            ViewState["ReturnUrl"] = returnUrl;
            MembershipUser user = null;
            user = string.IsNullOrEmpty(username) ? Membership.GetUser() 
                : Membership.GetUser((new Upn(base.TenantName, username)).ToString());
            if (user == null)
            {
                lblErrMsg.Text = string.Format(Resources.GlobalResources.UserNotExists, username);
                return;
            }

            Upn upn = new Upn(user.UserName);
            txtUserID.Text = upn.Username;
            GetUserInfo(user.UserName);
            BindRepeater(RepeaterRoles, user.UserName);
        }
    }

    private bool HasPriv()
    {
        string username = Request["username"];
        if (string.IsNullOrEmpty(username))
            return true; //edit the user himself
        return Roles.IsUserInRole(CrabApp.TenantRoles.Administrators.ToString());
    }
    /// <summary>
    /// Get the user infomation by user name
    /// </summary>
    /// <param name="user">user name </param>
    private void GetUserInfo(string user)
    {
        MembershipUser membUser = Membership.GetUser(user, true);
        txtEmail.Text = membUser.Email;
    }
    /// <summary>
    /// Bind repeater and check the user in role
    /// </summary>
    /// <param name="rp"></param>
    /// <param name="user"></param>
    private void BindRepeater(Repeater rp, string username)
    {
        string[] roles = Roles.GetAllRoles();
        IList<string> newRoles = new List<string>();
        Upn upn = new Upn(username);
        foreach (string role in roles)
        {
            if (role.ToLower() == CrabApp.TenantRoles.SysAdmin.ToString().ToLower())
                continue;
            else if (role.ToLower() == CrabApp.TenantRoles.Administrators.ToString().ToLower()
                && upn.Username.ToLower() == base.Username.ToLower())
                continue;
            newRoles.Add(role);
        }

        rp.DataSource = newRoles;
        rp.DataBind();
        for (int i = 0; i < rp.Items.Count; i++)
        {
            CheckBox ck = (CheckBox)rp.Items[i].FindControl("chbRoles");
            bool IsChked = Roles.IsUserInRole(username, ck.Text.ToString());
            if (IsChked)
            {
                ck.Checked = true;
            }
        }
    }
    protected void btnSaveButton_ServerClick(object sender, EventArgs e)
    {
        if (!HasPriv())
            return;
        try
        {
            string username = Request["username"];
            MembershipUser user = Membership.GetUser(new Upn(TenantName, username).ToString());
            if (user.Email != txtEmail.Text.Trim())
            {
                user.Email = txtEmail.Text.Trim();
                Membership.UpdateUser(user);
            }
            lblErrMsg.Text = Resources.GlobalResources.SuccessSave;
        }
        catch (Exception)
        {

            lblErrMsg.Text = Resources.GlobalResources.FailSave;
        }
    }

    protected void chbRoles_CheckedChanged(object sender, EventArgs e)
    {
        string membershipUserName = new Upn(TenantName, Request["username"]).ToString();
        if (string.IsNullOrEmpty(membershipUserName))
            membershipUserName = Membership.GetUser().UserName;
        CheckBox chkBox = sender as CheckBox;
        string roleName = chkBox.Text;
        if (chkBox.Checked)
        {
            if (!Roles.IsUserInRole(membershipUserName, roleName))
                Roles.AddUserToRole(membershipUserName, roleName);
        }
        else
        {
            if (Roles.IsUserInRole(membershipUserName, roleName))
                Roles.RemoveUserFromRole(membershipUserName, roleName);
        }
    }

    protected  void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(ViewState["ReturnUrl"].ToString());
    }

    protected void btnEditProfile_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/EditProfile.aspx?username=" + Request["username"]);
    }
}
