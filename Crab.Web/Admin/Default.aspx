<%@ Page Title="" Language="C#" MasterPageFile="~/WebpartMasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Admin_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderWebPart" Runat="Server">
     <table align="left" cellpadding="0" cellspacing="0" width="100%">
        <tr valign="top">
            <td width="100%">
                <asp:WebPartZone ID="Zone1" runat="server"  TitleBarVerbButtonType="Button"
                    Width="100%">
                    <ZoneTemplate>
                        <asp:Panel ID="Panel1" runat="server" title="Customization">
                            <a href="Customization/CustomizeDataModel.aspx">
                                <asp:Label ID="lblDataModelTip" Text="Customize Data Model" runat="server"></asp:Label></a>
                            <br />
                            <a href="Customization/CustomizeWorkflow.aspx">
                                <asp:Label ID="lblCustomizeWorkflow" Text="Customize Workflow" runat="server"></asp:Label></a>
                        </asp:Panel>
                        <asp:Panel ID="Panel2" runat="server"  title="Membership">
                            <a href="Users/ManageUsers.aspx">
                                <asp:Label ID="lblManageUsersTip" Text="Manage Users" runat="server"></asp:Label></a>
                            <br />
                            <a href="Users/CreateUser.aspx">
                                <asp:Label ID="lblCreateUserTip" Text="Create User" runat="server"></asp:Label></a>
                        </asp:Panel>
                    </ZoneTemplate>
                </asp:WebPartZone>
            </td>
            <td>
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

