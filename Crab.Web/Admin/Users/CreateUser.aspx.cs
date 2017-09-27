using Crab.Runtime.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Users_CreateUser : PageBase
{
    CheckBoxList cbl;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //roles = Roles.GetAllRoles();

            cbl = ((CheckBoxList)this.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cblBoundRoles"));
            BindRoles(cbl);
        }
    }


    private void BindRoles(CheckBoxList cbl)
    {
        string[] roles = Roles.GetAllRoles();
        IList<string> newRoles = new List<string>();
        foreach (string role in roles)
        {
            //if (role.ToLower() != Constants.Roles.SysAdmin.ToString().ToLower())
            {
                newRoles.Add(role);
            }
        }
        cbl.DataSource = newRoles;
        cbl.DataBind();
    }


    /// <summary>
    /// go to update the info of profile
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ConProfile_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/EditProfile.aspx?username=" + CreateUserWizard1.UserName, true);
    }
    protected void ConUsers_Click(object sender, EventArgs e)
    {
        CreateUserWizard1.ActiveStepIndex = 0;
    }
    protected void btnConUser_Click(object sender, EventArgs e)
    {
        CreateUserWizard1.ActiveStepIndex = 0;
    }

    protected void btnCreateUser_Click(object sender, EventArgs e)
    {
        try
        {
            List<string> roles = new List<string>();
            cbl = ((CheckBoxList)this.CreateUserWizardStep1.ContentTemplateContainer.FindControl("cblBoundRoles"));
            bool roleSelected = false;
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Selected)
                {
                    roles.Add(cbl.SelectedValue.ToString());
                    roleSelected = true;
                }
            }

            lblReturnMessage.Text = string.Empty;

            if (Membership.GetAllUsers().Count >= CurrentTenant.LicenseCount)
            {
                lblReturnMessage.Text = Resources.GlobalResources.OutOfUserLicense;
                return;
            }

            Upn upn = new Upn(TenantName, CreateUserWizard1.UserName.Trim());
            MembershipCreateStatus createStatus;
            MembershipUser user = Membership.CreateUser(upn.ToString(), CreateUserWizard1.Password, CreateUserWizard1.Email,
                null, null, true, out createStatus);

            if (user == null)
            {
                switch (createStatus)
                {
                    case MembershipCreateStatus.DuplicateUserName:
                        this.lblReturnMessage.Text = string.Format(Resources.GlobalResources.ExistsUser, CreateUserWizard1.UserName);
                        break;

                    case MembershipCreateStatus.InvalidUserName:
                        this.lblReturnMessage.Text = Resources.GlobalResources.InvalidUsername;
                        break;
                    default:
                        lblReturnMessage.Text = Resources.GlobalResources.FailCreateUser;
                        break;
                }
                return;
            }
            CreateUserWizard1.ActiveStepIndex = 1;

        }
        catch (Exception ex)
        {

            lblReturnMessage.Text = ex.Message;
        }

    }

    protected void btnManageUsers_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Admin/Users/ManageUsers.aspx");
    }
}