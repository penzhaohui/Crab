using System;
using System.Collections.Generic;
using System.Text;
using Crab.Business.Contract;
using Crab.Business.Logic;
using Crab.Runtime.Services;

namespace Crab.Business.Services
{
    public class ExportProcessService: IExportProcessService
    {
        public Guid OpenExportProcess(ShippingExportDC dc)
        {
            return ExportProcessLogic.OpenExportProcess(RequestContext.Current.TenantId, dc);
        }


        public ExportProcessDC[] GetExportProcessListByStatus(ProcessStatus status)
        {
            return ExportProcessLogic.GetExportProcessListByStatus(RequestContext.Current.TenantId, status);
        }

        public ExportProcessDC[] GetExportProcessListByCreator(string creator)
        {
            return ExportProcessLogic.GetExportProcessListByCreator(RequestContext.Current.TenantId, creator);
        }

        public ExportProcessDC GetExportProcessbyContractId(Guid contractId)
        {
            return ExportProcessLogic.GetExportProcessbyContractId(contractId);
        }

        public WorkflowAction[] GetSubscribedActions(Guid workflowId)
        {
            return ExportProcessLogic.GetSubscribedActions(workflowId);
        }

        public void Submit(Guid workflowId, Guid objectId, string upn)
        {
            ExportProcessLogic.Submit(workflowId, objectId, upn);
        }

        public void Approve(Guid workflowId, Guid objectId, string upn, string comment)
        {
            ExportProcessLogic.Approve(workflowId, objectId, upn, comment);
        }

        public void Reject(Guid workflowId, Guid objectId, string upn, string rejectReason)
        {
            ExportProcessLogic.Reject(workflowId, objectId, upn, rejectReason);
        }

        public void Cancel(Guid workflowId, Guid objectId, string upn)
        {
            ExportProcessLogic.Cancel(workflowId, objectId, upn);
        }
    }
}
