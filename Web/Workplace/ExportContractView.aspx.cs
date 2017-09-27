using System;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Crab.Services.Proxy;
using Crab.Business.Contract;
using Crab.Runtime.Contract;
using Crab.DataModel.Utility;
using Crab.DataModel.Common;
using CrabApp;

public partial class Workplace_ExportContractView : PageBase
{
    private IDictionary<string, Label> _namesLables = new Dictionary<string, Label>();
    private IDictionary<string, WebControl> _namesFieldControls = new Dictionary<string, WebControl>();
    private ShippingExportDC _currentContract;
    private ExportProcessDC _process;
    private EntityDef _entityDef;
    private EntityFieldDef[] _entityFieldDefs;

    protected ShippingExportDC CurrentContract
    {
        get 
        {
            if (_currentContract == null)
                _currentContract = ExportProxy.GetExportContractById(GetObjectId());
            return _currentContract; 
        }
    }

    protected ExportProcessDC Process
    {
        get
        {
            if(_process == null)
                _process = ExportProcessProxy.GetExportProcessbyContractId(CurrentContract.Id);
            return _process;
        }
    }

    protected EntityDef EntityDef
    {
        get 
        { 
            if(_entityDef == null)
                _entityDef = MetadataProxy.GetEnityDefByName(ExtensibleDC.GetEntityClassName(typeof(ShippingExportDC)));
            return _entityDef;
        }
    }

    protected EntityFieldDef[] FieldDefs
    {
        get
        {
            if(_entityFieldDefs == null)
                _entityFieldDefs = MetadataProxy.GetFieldDefs(EntityDef.Id);
            return _entityFieldDefs;
        }
    }

    protected EntityFieldDef[] SharedFields
    {
        get
        {
            List<EntityFieldDef> fields = new List<EntityFieldDef>();
            foreach (EntityFieldDef fieldDef in FieldDefs)
            {
                if (fieldDef.IsShared)
                    fields.Add(fieldDef);
            }
            return fields.ToArray();
        }
    }

    protected EntityFieldDef[] ExtensionFields
    {
        get
        {
            List<EntityFieldDef> fields = new List<EntityFieldDef>();
            foreach (EntityFieldDef fieldDef in FieldDefs)
            {
                if (!fieldDef.IsShared)
                    fields.Add(fieldDef);
            }
            return fields.ToArray();
        }
    }

