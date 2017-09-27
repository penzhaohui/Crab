<%@ Page Title="Edit User Profile" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EditProfile.aspx.cs" Inherits="EditProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table cellpadding="2" cellspacing="0" style="margin-top: 10px">
        <tr>
            <td width="100">
                <asp:Label ID="lblUserIDTip" runat="server" Text="Username:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtUserID" runat="server" Enabled="false" Width="150px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblEmailAddressTip" runat="server" Text="Email:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtEmail" runat="server" Width="150px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorEmail" runat="server" ControlToValidate="txtEmail"
                    Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                    Display="Dynamic" ErrorMessage="*" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>&nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblFullName" runat="server" Text="Full Name:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtFullName" runat="server" Width="150px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblTitleTip" runat="server" Text="Title:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtTitle" runat="server" Width="150px"></asp:TextBox>
            </td>
        </tr>
        <tr>
        </tr>
        <tr>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblSexTip" runat="server" Text="Gender:"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="dplSex" runat="server" Width="80px">
                    <asp:ListItem Selected="True"></asp:ListItem>
                    <asp:ListItem>Male</asp:ListItem>
                    <asp:ListItem>Female</asp:ListItem>
                </asp:DropDownList></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblBirthdayTip" runat="server" Text="Birthday:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtBirthday" runat="server" Width="150px"></asp:TextBox>(yyyy-mm-dd)
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMobileTip" runat="server" Text="Mobile:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtMobile" runat="server" Width="150px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblTelTip" runat="server" Text="Telephone:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtTel" runat="server" Width="150px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblFaxTip" runat="server" Text="Fax:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtFax" runat="server" Width="150px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblMsnTip" runat="server" Text="MSN:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtMsn" runat="server" Width="150px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblAddressTip" runat="server" Text="Address:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtAddress" runat="server" Width="150px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblZipCodeTip" runat="server" Text="Zip Code:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtZipCode" runat="server" Width="80px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Repeater ID="RepeaterRoles" runat="server">
                    <ItemTemplate>
                        <asp:CheckBox ID="chbRoles" runat="server" AutoPostBack="true" OnCheckedChanged="chbRoles_CheckedChanged"
                            Text="<%#Container.DataItem %>" />
                    </ItemTemplate>
                </asp:Repeater>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td style="padding-top: 20px">
                <asp:Button ID="btnSaveButton" runat="server" OnClick="btnSaveButton_ServerClick"
                    Text="Submit" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Label ID="lblErrMsg" runat="server" EnableViewState="false" ForeColor="red"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
</asp:Content>

