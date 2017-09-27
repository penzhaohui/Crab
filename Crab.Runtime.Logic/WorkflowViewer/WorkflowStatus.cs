using System;
using System.Collections.Generic;
using System.Text;

namespace Crab.Runtime.Logic.WorkflowViewer
{
    internal class WorkflowStatusInfo
    {
        private string idValue;
        private string nameValue;
        private string statusValue;
        private string createdDateTimeValue;
        private Guid instanceIdValue;

        internal WorkflowStatusInfo(string id, string name, string status, string createdDateTime, Guid instanceId)
        {
            this.idValue = id;
            this.nameValue = name;
            this.statusValue = status;
            this.createdDateTimeValue = createdDateTime;
            this.instanceIdValue = instanceId;
        }

        internal string Status
        {
            set { statusValue = value; }
        }


        internal Guid InstanceId
        {
            get { return instanceIdValue; }
        }
    }

    //Class to store activity information - create one per activity for the selected workflow
    internal class ActivityStatusInfo
    {
        private string nameValue;
        private string statusValue;

        internal ActivityStatusInfo(string name, string status)
        {
            this.nameValue = name;
            this.statusValue = status;
        }

        internal string Name
        {
            get { return nameValue; }
        }

        internal string Status
        {
            get { return statusValue; }
        }
    }
}
