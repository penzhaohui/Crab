using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.Activities;

namespace Crab.Services.EventArgs
{
    [Serializable]
    public class EventArgsBase : ExternalDataEventArgs
    {
        private Guid _objectId;

        protected EventArgsBase(Guid workflowId, Guid objectId, string upn)
            : base(workflowId)
        {
            _objectId = objectId;
            Identity = upn;
        }

        public Guid ObjectId
        {
            get { return _objectId; }
            set { _objectId = value; }
        }
    }
}
