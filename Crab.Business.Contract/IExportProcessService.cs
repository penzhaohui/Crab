using System;
using System.ServiceModel;

namespace Crab.Business.Contract
{
    [ServiceContract()]
    public interface IExportProcessService
    {
        [OperationContract]
        Guid OpenExportProcess(ShippingExportDC dc);

        [OperationContract]
        ExportProcessDC[] GetExportProcessListByStatus(ProcessStatus status);

        [OperationContract]
        ExportProcessDC[] GetExportProcessListByCreator(string creator);

        [OperationContract]
        ExportProcessDC GetExportProcessbyContractId(Guid contractId);

        [OperationContract]
        WorkflowAction[] GetSubscribedActions(Guid workflowId);

        [OperationContract]
        void Submit(Guid workflowId, Guid objectId, string upn);

        [OperationContract]
        void Approve(Guid workflowId, Guid objectId, string upn, string comment);

        [OperationContract]
        void Reject(Guid workflowId, Guid objectId, string upn, string rejectReason);

        [OperationContract]
        void Cancel(Guid workflowId, Guid objectId, string upn);
    }
}
