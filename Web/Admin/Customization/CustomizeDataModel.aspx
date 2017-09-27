<%@ Page AutoEventWireup="true" CodeFile="CustomizeDataModel.aspx.cs" Inherits="Admin_Customization_CustomizeDataModel"
    Language="C#" MasterPageFile="~/MasterPage.master" Title="Custom Data Model" %>

<asp:Content ID="Content1" runat="Server" ContentPlaceHolderID="ContentPlaceHolder1">
    <div style="margin-top: 4px">
        Select an entity type: &nbsp;
        <asp:DropDownList ID="drpDataEntities" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpDataEntities_SelectedIndexChanged">
            <asp:ListItem Value="">---Please choose the entity type---</asp:ListItem>
        </asp:DropDownList>
    </div>
    <br />
    <asp:Panel ID="panelGrids" runat="server" Visible="false">
        <asp:Label ID="lblSharedFields" runat="server" CssClass="t1">Shared Fields:</asp:Label><br />
        <div style="width: 500px">
            <asp:GridView ID="gridSharedFields" runat="server" AutoGenerateColumns="false" DataKeyNames="ID"
                OnRowEditing="gridSharedFields_RowEditing">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Name" />
                    <asp:BoundField DataField="Caption" HeaderText="Caption" />
                    <asp:CheckBoxField DataField="Nullable" HeaderStyle-Width="60" HeaderText="Nullable"
                        ItemStyle-HorizontalAlign="center" />
                    <asp:BoundField DataField="Length" HeaderStyle-Width="60" HeaderText="Length" ItemStyle-HorizontalAlign="center" />
                    <asp:CommandField ButtonType="Link" DeleteText=" Delete " EditText=" Edit " HeaderStyle-Width="70"
                        ItemStyle-HorizontalAlign="center" ShowDeleteButton="false" ShowEditButton="true" />
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <asp:Label ID="lblCustomizedFields" runat="server" CssClass="t1">Extension Fields:</asp:Label>
        &nbsp; &nbsp;
        <asp:Button ID="btnAddField" runat="server" Text="Add New" OnClick="btnAddField_Click" />
        <br />
        <div style="width: 500px">
            <asp:GridView ID="gridCustomizedFields" runat="server" AutoGenerateColumns="false"
                DataKeyNames="ID" OnRowEditing="gridCustomizedFields_RowEditing" OnRowDeleting="gridCustomizedFields_RowDeleting" OnRowDataBound="gridCustomizedFields_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Name" />
                    <asp:BoundField DataField="Caption" HeaderText="Caption" />
                    <asp:CheckBoxField DataField="Nullable" HeaderStyle-Width="60" HeaderText="Nullable"
                        ItemStyle-HorizontalAlign="center" />
                    <asp:BoundField DataField="Length" HeaderStyle-Width="60" HeaderText="Length" ItemStyle-HorizontalAlign="center" />
                    <asp:CommandField ButtonType="Link" DeleteText="Edit" EditText="Edit" HeaderStyle-Width="70"
                        ItemStyle-HorizontalAlign="center" ShowDeleteButton="false" ShowEditButton="true"/>
                    <asp:CommandField ButtonType="Link" DeleteText="Delete" EditText="Edit" HeaderStyle-Width="70"
                        ItemStyle-HorizontalAlign="center" ShowDeleteButton="true" ShowEditButton="false"/>
                </Columns>
            </asp:GridView>
        </div>
        <br />
        <br />
    </asp:Panel>
    <asp:Panel ID="panelEdit" runat="server" Style="width: 100%" Visible="false">
        <!-- Length NodeType Caption Id Nullable ColumnName Name  -->
        Edit Field Metadata:<br />
        <br />
        <table border="0" cellpadding="3" cellspacing="0" width="100%">
            <tr>
                <td width="80">
                    Field Name:</td>
                <td>
                    <asp:TextBox ID="txtName" runat="server" Width="150px">
                    </asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredValidaor1" runat="server" text="*"
                        ErrorMessage = "Field name is required!" ControlToValidate="txtName" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtName"
                        ErrorMessage="Only letters and numbers can be used as field name" ToolTip="Can not be empty and can use letters only"
                        ValidationExpression="^[0-9a-zA-Z]+$"></asp:RegularExpressionValidator></td>
            </tr>
            <tr>
                <td>
                    Caption:</td>
                <td>
                    <asp:TextBox ID="txtCaption" runat="server" Width="150px">
                    </asp:TextBox><asp:RequiredFieldValidator ID="RequiredValidaor2" runat="server" text="*"
                        ErrorMessage = "Caption is required!" ControlToValidate="txtCaption" /></td>
                <td style="width: 7px">
                </td>
            </tr>
            <tr>
                <td>
                    Data Type:
                </td>
                <td>
                    <asp:DropDownList ID="drpDataType" runat="server" Width="150px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                Length: </td>
                <td>
                    <asp:TextBox ID="txtLength" runat="server" Width="150px">
                    </asp:TextBox>
                    <asp:CompareValidator ID="CompareValidator1" runat="server"
                        ControlToValidate="txtLength" ErrorMessage="The value must be between 0 and 256!" ToolTip="The value must be between 0 and 256!"
                        Type="Integer" ValueToCompare="256" Operator="LessThanEqual" CssClass="c"></asp:CompareValidator></td>
            </tr>
            <tr>
                <td>
                    Nullable</td>
                <td>
                    <asp:CheckBox ID="chkNullable" runat="server" Text="Nullable" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    <asp:HiddenField ID="txtFieldId" runat="server" />
                    <asp:CheckBox ID="chkIsExtension" runat="server" Visible="false" />
                    <asp:Button ID="btnEdit" runat="server" Text="Edit" OnClick="btnEdit_Click" />
                    <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
</asp:Content>
