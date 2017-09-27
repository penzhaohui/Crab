<%@ Page AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" Language="C#" MasterPageFile="WebpartMasterPage.master"
    Title="Crab" %>

<asp:Content ID="Content1" runat="Server" ContentPlaceHolderID="ContentPlaceHolderWebPart">
    <table align="left" cellpadding="0" cellspacing="0" width="100%">
        
        <tr valign="top">
            <td width="100%">
                <asp:WebPartZone ID="LeftZone" runat="server" Width="100%" TitleBarVerbButtonType="Button">
                    <ZoneTemplate>
                        <asp:Panel ID="PanelHomeWorkPlace" runat="server" title="Workplace">
                            <a href="Workplace/Workplace.aspx">
                                <asp:Label ID="lblWorkplaceTip" runat="server" Text="Workplace"></asp:Label>
                            </a>
                            <br />
                            <a href="Workplace/ExportContractCreate.aspx">
                                <asp:Label ID="lblCreateExportContractTip" runat="server" Text="Create ExportContract"></asp:Label>
                            </a>
                            <br />
                            <a href="Workplace/SearchExportContract.aspx">
                                <asp:Label ID="lblSearchExportContractTip" runat="server" Text="Search ExportContract"></asp:Label>
                            </a>
                            <br />
                            <a href="Workplace/Customer.aspx">
                                <asp:Label ID="lblCustomerInformationTip" runat="server" Text="Customer Information"></asp:Label>
                            </a>
                            <br />
                        </asp:Panel>
                        <asp:Panel ID="PanelMyWorkPlace" runat="server" title="My Query">
                            <a href="Workplace/SearchExportContract.aspx?owner=me">
                                <asp:Label ID="lblMyExportContractTip" runat="server" Text="My Export Contract"></asp:Label>
                            </a>
                        </asp:Panel>
                        <asp:Panel ID="PanelAdmin" runat="server" Height="50px" title="Admin" Width="125px">
                            <a href="Admin/Users/ManageUsers.aspx">
                                <asp:Label ID="lblManageUsersTip" runat="server" Text="Manage Users"></asp:Label>
                            </a>
                            <br>
                            </br>
                            <a href="Admin/Customization/CustomizeDataModel.aspx">
                                <asp:Label ID="lblDataModelTip" Text="Customize Data Model" runat="server"></asp:Label></a>
                            <br />
                            <a href="Admin/Customization/CustomizeWorkflow.aspx">
                                <asp:Label ID="lblCustomizeWorkflow" Text="Customize Workflow" runat="server"></asp:Label></a>
                        </asp:Panel>
                    </ZoneTemplate>
                    <MenuPopupStyle GridLines="Vertical" />
                </asp:WebPartZone>
            </td>
            <td>
                <asp:WebPartZone ID="RightZone" runat="server" PartChromeType="None">
                    <ZoneTemplate>
                        <asp:AdRotator runat="server" ID="adBottom" EnableViewState="false"
                        AdvertisementFile ="~/Ads/AdSmall.xml" Target="_blank" />
                    </ZoneTemplate>
                </asp:WebPartZone>
            </td>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>
