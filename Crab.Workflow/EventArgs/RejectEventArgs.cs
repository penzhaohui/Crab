using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.Activities;

namespace Crab.Services.EventArgs
{
    [Serializable]
    public class RejectEventArgs : EventArgsBase
    {
        private string _rejectReason;

        public RejectEventArgs(Guid workflowId, Guid objectId, string upn, string rejectReason)
            : base(workflowId, objectId, upn)
        {
            _rejectReason = rejectReason;
        }

        public string RejectReason
        {
            get { return _rejectReason; }
            set { _rejectReason = value; }
        }
    }
}
