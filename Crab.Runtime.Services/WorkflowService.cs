using System;
using System.Collections.Generic;
using System.Text;
using Crab.Runtime.Contract;
using Crab.Runtime.Logic;

namespace Crab.Runtime.Services
{
    public class WorkflowService: IWorkflow
    {
        public WorkflowDefinition GetWorkflowDefinition(int workflowType)
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");
            return WorkflowLogic.GetWorkflowDefinition(RequestContext.Current.TenantId, workflowType);
        }


        public void UpdateWorkflowDefinition(int workflowType, string xoml, string rules)
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");
            if (string.IsNullOrEmpty(xoml))
                throw new ArgumentNullException("xoml");
            WorkflowLogic.UpdateWorkflowDefinition(RequestContext.Current.TenantId, workflowType, xoml, rules);
        }

        public byte[] GetWorkflowGraphic(Guid processId)
        {
            return WorkflowLogic.GetWorkflowGraphic(processId);
        }

        public byte[] GetWorkflowDefinitionGraphic(int workflowType)
        {
            if (RequestContext.Current.TenantId == Guid.Empty)
                throw new ArgumentNullException("tenantId");
            return WorkflowLogic.GetWorkflowDefinitionGraphic(RequestContext.Current.TenantId, workflowType);
        }

    }
}
