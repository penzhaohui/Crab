using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.Activities;
using Crab.Runtime.Contract;

namespace Crab.Services.EventArgs
{
    [Serializable]
    public class SubmitEventArgs : EventArgsBase
    {
        private ExtensibleDC _submittedObject;

        public SubmitEventArgs(Guid workflowId, Guid objectId, string upn, ExtensibleDC submittedObject)
            : base(workflowId, objectId, upn)
        {
            this._submittedObject = submittedObject;
        }

        public ExtensibleDC SubmittedObject
        {
            get { return _submittedObject; }
        }
    }
}
