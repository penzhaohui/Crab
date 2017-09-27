using System;

public partial class WorkflowView : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        processImage.Attributes.Add("onload", "document.getElementById('imageLoading').style.display='none'");
        processImage.ImageUrl = string.Format(processImage.ImageUrl, Request["processid"]);
    }
}