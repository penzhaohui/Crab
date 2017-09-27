using Crab.Runtime.Contract;
using Crab.Services.Proxy;
using System;

public partial class Admin_EditTenantProfile : PageBase
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