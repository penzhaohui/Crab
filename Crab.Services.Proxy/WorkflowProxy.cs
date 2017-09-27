using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;
using Crab.Runtime.Contract;

namespace Crab.Services.Proxy
{
    static public class WorkflowProxy
    {
        /// <summary>
        /// Channel to proxy IProvision
        /// </summary>
        private class BasicChannel : ClientBase<IWorkflow>, IWorkflow
        {
            public WorkflowDefinition GetWorkflowDefinition(int workflowType)
            {
                return base.Channel.GetWorkflowDefinition(workflowType);
            }

            public void UpdateWorkflowDefinition(int workflowType, string xoml, string rules)
            {
                base.Channel.UpdateWorkflowDefinition(workflowType, xoml, rules);
            }


            public byte[] GetWorkflowGraphic(Guid processId)
            {
                return base.Channel.GetWorkflowGraphic(processId);
            }

            public byte[] GetWorkflowDefinitionGraphic(int workflowType)
            {
                return base.Channel.GetWorkflowDefinitionGraphic(workflowType);
            }
        }


        static public WorkflowDefinition GetWorkflowDefinition(int workflowType)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetWorkflowDefinition(workflowType);
            }
        }

        static public void UpdateWorkflowDefinition(int workflowType, string xoml, string rules)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                channel.UpdateWorkflowDefinition(workflowType, xoml, rules);
            }
        }

        static public byte[] GetWorkflowGraphic(Guid processId)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetWorkflowGraphic(processId);
            }
        }

        static public byte[] GetWorkflowDefinitionGraphic(int workflowType)
        {
            using (BasicChannel channel = new BasicChannel())
            {
                return channel.GetWorkflowDefinitionGraphic(workflowType);
            }
        }

    }
}
