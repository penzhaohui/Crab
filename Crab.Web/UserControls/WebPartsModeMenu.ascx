<%@ Control AutoEventWireup="true" CodeFile="WebPartsModeMenu.ascx.cs" Inherits="UserControls_WebPartsModeMenu"
    Language="C#" %>
<asp:Menu ID="Menu1" runat="server" DynamicMenuItemStyle-CssClass="webpartMenu" Orientation="horizontal">
    <DynamicMenuItemStyle CssClass="wp_MenuItem" />
    <DynamicHoverStyle CssClass="wp_MenuItemHover" />
    <DynamicMenuStyle CssClass="wp_MenuFrame" />
    <Items>
        <asp:MenuItem PopOutImageUrl="~/Images/menudark.gif" Text="Modify Shared Page">
            <asp:MenuItem Text="Browse" Value="Browse"></asp:MenuItem>
            <asp:MenuItem Text="Design" Value="Design"></asp:MenuItem>
            <asp:MenuItem Text="Edit" Value="Edit"></asp:MenuItem>
            <asp:MenuItem Text="Add WebParts" Value="Catalog"></asp:MenuItem>
        </asp:MenuItem>
    </Items>
</asp:Menu>
