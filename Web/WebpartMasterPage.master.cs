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

public partial class WebpartMasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Control uc = LoadControl("~/UserControls/WebPartsModeMenu.ascx");
        Menu menu = uc.FindControl("Menu1") as Menu ;
        menu.MenuItemClick += new MenuEventHandler(menu_MenuItemClick);
        Panel panel = (Panel)Master.FindControl("panelPlaceHolder");
        panel.Controls.Add(uc);
    }

    void menu_MenuItemClick(object sender, MenuEventArgs e)
    {
         WebPartManager webPartManager = WebPartManager.GetCurrentWebPartManager(Page);
        if (webPartManager == null)
            return;
        if (e.Item.Depth == 1)
        {            
            webPartManager.DisplayMode = webPartManager.DisplayModes[e.Item.Value];
        }
    }  

   
}
