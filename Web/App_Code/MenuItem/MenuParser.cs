using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public class MenuParser
{
	public MenuParser(string xmlPath)
	{
		this.m_xmlDoc = new XmlDocument();
		m_xmlDoc.Load(xmlPath);
	}

	private XmlDocument m_xmlDoc;

	/// <summary>
	/// Fetch the MenuLink instance who's url is visiting by current user.
	/// </summary>
	/// <returns>MenuLink instance</returns>
	public MenuLink GetVisitingMenuLink()
	{
        XmlNode currentNode = this.GetUrlMacthedNode();
        return new MenuLink(currentNode.Attributes, currentNode.ParentNode.ParentNode);
	}

	/// <summary>
	/// Fetch main menu links configged in XML for the main navigation bar.
	/// </summary>h  
	/// <returns>MenuLink Array</returns>
	public IList<MenuLink> GetMainMenuLinks()
	{
		return this.XmlNode2MenuLink(m_xmlDoc.GetElementsByTagName("MainMenu"));
	}

	/// <summary>
	/// Fetch sub menu links for current.
	/// </summary>
	/// <returns>MenuLink Array</returns>
	public IList<MenuLink> GetCurrentSubMenuLinks()
	{
		XmlNode currentNode = this.GetUrlMacthedNode();
		if (currentNode != null)
		{
			return this.XmlNode2MenuLink(currentNode.ParentNode.ChildNodes);
		}
		return new MenuLink[0];
	}
    /// <summary>
    /// get submenu icon 
    /// </summary>
    /// <param name="children">list of nodes</param>
    /// <returns></returns>
    public string GetCurrentImgUrl(MenuLink[] children)
    {
        string url = GetCurrentScriptName();
        for (int i = 0; i < children.Length ; i++)
        {
            MenuLink menu = children[i];
            if (menu.Url.ToLower() == url )
            {
                return menu.Icon;

            }
        }
        return null;
    }

	private XmlNode GetUrlMacthedNode()
	{
		XmlNodeList allSubNodes = m_xmlDoc.GetElementsByTagName("SubMenu");
		string currentScriptName = this.GetCurrentScriptName();

		foreach (XmlNode xmlNode in allSubNodes)
		{
            XmlAttribute urkAttr = xmlNode.Attributes["Url"];
            if (urkAttr == null)
            {
                continue;
            }
            string url = urkAttr.Value.ToLower();
			if (url.StartsWith(currentScriptName))
			{
				return xmlNode;
			}
		}
        //return null;
        return allSubNodes[10];//modify temporarily by michael
	}

	private string GetCurrentScriptName()
	{
        
		string url = HttpContext.Current.Request.RawUrl.ToLower();
        string application = HttpContext.Current.Request.ApplicationPath;
        if (application == null || application == "/")
            application = "";
        string scriptName = url.Substring(application.Length, url.Length - application.Length);// url.Substring(4);//url.LastIndexOf("/"));   //modify temporarily by michael
		if (scriptName == "/")
		{
			return "~/default.aspx";
		}
		if (scriptName.IndexOf("?") != -1)
		{
			scriptName = scriptName.Split('?')[0];
		}
		return "~" + scriptName;
	}

	private IList< MenuLink> XmlNode2MenuLink(XmlNodeList xmlNodes)
	{
		int count = xmlNodes.Count;
        IList<MenuLink> menuLinkList = new List<MenuLink>();
		MenuLink[] menuLinks = new MenuLink[count];
		for (int i = 0; i < count; i++)
		{            
			menuLinks[i] = new MenuLink(xmlNodes[i].Attributes);
            XmlAttribute attribute = xmlNodes[i].Attributes["Roles"];
            if (attribute == null&&xmlNodes[i].Name=="SubMenu") // from the parent node
            {
                attribute = xmlNodes[i].ParentNode.ParentNode.Attributes["Roles"];
            }
            if (attribute != null && !string.IsNullOrEmpty(attribute.Value)&&attribute.Value!="*")
            {
                string userName = HttpContext.Current.User.Identity.Name;
                string roleName= attribute.Value;
                bool notFlag = false;
                if (roleName[0] == '~')
                {
                    roleName = roleName.Substring(1);
                    notFlag = true;
                }
                string[] roles = roleName.Split(new char[]{',', ';', '|'});
                bool userInRole = false;
                int j = 0;
                for (j = 0; j < roles.Length; j++)
                {
                    if (Roles.IsUserInRole(userName, roles[j]))
                    {
                        userInRole = true;
                        if (!notFlag)
                            menuLinkList.Add(menuLinks[i]);
                        break;
                    }
                }
                if (j == roles.Length && !userInRole && notFlag)
                    menuLinkList.Add(menuLinks[i]);
               
            }
            else 
                menuLinkList.Add(menuLinks[i]);
		}
        return menuLinkList;
	}
}

