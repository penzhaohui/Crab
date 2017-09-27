<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EditTenantProfile.aspx.cs" Inherits="Admin_EditTenantProfle" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" cellpadding="4" cellspacing="0" class="NonGridTable" style="margin: 15px">
        <tr>
            <td align="right">
                <asp:Label ID="Label10" runat="server" Text="Name:"></asp:Label></td>
            <td>
                <asp:TextBox ID="txtTenantName" readonly="true" style="border:0; font-weight:bold" runat="server" Width="150" >
                </asp:TextBox></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label11" runat="server" Text="Display Name:"></asp:Label></td>
            <td>
                <asp:TextBox ID="txtDisplayName" runat="server" Width="150">
                </asp:TextBox></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label1" runat="server" Text="Contact:"></asp:Label></td>
            <td>
                <asp:TextBox ID="txtContact" runat="server" Width="150">
                </asp:TextBox></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label2" runat="server" Text="Fax:"></asp:Label></td>
            <td>
                <asp:TextBox ID="txtFax" runat="server" Width="150">
                </asp:TextBox></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label3" runat="server" Text="Telephone:"></asp:Label></td>
            <td>
                <asp:TextBox ID="txtTelephone" runat="server" Width="150">
                </asp:TextBox></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label4" runat="server" Text="Mobile:"></asp:Label></td>
            <td>
                <asp:TextBox ID="txtMobile" runat="server" Width="150">
                </asp:TextBox></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label5" runat="server" Text="Email:"></asp:Label></td>
            <td>
                <asp:TextBox ID="txtTenantEmail" runat="server" Width="150">
                </asp:TextBox></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label6" runat="server" Text="Website:"></asp:Label></td>
            <td>
                <asp:TextBox ID="txtWebsite" runat="server" Width="150">
                </asp:TextBox></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label7" runat="server" Text="City:"></asp:Label></td>
            <td>
                <asp:TextBox ID="txtCity" runat="server" Width="150">
                </asp:TextBox></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label8" runat="server" Text="Address:"></asp:Label></td>
            <td>
                <asp:TextBox ID="txtAddress" runat="server" Width="150">
                </asp:TextBox></td>
        </tr>
        <tr>
            <td align="right">
                <asp:Label ID="Label9" runat="server" Text="Zip Code:"></asp:Label></td>
            <td>
                <asp:TextBox ID="txtZipCode" runat="server" Width="150">
                </asp:TextBox></td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Label ID="lblResult" runat="server" forecolor=red EnableViewState="false"></asp:Label>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" /></td>
        </tr>
    </table>
</asp:Content>

