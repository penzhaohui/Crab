<%@ Page AutoEventWireup="true" CodeFile="ManageUsers.aspx.cs" Inherits="Admin_Users_ManageUsers" Language="C#" MasterPageFile="~/MasterPage.master" %>

<asp:Content ID="Content" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table border="0" cellpadding="4" cellspacing="0" class="NonGridTable" width="100%">
        
        <tr>
            <td style="width: 601px">
                <asp:Label ID="lblSearchbyTip" runat="server" Text="Search by:"></asp:Label>
                &nbsp;&nbsp;                    
                <%--********************************************************--%>
                <asp:DropDownList ID="DropDownSerchCondition" runat="server">
                    <asp:ListItem Text="User name" Value="User name">
                    </asp:ListItem>
                </asp:DropDownList>
                <%--*******************************************************--%>
                &nbsp;&nbsp;
                <asp:Label ID="lblForTip" runat="server" Text="Start with:"></asp:Label>
                &nbsp;&nbsp;
                <asp:TextBox runat="server" ID="txtSerchKey" />
                &nbsp;&nbsp;
                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_ServerClick" Text="Find User" />
            </td>
        </tr>
        <tr>
            <td style="width: 601px">
                <asp:Label ID="lblError" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 601px">
                <asp:Repeater ID="repeaterLetters" runat="server" OnItemCommand="Letters_ItemCommand">
                    <ItemTemplate>
                        <asp:LinkButton ID="AlphabetClick" runat="server" CommandArgument="<%#Container.DataItem%>" CommandName="Display" Text="<%#Container.DataItem%>">
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:Repeater>
            </td>
        </tr>
    </table>
    <br />
    <table border="0" cellpadding="4" cellspacing="0" class="NonGridTable" width="100%">
        <tr valign="top">
            <td>
                <asp:GridView ID="ManagerUsers" runat="server" AutoGenerateColumns="False" BorderColor="#3399FF"
                    BorderWidth="1px" OnRowCommand="ManagerUsers_RowCommand">
                    <Columns>
                        <asp:TemplateField HeaderText="User Name">
                            <ItemTemplate>
                                <%# ExtractUsername((string)Eval("UserName"))%>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                            <HeaderStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Email" DataField="Email">
                            <ItemStyle Width="120px" />
                        </asp:BoundField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CommandArgument='<%#ExtractUsername((string)Eval("UserName"))%>' CommandName="EditUser" Text="Edit User"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEditRole" runat="server" CommandArgument='<%#ExtractUsername((string)Eval("UserName"))%>' CommandName="EditRole" Text="Edit Role"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" Width="80px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDel" runat="server" CommandArgument='<%#ExtractUsername((string)Eval("UserName"))%>' CommandName="DeleteUser"
                                    OnClientClick="return confirm('Are you sure to delete the user?');"
                                    Text="Delete User"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" Width="80px" />
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle HorizontalAlign="Right" Font-Size="Medium" />
                    <PagerSettings Mode="NextPreviousFirstLast" FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev" />
                </asp:GridView>
                <asp:Label ID="lblErrMsg" runat="server" ForeColor="Red"></asp:Label></td>
            <td valign="top">
                <asp:GridView ID="GvRoles" runat="server" AutoGenerateColumns="false" BorderColor="#3399ff"
                    BorderWidth="1" Visible="false">
                    <Columns>
                        <asp:TemplateField HeaderText="Roles">
                            <ItemTemplate>
                                <asp:CheckBox ID="chbRole" runat="server" AutoPostBack="true" OnCheckedChanged="chbRole_CheckedChanged" Text="<%#Container.DataItem %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr align = "left">
            <td>
                <%=PageCount>0?PageIndex+1:0%>/<%=PageCount%>
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:LinkButton ID="lbtnFirst" runat="server" CommandName="first" OnClick="lbtnFirst_Click" Text="First"></asp:LinkButton>&nbsp;
                <asp:LinkButton ID="lbtnPre" runat="server" CommandName="previous" OnClick="lbtnPre_Click" Text="Previous"></asp:LinkButton>&nbsp;
                <asp:LinkButton ID="lbtnNext" runat="server" CommandName="next" OnClick="lbtnNext_Click" Text="Next"></asp:LinkButton>&nbsp;
                <asp:LinkButton ID="lbtnLast" runat="server" CommandName="last" OnClick="lbtnLast_Click" Text="Last"></asp:LinkButton>
            </td>
            <td align="left">
            </td>
        </tr>
    </table>
</asp:Content>
