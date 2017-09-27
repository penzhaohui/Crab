using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Crab.Business.Contract
{
    [DataContract]
    public class WorkflowAction
    {
        private string _actionName;
        private string _stepName;
        private string[] _roles;

        [DataMember]
        public string ActionName
        {
            get { return _actionName; }
            set { _actionName = value; }
        }

        [DataMember]
        public string StepName
        {
            get { return _stepName; }
            set { _stepName = value; }
        }

        [DataMember]
        public string[] QualifiedRoles
        {
            get { return _roles; }
            set { _roles = value; }
        }
    }

    [Serializable]
    static public class ActionConstants
    {
        public const string Submit = "Submitted";
        public const string Approve = "Approved";
        public const string Reject = "Rejected";
        public const string Cancel = "Cancelled";
    }
}
