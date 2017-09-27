using Crab.Services.Proxy;
using System;

public partial class ProcessImage : PageBase
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