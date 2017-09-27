<%@ Page Title="" Language="C#" MasterPageFile="~/WebpartMasterPage.master" AutoEventWireup="true" CodeFile="Workplace.aspx.cs" Inherits="Workplace" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderWebPart" Runat="Server">
    <table align="left" cellpadding="0" cellspacing="0" width="100%">
        <tr valign="top">
            <td width="100%">
                <asp:WebPartZone ID="Zone1" runat="server" TitleBarVerbButtonType="Button"
                    Width="100%">
                    <ZoneTemplate>
                        <asp:Panel ID="pnlExportContract" runat="server" title="Shipping Export Contract">
                            <a href="SearchExportContract.aspx?owner=me">
                                <asp:Label ID="lblMyExportContractsTip" Text="My Contracts" runat="server"></asp:Label>
                            </a>
                            <br />
                            <a href="SearchExportContract.aspx">
                                <asp:Label ID="lblSearchExportContractTip" runat="server" Text="Search Contracts"></asp:Label>
                            </a>
                            <br />
                            <a href="ExportContractCreate.aspx">
                                <asp:Label ID="lblCreateExportContractTip" Text="Create Contract" runat="server"></asp:Label>
                            </a>
                        </asp:Panel>
                        <asp:Panel ID="pnlBasicInfo" runat="server" title="Basic Information">
                            <a href="Customer.aspx">
                                <asp:Label ID="lblCustomers" Text="Customers&Clients" runat="server"></asp:Label>
                            </a>
                        </asp:Panel>
                    </ZoneTemplate>
                </asp:WebPartZone>
            </td>
            <td valign="top">
                <asp:WebPartZone ID="Zone2" runat="server" PartChromeType="none"
                    TitleBarVerbButtonType="Button">
                     <ZoneTemplate>
                        <asp:AdRotator runat="server" ID="adBottom" EnableViewState="false"
                        AdvertisementFile ="~/Ads/AdSmall.xml" Target="_blank" />
                    </ZoneTemplate>
                    <MenuPopupStyle GridLines="Vertical" />
                </asp:WebPartZone>
            </td>
        </tr>
    </table>
</asp:Content>

