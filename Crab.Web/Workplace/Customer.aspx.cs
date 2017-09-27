using Crab.Business.Contract;
using Crab.Services.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Workplace_Customer : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //this.gridCustomer.DataSource = new Customer[] { new Customer() };
            //this.gridCustomer.DataBind();
            Search();
            if (!CanEdit())
            {
                btnAdd.Visible = false;
                btnDelete.Visible = false;
            }
        }

        this.lblError.Text = "";
    }


    private Customer GetFromControl()
    {
        Customer customer = new Customer();
        customer.Name = this.txtCustomerName.Text;
        customer.PhoneNumber = this.txtPhoneNumber.Text;
        customer.Address = this.txtAddress.Text;
        customer.Description = this.txtDescription.Text;

        if (!string.IsNullOrEmpty(this.txtId.Value))
            customer.Id = new Guid(this.txtId.Value);
        else
            customer.Id = Guid.NewGuid();

        return customer;
    }

    private void SetToControl(Customer customer)
    {
        if (customer == null)
        {
            this.txtAddress.Text = "";
            this.txtCustomerName.Text = "";
            this.txtDescription.Text = "";
            this.txtPhoneNumber.Text = "";
            this.txtId.Value = "";
        }
        else
        {
            this.txtAddress.Text = customer.Address;
            this.txtCustomerName.Text = customer.Name;
            this.txtDescription.Text = customer.Description;
            this.txtPhoneNumber.Text = customer.PhoneNumber;
            this.txtId.Value = customer.Id.ToString();
        }
    }

    private void Add()
    {
        if (!CanEdit())
            return;
        if (!string.IsNullOrEmpty(this.txtId.Value))
            return;

        if (string.IsNullOrEmpty(this.txtCustomerName.Text))
            return;

        BasicInformationProxy.CreateCustomer(
            this.txtCustomerName.Text.Trim(),
            this.txtDescription.Text.Trim(),
            this.txtAddress.Text.Trim(),
            this.txtPhoneNumber.Text.Trim());
    }

    private void Update()
    {
        if (!CanEdit())
            return;
        Customer customer = GetFromControl();
        BasicInformationProxy.UpdateCustomer(customer);
    }


    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Search();
    }

    private void Search()
    {

        Customer[] customers = BasicInformationProxy.FindCustomersByName(this.txtSCustomerName.Text.Trim());

        if (customers.Length == 0)
            customers = new Customer[] { new Customer() };

        this.gridCustomer.DataSource = customers;
        this.gridCustomer.DataBind();
    }

    protected void gridCustomer_RowEditing(object sender, GridViewEditEventArgs e)
    {
        HiddenField txtId = this.gridCustomer.Rows[e.NewEditIndex].Cells[0].FindControl("Id") as HiddenField;

        if (string.IsNullOrEmpty(txtId.Value))
            return;

        Customer customer = BasicInformationProxy.GetCustomerById(new Guid(txtId.Value));

        SetToControl(customer);

        this.pnlEdit.Visible = true;
    }

    protected void gridCustomer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row != null && e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem is Customer)
        {
            Customer customer = e.Row.DataItem as Customer;

            if (customer.Id == Guid.Empty)
            {
                e.Row.Visible = false;
            }
            else
            {
                string Id = customer.Id.ToString();

                HiddenField txtId = e.Row.Cells[0].FindControl("Id") as HiddenField;
                txtId.Value = Id;
            }
        }
    }

    protected void gridCustomer_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        HiddenField txtId = this.gridCustomer.Rows[e.RowIndex].Cells[0].FindControl("Id") as HiddenField;

        if (!string.IsNullOrEmpty(txtId.Value))
        {
            BasicInformationProxy.DeleteCustomer(new Guid(txtId.Value));
        }

        Search();
    }

    protected void btnDelete_Click(object sender, ImageClickEventArgs e)
    {
        if (!CanEdit())
            return;
        try
        {
            bool needSearch = false;
            foreach (GridViewRow row in this.gridCustomer.Rows)
            {
                CheckBox check = row.Cells[0].FindControl("check") as CheckBox;

                if (check == null)
                    continue;

                if (check.Checked)
                {
                    HiddenField txtId = row.Cells[0].FindControl("Id") as HiddenField;

                    if (!string.IsNullOrEmpty(txtId.Value))
                    {
                        BasicInformationProxy.DeleteCustomer(new Guid(txtId.Value));
                    }

                    needSearch = true;
                }
            }

            if (needSearch)
            {
                Search();
            }
        }
        catch (Exception ex)
        {
            this.lblError.Text = ex.Message;
        }
    }
    protected void btnAdd_Click(object sender, ImageClickEventArgs e)
    {
        if (!CanEdit())
            return;
        this.pnlEdit.Visible = true;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!CanEdit())
            return;
        try
        {
            if (string.IsNullOrEmpty(this.txtId.Value))
            {
                Add();
            }
            else
            {
                Update();
            }

            SetToControl(null);

            Search();

            this.pnlEdit.Visible = false;
        }
        catch (Exception ex)
        {
            this.lblError.Text = ex.Message;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.pnlEdit.Visible = false;
        SetToControl(null);
    }

    private bool CanEdit()
    {
        return User.IsInRole(CrabApp.TenantRoles.Users.ToString()) ||
            User.IsInRole(CrabApp.TenantRoles.Managers.ToString()) ||
            User.IsInRole(CrabApp.TenantRoles.Administrators.ToString());
    }
}