<%@ Page AutoEventWireup="true" CodeFile="ManageTenants.aspx.cs" Inherits="SysAdmin_Tenants_ManageTenants"
    Language="C#" MasterPageFile="~/MasterPage.master" Title="Untitled Page" %>

<%@ Register Src="../../UserControls/Pagination.ascx" TagName="Pagination" TagPrefix="uc1" %>

<asp:Content ID="Content1" runat="Server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table border="0" cellpadding="4" cellspacing="0" class="NonGridTable">
        <tr>
            <td><asp:Label runat="server" ID="lblName">Name(Start with) </asp:Label> </td>
            <td> <asp:TextBox ID="txtTenantName" runat="server" ForeColor="gray" onclick="javascript:this.style.color='';this.select();"
                    Width="150px" />
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="lblIsExpired" runat="server">Is Expired? </asp:Label> </td>
            <td><asp:DropDownList ID="drpIsExpired" runat="server" Width="150px">
                    <asp:ListItem Selected="True" Value=""></asp:ListItem>
                    <asp:ListItem Value="True">Yes</asp:ListItem>
                    <asp:ListItem Value="False">No</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td><asp:Label ID="lblIsApproved" runat="server">Is Approved? </asp:Label></td>
            <td><asp:DropDownList ID="drpIsApproved" runat="server" Width="150px">
                    <asp:ListItem Selected="True" Value=""></asp:ListItem>
                    <asp:ListItem Value="True">Yes</asp:ListItem>
                    <asp:ListItem Value="False">No</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>   
        <tr>
            <td>
            </td>
            <td>
                <asp:Button ID="btnSearch" runat="server" Text="Quick Search" OnClick="btnSearch_Click" />
            </td>
        </tr>
    </table>
    <br />
    <asp:GridView ID="gvManageTenants" runat="server" AllowPaging="true" AutoGenerateColumns="false"
        DataKeyNames="Name" OnPageIndexChanging="gvManageTenants_PageIndexChanging" OnRowCommand="gvManageTenants_RowCommand"
        OnRowDataBound="gvManageTenants_RowDataBound" OnRowDeleting="gvManageTenants_RowDeleting"
        PagerSettings-Mode="Numeric" PageSize="10">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="DisplayName" HeaderText="Display Name" />
            <asp:TemplateField HeaderText="Create Date">
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <%# ((DateTime)DataBinder.Eval(Container.DataItem, "CreateDate")).ToString("yyyy-MM-dd") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField  HeaderText="End Date">
                <ItemStyle HorizontalAlign="center" />
                <ItemTemplate>
                    <%# ((DateTime)DataBinder.Eval(Container.DataItem, "EndDate")).ToString("yyyy-MM-dd") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Expired?">
                <ItemStyle HorizontalAlign=center />
                <ItemTemplate>
                    <asp:Label ID="lblExpired" runat=server />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Approved" HeaderText="Approved?">
                <ItemStyle HorizontalAlign="center" />
            </asp:BoundField>
            <asp:BoundField DataField="LicenseCount" HeaderText="License Count">
                <ItemStyle HorizontalAlign="center" />
            </asp:BoundField>
            <asp:TemplateField>
                <ItemStyle HorizontalAlign="Center" />
                <ItemTemplate>
                    <asp:LinkButton ID="lbModify" runat="server" CommandArgument='<%#Eval("ID")%>' CommandName="Modify"
                        Text="Edit"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ButtonType="Link" DeleteText="Delete" EditText="Edit"
                        ItemStyle-HorizontalAlign="center" ShowDeleteButton="true" ShowEditButton="false"/>
        </Columns>
    </asp:GridView>
</asp:Content>
