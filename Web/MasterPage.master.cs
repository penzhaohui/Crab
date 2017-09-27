using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class MasterPage : System.Web.UI.MasterPage 
{
	private MenuLink m_visitingMenuLink;
    private MenuLink m_submenuHome;

    protected void Page_Load(object sender, EventArgs e)
    {
        string url = Context.Request.RawUrl;
        lblTenantDisplayName.Text = ((PageBase)this.Page).CurrentTenant.DisplayName;

        if (!IsPostBack)
        {
            MenuParser parser = new MenuParser(Server.MapPath("~/Web.Menu.xml"));
            this.m_visitingMenuLink = parser.GetVisitingMenuLink();

            this.lblCurrentMenuDescription.Text = m_visitingMenuLink.Text;
            if (this.imgCurrentMenuIcon.Visible = m_visitingMenuLink.Icon != string.Empty)
            {
                this.imgCurrentMenuIcon.ImageUrl = m_visitingMenuLink.Icon;
            }

            rptMainMenu.DataSource = parser.GetMainMenuLinks();
            rptMainMenu.DataBind();
            IList<MenuLink> submenus = parser.GetCurrentSubMenuLinks();
            if (submenus.Count > 0)
                m_submenuHome = submenus[0];
            for (int i = 0; i < submenus.Count; ) //delete the invisible submenus
            {
                if (submenus[i].Visible == false 
                    && submenus[i].Url != m_visitingMenuLink.Url) //keep the current submenu visible
                {
                    submenus.RemoveAt(i);
                    continue;
                }
                i++;
            }
            rptSubMenu.DataSource = submenus;
            rptSubMenu.DataBind();
            string displayName = string.IsNullOrEmpty(Profile.FullName) ? Context.User.Identity.Name : Profile.FullName;
            lblUserLogin.Text = Resources.GlobalResources.Welcome + " " + displayName;
        }
    }
    protected void rptMainMenu_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            if (e.Item.ItemIndex == 0)
            {
                e.Item.FindControl("ltrDelimiter").Visible = false;
            }

            //todo here:
            //if has no permission, set current item no visible.

            MenuLink menuLink = e.Item.DataItem as MenuLink;
            HyperLink link = (HyperLink)e.Item.FindControl("linkMenuItem");
            link.Text = menuLink.Text;
            link.NavigateUrl = menuLink.Url;
            link.ToolTip = menuLink.Text;
        }

    }

    protected void rptSubMenu_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            MenuLink menuLink = e.Item.DataItem as MenuLink;

            //todo here:
            //if has no permission, set current item no visible.

            HyperLink link = (HyperLink)e.Item.FindControl("linkMenuItem");
            link.Text = menuLink.Text;
            link.NavigateUrl = menuLink.Url;
            link.ToolTip = menuLink.Text;
            if (menuLink.Url == m_visitingMenuLink.Url)
                link.NavigateUrl = Request.Url.ToString();
            else
                link.NavigateUrl = menuLink.Url;
            HtmlTableCell td = e.Item.FindControl("tdMenuItemCell") as HtmlTableCell;

            if (m_visitingMenuLink != null && m_visitingMenuLink.Url == menuLink.Url)
            {
                td.Attributes.Add("class", "CurrentSubmenuItem");
            }          
        }
    }

    protected void LoginStatus1_OnLoggingOut(object sender, System.Web.UI.WebControls.LoginCancelEventArgs e)
    {
        Session.Abandon();
        Session.Clear();
    }
}
