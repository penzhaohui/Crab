using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Crab.Runtime.Contract;
/// <summary>
/// Summary description for TenantSqlPersonalizationProvider
/// </summary>
namespace CrabApp.Providers
{
    public class TenantSqlPersonalizationProvider : SqlPersonalizationProvider
    {
        private string tenantName;
        public string TenantName
        {
            get
            {
                Upn upn = new Upn(HttpContext.Current.User.Identity.Name);
                if (!string.IsNullOrEmpty(upn.TenantName))
                {
                    return upn.TenantName;
                }
                return tenantName;
            }
            set { tenantName = value; }
        }

        public override string ApplicationName
        {
            get
            {
                if (string.IsNullOrEmpty(TenantName))
                    return base.ApplicationName;
                else 
                    return "Crab_" + TenantName + "_" + base.ApplicationName;
            }
            set
            {
                base.ApplicationName = value;
            }
        }
    }
}