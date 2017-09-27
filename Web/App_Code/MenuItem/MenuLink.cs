using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;

/// <summary>
/// Summary description for MenuLink
/// </summary>

public class MenuLink
{
    #region private fields
    private string _text;
    private string _url;
    private string _descrpition;
    private string _icon;
    private string _permittedRoles;
    private bool _visible;
    private string _resourceKey;
    #endregion

    #region properties
    public string Text
    {
        get
        {
            string resourceText = string.IsNullOrEmpty(_resourceKey) ? null : Resources.GlobalResources.ResourceManager.GetString(_resourceKey);
            return string.IsNullOrEmpty(resourceText) ? _text : resourceText;
        }
    }

    public string Url
    {
        get
        {
            return _url;
        }
    }

    public string Description
    {
        get
        {
            return _descrpition;
        }
    }

    public string Icon
    {
        get
        {
            return _icon;
        }
    }

    public string PermittedRoles
    {
        get
        {
            return _permittedRoles;
        }
    }

    public bool Visible
    {
        get
        {
            return _visible;
        }
    }

    public string ResourceKey
    {
        get
        {
            return _resourceKey;
        }
    }
    #endregion

    #region constructors
    public MenuLink(XmlAttributeCollection dataSrc)
    {
        _text = GetAttributeValue(dataSrc["Text"]);
        _url = GetAttributeValue(dataSrc["Url"]);
        _descrpition = GetAttributeValue(dataSrc["Description"]);
        _icon = GetAttributeValue(dataSrc["Icon"]);
        _permittedRoles = GetAttributeValue(dataSrc["Roles"]);
        string visibleString = GetAttributeValue(dataSrc["Visible"]);
        if (string.IsNullOrEmpty(visibleString))
            _visible = true;
        else
            _visible = visibleString.ToLower() != "false";
        _resourceKey = GetAttributeValue(dataSrc["ResourceKey"]);
    }

    public MenuLink(XmlAttributeCollection dataSrc, XmlNode parentNode)
        : this(dataSrc)
    {
        if (string.IsNullOrEmpty(_icon))
        {
            //get the icon of the parent node
            _icon = parentNode.Attributes["Icon"] == null ? "" : parentNode.Attributes["Icon"].Value;
        }
    }
    #endregion

    private string GetAttributeValue(XmlAttribute attr)
    {
        if (attr == null)
        {
            return string.Empty;
        }
        return attr.Value;
    }


}