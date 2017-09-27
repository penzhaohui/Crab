<%@ Page AutoEventWireup="true" CodeFile="ModifyPassword.aspx.cs" Inherits="ModifyPassword"
    Language="C#" MasterPageFile="~/MasterPage.master" Title="Untitled Page" %>

<asp:Content ID="Content1" runat="Server" ContentPlaceHolderID="ContentPlaceHolder1">
    <asp:ChangePassword ID="ChangePassword1" runat="server" ChangePasswordButtonText="Change"
        ContinueDestinationPageUrl="~/Default.aspx" CssClass="PwdChangeForm" ChangePasswordTitleText="">
        <LabelStyle HorizontalAlign="Left" Width="125px" />
        <TitleTextStyle Height="40px" HorizontalAlign="Left" VerticalAlign="Top" />
        <TextBoxStyle CssClass="TextBox" />
        <CancelButtonStyle CssClass="ButtonStyle" />
        <ChangePasswordButtonStyle CssClass="ButtonStyle" />
    </asp:ChangePassword>
</asp:Content>
