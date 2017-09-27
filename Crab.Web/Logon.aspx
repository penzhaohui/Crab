<%@ Page Title="" Language="C#" MasterPageFile="~/SimpleMasterPage.master" AutoEventWireup="true" CodeFile="Logon.aspx.cs" Inherits="Logon" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <br />
    <br />
    <table align="center" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:Login ID="Login1" runat="server" class="border" BorderWidth="1px" OnAuthenticate="Login1_Authenticate"
                    TextBoxStyle-CssClass="textbox" Width="350px">
                    <LayoutTemplate>
                        <table align="center" border="0" cellpadding="3" cellspacing="1" style="width: 100%">
                            <tr>
                                <td class="tableHeader" colspan="2" style="font-weight: bold; text-indent: 5px;">
                                    <asp:Label ID="lblSignInTitle" runat="server" Text="Sign In"></asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 15px">
                                </td>
                            </tr>
                            <tr>
                                <td style="text-indent: 30px" width="120" align="right">
                                    <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Username</asp:Label>
                                </td>
                                <td >
                                    <asp:TextBox ID="UserName" runat="server" Style="width: 150px;"></asp:TextBox>
                                    &nbsp;
                                    <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ForeColor="Red"
                                        ErrorMessage="User name is required." ToolTip="User name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-indent: 30px" align="right">
                                    <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="Password" runat="server" Style="width: 150px;" TextMode="Password"></asp:TextBox>
                                    &nbsp;
                                    <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"  ForeColor="Red"
                                        ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-indent: 30px" align="right">
                                    <asp:Label ID="TenantLabel" runat="server" AssociatedControlID="TenantName">Tenant</asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="TenantName" runat="server" Style="width: 150px;"></asp:TextBox>
                                    &nbsp;
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="TenantName"  ForeColor="Red"
                                        ErrorMessage="Tenant name is required." ToolTip="Tenant name is required." ValidationGroup="Login1">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-indent: 10px; padding-top: 7px; padding-bottom: 7px">
                                </td>
                                <td>
                                    <asp:CheckBox ID="RememberMe" runat="server" Text=" Remember me next time." /></td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td style="padding-top: 6px; padding-bottom: 6px">
                                    <asp:Button ID="LoginButton" runat="server" CommandName="Login" Style="width: 70px"
                                        Text="Sign In" ValidationGroup="Login1" /></td>
                            </tr>
                        </table>
                        <div style="padding: 10px; color: Red">
                            <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal></div>
                    </LayoutTemplate>
                </asp:Login>
                <br />
                <br />
                <div style="text-align: right; margin: 15px 5px">
                    Do not have a tenant account ?<br />
                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Register.aspx">Sign up now for free trial !</asp:HyperLink></div>
            </td>
        </tr>
    </table>
</asp:Content>

