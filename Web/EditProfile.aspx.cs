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

public partial class EditProfile : PageBase
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
            MembershipUser user = null;
            user = string.IsNullOrEmpty(username)?  Membership.GetUser()
                : Membership.GetUser(new Upn(TenantName, username).ToString());
            if(user == null)
            {
                lblErrMsg.Text = string.Format(Resources.GlobalResources.UserNotExists, username);
                return;
            }

            txtUserID.Text = string.IsNullOrEmpty(username) ? base.Username : username;
            GetUserInfo(user.UserName);
        }
    }

    private bool HasPriv()
    {
        string username = Request["username"];
        if (string.IsNullOrEmpty(username))
            return true; 
        return Roles.IsUserInRole(CrabApp.TenantRoles.Administrators.ToString());
    }
    /// <summary>
    /// Get the user infomation by user name
    /// </summary>
    /// <param name="user">user name </param>
    private void GetUserInfo(string user)
    {
        MembershipUser membUser = Membership.GetUser(user, true);
        if (membUser != null)
        {
            txtEmail.Text = membUser.Email;
        }

        ProfileCommon profile = Profile.GetProfile(user);
        txtFullName.Text = profile.FullName;
        txtTitle.Text = profile.Title;
        dplSex.Text = profile.Sex;
        txtBirthday.Text = profile.Birthday;
        txtTel.Text = profile.Telephone;
        txtFax.Text = profile.Fax;
        txtMsn.Text = profile.MsnAccount;
        txtAddress.Text = profile.Address;
        txtZipCode.Text = profile.ZipCode;
        txtMobile.Text = profile.Mobile;
    }
    /// <summary>
    /// Bind repeater and check the user in role
    /// </summary>
    /// <param name="rp"></param>
    /// <param name="user"></param>
    private void BindRepeater(Repeater rp, string user)
    {
        string[] RolesGroup = Roles.GetAllRoles();

        rp.DataSource = RolesGroup;
        rp.DataBind();

        for (int i = 0; i < rp.Items.Count; i++)
        {
            CheckBox ck = (CheckBox)rp.Items[i].FindControl("chbRoles");
            bool IsChked = Roles.IsUserInRole(user, ck.Text.ToString());
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
            string tenantUserName = new Upn(CurrentTenant.Name, txtUserID.Text.Trim()).ToString();
            MembershipUser user = Membership.GetUser(tenantUserName);
            if (user.Email != txtEmail.Text.Trim())
            {
                user.Email = txtEmail.Text.Trim();
                Membership.UpdateUser(user);
            }

            ProfileCommon commProfile = Profile.GetProfile(tenantUserName);
            commProfile.FullName = txtFullName.Text.Trim();
            commProfile.Title = txtTitle.Text.Trim();
            commProfile.Sex = dplSex.Text.Trim();
            commProfile.Birthday = txtBirthday.Text.Trim();
            commProfile.Telephone = txtTel.Text.Trim();
            commProfile.Fax = txtFax.Text.Trim();
            commProfile.MsnAccount = txtMsn.Text.Trim();
            commProfile.Address = txtAddress.Text.Trim();
            commProfile.ZipCode = txtZipCode.Text.Trim();
            commProfile.Mobile = txtMobile.Text.Trim();
            commProfile.Save();
            lblErrMsg.Text = Resources.GlobalResources.SuccessSave ;
        }
        catch (Exception)
        {

            lblErrMsg.Text = Resources.GlobalResources.FailSave;
        }
    }

    protected void chbRoles_CheckedChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < RepeaterRoles.Items.Count; i++)
        {
            CheckBox cb = (CheckBox)RepeaterRoles.Items[i].FindControl("chbRoles");

            if (cb.Checked)
            {
                bool isInRole = Roles.IsUserInRole(txtUserID.Text.Trim(), cb.Text);
                if (!isInRole)
                {
                    Roles.AddUserToRole(txtUserID.Text.Trim(), cb.Text);
                }
            }
            else
            {
                bool isInRole = Roles.IsUserInRole(txtUserID.Text.Trim(), cb.Text);
                if (isInRole)
                {
                    Roles.RemoveUserFromRole(txtUserID.Text.Trim(), cb.Text);
                }
            }
        }
    }
}
