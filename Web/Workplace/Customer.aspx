<%@ Page AutoEventWireup="true" CodeFile="Customer.aspx.cs" Inherits="Workplace_Customer"
    Language="C#" MasterPageFile="~/MasterPage.master" Title="Customer Information" %>

<%@ Register Src="../UserControls/Pagination.ascx" TagName="Pagination" TagPrefix="uc2" %>
<asp:Content ID="Content1" runat="Server" ContentPlaceHolderID="ContentPlaceHolder1">
    <asp:ScriptManager ID="ScriptManager" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table cellSpacing="0" cellPadding="5">
                <tr>
                <td><asp:Label ID="lblSearchCustomerName" runat="server" Text="Customer Name"></asp:Label></td>
                <td><asp:TextBox ID="txtSCustomerName" runat="server"></asp:TextBox></td>
                <td><asp:Button ID="btnSearch" runat="server" CausesValidation="False" OnClick="btnSearch_Click"
                    Text="Search" /></td>
                    <td></td>
                </tr>
            </table>
            <br />
            <table cellSpacing="0" width="100%">
                <tr>
                    <td colspan="4" width="100%">
                        <asp:GridView ID="gridCustomer" runat="server" AutoGenerateColumns="False" CssClass="Fixed"
                            DataKeyNames="Id" EnableTheming="true" OnRowDataBound="gridCustomer_RowDataBound"
                            OnRowDeleting="gridCustomer_RowDeleting" OnRowEditing="gridCustomer_RowEditing"
                            ShowFooter="False">
                            <EmptyDataTemplate>
                                No Data
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="30">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderTemplate>
                                        <input onclick="javascript:selectAllCheckboxes(this, 'check')" type="checkbox" /></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="check" runat="server" /><asp:HiddenField ID="Id" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Name" HeaderText="Name" />
                                <asp:BoundField DataField="Address" HeaderText="Address" />
                                <asp:BoundField DataField="PhoneNumber" HeaderText="Phone Number" />
                                <asp:BoundField DataField="Description" HeaderText="Description" />
                                <asp:CommandField ButtonType="Link" DeleteText=" Delete" EditText="Edit " HeaderStyle-Width="40"
                                    ItemStyle-HorizontalAlign="Center" ShowDeleteButton="false" ShowEditButton="true" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td width="30">
                        <asp:ImageButton ID="btnAdd" runat="server" CausesValidation="False" ImageUrl="~/Images/addicon.gif" OnClick="btnAdd_Click" ToolTip="Add a new customer" /></td>
                    <td width="30">
                        <asp:ImageButton ID="btnDelete" OnClientClick="return confirm('Are you sure to delete the selected rows');" runat="server" OnClick="btnDelete_Click" CausesValidation="False" ImageUrl="~/Images/deleteicon.gif" ToolTip="Delete the selected customer(s)" /></td>
                    <td width="30">&nbsp;&nbsp;</td>
                    <td width="100%" noWrap>
                    </td>
                </tr>
            </table>
            <br />
            <div id="pnlEdit" runAt="Server" visible="false">
                <table cellpadding="5" cellspacing="0" >
                    <tbody>
                        <tr>
                            <td nowrap="nowrap">
                                <asp:Label ID="lblName" runat="server" Text="Customer Name"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="txtCustomerName" runat="server"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="valName" runat="server" ControlToValidate="txtCustomerName"
                                    ErrorMessage="*"></asp:RequiredFieldValidator></td>
                            <td>
                                <asp:Label ID="lblPhoneNumber" runat="server" Text="Phone Number"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="txtPhoneNumber" runat="server"></asp:TextBox></td>
                            <td>
                                <asp:Label ID="lblAddress" runat="server" Text="Address"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox><br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="txtDescription" runat="server"></asp:TextBox></td>
                            <td colspan="2">
                                &nbsp;<asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label></td>
                            <td>
                                <asp:HiddenField ID="txtId" runat="server" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            
            <br />
            <div>
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />&nbsp;&nbsp;
                <asp:Button ID="btnCancel" runat="server" CausesValidation="False" OnClick="btnCancel_Click"
                    Text="Cancel" /></div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
