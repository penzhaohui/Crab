using System;
using System.Collections.Generic;
using System.Text;

namespace Crab.Services.EventArgs
{
    [Serializable]
    public class CancelEventArgs : EventArgsBase
    {
        public CancelEventArgs(Guid workflowId, Guid objectId, string upn)
            : base(workflowId, objectId, upn)
        {
        }
    }
}
