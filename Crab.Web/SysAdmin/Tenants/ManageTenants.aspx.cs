using Crab.Runtime.Contract;
using Crab.Services.Proxy;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

public partial class ManageTenants : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            LoadGrid();
        }
    }

    private IList<Tenant> GetEmptyTenantList()
    {
        Tenant t = new Tenant();
        t.CreateDate = DateTime.Now;

        IList<Tenant> list = new List<Tenant>();
        list.Add(t);

        return list;
    }

    private void LoadGrid()
    {
        bool temp;
        bool? isExpired = null;
        bool? isApproved = null;

        if (drpIsExpired.SelectedValue.Length != 0)
        {
            bool.TryParse(drpIsExpired.SelectedValue, out temp);
            isExpired = temp;
        }

        if (drpIsApproved.SelectedValue.Length != 0)
        {
            bool.TryParse(drpIsApproved.SelectedValue, out temp);
            isApproved = temp;
        }

        IList<Tenant> list = ProvisionProxy.FindTenants(txtTenantName.Text.Trim(), isApproved, isExpired);

        if (list.Count == 0)
            list = GetEmptyTenantList();

        this.gvManageTenants.DataSource = list;
        this.gvManageTenants.DataBind();
    }


    protected void gvManageTenants_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string ReturnUrl = Context.Request.RawUrl;
        if (e.CommandName == "Modify")
        {
            Response.Redirect("ModifyTenants.aspx?ReturnUrl=" + ReturnUrl + "&TenantId=" + e.CommandArgument);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadGrid();
    }

    protected void gvManageTenants_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        Tenant t = e.Row.DataItem as Tenant;

        TableCell cell = e.Row.Cells[e.Row.Cells.Count - 1];
        LinkButton linkButton = cell.Controls[0] as LinkButton;
        //Add confirm information to the delete link button
        if (linkButton != null)
            linkButton.Attributes.Add("onclick",
                string.Format("javascript:return confirm('{0}');",
                Resources.GlobalResources.ConfirmDelete));


        Label lblExpired = (Label)e.Row.FindControl("lblExpired");
        if (lblExpired != null)
            lblExpired.Text = t.IsOverdue().ToString();

        if (t != null && t.Id == Guid.Empty)
        {
            e.Row.Visible = false;
        }
    }

    protected void gvManageTenants_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string tenantName = (string)gvManageTenants.DataKeys[e.RowIndex].Value;
        ProvisionProxy.DeleteTenant(tenantName, true);
        LoadGrid();
    }

    protected void gvManageTenants_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvManageTenants.PageIndex = e.NewPageIndex;
        LoadGrid();
    }
}