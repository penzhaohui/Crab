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
using Crab.Services.Proxy;

public partial class Workplace_ProcessImage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ClearContent();
        Response.ContentType = "image/png";
        Guid processId = new Guid(Request["processid"]);
        byte[] binaries = WorkflowProxy.GetWorkflowGraphic(processId);
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
