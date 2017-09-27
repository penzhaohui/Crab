using System;
using System.Collections.Generic;
using System.Text;
using Crab.Services.EventArgs;
using Crab.Runtime.Contract;

namespace Crab.Workflow
{
    public class ContractService : IProcessService, IReviewService
    {
        #region Implement IProcessService
        public event EventHandler<StatusEventArgs> StatusProceeded;

        /// <summary>
        /// Go to next status
        /// </summary>
        /// <param name="id">Workflow ID</param>
        /// <param name="upn">The user principle name</param>
        /// <param name="objectId">The ID of the business object</param>
        public void ProceedStatus(Guid workflowId, Guid objectId, string upn)
        {
            StatusEventArgs args = new StatusEventArgs(workflowId, objectId, upn);
            args.Identity = upn;
            EventHandler<StatusEventArgs> eventHandler = StatusProceeded;
            if (eventHandler != null)
                eventHandler(null, args);
        }

        #endregion

        #region Implement IReviewService
        public event EventHandler<SubmitEventArgs> Submitted;
        public event EventHandler<ApproveEventArgs> Approved;
        public event EventHandler<RejectEventArgs> Rejected;
        public event EventHandler<CancelEventArgs> Cancelled;

        public void Submit(Guid workflowId, Guid objectId, string upn, ExtensibleDC submittedObject)
        {
            SubmitEventArgs args = new SubmitEventArgs(workflowId, objectId, upn, submittedObject);
            args.Identity = upn;
            EventHandler<SubmitEventArgs> eventHandler = Submitted;
            if (eventHandler != null)
                eventHandler(null, args);
        }

        public void Approve(Guid workflowId, Guid objectId, string upn, string comment)
        {
            ApproveEventArgs args = new ApproveEventArgs(workflowId, objectId, upn, comment);
            args.Identity = upn;
            EventHandler<ApproveEventArgs> eventHandler = Approved;
            if (eventHandler != null)
                eventHandler(null, args);
        }

        public void Reject(Guid workflowId, Guid objectId, string upn, string rejectReason)
        {
            RejectEventArgs args = new RejectEventArgs(workflowId, objectId, upn, rejectReason);
            args.Identity = upn;
            EventHandler<RejectEventArgs> eventHandler = Rejected;
            if (eventHandler != null)
                eventHandler(null, args);
        }

        public void Cancel(Guid workflowId, Guid objectId, string upn)
        {
            CancelEventArgs args = new CancelEventArgs(workflowId, objectId, upn);
            args.Identity = upn;
            EventHandler<CancelEventArgs> eventHandler = Cancelled;
            if (eventHandler != null)
                eventHandler(null, args);
        }
        #endregion
    }
}
