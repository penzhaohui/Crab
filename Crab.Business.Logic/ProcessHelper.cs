using System;
using System.Collections.Generic;
using System.Text;
using System.Workflow.Runtime;
using Crab.Business.Contract;

namespace Crab.Business.Logic
{
    public class ProcessHelper
    {
        static public WorkflowStatus ToWorkflowStatus(ProcessStatus status)
        {
            switch (status)
            {
                case ProcessStatus.Running:
                    return WorkflowStatus.Running;
                case ProcessStatus.Completed:
                    return WorkflowStatus.Completed;
                case ProcessStatus.Teminated:
                    return WorkflowStatus.Terminated;
                default:
                    return WorkflowStatus.Terminated;
            }
        }

        static public ProcessStatus ToProcessStatus(WorkflowStatus status)
        {
            switch (status)
            {
                case WorkflowStatus.Created:
                case WorkflowStatus.Running:
                case WorkflowStatus.Suspended:
                    return ProcessStatus.Running;
                case WorkflowStatus.Completed:
                    return ProcessStatus.Completed;
                case WorkflowStatus.Terminated:
                    return ProcessStatus.Teminated;
                default:
                    return ProcessStatus.Teminated;
            }
        }
    }
}
