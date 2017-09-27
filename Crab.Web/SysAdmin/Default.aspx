<%@ Page Title="" Language="C#" MasterPageFile="~/WebpartMasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderWebPart" runat="Server">
    <table align="left" cellpadding="0" cellspacing="0" width="100%">
        <tr valign="top">
            <td width="100%">
                <asp:WebPartZone ID="WPZoneMain" runat="server" TitleBarVerbButtonType="button"
                    Width="100%">
                    <ZoneTemplate>
                        <asp:Panel ID="PnlManageTenant" runat="server" title="Manage tenant" Width="100%">
                            <table>
                                <tr>
                                    <td rowspan="3">
                                        <img alt="tenant" src="../Images/user.gif" /></td>
                                    <td>
                                        <a href="Tenants/ManageTenants.aspx">Manage tenants</a></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ZoneTemplate>
                    <MenuPopupStyle GridLines="Vertical" />
                </asp:WebPartZone>
            </td>
            <td valign="top">
                <asp:WebPartZone ID="RightZone" runat="server" PartChromeType="None"
                    Width="100%">
                    <ZoneTemplate>
                        <asp:AdRotator runat="server" ID="adBottom" EnableViewState="false"
                            AdvertisementFile="~/Ads/AdSmall.xml" Target="_blank" />
                    </ZoneTemplate>
                </asp:WebPartZone>
            </td>
        </tr>
    </table>
</asp:Content>

