using System;
using System.Data;
using System.Drawing;
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

public partial class Register : PageBase 
{
    protected void Page_Load(object sender, EventArgs e)
    {
      
    }
   
    protected void Wizard1_NextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        switch(e.CurrentStepIndex)
        {
            case 0:
                {
                    string tenantName = this.txtTenantName.Text.Trim();
                    if (!CrabApp.ValidationUtil.CheckName(tenantName))
                    {
                        Message.Text = Resources.GlobalResources.TenantNameCouldNotContain;
                        Message.ForeColor = Color.Red;
                        e.Cancel = true;
                    }
                    if (tenantName.Length > 12)
                    {
                        Message.Text = Resources.GlobalResources.TenantNameLength;
                        Message.ForeColor = Color.Red;
                        e.Cancel = true;
                    }

                    if (ProvisionProxy.TenantExists(tenantName))
                    {
                        Message.Text = string.Format(Resources.GlobalResources.ExistsTenant, txtTenantName.Text);
                        Message.ForeColor = Color.Red;
                        e.Cancel = true;
                    }
                } break;
            case 1:
                {
                    if (!CrabApp.ValidationUtil.CheckName(txtAdmin.Text.Trim()))
                    {
                        lblMessageNext.Text = Resources.GlobalResources.InvalidName;
                        lblMessageNext.ForeColor = Color.Red;
                        e.Cancel = true;
                        return;
                    }

                    if (ProvisionProxy.TenantExists(this.txtTenantName.Text.Trim()))
                    {
                        lblMessageNext.Text = string.Format(Resources.GlobalResources.ExistsTenant, txtTenantName.Text);
                        lblMessageNext.ForeColor = Color.Red;
                        e.Cancel = true;
                        return;
                    }

                    try
                    {
                        Tenant tenant = ProvisionProxy.CreateTenant(
                            txtTenantName.Text.Trim(),
                            txtDescription.Text.Trim(),
                            int.Parse(this.txtLicenseNum.Text.Trim()),
                            txtContact.Text.Trim(),
                            txtTelephone.Text.Trim(),
                            txtFax.Text.Trim(),
                            txtMobile.Text.Trim(),
                            txtTenantEmail.Text.Trim(),
                            txtWebsite.Text.Trim(),
                            txtCity.Text.Trim(),
                            txtAddress.Text.Trim(),
                            txtZipCode.Text.Trim(),
                            txtAdmin.Text.Trim(),
                            txtAdminPassword.Text.Trim(),
                            txtEmail.Text.Trim()
                        );

                        if (tenant == null)
                        {
                            lblMessageNext.Text = Resources.GlobalResources.FailToCreateTanant;
                            e.Cancel = true;
                            return;
                        }

                        this.lblTenantName.Text = txtTenantName.Text.Trim();
                        this.lblDesc.Text = txtDescription.Text.Trim();
                        this.lblLicenseNum.Text = txtLicenseNum.Text.Trim();
                        this.lblMaxCon.Text = txtMaxConcurrency.Text.Trim();
                        this.lblUserName.Text = txtAdmin.Text.Trim();
                    }
                    catch (Exception ex)
                    {
                        this.lblMessageNext.Text = ex.Message;
                        this.lblMessageNext.ForeColor = Color.Red;
                        e.Cancel = true;
                    }
                } break;
        }
    }
    
    
    protected void StartBackButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("Logon.aspx");
    }

    protected void lbtnSucc_Click(object sender, EventArgs e)
    {
        Response.Redirect("Logon.aspx");
    }
}
