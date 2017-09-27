using Crab.Services.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Customization_WorkflowImage : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ClearContent();
        Response.ContentType = "image/png";
        byte[] binaries = WorkflowProxy.GetWorkflowDefinitionGraphic(0);
        if (binaries != null && binaries.Length != 0)
        {
            Response.AddHeader("Content-Length", binaries.Length.ToString());
            Response.BinaryWrite(binaries);
        }
        else
        {
            Response.AddHeader("Content-Length", "0");
        }
        Response.Flush();
        Response.End();
    }
}