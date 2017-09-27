using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Crab.Runtime.Contract;
using Crab.DataModel.Common;
using Crab.Services.Proxy;

public partial class Admin_Customization_CustomizeDataModel : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            foreach (EntityDef entityDef in MetadataProxy.GetEntityDefs())
            {
                ListItem listItem = new ListItem(entityDef.Caption, entityDef.Id.ToString());
                drpDataEntities.Items.Add(listItem);
            }
        }
    }

    #region LoadGrid
    protected void drpDataEntities_SelectedIndexChanged(object sender, EventArgs e)
    {
        //remove empty item
        if (drpDataEntities.Items[0].Value.Trim() == "")
            drpDataEntities.Items.RemoveAt(0);

        //display gridviews
        panelGrids.Visible = true;
        panelEdit.Visible = false;

        Guid entityDefId = new Guid(drpDataEntities.SelectedItem.Value);
        EntityFieldDef[] fieldDefs = MetadataProxy.GetFieldDefs(entityDefId);
        EntityFieldDef[] predefined;
        EntityFieldDef[] extended;
        ClassifyFieldDef(fieldDefs, out predefined, out extended);

        gridSharedFields.DataSource = predefined;
        gridSharedFields.DataBind();

        gridCustomizedFields.DataSource = extended;
        gridCustomizedFields.DataBind();
    }
    #endregion

    private void ClassifyFieldDef(EntityFieldDef[] fieldDefs, out EntityFieldDef[] predefined, out EntityFieldDef[] extended)
    {
        List<EntityFieldDef> predefinedList = new List<EntityFieldDef>();
        List<EntityFieldDef> extendedList = new List<EntityFieldDef>();
        foreach (EntityFieldDef field in fieldDefs)
        {
            if (field.IsShared)
                predefinedList.Add(field);
            else
                extendedList.Add(field);
        }
        predefined = predefinedList.ToArray();
        extended = extendedList.ToArray();
    }
    #region Show Edit Form

    protected void btnAddField_Click(object sender, EventArgs e)
    {
        panelGrids.Visible = false;
        panelEdit.Visible = true;
        btnAdd.Visible = true;
        btnEdit.Visible = false;

        txtName.Text = "";
        txtCaption.Text = "";
        txtLength.Text = "";
        chkNullable.Checked = false;
        this.LoadNodeTypeDropList(-1);

        txtName.Enabled = true;
        txtLength.Enabled = true;
        
        drpDataType.Enabled = true;
        chkNullable.Enabled = false;
        chkNullable.Checked = true;
        chkIsExtension.Checked = true;
    }

    protected void gridSharedFields_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Guid entityDefId = new Guid(drpDataEntities.SelectedItem.Value);
        EntityFieldDef field = MetadataProxy.GetFieldDefById((Guid)gridSharedFields.DataKeys[e.NewEditIndex].Value);

        txtFieldId.Value = field.Id.ToString();
        txtName.Text = field.Name;
        txtCaption.Text = field.Caption;
        txtLength.Text = field.Length.ToString();
        chkNullable.Checked = field.Nullable;
        this.LoadNodeTypeDropList((int)field.DataType);

        panelGrids.Visible = false;
        panelEdit.Visible = true;
        btnAdd.Visible = false;
        btnEdit.Visible = true;

        txtName.Enabled = false;
        drpDataType.Enabled = false;
        txtLength.Enabled = false;
        chkNullable.Enabled = false;
        chkIsExtension.Checked = false;
    }

    protected void gridCustomizedFields_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Guid entityDefId = new Guid(drpDataEntities.SelectedItem.Value);
        EntityFieldDef field = MetadataProxy.GetFieldDefById((Guid)gridSharedFields.DataKeys[e.NewEditIndex].Value);

        txtFieldId.Value = field.Id.ToString();
        txtName.Text = field.Name;
        txtCaption.Text = field.Caption;
        txtLength.Text = field.Length.ToString();
        chkNullable.Checked = field.Nullable;
        this.LoadNodeTypeDropList((int)field.DataType);

        panelGrids.Visible = false;
        panelEdit.Visible = true;
        btnAdd.Visible = false;
        btnEdit.Visible = true;

        txtName.Enabled = false;
        txtLength.Enabled = true;
        drpDataType.Enabled = false;
        chkNullable.Enabled = true;
        chkIsExtension.Checked = true;
    }
    #endregion

    #region Save Add/Edit
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        MetadataProxy.UpdateEntityFieldDef(new Guid(txtFieldId.Value), txtCaption.Text.Trim());
        drpDataEntities_SelectedIndexChanged(null, null);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        //TODO: Check Unique
        EntityFieldDef newField = MetadataProxy.AddEntityFieldDef(
            new Guid(drpDataEntities.SelectedItem.Value),
            txtName.Text.Trim(),
            txtCaption.Text.Trim(),
            (DataTypes)int.Parse(drpDataType.SelectedItem.Value),
            string.IsNullOrEmpty(txtLength.Text.Trim()) ? 0 : int.Parse(txtLength.Text.Trim()));

        drpDataEntities_SelectedIndexChanged(null, null);
    }
    #endregion

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        panelGrids.Visible = true;
        panelEdit.Visible = false;
    }

    private void LoadNodeTypeDropList(int selectedValue)
    {
        drpDataType.Items.Clear();
        Array values = Enum.GetValues(typeof(DataTypes));

        foreach (int value in values)
	    {
            ListItem listItem = new ListItem(((DataTypes)value).ToString(), value.ToString());
		    drpDataType.Items.Add(listItem);

            if (selectedValue == value)
            {
                listItem.Selected = true;
            }
	    }
    }

    protected void gridCustomizedFields_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        MetadataProxy.DeleteEntityFieldDef((Guid)gridCustomizedFields.DataKeys[e.RowIndex].Value);
        drpDataEntities_SelectedIndexChanged(null, null);
    }

    protected void gridCustomizedFields_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow row = e.Row;
        if (row.RowType != DataControlRowType.DataRow)
            return;
        TableCell cell = row.Cells[row.Cells.Count - 1];
        LinkButton linkButton = cell.Controls[0] as LinkButton;
        //Add confirm information to the delete link button
        if (linkButton != null)
            linkButton.Attributes.Add("onclick",
                string.Format("javascript:return confirm('{0}');",
                Resources.GlobalResources.ConfirmDelete));
    }
}
