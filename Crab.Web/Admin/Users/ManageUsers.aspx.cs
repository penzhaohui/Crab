using Crab.Runtime.Contract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Users_ManageUsers : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblErrMsg.Text = string.Empty;
        if (!IsPostBack)
        {
            BindRepeater(this.repeaterLetters);
            BindGrid(this.ManagerUsers, 0);
        }
    }

    #region pager
    protected int PageIndex
    {
        get { return Convert.ToInt32(ViewState["CurrentIndex"]); }
        set { ViewState["CurrentIndex"] = value; }
    }

    protected int PageCount
    {
        get { return Convert.ToInt32(ViewState["PageCount"]); }
        set { ViewState["PageCount"] = value; }
    }

    protected int PageSize
    {
        get { return int.Parse(Resources.GlobalResources.PageSize); }
    }
    #endregion

    private SearchUserMethod SearchMethod
    {
        get
        {
            if (ViewState["SearchUserMethod"] != null)
                return (SearchUserMethod)ViewState["SearchUserMethod"];
            else
                return SearchUserMethod.ByUsername;
        }
        set
        {
            ViewState["SearchUserMethod"] = value;
        }
    }

    private string SearchKey
    {
        get { return (string)ViewState["SearchKey"]; }
        set { ViewState["SearchKey"] = value; }
    }

    /// <summary>
    /// show all users when load page first time
    /// </summary>
    private void BindGrid(GridView gv, int pageIndex)
    {
        if (SearchMethod == SearchUserMethod.ByUsername)
        {
            int totalRecords = 0;
            Guid tenantId = CurrentTenant.Id;
            MembershipUserCollection users = null;
            if (string.IsNullOrEmpty(SearchKey))
                users = Membership.GetAllUsers(pageIndex, PageSize, out totalRecords);
            else
                users = Membership.FindUsersByName(SearchKey, pageIndex, PageSize, out totalRecords);
            if (totalRecords == 0)
            {
                PageIndex = PageCount = 0;
            }
            else
            {
                PageIndex = pageIndex;
                PageCount = totalRecords / PageSize;
                if (totalRecords % PageSize != 0)
                    PageCount++;
            }
            lbtnFirst.Visible = lbtnNext.Visible = lbtnPre.Visible = lbtnLast.Visible = false;
            if (PageCount > PageIndex + 1)
                lbtnNext.Visible = lbtnLast.Visible = true;
            if (PageIndex > 0)
                lbtnFirst.Visible = lbtnPre.Visible = true;
            ManagerUsers.DataSource = users;
            ManagerUsers.DataBind();
        }
        else
        {
        }
    }

    /// <summary>
    /// show alphabet a-z and all
    /// </summary>
    /// <param name="repeater"></param>
    private void BindRepeater(Repeater repeater)
    {
        // display alphabet row only if language is has Alphabet resource
        ArrayList arr = new ArrayList();

        String chars = Resources.GlobalResources.Alphabet;
        foreach (String s in chars.Split(';'))
        {
            arr.Add(s);
        }
        if (arr.Count == 0)
        {
            repeater.Visible = false;
        }
        else
        {
            arr.Add(Resources.GlobalResources.All);
            repeater.DataSource = arr;
            repeater.Visible = true;
            repeater.DataBind();
        }
    }
    /// <summary>
    /// bind all roles to the view
    /// </summary>
    /// <param name="gv"></param>
    private void BindGvRole(GridView gv)
    {
        string[] roles = Roles.GetAllRoles();
        IList<string> newRoles = new List<string>();
        foreach (string role in roles)
        {
            if (role.ToLower() == CrabApp.TenantRoles.SysAdmin.ToString().ToLower())
                continue;
            newRoles.Add(role);
        }
        gv.DataSource = newRoles;
        gv.DataBind();
    }
    /// <summary>
    /// cache the user name
    /// </summary>
    protected string SelectedUsername
    {
        get { return (string)ViewState["SelectedUsername"]; }
        set { ViewState["SelectedUsername"] = value; }
    }

    protected void ManagerUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        SelectedUsername = e.CommandArgument.ToString();
        string membershipUserName = new Upn(TenantName, SelectedUsername).ToString();
        GvRoles.Visible = false;
        if (e.CommandName == "DeleteUser")
        {
            if (SelectedUsername.ToLower() == base.Username.ToLower())
            {
                lblErrMsg.Text = Resources.GlobalResources.CannotDelMyself;
                return;
            }
            bool result = Membership.DeleteUser(new Upn(TenantName, SelectedUsername).ToString(), true);
            if (result == true)
                BindGrid(this.ManagerUsers, PageIndex);
        }
        if (e.CommandName == "EditUser")
        {
            string returnUrl = Request.RawUrl;
            Response.Redirect("~/Admin/Users/EditUser.aspx?Username=" + ExtractUsername(membershipUserName) + "&ReturnUrl=" + returnUrl);
        }
        if (e.CommandName == "EditRole")
        {
            //Bind Role 
            GvRoles.Visible = true;
            BindGvRole(this.GvRoles);
            string[] roles = Roles.GetRolesForUser(membershipUserName);
            for (int i = 0; i < GvRoles.Rows.Count; i++)
            {
                CheckBox chb = (CheckBox)GvRoles.Rows[i].Cells[0].FindControl("chbRole");
                chb.Checked = false;
            }
            for (int i = 0; i < GvRoles.Rows.Count; i++)
            {
                CheckBox chb = (CheckBox)GvRoles.Rows[i].Cells[0].FindControl("chbRole");

                for (int j = 0; j < roles.Length; j++)
                {
                    if (chb.Text.ToLower() == roles[j].ToLower())
                    {
                        chb.Checked = true;
                    }
                }
            }
        }
    }
    protected void btnSubmit_ServerClick(object sender, EventArgs e)
    {
        GvRoles.Visible = false;
        if (this.DropDownSerchCondition.SelectedIndex == 0)
        {
            try
            {
                SearchMethod = SearchUserMethod.ByUsername;
                string key = txtSerchKey.Text.Trim();
                if (string.IsNullOrEmpty(key))
                    SearchKey = "*";
                else
                    SearchKey = key.Trim() + "*";
                BindGrid(ManagerUsers, 0);
            }
            catch
            {

                lblError.Text = string.Format(Resources.GlobalResources.UserNotExists, txtSerchKey.Text);
            }
        }
        else
        {

        }
    }
    /// <summary>
    /// find by letters
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void Letters_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        GvRoles.Visible = false;
        SearchMethod = SearchUserMethod.ByUsername;
        if (e.CommandArgument.ToString().Length > 1) //All
            SearchKey = "*";
        else
            SearchKey = e.CommandArgument.ToString() + "*";
        BindGrid(ManagerUsers, 0);
    }

    protected void chbRole_CheckedChanged(object sender, EventArgs e)
    {
        string membershipUserName = new Upn(TenantName, SelectedUsername).ToString();
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
            {
                if (string.Compare(roleName, CrabApp.TenantRoles.Administrators.ToString(), true) == 0)
                {
                    lblError.Text = string.Format("The admin user could not remove the role Administrators by himself/herself.");
                    return;
                }
                Roles.RemoveUserFromRole(membershipUserName, roleName);
            }
        }
    }

    protected void lbtnPre_Click(object sender, EventArgs e)
    {
        GvRoles.Visible = false;
        BindGrid(this.ManagerUsers, PageIndex - 1);
    }
    protected void lbtnNext_Click(object sender, EventArgs e)
    {
        GvRoles.Visible = false;
        BindGrid(this.ManagerUsers, PageIndex + 1);
    }

    protected void lbtnFirst_Click(object sender, EventArgs e)
    {
        GvRoles.Visible = false;
        BindGrid(this.ManagerUsers, 0);
    }

    protected void lbtnLast_Click(object sender, EventArgs e)
    {
        GvRoles.Visible = false;
        BindGrid(this.ManagerUsers, PageCount - 1);
    }

    protected string ExtractUsername(string upn)
    {
        Upn principle = new Upn(upn);
        return principle.Username;
    }

    enum SearchUserMethod
    {
        ByUsername,
        ByEmail
    }
}