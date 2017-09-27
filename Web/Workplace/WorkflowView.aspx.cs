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

public partial class Workplace_WorkflowView : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        processImage.Attributes.Add("onload", "document.getElementById('imageLoading').style.display='none'");
        processImage.ImageUrl = string.Format(processImage.ImageUrl, Request["processid"]);
    }
}
