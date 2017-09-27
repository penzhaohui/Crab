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

public partial class Admin_EditTenantProfle : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Tenant tenant = ProvisionProxy.GetTenantById(base.TenantId);
            if (tenant != null)
            {
                txtTenantName.Text = tenant.Name;
                txtDisplayName.Text = tenant.DisplayName;
                txtAddress.Text = tenant.Address;
                txtCity.Text = tenant.City;
                txtContact.Text = tenant.Contact;
                txtFax.Text = tenant.Fax;
                txtMobile.Text = tenant.Mobile;
                txtTelephone.Text = tenant.Phone;
                txtTenantEmail.Text = tenant.Email;
                txtWebsite.Text = tenant.Website;
                txtZipCode.Text = tenant.ZipCode;
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Tenant tenant = ProvisionProxy.GetTenantById(base.TenantId);
        if (tenant != null)
        {
            ProvisionProxy.UpdateTenantProfile(
               tenant.Name,
               txtDisplayName.Text,
               txtContact.Text,
               txtTelephone.Text,
               txtFax.Text,
               txtMobile.Text,
               txtTenantEmail.Text,
               txtWebsite.Text,
               txtCity.Text,
               txtAddress.Text,
               txtZipCode.Text
            );

            lblResult.Text = "Succeeded !";
        }
    }
}
