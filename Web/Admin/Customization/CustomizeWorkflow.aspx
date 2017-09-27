<%@ Page AutoEventWireup="true" CodeFile="CustomizeWorkflow.aspx.cs" Inherits="Admin_Customization_CustomizeWorkflow" Language="C#" MasterPageFile="~/MasterPage.master" Title="Customize Workflow" %>

<asp:Content ID="Content1" runat="Server" ContentPlaceHolderID="ContentPlaceHolder1">
    <table align="center" border="0" cellpadding="4" cellspacing="0" class="NonGridTable" width="100%">
        <tr>
            <td>
                <a href="../../WorkflowClient/WorkflowClient.application">
                    <asp:Label ID="lblEditWorkflowTip" runat="server" Text="Edit Workflow"></asp:Label>
                </a>
            </td>
        </tr>
        <tr>
            <td align="center">
                <img id="imageLoading" alt="" src="../../Images/loading.gif" />
            </td>
        </tr>
        <tr>
            <td>
                <img id="ImageXoml" alt="" onload="document.getElementById('imageLoading').style.display='none'" src="WorkflowImage.aspx" />
            </td>
        </tr>
    </table>
</asp:Content>
