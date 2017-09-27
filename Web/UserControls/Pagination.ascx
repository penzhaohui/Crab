<%@ Control AutoEventWireup="true" CodeFile="Pagination.ascx.cs" Inherits="Workplace_Pager"
    Language="C#" %>
<table width="100%" cellspacing="0">
    <tr>
        <td>
            <asp:LinkButton ID="btnFirst" runat="server" CausesValidation="False" OnClick="btnFirst_Click"
                ToolTip="First Page">First</asp:LinkButton>&nbsp;&nbsp;
            <asp:LinkButton ID="btnPrevious" runat="server" CausesValidation="False" OnClick="btnPrevious_Click"
                ToolTip="Previous Page">Previous</asp:LinkButton>&nbsp;&nbsp;
            <asp:LinkButton ID="btnNext" runat="server" CausesValidation="False" OnClick="btnNext_Click"
                ToolTip="Next Page">Next</asp:LinkButton>&nbsp;&nbsp;
            <asp:LinkButton ID="btnLast" runat="server" CausesValidation="False" OnClick="btnLast_Click"
                ToolTip="Last Page">Last</asp:LinkButton></td>
        <td align="right">
            <asp:Label ID="lblCount" runat="server" Text="1 / 1"></asp:Label></td>
    </tr>
</table>


