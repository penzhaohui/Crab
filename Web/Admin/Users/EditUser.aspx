<%@ Page AutoEventWireup="true" CodeFile="EditUser.aspx.cs" Inherits="Admin_Users_EditUser" Language="C#" MasterPageFile="~/MasterPage.master" %>

<asp:Content ID="Content1" runat="Server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table border="0" cellpadding="4" cellspacing="0" class="NonGridTable">
        <tr>
            <td align="right">
                <asp:Label ID="lblUserIDTip" runat="server" Text="Username:"></asp:Label>
            </td>
            <td>
                <asp:TextBox id="txtUserID" runat="server"  Enabled="false" maxlength="255" />
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblInRolesTip" runat="server" Text="Roles:"></asp:Label>
            </td>
            <td>
                <asp:Repeater ID="RepeaterRoles" runat="server">
                    <ItemTemplate>
                        <asp:CheckBox ID="chbRoles" runat="server" AutoPostBack="true" OnCheckedChanged="chbRoles_CheckedChanged" Text="<%#Container.DataItem %>" />
                    </ItemTemplate>
                </asp:Repeater>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="lblEmailaddressTip" runat="server" Text="Email:"></asp:Label>
            </td>
            <td>
                <asp:TextBox  id="txtEmail" runat="server" maxlength="128" />
                <asp:RequiredFieldValidator ID="valRequiredEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="*" ToolTip="Email is required." />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail" ErrorMessage="*" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Label ID="lblErrMsg" runat="server" EnableViewState="false" ForeColor="red"></asp:Label></td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Button ID="btnSaveButton" runat="server" OnClick="btnSaveButton_ServerClick" Width="70" Text="Save" />
                <asp:Button ID="btnEditProfile" runat="server" Text="Edit Profile" OnClick="btnEditProfile_Click" />
                <asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" Text="Back" Width="70" /></td>
        </tr>
    </table>
</asp:Content>
