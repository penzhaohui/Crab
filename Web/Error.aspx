<%@ Page AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="Error" Language="C#"
    MasterPageFile="~/SimpleMasterPage.master" %>

<asp:Content ID="Content1" runat="Server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table border="0" cellpadding="4" cellspacing="0" class="NonGridTable" width="100%">
        <tr>
            <td>
                <asp:Label ID="lblSystemInformationTip" runat="server" Text="System Information"></asp:Label>
            </td>
        </tr>
        <tr>
            <%--<td>
                <span style='font-size: 16px; zoom: 4; color: #b9b9b9'><font face='webdings'>i</font></span>
            </td>--%>
            <td>
                <img src="Images/SystemInformation.gif" />
            </td>
            <td>
                <asp:Label ID="lblErrorTip" Text="Sorry, there was an unhandled error occured." runat="server"></asp:Label>
                <br />
                <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="javascript:history.back();"
                    Text="Return back"></asp:HyperLink>
                <br />
                <asp:HyperLink ID="returnLink" runat="server" NavigateUrl="~/default.aspx" Text="Return default page"></asp:HyperLink>
                <br />
                <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="mailto:webmaster@suzsoft.com"
                    Text="Email to webmaster"></asp:HyperLink>
            </td>
        </tr>
    </table>
</asp:Content>
