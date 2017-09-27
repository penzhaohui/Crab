<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CreateUser.aspx.cs" Inherits="Admin_Users_CreateUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" LabelStyle-HorizontalAlign="Left"  SideBarStyle-HorizontalAlign="Left" StepStyle-HorizontalAlign="Left" TitleTextStyle-HorizontalAlign="Left" Width="400px">
        <WizardSteps>
            <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
                <ContentTemplate>
                    <table border="0" cellpadding="4" cellspacing="0" class="NonGridTable" width="100%">
                        
                        <tr>
                            <td>
                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName" Text="User Name:"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="UserName" runat="server" Width="145px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ForeColor="Red" ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="CreateUserWizard1">
                                    <asp:Label ID="lblValidatorTip0" runat="server" Text="*"></asp:Label></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblInRolesTip" runat="server" Text="Roles:"></asp:Label></td>
                            <td>
                                <asp:CheckBoxList ID="cblBoundRoles" runat="server" RepeatDirection="Horizontal">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password" Text="Password:"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="Password" runat="server" TextMode="Password" Width="145px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ForeColor="Red" ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="CreateUserWizard1">
                                    <asp:Label ID="lblValidatorTip1" runat="server" Text="*"></asp:Label></asp:RequiredFieldValidator>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Password" ErrorMessage="*"></asp:RequiredFieldValidator></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword" Text="Confirm Password:"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password" Width="145px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword" ForeColor="Red" ErrorMessage="Confirm Password is required." ToolTip="Confirm Password is required." ValidationGroup="CreateUserWizard1">
                                    <asp:Label ID="lblValidatorTip2" runat="server" Text="*"></asp:Label></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email" Text="E-mail:"></asp:Label></td>
                            <td>
                                <asp:TextBox ID="Email" runat="server" Width="145px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="valRequiredEmail" runat="server" ControlToValidate="Email" ForeColor="Red" ErrorMessage="*" ToolTip="Email is required." ValidationGroup="CreateUserWizard1" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationGroup="CreateUserWizard1" ControlToValidate="Email" ErrorMessage="*" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Width="1px"></asp:RegularExpressionValidator></td>
                        </tr>
                        <tr>
                            <tdcolspan><asp:CompareValidator id="PasswordCompare" runat="server" ValidationGroup="CreateUserWizard1" ForeColor="Red" ErrorMessage="The Password and Confirmation Password must match." ControlToValidate="ConfirmPassword" Display="Dynamic" ControlToCompare="Password"></asp:CompareValidator><asp:CompareValidator id="ComfirmPasswordCompare" runat="server" ValidationGroup="CreateUserWizard1" ErrorMessage="The Password and Confirmation Password must match." ControlToValidate="ConfirmPassword" Display="Dynamic" ControlToCompare="Password"></asp:CompareValidator>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="ErrorMessage" runat="server" ForeColor="Red"></asp:Label>&nbsp;</td>
                        </tr>
                    </table>
                </ContentTemplate>
                <CustomNavigationTemplate>
                    <table border="0" cellpadding="4" cellspacing="0" class="NonGridTable" width="100%">
                        <tr>
                            <td colspan="0" align = "left">
                                <asp:Button ID="btnCreateUser" runat="server" OnClick="btnCreateUser_Click" Text="Create User" ValidationGroup="CreateUserWizard1" />
                                <asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" Text="Create User" ValidationGroup="CreateUserWizard1" Visible="False" />
                            </td>
                        </tr>
                    </table>
                </CustomNavigationTemplate>
            </asp:CreateUserWizardStep>
            <asp:WizardStep ID="CreateUserWizardStep2" runat="server" AllowReturn="False" StepType="Step" Title="Continue">
                <asp:Label ID="lblCongratulationsTheaccountTip" runat="server" Text="Congratulations! The account"></asp:Label>
                <asp:Label ID="Account" runat="server" Text="has been created successfully!"></asp:Label>
            </asp:WizardStep>
        </WizardSteps>
        <StepNavigationTemplate>
          <div align = "left">
            <br />
            <asp:Button ID="ConProfile" runat="server" OnClick="ConProfile_Click" Text="Edit Profile" />
            <asp:Button ID="ConUsers" runat="server" OnClick="ConUsers_Click" Text="Create User" />
            <asp:Button ID="btnManageUsers" runat="server" Text="Manage Users" OnClick="btnManageUsers_Click" />
           </div>
        </StepNavigationTemplate>
        <SideBarStyle HorizontalAlign="Left" />
        <TitleTextStyle HorizontalAlign="Left" />
        <LabelStyle HorizontalAlign="Left" />
        <StepStyle HorizontalAlign="Left" />
    </asp:CreateUserWizard>
    <asp:Label ID="lblReturnMessage" runat="server" ForeColor="Red" ></asp:Label>
</asp:Content>