    protected FieldBehavior[] SharedFieldsBehaviors
    {
        get 
        {
            return new FieldBehavior[]
            {
                new FieldBehavior("Number", ControlTypes.Label),
                new FieldBehavior("Creator",ControlTypes.Label),
                new FieldBehavior("ExportSite", ControlTypes.Text),
                new FieldBehavior("Destination", ControlTypes.Text),
                new FieldBehavior("Shipper", ControlTypes.List), 
                new FieldBehavior("Consignee", ControlTypes.List),
                new FieldBehavior("NotifyPart",ControlTypes.List),
                new FieldBehavior("Amount", ControlTypes.Text),
                new FieldBehavior("Batch",ControlTypes.List),
                new FieldBehavior("Reship", ControlTypes.List),
                new FieldBehavior("CreditId", ControlTypes.Text),
                new FieldBehavior("PaymentMethod", ControlTypes.List, 0, false, "FREIGHT PREPAID,Prepaid|FREIGHT COLLECT,Collect"),
                new FieldBehavior("Description", ControlTypes.MultiText),
                new FieldBehavior("ShippingMarks",ControlTypes.Text),
                new FieldBehavior("PruductName", ControlTypes.Text),
                new FieldBehavior("Quantity", ControlTypes.Text),
                new FieldBehavior("Net", ControlTypes.Text)
            };
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        DataModelControlHelper.RenderControls(leftRegion, SharedFields, SharedFieldsBehaviors, string.Empty);
        DataModelControlHelper.RenderControls(rightRegion, ExtensionFields, null, string.Empty);
        string[] customerList = new string[]{"Shipper", "Consignee", "NotifyPart"};
        Customer[] customers = BasicInformationProxy.FindCustomersByName(string.Empty);
        foreach(string customerField in customerList)
        {
            DropDownList list = DataModelControlHelper.GetControl(leftRegion, customerField) as DropDownList;
            list.DataTextField = "Name";
            list.DataValueField = "Id";
            list.DataSource = customers;
            list.DataBind();
            if (list.Items.Count > 0)
                list.Items.Insert(0, new ListItem(string.Empty, string.Empty));
        }
        

        if (!Page.IsPostBack)
        {
            InitialControls();
        }
    }

    private Guid GetObjectId()
    {
        string stringId = Request["objectId"];
        if (string.IsNullOrEmpty(stringId))
            throw new ArgumentNullException("objectId");
        try
        {
            return new Guid(stringId);
        }
        catch
        {
            throw new ArgumentException("objectId");
        }
    }


    private void InitialControls()
    {
        DataModelControlHelper.FillValues(leftRegion, SharedFields, CurrentContract);
        DataModelControlHelper.FillValues(rightRegion, ExtensionFields, CurrentContract);
        Label label = DataModelControlHelper.GetControl(leftRegion, "Creator") as Label;
        label.Text = ConvertToUsername(CurrentContract.Creator);

        if (!CanEdit())
        {
            DisableInputControls(tblInputControls);
            btnSave.Enabled = false;
            btnRefresh.Enabled = false;
            btnDelete.Enabled = false;
        }
        else
            SetControlMode();

        // for workflow
        ShowStatus();
        ShowActionButtons();
    }

    private void SetControlMode()
    {
        if(Process == null)
            return;
        Crab.Business.Contract.ProcessStatus status = Process.Status;

        if (status == Crab.Business.Contract.ProcessStatus.Completed ||
            status == Crab.Business.Contract.ProcessStatus.Teminated)
        {
            DisableInputControls(tblInputControls);
            btnSave.Enabled = false;
            btnRefresh.Enabled = false;
        }
        else
        {
            /*DataModelControlHelper.GetControl(leftRegion, "ExportSite").Enabled = false;
            DataModelControlHelper.GetControl(leftRegion, "Destination").Enabled = false;
            DataModelControlHelper.GetControl(leftRegion, "Shipper").Enabled = false;
            DataModelControlHelper.GetControl(leftRegion, "Consignee").Enabled = false;
            DataModelControlHelper.GetControl(leftRegion, "NotifyPart").Enabled = false;
            DataModelControlHelper.GetControl(leftRegion, "Amount").Enabled = false;*/
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveObject();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        RefreshObject();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        ExportProxy.DeleteExportContract(GetObjectId());
        Response.Redirect("~/Workplace/SearchExportContract.aspx");
    }

    #region for role control
    public bool CanEdit()
    {
        return CurrentContract.Creator == base.UserId
            || Roles.IsUserInRole(CrabApp.TenantRoles.Managers.ToString());
    }

    public bool CanApprove()
    {
        return Roles.IsUserInRole(CrabApp.TenantRoles.Managers.ToString());
    }
    #endregion 

    private void SaveObject()
    {
        if (!CanEdit())
            return;
        DataModelControlHelper.RetrieveValues(leftRegion, SharedFields, CurrentContract);
        DataModelControlHelper.RetrieveValues(rightRegion, ExtensionFields, CurrentContract);
        ExportProxy.UpdateExportContract(CurrentContract);
    }

    private void RefreshObject()
    {
        Server.Transfer(Request.RawUrl);
    }

    protected void btnOpenProcess_Click(object sender, EventArgs e)
    {
        SaveObject();
        ExportProcessProxy.OpenExportProcess(CurrentContract);
        Response.Redirect(Request.RawUrl);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SaveObject();
        ExportProcessProxy.Submit(Process.ProcessId, Process.ObjectId, HttpContext.Current.User.Identity.Name);
        Response.Redirect(Request.RawUrl);
    }

    protected void btnApprove_Click(object sender, EventArgs e)
    {
        ExportProcessProxy.Approve(Process.ProcessId, Process.ObjectId, HttpContext.Current.User.Identity.Name, this.txtApproveRemark.Text);
        Response.Redirect(Request.RawUrl);
    }

    protected void btnReject_Click(object sender, EventArgs e)
    {
        ExportProcessProxy.Reject(Process.ProcessId, Process.ObjectId, HttpContext.Current.User.Identity.Name, this.txtApproveRemark.Text);
        Response.Redirect(Request.RawUrl);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ExportProcessProxy.Cancel(Process.ProcessId, CurrentContract.Id, HttpContext.Current.User.Identity.Name);
        Response.Redirect(Request.RawUrl);
    }

    private void HideButtons()
    {
        this.btnSubmit.Visible = false;
        this.btnCancel.Visible = false;
        this.btnApprove.Visible = false;
        this.btnReject.Visible = false;
        this.pnlRemark.Visible = false;
    }

    private void ShowActionButtons()
    {
        // First we hide all buttons
        HideButtons();
        if(CurrentContract == null)
            return;
        if (Process==null||Process.Status == Crab.Business.Contract.ProcessStatus.Completed ||
            Process.Status == Crab.Business.Contract.ProcessStatus.Teminated)
            return;
        WorkflowAction[] subscribedActions = ExportProcessProxy.GetSubscribedActions(Process.ProcessId);
        if (subscribedActions.Length == 0)
            lblCurrentStep.Visible = lblCurrentStepCaption.Visible = false;
        else
            lblCurrentStep.Text = subscribedActions[0].StepName;
        foreach (WorkflowAction action in subscribedActions)
        {
            switch (action.ActionName)
            {
                case ActionConstants.Submit:
                    {
                        if (CanEdit())
                            btnSubmit.Visible = true;
                    } break;
                case ActionConstants.Approve:
                    {
                        if (CanApprove())
                        {
                            pnlRemark.Visible = true;
                            btnApprove.Visible = true;
                        }
                    } break;
                case ActionConstants.Reject:
                    {
                        if (CanApprove())
                        {
                            pnlRemark.Visible = true;
                            btnReject.Visible = true;
                        }
                    } break;
                case ActionConstants.Cancel:
                    {
                        if (CanApprove())
                        {
                            btnCancel.Visible = true;
                        }
                    } break;
            }
        }
    }

    private void ShowStatus()
    {
        if (Process != null)
        {
            lblStatus.Text = Process.Status.ToString();
            lnkViewProcess.NavigateUrl = string.Format(lnkViewProcess.NavigateUrl, Process.ProcessId.ToString());
        }

        btnOpenProcess.Visible = Process == null;
        lnkViewProcess.Visible = Process != null;
        lblCurrentStepCaption.Visible = lblCurrentStep.Visible = Process != null;
    }

    protected string ConvertToUsername(Guid? userId)
    {
        if (userId == null)
            return string.Empty;
        string principleName = AuthenticationProxy.GetUpnByUserId((Guid)userId);
        if (string.IsNullOrEmpty(principleName))
            return string.Empty;
        ProfileCommon profile = Profile.GetProfile(principleName);
        if (profile != null && !string.IsNullOrEmpty(profile.FullName))
            return profile.FullName;
        else
            return new Upn(principleName).Username;
    }

    private void DisableInputControls(Control parentControl)
    {
        foreach (Control ctl in parentControl.Controls)
        {
            if (ctl is TextBox || ctl is DropDownList)
            {
                ((WebControl)ctl).Enabled = false;
                continue;
            }
            if (ctl.HasControls())
            {
                DisableInputControls(ctl);
            }
        }
    }
}
