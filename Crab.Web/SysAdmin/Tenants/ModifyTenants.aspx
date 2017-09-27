<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ModifyTenants.aspx.cs" Inherits="ModifyTenants" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <table cellpadding="0" cellspacing="0" width="80%" style="table-layout:fixed">
        <tr>
            <td valign=top>
                <table border="0" cellpadding="4" cellspacing="0" class="NonGridTable">
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblNameTip" runat="server" Text="Name:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtName" runat="server" Font-Bold="true" ReadOnly="true" Style="width: 100px; font-weight:bold;
                                border: 0px;" Width="200px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblDisplayName" runat="server" Text="Display Name:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDisplayName" runat="server" Style="width: 100px; border: 0px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblCreateDateTip" runat="server" Text="Create Date:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCreateDate" runat="server" ReadOnly="true" Style="width: 100px;
                                border: 0px">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" OnClientClick="javascript:return confirm('This tenant will be removed, are you sure?')"
                                Text="Delete" /></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="background: #cccccc; height: 1px; margin: 10px; overflow: hidden">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lblEndDate" runat="server" Text="End Date:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEndDate" runat="server" Width="100px">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnSetDeadline" runat="server" OnClick="btnSetDeadline_Click" Text="Save" />
                            <asp:Label ID=lblDeadlineResult runat=server ForeColor=red></asp:Label></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div style="background:#cccccc; height:1px; margin:10px; overflow:hidden"></div>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label10" runat="server" Text="Is Approved:"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblApproved" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnApprove" runat="server" OnClick="btnApprove_Click" OnClientClick="javascript:return confirm('Are you sure to approve this tenant?');" Text="Approve" /></td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="ErrMsg" runat="server" ForeColor="Red"></asp:Label></td>
                    </tr>
                </table>
            </td>
            <td valign=top>
                <table border="0" cellpadding="4" cellspacing="0">
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label1" runat="server" Text="Contact:"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblContact" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label2" runat="server" Text="Fax:"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblFax" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label3" runat="server" Text="Telephone:"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblTelephone" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label4" runat="server" Text="Mobile:"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblMobile" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label5" runat="server" Text="Email:"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblTenantEmail" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label6" runat="server" Text="Website:"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblWebsite" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label7" runat="server" Text="City:"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblCity" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label8" runat="server" Text="Address:"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblAddress" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="Label9" runat="server" Text="Zip Code:"></asp:Label></td>
                        <td>
                            <asp:Label ID="lblZipCode" runat="server"></asp:Label></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

