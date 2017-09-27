using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CrabApp
{
    /// <summary>
    /// Summary description for TenantRoles
    /// </summary>
    public enum TenantRoles
    {
        SysAdmin = 0,
        Administrators,
        Managers,
        Users,
        Readers
    }
}
