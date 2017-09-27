<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ExportContractView.aspx.cs" Inherits="Workplace_ExportContractView" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div noWrap>   
            <asp:Label ID="lblStatusCaption" runat="server" Text="Process Status:"></asp:Label><asp:Label ID="lblStatus" runat="server" Text="" ForeColor="red" ></asp:Label>
            <asp:Button ID="btnOpenProcess" runat="server" OnClick="btnOpenProcess_Click" Width="100" Text="Open Process"  CausesValidation="true"/>
            &nbsp;&nbsp;
            <asp:Label ID="lblCurrentStepCaption" runat="server" Text="Current Step:"></asp:Label><asp:Label ID="lblCurrentStep" runat="server" Text="" ForeColor="red" ></asp:Label>
            <asp:HyperLink ID="lnkViewProcess" runat="server"  ForeColor="blue" NavigateUrl="~/Workplace/WorkflowView.aspx?processId={0}">View Process</asp:HyperLink>
        </div>
        <br />
        <asp:Panel ID="pnlRemark" runat="server" Visible="false">
            Remark: &nbsp;
            <asp:TextBox ID="txtApproveRemark" Width="300px" runat="server"></asp:TextBox>
        </asp:Panel>
    &nbsp; &nbsp;<asp:Button ID="btnSubmit" runat="server" Text="Submit" Visible="false" OnClick="btnSubmit_Click" />&nbsp;
        <asp:Button ID="btnApprove" runat="server" Text="Approve" Visible="False" OnClick="btnApprove_Click" />&nbsp;&nbsp;
        <asp:Button ID="btnReject" runat="server" Text="Reject" Visible="False" OnClick="btnReject_Click" />&nbsp;&nbsp;
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Visible="False" OnClick="btnCancel_Click" />
        <br />
        <br />
<div align="left">
  <asp:Label ID="lblShippingExportContract" runat="server" Text="Shipping Export Contract"></asp:Label>
</div>
<div align="left" style="height:1px; overflow:hidden; background:#dddddd; width: 50%; margin-top:5px; margin-bottom:10px;"></div>
<table style="table-layout:fixed" id="tblInputControls" runat="server">
    <tr>
        <td valign="top" runat="server" id="leftRegion">
        <!-- Left Part -->
        </td>
        <td valign="top" runat="server" id="rightRegion">
        <!-- Right Part -->
        </td>
    </tr>
</table>

<p style="padding-left:150px;">
    <asp:Button ID="btnSave"
        runat="server" Text="Save" OnClick="btnSave_Click" />
    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" OnClick="btnRefresh_Click" CausesValidation="false" />
    <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" CausesValidation="false"/>
</p>


</asp:Content>

