using Crab.Business.Contract;
using Crab.Services.Proxy;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;

public partial class SearchExportContract : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string[] statusNames = Enum.GetNames(typeof(Crab.Business.Contract.ProcessStatus));

            for (int i = 0; i < statusNames.Length; i++)
                DropDownListStatus.Items.Add(new ListItem(statusNames[i], statusNames[i]));

            DropDownListStatus.SelectedValue = Crab.Business.Contract.ProcessStatus.Running.ToString();

            if (Request["owner"] != null)
            {
                ConditionPanel.Visible = false;
                ExecuteQuery();
            }
        }
    }


    private void ExecuteQuery()
    {
        IList<ExportProcessDC> processes = null;
        if (Request["owner"] != null)
        {
            processes = ExportProcessProxy.GetExportProcessListByCreator(HttpContext.Current.User.Identity.Name);
        }
        else
        {
            Crab.Business.Contract.ProcessStatus processStatus = (Crab.Business.Contract.ProcessStatus)Enum.Parse(typeof(Crab.Business.Contract.ProcessStatus), DropDownListStatus.SelectedValue);
            processes = ExportProcessProxy.GetExportProcessListByStatus(processStatus);
        }
        this.dGrid.DataSource = processes;
        this.dGrid.DataBind();
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        ExecuteQuery();
    }

    protected void dGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dGrid.PageIndex = e.NewPageIndex;
        ExecuteQuery();
    }
}