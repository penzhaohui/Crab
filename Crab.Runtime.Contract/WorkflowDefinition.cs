using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace Crab.Runtime.Contract
{
    [DataContract, Serializable]
    public class WorkflowDefinition
    {
        #region private members
        private Guid _id;
        private int _workflowType;
        private string _xoml;
        private string _rules;
        #endregion

        [DataMember]
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [DataMember]
        public int WorkflowType
        {
            get { return _workflowType; }
            set { _workflowType = value; }
        }

        [DataMember]
        public string Xoml
        {
            get { return _xoml; }
            set { _xoml = value; }
        }

        [DataMember]
        public string Rules
        {
            get { return _rules; }
            set { _rules = value; }
        }
    }
}
