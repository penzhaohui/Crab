using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.Activities;

namespace Crab.Services.EventArgs
{
    [Serializable]
    public class ApproveEventArgs : EventArgsBase
    {
        private string _comment;

        public ApproveEventArgs(Guid workflowId, Guid objectId, string upn, string comment)
            : base(workflowId, objectId, upn)
        {
            _comment = comment;
        }

        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }
    }
}
