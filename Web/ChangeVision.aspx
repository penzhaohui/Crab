<%@ Page AutoEventWireup="true" CodeFile="ChangeVision.aspx.cs" Inherits="ChangeVision"
    Language="C#" MasterPageFile="~/MasterPage.master" Title="Change Vision" %>

<asp:Content ID="Content1" runat="Server" ContentPlaceHolderID="ContentPlaceHolder1">
    <asp:Panel ID="formPanel" runat="server">
        <table border="0" cellpadding="5" cellspacing="0" class="fixed" height="240" width="400">
            <tr align="center">
                <td width="25%">
                    <img src="App_Themes/Alternating/images/thumbnail.jpg" style="margin: 10px; border: 1px solid #cccccc" /><br />
                    <input id="Radio1" runat="server" checked="true" name="Selection" type="radio" value="Alternating" />
                    Blue
                </td>
                <td width="25%">
                    <img src="App_Themes/Default/images/thumbnail.jpg" style="margin: 10px; border: 1px solid #cccccc" /><br />
                    <input id="Radio2" runat="server" name="Selection" type="radio" value="Default" />
                    Green
                </td>
            </tr>
            <tr align="center">
                <td colspan="2">
                    <asp:Button ID="btnOK" runat="server" OnClick="btnOK_Click" Text="Change" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
