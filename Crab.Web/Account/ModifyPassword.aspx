<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ModifyPassword.aspx.cs" Inherits="ModifyPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ChangePassword ID="ChangePassword1" runat="server" ChangePasswordButtonText="Change"
        ContinueDestinationPageUrl="~/Default.aspx" CssClass="PwdChangeForm" ChangePasswordTitleText="">
        <LabelStyle HorizontalAlign="Left" Width="125px" />
        <TitleTextStyle Height="40px" HorizontalAlign="Left" VerticalAlign="Top" />
        <TextBoxStyle CssClass="Textbox" />
        <CancelButtonStyle CssClass="buttonstyle" />
        <ChangePasswordButtonStyle CssClass="buttonstyle" />
    </asp:ChangePassword>
</asp:Content>

