using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using System.Diagnostics;

namespace Crab.Workflow
{
    public class ExportProcess : SequentialWorkflowActivity
	{
        private int _internalWorkflowType;
        private Guid _tenantId;
        private Guid _objectId;
        private string _number;
        private string _creator;

        public ExportProcess()
        {
        }

        [Browsable(false)]
        public int InternalWorkflowType
        {
            get { return _internalWorkflowType;}
            set { _internalWorkflowType = value; }
        }

        [Browsable(false)]
        public Guid TenantId
        {
            get { return _tenantId; }
            set { _tenantId = value; }
        }

        [Browsable(false)]
        public Guid ObjectId
        {
            get { return _objectId; }
            set { _objectId = value; }
        }

        [Browsable(false)]
        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }

        [Browsable(false)]
        public string Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }
	}
}
