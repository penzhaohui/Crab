using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!User.IsInRole(CrabApp.TenantRoles.Administrators.ToString()))
                PanelAdmin.Visible = false;
        }
    }
}