<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddAttachmentDialog.ascx.cs" Inherits="UserControls_AddAttachmentDialog" %>
<asp:Panel runat="server" ID="popupPanel" CssClass="modalPopup" Style="display: none">
    <table width="100%" height="100%" class="NonGridTable">
        <tr>
            <td class="tableHeader">
                <asp:Label runat="server" ID="lblAddAttachmentTitle" Text="Add/Edit Attachment File" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" ID="lblFile" Text = "File:" /> 
                <asp:FileUpload ID="FileUpload" runat="server" />
            </td>
        </tr>
        <tr>
            <td height="100%">
                <asp:Panel runat="server" ID="popupContainer" HorizontalAlign="left"/>
            </td>
        </tr>
        <tr>
            <td align="left">
                <br />
                <asp:Button ID="OkButton" runat="server" Text="OK" ValidationGroup="AddAttachment" />
                <asp:Button ID="CancelButton" runat="server" Text="Cancel" />
            </td>
        </tr>
    </table>
</asp:Panel>