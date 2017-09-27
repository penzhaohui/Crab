using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.Activities;
using Crab.Services.EventArgs;
using Crab.Runtime.Contract;

namespace Crab.Workflow
{
    [ExternalDataExchange]
    public interface IReviewService
    {
        event EventHandler<SubmitEventArgs> Submitted;
        event EventHandler<ApproveEventArgs> Approved;
        event EventHandler<RejectEventArgs> Rejected;
        event EventHandler<CancelEventArgs> Cancelled;

        void Submit(Guid workflowId, Guid objectId, string upn, ExtensibleDC submittedObject);
        void Approve(Guid workflowId, Guid objectId, string upn, string comment);
        void Reject(Guid workflowId, Guid objectId, string upn, string rejectReason);
        void Cancel(Guid workflowId, Guid objectId, string upn);
    }
}
