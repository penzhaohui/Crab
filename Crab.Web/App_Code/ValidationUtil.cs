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
    /// Summary description for ValidationUtil
    /// </summary>
    static public class ValidationUtil
    {
        private const string ReservedChars = "$/\\~!@#%^&*()+-<>,?;'\"\"[]{}`";

        public static bool CheckName(string name)
        {
            for (int i = 0; i < ReservedChars.Length; i++)
                if (name.Contains(ReservedChars[i].ToString()))
                    return false;
            return true;
        }
    }
}
