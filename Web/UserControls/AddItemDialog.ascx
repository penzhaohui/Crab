<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddItemDialog.ascx.cs" Inherits="UserControls_AddItemDialog" %>
<asp:Panel runat="server" ID="popupPanel" CssClass="modalPopup" Style="display: none">
    <table width="100%" height="100%" class="NonGridTable">
        <tr>
            <td class="tableHeader">
                <asp:Label runat="server" ID="lblAddItemTitle" Text="Add/Edit table item" />
            </td>
        </tr>
        <tr>
            <td height="100%">
                <asp:Panel runat="server" ID="popupContainer" HorizontalAlign="Center"/>
            </td>
        </tr>
        <tr  valign="bottom">
            <td align="center">
                <br />
                <asp:Button ID="OkButton" runat="server" Text="OK" ValidationGroup="AddItem" />
                <asp:Button ID="CancelButton" runat="server" Text="Cancel" />
            </td>
        </tr>
    </table>
</asp:Panel>
