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
using Crab.Services.Proxy;
using Crab.Runtime.Contract;
using Crab.Business.Contract;

public partial class Workplace_ExportContractCreate : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!CanCreate())
            {
                ExportContractNumber.Enabled = false;
                ButtonCreate.Enabled = false;
            }
        }
    }
    protected void ButtonCreate_Click(object sender, EventArgs e)
    {
        if (!CanCreate())
            return;
        string number = ExportContractNumber.Text.Trim();
        if(ExportProxy.ContractExists(number))
        {
            LabelErrMsg.Text = string.Format("The Export Contract with the Number {0} has already existed!", number);
            LabelErrMsg.Visible = true;
            return;
        }
        Guid contractId = ExportProxy.CreateExportContract(number, base.UserId);
        Response.Redirect("ExportContractView.aspx?objectId=" + contractId.ToString());
    }

    private bool CanCreate()
    {
        return User.IsInRole(CrabApp.TenantRoles.Users.ToString()) ||
            User.IsInRole(CrabApp.TenantRoles.Managers.ToString());
    }
}
