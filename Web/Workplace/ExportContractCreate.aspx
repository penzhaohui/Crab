<%@ Page AutoEventWireup="true" CodeFile="ExportContractCreate.aspx.cs" Inherits="Workplace_ExportContractCreate" Language="C#" MasterPageFile="~/MasterPage.master" %>

<asp:Content ID="Content1" runat="Server" ContentPlaceHolderID="ContentPlaceHolder1">

    <script language="javascript">
        function checkKey(){
            if (window.event) {
                if (event.keyCode == 13) {
                    var nodes = document.getElementsByTagName("INPUT");
                    for (var i = 0; i < nodes.length; i++) {
                        if (nodes[i].type == "submit") {
		                    event.cancelBubble = true;
		                    event.returnValue = false;
                            nodes[i].click();
                            break;
                        }
                    }
                }
            }
        }
    </script>

    <table>
        <tr>
            <td>
                <asp:Label ID="lblExportContractNumber" runat="server" Text="Input a contract number: "></asp:Label></td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="ExportContractNumber" runat="server" MaxLength="50" onkeydown="javascript:checkKey();" Width="150"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ExportContractNumber" Display="static" EnableClientScript="true" EnableTheming="true" EnableViewState="true" ErrorMessage="*"></asp:RequiredFieldValidator>
                <asp:Label ID="LabelErrMsg" runat="server" ForeColor="Red" Text="The Export Contract with the Number has already existed!" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="ButtonCreate" runat="server" OnClick="ButtonCreate_Click" Text="Create" Width="70" /></td>
        </tr>
    </table>
</asp:Content>
