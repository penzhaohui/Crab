<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SearchExportContract.aspx.cs" Inherits="SearchExportContract" %>
<%@ Register Src="../UserControls/Pagination.ascx" TagName="Pagination" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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

    <asp:ScriptManager ID="ScriptManager" runat="server"/>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <TABLE class="NonGridTable" cellSpacing=0 cellPadding=4 border=0 runat="server" id="ConditionPanel"><TBODY><TR><TD><asp:Label id="Label2" runat="server" Text="Status:"></asp:Label></TD><TD><asp:DropDownList id="DropDownListStatus" runat="server" Width="150">
                </asp:DropDownList></TD></TR>

                <TR><TD><asp:Label id="lbMsg" runat="server" ForeColor="red" Text="No data!" Visible="false"></asp:Label></TD><TD><asp:Button id="btnQuery" onclick="btnQuery_Click" runat="server" Text="Search" Width="70"></asp:Button></TD></TR></TBODY></TABLE><BR />
    <asp:GridView id="dGrid" runat="server" AllowPaging="True" width="100%" AutoGenerateColumns="False" OnPageIndexChanging="dGrid_PageIndexChanging">
    <HeaderStyle Wrap="False"/>
        <Columns>
            <asp:BoundField DataField="Number" HeaderText="Number">
                <ItemStyle Width="100px" />
            </asp:BoundField>
            <asp:BoundField DataField="Status" HeaderText="Status">
                <ItemStyle Width="80px"/>
            </asp:BoundField>
            <asp:BoundField DataField="Creator" HeaderText="Creator">
                <ItemStyle Width="80px"/>
            </asp:BoundField>
            <asp:HyperLinkField DataNavigateUrlFields="ObjectId" DataNavigateUrlFormatString="~/Workplace/ExportContractView.aspx?objectid={0}" HeaderText="View" Text="View">
                <ItemStyle Width="50px" />
            </asp:HyperLinkField>
            <asp:HyperLinkField DataNavigateUrlFields="ProcessId" DataNavigateUrlFormatString="~/Workplace/WorkflowView.aspx?processid={0}" HeaderText="ViewWorkflow" Text="View Workflow">
                <ItemStyle Width="80px" />
            </asp:HyperLinkField>
        </Columns>
    </asp:GridView> 
</ContentTemplate>
    </asp:UpdatePanel>
    <%-- Panel used control table Visable or Not Visalbe It is not the stand frame --%>
</asp:Content>

