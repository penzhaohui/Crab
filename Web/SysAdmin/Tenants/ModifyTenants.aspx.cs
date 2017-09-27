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
using Crab.Services.Proxy;

public partial class SysAdmin_Tenants_ModifyTenants : PageBase
{
    private string returnUrl;
    private Guid tenantId;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (User.IsInRole(CrabApp.TenantRoles.SysAdmin.ToString()))
        {
            btnDelete.Visible = false;
        }

        if (!IsPostBack)
        {
            if (object.Equals(Request["ReturnUrl"],null)||object.Equals(Request["TenantId"],null))
            {
                Response.Redirect("ManageTenants.aspx", true);
            }
            returnUrl = Request["ReturnUrl"].ToString();

            tenantId = new Guid(Request["TenantId"].ToString());
            ViewState["TenantId"] = tenantId;
            ViewState["ReturnUrl"] = returnUrl;

            Tenant tenant = ProvisionProxy.GetTenantById(tenantId);
            BindPageControls(tenant);
        }
        else
        {
            tenantId = (Guid)ViewState["TenantId"];
            returnUrl = ViewState["ReturnUrl"] as string;
        }
    }

    /// <summary>
    /// Bind the textbox when the page is load
    /// </summary>
    /// <param name="t">tenant infomation</param>
    private void BindPageControls(Tenant tenant)
    {
        txtName.Text = tenant.Name;
        txtDisplayName.Text = tenant.DisplayName;
        txtCreateDate.Text = tenant.CreateDate.ToString("d");

        txtEndDate.Text = tenant.EndDate.ToString("d");

        lblApproved.Text = tenant.Approved.ToString();
        btnApprove.Enabled = !tenant.Approved;

        lblAddress.Text = tenant.Address;
        lblCity.Text = tenant.City;
        lblContact.Text = tenant.Contact;
        lblFax.Text = tenant.Fax;
        lblMobile.Text = tenant.Mobile;
        lblTelephone.Text = tenant.Phone;
        lblTenantEmail.Text = tenant.Email;
        lblWebsite.Text = tenant.Website;
        lblZipCode.Text = tenant.ZipCode;
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        ProvisionProxy.DeleteTenant(txtName.Text, true);
        Response.Redirect("ManageTenants.aspx", true);
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        ProvisionProxy.ApproveTenant(txtName.Text);
        lblApproved.Text = "True";
        btnApprove.Enabled = false;

    }

    protected void btnSetDeadline_Click(object sender, EventArgs e)
    {
        DateTime endDate;
        DateTime.TryParse(txtEndDate.Text, out endDate);

        if (endDate == DateTime.MinValue)
        {
            lblDeadlineResult.Text = "<br />Failed, '" + txtEndDate.Text + "' is not a correct format of datetime";
        }

        ProvisionProxy.SetDeadline(txtName.Text, endDate);
        lblDeadlineResult.Text = " Suceeded !";
    }
}
