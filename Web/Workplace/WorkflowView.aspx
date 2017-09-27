<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="WorkflowView.aspx.cs" Inherits="Workplace_WorkflowView" Title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table align="center" border="0" cellpadding="4" cellspacing="0" class="NonGridTable" width="100%">
        <tr>
            <td align="center">
                <img id="imageLoading" alt="" src="../Images/loading.gif" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Image ID="processImage" runat="server" 
                   EnableViewState="false" 
                   ImageUrl="~/Workplace/ProcessImage.aspx?processId={0}"/>
            </td>
        </tr>
    </table>
</asp:Content>