using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.Activities;

namespace Crab.Services.EventArgs
{
    [Serializable]
    public class StatusEventArgs : EventArgsBase
    {
        public StatusEventArgs(Guid workflowId, Guid objectId, string upn)
            : base(workflowId, objectId, upn)
        {
        }
    }
}
