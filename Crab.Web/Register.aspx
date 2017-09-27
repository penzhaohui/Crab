<%@ Page Title="" Language="C#" MasterPageFile="~/SimpleMasterPage.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table align="center" cellpadding="10" cellspacing="0" border="0">
        <tr>
            <td>
                <asp:label runat="server" ID="lblTitle" Text="Sign up your tenant account now !" Font-Bold="true"/>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Wizard ID="Wizard1" runat="server" ActiveStepIndex="0" 
                    OnNextButtonClick="Wizard1_NextButtonClick">
                    <SideBarStyle VerticalAlign="Top" Width="170px" />
                    <StepStyle Width="400px" />
                    <WizardSteps>
                        <asp:WizardStep runat="server" Title="1. Enter Tenant Information">
                            <table border="0" cellpadding="4" cellspacing="0" style="margin:15px" class="NonGridTable">
                                <tr>
                                    <td width="115" align="right">
                                        <asp:Label ID="lblTenantNameIInTenantInfoStep" Text="Tenant Name:" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtTenantName" Width="150px" Height="20px" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_Tenant_Name" runat="server" ControlToValidate="txtTenantName" ForeColor="Red" 
                                            ErrorMessage="Tenant name is required!" Text="*"></asp:RequiredFieldValidator>
                                        &nbsp;<asp:RegularExpressionValidator ID="REV_Tenant_Fomrat" runat="server"
                                            ControlToValidate="txtTenantName" ForeColor="Red"  Text="*" ErrorMessage="Tenant name must have alphabets and digiters. The initial character must be alphabet."
                                            ValidationExpression="^[a-z,A-Z]+[0-9]*[a-z,A-Z]*[0-9]*" Width="8px"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDescriptionInTenantInfoStep" runat="server" Text="Display Name:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtDescription" runat="server" Width="150px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_Tenant_Description" runat="server" ForeColor="Red" ErrorMessage="Display name is required!"
                                            ControlToValidate="txtDescription" Text="*"></asp:RequiredFieldValidator></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblLicenseCountInTenantInfoStep" Text="License Count:" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtLicenseNum" runat="server" Text="10" Width="60px" Height="20px" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_License_Count" runat="server" ForeColor="Red"  ErrorMessage="License count is required!"
                                            ControlToValidate="txtLicenseNum" Text="*" />
                                        <asp:RangeValidator ID="RV_LicenseNum" runat="server" ControlToValidate="txtLicenseNum" ForeColor="Red" 
                                            ErrorMessage="License count must be between 1-100" MaximumValue="100" MinimumValue="1" Type="Integer"
                                            Text="*"></asp:RangeValidator></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblConcurrencyCountInTenantInfoStep" Text="Concurrency Count:" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtMaxConcurrency" runat="server" Text="5" Width="60px" Height="20px" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RFV_Concurrency_Count" runat="server" ForeColor="Red" ErrorMessage="Concurrency count is required!"
                                            ControlToValidate="txtMaxConcurrency" Text="*"/>
                                        <asp:RangeValidator ID="RVMaxConcurrency" runat="server" ControlToValidate="txtMaxConcurrency" ForeColor="Red" 
                                            ErrorMessage="Concurrency count must be between 1-10" MaximumValue="10" MinimumValue="1" Type="Integer"
                                            Text="*"></asp:RangeValidator></td>
                                </tr>
                                <tr>
                                    <td colspan="2"><div style="height: 1px; overflow: hidden; background: #cccccc; margin: 10px"></div></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label1" runat="server" Text="Contact:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtContact" runat="server" Width="150">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label2" runat="server" Text="Fax:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtFax" runat="server" Width="150">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label3" runat="server" Text="Telephone:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtTelephone" runat="server" Width="150">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label4" runat="server" Text="Mobile:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtMobile" runat="server" Width="150">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label5" runat="server" Text="Email:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtTenantEmail" runat="server" Width="150">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label6" runat="server" Text="Website:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtWebsite" runat="server" Width="150">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label7" runat="server" Text="City:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtCity" runat="server" Width="150">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label8" runat="server" Text="Address:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtAddress" runat="server" Width="150">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="Label9" runat="server" Text="Zip Code:"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtZipCode" runat="server" Width="150">
                                        </asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 50px;">
                                        <asp:ValidationSummary ID="ValidationSummary" runat="server" ForeColor="Red"  />
                                        <asp:Label runat="server" ID="Message" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:WizardStep>
                        <asp:WizardStep runat="server" Title="2. Enter Admin's Settings">
                            <table border="0" cellpadding="4" cellspacing="0" class="NonGridTable" style="margin: 5px">
                                <tr>
                                    <td width="115" align="right">
                                        <asp:Label ID="lblUsernameInAdminUserInfoStep" Text="Username:" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtAdmin" runat="server" Text="admin" Width="120px" Height="20px" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="*" ForeColor="Red" ErrorMessage="Username is required!"
                                            ControlToValidate="txtAdmin"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtAdmin" Text="*" ForeColor="Red"
                                            ErrorMessage="User name must have alphabets and digiters. The initial character must be alphabet." ValidationExpression="^[a-z,A-Z]+[0-9]*[a-z,A-Z,\.]*[0-9]*"></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblPasswordInAdminUserInfoStep" Text="Password:" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtAdminPassword" runat="server" TextMode="Password" Width="120px" Height="20px" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtAdminPassword" ForeColor="Red" Text="*"
                                            ErrorMessage="Password is required!"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblConfirmInAdminUserInfoStep" Text="Confirm Password:" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" Width="120px" Height="20px" ></asp:TextBox>
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtAdminPassword" ForeColor="Red" Text="*"
                                            ControlToValidate="txtConfirmPassword" ErrorMessage="The passwords you enterred are not the same!"></asp:CompareValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblEmailInAdminUserInfoStep" Text="Email:" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtEmail" runat="server" Width="120px" Height="20px" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red" Text="*" ErrorMessage="Email is required!"
                                            ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ForeColor="Red" Text="*" ControlToValidate="txtEmail"
                                            ErrorMessage="Incorrect format of email!" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                           ></asp:RegularExpressionValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="height: 50px;">
                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" />
                                        <asp:Label ID="lblMessageNext" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:WizardStep>
                        <asp:WizardStep runat="server" AllowReturn="False" StepType="Finish" Title="3. Succeed!">
                            <font style="color:DodgerBlue;width:150px;"><strong><asp:Label runat="server" ID="lblRegisterSucceed" Text="Register Successfully" /></strong></font>
                            <table border="0" cellpadding="4" cellspacing="0" class="NonGridTable" style="margin: 15px">
                                <tr>
                                    <td width="115" align="right">
                                        <asp:Label ID="lblTenantNameInFinishStep" Text="Tenant Name:" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblTenantName" ForeColor="#FF6600" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblDescriptionInFinishStep" Text="Display Name:" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblDesc" runat="server" ForeColor="#FF6600"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblLicenseCountInFinishStep" Text="License Count:" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblLicenseNum" runat="server" ForeColor="#FF6600"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblConcurrencyInFinishStep" Text="Concurrency Count:" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblMaxCon" runat="server" ForeColor="#FF6600"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lblUsernameInFinishStep" Text="Username:" runat="server"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblUserName" runat="server" ForeColor="#FF6600"></asp:Label></td>
                                </tr>

                            </table>
                        </asp:WizardStep>
                    </WizardSteps>
                    <StartNavigationTemplate>
                        <div style="width:100%;" align=center>
                        <asp:Button ID="StartBackButton" runat="server" CommandName="MoveNext" Text="Back"
                            OnClick="StartBackButton_Click" CausesValidation="false" />
                        <asp:Button ID="StartNextButton" runat="server" CommandName="MoveNext" Text="Next" />
                        </div>
                    </StartNavigationTemplate>
                    <StepNavigationTemplate>
                        <div align="center" style="width: 100%;">
                        
                        <asp:Button ID="StepPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                            Text="Previous" />
                        <asp:Button ID="StepNextButton" runat="server" CommandName="MoveNext" Text="Next" />
                        </div>
                    </StepNavigationTemplate>
                    <SideBarTemplate>
                        <asp:DataList ID="SideBarList" runat="server">
                            <SelectedItemStyle Font-Bold="True" />
                            <ItemStyle Height="21px" />
                            <ItemTemplate>
                                <asp:LinkButton ID="SideBarButton" runat="server" BorderWidth="0px"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:DataList>
                    </SideBarTemplate>
                    <FinishNavigationTemplate>
                        <div style="width: 100%; margin-right:100px;">
                        
                        <asp:Button ID="FinishPreviousButton" runat="server" CausesValidation="False" CommandName="MovePrevious"
                            Text="Previous" Visible="False" />&nbsp;
                            <asp:LinkButton ID="lbtnSucc" runat="server" OnClick="lbtnSucc_Click">Sign in now</asp:LinkButton></div>
                    </FinishNavigationTemplate>
                </asp:Wizard>
            </td>
        </tr>
    </table>
</asp:Content>

